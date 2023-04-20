using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Logic
{
    public class NumberCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private List<Cell> _savedSelection;

        [field:SerializeField] public int CountToFill { get; private set; }
        [field:SerializeField] public Color Color { get; private set; }

        private void OnValidate()
        {
            text = GetComponentInChildren<TMP_Text>();
            UpdateText();
        }

        private void Awake()
        {
            InitializeDefault();
        }

        private void InitializeDefault()
        {
            _savedSelection = new List<Cell>();
        }

        public void Initialize()
        {
            CountToFill = Random.Range(1, 7);
            UpdateText();
        }

        private void UpdateText()
        {
            text.text = CountToFill.ToString();
        }

        public void SaveSelection(List<Cell> currentSelection)
        {
            _savedSelection = currentSelection;
            foreach (Cell cell in currentSelection)
            {
                cell.InteractedFilled += ResetSelection;
            }
        }

        private void ResetSelection()
        {
            foreach (Cell cell in _savedSelection)
            {
                cell.ResetCell();
                cell.InteractedFilled -= ResetSelection;
            }

            _savedSelection = new List<Cell>();
        }
    }
}