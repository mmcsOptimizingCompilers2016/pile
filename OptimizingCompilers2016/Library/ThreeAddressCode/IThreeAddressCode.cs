namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    using Values;
    public interface IThreeAddressCode
    {
        Operation Operation { get; set; }
        IValue LeftOperand { get; set; }
        IValue RightOperand { get; set; }
        IdentificatorValue Destination { get; set; }
        LabelValue Label { get; set; }
    }
}
