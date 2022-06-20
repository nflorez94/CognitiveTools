using Frontend.ViewModels;

namespace Frontend.Core
{
    public interface IV2TServices
    {
        Task<IEnumerable<string>> ConvertAudio2TextAsyncGet(AudioUploadViewModel model);
        Task SaveRequest(AudioUploadViewModel model, int state, Guid requestIdentifier);
    }
}