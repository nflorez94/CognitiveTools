using Frontend.Entities;

namespace Frontend.VTRepository
{
    public interface IV2TRepository
    {
        Task<SpeechToTextRequest> SaveRequest(SpeechToTextRequest request);
    }
}