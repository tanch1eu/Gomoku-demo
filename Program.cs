using System;
using System.IO;

namespace ConsoleGames_Demo
{
    class Program
    {
        static int numTurns = 0; // counting player's turns.
        public const int boardSize = 15;
        static string[,] chessPos = new string[boardSize, boardSize]; // array to store player input for chess positions.
        static string[,] chessFormation1 = new string[boardSize, boardSize] {
        {"", "", "O", "", "", "", "X", "O", "", "", "X", "", "", "", ""},
        {"", "", "O", "", "", "", "", "", "X", "", "", "", "", "", ""},
        {"", "", "X", "", "", "", "", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", "O", "", "", "X", "", "", ""},
        {"", "", "", "", "", "", "X", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", "", "X", "", "", "", "", ""},
        {"", "", "", "O", "", "", "", "", "", "O", "", "", "", "", ""},
        {"", "", "", "", "", "", "O", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "X", "X", "O", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "O", "", "", "", "", "", "", "", ""},
        {"", "", "", "O", "", "", "", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "O", "", "", "", "", "", "", ""},
        {"", "X", "", "", "", "", "", "", "", "", "X", "", "", "", ""},
        {"", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", "", "O", "", "", "", "", ""}
        }; // array to store chess formation, ready for Strategic play mode (under development) with different tactical formations.
        private Player[] players = new Player[2];
        
        static void Main(string[] args)
        {
            int player = 1, input_row, input_col, auto_row, auto_col;
            string gameType, namePlayer, namePlayer1, namePlayer2, winner;
            char vsComp, undo;
            bool goodInput, goodAutoInput, checkRange, playwithAI, isUndo;
            Random rnd = new Random();

            Console.WriteLine("Which game you want to play? Gomoku, or Othello, or Mills >> ");// Select game to play. Gomoku only. Others are not included in this program.
            gameType = Console.ReadLine();
            while (gameType != "Gomoku")
            {
                if ((gameType == "Othello")||(gameType == "Mills"))
                {
                    Console.WriteLine("This game is under development. Please choose another game >> ");// Choose another game
                    gameType = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Game not exists. Try again >> ");
                    gameType = Console.ReadLine();
                }
            }// Check typo. 

            Console.WriteLine("Do you want to play versus computer? Input Y/N >> ");// Select to play vs human/computer.
            vsComp = Convert.ToChar(Console.ReadLine());           
            while (!(vsComp == 'N' || vsComp == 'Y' || vsComp == 'n' || vsComp == 'y'))
            {               
                Console.WriteLine("Wrong character. Try again >> ");
                vsComp = Convert.ToChar(Console.ReadLine());
            }// Check typo.
            
            playwithAI = vsComp == 'Y' || vsComp == 'y';

            if (gameType == "Gomoku") // Starting to play Gomoku.
            {
                if (playwithAI == false) 
                {
                    Intro_Gomoku();// Begin Gomoku game. 

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Please input first player's name: ");
                    Console.ResetColor();
                    namePlayer1 = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Please input second player's name: ");
                    Console.ResetColor();
                    namePlayer2 = Console.ReadLine();

                    Player player1 = new Player(namePlayer1);
                    Player player2 = new Player(namePlayer2);

                    BoardInit_Gomoku(); // First view of the chess board.

                    Console.WriteLine("\nPlayer-1 {0} go first. Let's begin your first move ...", player1.Name);
                    Console.WriteLine("Choose a ROW number from the chessboard to place your chess >> ");
                    input_col = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Now choose its COLUMN number >> ");
                    input_row = Convert.ToInt32(Console.ReadLine());
                    placeChess(player, input_col, input_row);

                    player = 2;

                    while (true)
                    {
                        Console.WriteLine("Are you sure :D Do you want to undo? just type Y/N >> ");
                        undo = Convert.ToChar(Console.ReadLine());
                         
                        while (!(undo == 'N' || undo == 'Y' || undo == 'n' || undo == 'y'))
                        {
                            Console.WriteLine("Wrong character. Try again >> ");
                            undo = Convert.ToChar(Console.ReadLine());
                        }// Check typo.

                        isUndo = undo == 'Y' || undo == 'y';
                        if (isUndo == true)
                        {
                            chessPos[input_col, input_row] = ""; // return the chess position element to Null.
                            BoardInit_Gomoku(); // re-draw the chess board.
                            if (player == 1)
                            {
                                player = 2;
                            }
                            else if (player == 2)
                            {
                                player = 1;
                            } // swap players
                            --numTurns; 
                        }
                        Console.Clear();
                        BoardInit_Gomoku(); // re-draw the chess board.
                        Console.WriteLine($"\nPlayer-{player}: It's your turn.");
                        Console.WriteLine("Choose a ROW number to place your chess >> ");
                        input_col = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Now choose its COLUMN number >> ");
                        input_row = Convert.ToInt32(Console.ReadLine());

                        checkRange = (input_col >= 0 && input_col < boardSize) && (input_row >= 0 && input_row < boardSize);

                        while (!checkRange)
                        {
                            Console.WriteLine($"\nPlayer-{player}, you input a wrong number. Try again !");
                            Console.WriteLine("Choose a ROW number to place your chess >> ");
                            input_col = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Now choose its COLUMN number >> ");
                            input_row = Convert.ToInt32(Console.ReadLine());
                            checkRange = (input_col >= 0 && input_col < boardSize) && (input_row >= 0 && input_row < boardSize);
                        } // check for valid range of input number.
                        
                        goodInput = checkNull(input_col, input_row); // check for free position to add new element.

                        while (!goodInput)
                        {
                            Console.WriteLine("\nThis position already has a CHESS, it is {0}. Check again !", chessPos[input_col, input_row]);
                            Console.WriteLine("Choose a ROW number to place your chess >> ");
                            input_col = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Now choose COLUMN number >> ");
                            input_row = Convert.ToInt32(Console.ReadLine());
                            goodInput = checkNull(input_col, input_row);
                        }

                        placeChess(player, input_col, input_row);
                        ++numTurns;

                        if (isENDGAME(input_col, input_row))
                        {
                            if (player == 1) winner = player1.Name;
                            else winner = player2.Name;
                            endGame(winner);
                            break;
                        }
                        else
                        {
                            if (player == 1)
                            {
                                player = 2;
                            }
                            else if (player == 2)
                            {
                                player = 1;
                            }

                        }
                    };
                } // Player vs Player mode.
                else
                {                    
                    Intro_Gomoku();// Begin Gomoku game. 

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Please input your name: ");
                    Console.ResetColor();
                    namePlayer = Console.ReadLine();

                    Player mainplayer = new Player(namePlayer);

                    BoardInit_Gomoku(); // First view of the chess board.

                    Console.WriteLine("\nPlayer {0} go first. Let's begin your first move ...", mainplayer.Name);
                    Console.WriteLine("Choose a ROW number from the chessboard to place your chess >> ");
                    input_col = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Now choose its COLUMN number >> ");
                    input_row = Convert.ToInt32(Console.ReadLine());
                    placeChess(player, input_col, input_row);

                    auto_col = rnd.Next(boardSize - 1);
                    auto_row = rnd.Next(boardSize - 1);
                    goodAutoInput = checkNull(auto_col, auto_row);
                    while (!goodAutoInput)
                    {
                        auto_col = rnd.Next(boardSize - 1);
                        auto_row = rnd.Next(boardSize - 1);
                        goodAutoInput = checkNull(auto_col, auto_row);
                    } // check for free position to place AI chess
                    AIplaceChess(auto_col, auto_row); // done AI first chess placement

                    while (true)
                    {
                        Console.WriteLine("Are you sure :D Do you want to undo? just type Y/N >> ");
                        undo = Convert.ToChar(Console.ReadLine());

                        while (!(undo == 'N' || undo == 'Y' || undo == 'n' || undo == 'y'))
                        {
                            Console.WriteLine("Wrong character. Try again >> ");
                            undo = Convert.ToChar(Console.ReadLine());
                        }// Check typo.

                        isUndo = undo == 'Y' || undo == 'y';
                        if (isUndo == true)
                        {
                            chessPos[input_col, input_row] = ""; // return the chess position element to Null.
                            chessPos[auto_col, auto_row] = ""; // return the AI previous move.
                            BoardInit_Gomoku(); // re-draw the chess board. 
                            player = 1;                            
                        } // Check if Undo.
                        else player = 1;
                        Console.WriteLine($"\n{mainplayer.Name}: Now it's your turn.");
                        Console.WriteLine("Choose a ROW number to place your chess >> ");
                        input_col = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Now choose a COLUMN number >> ");
                        input_row = Convert.ToInt32(Console.ReadLine());

                        checkRange = (input_col >= 0 && input_col < boardSize) && (input_row >= 0 && input_row < boardSize);
                        while (!checkRange)
                        {
                            Console.WriteLine($"\nPlayer-{player}, you input a wrong number. Try again !");
                            Console.WriteLine("Choose a ROW number to place your chess >> ");
                            input_col = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Now choose its COLUMN number >> ");
                            input_row = Convert.ToInt32(Console.ReadLine());
                            checkRange = (input_col >= 0 && input_col < boardSize) && (input_row >= 0 && input_row < boardSize);
                        } // check for valid range of number from player input.

                        goodInput = checkNull(input_col, input_row); // check for free position to add new element from player input.

                        while (!goodInput)
                        {
                            Console.WriteLine("\nThis position already has a CHESS, it is {0}. Check again !", chessPos[input_col, input_row]);
                            Console.WriteLine("Choose a ROW number to place your chess >> ");
                            input_col = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Now choose COLUMN number >> ");
                            input_row = Convert.ToInt32(Console.ReadLine());
                            goodInput = checkNull(input_col, input_row);
                        } // check for free position to add new element for player's placement

                        placeChess(player, input_col, input_row);
                        // Main player chess placement part.

                        ++numTurns;
                        if (isENDGAME(input_col, input_row))
                        {
                            winner = mainplayer.Name;                          
                            endGame(winner);
                            break;
                        } // Check if player wins.
                        else
                        {
                            player = 2;
                            auto_col = rnd.Next(boardSize - 1);
                            auto_row = rnd.Next(boardSize - 1);
                            goodAutoInput = checkNull(auto_col, auto_row);
                            while (!goodAutoInput)
                            {
                                auto_col = rnd.Next(boardSize - 1);
                                auto_row = rnd.Next(boardSize - 1);
                                goodAutoInput = checkNull(auto_col, auto_row);
                            } // check for free position to place AI chess
                            AIplaceChess(auto_col, auto_row); // done AI chess placement
                            if (isENDGAME(auto_col, auto_row))
                            {
                                winner = "Computer";
                                endGameAI(winner);
                                break;
                            } // calculate winning for AI.
                        } // Start AI chess placement and check if computer wins.
                    }
                } // Player vs Computer mode (Strategic mode == "random move without thinking ahead").
            }          
        } //Gameplay main control board. Controlling player's turns with players input.

        public static void BoardInit_Gomoku()
        {
            Console.Clear();
            string border = "   -------------------------------------------------------------";
            string[] marker = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"};
            for (int i = 0; i < boardSize; ++i)
            {
                Console.WriteLine(border);
                Console.Write("{0,2} ", i);
                for (int j = 0; j < boardSize; ++j)
                {
                    Console.Write("| {0,1} ", chessPos[i, j]);
                }
                Console.WriteLine("|");
            }
            Console.WriteLine(border);
            Console.Write("   ");
            for (int i = 0; i < boardSize; ++i)
            {
                Console.Write(" {0,2} ", marker[i]);
            }
            Console.WriteLine("");
        }// Draw the 15x15 chess board to the console 
        public static void Intro_Gomoku()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nWelcome to Gomoku (Five-in-a-row), press any key to begin");
            Console.ReadKey();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("INTRODUCTION");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Gomoku is a two player turn based game." +
                              "\nYou can play with your friend or computer." +
                              "\nPress any key to continue");
            Console.ReadKey();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("HOW TO PLAY");
            Console.ForegroundColor = ConsoleColor.Yellow;           
            Console.WriteLine("Players are represented with a unique CHESS signature X or O" +
                              "\nPlayer 1 = X. Player 2 = O" + 
                              "\nIf you play vs Computer. Then, Computer AI will be = O");
            Console.WriteLine("\nThe first player to form a FIVE UNBROKEN CHAIN in a row (horizontal, vertical, or diagonal) is the winner");
            Console.WriteLine();
            Console.WriteLine("Good luck! \nNow press any key to begin the game!...");
            Console.ReadKey();
            Console.ResetColor();
            Console.Clear();
        }// Games introduction      
        public static void placeChess(int player, int col, int row) 
        {
            if (player == 1)
            {
                chessPos[col, row] = "X";
                BoardInit_Gomoku();
            }
            else if (player == 2)
            {
                chessPos[col, row] = "O";
                BoardInit_Gomoku();
            }           
        } // place chess, if player 1 - place "X", if player 2 - place "O"
        public static void AIplaceChess(int col, int row)
        {           
            chessPos[col, row] = "O";
            BoardInit_Gomoku();
        }
        public static bool checkNull(int col, int row)
        {
            if (chessPos[col, row] == "X" || chessPos[col, row] == "O")
            {
                return false;
            }
            else return true;
        } // Check null position to place chess.
        public static bool isENDGAME(int col, int row)
        {
            return isEND_horizontal(col, row) || isEND_vertical(col, row) || isEND_primaryDiagonal(col, row) || isEND_secondaryDiagonal(col, row);
        } // Check it is End game or not.
        public static bool isEND_horizontal(int col, int row)
        {
            int countLeft = 0;

            for (int i = row; i >= 0; --i)
            {
                if (chessPos[col, i] == chessPos[col, row])
                {
                    countLeft++;
                }
                else break;
            }

            int countRight = 0;

            for (int i = row + 1; i < boardSize; ++i)
            {
                if (chessPos[col, i] == chessPos[col, row])
                {
                    countRight++;
                }
                else break;
            }

            return countLeft + countRight == 5;
        } // Horizontal end.
        public static bool isEND_vertical(int col, int row)
        {
            int countDown = 0;

            for (int i = col; i >= 0; --i)
            {
                if (chessPos[i, row] == chessPos[col, row])
                {
                    countDown++;
                }
                else break;
            }

            int countUp = 0;

            for (int i = col + 1; i < boardSize; ++i)
            {
                if (chessPos[i, row] == chessPos[col, row])
                {
                    countUp++;
                }
                else break;
            }

            return countUp + countDown == 5;
        } // Vertical end.
        public static bool isEND_primaryDiagonal(int col, int row)
        {
            int countUp = 0;

            for (int i = 0; i <= col; ++i)
            {
                if (col - i < 0 || row - i < 0)
                {
                    break;
                }
                if (chessPos[col - i, row - i] == chessPos[col, row])
                {
                    countUp++;
                }
                else break;
            }

            int countDown = 0;

            for (int i = 1; i < boardSize - col; ++i)
            {
                if (col + i >= boardSize || row + i >= boardSize)
                {
                    break;
                }
                if (chessPos[col + i, row + i] == chessPos[col, row])
                {
                    countDown++;
                }
                else break;
            }

            return countUp + countDown == 5;
        } // Primary diagonal end.
        public static bool isEND_secondaryDiagonal(int col, int row)
        {
            int countUp = 0;

            for (int i = 0; i <= col; ++i)
            {
                if (col - i < 0 || row + i > boardSize)
                {
                    break;
                }
                if (chessPos[col - i, row + i] == chessPos[col, row])
                {
                    countUp++;
                }
                else break;
            }

            int countDown = 0;

            for (int i = 1; i < boardSize - col; ++i)
            {
                if (col + i >= boardSize || row - i < 0)
                {
                    break;
                }
                if (chessPos[col + i, row - i] == chessPos[col, row])
                {
                    countDown++;
                }
                else break;
            }

            return countUp + countDown == 5;
        } // Secondary diagonal end.
        public static void endGame(string winner)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Congratulation {0}, you've won the Gomoku match by {1} turns ! Brilliant !", winner, numTurns);
            Console.WriteLine();
            Console.WriteLine("----END GAME.----");           
        }
        public static void endGameAI(string winner)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("OOPS... {0} have won you by {1} turns. Better luck next time. Good luck!", winner, numTurns);
            Console.WriteLine();
            Console.WriteLine("----END GAME.----");
        }
    }
}
