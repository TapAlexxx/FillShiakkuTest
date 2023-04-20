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

        private Camera _camera;
        private RaycastHit _hit;
        private int _cellMask;

        private bool _isSelecting;
        private Cell _currentCell;
        private List<Cell> _currentSelection;
        private Vector2Int _startPosition;
        private Vector2Int _currentPosition;
        private List<NumberCell> _numberCells;

        void Start()
        {
            _currentSelection = new List<Cell>();
            _numberCells = new List<NumberCell>();

            _camera = Camera.main;
            _hit = new RaycastHit();
            _cellMask = LayerMask.GetMask("Cell", "NumberCell");
            InitializeGrid();
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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryStartSelecting();
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isSelecting = false;

                if (_numberCells.Count > 1 || _numberCells.Count < 1)
                {
                    ResetSelection();
                }
                else if (_numberCells.Count == 1)
                {
                    if (_numberCells[0].CountToFill != _currentSelection.Count)
                    {
                        ResetSelection();
                    }
                    else
                    {
                        foreach (Cell cell in _currentSelection)
                        {
                            cell.SetColor(_numberCells[0].Color);
                        }
                    }
                }


                _currentCell = null;
                _numberCells = new List<NumberCell>();
            }

            if (_isSelecting)
            {
                UpdateCurrentPosition();
                UpdateCurrentSelection();
            }
        }

        private void ResetSelection()
        {
            foreach (Cell currentSelectionCell in _currentSelection)
                currentSelectionCell.MakeNonSelected();
            _currentSelection = new List<Cell>();
            _isSelecting = false;
            _currentCell = null;
            _numberCells = new List<NumberCell>();
        }

        private void TryStartSelecting()
        {
            _currentSelection = new List<Cell>();
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit, 100, _cellMask))
            {
                if (_hit.collider.TryGetComponent(out NumberCell numberCell))
                {
                    if(!_numberCells.Contains(numberCell))
                        _numberCells.Add(numberCell);
                }
                if (_hit.collider.TryGetComponent(out Cell cell))
                {
                    _isSelecting = true;
                    _startPosition = cell.Position;
                }
            }
        }

        private void UpdateCurrentPosition()
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit, 100, _cellMask))
            {
                if (_hit.collider.TryGetComponent(out Cell cell))
                {
                    if(cell == _currentCell)
                        return;
                    _currentPosition = cell.Position;
                    _currentCell = cell;
                }
            }
        }

        private void UpdateCurrentSelection()
        {
            foreach (Cell cell in _cells)
            {
                if (cell.TryGetComponent(out NumberCell numberCell))
                {
                    if (!_numberCells.Contains(numberCell)) 
                        _numberCells.Add(numberCell);
                }
                if (IsInRange(cell.Position.x, _startPosition.x, _currentPosition.x) &&
                    IsInRange(cell.Position.y, _startPosition.y, _currentPosition.y))
                {
                    if (!_currentSelection.Contains(cell))
                    {
                        if (cell.IsSelected)
                        {
                            ResetSelection();
                            return;
                        }
                        else
                        {
                            _currentSelection.Add(cell);
                        }
                    }

                    Color markColor = _numberCells.Count == 1
                        ? _numberCells[0].Color
                        : Color.grey;
                    
                    cell.MakeSelected();
                    cell.SetColor(markColor);
                }
                else
                {
                    if (_currentSelection.Contains(cell))
                    {
                        cell.MakeNonSelected();
                        _currentSelection.Remove(cell);
                    }
                    if (_numberCells.Contains(numberCell))
                    {
                        _numberCells.Remove(numberCell);
                    }
                }
            }
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