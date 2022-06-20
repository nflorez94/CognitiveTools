using Microsoft.AspNetCore.Mvc;
using Speech2Text.Core;
using Speech2Text.Dto;

namespace Speech2Text.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpeechTextController : ControllerBase
    {
        private readonly ILogger<SpeechTextController> _logger;
        private readonly IMainServices _mainService;

        public SpeechTextController(ILogger<SpeechTextController> logger, IMainServices mainService)
        {
            _logger = logger;
            _mainService = mainService;
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IEnumerable<string>> ConvertAudio2TextAsyncGet(IFormFile audio, [FromQuery]string audioLanguaje)
        {
            AudioFileUploadDto audioFile = new AudioFileUploadDto { AudioFile = audio, Id = Guid.NewGuid(), UploadTime = DateTime.Now, Language=audioLanguaje };
            return await _mainService.ConvertAudio2TextAsync(audioFile);
        }
    }
}