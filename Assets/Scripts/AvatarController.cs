using UnityEngine;
using Photon.Pun;
using VRoidSDK;
using VRoidSDK.Extension.Multiplay;

namespace Comony
{
    public class AvatarController : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
    {
        [SerializeField] private GameObject _unloadIcon;
        [SerializeField] private GameObject _namePlate;
        [SerializeField] private GameObject _avatarVisibleCollider;
        [SerializeField] private float _voiceRange = 2f;
        [SerializeField] private float _despawnHeight = -10f;
        [SerializeField] private float _intervalTime = 3.0f;

        // [SerializeField] private GameObject _firstCamera;

        private Vector3 _correctPlayerPos;
        private Quaternion _correctPlayerRot;
        private Rigidbody _rigidbody;
        private bool _isInitialPos = true;
        private GameObject _vrm;
        private bool _isReady = false;
        private string _downloadLicenseId;

        public bool Stay { get; set; }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                Camera mainCamera = Camera.main;
                // mainCamera.gameObject.SetActive(false);
                // _firstCamera.SetActive(true);

                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
            else
            {
                _avatarVisibleCollider.AddComponent<AvatarVisibleCollider>();
            }

            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = _voiceRange;
        }

        private void Update()
        {
            UpdateControl();
            UpdatePhoton();
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
                transform.position = Vector3.Lerp(transform.position, _correctPlayerPos, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, _correctPlayerRot, Time.deltaTime * 5);
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

                if (_isInitialPos)
                {
                    transform.position = _correctPlayerPos;
                    transform.rotation = _correctPlayerRot;

                    _isInitialPos = false;
                }
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            _downloadLicenseId = (string)photonView.InstantiationData[0];
            _isReady = true;

            if (photonView.IsMine)
            {
                LoadAvatar(gameObject.transform);
            }
        }

        public void ShowModel()
        {
            if (_vrm)
            {
                _vrm.SetActive(true);
                _unloadIcon.SetActive(false);
                _namePlate.SetActive(true);
            }
            else
            {
                LoadAvatar(gameObject.transform);
            }
        }

        public void HideModel()
        {
            if (_vrm)
            {
                _vrm.SetActive(false);
                _unloadIcon.SetActive(true);
                _namePlate.SetActive(false);
            }
        }

        [PunRPC]
        public void ChangeAvatarModel(string downloadLicenseId)
        {
            _downloadLicenseId = downloadLicenseId;

            Destroy(_vrm);
            _vrm = null;
            _unloadIcon.SetActive(true);
            _isReady = true;

            if (photonView.IsMine)
            {
                LoadAvatar(gameObject.transform);
            }
            else
            {
                if (Stay)
                {
                    LoadAvatar(gameObject.transform);
                }
            }
        }

        public void LoadAvatar(Transform parent)
        {
            if (_isReady)
            {
                _isReady = false;

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
                            _namePlate.SetActive(true);

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
                            Debug.LogError("Destroy Avatar");
                            Destroy(go);
                        }
                    },
                    onDownloadProgress: null,
                    onError: (error) =>
                    {
                        Debug.LogError("Download Error");
                    }
                );
            }
        }
    }
}
