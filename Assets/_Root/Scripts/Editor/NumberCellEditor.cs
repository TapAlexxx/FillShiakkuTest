using Scripts.Logic;
using UnityEditor;
using UnityEngine;

namespace _Root.Scripts.Editor
{
    [CustomEditor(typeof(NumberCell))]
    public class NumberCellEditor : UnityEditor.Editor
    {
        private int _number;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            NumberCell cell = (NumberCell)target;
            _number = EditorGUILayout.IntField("Number of Floors: ", _number);
            if (GUILayout.Button("SetColor from pallete"))
            {
                cell.SetRandomColorFromSource();
            }
        }
    }
}