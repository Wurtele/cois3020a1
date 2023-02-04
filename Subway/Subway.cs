using System;

namespace A1Q1 {
    public enum Colour { RED, YELLOW, GREEN } // For example
    public class SubwayMap
    {
        private class Node
        {
            public Station Connection { get; set; }     // Adjacent station (connection)
            public Colour Line { get; set; }            // Colour of its subway line
            public Node Next { get; set; }              // Link to the next adjacent station (Node)
            public Node(Station connection, Colour c, Node next) {
                Connection = connection;
                Line = c;
                Next = next;
            }
        }

        private class Station
        {
            public string Name { get; set; }            // Name of the subway station
            public bool Visited { get; set; }            // Used for the breadth-first search
            public Node E { get; set; }                 // Header node for a linked list of adjacent stations
            public Station(string name) {
                Name = name;
                Visited = false;
                //E = new Node<T>;
            }
        }

        //private Dictionary<string, Station> S;          // Dictionary of stations
        //public SubwayMap() { … }

        //public void InsertStation(string name) { … }

        //public bool RemoveStation(string name) { … }

        //public bool InsertConnection(string name1, string name2, Colour c) { … }

        //public bool RemoveConnection(string name1, string name2, Colour c) { … }

        //public void ShortestRoute(string name1, string name2) { … }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world !!");
        }
    }

}