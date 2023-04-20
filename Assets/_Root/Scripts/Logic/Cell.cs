using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Logic
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        
        public Vector2Int Position { get; private set; }
        public bool IsSelected { get; private set; }
        
        
        private void OnValidate()
        {
            if (!_renderer) TryGetComponent(out _renderer);
        }

        public void Initialize(Vector2Int position)
        {
            Position = position;
        }

        public void MakeSelected()
        {
            IsSelected = true;
        }

        public void SetColor(Color color)
        {
            _renderer.material.color = color;
        }

        public void MakeNonSelected()
        {
            _renderer.material.color = Color.white;
            IsSelected = false;
        }
    }
}