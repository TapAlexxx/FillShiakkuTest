using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic
{
    public class CellSelector : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Raycaster raycaster;
        [SerializeField] private Field field;

        private List<Cell> _currentSelection;
        private List<NumberCell> _numberCells;
        
        private Vector2Int _startPosition;
        private Vector2Int _currentPosition;

        private bool _isSelecting;
        private int _cellMask;
        private Cell _currentCell;

        private void Start()
        {
            InitializeDefault();
            playerInput.ButtonDown += OnButtonDown;
            playerInput.ButtonUp += OnButtonUp;
        }

        private void OnDestroy()
        {
            playerInput.ButtonDown -= OnButtonDown;
            playerInput.ButtonUp -= OnButtonUp;
        }

        private void Update()
        {
            if(!_isSelecting)
                return;
            
            UpdateCurrentPosition();
            UpdateNotSelected();
            UpdateCurrentSelection();
        }

        private void UpdateNotSelected()
        {
            if (_currentSelection.Count > 0)
            {
                List<Cell> cellsNotInRange = field.GetCellsNotInRange(_startPosition, _currentPosition, _currentSelection);
                foreach (Cell cell in cellsNotInRange)
                {
                    if (IsNumberCell(cell, out NumberCell numberCell)) 
                        TryRemove(ref _numberCells, numberCell);
                    
                    cell.MakeNonSelected();
                    TryRemove(ref _currentSelection, cell);
                }
            }
        }

        private void UpdateCurrentSelection()
        {
            if (field.GetCellsInRange(_startPosition,_currentPosition, out List<Cell> cellsInRange))
            {
                foreach (Cell cell in cellsInRange)
                {
                    if (IsNumberCell(cell, out NumberCell numberCell)) 
                        TryAdd(ref _numberCells, numberCell);

                    if (!_currentSelection.Contains(cell))
                    {
                        if (cell.IsSelected)
                        {
                            ResetSelection();
                            StopSelect();
                            return;
                        }
                        _currentSelection.Add(cell);
                    }

                    Color markColor = _numberCells.Count == 1
                        ? _numberCells[0].Color
                        : Color.grey;
                    
                    cell.MakeSelected();
                    cell.SetColor(markColor);
                }
            }
        }

        private void UpdateCurrentPosition()
        {
            if (raycaster.Raycast(_cellMask, out RaycastHit hit))
            {
                if (IsCell(hit, out Cell cell))
                {
                    if(CurrentCellIsEqual(cell))
                        return;
                    _currentPosition = cell.Position;
                    _currentCell = cell;
                }
            }
        }

        private void TryRemove<T>(ref List<T> list, T element)
        {
            if (list.Contains(element))
                list.Remove(element);
        }

        private void TryAdd<T>(ref List<T> list, T element)
        {
            if(!list.Contains(element))
                list.Add(element);
        }

        private bool CurrentCellIsEqual(Cell cell) => 
            cell == _currentCell;

        private void InitializeDefault()
        {
            _currentSelection = new List<Cell>();
            _numberCells = new List<NumberCell>();
            _cellMask = LayerMask.GetMask("Cell", "NumberCell");
        }

        private void OnButtonDown() => 
            TryStartSelecting();

        private void OnButtonUp() => 
            ValidateCurrentSelection();

        private void ValidateCurrentSelection()
        {
            if (_numberCells.Count != 1)
                ResetSelection();
            else if (!IsValidCountToFill())
                ResetSelection();
            else
                FillSelection();

            StopSelect();
        }

        private void ResetSelection()
        {
            foreach (Cell currentSelectionCell in _currentSelection)
                currentSelectionCell.MakeNonSelected();
        }

        private void FillSelection()
        {
            foreach (Cell cell in _currentSelection)
                cell.SetColor(_numberCells[0].Color);
        }

        private void StopSelect()
        {
            _isSelecting = false;
            _currentCell = null;
            _numberCells = new List<NumberCell>();
            _currentSelection = new List<Cell>();
        }

        private bool IsValidCountToFill() => 
            _numberCells[0].CountToFill == _currentSelection.Count;

        private void TryStartSelecting()
        {
            _currentSelection = new List<Cell>();
            if (raycaster.Raycast(_cellMask, out RaycastHit hit))
            {
                if (IsNumberCell(hit, out NumberCell numberCell)) 
                    TryAdd(ref _numberCells, numberCell);

                if (IsCell(hit, out Cell cell))
                {
                    _isSelecting = true;
                    _startPosition = cell.Position;
                }
            }
        }

        private bool IsCell(RaycastHit hit, out Cell cell) =>
            hit.collider.TryGetComponent(out cell);

        private bool IsNumberCell(RaycastHit hit, out NumberCell numberCell) => 
            hit.collider.TryGetComponent(out numberCell);

        private bool IsNumberCell(Cell cell, out NumberCell numberCell) => 
            cell.TryGetComponent(out numberCell);
    }
}