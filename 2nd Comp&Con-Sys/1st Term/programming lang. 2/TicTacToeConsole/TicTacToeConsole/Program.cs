using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeConsole
{
    class Program
    {
        public static int Winner(int[] board)
        {
            UInt32[,] WinPos = new UInt32[8, 3] 
            {
                {0, 1, 2}, {3, 4, 5}, {6, 7, 8},    // Horizontal
                {0, 3, 6}, {1, 4, 7}, {2, 5, 8},    // Vertical
                {0, 4, 8}, {2, 4, 6}                // Diagonal
            };

            for (int i = 0; i < 8; i++)
            {
                if (board[WinPos[i, 0]] != 0 &&
                    board[WinPos[i, 0]] == board[WinPos[i, 1]] &&
                    board[WinPos[i, 0]] == board[WinPos[i, 2]])
                    return board[WinPos[i, 2]];
            }

            return 0;
        }

        public static int MiniMax(int[] board, int Player)
        {
            int WinnerPlayer = Winner(board);
            if (WinnerPlayer != 0) return WinnerPlayer * Player;

            int Move = -1;      // arbitrary
            int BestScore = -2;     // To ensure a long albeit losing game.

            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)  // Cell is empty. Legal ove.
                {
                    board[i] = Player; // Tests a move
                    int TestScore = -MiniMax(board, Player * -1); // Alternate
                    if (TestScore > BestScore) 
                    // Best score for player, Worst for opponent
                    {
                        BestScore = TestScore;
                        Move = i;
                    }
                    board[i] = 0; // Reset Board, Continue
                }
            }

            if (Move == -1) return 0;
            return BestScore;
        }

        public static char PrintGridChar(int p)
        {
            switch (p)
            {
                case 1:
                    return 'O';
                case -1:
                    return 'X';
                case 0:
                    return ' ';
            }
            return ' ';
        }

        public static void PrintGrid(int[] board)
        {
            Console.WriteLine(" {0} | {1} | {2}", PrintGridChar(board[0]), PrintGridChar(board[1]), PrintGridChar(board[2]));
            Console.WriteLine("---+---+---");
            Console.WriteLine(" {0} | {1} | {2}", PrintGridChar(board[3]), PrintGridChar(board[4]), PrintGridChar(board[5]));
            Console.WriteLine("---+---+---");
            Console.WriteLine(" {0} | {1} | {2}", PrintGridChar(board[6]), PrintGridChar(board[7]), PrintGridChar(board[8]));
        }

        public static void ComputerPlayer(int[] board)
        {
            int BestMove = -1;
            int BestScore = -2;

            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = 1;
                    int TestScore = -MiniMax(board, -1);
                    board[i] = 0;
                    if (TestScore > BestScore)
                    {
                        BestScore = TestScore;
                        BestMove = i;
                    }

                }
            }

            board[BestMove] = 1;
        }

        public static void HumanPlayer(int[] board)
        {
            int Move = 0;

            do
            {
                Console.Write("Enter your preferred cell: ");
                Move = int.Parse(Console.ReadLine());
            } 
            while (Move >= 9 || Move < 0 || board[Move] != 0);

            board[Move] = -1;
        }

        static void Main(string[] args)
        {
            int[] Board = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Console.WriteLine("Computer (O), You (X)");
            Console.Write("Wanna go first? ");
            string prompt = Console.ReadLine();
            int Player = (prompt == "Yes" || prompt == "yes") ? 1 : 2;
            Console.WriteLine("");
            for (int i = 0; i < 9 && Winner(Board) == 0; i++)
            {
                if ((i + Player) % 2 == 0)
                    ComputerPlayer(Board);
                else
                {
                    PrintGrid(Board);
                    HumanPlayer(Board);
                }
            }

            switch (Winner(Board))
            {
                case 0:
                    Console.WriteLine("Draw!");
                    break;
                case 1:
                    PrintGrid(Board);
                    Console.WriteLine("You Lose!");
                    break;
                case -1:
                    Console.WriteLine("You WIN?!!");
                    break;
            }

            Console.ReadKey();

        }
    }
}
