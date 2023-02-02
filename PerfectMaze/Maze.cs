using System;
using System.Linq;

public class Maze
{
    private char[,] M;
    private int size;

    private static int[][] directions = new int[][] {
            new int[] {-1,0,100},
            new int[] {0,-1,100},
            new int[] {1,0,100},
            new int[] {0,1,100}};

    // Constructor
    public Maze(int n)
    {
        size = n * 2 + 1;
        M = new char[size,size];
        Initialize();
        if (n != 0)
            Create();
    }

    public int Size
    {
        get { return size; }
    }

    private void Initialize()
    {
        char c;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i % 2 != 0 && j % 2 != 0)
                    c = ' ';
                else
                    c = '|';
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
                    visited[i,j] = false; //set all cells to unvisited
                else 
                    visited[i,j] = true; // set all walls to visited
            }
        }

        DepthFirstSearch(1, 1, visited);
    }

    private void DepthFirstSearch(int i, int j, bool[,] visited)
    {
        visited[i,j] = true; // mark current cell as visited
        int x, y;

        Random rand = new Random();
        for (int k = 0; k < 4; k++) //add rand weight to each direction
            directions[k][2] = rand.Next(0, 100);

        int [][] randDirections = directions.OrderBy(r => r[2]).ToArray(); //sort by random weight

        for (int k = 0; k < 4; k++) // for each adjacent cell (vertex)
        {
            x = i + randDirections[k][0]*2; // x position of cell
            y = j + randDirections[k][1]*2; // y
            if(x >= 0 && y >= 0 && x < size && y < size) // check if cell is valid
            {
                if (!visited[x,y]) // cell is unvisited
                {
                    if (x != i) 
                        M[(i + randDirections[k][0]), j] = ' '; // remove wall to next cell
                    else if (y != j)
                        M[i, (j + randDirections[k][1])] = ' ';

                    DepthFirstSearch(x, y, visited); // move to adjacent cell
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
