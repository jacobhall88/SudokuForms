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

        public SudokuButton(int re, int ro, int co, int va)
        {
            region = re; row = ro; col = co; value = va;
        }

        public SudokuButton(int re, int ro, int co)
        {
            region = re; row = ro; col = co;
        }
    }
}
