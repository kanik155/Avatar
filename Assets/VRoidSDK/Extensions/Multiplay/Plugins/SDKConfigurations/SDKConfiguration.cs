using UnityEngine;

namespace VRoidSDK
{
    public partial class SDKConfiguration : ScriptableObject
    {
        public partial class ScopeKind
        {
            [Tooltip("自分がアプリに読み込んだキャラクターを他の人も読み込めるようになります。")]
            [SerializeField]
            public bool Multiplay;
        }
    }
}
