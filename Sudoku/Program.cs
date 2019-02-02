using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,,] testVals = new int[9, 3, 3];
            testVals[0, 0, 0] = 0;
            testVals[1, 0, 0] = 1;
            testVals[2, 0, 0] = 2;
            testVals[3, 0, 0] = 3;
            testVals[4, 0, 0] = 4;
            testVals[5, 0, 0] = 5;
            testVals[6, 0, 0] = 6;
            testVals[7, 0, 0] = 7;
            testVals[8, 0, 0] = 8;
            BoardState testBoard = new BoardState(testVals);
            new SudokuController(testBoard);
        }

    }
}
