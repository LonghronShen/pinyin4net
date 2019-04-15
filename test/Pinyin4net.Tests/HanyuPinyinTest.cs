using Pinyin4net.Exceptions;
using Pinyin4net.Format;
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
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace Pinyin4net.Tests
{

    public class HanyuPinyinTest
    {

        #region Tests

        [Fact(DisplayName = "Test null input")]
        public void TestNullInput()
        {
            Assert.Throws<ArgumentNullException>(() => PinyinHelper.ToHanyuPinyinStringArray('李', null));
        }

        [Theory(DisplayName = "Test non Chinese character input")]
        [InlineData('A')]
        [InlineData('ガ')]
        [InlineData('ç')]
        [InlineData('匇')]
        public void TestNonChineseCharacter(char ch)
        {
            Assert.Null(PinyinHelper.ToHanyuPinyinStringArray(ch));
        }

        [Theory(DisplayName = "Test get hanyupinyin with different VCharType format")]
        //  Simplified Chinese
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_AND_COLON, "lu:3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_AND_COLON, "li3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_V, "lv3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_V, "li3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_UNICODE, "lü3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_UNICODE, "li3")]
        //  Traditional Chinese
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_AND_COLON, "lu:3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_V, "lv3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_UNICODE, "lü3")]
        public void TestVCharType(char ch, HanyuPinyinVCharType vcharType, string result)
        {
            var format = new HanyuPinyinOutputFormat
            {
                VCharType = vcharType
            };
            Assert.Equal(result, PinyinHelper.ToHanyuPinyinStringArray(ch, format)[0]);
        }

        [Theory(DisplayName = "Test get hanyupinyin with upper case format")]
        //  Simplified Chinese
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_AND_COLON, "LU:3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_AND_COLON, "LI3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_V, "LV3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_V, "LI3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_UNICODE, "LÜ3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_UNICODE, "LI3")]
        //  Traditional Chinese
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_AND_COLON, "LU:3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_V, "LV3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_UNICODE, "LÜ3")]
        public void TestCaseType(char ch, HanyuPinyinVCharType vcharType, string result)
        {
            var format = new HanyuPinyinOutputFormat
            {
                CaseType = HanyuPinyinCaseType.UPPERCASE,
                VCharType = vcharType
            };
            Assert.Equal(result, PinyinHelper.ToHanyuPinyinStringArray(ch, format)[0]);
        }

        [Theory(DisplayName = "Test get hanyupinyin with invalid format")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_AND_COLON)]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_AND_COLON)]
        public void TestToneMarkWithUAndColon(char ch, HanyuPinyinVCharType vcharType)
        {
            Assert.Throws<InvalidHanyuPinyinFormatException>(() =>
            {
                var format = new HanyuPinyinOutputFormat
                {
                    ToneType = HanyuPinyinToneType.WITH_TONE_MARK,
                    VCharType = vcharType
                };
                PinyinHelper.ToHanyuPinyinStringArray(ch, format);
            });
        }

        [Theory(DisplayName = "Test get hanyupinyin with tone mark format")]
        #region Test data
        //  Simplified Chinese
        [InlineData('爸', "bà")]
        [InlineData('波', "bō")]
        [InlineData('苛', "kē")]
        [InlineData('李', "lǐ")]
        [InlineData('露', "lù")]
        [InlineData('吕', "lǚ")]
        [InlineData('来', "lái")]
        [InlineData('背', "bèi")]
        [InlineData('宝', "bǎo")]
        [InlineData('抠', "kōu")]
        [InlineData('虾', "xiā")]
        [InlineData('携', "xié")]
        [InlineData('表', "biǎo")]
        [InlineData('球', "qiú")]
        [InlineData('花', "huā")]
        [InlineData('落', "luò")]
        [InlineData('槐', "huái")]
        [InlineData('徽', "huī")]
        [InlineData('月', "yuè")]
        [InlineData('汗', "hàn")]
        [InlineData('狠', "hěn")]
        [InlineData('邦', "bāng")]
        [InlineData('烹', "pēng")]
        [InlineData('轰', "hōng")]
        [InlineData('天', "tiān")]
        [InlineData('银', "yín")]
        [InlineData('鹰', "yīng")]
        [InlineData('想', "xiǎng")]
        [InlineData('炯', "jiǒng")]
        [InlineData('环', "huán")]
        [InlineData('云', "yún")]
        [InlineData('黄', "huáng")]
        [InlineData('渊', "yuān")]
        [InlineData('儿', "ér")]
        //  Traditional Chinese
        [InlineData('呂', "lǚ")]
        [InlineData('來', "lái")]
        [InlineData('寶', "bǎo")]
        [InlineData('摳', "kōu")]
        [InlineData('蝦', "xiā")]
        [InlineData('攜', "xié")]
        [InlineData('轟', "hōng")]
        [InlineData('銀', "yín")]
        [InlineData('鷹', "yīng")]
        [InlineData('環', "huán")]
        [InlineData('雲', "yún")]
        [InlineData('黃', "huáng")]
        [InlineData('淵', "yuān")]
        [InlineData('兒', "ér")]
        #endregion
        public void TestToneMark(char ch, string result)
        {
            var format = new HanyuPinyinOutputFormat
            {
                ToneType = HanyuPinyinToneType.WITH_TONE_MARK,
                VCharType = HanyuPinyinVCharType.WITH_U_UNICODE
            };
            Assert.Equal(result, PinyinHelper.ToHanyuPinyinStringArray(ch, format)[0]);
        }

        [Theory(DisplayName = "Test get hanyupinyin with out tone format")]
        #region Test data
        //  Simplified Chinese
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, "lu:")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, "li")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, "LU:")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, "LI")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, "lv")]
        [InlineData('李', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, "li")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, "LV")]
        [InlineData('李', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, "LI")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, "lü")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, "li")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, "LÜ")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, "LI")]
        //  Traditional Chinese
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, "lu:")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, "LU:")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, "lv")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, "LV")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, "lü")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, "LÜ")]
        #endregion
        public void TestWithoutToneNumber(char ch, HanyuPinyinVCharType vcharType, HanyuPinyinCaseType caseType, string result)
        {
            var format = new HanyuPinyinOutputFormat
            {
                ToneType = HanyuPinyinToneType.WITHOUT_TONE,
                VCharType = vcharType,
                CaseType = caseType
            };
            Assert.Equal(result, PinyinHelper.ToHanyuPinyinStringArray(ch, format)[0]);
        }

        [Theory(DisplayName = "Test get hanyupinyin with tone number format")]
        #region Test data
        //  Simplified Chinese
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, "lu:3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, "li3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, "LU:3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, "LI3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, "lv3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, "li3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, "LV3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, "LI3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, "lü3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, "li3")]
        [InlineData('吕', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, "LÜ3")]
        [InlineData('李', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, "LI3")]
        //  Traditional Chinese
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, "lu:3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, "LU:3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, "lv3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, "LV3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, "lü3")]
        [InlineData('呂', HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, "LÜ3")]
        #endregion
        public void TestWithToneNumber(char ch, HanyuPinyinVCharType vcharType, HanyuPinyinCaseType caseType, string result)
        {
            var format = new HanyuPinyinOutputFormat
            {
                ToneType = HanyuPinyinToneType.WITH_TONE_NUMBER,
                VCharType = vcharType,
                CaseType = caseType
            };
            Assert.Equal(result, PinyinHelper.ToHanyuPinyinStringArray(ch, format)[0]);
        }

        [Category("Simplified Chinese")]
        [Theory(DisplayName = "Test character with multiple pronounciations")]
        #region Test data
        //  Simplified Chinese
        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou2", "lu:3" })]
        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU2", "LU:3" })]
        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou2", "lv3" })]
        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU2", "LV3" })]
        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou2", "lü3" })]
        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU2", "LÜ3" })]

        [InlineData('偻', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou", "lu:" })]
        [InlineData('偻', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU", "LU:" })]
        [InlineData('偻', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou", "lv" })]
        [InlineData('偻', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU", "LV" })]
        [InlineData('偻', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou", "lü" })]
        [InlineData('偻', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU", "LÜ" })]

        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_MARK, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, new string[] { "lóu", "lǚ" })]
        [InlineData('偻', HanyuPinyinToneType.WITH_TONE_MARK, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, new string[] { "LÓU", "LǙ" })]
        //  Traditional Chinese
        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou2", "lu:3" })]
        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU2", "LU:3" })]
        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou2", "lv3" })]
        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU2", "LV3" })]
        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou2", "lü3" })]
        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_NUMBER, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU2", "LÜ3" })]

        [InlineData('僂', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou", "lu:" })]
        [InlineData('僂', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_AND_COLON, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU", "LU:" })]
        [InlineData('僂', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou", "lv" })]
        [InlineData('僂', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_V, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU", "LV" })]
        [InlineData('僂', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, new string[] { "lou", "lü" })]
        [InlineData('僂', HanyuPinyinToneType.WITHOUT_TONE, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, new string[] { "LOU", "LÜ" })]

        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_MARK, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.LOWERCASE, new string[] { "lóu", "lǚ" })]
        [InlineData('僂', HanyuPinyinToneType.WITH_TONE_MARK, HanyuPinyinVCharType.WITH_U_UNICODE, HanyuPinyinCaseType.UPPERCASE, new string[] { "LÓU", "LǙ" })]
        #endregion
        public void TestCharWithMultiplePronouciations(
            char ch, HanyuPinyinToneType toneType, HanyuPinyinVCharType vcharType,
            HanyuPinyinCaseType caseType, string[] result)
        {
            var format = new HanyuPinyinOutputFormat
            {
                ToneType = toneType,
                VCharType = vcharType,
                CaseType = caseType
            };
            Assert.True(Enumerable.SequenceEqual(result, PinyinHelper.ToHanyuPinyinStringArray(ch, format)));
        }

        #endregion

    }

}