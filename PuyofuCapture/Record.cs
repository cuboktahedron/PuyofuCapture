/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/license/LICENSE-MIT.txt
 */
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
        /// <summary>レコード日付</summary>
        [DataMember(Name = "date")]
        public string Date { get; set; }

        /// <summary>レコードID</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>ぷよ譜コード</summary>
        [DataMember(Name = "record")]
        public string StepRecord { get; set; }

        /// <summary>タグ</summary>
        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }
    }
}
