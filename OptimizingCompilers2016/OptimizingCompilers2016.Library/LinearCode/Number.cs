namespace OptimizingCompilers2016.Library.LinearCode
{
    public class Number : Value
    {
        public Number(int n) { number = n; }
        public int number;

        public override string ToString() { return number.ToString(); }
    }
}