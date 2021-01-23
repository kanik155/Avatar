using System.Collections.Generic;
using UnityEngine;
using VRoidSDK.Examples.Core.View.Parts;
using VRoidSDK.Examples.MultiplayExample;
using Component = VRoidSDK.Examples.Core.View.Component;

namespace VRoidSDK.Examples.MultiplayExample.View
{
    public class CharacterModelsView : Component
    {
        [SerializeField] private Routes _routes;

        public Tab selectTab;
        public VerticalScrollGroup userCharacterModelsScrollRoot;
        public Button seeMoreButton;

        public void SetCharacterModelThumbnails(List<CharacterModel> characterModels)
        {
            userCharacterModelsScrollRoot.Insert<CharacterModel, CharacterModelThumbnail>(characterModels, (characterModelThumb) =>
            {
                _routes.ShowCharacterModel(characterModelThumb.CharacterModel);
            });
        }
    }
}
