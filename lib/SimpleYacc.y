%using SimpleLang;
%{
// Ёти объ€влени€ добавл€ютс€ в класс GPPGParser, представл€ющий собой парсер, генерируемый системой gppg
	public BlockNode root;
    public Parser(AbstractScanner<SimpleParser.ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union {
		public double dVal;
		public int iVal;
		public string sVal;
		public Node nVal;
		public ExprNode eVal;
		public StatementNode stVal;
		public BlockNode blVal;
		public BinSign bsVal;
}

//%using ProgramTree

%namespace SimpleParser

%token BEGIN END CYCLE ASSIGN SEMICOLON LBRACE RBRACE PLUS MINUS MULT DIV COMMA IF THEN ELSE FOR TO DO REPEAT UNTIL WHILE
%token <bsVal> LS GT LE GE EQ NE PLUS MINUS MULT DIV
%token <iVal> INUM
%token <dVal> RNUM
%token <sVal> ID

%type <eVal> expr ident bin_expr in_br m_d common_expr
%type <stVal> assign statement cycle if_st rep_unt while_st for_st
%type <blVal> stlist block
%type <bsVal> bin_sign

%nonassoc IFX
%nonassoc ELSE

%%


progr   	: block { root = $1; }
			;

stlist		: statement{ $$ = new BlockNode($1); }
			| stlist statement
			{
				$1.Add($2);
				$$ = $1;
			}
			;

statement	: assign SEMICOLON { $$ = $1; }
			| block { $$ = $1; }
			| cycle { $$ = $1; }
			| if_st { $$ = $1; }
			| for_st { $$ = $1; }
			| rep_unt { $$ = $1; }
			| while_st { $$ = $1; }
			;

ident 		: ID { $$ = new IdNode($1); }
			;
	
assign 		: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3); }
			;

block		: BEGIN stlist END { $$ = $2; }
			;

cycle		: CYCLE common_expr statement { $$ = new CycleNode($2, $3); }
			;

if_st   	: IF common_expr THEN statement %prec IFX { $$ = new IfNode($2, $4); }
			| IF common_expr THEN statement ELSE statement { $$ = new IfNode($2, $4, $6); }
			;

rep_unt 	: REPEAT stlist UNTIL common_expr { $$ = new RepUntNode($2, $4 as BinExprNode); }
			;

while_st	: WHILE common_expr DO statement { $$ = new WhileNode($2, $4); }
			;

for_st		: FOR assign TO expr DO statement { $$ = new ForNode($2 as AssignNode, $4, $6); }
			;

bin_sign	: LS { $$ = BinSign.LS; }
			| GT { $$ = BinSign.GT; }
			| LE { $$ = BinSign.LE; }
			| GE { $$ = BinSign.GE; }
			| EQ { $$ = BinSign.EQ; }
			| NE { $$ = BinSign.NE; }
			;

common_expr	: bin_expr { $$ = $1; }
			| expr { $$ = $1; }
			;
			
bin_expr	: expr bin_sign expr { $$ = new BinExprNode($1, $2, $3); }
			;

expr		: m_d { $$ = $1; }
			| expr PLUS m_d { $$ = new BinExprNode($1, BinSign.PLUS, $3); }
			| expr MINUS m_d { $$ = new BinExprNode($1, BinSign.MINUS, $3); }
			;

m_d			: in_br { $$ = $1; }
			| m_d MULT in_br { $$ = new BinExprNode($1, BinSign.MULT, $3); }
			| m_d DIV in_br { $$ = new BinExprNode($1, BinSign.DIV, $3); }
			;
// priority
in_br		: ident { $$ = $1 as IdNode; }
			| INUM { $$ = new IntNumNode($1); }
			| LBRACE expr RBRACE { $$ = $2; }
			;
%%