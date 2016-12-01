using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public static class SpriteBatchExtensions
    {
        public static void DrawStringWithShadow(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position)
        {
            spriteBatch.DrawString(font, text, position + new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(font, text, position, Color.White);
        }
    }
}
