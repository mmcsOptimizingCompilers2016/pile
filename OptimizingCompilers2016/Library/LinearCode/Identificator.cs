namespace OptimizingCompilers2016.Library.LinearCode
{
    public class Identificator : Value
    {
        public Identificator(string i) { id = i; }
        public string id;

        public override string ToString() { return id; }

        public override bool Equals(object obj)
        {
            return obj.ToString().Equals(ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
