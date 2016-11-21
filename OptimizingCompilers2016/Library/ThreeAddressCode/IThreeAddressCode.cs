using OptimizingCompilers2016.Library.ThreeAddressCode.Values.Base;

namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    using Values;
    public interface IThreeAddressCode
    {
        Operation Operation { get; set; }
        IValue LeftOperand { get; set; }
        IValue RightOperand { get; set; }
        StringValue Destination { get; set; }
        LabelValue Label { get; set; }
    }
}
