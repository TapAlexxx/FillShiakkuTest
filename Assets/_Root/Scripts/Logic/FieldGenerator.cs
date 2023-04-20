using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic
{
    public class FieldGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int size;
        [SerializeField] private float cubeSize = 1;
        [SerializeField] private CellPool pool;
        [SerializeField] private Field field;
        [SerializeField] private FillShikakuGenerator shikakuGenerator;
        
        private Cell[,] _grid;
        
        private void Start()
        {
            GenerateValidLevel();
        }

        private void GenerateValidLevel()
        {
            shikakuGenerator.Generate(size.x, size.y);
            while (shikakuGenerator.IsInvalid)
            {
                shikakuGenerator.Generate(size.x, size.y);
            }

            SetupLevel();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                GenerateValidLevel();
        }

        private void SetupLevel()
        {
            _grid = new Cell[size.x, size.y];
            pool.ResetPool();
            GenerateField();
        }

        private void GenerateField()
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    int number = shikakuGenerator.GetCell(i, j);
                    if (number == 0)
                    {
                        pool.TryGetCell(out Cell cell);
                        Vector3 position = new Vector3(i * cubeSize, 0, j * cubeSize);
                        cell.transform.localPosition = position;
                        cell.name = $"{i} {j}";
                        cell.Initialize(new Vector2Int(i, j));
                        cell.gameObject.SetActive(true);
                        _grid[i, j] = cell;
                    }
                    else
                    {
                        pool.TryGetNumberCell(out NumberCell numberCell);
                        numberCell.Initialize(number);

                        Cell cell = numberCell.GetComponent<Cell>();
                        Vector3 position = new Vector3(i * cubeSize, 0, j * cubeSize);
                        cell.transform.localPosition = position;
                        cell.name = $"{i} {j}";
                        cell.Initialize(new Vector2Int(i, j));
                        cell.gameObject.SetActive(true);
                        _grid[i, j] = cell;
                    }
                    
                }
            }
            
            field.Initialize(_grid);
        }
    }
}