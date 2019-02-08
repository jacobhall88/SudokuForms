using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Sudoku
{
    class SudokuController
    {
        //temporarily holds the number picked to update a value, used to pass number from the event handler back to the parent Control
        private int numberPicked = 0;

        //object keeps track of all fixed and mutable values currentl y on the board
        private BoardState state;

        //list of all buttons, used by the validating methods to set the color of invalid entries
        private List<SudokuButton> buttons = new List<SudokuButton>();

        private bool newGame = true;

        //threshold the controls tendency for entries on a random board to be immutable
        //higher number = fewer entries. lower than 65 will make it difficult to generate a valid board
        const int RANDO = 65;

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

        //construct the primary interface
        private void buildForm(BoardState buildState)
        {
            Form mainView = new Form();
            mainView.FormBorderStyle = FormBorderStyle.Fixed3D;
            mainView.Size = new Size(500, 600);
            List<TableLayoutPanel> subRegions = new List<TableLayoutPanel>();

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
                        SudokuButton addButton = new SudokuButton(i, j, k, buildState.getVals()[i, j, k]);
                        addButton.Dock = DockStyle.Fill;
                        if (!buildState.getFixed()[i, k, j])
                        {
                            addButton.Click += new EventHandler(ButtonClick);
                            addButton.BackColor = Color.White;
                        }
                        else
                        {
                            addButton.ForeColor = Color.White;
                            addButton.BackColor = Color.Black;
                        }

                        if (buildState.getVals()[i, k, j] > 0)
                            addButton.Text = buildState.getVals()[i, k, j].ToString();
                        subRegions[i].Controls.Add(addButton, k, j);
                        buttons.Add(addButton);
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

            //create parent container to hold parent region and menu bar
            TableLayoutPanel parent = new TableLayoutPanel();
            parent.Dock = DockStyle.Fill;
            parent.ColumnCount = 1;
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            parent.RowCount = 2;
            parent.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            parent.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            //create the menu bar, then add the menu and the parent region
            parent.Controls.Add(buildMenu());
            parent.Controls.Add(regions);
            mainView.Controls.Add(parent);

            NewGame(false);
            //display constructed board
            mainView.ShowDialog();

        }

        //build a 3x3 table, used for both the parent region and each of the child 3x3 regions
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

        //colors each entry depending on whether or not it is a valid value
        private void colorValid(bool[,] valid)
        {
            Console.WriteLine("Trying to validate");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(valid[i, j]);
                Console.WriteLine();
            }
            state.testState();
            //if an entry is colored Red but is not invalid, set ForeColor to Black or White
            foreach (SudokuButton updateButton in buttons)
            {
                if (updateButton.ForeColor == Color.FromName("Red"))
                {
                    //sets black-backgrounded immutable values to white, and white-backgrounded mutable entries to black
                    if (!valid[0, updateButton.region] && !valid[1, updateButton.altRow] && !valid[2, updateButton.altCol])
                    {
                        if (updateButton.BackColor == Color.Black)
                            updateButton.ForeColor = Color.FromName("White");
                        else
                            updateButton.ForeColor = Color.FromName("Black");
                    }
                }
            }
            //if an entry is in an invalid region, row, or column, set ForeColor to Red
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (valid[i, j])
                    {
                        foreach (SudokuButton updateButton in buttons)
                        {
                            switch (i)
                            {
                                case 0:
                                    if (updateButton.region == j)
                                        updateButton.ForeColor = Color.FromName("Red");
                                    break;
                                case 1:
                                    if (updateButton.altRow == j)
                                        updateButton.ForeColor = Color.FromName("Red");
                                    break;
                                case 2:
                                    if (updateButton.altCol == j)
                                        updateButton.ForeColor = Color.FromName("Red");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        //check if the board is complete and valid
        private void victoryCheck()
        {
            bool checkFull = true;
            bool checkValid = true;
            foreach (SudokuButton victoryButton in buttons)
            {
                if (victoryButton.value == 0)
                {
                    checkFull = false;
                    break;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (state.validateBoard()[i, j])
                    {
                        checkValid = false;
                        i = 3;
                        break;
                    }
                }

            }

            if (checkFull && checkValid)
            {
                newGame = true;
                NewGame(true);
            }
        }

        //create and return the menu bar
        private TableLayoutPanel buildMenu()
        {
            TableLayoutPanel menu = new TableLayoutPanel();
            menu.ColumnCount = 5;
            menu.Dock = DockStyle.Fill;
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            Button open = new Button();
            open.Dock = DockStyle.Fill;
            open.Text = "Open";
            open.Click += new EventHandler(OpenClick);
            menu.Controls.Add(open);

            Button save = new Button();
            save.Dock = DockStyle.Fill;
            save.Text = "Save";
            save.Click += new EventHandler(SaveClick);
            menu.Controls.Add(save);

            Button newBoard = new Button();
            newBoard.Dock = DockStyle.Fill;
            newBoard.Text = "New";
            newBoard.Click += new EventHandler(NewClick);
            menu.Controls.Add(newBoard);

            Button imprint = new Button();
            imprint.Dock = DockStyle.Fill;
            imprint.Text = "Imprint";
            imprint.Click += new EventHandler(ImprintClick);
            menu.Controls.Add(imprint);

            Button solve = new Button();
            solve.Dock = DockStyle.Fill;
            solve.Text = "Solve";
            solve.Click += new EventHandler(SolveClick);
            menu.Controls.Add(solve);

            return menu;
        }

        //display completed/new game dialogue
        private void NewGame(bool checkVictory)
        {
            Form victoryForm = new Form();
            TableLayoutPanel parent = new TableLayoutPanel();
            parent.Dock = DockStyle.Fill;
            parent.ColumnCount = 1;
            parent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            parent.RowCount = 4;
            parent.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            parent.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            parent.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            parent.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            Button message = new Button();
            message.Dock = DockStyle.Fill;
            if (checkVictory)
                message.Text = "Congratulations! Board Complete";
            else
                message.Text = "Welcome to Sudoku!";
            parent.Controls.Add(message);

            Button open = new Button();
            open.Dock = DockStyle.Fill;
            open.Text = "Open Game";
            open.Click += new EventHandler(OpenClick);
            parent.Controls.Add(open);

            Button newBoard = new Button();
            newBoard.Dock = DockStyle.Fill;
            newBoard.Text = "New Blank Board";
            newBoard.Click += new EventHandler(NewClick);
            parent.Controls.Add(newBoard);

            Button randoBoard = new Button();
            randoBoard.Dock = DockStyle.Fill;
            randoBoard.Text = "New Random Board";
            randoBoard.Click += new EventHandler(RandomizeClick);
            parent.Controls.Add(randoBoard);

            victoryForm.Controls.Add(parent);
            victoryForm.ShowDialog();
        }

        //event handler for when a button is clicked on from the main view. opens a new dialogue to pick a value for that button
        private void ButtonClick(object sender, EventArgs e)
        {
            //initialize variables
            SudokuButton clickedButton;
            clickedButton = (SudokuButton)sender;
            int[,,] currentState = state.getVals();
            numberPicked = 0;

            //create a new dialogue with a 3x3 grid of numbers to pick for the new value
            Form pickNum = new Form();
            TableLayoutPanel numRegion = buildRegion();
            int buttonNumber = 1;
            for (int r = 0; r < 3; ++r)
                for (int c = 0; c < 3; ++c)
                {
                    SudokuButton addButton = new SudokuButton(0, 0, 0);
                    addButton.Text = buttonNumber++.ToString();
                    addButton.Click += new EventHandler(PickedButtonClicked);
                    numRegion.Controls.Add(addButton, c, r);
                }

            pickNum.Controls.Add(numRegion);
            pickNum.ShowDialog();


            //update the button clicked with the text of the new number chosen
            if (numberPicked != 0)
            {
                clickedButton.Text = numberPicked.ToString();
                clickedButton.value = numberPicked;
            }

            //update the board state with the new value and validate the new board state
            currentState[clickedButton.region, clickedButton.col, clickedButton.row] = numberPicked;
            state.updateState(currentState);
            bool[,] validation = state.validateBoard();
            colorValid(validation);
            victoryCheck();

        }

        //event handler for when a new value is selected for an entry
        //TODO: fix having to double click
        private void PickedButtonClicked(object sender, EventArgs e)
        {
            //initialize variables
            SudokuButton pickedButton;
            pickedButton = (SudokuButton)sender;

            //hold the number chosen in the numberPicked variable for the ButtonClick listener to refer to
            numberPicked = Int32.Parse(pickedButton.Text);
            Form parent = (Form)pickedButton.Parent.Parent;
            parent.Close();
        }

        //event handler for when 'Open Board' button is clicked
        private void OpenClick(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Sudoku File | *.sdk";

            if (open.ShowDialog() == DialogResult.OK)
            {
                state.updateFromFile(open.OpenFile());

                //update appearance of each button per the new board state
                foreach (SudokuButton openButton in buttons)
                {
                    openButton.value = state.getVals()[openButton.region, openButton.col, openButton.row];
                    if (openButton.value != 0)
                        openButton.Text = state.getVals()[openButton.region, openButton.col, openButton.row].ToString();
                    else
                        openButton.Text = "";

                    //convert button from mutable to immutable if necessary
                    if (openButton.BackColor == Color.Black && !state.getFixed()[openButton.region, openButton.row, openButton.col])
                    {
                        openButton.BackColor = Color.White;
                        openButton.ForeColor = Color.Black;
                        openButton.Click += new EventHandler(ButtonClick);
                    }
                    if (openButton.BackColor == Color.White && state.getFixed()[openButton.region, openButton.row, openButton.col] && openButton.value != 0)
                    {
                        openButton.BackColor = Color.Black;
                        openButton.ForeColor = Color.White;
                        openButton.Click -= ButtonClick;
                    }
                }
                colorValid(state.validateBoard());

            }
            if (newGame)
            {
                Button clicked = (Button)sender;
                Form parent = (Form)clicked.Parent.Parent;
                newGame = false;
                parent.Close();
            }
        }

        //event handler for when 'Save Board' button is clicked
        private void SaveClick(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Sudoku File | *.sdk";

            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            writer.Write(state.getVals()[i, j, k]);
                        }
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            if (state.getFixed()[i, j, k])
                                writer.Write(1);
                            else
                                writer.Write(0);
                        }
                    }
                }
                writer.Dispose();
                writer.Close();
            }
        }

        private void NewClick(object sender, EventArgs e)
        {

            foreach (SudokuButton newbutton in buttons)
            {
                newbutton.value = 0;
                newbutton.Text = "";
                if (newbutton.BackColor == Color.Black)
                    newbutton.Click += ButtonClick;
                newbutton.BackColor = Color.White;
                newbutton.ForeColor = Color.Black;
            }
            if (newGame)
            {
                Button clicked = (Button)sender;
                Form parent = (Form)clicked.Parent.Parent;
                newGame = false;
                parent.Close();
            }
            state = new BoardState();
        }

        //event handler for when 'Imprint' is clicked
        //makes all values currently on the board immutable, if board state is valid
        private void ImprintClick(object sender, EventArgs e)
        {
            foreach (SudokuButton imprintButton in buttons)
            {
                bool check = true;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (state.validateBoard()[i, j])
                        {
                            check = false;
                            i = 3;
                            break;
                        }
                    }

                }
                if (imprintButton.value != 0 && imprintButton.BackColor != Color.Black && check)
                {
                    imprintButton.BackColor = Color.Black;
                    imprintButton.ForeColor = Color.White;
                    imprintButton.Click -= ButtonClick;
                    state.setFixed();
                }
            }

        }

        //event handler for when 'Randomize' is clicked
        private void RandomizeClick(object sender, EventArgs e)
        {
            Random rando = new Random();
            bool validate = true;

            int[,,] newState = new int[9, 3, 3];
            do
            {
                newState = new int[9, 3, 3];
                //clear the board
                foreach (SudokuButton newbutton in buttons)
                {
                    newbutton.value = 0;
                    newbutton.Text = "";
                    if (newbutton.BackColor == Color.Black)
                        newbutton.Click += ButtonClick;
                    newbutton.BackColor = Color.White;
                    newbutton.ForeColor = Color.Black;
                }
                if (newGame)
                {
                    Button clicked = (Button)sender;
                    Form parent = (Form)clicked.Parent.Parent;
                    newGame = false;
                    parent.Close();
                }
                state = new BoardState();

                validate = true;
                foreach (SudokuButton randoButton in buttons)
                {
                    if (rando.Next(0, 99) > RANDO)
                    {
                        int newValue = rando.Next(1, 9);
                        randoButton.value = newValue;
                        randoButton.Text = newValue.ToString();
                        randoButton.ForeColor = Color.White;
                        randoButton.BackColor = Color.Black;
                        randoButton.Click -= ButtonClick;
                        newState[randoButton.region, randoButton.col, randoButton.row] = newValue;
                    }

                }
                state.updateState(newState);
                state.setFixed();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (state.validateBoard()[i, j])
                        {
                            validate = false;
                            i = 3;
                            break;
                        }
                    }
                }

            } while (!validate);
            if (newGame)
            {
                Button clicked = (Button)sender;
                Form parent = (Form)clicked.Parent.Parent;
                newGame = false;
                parent.Close();
            }
        }

        private void SolveClick(object sender, EventArgs e)
        {
            state.Solve();

            foreach (SudokuButton solveButton in buttons)
            {
                solveButton.Text = state.getVals()[solveButton.region, solveButton.col, solveButton.row].ToString();
            }
        }

    }
}
