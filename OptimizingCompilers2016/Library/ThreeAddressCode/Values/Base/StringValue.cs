namespace OptimizingCompilers2016.Library.ThreeAddressCode.Values.Base
{
    public abstract class StringValue: BaseValue<string>
    {
        protected StringValue(string value) : base(value)
        {
        }
    }
}
