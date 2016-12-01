using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public abstract class GameState
    {
        public abstract void Update();
        public abstract void Draw();
    }
}
