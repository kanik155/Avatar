using Photon.Pun;
using UnityEngine;
using VRoidSDK.Examples.MultiplayExample;
using VRoidSDK.Examples.Core.Localize;

namespace Dissonance.Integrations.PhotonUnityNetworking2.Demo
{
    public class PlayerSpawner : MultiplayExampleEventHandler
    {
        public GameObject ObjectToSpawn;

        [SerializeField] private Routes _routes;

        private void Start()
        {
            _routes.ShowCharacterModels();
        }

        public override void OnDownloadLicenseLoaded(string downloadLicenseId)
        {
            object[] data = new object[] { downloadLicenseId };

            var rand = new System.Random();
            var pos = new Vector3(rand.Next(-4, 4), 0, rand.Next(-4, 4));

            PhotonNetwork.Instantiate(ObjectToSpawn.name, pos, Quaternion.identity, 0, data);
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
