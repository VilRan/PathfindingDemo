using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public class DijkstraGameState : PathfinderGameState
    {
        protected override string Name { get { return "Dijkstra Mode"; } }

        public DijkstraGameState(PathfindingDemoGame game) 
            : base(game)
        {

        }

        protected override Path FindPath(Node start, Node destination)
        {
            return start.FindDijkstraPath(destination);
        }
    }
}
