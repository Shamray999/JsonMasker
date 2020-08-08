using System;
using System.Collections.Generic;
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
                ""loginData"": {
                  ""UserName"": ""TestUser"",
                  ""Password"": ""TestPassword""
                },
                ""creditCards"": [{ ""cardNum"": ""4581111111111"" }, { ""cardNum"": ""4581111111234"" }, { ""cardNum"": ""4581111117978"" } ]
              }
            }";
        }

        [Fact]
        public void Test1()
        {
            var maskConfig = new List<MaskerConfig>()
            {
                new MaskerConfig(){ JsonPath = "Result.creditCards[*].cardNum"},
            };

            var jsonMasker = new JsonMasker();
            var maskedData = jsonMasker.MaskJson(JsonData, maskConfig);

        }
    }
}
