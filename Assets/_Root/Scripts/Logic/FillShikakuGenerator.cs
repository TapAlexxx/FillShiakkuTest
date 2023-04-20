using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Logic
{
    public class FillShikakuGenerator : MonoBehaviour
    {
        private int xSize;
        private int ySize;
        private int[,] grid;
        public bool IsInvalid { get; private set; }

        public void Generate(int x, int y)
        {
            xSize = x;
            ySize = y;
            grid = new int[x, y];
            ResetGrid(x, y);
            
            int width;
            int height;
            bool generating = true;
            bool needNewNumber = false;

            List<int> numbers = new List<int> {2,3,4,5,6};
            int number = Random.Range(2, 7);
            
            int attempts = 0;
            while (generating)
            {
                generating = ContinueGenerating();
                if(!generating || attempts >= 100)
                    break;

                if (needNewNumber)
                {
                    int num = number;
                    while (num == number)
                    {
                        num = Random.Range(2, 6);
                    }

                    number = num;
                    /*numbers[indexOf >= numbers.Count 
                    ? 0 
                    : indexOf];*/
                    needNewNumber = false;
                }

                for (int i = 0; i < xSize; i++)
                {
                    if (needNewNumber)
                        break;
                    for (int j = 0; j < ySize; j++)
                    {
                        if (grid[i, j] != -1)
                            continue;
                        Debug.Log($"number = {number}, x = {i}, y = {j}");
                        float randomFit = Random.Range(0, 1f);
                        if (number == 2)
                        {
                            width = 2;
                            if (randomFit > 0.5f)
                            {
                                if (TryFitHorizontal(i, j, width, number, ref needNewNumber))
                                    break;
                                if (TryFitVertical(i, j, width, number, ref needNewNumber)) 
                                    break;
                            }
                            else
                            {
                                if (TryFitVertical(i, j, width, number, ref needNewNumber)) 
                                    break;
                                if (TryFitHorizontal(i, j, width, number, ref needNewNumber))
                                    break;
                            }

                            attempts++;
                        }

                        if (number == 3)
                        {
                            width = 3;
                            if (randomFit > 0.5f)
                            {
                                if (TryFitHorizontal(i, j, width, number, ref needNewNumber))
                                    break;
                                if (TryFitVertical(i, j, width, number, ref needNewNumber)) 
                                    break;
                            }
                            else
                            {
                                if (TryFitVertical(i, j, width, number, ref needNewNumber)) 
                                    break;
                                if (TryFitHorizontal(i, j, width, number, ref needNewNumber))
                                    break;
                            }

                            attempts++;
                        }

                        if (number == 4)
                        {
                            width = 2;
                            height = 2;
                            if (CanFitSquare(i, j,width, height))
                            {
                                grid[i, j] = number;
                                grid[i, j + 1] = 0;
                                grid[i + 1, j] = 0;
                                grid[i + 1, j + 1] = 0;
                                needNewNumber = true;
                                break;
                            }

                            attempts++;
                        }

                        if (number == 5)
                        {
                            width = 5;
                            if (randomFit > 0.5f)
                            {
                                if (TryFitHorizontal(i, j, width, number, ref needNewNumber))
                                    break;
                                if (TryFitVertical(i, j, width, number, ref needNewNumber)) 
                                    break;
                            }
                            else
                            {
                                if (TryFitVertical(i, j, width, number, ref needNewNumber)) 
                                    break;
                                if (TryFitHorizontal(i, j, width, number, ref needNewNumber))
                                    break;
                            }

                            attempts++;
                        }

                        if (number == 6)
                        {
                            width = 2;
                            height = 3;
                            if (CanFitSquare(i, j,width, height))
                            {
                                Debug.Log("vertical place");
                                grid[i, j] = number;
                                grid[i, j + 1] = 0;
                                grid[i, j + 2] = 0;
                                
                                grid[i + 1, j] = 0;
                                grid[i + 1, j + 1] = 0;
                                grid[i + 1, j + 2] = 0;
                                needNewNumber = true;
                                break;
                            }

                            if (CanFitSquare(i, j,height, width))
                            {
                                Debug.Log("horizontal place");
                                grid[i, j] = number;
                                grid[i, j + 1] = 0;
                                
                                grid[i + 1, j] = 0;
                                grid[i + 1, j + 1] = 0;
                                
                                grid[i + 2, j] = 0;
                                grid[i + 2, j + 1] = 0;
                                needNewNumber = true;
                                break;
                            }
                            
                            attempts++;
                        }
                        
                        needNewNumber = true;
                    }
                }
            }

            Validate();
        }

        private bool TryFitVertical(int i, int j, int width, int number, ref bool needNewNumber)
        {
            if (CanFitVertical(i, j, width))
            {
                Debug.Log("vertical place");
                FillVerticalLine(i, j, number);
                needNewNumber = true;
                return true;
            }

            return false;
        }

        private bool TryFitHorizontal(int i, int j, int width, int number, ref bool needNewNumber)
        {
            if (CanFitHorizontal(i, j, width))
            {
                Debug.Log("horizontal place");
                FillHorizontalLine(i, j, number);
                needNewNumber = true;
                return true;
            }

            return false;
        }

        private void FillHorizontalLine(int x, int y, int number)
        {
            int randomNumPosition = Random.Range(0, number);
            for (int i = 0; i < number; i++)
            {
                if(i == randomNumPosition)
                    grid[x + i, y] = number;
                else
                    grid[x + i, y] = 0;
            }
        }

        public void FillVerticalLine(int x, int y, int number)
        {
            int randomNumPosition = Random.Range(0, number);
            for (int i = 0; i < number; i++)
            {
                if(i == randomNumPosition)
                    grid[x, y + i] = number;
                else
                    grid[x, y + i] = 0;
            }
        }

        private void Validate()
        {
            IsInvalid = false;
            foreach (int i in grid)
            {
                if (i == -1)
                    IsInvalid = true;
            }
        }

        private bool ContinueGenerating()
        {
            foreach (int i in grid)
            {
                if (i == -1)
                    return true;
            }
            return false;
        }

        private bool CanFitSquare(int x, int y, int width, int height)
        {
            for (int i = 0; i <= width; i++)               
            {                                               
                for (int j = 0; j <= height; j++)            
                {                                            
                    if (i + x >= xSize)                      
                        return false;                        
                    if (j + y >= ySize)                                
                        return false;                        
                                                             
                    int cellValue = GetCell(i + x, j + y);    
                    if (cellValue == -1)                    
                    {                                        
                        continue;                            
                    }                                        
                                                             
                    return false;                            
                }
            }

            return true;
        }

        private bool CanFitVertical(int x, int y, int height)
        {
            if (y + height > ySize)
                return false;
            for (int i = 0; i < height; i++)
            {
                int cellValue = GetCell(x, y + i);
                if (cellValue == -1)                    
                {                                        
                    continue;                            
                }

                return false;
            }

            return true;
        }
        
        private bool CanFitHorizontal(int x, int y, int width)
        {
            if (x + width > xSize)
                return false;
            for (int i = 0; i < width; i++)
            {
                int cellValue = GetCell(x + i, y);
                if (cellValue == -1)                    
                {                                        
                    continue;                            
                }

                return false;
            }

            return true;
        }

        private void ResetGrid(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    grid[i, j] = -1;
                }
            }
        }

        public int GetCell(int x, int y)
        {
            return grid[x, y];
        }
    }
}