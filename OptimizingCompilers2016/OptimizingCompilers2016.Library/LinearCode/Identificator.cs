namespace OptimizingCompilers2016.Library.LinearCode
{
    public class Identificator : Value
    {
        public Identificator(string i) { id = i; }
        public string id;

        public override string ToString() { return id; }
    }
}