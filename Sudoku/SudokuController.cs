﻿using System;
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
        private Form mainView = new Form();
        private int numberPicked = 0;
        private List<TableLayoutPanel> subRegions= new List<TableLayoutPanel>();
        private BoardState state;

        public SudokuController()
        {
            state = new BoardState();
            buildForm(state);
        }

        public SudokuController(BoardState inputState)
        {
            state = inputState;
            buildForm(inputState);
        }

        private void buildForm(BoardState buildState)
        {
            mainView.FormBorderStyle = FormBorderStyle.Fixed3D;

            //build the parent table that will hold each of the 3x3 subregions
            TableLayoutPanel regions = buildRegion();

            //build each subregion
            for (int i = 0; i < 10; i++)
                subRegions.Add(buildRegion());


            //Fill out each region's value based on the board state that was passed at intitialization
            for (int i = 0; i < 9; ++i)
            {
              
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
                        subRegions[i].Controls.Add(addButton, j, k);
                    }
  
            }


            //add each subregion to the parent region container
            regions.Controls.Add(subRegions[0], 0, 0);
            regions.Controls.Add(subRegions[1], 1, 0);
            regions.Controls.Add(subRegions[2], 2, 0);
            regions.Controls.Add(subRegions[3], 0, 1);
            regions.Controls.Add(subRegions[4], 1, 1);
            regions.Controls.Add(subRegions[5], 2, 1);
            regions.Controls.Add(subRegions[6], 0, 2);
            regions.Controls.Add(subRegions[7], 1, 2);
            regions.Controls.Add(subRegions[8], 2, 2);


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
