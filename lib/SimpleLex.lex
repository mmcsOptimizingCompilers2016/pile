%using SimpleParser;
%using QUT.Gppg;
%using System.Linq;

%namespace SimpleScanner

Alpha 	[a-zA-Z_]
Digit   [0-9] 
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
REALNUM {INTNUM}\.{INTNUM}
ID {Alpha}{AlphaDigit}*

%%

{INTNUM} {
  yylval.iVal = int.Parse(yytext);
  return (int)Tokens.INUM;
}

{REALNUM} {
	yylval.dVal = double.Parse(yytext);
  return (int)Tokens.RNUM;
}

{ID} {
  int res = ScannerHelper.GetIDToken(yytext);
  if (res == (int)Tokens.ID)
	yylval.sVal = yytext;
  return res;
}

":=" { return (int)Tokens.ASSIGN; }
";"  { return (int)Tokens.SEMICOLON; }
"{"  { return (int)Tokens.BEGIN; }
"}"  { return (int)Tokens.END; }
"("  { return (int)Tokens.LBRACE; }
")"  { return (int)Tokens.RBRACE; }
"+"  { return (int)Tokens.PLUS; }
"-"  { return (int)Tokens.MINUS; }
"*"  { return (int)Tokens.MULT; }
"/"  { return (int)Tokens.DIV; }
"<"  { return (int)Tokens.LS; }
">"  { return (int)Tokens.GT; }
"<=" { return (int)Tokens.LE; }
">=" { return (int)Tokens.GE; }
"=" { return (int)Tokens.EQ; }
"!=" { return (int)Tokens.NE; }

[^ \r\n\t] {
	LexError();
	return (int)Tokens.EOF; // ����� �������
}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol); // ������� ������� (������������� ��� ���������������), ������������ @1 @2 � �.�.
%}

%%

public override void yyerror(string format, params object[] args) // ��������� �������������� ������
{
  var ww = args.Skip(1).Cast<string>().ToArray();
  string errorMsg = string.Format("({0},{1}): ��������� {2}, � ��������� {3}", yyline, yycol, args[0], string.Join(" ��� ", ww));
  throw new SyntaxException(errorMsg);
}

public void LexError()
{
  string errorMsg = string.Format("({0},{1}): ����������� ������ {2}", yyline, yycol, yytext);
  throw new LexException(errorMsg);
}

class ScannerHelper
{
  private static Dictionary<string,int> keywords;

  static ScannerHelper()
  {
	keywords = new Dictionary<string,int>();
	keywords.Add("cycle",(int)Tokens.CYCLE);
	keywords.Add("if",(int)Tokens.IF);
	keywords.Add("then",(int)Tokens.THEN);
	keywords.Add("else",(int)Tokens.ELSE);
	keywords.Add("for",(int)Tokens.FOR);
	keywords.Add("to",(int)Tokens.TO);
	keywords.Add("do",(int)Tokens.DO);
	keywords.Add("repeat",(int)Tokens.REPEAT);
	keywords.Add("until",(int)Tokens.UNTIL);
	keywords.Add("while",(int)Tokens.WHILE);
  }
  public static int GetIDToken(string s)
  {
    if (keywords.ContainsKey(s.ToLower())) // ���� �������������� � ��������
      return keywords[s];
    else
      return (int)Tokens.ID;
  }
}
