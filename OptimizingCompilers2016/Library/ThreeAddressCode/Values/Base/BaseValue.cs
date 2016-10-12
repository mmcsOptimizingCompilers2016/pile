namespace OptimizingCompilers2016.Library.ThreeAddressCode.Values.Base
{
    public abstract class BaseValue<T>: IValue
    {
        protected BaseValue(T value)
        {
            _value = value;
        }

        private T _value;

        public object Value
        {
            get { return _value; }

            set { _value = (T) value; }
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
