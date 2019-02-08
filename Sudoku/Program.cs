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
            testVals[1, 0, 0] = 0;
            testVals[2, 0, 0] = 0;
            testVals[3, 0, 0] = 0;
            testVals[4, 0, 0] = 0;
            testVals[5, 0, 0] = 0;
            testVals[6, 0, 0] = 0;
            testVals[7, 0, 0] = 0;
            testVals[8, 0, 0] = 0;
            BoardState testBoard = new BoardState(testVals);
            new SudokuController(testBoard);
        }

    }
}
