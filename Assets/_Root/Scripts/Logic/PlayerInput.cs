using System;
using UnityEngine;

namespace Scripts.Logic
{
    public class PlayerInput : MonoBehaviour
    {
        public event Action ButtonDown;
        public event Action ButtonUp;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
                ButtonDown?.Invoke();

            if (Input.GetMouseButtonUp(0)) 
                ButtonUp?.Invoke();
        }
    }
}