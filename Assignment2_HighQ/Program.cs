using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemHunter
{

    public class Cell
    {
        public string Occupant { get; set; } 

  
        public Cell()
        {
            Occupant = "-";
        }
    }

    public class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Board
    {
        public Cell[,] Grid { get; } // 2D array to represent the game board

        // pre-set positions for Player 1 and Player 2
        public Board()
        {
            Grid = new Cell[6, 6]; // 6x6 square board

            InitializeBoard(); 
        }

        // Method to initialize the board 
        private void InitializeBoard()
        {
            // Initialize each cell on the board
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Grid[i, j] = new Cell();
                }
            }

            // Set pre-set positions for Player 1 and Player 2
            Grid[0, 0].Occupant = "P1"; 
            Grid[5, 5].Occupant = "P2"; 

            
            PlaceGems();

            PlaceObstacles();
        }

        // Method to randomly place gems on the board
        private void PlaceGems()
        {
            Random random = new Random();
            int gemCount = 0;

            while (gemCount < 5)
            {
                int x = random.Next(6);
                int y = random.Next(6);

                if (Grid[x, y].Occupant == "-")
                {
                    Grid[x, y].Occupant = "G";
                    gemCount++;
                }
            }
        }

        // Method to randomly place obstacles on the board
        private void PlaceObstacles()
        {
            Random random = new Random();
            int obstacleCount = 0;

            while (obstacleCount < 5)
            {
                int x = random.Next(6);
                int y = random.Next(6);

                if (Grid[x, y].Occupant == "-")
                {
                    Grid[x, y].Occupant = "O";
                    obstacleCount++;
                }
            }
        }

        // Method to display the current state of the board
        public void Display()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Console.Write(Grid[i, j].Occupant + " "); // Display the occupant of each cell
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Creating a new board
            Board board = new Board();

            // Display the board
            board.Display();
        }
    }

}