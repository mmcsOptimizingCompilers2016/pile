using System.ComponentModel;

namespace OptimizingCompilers2016.Library.Nodes
{
    public enum BinSign
    {
        [Description("<")]
        LS,
        [Description(">")]
        GT,
        [Description("<=")]
        LE,
        [Description(">=")]
        GE,
        [Description("==")]
        EQ,
        [Description("!=")]
        NE,
        [Description("+")]
        PLUS,
        [Description("-")]
        MINUS,
        [Description("*")]
        MULT,
        [Description("/")]
        DIV
    }
}