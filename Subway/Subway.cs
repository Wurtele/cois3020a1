using System;

public enum Colour { RED, YELLOW, GREEN, … } // For example
public class SubwayMap
{
    private class Node
    {
        public Station connection; // Adjacent station (connection)
        public Colour line; // Colour of its subway line
        public Node next; // Link to the next adjacent station (Node)
        public Node() { … }
        public Node(Station connection, Colour c, Node next) { … }
    }

    private class Station
    {
        public string name; // Name of the subway station
        public bool visited; // Used for the breadth-first search
        public Node E; // Header node for a linked list of adjacent stations
        public Station(string name) { … }
    }

    private Dictionary<string, Station> S; // Dictionary of stations
    public SubwayMap() { … }

    public void InsertStation(string name) { … }

    public bool RemoveStation(string name) { … }

    public bool InsertConnection(string name1, string name2, Colour c) { … }

    public bool RemoveConnection(string name1, string name2, Colour c) { … }

    public void ShortestRoute(string name1, string name2) { … }

}