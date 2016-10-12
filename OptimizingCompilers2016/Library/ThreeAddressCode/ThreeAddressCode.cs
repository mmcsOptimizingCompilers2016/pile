namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    using Values;

    public abstract class ThreeAddressCode : IThreeAddressCode
    {
        public IndentificatorValue Destination { get; set; }
        public LabelValue Label { get; set; }
        public IValue LeftOperand { get; set; }
        public Operation Operation { get; set; }
        public IValue RightOperand { get; set; }
    }
}
