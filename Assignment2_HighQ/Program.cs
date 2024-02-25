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