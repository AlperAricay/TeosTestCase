using System;
using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Controllers
{
    public class InputController : SingletonBehaviour<InputController>
    {
        public event Action LmbPressed;
        [NonSerialized] public float HorizontalInput;
        [NonSerialized] public float VerticalInput;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) LmbPressed?.Invoke();
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");
        }
    }
}
