
namespace VRoidSDK.Extension.Multiplay
{
    public class MultiplaySdkConfiguration
    {
        private static MultiplaySdkConfiguration s_instance = null;
        public static MultiplaySdkConfiguration Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new MultiplaySdkConfiguration();
                }

                return s_instance;
            }
        }

        // FIXME: VRMの暗号化に使用するパスワードを文字列で指定してください
        public string AppPassword = null;
        private MultiplaySdkConfiguration() { }
    }
}
