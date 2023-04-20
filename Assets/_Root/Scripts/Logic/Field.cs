using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private int gridSizeX;
        [SerializeField] private int gridSizeY;
        [SerializeField] private float cubeSize;
        [SerializeField] private List<Cell> cells;
        
        private Cell[,] _cells;

        void Start()
        {
            InitializeGrid();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                foreach (Cell cell in cells)
                    cell.ResetCell();
        }

        private void InitializeGrid()
        {
            _cells = new Cell[gridSizeX, gridSizeY];
            int i = 0;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    cells[i].Initialize(new Vector2Int(x, y));
                    _cells[x, y] = cells[i];
                    i++;
                }
            }
        }

        public bool GetCellsInRange(Vector2Int startPosition, Vector2Int currentPosition, out List<Cell> list)
        {
            list = new List<Cell>();
            foreach (Cell cell in _cells)
            {
                if (IsInRange(cell.Position.x, startPosition.x, currentPosition.x) &&
                    IsInRange(cell.Position.y, startPosition.y, currentPosition.y))
                {
                    list.Add(cell);
                }
            }

            return list.Count > 0;
        }

        public List<Cell> GetCellsNotInRange(Vector2Int startPosition, Vector2Int currentPosition, List<Cell> currentSelection)
        {
            List<Cell> cells = new List<Cell>();
            foreach (Cell cell in currentSelection)
            {
                if (!IsInRange(cell.Position.x, startPosition.x, currentPosition.x) ||
                    !IsInRange(cell.Position.y, startPosition.y, currentPosition.y))
                {
                    cells.Add(cell);
                }
            }

            return cells;
        }

        private bool IsInRange(int number, int rangeX, int rangeZ)
        {
            if (rangeX <= rangeZ)
                return number >= rangeX && number <= rangeZ;
            
            return number >= rangeZ && number <= rangeX;
        }


#if UNITY_EDITOR

        public void PlaceCells()
        {
            int i = 0;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 position = new Vector3(x * cubeSize, 0, y * cubeSize);
                    cells[i].transform.localPosition = position;
                    cells[i].name = $"{x} {y}";
                    i++;
                }
            }
        }
        #endif
    }
}