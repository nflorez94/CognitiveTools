using AutoMapper;
using Frontend.Dto;
using Frontend.Entities;
using Frontend.VTRepository;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Frontend.ViewModels;
using System.Globalization;

namespace Frontend.Core
{
    public class V2TServices : IV2TServices
    {
        private readonly IOptions<CognitiveServiceApi> _cognitiveServices;
        private readonly IV2TRepository _repository;
        private readonly IMapper _mapper;

        public V2TServices(IOptions<CognitiveServiceApi> cognitiveServices, IV2TRepository repository, IMapper mapper)
        {
            _cognitiveServices = cognitiveServices;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> ConvertAudio2TextAsyncGet(AudioUploadViewModel model)
        {
            var listAdmitedExtensions = new List<string> { "3g2", "3gp", "3gp2", "3gpp", "asf", "wma", "wmv", "aac", "adts", "avi", "mp3", "m4a", "m4v", "mov", "mp4", "sami", "smi", "wav" };
            bool fileAdmited = false;
            foreach (var _ in from extension in listAdmitedExtensions
                              where model.AudioFile.FileName.Contains(extension)
                              select new { })
            {
                fileAdmited = true;
            }

            if (!fileAdmited)
            {
                return new List<string>() { "Audio File Not Supported, please use one of the next formats: 3g2, 3gp, 3gp2, 3gpp, asf, wma, wmv, aac, adts, avi, mp3, m4a, m4v, mov, mp4, sami, smi, wav" };
            }
            using var httpClient = new HttpClient();
            using var form = new MultipartFormDataContent();
            using var streamContent = new StreamContent(model.AudioFile.OpenReadStream());
            using var multipartContent = new MultipartFormDataContent
            {
                { new StreamContent(model.AudioFile.OpenReadStream()), "audio", model.AudioFile.FileName }
            };
            var apiRequestUrl = $"{_cognitiveServices.Value.Url}{model.AudioLanguaje}";
            var response = new HttpResponseMessage();
            Task makeResquest = Task.Run(async () => response = await httpClient.PostAsync(apiRequestUrl, multipartContent));
            makeResquest.Wait();
            return JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task SaveRequest(AudioUploadViewModel model, int state, Guid requestIdentifier)
        {
            var file = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                model.AudioFile.OpenReadStream().CopyTo(memoryStream);
                file = Convert.ToBase64String(memoryStream.ToArray());
            }
            var audiotext = string.Empty;
            if(model.AudioText != null)
            {
                foreach(var text in model.AudioText)
                {
                    audiotext += text + Environment.NewLine;
                }
            }
            var request = new SpeechToTextRequestDto
            {
                AudioLanguaje = model.AudioLanguaje,
                Extension = model.AudioFile.FileName.Split('.')[^1].ToLower(),
                FileSize = model.AudioFile.Length,
                RequestDate = DateTime.Now,
                StateId = state,
                RequestedFromCountry = RegionInfo.CurrentRegion.DisplayName,
                RequestId = requestIdentifier,
                Response=audiotext,
                ResponseSize=audiotext.Length
            };
            Thread save = new Thread(async () => await _repository.SaveRequest(_mapper.Map<SpeechToTextRequest>(request)));
            save.Start();
        }
    }
}