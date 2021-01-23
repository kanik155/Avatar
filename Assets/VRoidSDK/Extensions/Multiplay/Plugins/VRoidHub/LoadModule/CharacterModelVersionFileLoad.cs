using System.IO;
using System.Security.Cryptography;
using VRoidSDK.IO;

namespace VRoidSDK.Extension.Multiplay
{
    /// <summary>
    /// ダウンロードライセンスをもとにモデルバージョンファイルをロードする
    /// </summary>
    public class CharacterModelVersionFileLoad : IModelLoadable
    {
        private IKeyValueStorable<CacheFileInfo> _storage;
        private IFileRead _fileReader;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="storage">CacheFileInfoを記録するKeyValueStorage
        /// </param>
        /// <param name="fileReader">ファイルを読み込むモジュール
        /// </param>
        public CharacterModelVersionFileLoad(IKeyValueStorable<CacheFileInfo> storage, IFileRead fileReader)
        {
            _storage = storage;
            _fileReader = fileReader;
        }

        /// <summary>
        /// モデルデータをストレージからロードする
        /// </summary>
        /// <param name="license">ロードに使用するダウンロードライセンス</param>
        /// <returns>ロードしたバイナリデータ</returns>
        /// <exception cref="FileNotFoundException">ファイルが存在しない</exception>
        /// <exception cref="CryptographicException">ファイルの復号に失敗</exception>
        public byte[] Load(DownloadLicense license)
        {
            var cacheFileInfo = _storage.GetObject(license.FileName);
            if (cacheFileInfo == null)
            {
                throw new FileNotFoundException(string.Format("CharacterModelVersion {0} is not found", license.FileName));
            }

            byte[] binary = _fileReader.Load(cacheFileInfo.FilePath);
            cacheFileInfo.UpdateLastAccessTime();
            _storage.SetValue(cacheFileInfo.FilePath, cacheFileInfo);
            _storage.Save();

            return binary;
        }
    }
}
