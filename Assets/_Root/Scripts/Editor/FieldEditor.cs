using Scripts.Logic;
using UnityEditor;
using UnityEngine;

namespace _Root.Scripts.Editor
{
    [CustomEditor(typeof(Field))]
    public class FieldEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Field field = (Field)target;
            if (GUILayout.Button("GenerateGrid"))
            {
                field.PlaceCells();
            }
        }
    }
}