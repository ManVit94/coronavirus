using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirus.Domain
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
