using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Logic
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private Vector3 targetScale = new Vector3(1, 0.2f, 1);
        [SerializeField] private Color defaultColor;
        
        private Vector3 _startPosition;
        private Vector3 _startScale;
        private Vector3 _selectedScale;

        public Vector2Int Position { get; private set; }
        public bool IsSelected { get; private set; }
        public bool IsFilled { get; private set; }
        
        public event Action InteractedFilled;


        private void OnValidate()
        {
            if (!renderer) TryGetComponent(out renderer);
        }

        public void Initialize(Vector2Int position)
        {
            Position = position;
            _startPosition = transform.position;
            _startScale = new Vector3(0.8f, 0.1f, 0.8f);
            _selectedScale = targetScale;
        }

        public void ResetCell()
        {
            renderer.material.color = defaultColor;
            transform.DOMove(_startPosition, 0.2f);
            transform.DOScale(_startScale, 0.2f);
            IsSelected = false;
            IsFilled = false;
        }

        public void MakeSelected()
        {
            IsSelected = true;
            transform.DOMove(_startPosition + Vector3.up, 0.2f);
            transform.DOScale(_selectedScale, 0.2f);
        }

        public void InteractFilled() => 
            InteractedFilled?.Invoke();

        public void SetColor(Color color)
        {
            renderer.material.color = color;
        }

        public void MakeNonSelected()
        {
            renderer.material.color = defaultColor;
            transform.DOMove(_startPosition, 0.2f);
            transform.DOScale(_startScale, 0.2f);
            IsSelected = false;
        }

        public void Fill()
        {
            transform.DOMove(_startPosition, 0.2f);
            transform.DOScale(_selectedScale, 0.2f);
            IsFilled = true;
        }
    }
}