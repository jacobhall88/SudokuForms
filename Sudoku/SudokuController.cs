using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    class SudokuController
    {
        Form mainView = new Form();
        int numberPicked = 0;
        TableLayoutPanel region0;
        TableLayoutPanel region1;
        TableLayoutPanel region2;
        TableLayoutPanel region3;
        TableLayoutPanel region4;
        TableLayoutPanel region5;
        TableLayoutPanel region6;
        TableLayoutPanel region7;
        TableLayoutPanel region8;

        public SudokuController()
        {
            buildForm(new BoardState());
        }

        public SudokuController(BoardState inputState)
        {
            buildForm(inputState);
        }

        private void buildForm(BoardState buildState)
        {
            mainView.FormBorderStyle = FormBorderStyle.Fixed3D;

            //build the parent table that will hold each of the 3x3 subregions
            TableLayoutPanel regions = buildRegion();

            //build each subregion
            region0 = buildRegion();
            region1 = buildRegion();
            region2 = buildRegion();
            region3 = buildRegion();
            region4 = buildRegion();
            region5 = buildRegion();
            region6 = buildRegion();
            region7 = buildRegion();
            region8 = buildRegion();

            //Fill out each region's value based on the board state that was passed at intitialization
            for (int i = 0; i < 9; ++i)
            {
                switch (i)
                {

                    case 0:
                        //for each row and column in a region, create and add a button showing the corresponding value
                        //from the board state. If the value is 0, display nothing.
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region0.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 1:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region1.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 2:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region2.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 3:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region3.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 4:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region4.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 5:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region5.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 6:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region6.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 7:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region7.Controls.Add(addButton, j, k);
                            }
                        break;
                    case 8:
                        for (int j = 0; j < 3; ++j)
                            for (int k = 0; k < 3; ++k)
                            {
                                Button addButton = new Button();
                                addButton.Dock = DockStyle.Fill;
                                addButton.Click += new EventHandler(ButtonClick);
                                if (buildState.getVals()[i, j, k] > 0)
                                    addButton.Text = buildState.getVals()[i, j, k].ToString();
                                region8.Controls.Add(addButton, j, k);
                            }
                        break;

                }

            }

            //add each subregion to the parent region container
            regions.Controls.Add(region0, 0, 0);
            regions.Controls.Add(region1, 1, 0);
            regions.Controls.Add(region2, 2, 0);
            regions.Controls.Add(region3, 0, 1);
            regions.Controls.Add(region4, 1, 1);
            regions.Controls.Add(region5, 2, 1);
            regions.Controls.Add(region6, 0, 2);
            regions.Controls.Add(region7, 1, 2);
            regions.Controls.Add(region8, 2, 2);


            mainView.Controls.Add(regions);
            mainView.ShowDialog();

        }

        //build a 3x3 table, used for both the parent container and each of the child 3x3 regions
        private TableLayoutPanel buildRegion()
        {
            TableLayoutPanel retTable = new TableLayoutPanel();
            retTable.RowCount = 3;
            retTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            retTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            retTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            retTable.ColumnCount = 3;
            retTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            retTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            retTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            retTable.Dock = DockStyle.Fill;
            return retTable;
        }


        private void ButtonClick(object sender, EventArgs e)
        {
            Button clickedButton;
            clickedButton = (Button)sender;

            Form pickNum = new Form();
            TableLayoutPanel numRegion = buildRegion();
            int i = 1;
            for (int r = 0; r < 3; ++r)
                for (int c = 0; c < 3; ++c)
                {
                    Button addButton = new Button();
                    addButton.Text = i++.ToString();
                    addButton.Click += new EventHandler(PickedButtonClicked);
                    numRegion.Controls.Add(addButton, c, r);
                }

            pickNum.Controls.Add(numRegion);
            pickNum.ShowDialog();
            clickedButton.Text = numberPicked.ToString();
        }

        private void PickedButtonClicked(object sender, EventArgs e)
        {
            Button pickedButton;
            pickedButton = (Button)sender;
            numberPicked = Int32.Parse(pickedButton.Text);
            pickedButton.DialogResult = DialogResult.Cancel;
        }
    }
}
