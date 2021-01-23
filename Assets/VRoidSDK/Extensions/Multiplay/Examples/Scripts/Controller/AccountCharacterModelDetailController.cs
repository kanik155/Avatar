using System;
using System.Collections.Generic;
using System.Linq;
using VRoidSDK;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.MultiplayExample.Model;
using VRoidSDK.Examples.MultiplayExample.Renderer;

namespace VRoidSDK.Examples.MultiplayExample.Controller
{
    public class AccountCharacterModelDetailController : BaseController
    {
        private AccountCharacterModelDetailModel _model;

        public AccountCharacterModelDetailController()
        {
            _model = new AccountCharacterModelDetailModel();
        }

        public void OpenWithoutAccept(CharacterModel characterModel, Action<IRenderer> onResponse)
        {
            _model.CharacterModel = characterModel;
            _model.Active = true;
            onResponse(new AccountCharacterModelDetailRenderer(_model));
        }

        public void Close(Action<IRenderer> onResponse)
        {
            _model.CharacterModel = null;
            _model.Active = false;
            onResponse(new AccountCharacterModelDetailRenderer(_model));
        }
    }
}
