circuit TopHalfAdder :
  module HalfAdder :
    input clk : Clock
    input reset : UInt<1>
    input io_a : UInt<1>
    input io_b : UInt<1>
    output io_s : UInt<1>
    output io_c : UInt<1>
  
    skip
    io_s <= and(io_a, io_b)
    io_c <= xor(io_a, io_b)

  module TopHalfAdder :
    input clk : Clock
    input reset : UInt<1>
  
    inst halfadder of HalfAdder
    halfadder.io_a is invalid
    halfadder.io_b is invalid
    halfadder.clk <= clk
    halfadder.reset <= reset
