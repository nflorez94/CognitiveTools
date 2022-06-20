using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using Speech2Text.Dto;
using Steech2Text.Entities;

namespace Speech2Text.Core
{
    public class MainServices : IMainServices
    {
        private readonly IOptions<AzCognitive> _azureCognitive;

        public MainServices(IOptions<AzCognitive> azureCognitive)
        {
            _azureCognitive = azureCognitive;
        }

        /// <summary>
        /// Convierte un audio a texto
        /// </summary>
        /// <param name="audio"></param>
        /// <returns>lista de strings con las frases reconocidas</returns>
        public async Task<IEnumerable<string>> ConvertAudio2TextAsync(AudioFileUploadDto audio)
        {
            //Gets the extension of the file
            string extension = audio.AudioFile.FileName.Split('.')[^1].ToLower();
            var tempName = Guid.NewGuid();
            var filePathBase = Path.GetTempPath();
            var filePath = filePathBase + $@"{tempName}.";
            bool converted = false;
            if (!extension.ToLower().Equals("wav"))
            {
                using (var stream = File.Create(filePath+extension))
                {
                    await audio.AudioFile.CopyToAsync(stream);
                }
                converted = ConvertAudioToWav(extension, filePath+extension);
                File.Delete(filePath+extension);
            }
            SpeechConfig? config = SpeechConfig.FromEndpoint(new Uri(_azureCognitive.Value.Url),
                                                             _azureCognitive.Value.Token);

            config.SpeechRecognitionLanguage = audio.Language;
            var stopRecognition = new TaskCompletionSource<int>();

            if (!converted)
            {
                using var stream = File.Create(filePath + extension);
                await audio.AudioFile.CopyToAsync(stream);
            }

            var audioConfig = AudioConfig.FromWavFileInput(filePath+"wav");
            using var recognizer = new SpeechRecognizer(config, audioConfig);


            List<string> texto = new();
            //texto.Add("Prueba Exitosa");
            //audioConfig.Dispose();
            //recognizer.Dispose();
            //File.Delete(filePath + "wav");
            //return texto;
            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
            };
            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
                    texto.Add(e.Result.Text);
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
            };

            recognizer.Canceled += (s, e) =>
            {
                Console.WriteLine($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }

                stopRecognition.TrySetResult(0);
            };

            recognizer.SessionStopped += (s, e) =>
            {
                Console.WriteLine("\n    Session stopped event.");
                stopRecognition.TrySetResult(0);
            };
            await recognizer.StartContinuousRecognitionAsync();

            Task.WaitAny(new[] { stopRecognition.Task });

            recognizer.Dispose();
            File.Delete(filePath+"wav");
            return texto;
        }

        /// <summary>
        /// Convierte cualquier audio a wav y lo guarda en una ubicacion temporal
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="extension"></param>
        /// <param name="filePath"></param>
        /// <returns>true si logro convertir el audio</returns>
        public static bool ConvertAudioToWav(string extension, string filePath)
        {
            using (AudioFileReader reader = new(filePath))
            {
                WaveFileWriter.CreateWaveFile(filePath.Replace(extension, "wav"), reader);
                reader.Dispose();
            }

            return true;
        }
    }
}