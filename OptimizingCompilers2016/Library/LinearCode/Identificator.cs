namespace OptimizingCompilers2016.Library.LinearCode
{
    public class Identificator : Value
    {
        public Identificator(string i) { id = i; }
        public string id;

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Identificator)
            {
                return ((Identificator)obj).id == this.id;
            }
            return false;
        }
        public override string ToString() { return id; }
    }
}