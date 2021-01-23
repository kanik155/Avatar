using VRoidSDK;
using VRoidSDK.Examples.Core.Model;

namespace VRoidSDK.Examples.MultiplayExample.Model
{
    public class CharacterModelDetailModel : ApplicationModel
    {
        public CharacterModel? CharacterModel { get; set; }
        public bool IsLicenseAccepted { get; set; }
    }
}
