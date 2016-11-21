using OptimizingCompilers2016.Library.ThreeAddressCode.Values.Base;

namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    using Values;

    public class ThreeAddressCode : IThreeAddressCode
    {
        public StringValue Destination { get; set; }
        public LabelValue Label { get; set; }
        public IValue LeftOperand { get; set; }
        public Operation Operation { get; set; }
        public IValue RightOperand { get; set; }
    }
}
