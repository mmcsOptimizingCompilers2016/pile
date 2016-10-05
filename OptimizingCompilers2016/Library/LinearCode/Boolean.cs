namespace OptimizingCompilers2016.Library.LinearCode
{
    public class Boolean : Value
    {
        public Boolean(bool b) { boolean = b; }
        public bool boolean;
        public override string ToString() { return boolean.ToString(); }
    }
}