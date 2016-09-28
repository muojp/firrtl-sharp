`ifdef RANDOMIZE_GARBAGE_ASSIGN
`define RANDOMIZE
`endif
`ifdef RANDOMIZE_INVALID_ASSIGN
`define RANDOMIZE
`endif
`ifdef RANDOMIZE_REG_INIT
`define RANDOMIZE
`endif
`ifdef RANDOMIZE_MEM_INIT
`define RANDOMIZE
`endif

module HalfAdder(
  input   clk,
  input   reset,
  input   io_a,
  input   io_b,
  output  io_s,
  output  io_c
);
  assign io_s = io_a & io_b;
  assign io_c = io_a ^ io_b;
endmodule
module TopHalfAdder(
  input   clk,
  input   reset
);
  wire  halfadder_clk;
  wire  halfadder_reset;
  wire  halfadder_io_a;
  wire  halfadder_io_b;
  wire  halfadder_io_s;
  wire  halfadder_io_c;
  reg  GEN_0;
  reg [31:0] GEN_2;
  reg  GEN_1;
  reg [31:0] GEN_3;
  HalfAdder halfadder (
    .clk(halfadder_clk),
    .reset(halfadder_reset),
    .io_a(halfadder_io_a),
    .io_b(halfadder_io_b),
    .io_s(halfadder_io_s),
    .io_c(halfadder_io_c)
  );
  assign halfadder_clk = clk;
  assign halfadder_reset = reset;
  assign halfadder_io_a = GEN_0;
  assign halfadder_io_b = GEN_1;
`ifdef RANDOMIZE
  integer initvar;
  initial begin
    `ifndef verilator
      #0.002 begin end
    `endif
  `ifdef RANDOMIZE_REG_INIT
  GEN_2 = {1{$random}};
  GEN_0 = GEN_2[0:0];
  `endif
  `ifdef RANDOMIZE_REG_INIT
  GEN_3 = {1{$random}};
  GEN_1 = GEN_3[0:0];
  `endif
  end
`endif
endmodule
