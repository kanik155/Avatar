using UnityEngine;
using Photon.Pun;
using VRoidSDK;
using VRoidSDK.Extension.Multiplay;

namespace Comony
{
    public class AvatarController : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
    {
        [SerializeField] private GameObject _unloadIcon;
        [SerializeField] private float _voiceRange = 2f;
        [SerializeField] private float _despawnHeight = -10f;
        [SerializeField] private float _intervalTime = 3.0f;

        private float _currentTime = 0;
        private Vector3 _correctPlayerPos;
        private Quaternion _correctPlayerRot;
        private Rigidbody _rigidbody;
        private bool _isInitialPos = true;
        private GameObject _vrm;

        private string _downloadLicenseId;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = _voiceRange;
        }

        private void Update()
        {
            UpdateControl();
            UpdatePhoton();

            _currentTime += Time.deltaTime;

            if (photonView.IsMine)
            {
                // LoadAvatarWhenUnload(transform.parent);
            }
        }

        private void UpdateControl()
        {
            if (photonView.IsMine)
            {

                var rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
                var speed = Input.GetAxis("Vertical") * Time.deltaTime * 6.0f;

                transform.Rotate(0, rotation, 0);
                var forward = transform.TransformDirection(Vector3.forward);
                transform.position += forward * speed;

                if (transform.position.y < _despawnHeight)
                {
                    transform.position = Vector3.zero;
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        private void UpdatePhoton()
        {
            if (!photonView.IsMine)
            {
                if (_isInitialPos)
                {
                    transform.position = _correctPlayerPos;
                    transform.rotation = _correctPlayerRot;

                    _isInitialPos = false;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, _correctPlayerPos, Time.deltaTime * 5);
                    transform.rotation = Quaternion.Lerp(transform.rotation, _correctPlayerRot, Time.deltaTime * 5);
                }
            }
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                _correctPlayerPos = (Vector3)stream.ReceiveNext();
                _correctPlayerRot = (Quaternion)stream.ReceiveNext();
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            _downloadLicenseId = (string)photonView.InstantiationData[0];

            if (photonView.IsMine)
            {
                LoadAvatarWhenUnload(gameObject.transform, true);
            }
        }

        public void ShowModel()
        {
            if (_vrm)
            {
                _vrm.SetActive(true);
                _unloadIcon.SetActive(false);
            }

            LoadAvatarWhenUnload(gameObject.transform);
        }

        public void HideModel()
        {
            if (_vrm)
            {
                _vrm.SetActive(false);
                _unloadIcon.SetActive(true);
            }
        }

        [PunRPC]
        public void ChangeAvatarModel(string downloadLicenseId)
        {
            _downloadLicenseId = downloadLicenseId;

            Destroy(_vrm);
            _vrm = null;
            _unloadIcon.SetActive(true);

            if (photonView.IsMine)
            {
                LoadAvatarWhenUnload(gameObject.transform);
            }
        }


        public void LoadAvatarWhenUnload(Transform parent, bool force = false)
        {
            if (_vrm == null)
            {
                LoadAvatar(parent, force);
            }
        }

        public void LoadAvatar(Transform parent, bool force = false)
        {
            // if (force || _currentTime >= _intervalTime)
            // {
            _currentTime = 0f;

            HubMultiplayModelDeserializer.Instance.LoadCharacterAsync(
                downloadLicenseId: _downloadLicenseId,
                option: new HubModelDeserializerOption()
                {
                    DownloadTimeout = 300,
                },
                onLoadComplete: (go) =>
                {
                    if (_vrm == null)
                    {
                        go.transform.parent = parent;
                        go.transform.position = parent.position;
                        go.transform.rotation = parent.rotation;
                        _vrm = go;

                        var animator = _vrm.GetComponent<Animator>();
                        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AvatarAnimatorController");
                        _unloadIcon.SetActive(false);

                        if (photonView.IsMine)
                        {
                            _vrm.AddComponent<AvatarAnimatorControllerController>();
                        }

                        PhotonAnimatorView photonAnimatorView = _vrm.AddComponent<PhotonAnimatorView>();

                        for (var count = 0; count < animator.layerCount; count++)
                        {
                            photonAnimatorView.SetLayerSynchronized(count, PhotonAnimatorView.SynchronizeType.Discrete);
                        }

                        foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
                        {
                            photonAnimatorView.SetParameterSynchronized(animatorControllerParameter.name,
                                (PhotonAnimatorView.ParameterType)animatorControllerParameter.type,
                                PhotonAnimatorView.SynchronizeType.Discrete);
                        }

                        photonView.ObservedComponents.Add(photonAnimatorView);
                    }
                    else
                    {
                        Debug.LogError("Destroy VRM");
                        Destroy(go);
                    }
                },
                onDownloadProgress: null,
                onError: null
            );
            //}
        }
    }
}
