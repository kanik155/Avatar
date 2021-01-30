using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.UI.ProceduralImage;

namespace Comony
{
    [RequireComponent(typeof(ProceduralImage))]
    public class ProceduralImageLoader : MonoBehaviour
    {
        ProceduralImage rawImage;

        string rawUrl;

        string reservedLoadUrl;

        [SerializeField]
        GameObject loading;

        Vector2 dimension;

        string loadingUrl;

        void Awake()
        {
            rawImage = GetComponent<ProceduralImage>();

            if (rawImage)
            {
                dimension = new Vector2(rawImage.rectTransform.sizeDelta.x, rawImage.rectTransform.sizeDelta.y);
            }
        }

        void OnEnable()
        {
            if (!string.IsNullOrEmpty(reservedLoadUrl))
            {
                StartCoroutine(LoadTextureFromWeb(reservedLoadUrl));
            }
        }

        public void Load(string url)
        {
            if (rawUrl == url)
                return;
            if (loadingUrl == url)
                return;

            Unload();

            if (loading)
            {
                loading.SetActive(true);
            }

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(LoadTextureFromWeb(url));
            }
            else
            {
                reservedLoadUrl = url;
            }
        }

        public void Unload()
        {
            rawImage.sprite = null;
            rawImage.color = new Color(0, 0, 0, 0);
            rawUrl = "";

            if (loading)
            {
                loading.SetActive(false);
            }
        }

        IEnumerator LoadTextureFromWeb(string url)
        {
            loadingUrl = url;

            var req = UnityWebRequestTexture.GetTexture(url);
            req.timeout = 120;

            yield return req.SendWebRequest();

            while (!req.isDone)
            {
                Debug.Log("response: " + req.responseCode);
                if (req.isNetworkError)
                {
                    Debug.Log(req.error);
                }
                Debug.Log("downloaded:" + req.downloadedBytes);
                yield return null;
            }

            if (req.error != null)
            {
                Debug.LogError("Encountered error while downloading: <color=blue>" + url + "</color>: " + req.error);
            }

            // 読み込み対象のものとダウンロードのＵＲＬが違うなら無視
            if (loadingUrl != url)
                yield break;

            if (req.isDone)
            {
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.Log(req.error);
                }
                else
                {
                    var texture = ((DownloadHandlerTexture)req.downloadHandler).texture;
                    rawImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    rawImage.color = new Color(1, 1, 1, 1);
                    rawUrl = url;
                }
            }

            if (loading)
            {
                loading.SetActive(false);
            }

            reservedLoadUrl = null;
        }
    }
}