﻿namespace OptimizingCompilers2016.Library.ThreeAddressCode.Values
{
    using Base;

    public class LabelValue: BaseValue<string>
    {
        public LabelValue(string value) : base(value)
        {
        }
    }
}