using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic
{
    public class Field : MonoBehaviour
    {
        private Cell[,] _grid;

        public void Initialize(Cell[,] grid)
        {
            _grid = grid;
            foreach (Cell cell in grid) 
                cell.ResetCell();
        }

        public bool GetCellsInRange(Vector2Int startPosition, Vector2Int currentPosition, out List<Cell> list)
        {
            list = new List<Cell>();
            foreach (Cell cell in _grid)
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
    }
}