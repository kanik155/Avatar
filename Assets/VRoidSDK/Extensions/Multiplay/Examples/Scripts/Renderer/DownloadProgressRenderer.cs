using System.Collections.Generic;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.Core.View;
using VRoidSDK.Examples.Core.Localize;
using VRoidSDK.Examples.MultiplayExample.Model;
using VRoidSDK.Examples.MultiplayExample.View;
using VRoidSDK.Examples.MultiplayExample.Localize;

namespace VRoidSDK.Examples.MultiplayExample.Renderer
{
    public class DownloadProgressRenderer : IRenderer
    {
        private bool _isActive;
        private float _progress;

        public DownloadProgressRenderer(DownloadProgressModel model)
        {
            _isActive = model.Active;
            _progress = model.Progress;
        }

        public void Rendering(RootView root)
        {
            root.ApiErrorMessage.Active = false;
            var characterModelRoot = (MultiplayRootView)root;
            characterModelRoot.overlay.Active = _isActive;
            characterModelRoot.downloadProgressView.Active = _isActive;

            if (_isActive == false) return;

            characterModelRoot.downloadProgressView.title.Text = Translator.Lang.Get(MultiplayExampleViewKey.ViewMultiplayUseDownloadLicense);
            if (_progress < 1.0f)
            {
                characterModelRoot.downloadProgressView.downloadProgress.Active = true;
                characterModelRoot.downloadProgressView.downloadProgress.Value = _progress;
                characterModelRoot.downloadProgressView.loadingText.Active = false;
            }
            else
            {
                characterModelRoot.downloadProgressView.downloadProgress.Active = false;
                characterModelRoot.downloadProgressView.loadingText.Active = true;
            }
        }
    }
}
