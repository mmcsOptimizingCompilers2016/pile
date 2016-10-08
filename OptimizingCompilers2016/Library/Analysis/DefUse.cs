using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.LinearCode.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Occurrence = System.Tuple<int, OptimizingCompilers2016.Library.LinearCode.Identificator>;
using DefsMap = System.Collections.Generic.Dictionary<OptimizingCompilers2016.Library.LinearCode.Identificator, System.Tuple<int, OptimizingCompilers2016.Library.LinearCode.Identificator>>;

namespace OptimizingCompilers2016.Library.Analysis
{

    public class InblockDefUse
    {
        Dictionary<Occurrence, HashSet<Occurrence>> result = new Dictionary<Occurrence, HashSet<Occurrence>>();
        DefsMap lastDef = new DefsMap();

        private void setLastDef(Identificator variable, Occurrence occurrence)
        {
            if ( !lastDef.ContainsKey(variable) )
            {
                lastDef.Add(variable, occurrence);
            }
            else
            {
                lastDef[variable] = occurrence;
            }
            result.Add(lastDef[variable], new HashSet<Occurrence>());
        }

        private bool checkLastDefs(Identificator occ)
        {
            if (!lastDef.ContainsKey(occ))
            {
                Console.WriteLine("Warning: There isn't such variable: " + occ.ToString());
                return false;
            }
            else
                return true;
        }

        private void addUse(InstructionTerm term, int index)
        {
            if (!(term is Identificator))
                return;

            var variable = term as Identificator;

            if ( checkLastDefs(variable))
            {
                result[lastDef[variable]].Add(new Occurrence(index, variable));
            }
        }


        public Dictionary<Occurrence, HashSet<Occurrence>> runAnalys(List<LinearRepresentation> code)
        {
            
            int index = 0;
            foreach ( var line in code )
            {
                addUse(line.leftOperand, index);
                addUse(line.rightOperand, index);

                if (line.destination is Identificator)
                    setLastDef(line.destination as Identificator, new Occurrence(index, line.destination as Identificator));

                index++;
            }

            return result;
        }
    }

    public class GlobalDefUse
    {

    }
}
