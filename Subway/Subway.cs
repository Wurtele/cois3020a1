using System;
using System.Drawing;

namespace A1Q1 {
    public enum Colour { RED, YELLOW, GREEN } // For example
    public class SubwaySystem
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
            public List<Node> E { get; set; }                 // Header node for a linked list of adjacent stations
            public Station(string name) {
                Name = name;
                Visited = false;
                E = new List<Node>();
            }

            public int FindConnection(string name)      //i think this should be changed to traverse a linked list, not an array
            {
                int i;
                //Node *p;
                for (i = 0; i < E.Count; i++)
                {
                    if (E[i].Connection.Name.Equals(name))
                        return i;
                }
                return -1;
            }
        }

        
        public interface ISubwayMap
        {
            void InsertStation(string name);
            bool RemoveStation(string name);
            bool InsertConnection(string name1, string name2, Color c);
            bool RemoveConnection(string name1, string name2, Color c);
            void ShortestRoute(string name1, string name2, Color c);
        }


        public class SubwayMap : ISubwayMap
        {
            private Dictionary<string, Station> S;          // Dictionary of stations

            public SubwayMap() {
                S = new Dictionary<string, Station>();
            }

            public void InsertStation(string name) {
                if (S.ContainsKey(name) == false)
                {
                    Station newStation = new Station(name);
                    S.Add(name, newStation);
                }
            }

            public bool RemoveStation(string name) { … }

            public bool InsertConnection(string stationName1, string stationName2, Colour c) {

                Node n;

                if (S.ContainsKey(stationName1) && S.ContainsKey(stationName2)) {
                    if (S[stationName1].FindConnection(stationName2) == -1)
                    {
                        Node e = new Node(S[stationName2], c, null);
                        S[stationName1].E.Add(e);
                    }
                }
            }

            public bool RemoveConnection(string name1, string name2, Colour c) {

                int i;

                //REMOVE EDGE FOR BOTH STATIONS SINCE UNDIRECTED
                if (S.ContainsKey(name1) && (i = S[name1].FindConnection(name2) > -1))
                {
                    S[name1].E.RemoveAt(i);
                }
            }

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