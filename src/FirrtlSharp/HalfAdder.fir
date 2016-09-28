circuit TopHalfAdder :
  module HalfAdder :
    input clk : Clock
    input reset : UInt<1>
    output io : { flip a: UInt<1>, flip b: UInt<1>, s: UInt<1>, c: UInt<1> }

    io is invalid
    io.s <= and(io.aaaaaaa, io.b) @["/Users/muo/workspace/firrtl-sharp/src/FirrtlSharp/HalfAdder.cs: 38"]
    io.c <= xor(io.a, io.b) @["/Users/muo/workspace/firrtl-sharp/src/FirrtlSharp/HalfAdder.cs: 39"]

  module TopHalfAdder :
    input clk : Clock
    input reset : UInt<1>
    inst halfadder of HalfAdder
    halfadder.io is invalid
    halfadder.clk <= clk
    halfadder.reset <= reset
