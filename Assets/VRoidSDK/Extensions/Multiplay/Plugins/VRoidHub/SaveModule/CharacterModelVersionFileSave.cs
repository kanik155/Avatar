using System;
using UnityEngine.Networking;
using VRoidSDK.IO;

namespace VRoidSDK.Extension.Multiplay
{
    /// <summary>
    /// モデルバージョンファイルを暗号化して保存する
    /// </summary>
    public class CharacterModelVersionFileSave : IModelSavable
    {
        private IKeyValueStorable<CacheFileInfo> _storage;
        private IFileWrite _fileWriter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="storage">CacheFileInfoを記録するKeyValueStorage
        /// </param>
        /// <param name="fileWriter">ファイルを保存するモジュール
        /// </param>
        public CharacterModelVersionFileSave(IKeyValueStorable<CacheFileInfo> storage, IFileWrite fileWriter)
        {
            _storage = storage;
            _fileWriter = fileWriter;
        }

        /// <summary>
        /// モデルデータをストレージに保存する
        /// </summary>
        /// <param name="license">保存するキャッシュライセンス</param>
        /// <param name="downloadedData">保存するバイナリデータ</param>
        public void Save(DownloadLicense license, byte[] downloadedData)
        {
            var cacheFileInfo = new CacheFileInfo();
            cacheFileInfo.FilePath = license.FileName;
            cacheFileInfo.ExpiresAt = license.ExpiresAtDateTime();
            cacheFileInfo.UpdateLastAccessTime();
            // MARK: Catch IOException
            try
            {
                _fileWriter.Save(cacheFileInfo.FilePath, downloadedData);
            }
            catch (System.IO.IOException e)
            { }
            _storage.SetValue(cacheFileInfo.FilePath, cacheFileInfo);
            _storage.Save();
        }
    }
}
