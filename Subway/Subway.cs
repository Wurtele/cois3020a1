using System;
using System.Drawing;

namespace A1Q1 {
    public enum Colour { RED, YELLOW, GREEN } // For example
    public class SubwaySystem
    {
        private class Node<T>
        {
            public Station<T> Connection { get; set; }     // Adjacent station (connection)
            public Colour Line { get; set; }            // Colour of its subway line
            public Node<T> Next { get; set; }              // Link to the next adjacent station (Node)
            public Node(Station<T> connection, Colour c, Node<T> next) {
                Connection = connection;
                Line = c;
                Next = next;
            }
        }

        private class Station<T>
        {
            public string Name { get; set; }            // Name of the subway station
            public bool Visited { get; set; }            // Used for the breadth-first search
            public List<Node<T>> E { get; set; }                 // Header node for a linked list of adjacent stations
            public Station(string name) {
                Name = name;
                Visited = false;
                E = new List<Node<T>>();
            }
        }

        
        public interface ISubwayMap<T>
        {
            void InsertStation(string name);
            bool RemoveStation(string name);
            bool InsertConnection(string name1, string name2, Color c);
            bool RemoveConnection(string name1, string name2, Color c);
            void ShortestRoute(string name1, string name2, Color c);
        }


        public class SubwayMap<T> : ISubwayMap<T>
        {
            private Dictionary<string, Station<T>> S;          // Dictionary of stations

            public SubwayMap() {
                S = new Dictionary<string, Station<T>>();
            }

            public void InsertStation(string name) {
                if (S.ContainsKey(name) == false)
                {
                    Station<T> newStation = new Station<T>(name);
                    S.Add(name, newStation);
                }
            }

            public bool RemoveStation(string name) { … }

            public bool InsertConnection(string name1, string name2, Colour c) { … }

            public bool RemoveConnection(string name1, string name2, Colour c) { … }

            public void ShortestRoute(string name1, string name2) { … }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world !!");
        }
    }

}