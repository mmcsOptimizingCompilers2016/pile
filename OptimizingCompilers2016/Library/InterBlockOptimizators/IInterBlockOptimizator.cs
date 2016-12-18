using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizingCompilers2016.Library.InterBlockOptimizators
{
    public interface IInterBlockOptimizator
    {
        bool Optimize(ControlFlowGraph Blocks);
    }
}
