using UnityEngine;
using Photon.Pun;

namespace Comony
{
    public class AvatarVisibleCollider : MonoBehaviour
    {
        [SerializeField] private float _intervalTime = 3.0f;

        private PhotonView _photonView;
        private float _currentTime = 0;

        private AvatarController _avatarController;

        private void Awake()
        {
            _photonView = transform.parent.GetComponent<PhotonView>();
            _avatarController = transform.parent.GetComponent<AvatarController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_photonView.IsMine)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("AvatarVisibleCollider"))
                {
                    _avatarController.ShowModel();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_photonView.IsMine)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("AvatarVisibleCollider"))
                {
                    _currentTime = 0;

                    _avatarController.HideModel();

                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_photonView.IsMine)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("AvatarVisibleCollider"))
                {
                    _currentTime += Time.deltaTime;

                    if (_currentTime >= _intervalTime)
                    {
                        _currentTime = 0;

                        _avatarController.LoadAvatarWhenUnload(transform.parent);
                    }
                }
            }
        }
    }
}
