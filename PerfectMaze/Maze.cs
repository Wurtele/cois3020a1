// AUTHORS: Owen Wertele, Darren Dsilva, Allen Kim
// COIS-3020 Winter 2023 -- Assignment 1

using System;
using System.Linq;

namespace PerfectMaze
{

    public class Maze
    {
        private char[,] M; // Maze
        private int size; // Height and width // cells + walls

        private static int[][] directions = new int[][] { // Directions  on a grid // default weight
                new int[] {-1,0,100},
                new int[] {0,-1,100},
                new int[] {1,0,100},
                new int[] {0,1,100}};

        // Constructor
        public Maze(int n)
        {
            size = n * 2 + 1; // Calculate size
            M = new char[size,size]; // Create maze grid
            Initialize(); // Fill maze 
            if (n != 0)
                Create(); // Break down walls to form maze
        }

        public int Size
        {
            get { return size; }
        }

        private void Initialize() // Fill maze with cells surrounded by walls
        {
            char c;
            for (int i = 0; i < size; i++) // For each element of grid
            {
                for (int j = 0; j < size; j++)
                {
                    if (i % 2 != 0 && j % 2 != 0) 
                        c = ' '; // Place empty space
                    else          
                        c = '|'; // Place wall
                    M[i, j] = c;
                }
            }
        }

        private void Create() 
        {
            bool[,] visited = new bool[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i % 2 != 0 && j % 2 != 0) 
                        visited[i,j] = false; // Set all cells to unvisited
                    else 
                        visited[i,j] = true; // Set all walls to visited
                }
            }

            DepthFirstSearch(1, 1, visited);
        }

        private void DepthFirstSearch(int i, int j, bool[,] visited) // Depth-first search to breakdown walls between cells
        {
            visited[i,j] = true; // Mark current cell as visited
            int x, y;

            Random rand = new Random();
            for (int k = 0; k < 4; k++) // Add random weight to each direction
                directions[k][2] = rand.Next(0, 100);

            int [][] randDirections = directions.OrderBy(r => r[2]).ToArray(); // Sort by random weight

            for (int k = 0; k < 4; k++) // For each adjacent cell (vertex)
            {
                x = i + randDirections[k][0]*2; // x position of cell
                y = j + randDirections[k][1]*2; // y
                if(x >= 0 && y >= 0 && x < size && y < size) // Check if cell is valid
                {
                    if (!visited[x,y]) // Cell is unvisited
                    {
                        if (x != i) 
                            M[(i + randDirections[k][0]), j] = ' '; // Remove wall to next cell
                        else if (y != j)
                            M[i, (j + randDirections[k][1])] = ' '; // Remove wall to next cell

                        DepthFirstSearch(x, y, visited); // Move to adjacent cell
                    }
                } 
            }
        }


        public void Print()
        {
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                    Console.Write(M[i,j]);
                Console.WriteLine();
            }
        }
    }

    public class MazeProgram
    {
        static void Main()
        {
            // Create mazes
            Maze maze0 = new Maze(0);
            Maze maze1 = new Maze(1);
            Maze maze2 = new Maze(2);
            Maze maze5 = new Maze(5);
            Maze maze10 = new Maze(10);
            Maze maze20 = new Maze(20);

            // Print mazes with title
            Console.WriteLine("\nSIZE = {0}", 0);
            maze0.Print();

            Console.WriteLine("\nSIZE = {0}", 1);
            maze1.Print();

            Console.WriteLine("\nSIZE = {0}", 2);
            maze2.Print();

            Console.WriteLine("\nSIZE = {0}", 5);
            maze5.Print();

            Console.WriteLine("\nSIZE = {0}", 10);
            maze10.Print();

            Console.WriteLine("\nSIZE = {0}", 20);
            maze20.Print();

            Console.ReadLine();
        }
    }
}
