using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace JsonMasker.Tests
{
    public class JsonMaskerUnitTests
    {
        public string JsonData { get; set; }

        public JsonMaskerUnitTests()
        {
            JsonData = @"
            {
              ""Result"": {
                ""UserData"": {
                  ""UserName"": ""TestUser"",
                  ""Password"": ""TestPassword""
                },
                ""CreditCards"": [
                                    { ""CardNum"": ""4581111111111"", ""CVV"": ""123"" },
                                    { ""CardNum"": ""4581111111234"", ""CVV"": ""589"" },
                                    { ""CardNum"": ""4581111115698"", ""CVV"": ""987"" },
                                    { ""CardNum"": ""4581111117978"", ""CVV"": ""987"" }
                                 ]
              }
            }";
        }

        [Fact]
        public void MaskUserNameFull()
        {
            var maskConfig = new List<MaskerConfig>()
            {
                new MaskerConfig(){ JsonPath = "Result.UserData.UserName"},
            };

            var jsonMasker = new JsonMasker();
            var maskedData = jsonMasker.MaskJson(JsonData, maskConfig);

            dynamic maskedJson = JsonConvert.DeserializeObject(maskedData);
            string maskedUserName = maskedJson.Result.UserData.UserName;

            Assert.Equal("********", maskedUserName);
        }

        [Fact]
        public void MaskUserNamePartial()
        {
            var maskConfig = new List<MaskerConfig>()
            {
                new MaskerConfig(){ JsonPath = "Result.UserData.UserName", ShowFirst = 2, ShowLast = 2},
            };

            var jsonMasker = new JsonMasker();
            var maskedData = jsonMasker.MaskJson(JsonData, maskConfig);

            dynamic maskedJson = JsonConvert.DeserializeObject(maskedData);
            string maskedUserName = maskedJson.Result.UserData.UserName;

            Assert.Equal("Te****er", maskedUserName);
        }

        [Fact]
        public void MaskFullCardNumList()
        {
            var maskConfig = new List<MaskerConfig>()
            {
                new MaskerConfig(){ JsonPath = "Result.CreditCards[*].CardNum"},
            };

            var jsonMasker = new JsonMasker();
            var maskedData = jsonMasker.MaskJson(JsonData, maskConfig);
            dynamic maskedJson = JsonConvert.DeserializeObject(maskedData);

            foreach (dynamic item in maskedJson.Result.CreditCards)
            {
                string cardNum = item.CardNum;
                Assert.Equal("*************", cardNum);
            }
        }

        [Fact]
        public void MaskPartialCardNumList()
        {
            var maskConfig = new List<MaskerConfig>()
            {
                new MaskerConfig(){ JsonPath = "$..CardNum", ShowFirst = 4, ShowLast = 4},
            };

            var jsonMasker = new JsonMasker();
            var maskedData = jsonMasker.MaskJson(JsonData, maskConfig);
            dynamic maskedJson = JsonConvert.DeserializeObject(maskedData);

            foreach (dynamic item in maskedJson.Result.CreditCards)
            {
                string cardNum = item.CardNum;
                Assert.Matches(@"\d{4}\*{5}\d{4}$", cardNum);
            }
        }
    }
}
