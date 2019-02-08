using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sudoku
{
    //object keeps track of the value of all numbers currently on the board using an array

    class BoardState
    {
        int testcount = 1;
        //3d array of ints representing all values on the board, represted as region, column, row
        int[,,] boardVals;

        //3d array of bools representing if a number is immutable (true cannot be changed, false can be)
        bool[,,] fixedVals;

        //create a blank board if no initial values are provided
        public BoardState()
        {
            boardVals = new int[9, 3, 3];
            fixedVals = new bool[9, 3, 3];
        }

        //create a board with the listed initial values as immutable
        public BoardState(int[,,] inputVals)
        {
            boardVals = inputVals;
            fixedVals = new bool[9, 3, 3];
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 3; ++j)
                    for (int k = 0; k < 3; ++k)
                        if (boardVals[i, j, k] > 0) fixedVals[i, j, k] = true;
        }

        //update the state of the board using the coordinates of a single value
        //**note** method has not been implemented and tested yet
        public void updateState(int region, int column, int row, int val)
        {
            if (!fixedVals[region, column, row])
            {
                boardVals[region, column, row] = val;
            }
        }

        //update the state of the board to match the state of another board
        public void updateState(int[,,] newState)
        {
            boardVals = newState;
        }

        public int[,,] getVals()
        {
            return boardVals;
        }

        public bool[,,] getFixed()
        {
            return fixedVals;
        }

        //set all non-zero values to be immutable
        public void setFixed()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                    {
                        if (boardVals[i, j, k] != 0)
                            fixedVals[i, j, k] = true;
                        else
                            fixedVals[i, j, k] = false;
                    }
        }

        //test method
        public void testState()
        {
            int[,] rows = makeRows(getVals());
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(rows[i, j]);
                Console.WriteLine();
            }
        }

        //accepts a 3d array representing the current board state.
        //returns a 3d bool array denoting whether any region, row, or column is invalid (true=invalid)
        //first value of array designates the region, row, or column respectively
        //second value designates which of those is invalid
        public bool[,] validateBoard()
        {
            bool[,] valid = new bool[3, 9];
            int[,] rows = makeRows(boardVals);
            int[,] cols = makeCols(boardVals);

            //compare each value in each row to each other value in that row
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    //only validate an entry if it has been assigned a value
                    if (rows[i, j] != 0)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            //if two values are not the same entry and are equal, set that entry to invalid
                            if (j != k && rows[i, j] == rows[i, k])
                                valid[1, i] = true;
                        }
                    }
                }
            }

            //compare each value in each column to each other value in that column
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    //only validate an entry if it has been assigned a value
                    if (cols[i, j] != 0)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            //if two values are not the same entry and are equal, set that entry to invalid
                            if (j != k && cols[i, j] == cols[i, k])
                                valid[2, i] = true;
                        }
                    }
                }
            }

            //compare each value in each region to each other value in that region
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        //only validate an entry if it has been assigned a value
                        if (boardVals[i, j, k] != 0)
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                for (int m = 0; m < 3; m++)
                                {
                                    //if two values are not the same entry and are equal, set that entry to invalid
                                    if (!(l == j && m == k) && boardVals[i, j, k] == boardVals[i, l, m])
                                        valid[0, i] = true;
                                }
                            }
                        }
                    }
                }
            }

            return valid;
        }

        public bool isValid()
        {
            bool retFlag = true;
            bool[,] valid = new bool[3, 9];
            int[,] rows = makeRows(boardVals);
            int[,] cols = makeCols(boardVals);

            //compare each value in each row to each other value in that row
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    //only validate an entry if it has been assigned a value
                    if (rows[i, j] != 0)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            //if two values are not the same entry and are equal, set that entry to invalid
                            if (j != k && rows[i, j] == rows[i, k])
                            {
                                retFlag = false;
                                i = 10;
                                j = 10;
                                break;
                            }

                        }
                    }
                }
            }

            if (retFlag)
            {
                //compare each value in each column to each other value in that column
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {

                        //only validate an entry if it has been assigned a value
                        if (cols[i, j] != 0)
                        {
                            for (int k = 0; k < 9; k++)
                            {
                                //if two values are not the same entry and are equal, set that entry to invalid
                                if (j != k && cols[i, j] == cols[i, k])
                                {
                                    retFlag = false;
                                    i = 10;
                                    j = 10;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (retFlag)
            {
                //compare each value in each region to each other value in that region
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            //only validate an entry if it has been assigned a value
                            if (boardVals[i, j, k] != 0)
                            {
                                for (int l = 0; l < 3; l++)
                                {
                                    for (int m = 0; m < 3; m++)
                                    {
                                        //if two values are not the same entry and are equal, set that entry to invalid
                                        if (!(l == j && m == k) && boardVals[i, j, k] == boardVals[i, l, m])
                                        {
                                            retFlag = false;
                                            i = 10;
                                            j = 4;
                                            k = 4;
                                            l = 4;
                                            m = 4;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return retFlag;

        }

        //returns an array of all the values in each row
        public int[,] makeRows(int[,,] boardState)
        {
            int[,] rows = new int[9, 9];

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    rows[i, j] = boardState[j / 3 + ((i / 3) * 3), j % 3, i - ((i / 3) * 3)];

            return rows;
        }

        //returns an array of all values in each column
        public int[,] makeCols(int[,,] boardState)
        {
            int[,] cols = new int[9, 9];

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    cols[i, j] = boardState[((j / 3) * 3) + (i / 3), i - ((i / 3) * 3), j % 3];

            return cols;
        }

        //update the board state to a state read in from a file
        public void updateFromFile(Stream update)
        {
            int[,,] newVals = new int[9, 3, 3];
            int[,,] newFixed = new int[9, 3, 3];

            StreamReader file = new StreamReader(update);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        boardVals[i, j, k] = (int)char.GetNumericValue((char)file.Read());
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        fixedVals[i, j, k] = (0 != ((int)char.GetNumericValue((char)file.Read())));
                    }
                }
            }

            file.Close();
        }

        public void Solve()
        {
            int entry = 0;
            int val = 1;
            bool valid;

            do
            {
                testcount++;
                Console.WriteLine("At entry " + entry);
                if (val == 10)
                {
                    Console.WriteLine("That Shouldn't Happen");
                    Console.WriteLine("Broke on loop " + testcount);
                    break;
                }
                //creating reference coordinates based on entry value
                int reg = entry / 9;
                int col = (entry - (reg * 9)) % 3;
                int row = (entry - (reg * 9)) / 3;


                //if current entry being checked is a fixed value, go the next value
                if (!fixedVals[reg, row, col])
                {
                    //assign entry the value to be tested
                    boardVals[reg, row, col] = val;
                    valid = isValid();

                    //if the value is valid, go to the next entry
                    if (valid)
                    {
                        val = 1;
                        entry++;
                    }

                    //if the value in invalid and all 9 values have been tried,
                    //zero out the entry and go back to the previous non-fixed entry
                    else if (!valid && val == 9)
                    {
                        do
                        {
                            if (!fixedVals[reg, row, col])
                                boardVals[reg, row, col] = 0;

                            --entry;
                            reg = entry / 9;
                            col = (entry - (reg * 9)) % 3;
                            row = (entry - (reg * 9)) / 3;

                        } while (fixedVals[reg, row, col] || boardVals[reg, row, col] == 9);
                        val = boardVals[reg, row, col] + 1;
                    }
                    //if the value is invlid and all 9 values have not been tried,
                    //increment value and call again at same entry
                    else if (!valid && val < 9)
                    {
                        ++val;
                    }
                }
                else ++entry;

            } while (entry != 81);

            Console.Write("Solution took " + testcount + " tries");

        }

    }
}