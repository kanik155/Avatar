using UnityEngine;

namespace Comony
{
    public class MainCameraBillBoard : MonoBehaviour
    {
        [SerializeField] private float _intervalTime = 0.2f;
        private float _currentTime = 0;

        private void Start()
        {
            _currentTime = _intervalTime;
        }

        private void Update()
        {
            /*
            if (_currentTime < _intervalTime)
            {
                _currentTime += Time.deltaTime;
                return;
            }

            _currentTime = 0;
            */

            transform.LookAt(Camera.main.transform.position);
        }
    }
}
