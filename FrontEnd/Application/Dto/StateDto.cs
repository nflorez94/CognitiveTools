using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Dto
{
    public class StateDto
    {
        public int StateId { get; set; }
        public string? StateDescription { get; set; }
    }
    public enum StateType
    {
        Unknown,
        Requested,
        Success
    }
}
