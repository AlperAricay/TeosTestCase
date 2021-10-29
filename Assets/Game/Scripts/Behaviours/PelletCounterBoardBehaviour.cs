using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class PelletCounterBoardBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMesh _pelletCountText;

        private int _currentFiredPelletsCount;
        
        private void Awake() => PlayerShootingBehaviour.ShotsFired += OnShotsFired;

        private void OnShotsFired(int firedPelletsCount)
        {
            _currentFiredPelletsCount += firedPelletsCount;
            _pelletCountText.text = _currentFiredPelletsCount.ToString();
        }
    }
}
