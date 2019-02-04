using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuBrain
    {
        public SudokuBrain()
        {
        }

        //accepts a 3d array representing the current board state.
        //returns a 3d bool array denoting whether any region, column, or row is invalid (true=invalid)
        //first value of array designates the region, column, or row respectively
        //second value designates which of those is invalid
        public bool[,] validateBoard(int[,,] boardState)
        {
            bool[,] valid = new bool[3,9];
            int[,] rows = makeRows(boardState);
            int[,] cols = makeCols(boardState);

            //compare each value in each row to each other value in that row
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++)
                {
                    //only validate an entry if it has been assigned a value
                    if (rows[i, j] != 0)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            //if two values are not the same entry and are equal, set that entry to invalid
                            if (j != k && rows[i, j] == rows[i, k])
                                valid[2, i] = true;
                        }
                    }
                }
            }

            //compare each value in each column to each other value in that column
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //only validate an entry if it has been assigned a value
                    if (cols[i, j] != 0)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            //if two values are not the same entry and are equal, set that entry to invalid
                            if (j != k && cols[i, j] == cols[i, k])
                                valid[1, i] = true;
                        }
                    }
                }
            }

            //compare each value in each region to each other value in that region
            for (int i = 0; i<10; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    for(int k = 0; k < 3; k++)
                    {
                        //only validate an entry if it has been assigned a value
                        if (boardState[i, j, k] != 0)
                        {
                            for(int l = 0; l < 3; l++)
                            {
                                for(int m = 0; m < 3; m++)
                                {
                                    //if two values are not the same entry and are equal, set that entry to invalid
                                    if (!(l == j && m == k) && boardState[i, j, k] == boardState[i, l, m])
                                        valid[0, i] = true;
                                }
                            }
                        }
                    }
                }
            }

            return valid;
        }

        //returns an array of all the values in each row
        public int[,] makeRows(int[,,] boardState)
        {
            int[,] rows = new int[9, 9];

            return rows;
        }

        //returns an array of all values in each column
        public int[,] makeCols(int[,,] boardState)
        {
            int[,] cols = new int[9, 9];

            return cols;
        }
    }
}
