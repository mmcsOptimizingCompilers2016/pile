using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizingCompilers2016.Library.ILCodeGenerator
{
    interface IAssemblyRepresentation
    {
        void PrintILCode();
        void Save(string filename);
    }
}
