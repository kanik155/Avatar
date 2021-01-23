using System;
using VRoidSDK.Extension.Multiplay;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.MultiplayExample.Model;
using VRoidSDK.Examples.MultiplayExample.Renderer;

namespace VRoidSDK.Examples.MultiplayExample.Controller
{
    public class DownloadProgressController : BaseController
    {
        private DownloadProgressModel _model;
        private LoginController _loginController;
        private MultiplayExampleEventHandler _eventHandler;

        public DownloadProgressController(LoginController loginController, MultiplayExampleEventHandler eventHandler)
        {
            _model = new DownloadProgressModel();
            _loginController = loginController;
            _eventHandler = eventHandler;
        }

        public void InvokeLicenseLoadedEvent(string downloadLicenseId)
        {
            if (_eventHandler != null)
            {
                _eventHandler.OnDownloadLicenseLoaded(downloadLicenseId);
            }
        }

        public void SetLicenseId(string downloadLicenseId)
        {
            _model.DownloadLicenseId = downloadLicenseId;
        }

        public void UseDownloadLicense(Action<IRenderer> onResponse)
        {
            // MARK: don't check login
            // CheckLogin(_loginController, onResponse, (_) =>
            // {
            if (_model.DownloadLicenseId == "")
            {
                return;
            }

            _model.Active = true;
            onResponse(new DownloadProgressRenderer(_model));

            try
            {
                LoadCharacterAsync(onResponse);
            }
            catch (Exception e)
            {
                GetException(onResponse)(e);
            }
            // });
        }

        private void LoadCharacterAsync(Action<IRenderer> onResponse)
        {
            HubMultiplayModelDeserializer.Instance.LoadCharacterAsync(
                downloadLicenseId: _model.DownloadLicenseId,
                option: new HubModelDeserializerOption()
                {
                    DownloadTimeout = 300,
                },
                onLoadComplete: (go) =>
                {
                    _model.Active = false;
                    onResponse(new DownloadProgressRenderer(_model));

                    if (_eventHandler != null)
                    {
                        _eventHandler.OnModelLoaded(go);
                    }
                },
                onDownloadProgress: (progress) =>
                {
                    _model.Progress = progress;
                    onResponse(new DownloadProgressRenderer(_model));
                },
                onError: GetException(onResponse));
        }

        private Action<Exception> GetException(Action<IRenderer> onResponse)
        {
            return (error) =>
            {
                _model.ApiError = new ApiErrorFormat()
                {
                    message = error.Message
                };
                _model.Active = false;
                onResponse(new DownloadProgressRenderer(_model));
                onResponse(new ApiErrorRenderer(_model));
            };
        }
    }
}
