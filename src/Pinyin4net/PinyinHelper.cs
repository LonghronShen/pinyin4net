/**
 * Copyright (c) 2012 Yang Kuang
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Pinyin4net.Format;

namespace Pinyin4net
{
    /// <summary>
    /// Summary description for PinyinHelper.
    /// </summary>
    public class PinyinHelper
    {
        private static Dictionary<string, string> dict;


        /// <summary>
        /// We don't need any instances of this object.
        /// </summary>
        private PinyinHelper()
        {
        }

        /// <summary>
        /// Load unicode-pinyin map to memery while this class first use.
        /// </summary>
        static PinyinHelper()
        {
            dict = new Dictionary<string, string>();
            var assembly =
#if NET20 || NET35 || PROFILE336
                typeof(PinyinHelper).Assembly;
#else
                typeof(PinyinHelper).GetTypeInfo().Assembly;
#endif

            foreach (var item in assembly.GetManifestResourceNames())
            {
                if (item.EndsWith("Pinyin4net.Resources.unicode_to_hanyu_pinyin.json"))
                {
                    using (var stream = assembly.GetManifestResourceStream(item))
                    {
                        using (TextReader tr = new StreamReader(stream))
                        {
                            using (var jtr = new JsonTextReader(tr))
                            {
                                var serializer = new JsonSerializer();
                                var items = serializer.Deserialize<PinyinItems>(jtr);
                                dict = items.Items
                                    .Where(x => !string.IsNullOrEmpty(x.Hanyu))
                                    .ToDictionary(x => x.Unicode, x => x.Hanyu);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get all Hanyu pinyin of a single Chinese character (both
        /// Simplified Chinese and Traditional Chinese).
        /// 
        /// This function is same with: 
        ///     ToHanyuPinyinStringArray(ch, new HanyuPinyinOutputFormat());
        ///
        /// For example, if the input is '偻', the return will be an array with 
        /// two Hanyu pinyin strings: "lou2", "lv3". If the input is '李', the
        /// return will be an array with one Hanyu pinyin string: "li3".
        /// </summary>
        /// <param name="ch">The given Chinese character</param>
        /// <returns>A string array contains all Hanyu pinyin presentations; return 
        /// null for non-Chinese character.</returns>
        public static string[] ToHanyuPinyinStringArray(char ch)
        {
            return ToHanyuPinyinStringArray(ch, new HanyuPinyinOutputFormat());
        }

        /// <summary>
        /// Get all Hanyu pinyin of a single Chinese character (both
        /// Simplified Chinese and Traditional Chinese).
        /// </summary>
        /// <param name="ch">The given Chinese character</param>
        /// <param name="format">The given output format</param>
        /// <returns>A string array contains all Hanyu pinyin presentations; return 
        /// null for non-Chinese character.</returns>
        public static string[] ToHanyuPinyinStringArray(
            char ch, HanyuPinyinOutputFormat format)
        {
            return GetFomattedHanyuPinyinStringArray(ch, format);
        }

        #region Private Functions
        private static string[] GetFomattedHanyuPinyinStringArray(
            char ch, HanyuPinyinOutputFormat format)
        {
            var unformattedArr = GetUnformattedHanyuPinyinStringArray(ch);
            if (null != unformattedArr)
            {
                for (var i = 0; i < unformattedArr.Length; i++)
                {
                    unformattedArr[i] = PinyinFormatter.FormatHanyuPinyin(unformattedArr[i], format);
                }
            }

            return unformattedArr;
        }

        private static string[] GetUnformattedHanyuPinyinStringArray(char ch)
        {
            var code = String.Format("{0:X}", (int)ch).ToUpper();

            if (dict.ContainsKey(code))
            {
                return dict[code].Split(',');
            }

            return null;
        }
        #endregion

    }

}
