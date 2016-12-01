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
    public class EditorGameState : MapGameState
    {
        double terrain = Node.Water;
        int brushSize = 0;

        public EditorGameState(PathfindingDemoGame game) 
            : base(game)
        {

        }

        public override void Update()
        {
            base.Update();
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Q))
                terrain = Node.Water;
            else if (keyboard.IsKeyDown(Keys.W))
                terrain = Node.Forest;
            else if (keyboard.IsKeyDown(Keys.A))
                brushSize = 0;
            else if (keyboard.IsKeyDown(Keys.S))
                brushSize = 1;
            else if (keyboard.IsKeyDown(Keys.D))
                brushSize = 2;
            else if (keyboard.IsKeyDown(Keys.F))
                brushSize = 3;

            MouseState mouse = Mouse.GetState();
            Point point = new Point(mouse.X / Map.TileSize, mouse.Y / Map.TileSize);
            Point min = point - new Point(brushSize, brushSize);
            Point max = point + new Point(brushSize, brushSize);
            for (int x = min.X; x <= max.X; x++)
                for (int y = min.Y; y <= max.Y; y++)
                {
                    Node node = Map.TryGetNodeAt(x, y);
                    if (node != null)
                    {
                        if (mouse.LeftButton == ButtonState.Pressed)
                            node.Cost = terrain;
                        else if (mouse.RightButton == ButtonState.Pressed)
                            node.Cost = Node.Grass;
                    }
                }
        }

        public override void Draw()
        {
            base.Draw();

            SpriteBatch spriteBatch = Game.SpriteBatch;
            SpriteFont font = Game.Font;
            
            spriteBatch.DrawStringWithShadow(font, "Editor Mode", new Vector2(16, 16));
            if (terrain == Node.Water)
            {
                spriteBatch.DrawStringWithShadow(font, "Water", new Vector2(16, 32));
            }
            else if (terrain == Node.Forest)
            {
                spriteBatch.DrawStringWithShadow(font, "Forest", new Vector2(16, 32));
            }
            spriteBatch.DrawStringWithShadow(font, "Brush size: " + brushSize, new Vector2(16, 48));
        }
    }
}
