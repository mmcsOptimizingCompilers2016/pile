namespace OptimizingCompilers2016.Library.ThreeAddressCode.Base
{
    public abstract class BaseThreeAddressCode : IThreeAddressCode
    {
        public abstract object Destination { get; set; }
        public abstract object Label { get; set; }
        public abstract object LeftOperand { get; set; }
        public abstract Operation Operation { get; set; }
        public abstract object RightOperand { get; set; }
    }
}
