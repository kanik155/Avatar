using VRoidSDK;
using VRoidSDK.Examples.Core.Model;

namespace VRoidSDK.Examples.MultiplayExample.Model
{
    public class DownloadProgressModel : ApplicationModel
    {
        public string DownloadLicenseId { get; set; }
        public float Progress { get; set; }

        public DownloadProgressModel()
        {
            DownloadLicenseId = "";
            Progress = 0.0f;
        }
    }
}
