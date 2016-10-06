namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    using Values;
    public interface IThreeAddressCode
    {
        Operation Operation { get; set; }
        object LeftOperand { get; set; }
        object RightOperand { get; set; }
        object Destination { get; set; }
        LabelValue Label { get; set; }
    }
}
