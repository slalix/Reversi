using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ReversiNew
{
    public partial class Form1 : Form
    {
        const int BoardSize = 8;
        const int ButtonSize = 40;

        int[,] MainBoard = new int[BoardSize, BoardSize];

        Button[,] BoardButtons = new Button[BoardSize, BoardSize];

        Stack BoardHistory = new Stack();

        int PlayerNum = 1;
        Button BoardButton;

        int FirstPlayerScore = 0;
        int SecondPlayerScore = 0;

        public Form1()
        {
            InitializeComponent();
            DrawBoard();
            NewGame();
            UpdateBoard();
        }

        private void DrawBoard()
        {
            for (int column = 0; column < BoardSize; column++)
            {
                for (int line = 0; line < BoardSize; line++)
                {
                    BoardButtons[column, line] = new Button();
                    BoardButtons[column, line].Name = "btn" + line + "." + column;
                    BoardButtons[column, line].Size = new Size(ButtonSize, ButtonSize);
                    BoardButtons[column, line].Location = new Point(10 + line * 45, 60 + column * 45);
                    BoardButtons[column, line].Click += new EventHandler(BoardButtonClick);

                    this.Controls.Add(BoardButtons[column, line]);
                }
            }
        }

        private void UpdateBoard()
        {
            string[] btnCoords;
            foreach (Button button in BoardButtons)
            {
                btnCoords = Regex.Split(button.Name.Replace("btn", ""), @"\D+");
                if (MainBoard[Int32.Parse(btnCoords[0]), Int32.Parse(btnCoords[1])] == 0) { button.Text = ""; }
                if (MainBoard[Int32.Parse(btnCoords[0]), Int32.Parse(btnCoords[1])] == 1) { button.Text = "●"; }
                if (MainBoard[Int32.Parse(btnCoords[0]), Int32.Parse(btnCoords[1])] == 2) { button.Text = "○"; }
            }
        }

        private void BoardButtonClick(object sender, EventArgs e)
        {
            string[] btnCoords;
            BoardButton = sender as Button;
            btnCoords = Regex.Split(BoardButton.Name.Replace("btn", ""), @"\D+");

            if (MainBoard[Int32.Parse(btnCoords[0]), Int32.Parse(btnCoords[1])] != 0)
            {
                MessageBox.Show("Клетка занята!");
                return;
            }

            bool changed = false;
            GetMove(Int32.Parse(btnCoords[0]), Int32.Parse(btnCoords[1]), out changed);
            CountScore();
            UpdateBoard();
            CheckGameEnd();

            if (level1ToolStripMenuItem.Checked == true && changed == true)
                AILevel1();
        }

        private void ChangePlayer()
        {
            if (PlayerNum == 1) { PlayerNum = 2; return; }
            if (PlayerNum == 2) { PlayerNum = 1; return; }
        }

        private void CountScore()
        {
            FirstPlayerScore = 0;
            SecondPlayerScore = 0;

            foreach (int Cell in MainBoard)
            {
                if (Cell == 1) FirstPlayerScore++;
                if (Cell == 2) SecondPlayerScore++;
            }

            scorelbl.Text = FirstPlayerScore + ":" + SecondPlayerScore;
        }

        private void GetMoveNew(int x, int y, int direction)
        {
            int[,] MainBoardNew = MainBoard.Clone() as int[,];

            int dx = 0;
            int dy = 0;

            if (direction == 0) { dx = -1; dy = -1; };

            for (int step = 0; step < BoardSize; step++)
            {

            }
        }

        private void GetMove(int x, int y, out bool changed)
        {
            int[,] MainBoardNew = MainBoard.Clone() as int[,];
            changed = false;


            //Right
            for (int next = x - 1; next >= 0; next--)
                if (MainBoardNew[next, y] == 0) break;
                else if (MainBoardNew[next, y] == PlayerNum && next == x - 1) break;

                else if (MainBoardNew[next, y] == PlayerNum)
                    for (int nextchage = x - 1; nextchage >= next; nextchage--)
                        MainBoardNew[nextchage, y] = PlayerNum;

            //Left
            for (int next = x + 1; next <= 7; next++)
                if (MainBoardNew[next, y] == 0) break;
                else if (MainBoardNew[next, y] == PlayerNum && next == x + 1) break;

                else if (MainBoardNew[next, y] == PlayerNum)
                    for (int nextchange = x + 1; nextchange <= next; nextchange++)
                        MainBoardNew[nextchange, y] = PlayerNum;

            //Up
            for (int next = y - 1; next >= 0; next--)
                if (MainBoardNew[x, next] == 0) break;
                else if (MainBoardNew[x, next] == PlayerNum && next == y - 1) break;

                else if (MainBoardNew[x, next] == PlayerNum)
                    for (int nextchange = y - 1; nextchange >= next; nextchange--)
                        MainBoardNew[x, nextchange] = PlayerNum;

            //Down
            for (int next = y + 1; next <= 7; next++)
                if (MainBoardNew[x, next] == 0) break;
                else if (MainBoardNew[x, next] == PlayerNum && next == y + 1) break;

                else if (MainBoardNew[x, next] == PlayerNum)
                    for (int nextchange = y + 1; nextchange <= next; nextchange++)
                        MainBoardNew[x, nextchange] = PlayerNum;


            //Up-Left
            int min = x;
            if (x > y) min = y;
            if (x < y) min = x;

            for (int next = 0; next <= min - 1; next++)
                if (MainBoardNew[x - next - 1, y - next - 1] == 0) break;
                else if (MainBoardNew[x - next - 1, y - next - 1] == PlayerNum && next == 0) break;

                else if (MainBoardNew[x - next - 1, y - next - 1] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x - nextchange, y - nextchange] = PlayerNum;

            //Up-Right
            min = x;
            if (x == 0 && y <= 6) min = y;
            if (x == 1 && y <= 5) min = y;
            if (x == 2 && y <= 4) min = y;
            if (x == 3 && y <= 3) min = y;
            if (x == 4 && y <= 2) min = y;
            if (x == 5 && y <= 1) min = y;
            if (x == 6 && y <= 0) min = y;


            if (y == 1 && x >= 7) min = 7 - x;
            if (y == 2 && x >= 6) min = 7 - x;
            if (y == 3 && x >= 5) min = 7 - x;
            if (y == 4 && x >= 4) min = 7 - x;
            if (y == 5 && x >= 3) min = 7 - x;
            if (y == 6 && x >= 2) min = 7 - x;
            if (y == 7 && x >= 1) min = 7 - x;

            if (x == 0 && y == 7) min = 7;
            if (x == 1 && y == 6) min = 6;
            if (x == 2 && y == 5) min = 5;
            if (x == 3 && y == 4) min = 4;
            if (x == 4 && y == 3) min = 3;
            if (x == 5 && y == 2) min = 2;
            if (x == 6 && y == 1) min = 1;
            if (x == 7 && y == 0) min = 0;

            for (int next = 1; next <= min; next++)
                if (MainBoardNew[x + next, y - next] == 0) break;
                else if (MainBoardNew[x + next, y - next] == PlayerNum && next == 1) break;

                else if (MainBoardNew[x + next, y - next] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x + nextchange, y - nextchange] = PlayerNum;

            //Down-Left
            min = x;
            if (x == 6 && y <= 0) min = x;
            if (x == 5 && y <= 1) min = x;
            if (x == 4 && y <= 2) min = x;
            if (x == 3 && y <= 3) min = x;
            if (x == 2 && y <= 4) min = x;
            if (x == 1 && y <= 5) min = x;
            if (x == 0 && y <= 6) min = x;


            if (y == 1 && x >= 7) min = 7 - y;
            if (y == 2 && x >= 6) min = 7 - y;
            if (y == 3 && x >= 5) min = 7 - y;
            if (y == 4 && x >= 4) min = 7 - y;
            if (y == 5 && x >= 3) min = 7 - y;
            if (y == 6 && x >= 2) min = 7 - y;
            if (y == 7 && x >= 1) min = 7 - y;

            if (x == 0 && y == 7) min = 0;
            if (x == 1 && y == 6) min = 1;
            if (x == 2 && y == 5) min = 2;
            if (x == 3 && y == 4) min = 3;
            if (x == 4 && y == 3) min = 4;
            if (x == 5 && y == 2) min = 5;
            if (x == 6 && y == 1) min = 6;
            if (x == 7 && y == 0) min = 7;

            for (int next = 1; next <= min; next++)
                if (MainBoardNew[x - next, y + next] == 0) break;
                else if (MainBoardNew[x - next, y + next] == PlayerNum && next == 1) break;

                else if (MainBoardNew[x - next, y + next] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x - nextchange, y + nextchange] = PlayerNum;


            //Down-Right
            min = 7 - x;
            if (x > y) min = 7 - x;
            if (x < y) min = 7 - y;
            for (int next = 1; next <= min; next++)
                if (MainBoardNew[x + next, y + next] == 0) break;
                else if (MainBoardNew[x + next, y + next] == PlayerNum && next == 1) break;

                else if (MainBoardNew[x + next, y + next] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x + nextchange, y + nextchange] = PlayerNum;

            if (CompareBoards(MainBoard, MainBoardNew))
            {
                MainBoardNew[x, y] = PlayerNum;
                MainBoard = MainBoardNew;
                ChangePlayer();
                BoardHistory.Push(MainBoard);
                changed = true;
            }
            

        }


        int[,] MainBoardNew = new int[8, 8];
        private int ScoreCount(int x, int y)
        {
            Array.Clear(MainBoardNew, 0, MainBoardNew.Length);
            MainBoardNew = MainBoard.Clone() as int[,];


            //Right
            for (int next = x - 1; next >= 0; next--)
                if (MainBoardNew[next, y] == 0) break;
                else if (MainBoardNew[next, y] == PlayerNum && next == x - 1) break;

                else if (MainBoardNew[next, y] == PlayerNum)
                    for (int nextchage = x - 1; nextchage >= next; nextchage--)
                        MainBoardNew[nextchage, y] = PlayerNum;

            //Left
            for (int next = x + 1; next <= 7; next++)
                if (MainBoardNew[next, y] == 0) break;
                else if (MainBoardNew[next, y] == PlayerNum && next == x + 1) break;

                else if (MainBoardNew[next, y] == PlayerNum)
                    for (int nextchange = x + 1; nextchange <= next; nextchange++)
                        MainBoardNew[nextchange, y] = PlayerNum;

            //Up
            for (int next = y - 1; next >= 0; next--)
                if (MainBoardNew[x, next] == 0) break;
                else if (MainBoardNew[x, next] == PlayerNum && next == y - 1) break;

                else if (MainBoardNew[x, next] == PlayerNum)
                    for (int nextchange = y - 1; nextchange >= next; nextchange--)
                        MainBoardNew[x, nextchange] = PlayerNum;

            //Down
            for (int next = y + 1; next <= 7; next++)
                if (MainBoardNew[x, next] == 0) break;
                else if (MainBoardNew[x, next] == PlayerNum && next == y + 1) break;

                else if (MainBoardNew[x, next] == PlayerNum)
                    for (int nextchange = y + 1; nextchange <= next; nextchange++)
                        MainBoardNew[x, nextchange] = PlayerNum;


            //Up-Left
            int min = x;
            if (x > y) min = y;
            if (x < y) min = x;

            for (int next = 0; next <= min - 1; next++)
                if (MainBoardNew[x - next - 1, y - next - 1] == 0) break;
                else if (MainBoardNew[x - next - 1, y - next - 1] == PlayerNum && next == 0) break;

                else if (MainBoardNew[x - next - 1, y - next - 1] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x - nextchange, y - nextchange] = PlayerNum;

            //Up-Right
            min = x;
            if (x == 0 && y <= 6) min = y;
            if (x == 1 && y <= 5) min = y;
            if (x == 2 && y <= 4) min = y;
            if (x == 3 && y <= 3) min = y;
            if (x == 4 && y <= 2) min = y;
            if (x == 5 && y <= 1) min = y;
            if (x == 6 && y <= 0) min = y;


            if (y == 1 && x >= 7) min = 7 - x;
            if (y == 2 && x >= 6) min = 7 - x;
            if (y == 3 && x >= 5) min = 7 - x;
            if (y == 4 && x >= 4) min = 7 - x;
            if (y == 5 && x >= 3) min = 7 - x;
            if (y == 6 && x >= 2) min = 7 - x;
            if (y == 7 && x >= 1) min = 7 - x;

            if (x == 0 && y == 7) min = 7;
            if (x == 1 && y == 6) min = 6;
            if (x == 2 && y == 5) min = 5;
            if (x == 3 && y == 4) min = 4;
            if (x == 4 && y == 3) min = 3;
            if (x == 5 && y == 2) min = 2;
            if (x == 6 && y == 1) min = 1;
            if (x == 7 && y == 0) min = 0;

            for (int next = 1; next <= min; next++)
                if (MainBoardNew[x + next, y - next] == 0) break;
                else if (MainBoardNew[x + next, y - next] == PlayerNum && next == 1) break;

                else if (MainBoardNew[x + next, y - next] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x + nextchange, y - nextchange] = PlayerNum;

            //Down-Left
            min = x;
            if (x == 6 && y <= 0) min = x;
            if (x == 5 && y <= 1) min = x;
            if (x == 4 && y <= 2) min = x;
            if (x == 3 && y <= 3) min = x;
            if (x == 2 && y <= 4) min = x;
            if (x == 1 && y <= 5) min = x;
            if (x == 0 && y <= 6) min = x;


            if (y == 1 && x >= 7) min = 7 - y;
            if (y == 2 && x >= 6) min = 7 - y;
            if (y == 3 && x >= 5) min = 7 - y;
            if (y == 4 && x >= 4) min = 7 - y;
            if (y == 5 && x >= 3) min = 7 - y;
            if (y == 6 && x >= 2) min = 7 - y;
            if (y == 7 && x >= 1) min = 7 - y;

            if (x == 0 && y == 7) min = 0;
            if (x == 1 && y == 6) min = 1;
            if (x == 2 && y == 5) min = 2;
            if (x == 3 && y == 4) min = 3;
            if (x == 4 && y == 3) min = 4;
            if (x == 5 && y == 2) min = 5;
            if (x == 6 && y == 1) min = 6;
            if (x == 7 && y == 0) min = 7;

            for (int next = 1; next <= min; next++)
                if (MainBoardNew[x - next, y + next] == 0) break;
                else if (MainBoardNew[x - next, y + next] == PlayerNum && next == 1) break;

                else if (MainBoardNew[x - next, y + next] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x - nextchange, y + nextchange] = PlayerNum;


            //Down-Right
            min = 7 - x;
            if (x > y) min = 7 - x;
            if (x < y) min = 7 - y;
            for (int next = 1; next <= min; next++)
                if (MainBoardNew[x + next, y + next] == 0) break;
                else if (MainBoardNew[x + next, y + next] == PlayerNum && next == 1) break;

                else if (MainBoardNew[x + next, y + next] == PlayerNum)
                    for (int nextchange = 1; nextchange <= next; nextchange++)
                        MainBoardNew[x + nextchange, y + nextchange] = PlayerNum;


            return CountDifferences(MainBoard, MainBoardNew);
        }

        private void CheckGameEnd()
        {
            if (CheckPlayerScoreIsZero() || CheckIfBoardIsFull())
            {

                foreach (Button Boardbtn in BoardButtons)
                    Boardbtn.Enabled = false;

                if (FirstPlayerScore > SecondPlayerScore)
                    MessageBox.Show("Игра окончена!\nПобедили черные!");
                else if (FirstPlayerScore == SecondPlayerScore)
                    MessageBox.Show("Игра окончена!\nНичья!");
                else
                    MessageBox.Show("Игра окончена!\nПобедили белые!");
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void пропуститьХодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePlayer();
        }

        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void NewGame()
        {
            for (int line = 0; line < BoardSize; line++)
                for (int column = 0; column < BoardSize; column++)
                    MainBoard[line, column] = 0;

            MainBoard[3, 3] = 2;
            MainBoard[4, 4] = 2;
            MainBoard[4, 3] = 1;
            MainBoard[3, 4] = 1;

            PlayerNum = 1;
            FirstPlayerScore = 0;
            SecondPlayerScore = 0;

            BoardHistory.Clear();
            BoardHistory.Push(MainBoard);
            CountScore();
            UpdateBoard();


            foreach (Button Boardbtn in BoardButtons)
                Boardbtn.Enabled = true;
        }

        private bool CheckPlayerScoreIsZero()
        {
            if (FirstPlayerScore == 0 || SecondPlayerScore == 0)
                return true;

            return false;
        }

        private bool CheckIfBoardIsFull()
        {
            foreach (int cell in MainBoard)
                if (cell == 0)
                    return false;

            return true;
        }

        private bool CompareBoards(int[,] MainBoard, int[,] MainBoardNew)
        {
            for (int line = 0; line < BoardSize; line++)
                for (int column = 0; column < BoardSize; column++)
                    if (MainBoardNew[line, column] != MainBoard[line, column])
                        return true;

            return false;
        }

        private int CountDifferences(int[,] MainBoard, int[,] MainBoardNew)
        {
            int DifferencesCount = 0;
            for (int line = 0; line < BoardSize; line++)
                for (int column = 0; column < BoardSize; column++)
                    if (MainBoardNew[line, column] != MainBoard[line, column])
                        DifferencesCount++;

            return DifferencesCount;
        }

        private void предыдущийХодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BoardHistory.Count == 0) return;

            MainBoard = BoardHistory.Pop() as int[,];
            CountScore();
            UpdateBoard();
            CheckGameEnd();
        }


        List<Tuple<int, int, int>> StepList = new List<Tuple<int, int, int>>();
        private void AILevel1()
        {
            StepList.Clear();

            for (int line = 0; line < BoardSize; line++)
                for (int column = 0; column < BoardSize; column++)
                    if (MainBoard[column, line] == 0)
                    {
                        int StepScore = ScoreCount(column, line);
                        if (StepScore > 0)
                            StepList.Add(new Tuple<int, int, int>(column, line, StepScore));
                    }

            if (StepList.Count == 0) return;

            StepList.Sort((x, y) => y.Item3.CompareTo(x.Item3));

            List<int> SameScoreStep = new List<int>();

            for (int step = 0; step < StepList.Count; step++)
                if (StepList[step].Item3 == StepList[0].Item3)
                    SameScoreStep.Add(step);
                else break;

            Random r = new Random(DateTime.Now.Millisecond);
            int RandomStep = r.Next(0, SameScoreStep.Count());
            bool changed;
            GetMove(StepList[RandomStep].Item1, StepList[RandomStep].Item2, out changed);


            CountScore();
            UpdateBoard();
            CheckGameEnd();
        }

        private void level1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (level1ToolStripMenuItem.Checked == true)
                level1ToolStripMenuItem.Checked = false;
            else
                level1ToolStripMenuItem.Checked = true;
        }
    }
}
