using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Animator _animator;
        
        private Vector3 _currentMovement = Vector3.zero;
        
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");

        private void Update()
        {
            _currentMovement.x = InputController.Instance.HorizontalInput;
            _currentMovement.z = InputController.Instance.VerticalInput;

            if (_currentMovement.magnitude > 0)
            {
                _currentMovement.Normalize();
                _currentMovement *= _movementSpeed * Time.deltaTime;
                _playerTransform.Translate(_currentMovement, Space.World);
            }

            var velocityZ = Vector3.Dot(_currentMovement.normalized, _playerTransform.forward);
            var velocityX = Vector3.Dot(_currentMovement.normalized, _playerTransform.right);
            
            _animator.SetFloat(VelocityX, velocityX, .1f, Time.deltaTime);
            _animator.SetFloat(VelocityZ, velocityZ, .1f, Time.deltaTime);
        }
    }
}
