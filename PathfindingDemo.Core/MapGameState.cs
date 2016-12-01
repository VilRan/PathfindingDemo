using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public abstract class MapGameState : GameState
    {
        public readonly PathfindingDemoGame Game;

        public Map Map { get { return Game.Map; } }

        public MapGameState(PathfindingDemoGame game)
        {
            Game = game;
        }

        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.D1))
                Game.State = new EditorGameState(Game);
            else if (keyboard.IsKeyDown(Keys.D2))
                Game.State = new DijkstraGameState(Game);
            else if (keyboard.IsKeyDown(Keys.D3))
                Game.State = new DijkstraOptimizedGameState(Game);
            else if (keyboard.IsKeyDown(Keys.D4))
                Game.State = new AStarGameState(Game);
            else if (keyboard.IsKeyDown(Keys.D5))
                Game.State = new AStarOptimizedGameState(Game);
        }

        public override void Draw()
        {
            SpriteBatch spriteBatch = Game.SpriteBatch;
            Texture2D texture = Game.Texture;
            int width = Map.Width;
            int height = Map.Height;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Node node = Map.GetNodeAt(x, y);

                    Color color = Color.ForestGreen;
                    if (node.Cost >= Node.Water)
                        color = Color.Blue;
                    else if (node.Cost >= Node.Forest)
                        color = Color.DarkGreen;

                    Rectangle destination = new Rectangle(Map.TileSize * x, Map.TileSize * y, Map.TileSize - 1, Map.TileSize - 1);
                    spriteBatch.Draw(texture, destination, color);
                }
        }
    }
}
