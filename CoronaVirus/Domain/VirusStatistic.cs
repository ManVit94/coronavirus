using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoronaVirus.Domain
{
    public class VirusStatistic : EntityBase
    {
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public DateTime ReportDate { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
