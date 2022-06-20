using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Frontend.ViewModels
{
    public class AudioUploadViewModel
    {
        [Required(ErrorMessage = "The {0} field is required")]
        [Display(Name = "Audio File")]
        [DataType(DataType.Upload)]
        public IFormFile? AudioFile { get; set; }
        [Required(ErrorMessage = "Select the languaje of your audio")]
        [Display(Name = "Audio Languaje")]
        public string? AudioLanguaje { get; set; }
        [ValidateNever]
        [Display(Name = "Transcription")]
        public IEnumerable<string>? AudioText { get; set; }
    }
}