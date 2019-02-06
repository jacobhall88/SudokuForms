using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    //object keeps track of the value of all numbers currently on the board using an array

    class BoardState
    {
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
            fixedVals = new bool[9,3,3];
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 3; ++j)
                    for (int k = 0; k < 3; ++k)
                        if (boardVals[i, j, k] > 0) fixedVals[i, j, k] = true;
        }

        //update the state of the board using the coordinates of a single value
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

        //test method
        public void testState()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                        Console.WriteLine(boardVals[i, k, j]);
        }
    }
}
