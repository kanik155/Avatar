using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRoidSDK;

namespace VRoidSDK.Extension.Multiplay
{
    [Serializable]
    public class CacheFileInfoHash : ISerializableHash<CacheFileInfo>
    {
        [Serializable]
        public class CacheFileInfoPair : SerializablePair<CacheFileInfo>
        {
            [SerializeField]
            private CacheFileInfo _value;

            public override CacheFileInfo Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        [SerializeField]
        private List<CacheFileInfoPair> _data;
        private SerializableHashOperator<CacheFileInfo, CacheFileInfoPair> _hashOperator;

        public CacheFileInfoHash()
        {
            _data = new List<CacheFileInfoPair>();
            _hashOperator = new SerializableHashOperator<CacheFileInfo, CacheFileInfoPair>(_data);
        }

        /// <summary>
        /// 指定されたキーに関連付けられている値を取得または設定する
        /// </summary>
        /// <param name="key">キー</param>
        public CacheFileInfo this[string key]
        {
            get { return _hashOperator[key]; }
            set { _hashOperator[key] = value; }
        }

        /// <summary>
        /// 指定したkeyのデータを削除する
        /// </summary>
        /// <param name="key">削除するキー</param>
        public bool Remove(string key)
        {
            return _hashOperator.Remove(key);
        }

        /// <summary>
        /// keyとvalueのペアの配列を取得する
        /// </summary>
        public SerializablePair<CacheFileInfo>[] ToArray()
        {
            return _data.Cast<SerializablePair<CacheFileInfo>>().ToArray();
        }
    }
}
