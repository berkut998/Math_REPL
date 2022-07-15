# Math_REPL

Simple interactive environment which calculate math expression, support functions and variables

Supported grammar:

function ::= fn-keyword fn-name { identifier } fn-operator expression

fn-name ::= identifier

fn-operator ::= '=>'

fn-keyword ::= 'fn'

expression ::= factor | expression operator expression

factor ::= number | identifier | assignment | '(' expression ')' | function-call

assignment ::= identifier '=' expression

function-call ::= fn-name { expression }

operator ::= '+' | '-' | '*' | '/' | '%'

identifier ::= letter | '_' { identifier-char }

identifier-char ::= '_' | letter | digit

number ::= { digit } [ '.' digit { digit } ]

letter ::= 'a' | 'b' | ... | 'y' | 'z' | 'A' | 'B' | ... | 'Y' | 'Z'

digit ::= '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'

Do not support error processing, all input should be valid.

Application screenshots: 

![image](https://user-images.githubusercontent.com/66470614/176257090-8880c5b1-be36-443a-8d56-bba67945e996.png)
