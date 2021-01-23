using System.Collections.Generic;
using VRoidSDK.Examples.Core.Localize;

namespace VRoidSDK.Examples.MultiplayExample.Localize
{
    public class JaMultiplayEx : JaEx
    {
        private static readonly Dictionary<string, string> s_localeDictionary = new Dictionary<string, string>()
        {
            {MultiplayExampleViewKey.ViewMultiplayCreateDownloadLicense, "ダウンロードライセンスの新規発行"},
            {MultiplayExampleViewKey.ViewMultiplayUseDownloadLicense, "ダウンロードライセンスIDで利用"},
        };

        public override string Get(string key)
        {
            if (s_localeDictionary.ContainsKey(key))
            {
                return s_localeDictionary[key];
            }

            return base.Get(key);
        }
    }
}
