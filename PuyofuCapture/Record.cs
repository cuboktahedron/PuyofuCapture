using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cubokta.Puyo
{
    /// <summary>
    /// ぷよ譜１レコード分の情報
    /// </summary>
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
