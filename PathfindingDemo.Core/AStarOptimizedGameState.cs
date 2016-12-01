using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    class AStarOptimizedGameState : PathfinderGameState
    {
        protected override string Name { get { return "A* Mode (Optimized)"; } }

        public AStarOptimizedGameState(PathfindingDemoGame game) 
            : base(game)
        {

        }

        protected override Path FindPath(Node start, Node destination)
        {
            return start.FindAStarOptimizedPath(destination);
        }
    }
}
