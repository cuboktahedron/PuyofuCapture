using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    [DataContract]
    public class Record
    {
        [DataMember(Name = "date")]
        public string Date { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "record")]
        public string StepRecord { get; set; }

        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }
    }
}
