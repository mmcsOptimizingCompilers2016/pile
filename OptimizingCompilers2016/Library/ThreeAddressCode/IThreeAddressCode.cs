namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    public interface IThreeAddressCode
    {
        Operation Operation { get; set; }
        object LeftOperand { get; set; }
        object RightOperand { get; set; }
        object Destination { get; set; }
        object Label { get; set; }
    }
}
