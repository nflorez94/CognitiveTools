using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Dto
{
    public class SpeechToTextRequestDto
    {
        public Guid RequestId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RequestedFromCountry { get; set; }
        public string? RequestFile { get; set; }
        public string? Extension { get; set; }
        public int StateId { get; set; }
        public long FileSize { get; set; }
        public string? Response { get; set; }
        public int ResponseSize { get; set; }
        public string? AudioLanguaje { get; set; }
    }
}
