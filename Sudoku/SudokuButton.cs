using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    class SudokuButton : Button
    {
        public int region;
        public int row;
        public int col;
        public int value;

        public int altRow;
        public int altCol;

        public SudokuButton(int re, int ro, int co, int va)
        {
            region = re; row = ro; col = co; value = va;
            altRow = ro + (region / 3) * 3;
            altCol = co + (region % 3) * 3;
        }

        public SudokuButton(int re, int ro, int co)
        {
            region = re; row = ro; col = co;
        }
    }
}
