using Speech2Text.Dto;

namespace Speech2Text.Core
{
    public interface IMainServices
    {
        Task<IEnumerable<string>> ConvertAudio2TextAsync(AudioFileUploadDto audio);
    }
}