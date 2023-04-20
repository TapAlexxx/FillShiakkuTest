using TMPro;
using UnityEngine;

namespace Scripts.Logic
{
    public class NumberCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        [field:SerializeField] public int CountToFill { get; private set; }
        [field:SerializeField] public Color Color { get; private set; }

        private void OnValidate()
        {
            text = GetComponentInChildren<TMP_Text>();
            UpdateText();
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
    }
}