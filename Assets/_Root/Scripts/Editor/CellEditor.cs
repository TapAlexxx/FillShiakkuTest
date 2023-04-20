using Scripts.Logic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _Root.Scripts.Editor
{
    [CustomEditor(typeof(Cell))]
    public class CellEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Cell cell = (Cell)target;
            if (GUILayout.Button("Make NumberCell"))
            {
                cell.gameObject.layer = LayerMask.NameToLayer("NumberCell");
                if (cell.TryGetComponent(out NumberCell numberCell))
                {
                    DestroyImmediate(numberCell);
                    foreach (Transform transform in cell.transform)
                    {
                        DestroyImmediate(transform.gameObject);
                    }
                }
                GameObject canvasObject = SetupCanvas(cell);
                SetupText(canvasObject);
                SetupNumberCell(cell);
            }
        }

        private void SetupNumberCell(Cell cell)
        {
            var numberCell = cell.gameObject.AddComponent<NumberCell>();
            numberCell.Initialize(2);
        }

        private void SetupText(GameObject canvasObject)
        {
            GameObject textObject = new GameObject()
            {
                name = "Text",
                transform = { parent = canvasObject.transform }
            };
            RectTransform textRect = textObject.AddComponent<RectTransform>();
            textRect.anchoredPosition3D = Vector3.zero;
            textRect.localEulerAngles = Vector3.zero;
            textRect.sizeDelta = new Vector2(10, 10);
            textRect.localScale = Vector3.one;

            textObject.AddComponent<CanvasRenderer>();

            TMP_Text text = textObject.AddComponent<TextMeshProUGUI>();

            text.color = Color.black;
            text.enableAutoSizing = true;
            text.fontSizeMin = 2;
            text.fontSizeMax = 6;

            text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;
        }

        private GameObject SetupCanvas(Cell cell)
        {
            GameObject canvasObject = new GameObject
            {
                name = "Canvas",
                transform =
                {
                    parent = cell.transform,
                    position = cell.transform.position,
                    rotation = cell.transform.rotation
                }
            };
            RectTransform canvasRect = canvasObject.AddComponent<RectTransform>();
            canvasRect.anchoredPosition3D = new Vector3(0, 1, 0);
            canvasRect.sizeDelta = new Vector2(10, 10);
            canvasRect.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            canvasRect.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            return canvasObject;
        }
    }
}