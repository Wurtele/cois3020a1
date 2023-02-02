using System;

public static class MazeDemo
{
    public static void Main()
    {
        // Create mazes
        Maze maze0 = new Maze(0);
        Maze maze1 = new Maze(1);
        Maze maze2 = new Maze(2);
        Maze maze5 = new Maze(5);
        Maze maze10 = new Maze(10);
        Maze maze20 = new Maze(20);

        // Print mazes with title
        Console.WriteLine("\nSIZE = {0}",0);
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