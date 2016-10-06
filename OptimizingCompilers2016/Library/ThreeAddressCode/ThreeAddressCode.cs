namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    using Values;

    public abstract class ThreeAddressCode : IThreeAddressCode
    {
        public object Destination { get; set; }
        public LabelValue Label { get; set; }
        public object LeftOperand { get; set; }
        public Operation Operation { get; set; }
        public object RightOperand { get; set; }
    }
}
