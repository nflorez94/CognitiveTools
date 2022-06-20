using Microsoft.AspNetCore.Http;

namespace Speech2Text.Dto
{
    public class AudioFileUploadDto
    {
        public Guid Id { get; set; }
        public IFormFile? AudioFile { get; set; }
        public DateTime UploadTime { get; set; }
        public string Language { get; set; } = "es-CO";

    }
}
