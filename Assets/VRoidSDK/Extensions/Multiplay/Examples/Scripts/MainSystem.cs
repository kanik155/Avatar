using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRoidSDK.Examples.Core.Localize;

namespace VRoidSDK.Examples.MultiplayExample
{
    public class MainSystem : MultiplayExampleEventHandler
    {
        [SerializeField] private Text _modelSelectionLabel;
        [SerializeField] private Routes _routes;

        private void Start()
        {
            // MARK: show avatar select window
            _routes.ShowCharacterModels();
        }

        public override void OnDownloadLicenseLoaded(string downloadLicenseId)
        {
            _routes.ChangeDownloadLicenseId(downloadLicenseId);
            _routes.UseDownloadLicense();
        }

        public override void OnModelLoaded(GameObject go)
        {
            DeleteAllChildren();
            go.transform.parent = this.transform;
        }

        public override void OnLangChanged(Core.Localize.Translator.Locales locale)
        {
            switch (locale)
            {
                case Translator.Locales.JA:
                    _modelSelectionLabel.text = "モデル選択";
                    break;
                case Translator.Locales.EN:
                    _modelSelectionLabel.text = "Select Model";
                    break;
                default:
                    break;
            }
        }

        private void DeleteAllChildren()
        {
            var transformList = new List<Transform>();
            foreach (Transform child in this.gameObject.transform)
            {
                transformList.Add(child);
            }
            this.gameObject.transform.DetachChildren();
            foreach (Transform child in transformList)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
