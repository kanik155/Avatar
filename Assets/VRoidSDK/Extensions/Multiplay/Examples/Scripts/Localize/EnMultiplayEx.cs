using System.Collections.Generic;
using VRoidSDK.Examples.Core.Localize;

namespace VRoidSDK.Examples.MultiplayExample.Localize
{
    public class EnMultiplayEx : EnEx
    {
        private static readonly Dictionary<string, string> s_localeDictionary = new Dictionary<string, string>()
        {
            {MultiplayExampleViewKey.ViewMultiplayCreateDownloadLicense, "Create multiplay license"},
            {MultiplayExampleViewKey.ViewMultiplayUseDownloadLicense, "Use Download License"},
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
