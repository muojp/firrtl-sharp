using FirrtlSharp;

namespace FirrtlSharpHalfAdder
{
    class HalfAdder : Module
    {
        // public override Bundle io { get; } = new Bundle
        // {
        //     ["a"] = new UInt(isOut: false),
        //     ["b"] = new UInt(isOut: false),
        //     ["s"] = new UInt(isOut: true),
        //     ["c"] = new UInt(isOut: true)
        // };

        // public override object io { get; } = new
        // {
        //     a = new UInt(isOut: false),
        //     b = new UInt(isOut: false),
        //     s = new UInt(isOut: true),
        //     c = new UInt(isOut: true)
        // };

        // io["s"] <= io["a"] + io["b"];
        // io["c"] <= io["a"] ^ io["b"];


        public HalfAdder()
        {
            var io = new
            {
                a = new UInt(isOut: false),
                b = new UInt(isOut: false),
                s = new UInt(isOut: true),
                c = new UInt(isOut: true)
            };
            this.io = io;

            io.s.Assign(io.a + io.b);
            io.c.Assign(io.a ^ io.b);
        }
    }

    public class AdderGenerator
    {
        public static void Main()
        {
            FirrtlSharp.Generator.GenerateHDL(new HalfAdder());
        }
    }
}
