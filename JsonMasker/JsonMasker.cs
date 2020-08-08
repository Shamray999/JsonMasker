using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonMasker
{
    public class JsonMasker
    {
        public string MaskJson(string jsonObject, IEnumerable<MaskerConfig> configs)
        {
            if (string.IsNullOrWhiteSpace(jsonObject))
                return string.Empty;

            JToken token = JToken.Parse(jsonObject);
            return MaskJson(token, configs);
        }

        public string MaskJson(JToken token, IEnumerable<MaskerConfig> configs)
        {
            if (token == null)
                return string.Empty;

            if (configs == null || configs.Count() == 0)
                return string.Empty;

            foreach (var config in configs)
            {
                foreach (JToken match in token.SelectTokens(config.JsonPath))
                {
                    match.Replace(new JValue(MaskValue(match.ToString(), config.StartWith, config.EndWith)));
                }
            }

            return token.ToString();
        }

        private string MaskValue(string jsonValue, int startWith, int endWith)
        {
            if (string.IsNullOrWhiteSpace(jsonValue))
                return string.Empty;

            var maskTemplate = "*****";
            
            
            return maskTemplate;
        }
    }
}
