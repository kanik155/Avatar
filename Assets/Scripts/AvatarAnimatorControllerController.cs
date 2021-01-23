using UnityEngine;

namespace Comony
{
    public class AvatarAnimatorControllerController : MonoBehaviour
    {
        [SerializeField] private float _intervalTime = 0.2f;
        [SerializeField] private Animator _animator;

        private float _currentTime = 0;
        private Vector3 _prePos = Vector3.zero;
        private float _speed;

        private void Start()
        {
            _prePos = transform.position;
            _animator = GetComponent<Animator>();
            _currentTime = _intervalTime;
        }

        void Update()
        {
            if (_currentTime < _intervalTime)
            {
                _currentTime += Time.deltaTime;
                return;
            }

            _currentTime = 0;

            _speed = (transform.position - _prePos).sqrMagnitude * 100;


            _animator.SetFloat("Foward", _speed);
            _prePos = transform.position;
        }
    }
}
