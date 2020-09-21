using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoronaVirus.Domain
{
    public class Country : EntityBase
    {
        public string Name { get; set; }

        public ICollection<VirusStatistic> VirusStatistics { get; set; }
    }
}
