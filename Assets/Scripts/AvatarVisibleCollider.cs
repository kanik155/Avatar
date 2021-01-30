using UnityEngine;

namespace Comony
{
    public class AvatarVisibleCollider : MonoBehaviour
    {
        public AvatarController _avatarController;

        private void Awake()
        {
            _avatarController = transform.parent.GetComponent<AvatarController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("AvatarVisibleCollider"))
            {
                _avatarController.ShowModel();
                _avatarController.Stay = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("AvatarVisibleCollider"))
            {
                _avatarController.HideModel();
                _avatarController.Stay = false;
            }
        }
    }
}
