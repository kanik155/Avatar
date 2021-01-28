using System.Text;
using UnityEngine;
using System.IO;
using VRoidSDK.IO;

namespace VRoidSDK.Extension.Multiplay
{
    public class CacheFileInfoStorage : IKeyValueStorable<CacheFileInfo>
    {
        private IFileWrite _fileWriter;
        private ISerializableHash<CacheFileInfo> _saveData;
        private string _storageFilePath;
        private readonly object _allDataLock;

        private static CacheFileInfoStorage s_multiplayCharacterModelVersionInstance;
        public static CacheFileInfoStorage MultiplayCharacterModelVersion
        {
            get
            {
                if (s_multiplayCharacterModelVersionInstance == null)
                {
                    var filePath = "multiplay_character_model_version_cache";
                    var fileReadWriteDeleter = new TemporaryKeyValueStorageFile(MultiplaySdkConfiguration.Instance.AppPassword);
                    var cacheDirectory = PathUtil.MultiplayCharacterModelVersionCacheDirectoryPath;
                    s_multiplayCharacterModelVersionInstance = InitializeInstanceForce(filePath, fileReadWriteDeleter, cacheDirectory);
                }

                return s_multiplayCharacterModelVersionInstance;
            }
        }

        private static CacheFileInfoStorage InitializeInstanceForce(string filePath, IFileReadWriteDelete fileReadWriteDeleter, string cacheDirectoryPath)
        {
            try
            {
                return new CacheFileInfoStorage(filePath, fileReadWriteDeleter, fileReadWriteDeleter);
            }
            catch
            {
                // 読み込みに失敗した場合は関連するファイルを消して初期化する
                fileReadWriteDeleter.Delete(filePath);
                if (Directory.Exists(cacheDirectoryPath))
                {
                    Directory.Delete(cacheDirectoryPath, true);
                }
                return new CacheFileInfoStorage(filePath, fileReadWriteDeleter, fileReadWriteDeleter);
            }
        }

        internal CacheFileInfoStorage(string storageFilePath, IFileRead fileRead, IFileWrite fileWrite)
        {
            _allDataLock = new object();
            _fileWriter = fileWrite;
            _storageFilePath = storageFilePath;
            if (fileRead.IsFileExist(_storageFilePath))
            {
                var jsonfile = Encoding.UTF8.GetString(fileRead.Load(_storageFilePath));
                _saveData = JsonUtility.FromJson<CacheFileInfoHash>(jsonfile);
                return;
            }

            _saveData = new CacheFileInfoHash();
        }

        /// <summary>
        /// 指定したkeyに該当するデータを取得する
        /// </summary>
        /// <param name="key">取得するキー</param>
        /// <returns>取得したオブジェクト</returns>
        public CacheFileInfo GetObject(string key)
        {
            lock (_allDataLock)
            {
                return _saveData[key];
            }
        }

        /// <summary>
        /// 指定したkeyにデータをセットする
        /// </summary>
        /// <param name="key">取得するキー</param>
        /// <param name="value">セットするオブジェクト</param>
        public void SetValue(string key, CacheFileInfo value)
        {
            lock (_allDataLock)
            {
                _saveData[key] = value;
            }
        }

        /// <summary>
        /// 指定したkeyのデータを削除する
        /// </summary>
        /// <param name="key">削除するキー</param>
        public bool RemoveKey(string key)
        {
            lock (_allDataLock)
            {
                return _saveData.Remove(key);
            }
        }

        /// <summary>
        /// keyとvalueのペアの配列を取得する
        /// </summary>
        public SerializablePair<CacheFileInfo>[] ToArray()
        {
            lock (_allDataLock)
            {
                return _saveData.ToArray();
            }
        }

        /// <summary>
        /// <para>メモリにのっているデータをストレージに保存する</para>
        /// </summary>
        public void Save()
        {
            lock (_allDataLock)
            {
                // MARK: Catch IOException
                try
                {
                    var json = JsonUtility.ToJson(_saveData);
                    _fileWriter.Save(_storageFilePath, Encoding.UTF8.GetBytes(json));
                }
                catch (IOException e) { }
            }
        }
    }
}
