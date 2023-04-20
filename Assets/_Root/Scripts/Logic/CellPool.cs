using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Logic
{
    public class CellPool : MonoBehaviour
    {
        [SerializeField] private int cellPoolSize = 100;
        [SerializeField] private int numberCellPoolSize = 20;
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private NumberCell numberCellPrefab;
        
        private List<Cell> _cells;
        private List<NumberCell> _numberCells;
        
        private void Awake()
        {
            InitializeCellPool();
            InitializeNumberCellPool();
        }

        private void InitializeCellPool()
        {
            _cells = new List<Cell>();
            for (int i = 0; i < cellPoolSize; i++)
            {
                Cell cell = Instantiate(cellPrefab, transform);
                cell.gameObject.SetActive(false);
                _cells.Add(cell);
            }
        }
        
        private void InitializeNumberCellPool()
        {
            _numberCells = new List<NumberCell>();
            for (int i = 0; i < numberCellPoolSize; i++)
            {
                NumberCell cell = Instantiate(numberCellPrefab, transform);
                cell.gameObject.SetActive(false);
                _numberCells.Add(cell);
            }
        }

        public bool TryGetCell(out Cell cell)
        {
            cell = _cells.FirstOrDefault(x => x.gameObject.activeSelf == false);
            return cell != null;
        }
        
        public bool TryGetNumberCell(out NumberCell cell)
        {
            cell = _numberCells.FirstOrDefault(x => x.gameObject.activeSelf == false);
            return cell != null;
        }

        public void ResetPool()
        {
            foreach (Cell cell in _cells) 
                cell.gameObject.SetActive(false);

            foreach (NumberCell numberCell in _numberCells) 
                numberCell.gameObject.SetActive(false);
        }
    }
}