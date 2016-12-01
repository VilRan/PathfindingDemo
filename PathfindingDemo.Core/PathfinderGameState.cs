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
        long time = 0;
        int slowness = 1;

        protected virtual string Name { get; }

        public PathfinderGameState(PathfindingDemoGame game) 
            : base(game)
        {
            previousMouse = Mouse.GetState();
        }

        public override void Update()
        {
            base.Update();

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Q))
                slowness = 1;
            else if (keyboard.IsKeyDown(Keys.W))
                slowness = 10;
            else if (keyboard.IsKeyDown(Keys.E))
                slowness = 100;
            else if (keyboard.IsKeyDown(Keys.R))
                slowness = 1000;

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
                        for (int i = 0; i < slowness; i++)
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
                        for (int i = 0; i < slowness; i++)
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
            }

            spriteBatch.DrawStringWithShadow(font, Name, new Vector2(16, 16));
            spriteBatch.DrawStringWithShadow(font, "Time (x" + slowness + "): " + time + " ms", new Vector2(16, 32));

            int nodesVisited = 0;
            if (path != null)
                nodesVisited = path.Closed.Count;
            spriteBatch.DrawStringWithShadow(font, "Nodes visited: " + nodesVisited, new Vector2(16, 48));
        }

        protected abstract Path FindPath(Node start, Node destination);
    }
}
