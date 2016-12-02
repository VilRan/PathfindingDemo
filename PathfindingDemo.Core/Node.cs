using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public class Node : IComparable<Node>
    {
        public const double Grass = 1;
        public const double Forest = 10;
        public const double Water = 1000;
        
        static int currentPathfinderRun = 0;

        public readonly int X, Y;
        public List<Node> Neighbors = new List<Node>();
        public double Cost = 1;

        NodeStatus status = NodeStatus.Unvisited;
        Node previous = null;
        double pathCost = 0f;
        double heuristic = 0f;
        int lastVisit;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Path FindDijkstraPath(Node destination)
        {
            return FindShortestPath(destination, (n, d) => { return 0; });
        }
        
        public Path FindAStarPath(Node destination)
        {
            return FindShortestPath(destination, computeHeuristic);
        }

        public Path FindShortestPath(Node destination, Func<Node, Node, double> heuristicFunction)
        {
            Path path = new Path();

            currentPathfinderRun++;
            lastVisit = currentPathfinderRun;
            previous = null;

            List<Node> closed = new List<Node>();
            List<Node> open = new List<Node>();
            open.Add(this);

            while (open.Count > 0)
            {
                open.Sort();
                Node active = open.First();
                open.RemoveAt(0);
                active.status = NodeStatus.Closed;
                closed.Add(active);

                if (active == destination)
                {
                    path = buildPath(destination);
                    break;
                }

                foreach (Node neighbor in active.Neighbors)
                {
                    if (neighbor.lastVisit != currentPathfinderRun)
                    {
                        neighbor.status = NodeStatus.Unvisited;
                        neighbor.lastVisit = currentPathfinderRun;
                    }

                    if (neighbor.status != NodeStatus.Closed)
                    {
                        double cost = active.pathCost + active.computeCost(neighbor);

                        if (neighbor.status != NodeStatus.Open)
                        {
                            neighbor.previous = active;
                            neighbor.pathCost = cost;
                            neighbor.heuristic = heuristicFunction(neighbor, destination);
                            neighbor.status = NodeStatus.Open;
                            open.Add(neighbor);
                        }
                        else if (cost < neighbor.pathCost)
                        {
                            neighbor.previous = active;
                            neighbor.pathCost = cost;
                        }
                    }
                }
            }

            path.Open = open.ToList();
            path.Closed = closed;
            return path;
        }

        public Path FindDijkstraPathOptimized(Node destination)
        {
            return FindShortestPathOptimized(destination, (n, d) => { return 0; });
        }

        public Path FindAStarPathOptimized(Node destination)
        {
            return FindShortestPathOptimized(destination, computeHeuristic);
        }

        public Path FindShortestPathOptimized(Node destination, Func<Node, Node, double> heuristicFunction)
        {
            Path path = new Path();

            currentPathfinderRun++;
            lastVisit = currentPathfinderRun;
            previous = null;

            List<Node> closed = new List<Node>();
            BinaryHeap<Node> open = new BinaryHeap<Node>();
            open.Add(this);

            while (open.Count > 0)
            {
                Node active = open.Remove();
                active.status = NodeStatus.Closed;
                closed.Add(active);
                if (active == destination)
                    break;

                foreach (Node neighbor in active.Neighbors)
                {
                    if (neighbor.lastVisit != currentPathfinderRun)
                    {
                        neighbor.status = NodeStatus.Unvisited;
                        neighbor.lastVisit = currentPathfinderRun;
                    }

                    if (neighbor.status != NodeStatus.Closed)
                    {
                        double cost = active.pathCost + active.computeCost(neighbor);

                        if (neighbor.status == NodeStatus.Unvisited)
                        {
                            neighbor.previous = active;
                            neighbor.pathCost = cost;
                            neighbor.heuristic = heuristicFunction(neighbor, destination);
                            neighbor.status = NodeStatus.Open;
                            open.Add(neighbor);
                        }
                        else if (cost < neighbor.pathCost)
                        {
                            neighbor.previous = active;
                            neighbor.pathCost = cost;
                            open.Reorder(neighbor);
                        }
                    }
                }
            }

            path = buildPath(destination);
            path.Open = open.ToList();
            path.Closed = closed;
            return path;
        }

        public void UpdateNeighbors(Map map)
        {
            Neighbors.Clear();

            int minX = Math.Max(X - 1, 0);
            int maxX = Math.Min(X + 1, map.Width - 1);
            int minY = Math.Max(Y - 1, 0);
            int maxY = Math.Min(Y + 1, map.Height - 1);
            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                {
                    Node neighbor = map.GetNodeAt(x, y);
                    if (neighbor != this)
                    {
                        Neighbors.Add(neighbor);
                    }
                }
        }

        public int CompareTo(Node other)
        {
            double valueSelf = pathCost + heuristic;
            double valueOther = other.pathCost + other.heuristic;
            if (valueSelf > valueOther)
                return 1;
            if (valueSelf < valueOther)
                return -1;
            return 0;
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y;
        }

        Path buildPath(Node destination)
        {
            Path path = new Path();
            Node active = destination;
            while (active != null)
            {
                path.Nodes.AddFirst(active);
                active = active.previous;
            }
            return path;
        }

        double computeCost(Node destination)
        {
            int deltaX = destination.X - X;
            int deltaY = destination.Y - Y;
            if (deltaX * deltaX + deltaY * deltaY > 1)
                return destination.Cost * 1.5;
            return destination.Cost;
        }

        double computeHeuristic(Node destination)
        {
            int deltaX = Math.Abs(destination.X - X);
            int deltaY = Math.Abs(destination.Y - Y);
            return deltaX + deltaY - 0.5 * Math.Min(deltaX, deltaY);
        }

        static double computeHeuristic(Node neighbor, Node destination)
        {
            int deltaX = Math.Abs(destination.X - neighbor.X);
            int deltaY = Math.Abs(destination.Y - neighbor.Y);
            return deltaX + deltaY - 0.5 * Math.Min(deltaX, deltaY);
        }
    }

    public enum NodeStatus
    {
        Unvisited, Open, Closed
    }
}
