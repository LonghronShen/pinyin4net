using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinyin4net
{

    internal class PinyinItems
    {

        [JsonProperty("items")]
        public PinyinItem[] Items { get; set; }

    }

}