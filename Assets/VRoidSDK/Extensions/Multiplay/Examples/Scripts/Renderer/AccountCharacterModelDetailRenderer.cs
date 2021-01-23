using System.Collections.Generic;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.Core.View;
using VRoidSDK.Examples.Core.Localize;
using VRoidSDK.Examples.MultiplayExample.Model;
using VRoidSDK.Examples.MultiplayExample.View;
using VRoidSDK.Examples.MultiplayExample.Localize;

namespace VRoidSDK.Examples.MultiplayExample.Renderer
{
    public class AccountCharacterModelDetailRenderer : IRenderer
    {
        private bool _isActive;
        private bool _isAccept;
        private string _characterName;
        private string _characterModelName;
        private string _characterPublisherName;
        private WebImage _portraitImage;

        public AccountCharacterModelDetailRenderer(AccountCharacterModelDetailModel model)
        {
            _isActive = model.Active;
            if (model.CharacterModel != null)
            {
                _characterName = model.CharacterModel.Value.character.name;
                _characterModelName = model.CharacterModel.Value.name;
                _characterPublisherName = model.CharacterModel.Value.character.user.name;
                _portraitImage = model.CharacterModel.Value.portrait_image.sq150;
            }
        }

        public void Rendering(RootView root)
        {
            var characterModelRoot = (MultiplayRootView)root;
            var accountCharacterModelView = characterModelRoot.accountCharacterModelView;
            characterModelRoot.overlay.Active = _isActive;
            accountCharacterModelView.Active = _isActive;

            if (_isActive == false) return;

            accountCharacterModelView.characterModelIcon.Load(_portraitImage);
            accountCharacterModelView.characterName.Text = _characterName;
            accountCharacterModelView.characterModelName.Text = _characterModelName;
            accountCharacterModelView.characterModelPublisherName.Text = _characterPublisherName;

            accountCharacterModelView.buttonGroup.Active = true;
            accountCharacterModelView.buttonGroup.acceptButton.Active = true;
            accountCharacterModelView.buttonGroup.acceptButton.Message.Text = Translator.Lang.Get(MultiplayExampleViewKey.ViewMultiplayCreateDownloadLicense);
            accountCharacterModelView.buttonGroup.cancelButton.Active = true;
            accountCharacterModelView.buttonGroup.cancelButton.Message.Text = Translator.Lang.Get(ExampleViewKey.ViewCharacterModelDetailModelUseCancel);
        }
    }
}
