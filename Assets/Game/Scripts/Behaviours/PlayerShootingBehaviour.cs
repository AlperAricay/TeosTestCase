using System;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class PlayerShootingBehaviour : MonoBehaviour
    {
        public static event Action<int> ShotsFired;

        [SerializeField] private LayerMask _groundLayer;
        [Space]
        [Header("WEAPON SETTINGS")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _barrelTransform;
        [SerializeField][Min(.1f)] private float _fireRate;
        [SerializeField][Min(2)] private int _pelletCount;
        [SerializeField][Min(.1f)] private float _bulletSpeed;

        private Camera _camera;
        private float _lastShootingTime;
        private const float MaxDeviationAngle = 45f;
        private const float DeviationMultiplier = 7.5f;

        private void Awake()
        {
            _camera = Camera.main;
            InputController.Instance.LmbPressed += OnShootButtonPressed;
        }

        private void Update() => Aim();

        private void OnShootButtonPressed() => Shoot();

        private void Shoot()
        {
            if (_lastShootingTime + 1 / _fireRate > Time.time) return;
            _lastShootingTime = Time.time;

            var currentDeviationAngle = Mathf.Min(_pelletCount * DeviationMultiplier, MaxDeviationAngle);
            float anglePerPellet;
            var i = 0;
            var angleMultiplier = 0;
            
            if (IsEven(_pelletCount))
            {
                anglePerPellet = currentDeviationAngle * 2 / _pelletCount;
                i = 1;
                angleMultiplier = 1;
            }
            else
                anglePerPellet = currentDeviationAngle * 2 / _pelletCount - 1;

            var limit = _pelletCount + i;
            for (; i < limit; i++)
            {
                var currentPelletAngle = angleMultiplier * anglePerPellet;
                if (IsEven(i))
                {
                    currentPelletAngle *= -1;
                    angleMultiplier++;
                }

                var bullet = ProjectileFactory.Instance.ProduceProjectile();
                var desiredAngle = _playerTransform.localRotation.eulerAngles;
                var bulletRb = bullet.GetComponent<Rigidbody>();
                
                desiredAngle.y += currentPelletAngle;
                bullet.transform.position = _barrelTransform.position;
                bullet.transform.localRotation = Quaternion.Euler(desiredAngle);
                bulletRb.AddForce(bullet.transform.forward * _bulletSpeed, ForceMode.VelocityChange);
            }

            ShotsFired?.Invoke(_pelletCount);
        }

        private void Aim()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _groundLayer)) return;
            var dir = hit.point - _playerTransform.position;
            dir.y = 0;
            dir.Normalize();
            _playerTransform.forward = dir;
        }

        private bool IsEven(int num) => num % 2 == 0;
    }
}
