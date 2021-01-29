using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using VRoidSDK.Examples.MultiplayExample;
using VRoidSDK.Examples.Core.Localize;

namespace Comony
{
    public class PlayerSpawner : MultiplayExampleEventHandler
    {
        public GameObject ObjectToSpawn;

        private GameObject _avatarRoot;

        [SerializeField] private Routes _routes;

        private void Start()
        {
            _routes.ShowCharacterModels();
        }

        public override void OnDownloadLicenseLoaded(string downloadLicenseId)
        {
            object[] data = new object[] { downloadLicenseId };
            Vector3 pos;
            Quaternion rot;

            if (_avatarRoot == null)
            {
                var rand = new System.Random();
                pos = new Vector3(rand.Next(-4, 4), 0, rand.Next(-4, 4));
                rot = Quaternion.identity;
                _avatarRoot = PhotonNetwork.Instantiate(ObjectToSpawn.name, pos, rot, 0, data);
            }
            else
            {
                _avatarRoot.GetPhotonView().RPC("ChangeAvatarModel", RpcTarget.AllBuffered, downloadLicenseId);
            }
        }

        public override void OnModelLoaded(GameObject go)
        {
        }

        public override void OnLangChanged(Translator.Locales locale)
        {
            switch (locale)
            {
                case Translator.Locales.JA:
                    break;
                case Translator.Locales.EN:
                    break;
                default:
                    break;
            }
        }
    }
}
