using UnityEngine;
using VRoidSDK.OAuth;
using VRoidSDK.Extension.Multiplay;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.Core.Localize;
using VRoidSDK.Examples.MultiplayExample.View;
using VRoidSDK.Examples.MultiplayExample.Controller;
using VRoidSDK.Examples.MultiplayExample.Model;
using VRoidSDK.Examples.MultiplayExample.Localize;

namespace VRoidSDK.Examples.MultiplayExample
{
    public class Routes : MonoBehaviour
    {
        [SerializeField] private SDKConfiguration _sdkConfiguration;
        [SerializeField] private string _appPassword;
        [SerializeField] private MultiplayRootView _rootView;
        [SerializeField] private MultiplayExampleEventHandler _eventHandler;

        private LoginController _loginController;
        private CharacterModelsController _characterModelsController;
        private CharacterModelDetailController _characterModelDetailController;
        private AccountCharacterModelDetailController _characterModelDownloadController;
        private DownloadProgressController _downloadProgressController;

        private void Awake()
        {
            MultiplaySdkConfiguration.Instance.AppPassword = _appPassword;
            Authentication.Instance.Init(_sdkConfiguration.AuthenticateMetaData);
            _loginController = new LoginController(Authentication.Instance, _sdkConfiguration);
            _characterModelsController = new CharacterModelsController(_loginController);
            _characterModelDownloadController = new AccountCharacterModelDetailController();
            _downloadProgressController = new DownloadProgressController(_loginController, _eventHandler);
            _characterModelDetailController = new CharacterModelDetailController(_loginController, _characterModelsController,
                _characterModelDownloadController, _downloadProgressController);
            Translator.Lang = new JaMultiplayEx();
        }

        public void SendAuthorizeCode(UnityEngine.UI.InputField code)
        {
            var text = code.text;
            _loginController.SendAuthorizationCode(text);
        }

        public void ShowLoginPanel()
        {
            _loginController.OpenLogin(Rendering);
        }

        public void CloseLoginPanel()
        {
            _loginController.CloseLogin(Rendering);
        }

        public void Logout()
        {
            _loginController.Logout(Rendering);
            HideCharacterModels();
        }

        public void Login()
        {
            _loginController.Login(Rendering);
        }

        public void ShowCharacterModels()
        {
            _characterModelsController.ShowCharacterModels(Rendering);
        }

        public void HideCharacterModels()
        {
            _characterModelsController.HideCharacterModels(Rendering);
        }

        public void ChangeTab(int tab)
        {
            _characterModelsController.ChangeTab((CharacterModelsModel.Tab)tab, Rendering);
        }

        public void SeeMore()
        {
            _characterModelsController.ShowNextCharacterModels(Rendering);
        }

        public void LocalizeChanged(int locale)
        {
            var translateLocale = (Translator.Locales)locale;
            switch (translateLocale)
            {
                case Translator.Locales.JA:
                    Translator.Lang = new JaMultiplayEx();
                    break;
                case Translator.Locales.EN:
                    Translator.Lang = new EnMultiplayEx();
                    break;
                default:
                    break;
            }

            if (_eventHandler != null)
            {
                _eventHandler.OnLangChanged(translateLocale);
            }
        }

        public void ShowCharacterModel(CharacterModel model)
        {
            _characterModelDetailController.ShowCharacterModel(model, Rendering);
        }

        public void HideCharacterModel()
        {
            _characterModelDetailController.HideCharacterModel(Rendering);
        }

        public void CheckAccept(UnityEngine.UI.Toggle toggle)
        {
            var result = toggle.isOn;
            _characterModelDetailController.CheckAccept(result, Rendering);
        }

        public void CreateDownloadLicense()
        {
            _characterModelDetailController.CreateDownloadLicense(Rendering);
        }

        public void ChangeDownloadLicenseId(string licenseId)
        {
            _downloadProgressController.SetLicenseId(licenseId);
        }

        public void UseDownloadLicense()
        {
            _downloadProgressController.UseDownloadLicense(Rendering);
        }

        public void HideDownloadModel()
        {
            _characterModelDownloadController.Close(Rendering);
        }

        private void Rendering(IRenderer renderer)
        {
            renderer.Rendering(_rootView);
        }
    }
}
