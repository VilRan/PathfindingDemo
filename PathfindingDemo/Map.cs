using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public class Map
    {
        public const int TileSize = 16;

        public readonly PathfindingDemoGame Game;
        Node[,] grid;

        public Rectangle Bounds { get { return new Rectangle(0, 0, Width, Height); } }
        public int Width { get { return grid.GetLength(0); } }
        public int Height { get { return grid.GetLength(1); } }

        public Map(PathfindingDemoGame game, int width, int height)
        {
            Game = game;
            grid = new Node[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Node(x, y);
                }

            foreach (Node node in grid)
            {
                node.UpdateNeighbors(this);
            }
        }

        public Node GetNodeAt(int x, int y)
        {
            return grid[x, y];
        }

        public Node GetNodeAt(Point point)
        {
            return grid[point.X, point.Y];
        }

        public Node TryGetNodeAt(int x, int y)
        {
            if (Bounds.Contains(new Point(x, y)))
                return grid[x, y];
            return null;
        }

        public Node TryGetNodeAt(Point point)
        {
            if (Bounds.Contains(point))
                return grid[point.X, point.Y];
            return null;
        }
    }
}
