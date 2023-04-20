using UnityEngine;

namespace Scripts.Logic
{
    public class Raycaster : MonoBehaviour
    {
        private Camera _camera;

        private void Awake() => 
            _camera = Camera.main;

        public bool Raycast(int layer, out RaycastHit hit) => 
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, layer);
    }
}