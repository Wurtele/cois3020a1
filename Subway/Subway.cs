// COIS-3020 Winter 2023 -- Assignment 1
// AUTHORS: Owen, Darren, Allen


using System;
using System.Drawing;
using System.Xml.Linq;

namespace A1Q1 {
    public enum Colour { RED, YELLOW, BLUE } // For example
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

        //------------------------------------------

        private class Station
        {
            public string Name { get; set; }            // Name of the subway station
            public bool Visited { get; set; }            // Used for the breadth-first search
            public Node E { get; set; }                 // Header node for a linked list of adjacent stations
            public Station(string name) {
                Name = name;
                Visited = false;
                //E = new List<Node>();
                E = new Node();                 // need to fill the Node instance with parameters but idk with what
            }

            public int FindConnection(string name)      //i think this should be changed to traverse a linked list, not an array
            {
                //int i;
                //Node *p;
                //for (i = 0; i < E.Count; i++)
                //{
                    //if (E[i].Connection.Name.Equals(name))
                        //return i;
                //}
                //return -1;
            }
        }

        //------------------------------------------

        //public class LinkedList
        //{
            //private Node head;

            //public void AddFirstNode(Station connection, Colour c, Node next)
            //{
                //Node newNode = new Node(connection, c, next);
                //head = newNode;
            //}
        //}

        //------------------------------------------

        public interface ISubwayMap
        {
            void InsertStation(string name);
            bool RemoveStation(string name);
            bool InsertConnection(string name1, string name2, Colour c);
            bool RemoveConnection(string name1, string name2, Colour c);
            void ShortestRoute(string name1, string name2, Colour c);
        }

        //------------------------------------------

        public class SubwayMap : ISubwayMap
        {
            private Dictionary<string, Station> S;          // Dictionary of stations

            public SubwayMap() {
                S = new Dictionary<string, Station>();
            }
            

            public bool InsertStation(string name) {
                if (S.ContainsKey(name) == false)
                {
                    Station newStation = new Station(name);
                    S.Add(name, newStation);
                    return true;
                }
                return false;
            }


            // UNFINISHED
            // access station-to-be-removed's linked list of adjacent stations and delete the node from their lists too.
            // finally, delete station-to-be-removed
            // reconnect the missing link
            public bool RemoveStation(string stationName) {
                
                if (S.ContainsKey(stationName))
                {

                    //for(int i = 0; i < S[stationName].E.Count; i++)       // update once linked list is implemented
                    //{

                    //}
                }
            }


            // UNFINISHED
            // check if both stations already exist
            // check if edge does not already exist
            // add edge to BOTH stations (since undirected)
            // maybe also reconnect lines to accomadate the new station?
            public bool InsertConnection(string stationName1, string stationName2, Colour c) {

                if (S.ContainsKey(stationName1) && S.ContainsKey(stationName2)) {

                    Insert(stationName1, stationName2, c);
                    Insert(stationName2, stationName1, c);          // call Insert() twice to establish connection both ways

                    // return bool somehow
                }
            }


            private bool Insert(string stationName1, string stationName2, Colour c)
            {
                Node listNodeA = S[stationName1].E;
                Station B = S[stationName2];

                if (listNodeA == null)                  // if list of Nodes is empty
                {
                    listNodeA = new Node(B, c, null);
                    return true;
                }

                // traverse through linked list of Nodes till end is reached, or until we notice the connection already exists
                while (listNodeA.Next != null && !(listNodeA.Connection.Name.Equals(B.Name) && listNodeA.Line == c))
                {
                    listNodeA = listNodeA.Next;
                }

                // return false if the connection already existed
                if (listNodeA.Connection.Name.Equals(B.Name) && listNodeA.Line == c)
                {
                    return false;
                }

                listNodeA.Next = new Node(B, c, null);
                return true; 
            }


            // UNFINISHED
            // check if station1 is in the dictionary of stations S, then access its adj stations and remove station2 (and vice versa)
            // i think we have to reconnect the broken link too?
            public bool RemoveConnection(string name1, string name2, Colour c) {

                int i;

                //REMOVE EDGE FOR BOTH STATIONS SINCE UNDIRECTED
                if (S.ContainsKey(name1) && (i = S[name1].FindConnection(name2) > -1))
                {
                    S[name1].E.RemoveAt(i);             // update once linked list is implemented
                }
            }


            // UNFINISHED
            public void ShortestRoute(string name1, string name2) { … }
        }
    }

    //------------------------------------------

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world !!");
        }
    }

}