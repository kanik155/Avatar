using UnityEngine;
using Photon.Pun;

namespace Comony
{
    public class AvatarVisibleCollider : MonoBehaviour
    {
        private PhotonView _photonView;
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

                    _avatarController.Stay = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_photonView.IsMine)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("AvatarVisibleCollider"))
                {
                    _avatarController.HideModel();

                    _avatarController.Stay = false;
                }
            }
        }
    }
}
