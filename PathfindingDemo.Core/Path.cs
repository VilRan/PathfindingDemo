using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    public class Path
    {
        public LinkedList<Node> Nodes = new LinkedList<Node>();
        public List<Node> Open = new List<Node>();
        public List<Node> Closed = new List<Node>();
    }
}
