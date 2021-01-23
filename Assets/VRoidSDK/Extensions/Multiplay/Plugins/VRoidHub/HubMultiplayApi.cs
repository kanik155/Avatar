using System;
using System.Collections.Generic;
using UnityEngine;
using VRoidSDK;
using VRoidSDK.Networking;
using VRoidSDK.OAuth;

namespace VRoidSDK.Extension.Multiplay
{
    /// <summary>
    /// VRoid HubへAPIリクエストを送信する
    /// </summary>
    /// <remarks>
    /// Multiplayスコープで利用可能なAPIと、認証なしで利用可能なAPIを提供している
    /// </remarks>
    public class HubMultiplayApi
    {
        /// <summary>
        /// マルチプレイ用のダウンロードライセンスを発行する
        /// </summary>
        /// <remarks>
        /// 認証ユーザーしかキャラクターモデルを利用しない場合はHubApi.PostDownloadLicenseやHubModelDeserializerを使ってください
        /// </remarks>
        /// <param name="characterModelId">キャラクターモデルID</param>
        /// <param name="onSuccess">成功した時のコールバック</param>
        /// <param name="onError">失敗した時のコールバック</param>
        /// <remarks>
        /// 使用可能スコープ: multiplay
        /// </remarks>
        public static void PostDownloadLicenseMultiplay(string characterModelId, Action<DownloadLicense> onSuccess, Action<ApiErrorFormat> onError)
        {
            var requestParams = new ApiRequestParam();
            requestParams.AddParam("character_model_id", characterModelId);

            var request = new GenericDataRequest<DownloadLicense>("/api/download_licenses/multiplay")
            {
                Methods = HTTPMethods.Post,
                Params = requestParams
            };
            request.SendRequest(onSuccess, onError);
        }

        /// <summary>
        /// 認証なしでダウンロードライセンス情報を取得
        /// </summary>
        /// <param name="licenseId">取得するライセンスID</param>
        /// <param name="onSuccess">成功した時のコールバック</param>
        /// <param name="onError">失敗した時のコールバック</param>
        public static void GetDownloadLicense(string licenseId, Action<DownloadLicense> onSuccess, Action<ApiErrorFormat> onError)
        {
            var requestParams = new ApiRequestParam();
            var request = new UnauthorizedGenericDataRequest<DownloadLicense>("/api/download_licenses/" + licenseId)
            {
                Params = requestParams
            };
            request.SendRequest(onSuccess, onError);
        }

        /// <summary>
        /// 認証なしでダウンロードライセンスに紐づくモデルのバージョンを取得(タイムアウトは300秒)
        /// </summary>
        /// <param name="licenseId">ライセンスID</param>
        /// <param name="onSuccess">成功した時のコールバック</param>
        /// <param name="onProgress">APIリクエスト中のコールバック</param>
        /// <param name="onError">失敗した時のコールバック</param>
        public static void GetDownloadLicenseDownload(string licenseId, Action<byte[]> onSuccess, Action<float> onProgress, Action<ApiErrorFormat> onError)
        {
            GetDownloadLicenseDownload(licenseId, 300, onSuccess, onProgress, onError);
        }


        /// <summary>
        /// 認証なしでダウンロードライセンスに紐づくモデルのバージョンを取得
        /// </summary>
        /// <param name="licenseId">ライセンスID</param>
        /// <param name="timeout">タイムアウト(秒)</param>
        /// <param name="onSuccess">成功した時のコールバック</param>
        /// <param name="onProgress">APIリクエスト中のコールバック</param>
        /// <param name="onError">失敗した時のコールバック</param>
        public static void GetDownloadLicenseDownload(string licenseId, int timeout, Action<byte[]> onSuccess, Action<float> onProgress, Action<ApiErrorFormat> onError)
        {
            var request = new UnauthorizedByteRequest("/api/download_licenses/" + licenseId + "/download")
            {
                Headers = new Dictionary<string, string>() { { "Accept-Encoding", "gzip" } },
                Timeout = timeout,
                OnDownloadProgress = onProgress
            };
            request.SendRequest(onSuccess, onError);
        }
    }
}
