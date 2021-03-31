using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonMasker
{
    public class JsonMasker
    {
        public string MaskJson(string source, IEnumerable<MaskerConfig> configs)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            JToken jtoken;

            try
            {
                jtoken = JToken.Parse(source);
            }
            catch (Exception)
            {
                return source;
            }

            return MaskJson(jtoken, configs);
        }

        public string MaskJson(JToken token, IEnumerable<MaskerConfig> configs)
        {
            if (token == null)
                return string.Empty;

            if (configs == null || configs.Count() == 0)
                token.ToString();

            foreach (var config in configs)
            {
                foreach (JToken match in token.SelectTokens(config.JsonPath))
                {
                    match.Replace(new JValue(MaskValue(match.ToString(), config.ShowFirst, config.ShowLast)));
                }
            }

            return token.ToString();
        }

        private string MaskValue(string jsonValue, int showFromHead, int showFromTail)
        {
            if (string.IsNullOrWhiteSpace(jsonValue))
                return string.Empty;

            if ((showFromHead + showFromTail) > jsonValue.Length)
                return jsonValue;

            var maskLength = jsonValue.Length - (showFromHead + showFromTail);
            var maskArray = Enumerable.Range(1, maskLength).Select(x => '*').ToArray();

            return $"{jsonValue.GetFirst(showFromHead)}{new string(maskArray)}{jsonValue.GetLast(showFromTail)}";
        }
    }
}
