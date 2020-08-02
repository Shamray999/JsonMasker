using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                    match.Replace(new JValue(MaskValue(match.ToString(), config.CustomMask)));
                }
            }

            return token.ToString();
        }

        private string MaskValue(string jsonValue, CustomMaskType regexMask = CustomMaskType.HideAll)
        {
            if (string.IsNullOrWhiteSpace(jsonValue))
                return string.Empty;

            var maskTemplate = "*****";
            var maskedValue = string.Empty;

            switch (regexMask)
            {
                case CustomMaskType.HideAll:
                    maskedValue = maskTemplate;
                    break;
                case CustomMaskType.ShowLast4Chars:
                    maskedValue = $"{maskTemplate}{jsonValue.GetLast(4)}";
                    break;
                case CustomMaskType.ShowFirst:
                    maskedValue = $"{jsonValue.First()}{maskTemplate}";
                    break;
                case CustomMaskType.ShowFirstAndLast:
                    maskedValue = $"{jsonValue.First()}{maskTemplate}{jsonValue.Last()}";
                    break;
            }

            return maskedValue;
        }
    }
}
