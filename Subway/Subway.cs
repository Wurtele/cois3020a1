// AUTHORS: Owen Wurtele, Darren D'Silva, Allen Kim
// COIS-3020 Winter 2023 -- Assignment 1


using System;
using System.ComponentModel.Design;
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
        }

        public class SubwayMap
        {
            private Dictionary<string, Station> S;          // Dictionary of stations

            public SubwayMap()
            {
                S = new Dictionary<string, Station>();
            }


            // inserts a station by checking if the input station isn't already existing
            // otherwise if the station already exists, nothing new will be added
            public bool InsertStation(string name)
            {
                if (S.ContainsKey(name) == false)
                {
                    Station newStation = new Station(name);
                    S.Add(name, newStation);
                    return true;
                }
                Console.WriteLine("Station " + name + " already exists. Failed to insert new station.");
                return false;
            }

            public bool RemoveStation(string stationName)
            {
                // checks the station name to be removed and deletes it
                if (S.ContainsKey(stationName))
                {
                    S.Remove(stationName);
                    // removes the connections from other stations that have connections to the station
                    // also checks if the current node could possibly be a station that needs to be removed
                    // created a node to store the previous node in order to remove the current node 
                    foreach (KeyValuePair<string, Station> kvp in S)
                    {
                        Node n = kvp.Value.E.Next;
                        Node p = null;
                        while (n != null)
                        {
                            if (n.Connection.Name == stationName)
                            {
                                if (p != null)
                                {
                                    p.Next = n.Next;
                                }
                                else
                                {
                                    kvp.Value.E.Next = n.Next;
                                }
                            }
                            else
                            {
                                p = n;
                            }
                            n = n.Next;
                        }
                    }
                    return true;
                }
                return false;
            }


            // Inserts a connection to the end of a linked list of each station in the connection 
            // Searches the stations to add a connection in between, also checks if the stations are together.
            public bool InsertConnection(string name1, string name2, Colour c)
            {
                if (S.ContainsKey(name1) && S.ContainsKey(name2))
                {
                    // call the method below twice to establish connection both ways!!
                    return InsertDirectedConnection(name1, name2, c) &&
                           InsertDirectedConnection(name2, name1, c);
                }
                else
                {
                    Console.WriteLine("Failed to insert new connection. Must be between two stations that already exist!");
                    return false;
                }
            }


            // traverses through startStation's linked list of connections to check if the given connection
            // with endStation doesn't already exist. Otherwise, it'll reached the end of the linked list and
            // create a new Node (connection) for startStation
            private bool InsertDirectedConnection(string start, string end, Colour c)
            {
                // keep track of each Linked List E for both startStation and endStation
                Station startStation = S[start];
                Station endStation = S[end];
                Node newConnection = new Node(endStation, c, null);

                Node n = startStation.E;
                while (n.Next != null && !(n.Next.Connection == endStation && n.Next.Line == c))
                    n = n.Next;

                if (n.Next != null && n.Next.Connection == endStation && n.Next.Line == c)
                {
                    Console.WriteLine("Failed to insert new connection. That connection already exists!");
                    return false;
                }
                else
                {
                    n.Next = newConnection;
                    return true;
                }
            }


            // removes connections between two linked lists that are connected to each other,
            // returns false if connection is not in between two stations
            public bool RemoveConnection(string name1, string name2, Colour c) 
            {
                if (S.ContainsKey(name1) && S.ContainsKey(name2))
                {
                    // removes the connection by skipping vertices from the first station
                    Node n = S[name1].E;
                    while (n.Next != null)
                    {
                        if (n.Next.Connection.Name == name2 && n.Next.Line == c) n.Next = n.Next.Next;
                        if (n.Next != null) n = n.Next;
                    }
                    // removes the connection by skipping vertices from the second station
                    n = S[name2].E;
                    while (n.Next != null)
                    {
                        if (n.Next.Connection.Name == name1 && n.Next.Line == c) n.Next = n.Next.Next;
                        if (n.Next != null) n = n.Next;
                    }
                    return true;
                }
                Console.WriteLine("Failed to remove connection. Make sure it's a connection between two stations that exist!");
                return false;
            } 
            

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

                
                // TEST CASE 1 - Inserting five stations

                M.InsertStation("AAA");
                M.InsertStation("BBB");
                M.InsertStation("CCC");
                M.InsertStation("DDD");
                M.InsertStation("EEE");
                M.InsertStation("FFF");
                //M.PrintStations();


                // TEST CASE 2 - Inserting a station that already exists

                //M.InsertStation("AAA");
                //M.PrintStations();


                // TEST CASE 3 - Inserting connections

                M.InsertConnection("AAA", "BBB", Colour.BLUE);
                M.InsertConnection("BBB", "CCC", Colour.BLUE);
                M.InsertConnection("CCC", "BBB", Colour.RED);
                M.InsertConnection("CCC", "DDD", Colour.YELLOW);
                M.InsertConnection("FFF", "EEE", Colour.BLUE);
                M.InsertConnection("EEE", "CCC", Colour.RED);
                M.InsertConnection("DDD", "EEE", Colour.RED);
                M.InsertConnection("BBB", "FFF", Colour.YELLOW);
                M.PrintStations();


                // TEST CASE 4 - Inserting a connection that already exists

                //M.InsertConnection("AAA", "BBB", Colour.BLUE);
                //M.PrintStations();


                // TEST CASE 5 - Inserting a connection between a station that doesn't exist

                //M.InsertConnection("AAA", "fakeStation", Colour.BLUE);
                //M.PrintStations();


                // TEST CASE 6 - Deleting a station

                //Console.WriteLine("Removed Station CCC");
                //M.RemoveStation("CCC");
                //M.PrintStations();


                // TEST CASE 7 - Deleting a connection

                //Console.WriteLine("Deleted EEE, FFF");
                //M.RemoveConnection("EEE", "FFF", Colour.BLUE);
                //M.PrintStations();


                // TEST CASE 8 - Deleting a connection between a station that doesn't exist

                //M.RemoveConnection("EEE", "fakeStation", Colour.BLUE);
                //M.PrintStations();


                Console.WriteLine("");


                // TEST CASE 9a - Finding the shortest path from Point A to Point B

                Console.WriteLine("The shortest route from AAA to FFF is:");
                M.ShortestRoute("AAA", "FFF");      // the shortest route should be AAA -> BBB -> FFF


                Console.WriteLine("");


                // TEST CASE 9b

                Console.WriteLine("The shortest route from DDD to AAA is:");
                M.ShortestRoute("DDD", "AAA");      // the shortest route should be DDD -> CCC -> BBB -> AAA

                Console.WriteLine("");
                Console.WriteLine("");

            }
        }
    }
}