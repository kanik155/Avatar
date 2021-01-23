using System;
using UnityEngine;
using VRoidSDK.IO;
using VRoidSDK.OAuth;

namespace VRoidSDK.Extension.Multiplay
{
    /// <summary>
    /// 状況に応じてモデルをロードするモジュールを作成するファクトリー
    /// </summary>
    /// <remarks>
    /// ファイルがすでにキャッシュ上に存在する場合は、キャッシュからロードするモジュールを作成し、
    /// 存在しない場合は、HubApiを実行してダウンロードしてからロードするモジュールを作成する
    /// </remarks>
    public static class MultiplayModelLoaderFactory
    {
        /// <summary>
        /// モデルをロードするモジュールを作成する
        /// </summary>
        /// <param name="downloadLicenseId">ロードするダウンロードライセンスのID</param>
        /// <param name="coroutineHandler">コルーチンが実行できるハンドラオブジェクト</param>
        /// <param name="maxCacheCount">キャッシュの最大保持件数</param>
        /// <param name="onSuccess">成功時のコールバック関数</param>
        /// <param name="onError">失敗時のコールバック関数</param>
        public static void Create(string downloadLicenseId, ICoroutineHandlable coroutineHandler, HubModelDeserializerOption option, Action<IModelLoader> onSuccess, Action<ApiErrorFormat> onError)
        {
            var storage = CacheFileInfoStorage.MultiplayCharacterModelVersion;
            var fileStorage = new CharacterModelVersionCacheFile(MultiplaySdkConfiguration.Instance.AppPassword);

            HubMultiplayApi.GetDownloadLicense(
                licenseId: downloadLicenseId,
                onSuccess: (DownloadLicense license) =>
                {
                    var cacheFileInfo = storage.GetObject(license.FileName);
                    var cacheFileCleaner = new CacheFileCleaner(storage, fileStorage);

                    if (cacheFileInfo != null && fileStorage.IsFileExist(cacheFileInfo.FilePath))
                    {
                        cacheFileInfo.UpdateToLongestExpirationTime(license.ExpiresAtDateTime());
                        storage.SetValue(cacheFileInfo.FilePath, cacheFileInfo);
                        storage.Save();
                        cacheFileCleaner.CleanExpiredCacheFile();

                        var loadModule = new CharacterModelVersionFileLoad(storage, fileStorage);
                        onSuccess(new ModelCachedLoader(license, coroutineHandler, UnityThreadQueue.Instance, loadModule));
                        return;
                    }

                    cacheFileCleaner.CleanExpiredCacheFile();
                    cacheFileCleaner.Clean(Math.Max(option.MaxCacheCount - 1, 0));

                    var saveModule = new CharacterModelVersionFileSave(storage, fileStorage);
                    onSuccess(new ModelDownloadLoader(license, saveModule, HubMultiplayApi.GetDownloadLicenseDownload, option.DownloadTimeout));
                },
                onError: onError
            );
        }
    }
}
