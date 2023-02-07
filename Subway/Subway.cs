// AUTHORS: Owen Wurtele, Darren Dsilva, Allen Kim
// COIS-3020 Winter 2023 -- Assignment 1


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
        }

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

            public bool RemoveStation(string stationName) {
                
                if (S.ContainsKey(stationName)) 
                {
                    // now we must remove the station-to-be-removed from other stations that have connections to it                    
                        S.Remove(stationName);
                }
                foreach (KeyValuePair<string, Station> kvp in S)
                {
                    Node n = kvp.Value.E;
                    while(n.Next != null)
                    {
                        if (n.Next.Connection.Name == stationName) n.Next = n.Next.Next; 
                        if (n.Next != null) n = n.Next;
                    }
                    return true;
                }
                return false;
            }


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

            public bool RemoveConnection(string name1, string name2, Colour c) 
            {
                if (S.ContainsKey(name1) && S.ContainsKey(name2))
                {
                    Node n = S[name1].E;
                    while (n.Next != null)
                    {
                        if (n.Next.Connection.Name == name2 && n.Next.Line == c) n.Next = n.Next.Next;
                        if (n.Next != null) n = n.Next;
                    }
                    n = S[name2].E;
                    while (n.Next != null)
                    {
                        if (n.Next.Connection.Name == name1 && n.Next.Line == c) n.Next = n.Next.Next;
                        if (n.Next != null) n = n.Next;
                    }
                    return true;
                }
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

                
                // TEST CASE 2 - Inserting five stations

                M.InsertStation("AAA");
                M.InsertStation("BBB");
                M.InsertStation("CCC");
                M.InsertStation("DDD");
                M.InsertStation("EEE");
                M.InsertStation("FFF");
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
                //M.PrintStations();


                // TEST CASE 4 - Deleting a station

                //Console.WriteLine("Removed Station CCC");
                //M.RemoveStation("CCC");
                //M.PrintStations();


                // TEST CASE 5 - Deleting a connection

                Console.WriteLine("Deleted EEE, FFF");
                M.RemoveConnection("EEE", "FFF", Colour.BLUE);
                M.PrintStations();


                // TEST CASE 6 - Finding the shortest path from Point A to Point B

                //M.ShortestRoute("AAA", "FFF");      // the shortest route should be AAA -> BBB -> FFF

                Console.ReadLine();
            }
        }
    }
}