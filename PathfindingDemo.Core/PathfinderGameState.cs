using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public abstract class PathfinderGameState : MapGameState
    {
        MouseState previousMouse;
        Node start;
        Node destination;
        Path path;
        long time;

        protected virtual string Name { get; }

        public PathfinderGameState(PathfindingDemoGame game) 
            : base(game)
        {
            previousMouse = Mouse.GetState();
        }

        public override void Update()
        {
            base.Update();

            MouseState mouse = Mouse.GetState();
            Point point = new Point(mouse.X / Map.TileSize, mouse.Y / Map.TileSize);
            Node node = Map.TryGetNodeAt(point);
            if (node != null)
            {
                if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
                {
                    start = node;
                    if (destination != null)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        path = FindPath(start, destination);
                        stopwatch.Stop();
                        time = stopwatch.ElapsedMilliseconds;
                    }
                }
                else if (mouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released)
                {
                    destination = node;
                    if (start != null)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        path = FindPath(start, destination);
                        stopwatch.Stop();
                        time = stopwatch.ElapsedMilliseconds;
                    }
                }
            }

            previousMouse = mouse;
        }

        public override void Draw()
        {
            base.Draw();

            SpriteBatch spriteBatch = Game.SpriteBatch;
            SpriteFont font = Game.Font;
            if (path != null)
            {
                Texture2D texture = Game.Texture;
                foreach (Node node in path.Closed)
                {
                    Rectangle destination = new Rectangle(Map.TileSize * node.X, Map.TileSize * node.Y, Map.TileSize, Map.TileSize);
                    Color color = Color.White * 0.25f;
                    spriteBatch.Draw(texture, destination, color);
                }
                foreach (Node node in path.Open)
                {
                    if (node != null)
                    {
                        Rectangle destination = new Rectangle(Map.TileSize * node.X, Map.TileSize * node.Y, Map.TileSize, Map.TileSize);
                        Color color = Color.White * 0.5f;
                        spriteBatch.Draw(texture, destination, color);
                    }
                }
                foreach (Node node in path.Nodes)
                {
                    Rectangle destination = new Rectangle(Map.TileSize * node.X, Map.TileSize * node.Y, Map.TileSize, Map.TileSize);
                    Color color = Color.White * 0.75f;
                    if (node == path.Nodes.First())
                        color = Color.GreenYellow * 0.75f;
                    else if (node == path.Nodes.Last())
                        color = Color.Red * 0.75f;
                    spriteBatch.Draw(texture, destination, color);
                }
                spriteBatch.DrawStringWithShadow(font, "Time: " + time + " ms", new Vector2(16, 32));
                spriteBatch.DrawStringWithShadow(font, "Nodes visited: " + path.Closed.Count, new Vector2(16, 48));
                spriteBatch.DrawStringWithShadow(font, "Heap sorted: " + Node.SortCount + " times", new Vector2(16, 64));
            }

            spriteBatch.DrawStringWithShadow(font, Name, new Vector2(16, 16));
        }

        protected abstract Path FindPath(Node start, Node destination);
    }
}
