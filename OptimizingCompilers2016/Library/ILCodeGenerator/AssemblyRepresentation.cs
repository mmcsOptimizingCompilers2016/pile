using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace OptimizingCompilers2016.Library.ILCodeGenerator
{
    class AssemblyRepresentation : IAssemblyRepresentation
    {
        private MethodBuilder _entryPoint;

        AssemblyRepresentation(MethodBuilder entryPoint)
        {
            _entryPoint = entryPoint;
        }

        public void PrintILCode()
        {

        }

        public void Save(string filename)
        {

        }
    }
}
