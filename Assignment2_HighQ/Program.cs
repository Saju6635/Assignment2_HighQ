using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GemHunter
{

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

    }

    public class Cell
    {
        public string Occupant { get; set; } 

  
        public Cell()
        {
            Occupant = "-";
        }
    }

    public class Player
    {
        //Properties
        public string Name { get; set; }
        public Position Position { get; set; }
        public int GemCount { get; set; }


        public Player(string name, Position position)
        {
            Name = name;
            Position = position;
            GemCount = 0;
        }
        public void Move(char direction)
        {
            int newX = Position.X;
            int newY = Position.Y;

            //checking the input value provided by the user (U,D,L,R)
            switch (direction)
            {
                case 'U':
                    newY--;
                    break;
                case 'D':
                    newY++;
                    break;
                case 'L':
                    newX--;
                    break;
                case 'R':
                    newX++;
                    break;


            }

            if (IsValidMove(newX, newY))
            {
                Position.X = newX;
                Position.Y = newY;
            }
            else
            {
                Console.WriteLine("Invalid move, please give U/D/L/R");
            }
        }
        private bool IsValidMove(int x, int y)
        {
            return x >= 0 && x < 6 && y >= 0 && y < 6;
        }
    }

    public class Board
    {
        public Cell[,] Grid { get; }
        public Board()   //constructor
        {
            Grid = new Cell[6, 6];
            InitialBoard();
        }
        private void InitialBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Grid[i, j] = new Cell();
                }
            }
            Grid[0, 0].Occupant = "P1";
            Grid[5, 5].Occupant = "P2";

            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                int x = random.Next(6);
                int y = random.Next(6);

                if (Grid[x, y].Occupant == "-")
                {
                    Grid[x, y].Occupant = "G";
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(6);
                int y = random.Next(6);

                if (Grid[x, y].Occupant == "-")
                {
                    Grid[x, y].Occupant = "O";
                }
                else
                {
                    i--;
                }
            }
        }

        //  to print the current state of the board
        public void Display(Player player1, Player player2)
        {
            Console.WriteLine("Gem Hunters Console Game");
            Console.WriteLine("**********************************");
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (player1.Position.X == x && player1.Position.Y == y)
                    {
                        Console.Write("P1 ");
                    }
                    else if (player2.Position.X == x && player2.Position.Y == y)
                    {
                        Console.Write("P2 ");
                    }
                    else
                    {
                        Console.Write(Grid[x, y].Occupant + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool IsValidMove(Player player, char direction)
        {
            int newX = player.Position.X;
            int newY = player.Position.Y;


            switch (direction)
            {
                case 'U':
                    newY = Math.Max(0, newY - 1);
                    break;
                case 'D':
                    newY = Math.Min(5, newY + 1);
                    break;
                case 'L':
                    newX = Math.Max(0, newX - 1);
                    break;
                case 'R':
                    newX = Math.Min(5, newX + 1);
                    break;

            }


            if (newX < 0 || newX >= Grid.GetLength(0) || newY < 0 || newY >= Grid.GetLength(1))
            {
                return false;
            }

            //Also Checks if the new position is not occupied by another player or an obstacle

            return Grid[newX, newY].Occupant != "P1" && Grid[newX, newY].Occupant != "P2" && Grid[newX, newY].Occupant != "O";

        }

        //Method to check if the player's new position contains a gem and updates the player's GemCount
        public void CollectGem(Player player)
        {
            if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
            {
                player.GemCount++;

                Grid[player.Position.X, player.Position.Y].Occupant = "-";

            }

        }
        public bool TotalGems()
        {
            foreach (Cell cell in Grid)
            {
                if (cell.Occupant == "G")
                {
                    return false;
                }
            }
            return true;
        }
        
    }

    public class Game
    {
        public Board Board { get; }
        public Player Player1 { get; }
        public Player Player2 { get; }
        public Player CurrentTurn { get; set; }
        public int TotalTurns { get; private set; }

        public Game()
        {
            Board = new Board();
            Player1 = new Player("P1", new Position(0, 0));
            Player2 = new Player("P2", new Position(5, 5));
            CurrentTurn = Player1;
            TotalTurns = 0;

        }


        public void Start()// method to Begin the game, displays the board, and alternates player turns.
        {

            do
            {

                Board.Display(Player1, Player2);
                Console.WriteLine($"Turn {TotalTurns + 1} - {CurrentTurn.Name}'s turn");
                Console.Write("Enter your move (U/D/L/R): ");
                char move = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (Board.IsValidMove(CurrentTurn, move))
                {
                    Board.Grid[CurrentTurn.Position.X, CurrentTurn.Position.Y].Occupant = "-";
                    CurrentTurn.Move(move);
                    Board.CollectGem(CurrentTurn);

                    // Updating the new position
                    Board.Grid[CurrentTurn.Position.X, CurrentTurn.Position.Y].Occupant = CurrentTurn.Name;
                    SwitchTurn();

                }
                else
                {
                    Console.WriteLine("Invalid move! Try again.");
                }

            } while (!IsGameOver());


            Board.Display(Player1, Player2);
            AnnounceWinner();
        }

        //to switch turns between player 1 and player 2
        private void SwitchTurn()
        {

            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
            TotalTurns++;
        }

        //Checking if reached 30 moves or total gems collected.
        public bool IsGameOver()
        {

            return TotalTurns >= 30 || Board.TotalGems();
        }
        //Displaying the winner of the game
        public void AnnounceWinner()
        {
            Console.WriteLine("Game Over");
            Console.WriteLine($"The {Player1.Name} collected {Player1.GemCount} gems.");
            Console.WriteLine($"The {Player2.Name} collected {Player2.GemCount} gems.");

            if (Player1.GemCount > Player2.GemCount)
            {
                Console.WriteLine($"{Player1.Name} wins the game");
            }
            else if (Player1.GemCount < Player2.GemCount)
            {
                Console.WriteLine($"{Player2.Name} wins the game");
            }
            else
            {
                Console.WriteLine("It's a tie between Player 1 and Player 2");
            }
        }


    }

    class Program
    {
        static void Main(string[] args)
        {

            Game gem = new Game();
            gem.Start();
        }
    }










}