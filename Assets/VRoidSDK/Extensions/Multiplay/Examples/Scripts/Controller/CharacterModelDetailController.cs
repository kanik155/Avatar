using System;
using System.Collections.Generic;
using System.Linq;
using VRoidSDK;
using VRoidSDK.Extension.Multiplay;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.MultiplayExample.Model;
using VRoidSDK.Examples.MultiplayExample.Renderer;

namespace VRoidSDK.Examples.MultiplayExample.Controller
{
    public class CharacterModelDetailController : BaseController
    {
        private LoginController _login;
        private CharacterModelDetailModel _model;
        private CharacterModelsController _characterModelsController;
        private AccountCharacterModelDetailController _accountCharacterModelDetailController;
        private DownloadProgressController _downloadProgressController;

        public CharacterModelDetailController(LoginController login, CharacterModelsController characterModelsController,
            AccountCharacterModelDetailController characterModelDownloadController, DownloadProgressController downloadProgressController)
        {
            _login = login;
            _model = new CharacterModelDetailModel();
            _characterModelsController = characterModelsController;
            _accountCharacterModelDetailController = characterModelDownloadController;
            _downloadProgressController = downloadProgressController;
        }

        public void ShowCharacterModel(CharacterModel characterModel, Action<IRenderer> onResponse)
        {
            _model.CharacterModel = characterModel;
            if (characterModel.character.user.id == _login.GetLoggedInUserId())
            {
                _accountCharacterModelDetailController.OpenWithoutAccept(characterModel, onResponse);
            }
            else
            {
                _model.IsLicenseAccepted = false;
                _model.Active = true;
                onResponse(new CharacterModelDetailRenderer(_model));
            }
        }

        public void CheckAccept(bool toggle, Action<IRenderer> onResponse)
        {
            _model.IsLicenseAccepted = toggle;
            onResponse(new CharacterModelDetailRenderer(_model));
        }

        public void CreateDownloadLicense(Action<IRenderer> onResponse)
        {
            CheckLogin(_login, onResponse, (_) =>
            {
                if (_model.CharacterModel == null) return;

                HubMultiplayApi.PostDownloadLicenseMultiplay(_model.CharacterModel.Value.id,
                    (downloadLicense) =>
                    {
                        _accountCharacterModelDetailController.Close(onResponse);
                        _characterModelsController.HideCharacterModels(onResponse);
                        _model.CharacterModel = null;
                        onResponse(new CharacterModelDetailRenderer(_model));
                        _downloadProgressController.InvokeLicenseLoadedEvent(downloadLicense.id);
                    }, (error) =>
                    {
                        _model.ApiError = error;
                        onResponse(new ApiErrorRenderer(_model));
                    });
            });
        }

        public void HideCharacterModel(Action<IRenderer> onResponse)
        {
            _model.CharacterModel = null;
            _model.Active = false;
            onResponse(new CharacterModelDetailRenderer(_model));
        }
    }
}
