using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinyin4net
{

    internal class PinyinItem
    {

        [JsonProperty("unicode")]
        public string Unicode { get; set; }

        [JsonProperty("hanyu")]
        public string Hanyu { get; set; }

    }

}