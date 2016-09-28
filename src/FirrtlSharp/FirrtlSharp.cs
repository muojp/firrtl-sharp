using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;

namespace FirrtlSharp
{
    public class Generator
    {
        public static void GenerateHDL(Module topLevelMod)
        {
            var outputFilename = $"{topLevelMod.GetType().Name}.fir";
            var dumped = topLevelMod.Dump();
            Console.WriteLine(dumped);
            File.WriteAllText(outputFilename, dumped);
        }
    }

    public class Bundle : Dictionary<string, UInt>
    {

    }

    public class Node
    {
        public static Tuple<string, int> GetCallstackIn(int idx)
        {
            var actualIdx = 3 + idx;
            var trace = Environment.StackTrace;
            var lines = trace.Split('\n');
            if (lines.Length < actualIdx)
            {
                throw new Exception("callstack too shallow");
            }
            var entry = lines[actualIdx];
            var m = entry.Split(new string[] { " in " }, StringSplitOptions.None);
            var x = m[1].Split(new string[] { ":line " }, StringSplitOptions.None);
            // Console.WriteLine(trace);
            return new Tuple<string, int>(x[0], Int32.Parse(x[1]));
        }
    }

    public enum OpType
    {
        Add,
        Xor
    };

    public class UInt : Node
    {
        public string Label { get; set; }
        public string SourceFilename { get; set; }
        public int SourceLine { get; set; }

        public UInt(bool isOut = false)
        {
            this.IsOut = isOut;
        }

        public UInt Assigned = null;
        public void Assign(UInt n)
        {
            Assigned = n;
        }
        public UInt OpA;
        public UInt OpB;
        public OpType Op;

        private UInt Add(UInt target)
        {
            var node = new UInt();
            var m = GetCallstackIn(2);
            node.OpA = this;
            node.OpB = target;
            node.Op = OpType.Add;
            node.SourceFilename = m.Item1;
            node.SourceLine = m.Item2;
            return node;
        }

        private UInt Xor(UInt target)
        {
            var node = new UInt();
            var m = GetCallstackIn(2);
            node.OpA = this;
            node.OpB = target;
            node.Op = OpType.Xor;
            node.SourceFilename = m.Item1;
            node.SourceLine = m.Item2;
            return node;
        }

        public bool IsOut;

        public static UInt operator +(UInt a, UInt b)
        {
            return a.Add(b);
        }

        public static UInt operator ^(UInt a, UInt b)
        {
            return a.Xor(b);
        }
    }
    public abstract class Module
    {
        // protected readonly object io;
        // public Module(object io)
        // {
        //     this.io = io;
        // }
        protected object io;

        public string Dump()
        {
            var ti = this.io.GetType();
            var entities = new List<UInt>();
            var b = new StringBuilder();
            // lookup io ports
            foreach (var i in ti.GetMethods().Where(o => o.Name.StartsWith("get_")))
            {
                var ident = i.Name.Substring(4);
                var entity = i.Invoke(this.io, new object[] { }) as UInt;
                if (entity == null)
                {
                    continue;
                }
                entity.Label = ident;
                entities.Add(entity);
                // Console.WriteLine($"{ident}");
                // if (entity.Assigned != null)
                // {
                //     Console.WriteLine("assigned entity!");
                // }
            }
            // generate top level circuit
            var moduleName = this.GetType().Name;
            var instName = moduleName.ToLower();
            b.Append($"circuit Top{moduleName} :\n");
            b.Append($"  module {moduleName} :\n");
            b.Append("    input clk : Clock\n");
            b.Append("    input reset : UInt<1>\n");
            // generate IO port list
            var ports = string.Join(", ", entities.Select(o => (o.IsOut ? "" : "flip ") + $"{o.Label}: UInt<1>"));
            b.Append("    output io : { " + ports + " }\n");
            b.Append("\n");
            b.Append("    io is invalid\n");

            // generate combinational circuits
            foreach (var e in entities.Where(o => o.Assigned != null))
            {
                string opLabel;
                var o = e.Assigned;
                switch (o.Op)
                {
                    case OpType.Add: opLabel = "and"; break;
                    case OpType.Xor: opLabel = "xor"; break;
                    default: opLabel = ""; break;
                }
                // TODO: handle wire/reg (currently handles only io.xxxx)
                b.Append($"    io.{e.Label} <= {opLabel}(io.{o.OpA.Label}, io.{o.OpB.Label}) @[\"{o.SourceFilename}: {o.SourceLine}\"]\n");
            }
            // add top level module
            b.Append("\n");
            b.Append($"  module Top{moduleName} :\n");
            b.Append("    input clk : Clock\n");
            b.Append("    input reset : UInt<1>\n");
            b.Append($"    inst {instName} of {moduleName}\n");
            b.Append($"    {instName}.io is invalid\n");
            b.Append($"    {instName}.clk <= clk\n");
            b.Append($"    {instName}.reset <= reset\n");
            return b.ToString();
        }
    }
}
