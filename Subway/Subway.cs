// COIS-3020 Winter 2023 -- Assignment 1
// AUTHORS: Owen Wertele, Darren, Allen Kim


using System;
using System.Drawing;
using System.Xml.Linq;

namespace A1Q1
{
    public enum Colour { RED, YELLOW, BLUE } // For example
    public class SubwaySystem
    {
        private class Node
        {
            public Station? Connection { get; set; }     // Adjacent station (connection)
            public Colour Line { get; set; }            // Colour of its subway line
            public Node? Next { get; set; }              // Link to the next adjacent station (Node)

            public Node()
            {
                Connection = null;
                Line = 0;
                Next = null;
            }

            public Node(Station connection, Colour c, Node next)
            {
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
            public Node? E { get; set; }                 // Header node for a linked list of adjacent stations
            public Station? Parent { get; set; }
            public Station(string name)
            {
                Name = name;
                Visited = false;
                E = new Node();
                Parent = null;
            }

            //public int FindConnection(string name)      //i think this should be changed to traverse a linked list, not an array
            //{
            //int i;
            //Node *p;
            //for (i = 0; i < E.Count; i++)
            //{
            //if (E[i].Connection.Name.Equals(name))
            //return i;
            //}
            //return -1;
            //}
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

        public class SubwayMap
        {
            private Dictionary<string, Station> S;          // Dictionary of stations

            public SubwayMap()
            {
                S = new Dictionary<string, Station>();
            }


            public bool InsertStation(string name)
            {
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
            //public bool RemoveStation(string stationName) {
                
                //if (S.ContainsKey(stationName))
                //{
                    // now we must remove the station-to-be-removed from other stations that have connections to it

                    //if (S[stationName].E == null)
                    //{
                        //S.Remove(stationName);

                    //} else if(S[stationName].E.Next == null)
                    //{

                    //}
                //}
            //}

            // UNFINISHED
            // check if both stations already exist
            // check if edge does not already exist
            // add edge to BOTH stations (since undirected)
            // maybe also reconnect lines to accomadate the new station?
            public bool InsertConnection(string name1, string name2, Colour c)
            {
                if (S.ContainsKey(name1) && S.ContainsKey(name2))
                    return InsertDirectedConnection(name1, name2, c) &&
                           InsertDirectedConnection(name2, name1, c);
                else
                    return false;
            }

            private bool InsertDirectedConnection(string start, string end, Colour c)
            {
                Station startStation = S[start];
                Station endStation = S[end];
                Node newConnection = new Node(endStation, c, null);

                Node n = startStation.E;
                while (n.Next != null && !(n.Next.Connection == endStation && n.Next.Line == c))
                    n = n.Next;

                if (n.Next != null && n.Next.Connection == endStation && n.Next.Line == c)
                    return false;
                else
                {
                    n.Next = newConnection;
                    return true;
                }
            }

            // UNFINISHED
            // check if station1 is in the dictionary of stations S, then access its adj stations and remove station2 (and vice versa)
            // i think we have to reconnect the broken link too?
            //public bool RemoveConnection(string name1, string name2, Colour c) {

            //int i;

            //REMOVE EDGE FOR BOTH STATIONS SINCE UNDIRECTED
            //if (S.ContainsKey(name1) && (i = S[name1].FindConnection(name2) > -1))
            //{
            //S[name1].E.RemoveAt(i);             // update once linked list is implemented
            //}
            //}


            // SHORTEST ROUTE
            //
            public bool ShortestRoute(string name1, string name2)
            {
                Queue<Station> Q = new Queue<Station>();
                Station station;
                Station adjStation;
                Node n;

                if (!(S.ContainsKey(name1) && S.ContainsKey(name2))) // Check if stations indicated exist
                    return false;

                station = S[name1]; // start search from first station
                station.Visited = true;
                station.Parent = station;
                Q.Enqueue(station); // add start to queue

                while (Q.Count > 0)
                {
                    station = Q.Dequeue(); // get station from queue
                    if (station.Name == name2) // if station is the destination
                    {
                        PrintParents(station); // print all stations from root to current
                        ClearVisited();
                        return true;
                    }
                    n = station.E.Next; //start of adjacent nodes in station
                    while (n != null && n.Connection != null)
                    {
                        adjStation = n.Connection; //adjStation to station
                        if (!adjStation.Visited) //if unvisited
                        {
                            adjStation.Visited = true; // now visited
                            adjStation.Parent = station; //parent is current station
                            Q.Enqueue(adjStation); // add to queue
                        }
                        n = n.Next; // next adjecent station to current
                    }
                }
                return false;
            }

            private void PrintParents(Station station)
            {
                if (station.Parent != null && station.Parent != station)
                    PrintParents(station.Parent);
                Console.Write(" {0}", station.Name);
                station.Parent = null;
            }

            private void ClearVisited()
            {
                foreach (KeyValuePair<string, Station> kvp in S)
                {
                    kvp.Value.Visited = false;
                }
            }

            public void PrintStations()
            {
                foreach (KeyValuePair<string, Station> kvp in S)
                {
                    Console.WriteLine(kvp.Key);
                    PrintConnections(kvp.Value);
                }
            }

            private void PrintConnections(Station st)
            {
                if (st.E.Next == null)
                    return;
                Node n = st.E.Next;
                while (n != null)
                {
                    if (n != null && n.Connection != null && n.Connection.Name != null)
                        Console.WriteLine("  {0} {1}", n.Connection.Name, n.Line);
                    n = n.Next;
                }
            }
        }

        //------------------------------------------

        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("SUBWAY SYSTEM TEST");

                SubwayMap M = new SubwayMap();

                M.InsertStation("AAA");
                M.InsertStation("BBB");
                M.InsertStation("CCC");
                M.InsertStation("DDD");
                M.InsertConnection("AAA", "BBB", Colour.RED);
                M.InsertConnection("AAA", "BBB", Colour.BLUE);
                M.InsertConnection("CCC", "AAA", Colour.YELLOW);
                M.InsertConnection("DDD", "AAA", Colour.YELLOW);



                M.PrintStations();

                Console.ReadLine();
            }
        }

    }
}