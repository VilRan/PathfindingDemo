using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    class AStarGameState : PathfinderGameState
    {
        protected override string Name { get { return "A* Mode"; } }

        public AStarGameState(PathfindingDemoGame game) 
            : base(game)
        {

        }

        protected override Path FindPath(Node start, Node destination)
        {
            return start.FindAStarPath(destination);
        }
    }
}
