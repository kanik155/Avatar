using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI.ProceduralImage;
using TMPro;
using UnityEngine.XR;

namespace Comony
{
    public class AvatarUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI userName;
        [SerializeField] private TextMeshProUGUI userOptionText;
        [SerializeField] private GameObject userIcon;
        [SerializeField] private Vector3 positionOffset = new Vector3(0, 2f, 0);
        [SerializeField] private JoshH.UI.UIGradient color;
        [SerializeField] private ColorTable colorTable;

        // private Avatar _target;
        private float _characterControllerHeight = 0f;
        private Vector3 _targetPosition;

        private Camera homeCamera;
        private CameraType homeCameraType;
        private VolumeIcon volumeIcon;

        private Camera tragetCamera;

        private void Start()
        {
            tragetCamera = GetComponent<Canvas>().worldCamera = Camera.main;

            volumeIcon = GetComponentInChildren<VolumeIcon>();

            var random = UnityEngine.Random.Range(0, colorTable.linearColor1.Count);
            color.LinearColor1 = colorTable.linearColor1[random];
            color.LinearColor2 = colorTable.linearColor2[random];
        }

        /*
        private void OnEnable()
        {
            CameraManager.Instance.onCameraChanged += OnChangedCamera;
            var camera = CameraManager.Instance.GetCurrentCamera();
            var cameraType = CameraManager.Instance.GetCurrentCameraType();
            OnChangedCamera(camera, cameraType);
        }

        private void OnDisable()
        {
            CameraManager.Instance.onCameraChanged -= OnChangedCamera;
        }
        */

        /*
        private void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
            }
            if (homeCamera == null)
            {
                var camera = CameraManager.Instance.GetCurrentCamera();
                var cameraType = CameraManager.Instance.GetCurrentCameraType();
                OnChangedCamera(camera, cameraType);
            }

            volumeIcon.Volume = _target.micVolumeLevel;
        }
        */

        private void LateUpdate()
        {
            /*
            _targetPosition = _target.transform.position;
            _targetPosition.y += _characterControllerHeight;
            transform.position = _targetPosition + positionOffset;

            if (homeCamera)
            {
                transform.rotation = homeCamera.transform.rotation;
            }
            */
            transform.position = transform.parent.position + positionOffset;
            transform.rotation = tragetCamera.transform.rotation;
        }

        /*
        private void OnChangedCamera(Camera camera, CameraType cameraType)
        {
            homeCamera = camera;
            homeCameraType = cameraType;
        }
        */

        /*
        public void SetTarget(Avatar target)
        {
            _target = target;
            if (userName != null)
            {
                userName.text = _target.photonView.Owner.NickName;
                userOptionText.text = _target.userOptionText;

                StartCoroutine(DelayMethod(1, () =>
                {
                    var newPos = new Vector3(-(userName.renderedWidth / 2), 0.0f, 0.0f);
                    userIcon.transform.localPosition = newPos;
                }));

                if (!string.IsNullOrEmpty(_target.userIconUrl))
                {
                    var loader = userIcon.GetComponent<ProceduralImageLoader>();
                    loader?.Load(_target.userIconUrl);
                }

                var random = UnityEngine.Random.Range(0, colorTable.linearColor1.Count);
                color.LinearColor1 = colorTable.linearColor1[random];
                color.LinearColor2 = colorTable.linearColor2[random];
            }
        }
        */

        private IEnumerator DelayMethod(int delayFrameCount, Action action)
        {
            for (var i = 0; i < delayFrameCount; i++)
            {
                yield return null;
            }

            action();
        }
    }
}
