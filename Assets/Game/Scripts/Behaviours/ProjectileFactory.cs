using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Behaviours
{
    public class ProjectileFactory : SingletonBehaviour<ProjectileFactory>
    {
        [SerializeField] private Toggle _explosive, _big, _red;
        
        private const string PelletTag = "Pellet";
        
        public GameObject ProduceProjectile()
        {
            var projectile = ObjectPooler.SharedInstance.GetPooledObject(PelletTag).GetComponent<ProjectileBehaviour>();
            projectile.Initialize(_explosive.isOn, _big.isOn, _red.isOn);
            return projectile.gameObject;
        }
    }
}