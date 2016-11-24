// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  MIL8A-315-03
// DateTime: ?? 20.10.16 14:40:56
// UserName: User
// Input file <SimpleYacc.y>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using OptimizingCompilers2016.Library.Nodes;
using OptimizingCompilers2016.Library.Nodes.Base;

namespace OptimizingCompilers2016.Library
{
public enum Tokens {
    error=1,EOF=2,BEGIN=3,END=4,CYCLE=5,ASSIGN=6,
    SEMICOLON=7,LBRACE=8,RBRACE=9,PLUS=10,MINUS=11,MULT=12,
    DIV=13,COMMA=14,IF=15,THEN=16,ELSE=17,FOR=18,
    TO=19,DO=20,REPEAT=21,UNTIL=22,WHILE=23,LS=24,
    GT=25,LE=26,GE=27,EQ=28,NE=29,INUM=30,
    RNUM=31,ID=32,IFX=33};

public struct ValueType
{
		public double dVal;
		public int iVal;
		public string sVal;
		public BaseNode nVal;
		public ExprNode eVal;
		public StatementNode stVal;
		public BlockNode blVal;
		public BinSign bsVal;
}
// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
  // Verbatim content from SimpleYacc.y
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
	public BlockNode root;
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
  // End verbatim content from SimpleYacc.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[39];
  private static State[] states = new State[68];
  private static string[] nonTerms = new string[] {
      "expr", "ident", "bin_expr", "in_br", "m_d", "common_expr", "assign", "statement", 
      "cycle", "if_st", "rep_unt", "while_st", "for_st", "stlist", "block", "bin_sign", 
      "progr", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{3,4},new int[]{-17,1,-15,3});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-14,5,-8,67,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[5] = new State(new int[]{4,6,32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-8,7,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[6] = new State(-14);
    states[7] = new State(-4);
    states[8] = new State(new int[]{7,9});
    states[9] = new State(-5);
    states[10] = new State(new int[]{6,11});
    states[11] = new State(new int[]{32,18,30,19,8,20},new int[]{-1,12,-5,28,-4,27,-2,17});
    states[12] = new State(new int[]{10,13,11,23,7,-13,19,-13});
    states[13] = new State(new int[]{32,18,30,19,8,20},new int[]{-5,14,-4,27,-2,17});
    states[14] = new State(new int[]{12,15,13,25,10,-31,11,-31,7,-31,19,-31,9,-31,24,-31,25,-31,26,-31,27,-31,28,-31,29,-31,32,-31,3,-31,5,-31,15,-31,18,-31,21,-31,23,-31,20,-31,4,-31,17,-31,22,-31});
    states[15] = new State(new int[]{32,18,30,19,8,20},new int[]{-4,16,-2,17});
    states[16] = new State(-34);
    states[17] = new State(-36);
    states[18] = new State(-12);
    states[19] = new State(-37);
    states[20] = new State(new int[]{32,18,30,19,8,20},new int[]{-1,21,-5,28,-4,27,-2,17});
    states[21] = new State(new int[]{9,22,10,13,11,23});
    states[22] = new State(-38);
    states[23] = new State(new int[]{32,18,30,19,8,20},new int[]{-5,24,-4,27,-2,17});
    states[24] = new State(new int[]{12,15,13,25,10,-32,11,-32,7,-32,19,-32,9,-32,24,-32,25,-32,26,-32,27,-32,28,-32,29,-32,32,-32,3,-32,5,-32,15,-32,18,-32,21,-32,23,-32,20,-32,4,-32,17,-32,22,-32});
    states[25] = new State(new int[]{32,18,30,19,8,20},new int[]{-4,26,-2,17});
    states[26] = new State(-35);
    states[27] = new State(-33);
    states[28] = new State(new int[]{12,15,13,25,10,-30,11,-30,7,-30,19,-30,9,-30,24,-30,25,-30,26,-30,27,-30,28,-30,29,-30,32,-30,3,-30,5,-30,15,-30,18,-30,21,-30,23,-30,20,-30,4,-30,17,-30,22,-30});
    states[29] = new State(-6);
    states[30] = new State(-7);
    states[31] = new State(new int[]{32,18,30,19,8,20},new int[]{-6,32,-3,52,-1,53,-5,28,-4,27,-2,17});
    states[32] = new State(new int[]{32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-8,33,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[33] = new State(-15);
    states[34] = new State(-8);
    states[35] = new State(new int[]{32,18,30,19,8,20},new int[]{-6,36,-3,52,-1,53,-5,28,-4,27,-2,17});
    states[36] = new State(new int[]{32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-8,37,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[37] = new State(new int[]{17,38,4,-16,32,-16,3,-16,5,-16,15,-16,18,-16,21,-16,23,-16,22,-16});
    states[38] = new State(new int[]{32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-8,39,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[39] = new State(-17);
    states[40] = new State(-9);
    states[41] = new State(new int[]{32,18},new int[]{-7,42,-2,10});
    states[42] = new State(new int[]{19,43});
    states[43] = new State(new int[]{32,18,30,19,8,20},new int[]{-1,44,-5,28,-4,27,-2,17});
    states[44] = new State(new int[]{20,45,10,13,11,23});
    states[45] = new State(new int[]{32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-8,46,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[46] = new State(-20);
    states[47] = new State(-10);
    states[48] = new State(new int[]{32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-14,49,-8,67,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[49] = new State(new int[]{22,50,32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-8,7,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[50] = new State(new int[]{32,18,30,19,8,20},new int[]{-6,51,-3,52,-1,53,-5,28,-4,27,-2,17});
    states[51] = new State(-18);
    states[52] = new State(-27);
    states[53] = new State(new int[]{10,13,11,23,24,56,25,57,26,58,27,59,28,60,29,61,32,-28,3,-28,5,-28,15,-28,18,-28,21,-28,23,-28,4,-28,17,-28,22,-28,20,-28},new int[]{-16,54});
    states[54] = new State(new int[]{32,18,30,19,8,20},new int[]{-1,55,-5,28,-4,27,-2,17});
    states[55] = new State(new int[]{10,13,11,23,32,-29,3,-29,5,-29,15,-29,18,-29,21,-29,23,-29,4,-29,17,-29,22,-29,20,-29});
    states[56] = new State(-21);
    states[57] = new State(-22);
    states[58] = new State(-23);
    states[59] = new State(-24);
    states[60] = new State(-25);
    states[61] = new State(-26);
    states[62] = new State(-11);
    states[63] = new State(new int[]{32,18,30,19,8,20},new int[]{-6,64,-3,52,-1,53,-5,28,-4,27,-2,17});
    states[64] = new State(new int[]{20,65});
    states[65] = new State(new int[]{32,18,3,4,5,31,15,35,18,41,21,48,23,63},new int[]{-8,66,-7,8,-2,10,-15,29,-9,30,-10,34,-13,40,-11,47,-12,62});
    states[66] = new State(-19);
    states[67] = new State(-3);

    rules[1] = new Rule(-18, new int[]{-17,2});
    rules[2] = new Rule(-17, new int[]{-15});
    rules[3] = new Rule(-14, new int[]{-8});
    rules[4] = new Rule(-14, new int[]{-14,-8});
    rules[5] = new Rule(-8, new int[]{-7,7});
    rules[6] = new Rule(-8, new int[]{-15});
    rules[7] = new Rule(-8, new int[]{-9});
    rules[8] = new Rule(-8, new int[]{-10});
    rules[9] = new Rule(-8, new int[]{-13});
    rules[10] = new Rule(-8, new int[]{-11});
    rules[11] = new Rule(-8, new int[]{-12});
    rules[12] = new Rule(-2, new int[]{32});
    rules[13] = new Rule(-7, new int[]{-2,6,-1});
    rules[14] = new Rule(-15, new int[]{3,-14,4});
    rules[15] = new Rule(-9, new int[]{5,-6,-8});
    rules[16] = new Rule(-10, new int[]{15,-6,-8});
    rules[17] = new Rule(-10, new int[]{15,-6,-8,17,-8});
    rules[18] = new Rule(-11, new int[]{21,-14,22,-6});
    rules[19] = new Rule(-12, new int[]{23,-6,20,-8});
    rules[20] = new Rule(-13, new int[]{18,-7,19,-1,20,-8});
    rules[21] = new Rule(-16, new int[]{24});
    rules[22] = new Rule(-16, new int[]{25});
    rules[23] = new Rule(-16, new int[]{26});
    rules[24] = new Rule(-16, new int[]{27});
    rules[25] = new Rule(-16, new int[]{28});
    rules[26] = new Rule(-16, new int[]{29});
    rules[27] = new Rule(-6, new int[]{-3});
    rules[28] = new Rule(-6, new int[]{-1});
    rules[29] = new Rule(-3, new int[]{-1,-16,-1});
    rules[30] = new Rule(-1, new int[]{-5});
    rules[31] = new Rule(-1, new int[]{-1,10,-5});
    rules[32] = new Rule(-1, new int[]{-1,11,-5});
    rules[33] = new Rule(-5, new int[]{-4});
    rules[34] = new Rule(-5, new int[]{-5,12,-4});
    rules[35] = new Rule(-5, new int[]{-5,13,-4});
    rules[36] = new Rule(-4, new int[]{-2});
    rules[37] = new Rule(-4, new int[]{30});
    rules[38] = new Rule(-4, new int[]{8,-1,9});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
    switch (action)
    {
      case 2: // progr -> block
{ root = ValueStack[ValueStack.Depth-1].blVal; }
        break;
      case 3: // stlist -> statement
{ CurrentSemanticValue.blVal = new BlockNode(ValueStack[ValueStack.Depth-1].stVal); }
        break;
      case 4: // stlist -> stlist, statement
{
				ValueStack[ValueStack.Depth-2].blVal.Add(ValueStack[ValueStack.Depth-1].stVal);
				CurrentSemanticValue.blVal = ValueStack[ValueStack.Depth-2].blVal;
			}
        break;
      case 5: // statement -> assign, SEMICOLON
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-2].stVal; }
        break;
      case 6: // statement -> block
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].blVal; }
        break;
      case 7: // statement -> cycle
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 8: // statement -> if_st
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 9: // statement -> for_st
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 10: // statement -> rep_unt
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 11: // statement -> while_st
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 12: // ident -> ID
{ CurrentSemanticValue.eVal = new IdNode(ValueStack[ValueStack.Depth-1].sVal); }
        break;
      case 13: // assign -> ident, ASSIGN, expr
{ CurrentSemanticValue.stVal = new AssignNode(ValueStack[ValueStack.Depth-3].eVal as IdNode, ValueStack[ValueStack.Depth-1].eVal); }
        break;
      case 14: // block -> BEGIN, stlist, END
{ CurrentSemanticValue.blVal = ValueStack[ValueStack.Depth-2].blVal; }
        break;
      case 15: // cycle -> CYCLE, common_expr, statement
{ CurrentSemanticValue.stVal = new CycleNode(ValueStack[ValueStack.Depth-2].eVal, ValueStack[ValueStack.Depth-1].stVal); }
        break;
      case 16: // if_st -> IF, common_expr, statement
{ CurrentSemanticValue.stVal = new IfNode(ValueStack[ValueStack.Depth-2].eVal, ValueStack[ValueStack.Depth-1].stVal); }
        break;
      case 17: // if_st -> IF, common_expr, statement, ELSE, statement
{ CurrentSemanticValue.stVal = new IfNode(ValueStack[ValueStack.Depth-4].eVal, ValueStack[ValueStack.Depth-3].stVal, ValueStack[ValueStack.Depth-1].stVal); }
        break;
      case 18: // rep_unt -> REPEAT, stlist, UNTIL, common_expr
{ CurrentSemanticValue.stVal = new RepUntNode(ValueStack[ValueStack.Depth-3].blVal, ValueStack[ValueStack.Depth-1].eVal as BinExprNode); }
        break;
      case 19: // while_st -> WHILE, common_expr, DO, statement
{ CurrentSemanticValue.stVal = new WhileNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].stVal); }
        break;
      case 20: // for_st -> FOR, assign, TO, expr, DO, statement
{ CurrentSemanticValue.stVal = new ForNode(ValueStack[ValueStack.Depth-5].stVal as AssignNode, ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].stVal); }
        break;
      case 21: // bin_sign -> LS
{ CurrentSemanticValue.bsVal = BinSign.LS; }
        break;
      case 22: // bin_sign -> GT
{ CurrentSemanticValue.bsVal = BinSign.GT; }
        break;
      case 23: // bin_sign -> LE
{ CurrentSemanticValue.bsVal = BinSign.LE; }
        break;
      case 24: // bin_sign -> GE
{ CurrentSemanticValue.bsVal = BinSign.GE; }
        break;
      case 25: // bin_sign -> EQ
{ CurrentSemanticValue.bsVal = BinSign.EQ; }
        break;
      case 26: // bin_sign -> NE
{ CurrentSemanticValue.bsVal = BinSign.NE; }
        break;
      case 27: // common_expr -> bin_expr
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 28: // common_expr -> expr
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 29: // bin_expr -> expr, bin_sign, expr
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-2].bsVal, ValueStack[ValueStack.Depth-1].eVal); }
        break;
      case 30: // expr -> m_d
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 31: // expr -> expr, PLUS, m_d
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, BinSign.PLUS, ValueStack[ValueStack.Depth-1].eVal); }
        break;
      case 32: // expr -> expr, MINUS, m_d
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, BinSign.MINUS, ValueStack[ValueStack.Depth-1].eVal); }
        break;
      case 33: // m_d -> in_br
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 34: // m_d -> m_d, MULT, in_br
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, BinSign.MULT, ValueStack[ValueStack.Depth-1].eVal); }
        break;
      case 35: // m_d -> m_d, DIV, in_br
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, BinSign.DIV, ValueStack[ValueStack.Depth-1].eVal); }
        break;
      case 36: // in_br -> ident
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal as IdNode; }
        break;
      case 37: // in_br -> INUM
{ CurrentSemanticValue.eVal = new IntNumNode(ValueStack[ValueStack.Depth-1].iVal); }
        break;
      case 38: // in_br -> LBRACE, expr, RBRACE
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-2].eVal; }
        break;
    }
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
}
