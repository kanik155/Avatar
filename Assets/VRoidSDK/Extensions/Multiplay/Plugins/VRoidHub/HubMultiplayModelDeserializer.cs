using UnityEngine;
using System;
using System.Collections;

namespace VRoidSDK.Extension.Multiplay
{
    /// <summary>
    /// VRoid Hubのキャラクターを3Dモデルとして読み込む機能を提供するシングルトン
    /// </summary>
    public class HubMultiplayModelDeserializer : MonoBehaviour, ICoroutineHandlable
    {
        // Singleton
        private static HubMultiplayModelDeserializer s_instance;

        /// <summary>
        /// シングルトンオブジェクトを取り出す
        /// </summary>
        public static HubMultiplayModelDeserializer Instance
        {
            get
            {
                if (HubMultiplayModelDeserializer.s_instance == null)
                {
                    GameObject gameObject = new GameObject(Guid.NewGuid().ToString());
                    DontDestroyOnLoad(gameObject);
                    HubMultiplayModelDeserializer.s_instance = gameObject.AddComponent<HubMultiplayModelDeserializer>();
                }
                return HubMultiplayModelDeserializer.s_instance;
            }
        }

        /// <summary>
        /// ダウンロードライセンスのIDからキャラクターモデルのGameObjectを取得する
        /// </summary>
        /// <remarks>
        /// 初めて取り込むキャラクターモデルは、VRoidHubApi経由でモデルデータをダウンロードし、ストレージにキャッシュされる。
        /// 一度取り込まれたキャラクターモデルは、次からキャッシュから読み込まれるようになる
        /// </remarks>
        /// <param name="downloadLicenseId">ダウンロードライセンスのID</param>
        /// <param name="option">オプション</param>
        /// <param name="onLoadComplete">キャラクターモデルの読み込みに成功した時のコールバック</param>
        /// <param name="onDownloadProgress">ダウンロードの進捗状況を通知するコールバック</param>
        /// <param name="onError">エラー発生時のコールバック</param>
        public void LoadCharacterAsync(string downloadLicenseId,
                                        HubModelDeserializerOption option,
                                        Action<GameObject> onLoadComplete,
                                        Action<float> onDownloadProgress,
                                        Action<Exception> onError)
        {
            MultiplayModelLoaderFactory.Create(downloadLicenseId, this, option, (loader) =>
            {
                loader.OnVrmModelLoaded = onLoadComplete;
                loader.OnProgress = onDownloadProgress;
                loader.OnError = onError;
                loader.Load();
            }, (ApiErrorFormat errorFormat) =>
            {
                onError(new ApiRequestFailException(errorFormat));
            });
        }

        /// <summary>
        /// ダウンロードライセンスのIDからキャラクターモデルのGameObjectを取得する
        /// </summary>
        /// <remarks>
        /// 初めて取り込むキャラクターモデルは、VRoidHubApi経由でモデルデータをダウンロードし、ストレージにキャッシュされる。
        /// 一度取り込まれたキャラクターモデルは、次からキャッシュから読み込まれるようになる
        /// </remarks>
        /// <remarks>
        /// オプションはデフォルト値 (キャッシュの最大10件、ダウンロードのタイムアウト300秒)
        /// </remarks>
        /// <param name="downloadLicenseId">ダウンロードライセンスのID</param>
        /// <param name="onLoadComplete">キャラクターモデルの読み込みに成功した時のコールバック</param>
        /// <param name="onDownloadProgress">ダウンロードの進捗状況を通知するコールバック</param>
        /// <param name="onError">エラー発生時のコールバック</param>
        public void LoadCharacterAsync(string downloadLicenseId,
                                        Action<GameObject> onLoadComplete,
                                        Action<float> onDownloadProgress,
                                        Action<Exception> onError)
        {
            LoadCharacterAsync(downloadLicenseId, new HubModelDeserializerOption(), onLoadComplete, onDownloadProgress, onError);
        }

        /// <summary>
        /// コルーチン処理を実行する
        /// </summary>
        /// <param name="routine">処理するコルーチン</param>
        public void RunMonoCoroutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}
