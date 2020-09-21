using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirus.Dto
{
    public class VirusStatisticResponseDto
    {
        public string Country { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
    }
}
