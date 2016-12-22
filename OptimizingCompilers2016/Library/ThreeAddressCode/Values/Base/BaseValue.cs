namespace OptimizingCompilers2016.Library.ThreeAddressCode.Values.Base
{
    public abstract class BaseValue<T>: IValue
    {
        protected BaseValue(T value)
        {
            Value = value;
        }

        public T Value { get; }

        object IValue.Value => Value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            var baseValue = obj as BaseValue<T>;
            return baseValue != null && Value.Equals(baseValue.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
