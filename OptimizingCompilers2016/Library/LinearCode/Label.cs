using OptimizingCompilers2016.Library.LinearCode.Base;

namespace OptimizingCompilers2016.Library.LinearCode
{
    public class Label : InstructionTerm
    {
        public Label(string s) { label = s; }
        public string label;

        public override string ToString() { return label; }
    }
}