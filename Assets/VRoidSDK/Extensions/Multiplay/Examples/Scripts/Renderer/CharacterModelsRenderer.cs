using System.Collections.Generic;
using VRoidSDK;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.Core.View;
using VRoidSDK.Examples.MultiplayExample.Model;
using VRoidSDK.Examples.MultiplayExample.View;

namespace VRoidSDK.Examples.MultiplayExample.Renderer
{
    public class CharacterModelsRenderer : IRenderer
    {
        private Account? _currentUser;
        private bool _active;
        private bool _isLicenseAccepted;
        private CharacterModelsModel.Tab _activeTab;
        private List<CharacterModel> _characterModels;
        private ApiLinksFormat _next;

        public CharacterModelsRenderer(CharacterModelsModel model)
        {
            _active = model.Active;
            _currentUser = model.CurrentUser;
            _characterModels = model.CharacterModels;
            _activeTab = model.ActiveTab;
            _isLicenseAccepted = model.IsLicenseAccepted;
            _next = model.Next;
        }

        public void Rendering(RootView root)
        {
            root.ApiErrorMessage.Active = false;
            var characterModelRoot = (MultiplayRootView)root;
            characterModelRoot.characterModelsView.Active = _active;

            if (_active)
            {
                characterModelRoot.characterModelsView.selectTab.ActiveIndex = (int)_activeTab;
                characterModelRoot.characterModelsView.SetCharacterModelThumbnails(_characterModels);

                characterModelRoot.characterModelsView.seeMoreButton.Active = _next.next != null;
            }
        }
    }
}
