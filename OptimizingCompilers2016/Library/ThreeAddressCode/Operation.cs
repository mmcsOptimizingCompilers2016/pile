namespace OptimizingCompilers2016.Library.ThreeAddressCode
{
    public enum Operation {
        NoOp,
        Assign,
        Plus,
        Minus,
        Mult,
        Div,
        Less,
        LessOrEq,
        Eq,
        NotEq,
        Great,
        GreatOrEq,
        Goto,
        CondGoto
    };
}