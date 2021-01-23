using VRoidSDK.Examples.Core.Localize;
using UnityEngine;

namespace VRoidSDK.Examples.MultiplayExample
{
    public abstract class MultiplayExampleEventHandler : MonoBehaviour
    {
        public abstract void OnDownloadLicenseLoaded(string downloadLicenseId);
        public abstract void OnModelLoaded(GameObject go);
        public abstract void OnLangChanged(Translator.Locales locale);
    }
}
