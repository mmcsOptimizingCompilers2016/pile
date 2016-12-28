using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace OptimizingCompilers2016.Library
{
    public class EdgeTypes: Dictionary<Edge<BaseBlock>, EdgeType>
    {
        public override string ToString()
        {
            return string.Join("\n", this.Select(ed => $"[{ed.Key.Source.Name} -> {ed.Key.Target.Name}]: {ed.Value}"));
        }
    }
}
