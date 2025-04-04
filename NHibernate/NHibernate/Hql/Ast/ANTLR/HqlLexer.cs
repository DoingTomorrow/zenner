// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.HqlLexer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  public class HqlLexer : Lexer
  {
    public const int LT = 109;
    public const int EXPONENT = 130;
    public const int STAR = 120;
    public const int FLOAT_SUFFIX = 131;
    public const int LITERAL_by = 56;
    public const int CASE = 57;
    public const int NEW = 37;
    public const int FILTER_ENTITY = 76;
    public const int PARAM = 106;
    public const int COUNT = 12;
    public const int NOT = 38;
    public const int EOF = -1;
    public const int UNARY_PLUS = 91;
    public const int QUOTED_String = 124;
    public const int ESCqs = 128;
    public const int WEIRD_IDENT = 93;
    public const int OPEN_BRACKET = 122;
    public const int FULL = 23;
    public const int ORDER_ELEMENT = 85;
    public const int IS_NULL = 80;
    public const int ESCAPE = 18;
    public const int INSERT = 29;
    public const int BOTH = 64;
    public const int NUM_DECIMAL = 97;
    public const int VERSIONED = 54;
    public const int EQ = 102;
    public const int SELECT = 45;
    public const int INTO = 30;
    public const int NE = 107;
    public const int GE = 112;
    public const int TAKE = 50;
    public const int CONCAT = 113;
    public const int ID_LETTER = 127;
    public const int NULL = 39;
    public const int ELSE = 59;
    public const int SELECT_FROM = 89;
    public const int TRAILING = 70;
    public const int ON = 62;
    public const int NUM_LONG = 99;
    public const int NUM_DOUBLE = 96;
    public const int UNARY_MINUS = 90;
    public const int DELETE = 13;
    public const int INDICES = 27;
    public const int OF = 69;
    public const int METHOD_CALL = 81;
    public const int LEADING = 66;
    public const int SKIP = 47;
    public const int EMPTY = 65;
    public const int GROUP = 24;
    public const int WS = 129;
    public const int FETCH = 21;
    public const int VECTOR_EXPR = 92;
    public const int NOT_IN = 83;
    public const int NUM_INT = 95;
    public const int OR = 40;
    public const int ALIAS = 72;
    public const int JAVA_CONSTANT = 100;
    public const int CONSTANT = 94;
    public const int GT = 110;
    public const int QUERY = 86;
    public const int BNOT = 114;
    public const int INDEX_OP = 78;
    public const int NUM_FLOAT = 98;
    public const int FROM = 22;
    public const int END = 58;
    public const int FALSE = 20;
    public const int DISTINCT = 16;
    public const int CONSTRUCTOR = 73;
    public const int T__133 = 133;
    public const int T__134 = 134;
    public const int CLOSE_BRACKET = 123;
    public const int WHERE = 55;
    public const int CLASS = 11;
    public const int MEMBER = 67;
    public const int INNER = 28;
    public const int PROPERTIES = 43;
    public const int ORDER = 41;
    public const int MAX = 35;
    public const int UPDATE = 53;
    public const int SQL_NE = 108;
    public const int AND = 6;
    public const int SUM = 49;
    public const int ASCENDING = 8;
    public const int EXPR_LIST = 75;
    public const int AS = 7;
    public const int IN = 26;
    public const int THEN = 60;
    public const int OBJECT = 68;
    public const int COMMA = 101;
    public const int IS = 31;
    public const int AVG = 9;
    public const int LEFT = 33;
    public const int SOME = 48;
    public const int ALL = 4;
    public const int BOR = 115;
    public const int IDENT = 125;
    public const int CASE2 = 74;
    public const int BXOR = 116;
    public const int PLUS = 118;
    public const int EXISTS = 19;
    public const int DOT = 15;
    public const int WITH = 63;
    public const int LIKE = 34;
    public const int OUTER = 42;
    public const int ID_START_LETTER = 126;
    public const int ROW_STAR = 88;
    public const int NOT_LIKE = 84;
    public const int RANGE = 87;
    public const int NOT_BETWEEN = 82;
    public const int HEX_DIGIT = 132;
    public const int SET = 46;
    public const int RIGHT = 44;
    public const int HAVING = 25;
    public const int MIN = 36;
    public const int IS_NOT_NULL = 79;
    public const int MINUS = 119;
    public const int ELEMENTS = 17;
    public const int BAND = 117;
    public const int TRUE = 51;
    public const int JOIN = 32;
    public const int IN_LIST = 77;
    public const int UNION = 52;
    public const int OPEN = 103;
    public const int COLON = 105;
    public const int ANY = 5;
    public const int CLOSE = 104;
    public const int WHEN = 61;
    public const int DIV = 121;
    public const int DESCENDING = 14;
    public const int AGGREGATE = 71;
    public const int BETWEEN = 10;
    public const int LE = 111;
    private const string DFA23_eotS = "\u0001\uFFFF\u0015(\u0001\uFFFF\u0001c\u0001e\u0001g\u0001h\u0001j\u0010\uFFFF\u0002(\u0001o\u0002(\u0001r\u0011(\u0001\u008A\u0001\u008B\t(\u0001\u0097\u0001(\u0001\u0099\u0001(\u0001\u009B\u000E(\n\uFFFF\u0001¬\u0001\u00AD\u0001®\u0001°\u0001\uFFFF\u0001±\u0001(\u0001\uFFFF\v(\u0001\u00BE\v(\u0002\uFFFF\u0004(\u0001Î\u0001Ï\u0001(\u0001Ñ\u0001Ò\u0002(\u0001\uFFFF\u0001(\u0001\uFFFF\u0001(\u0001\uFFFF\u0003(\u0001Ú\u0002(\u0001Ý\t(\u0003\uFFFF\u0001(\u0002\uFFFF\u0001(\u0001ê\u0002(\u0001í\u0001(\u0001ð\u0002(\u0001ó\u0002(\u0001\uFFFF\u0003(\u0001ù\u0001ú\u0005(\u0001Ā\u0001ā\u0001Ă\u0001(\u0001Ą\u0002\uFFFF\u0001(\u0002\uFFFF\u0001Ć\u0006(\u0001\uFFFF\u0001č\u0001Ď\u0001\uFFFF\u0001ď\u0001Đ\u0001(\u0001Ē\u0004(\u0001ė\u0001Ę\u0002(\u0001\uFFFF\u0001ě\u0001Ĝ\u0001\uFFFF\u0002(\u0001\uFFFF\u0002(\u0001\uFFFF\u0002(\u0001ģ\u0001Ĥ\u0001ĥ\u0002\uFFFF\u0001Ħ\u0002(\u0001ĩ\u0001(\u0003\uFFFF\u0001(\u0001\uFFFF\u0001(\u0001\uFFFF\u0001ĭ\u0001Į\u0002(\u0001ı\u0001(\u0004\uFFFF\u0001(\u0001\uFFFF\u0001Ĵ\u0002(\u0001ķ\u0002\uFFFF\u0002(\u0002\uFFFF\u0001ĺ\u0003(\u0001ľ\u0001Ŀ\u0004\uFFFF\u0001ŀ\u0001(\u0001\uFFFF\u0001ł\u0001(\u0001ń\u0002\uFFFF\u0001Ņ\u0001(\u0001\uFFFF\u0001Ň\u0001(\u0001\uFFFF\u0001ŉ\u0001(\u0001\uFFFF\u0001(\u0001Ō\u0001\uFFFF\u0003(\u0003\uFFFF\u0001Ő\u0001\uFFFF\u0001ő\u0002\uFFFF\u0001(\u0001\uFFFF\u0001(\u0001\uFFFF\u0002(\u0001\uFFFF\u0001(\u0001ŗ\u0001Ř\u0002\uFFFF\u0001(\u0001Ś\u0001(\u0001Ŝ\u0001(\u0002\uFFFF\u0001(\u0001\uFFFF\u0001ş\u0001\uFFFF\u0001Š\u0001š\u0003\uFFFF";
    private const string DFA23_eofS = "Ţ\uFFFF";
    private const string DFA23_minS = "\u0001\t\u0001l\u0001e\u0001a\u0001e\u0001l\u0001a\u0001r\u0001a\u0001n\u0001o\u0001e\u0001a\u0001e\u0001b\u0001r\u0001i\u0001e\u0001a\u0001n\u0001e\u0001h\u0001\uFFFF\u0004=\u0001|\u0010\uFFFF\u0001l\u0001d\u0001$\u0001g\u0001t\u0001$\u0001t\u0001a\u0001u\u0001s\u0001l\u0001s\u0001e\u0001c\u0001i\u0001d\u0001p\u0001l\u0001t\u0001o\u0001l\u0001o\u0001v\u0002$\u0001i\u0001a\u0001k\u0001x\u0001n\u0001m\u0001w\u0001t\u0001l\u0001$\u0001t\u0001$\u0001j\u0001$\u0001o\u0001g\u0001l\u0001i\u0002m\u0001k\u0001a\u0001e\u0001i\u0001d\u0001r\u0001e\u0001t\n\uFFFF\u0004$\u0001\uFFFF\u0001$\u0001w\u0001\uFFFF\u0001h\u0001s\u0001n\u0002e\u0001c\u0001t\u0001m\u0001e\u0001a\u0001s\u0001$\u0001t\u0001s\u0001c\u0001m\u0001l\u0001u\u0002i\u0002e\u0001o\u0002\uFFFF\u0001n\u0001t\u0001d\u0001e\u0002$\u0001b\u0002$\u0001l\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001p\u0001h\u0001e\u0001$\u0001p\u0001e\u0001$\u0002e\u0001i\u0001n\u0001o\u0001a\u0001s\u0001n\u0001h\u0003\uFFFF\u0001n\u0002\uFFFF\u0001e\u0001$\u0001s\u0001t\u0001$\u0001t\u0001$\u0001i\u0001e\u0001$\u0001p\u0001t\u0001\uFFFF\u0001y\u0001e\u0001h\u0002$\u0001p\u0001n\u0001c\u0002r\u0003$\u0001i\u0001$\u0002\uFFFF\u0001e\u0002\uFFFF\u0001$\u0002r\u0001c\u0001e\u0001t\u0001c\u0001\uFFFF\u0002$\u0001\uFFFF\u0002$\u0001l\u0001$\u0001n\u0001t\u0001i\u0001e\u0002$\u0001d\u0001e\u0001\uFFFF\u0002$\u0001\uFFFF\u0001e\u0001n\u0001\uFFFF\u0002n\u0001\uFFFF\u0001e\u0001s\u0003$\u0002\uFFFF\u0001$\u0001g\u0001e\u0001$\u0001t\u0003\uFFFF\u0001n\u0001\uFFFF\u0001r\u0001\uFFFF\u0002$\u0001t\u0001r\u0001$\u0001t\u0004\uFFFF\u0001i\u0001\uFFFF\u0001$\u0001e\u0001o\u0001$\u0002\uFFFF\u0001i\u0001n\u0002\uFFFF\u0001$\u0001d\u0001c\u0001t\u0002$\u0004\uFFFF\u0001$\u0001s\u0001\uFFFF\u0001$\u0001g\u0001$\u0002\uFFFF\u0001$\u0001t\u0001\uFFFF\u0001$\u0001n\u0001\uFFFF\u0001$\u0001n\u0001\uFFFF\u0001n\u0001$\u0001\uFFFF\u0001i\u0001t\u0001s\u0003\uFFFF\u0001$\u0001\uFFFF\u0001$\u0002\uFFFF\u0001i\u0001\uFFFF\u0001g\u0001\uFFFF\u0001e\u0001g\u0001\uFFFF\u0001n\u0002$\u0002\uFFFF\u0001e\u0001$\u0001d\u0001$\u0001g\u0002\uFFFF\u0001s\u0001\uFFFF\u0001$\u0001\uFFFF\u0002$\u0003\uFFFF";
    private const string DFA23_maxS = "\u0001\uFFFE\u0001v\u0001y\u0001o\u0001i\u0001x\u0001u\u0001r\u0001a\u0001s\u0001o\u0002i\u0002u\u0001r\u0001i\u0001u\u0001r\u0001p\u0001e\u0001i\u0001\uFFFF\u0001>\u0003=\u0001|\u0010\uFFFF\u0001l\u0001y\u0001\uFFFE\u0001g\u0001t\u0001\uFFFE\u0001t\u0001a\u0001u\u0004s\u0001c\u0001i\u0001d\u0001p\u0001l\u0001t\u0001o\u0001l\u0001o\u0001v\u0002\uFFFE\u0001i\u0001f\u0001k\u0001x\u0001n\u0001m\u0001w\u0001t\u0001l\u0001\uFFFE\u0001t\u0001\uFFFE\u0001j\u0001\uFFFE\u0001o\u0001g\u0001t\u0001i\u0002m\u0001k\u0001u\u0001e\u0001i\u0001d\u0001r\u0001e\u0001t\n\uFFFF\u0004\uFFFE\u0001\uFFFF\u0001\uFFFE\u0001w\u0001\uFFFF\u0001h\u0001s\u0001n\u0002e\u0001c\u0001t\u0001m\u0001e\u0001a\u0001s\u0001\uFFFE\u0001t\u0001s\u0001c\u0001m\u0001l\u0001u\u0002i\u0002e\u0001o\u0002\uFFFF\u0001n\u0001t\u0001d\u0001e\u0002\uFFFE\u0001b\u0002\uFFFE\u0001l\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001p\u0001h\u0001e\u0001\uFFFE\u0001p\u0001e\u0001\uFFFE\u0002e\u0001i\u0001n\u0001o\u0001a\u0001s\u0001r\u0001h\u0003\uFFFF\u0001n\u0002\uFFFF\u0001e\u0001\uFFFE\u0001s\u0001t\u0001\uFFFE\u0001t\u0001\uFFFE\u0001i\u0001e\u0001\uFFFE\u0001p\u0001t\u0001\uFFFF\u0001y\u0001e\u0001h\u0002\uFFFE\u0001p\u0001n\u0001c\u0002r\u0003\uFFFE\u0001i\u0001\uFFFE\u0002\uFFFF\u0001e\u0002\uFFFF\u0001\uFFFE\u0002r\u0001c\u0001e\u0001t\u0001c\u0001\uFFFF\u0002\uFFFE\u0001\uFFFF\u0002\uFFFE\u0001l\u0001\uFFFE\u0001n\u0001t\u0001i\u0001e\u0002\uFFFE\u0001d\u0001e\u0001\uFFFF\u0002\uFFFE\u0001\uFFFF\u0001e\u0001n\u0001\uFFFF\u0002n\u0001\uFFFF\u0001e\u0001s\u0003\uFFFE\u0002\uFFFF\u0001\uFFFE\u0001g\u0001e\u0001\uFFFE\u0001t\u0003\uFFFF\u0001n\u0001\uFFFF\u0001r\u0001\uFFFF\u0002\uFFFE\u0001t\u0001r\u0001\uFFFE\u0001t\u0004\uFFFF\u0001i\u0001\uFFFF\u0001\uFFFE\u0001e\u0001o\u0001\uFFFE\u0002\uFFFF\u0001i\u0001n\u0002\uFFFF\u0001\uFFFE\u0001d\u0001c\u0001t\u0002\uFFFE\u0004\uFFFF\u0001\uFFFE\u0001s\u0001\uFFFF\u0001\uFFFE\u0001g\u0001\uFFFE\u0002\uFFFF\u0001\uFFFE\u0001t\u0001\uFFFF\u0001\uFFFE\u0001n\u0001\uFFFF\u0001\uFFFE\u0001n\u0001\uFFFF\u0001n\u0001\uFFFE\u0001\uFFFF\u0001i\u0001t\u0001s\u0003\uFFFF\u0001\uFFFE\u0001\uFFFF\u0001\uFFFE\u0002\uFFFF\u0001i\u0001\uFFFF\u0001g\u0001\uFFFF\u0001e\u0001g\u0001\uFFFF\u0001n\u0002\uFFFE\u0002\uFFFF\u0001e\u0001\uFFFE\u0001d\u0001\uFFFE\u0001g\u0002\uFFFF\u0001s\u0001\uFFFF\u0001\uFFFE\u0001\uFFFF\u0002\uFFFE\u0003\uFFFF";
    private const string DFA23_acceptS = "\u0016\uFFFF\u0001E\u0005\uFFFF\u0001N\u0001P\u0001Q\u0001R\u0001S\u0001T\u0001V\u0001W\u0001X\u0001Y\u0001Z\u0001[\u0001\\\u0001]\u0001^\u0001_5\uFFFF\u0001H\u0001J\u0001F\u0001K\u0001G\u0001I\u0001O\u0001M\u0001U\u0001L\u0004\uFFFF\u0001\u0004\u0002\uFFFF\u00014\u0017\uFFFF\u0001\u0016\u0001\u001B\v\uFFFF\u0001$\u0001\uFFFF\u0001:\u0001\uFFFF\u0001A\u0010\uFFFF\u0001\u0001\u0001\u0002\u0001\u0003\u0001\uFFFF\u0001\u0005\u0001\u0006\f\uFFFF\u00016\u000F\uFFFF\u0001\u001F\u0001 \u0001\uFFFF\u0001!\u0001\"\a\uFFFF\u0001*\u0002\uFFFF\u0001-\f\uFFFF\u0001<\u0002\uFFFF\u00015\u0002\uFFFF\u0001\v\u0002\uFFFF\u00017\u0005\uFFFF\u0001\u0012\u0001\u0013\u0005\uFFFF\u0001\u001A\u0001\u001C\u0001\u001D\u0001\uFFFF\u0001\u001E\u0001\uFFFF\u0001#\u0006\uFFFF\u0001+\u0001,\u0001.\u0001/\u0001\uFFFF\u00018\u0004\uFFFF\u00019\u0001;\u0002\uFFFF\u0001\b\u0001\t\u0006\uFFFF\u0001=\u0001\u0010\u0001\u0011\u0001\u0014\u0002\uFFFF\u0001\u0018\u0003\uFFFF\u0001%\u0001&\u0002\uFFFF\u0001(\u0002\uFFFF\u00010\u0002\uFFFF\u00013\u0002\uFFFF\u0001\n\u0003\uFFFF\u0001\u000E\u0001\u000F\u0001\u0015\u0001\uFFFF\u0001\u0019\u0001\uFFFF\u0001?\u0001@\u0001\uFFFF\u0001)\u0001\uFFFF\u00011\u0002\uFFFF\u0001\a\u0003\uFFFF\u0001\u0017\u0001>\u0005\uFFFF\u0001\f\u0001\r\u0001\uFFFF\u0001B\u0001\uFFFF\u0001C\u0002\uFFFF\u00012\u0001D\u0001'";
    private const string DFA23_specialS = "Ţ\uFFFF}>";
    protected HqlLexer.DFA23 dfa23;
    private static readonly string[] DFA23_transitionS = new string[354]
    {
      "\u0002*\u0002\uFFFF\u0001*\u0012\uFFFF\u0001*\u0001\u0019\u0002\uFFFF\u0001(\u0001\uFFFF\u0001\u001C\u0001)\u0001\u001E\u0001\u001F\u0001$\u0001\"\u0001\u001D\u0001#\u0001+\u0001%\n+\u0001&\u0001\uFFFF\u0001\u0017\u0001\u0016\u0001\u0018\u0001'\u0001\uFFFF\u001A(\u0001 \u0001\uFFFF\u0001!\u0001\u001A\u0001(\u0001\uFFFF\u0001\u0001\u0001\u0002\u0001\u0003\u0001\u0004\u0001\u0005\u0001\u0006\u0001\a\u0001\b\u0001\t\u0001\n\u0001(\u0001\v\u0001\f\u0001\r\u0001\u000E\u0001\u000F\u0001(\u0001\u0010\u0001\u0011\u0001\u0012\u0001\u0013\u0001\u0014\u0001\u0015\u0003(\u0001\uFFFF\u0001\u001B\u0003\uFFFFｿ(",
      "\u0001,\u0001\uFFFF\u0001-\u0004\uFFFF\u0001.\u0002\uFFFF\u0001/",
      "\u00010\t\uFFFF\u00012\t\uFFFF\u00011",
      "\u00015\n\uFFFF\u00013\u0002\uFFFF\u00014",
      "\u00016\u0003\uFFFF\u00017",
      "\u00018\u0001<\u0001;\u0004\uFFFF\u00019\u0004\uFFFF\u0001:",
      "\u0001=\u0003\uFFFF\u0001>\f\uFFFF\u0001?\u0002\uFFFF\u0001@",
      "\u0001A",
      "\u0001B",
      "\u0001C\u0004\uFFFF\u0001D",
      "\u0001E",
      "\u0001F\u0003\uFFFF\u0001G",
      "\u0001H\u0003\uFFFF\u0001J\u0003\uFFFF\u0001I",
      "\u0001K\t\uFFFF\u0001L\u0005\uFFFF\u0001M",
      "\u0001Q\u0003\uFFFF\u0001R\a\uFFFF\u0001P\u0003\uFFFF\u0001N\u0002\uFFFF\u0001O",
      "\u0001S",
      "\u0001T",
      "\u0001U\u0005\uFFFF\u0001V\u0003\uFFFF\u0001W\u0005\uFFFF\u0001X",
      "\u0001Y\u0006\uFFFF\u0001[\t\uFFFF\u0001Z",
      "\u0001\\\u0001\uFFFF\u0001]",
      "\u0001^",
      "\u0001_\u0001`",
      "",
      "\u0001b\u0001a",
      "\u0001d",
      "\u0001f",
      "\u0001f",
      "\u0001i",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "\u0001k",
      "\u0001m\u0014\uFFFF\u0001l",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u0002(\u0001n\u0017(\u0005\uFFFFｿ(",
      "\u0001p",
      "\u0001q",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001s",
      "\u0001t",
      "\u0001u",
      "\u0001v",
      "\u0001w\u0006\uFFFF\u0001x",
      "\u0001y",
      "\u0001z\r\uFFFF\u0001{",
      "\u0001|",
      "\u0001}",
      "\u0001~",
      "\u0001\u007F",
      "\u0001\u0080",
      "\u0001\u0081",
      "\u0001\u0082",
      "\u0001\u0083",
      "\u0001\u0084",
      "\u0001\u0085",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u0003(\u0001\u0086\t(\u0001\u0087\u0004(\u0001\u0088\u0001\u0089\u0006(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001\u008C",
      "\u0001\u008E\u0004\uFFFF\u0001\u008D",
      "\u0001\u008F",
      "\u0001\u0090",
      "\u0001\u0091",
      "\u0001\u0092",
      "\u0001\u0093",
      "\u0001\u0094",
      "\u0001\u0095",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u0003(\u0001\u0096\u0016(\u0005\uFFFFｿ(",
      "\u0001\u0098",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001\u009A",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001\u009C",
      "\u0001\u009D",
      "\u0001\u009E\a\uFFFF\u0001\u009F",
      "\u0001 ",
      "\u0001¡",
      "\u0001¢",
      "\u0001£",
      "\u0001¥\u0013\uFFFF\u0001¤",
      "\u0001¦",
      "\u0001§",
      "\u0001¨",
      "\u0001©",
      "\u0001ª",
      "\u0001«",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u0004(\u0001¯\u0015(\u0005\uFFFFｿ(",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001\u00B2",
      "",
      "\u0001\u00B3",
      "\u0001´",
      "\u0001µ",
      "\u0001¶",
      "\u0001·",
      "\u0001¸",
      "\u0001\u00B9",
      "\u0001º",
      "\u0001»",
      "\u0001\u00BC",
      "\u0001\u00BD",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001¿",
      "\u0001À",
      "\u0001Á",
      "\u0001Â",
      "\u0001Ã",
      "\u0001Ä",
      "\u0001Å",
      "\u0001Æ",
      "\u0001Ç",
      "\u0001È",
      "\u0001É",
      "",
      "",
      "\u0001Ê",
      "\u0001Ë",
      "\u0001Ì",
      "\u0001Í",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ð",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ó",
      "\u0001Ô",
      "",
      "\u0001Õ",
      "",
      "\u0001Ö",
      "",
      "\u0001×",
      "\u0001Ø",
      "\u0001Ù",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Û",
      "\u0001Ü",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Þ",
      "\u0001ß",
      "\u0001à",
      "\u0001á",
      "\u0001â",
      "\u0001ã",
      "\u0001ä",
      "\u0001æ\u0003\uFFFF\u0001å",
      "\u0001ç",
      "",
      "",
      "",
      "\u0001è",
      "",
      "",
      "\u0001é",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ë",
      "\u0001ì",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001î",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u0004(\u0001ï\u0015(\u0005\uFFFFｿ(",
      "\u0001ñ",
      "\u0001ò",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ô",
      "\u0001õ",
      "",
      "\u0001ö",
      "\u0001÷",
      "\u0001ø",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001û",
      "\u0001ü",
      "\u0001ý",
      "\u0001þ",
      "\u0001ÿ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ă",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      "\u0001ą",
      "",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ć",
      "\u0001Ĉ",
      "\u0001ĉ",
      "\u0001Ċ",
      "\u0001ċ",
      "\u0001Č",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001đ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ē",
      "\u0001Ĕ",
      "\u0001ĕ",
      "\u0001Ė",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ę",
      "\u0001Ě",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "\u0001ĝ",
      "\u0001Ğ",
      "",
      "\u0001ğ",
      "\u0001Ġ",
      "",
      "\u0001ġ",
      "\u0001Ģ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ħ",
      "\u0001Ĩ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ī",
      "",
      "",
      "",
      "\u0001ī",
      "",
      "\u0001Ĭ",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001į",
      "\u0001İ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ĳ",
      "",
      "",
      "",
      "",
      "\u0001ĳ",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ĵ",
      "\u0001Ķ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      "\u0001ĸ",
      "\u0001Ĺ",
      "",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ļ",
      "\u0001ļ",
      "\u0001Ľ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      "",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ł",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ń",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ņ",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ň",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001Ŋ",
      "",
      "\u0001ŋ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "\u0001ō",
      "\u0001Ŏ",
      "\u0001ŏ",
      "",
      "",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      "\u0001Œ",
      "",
      "\u0001œ",
      "",
      "\u0001Ŕ",
      "\u0001ŕ",
      "",
      "\u0001Ŗ",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      "\u0001ř",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ś",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001ŝ",
      "",
      "",
      "\u0001Ş",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "\u0001(\v\uFFFF\n(\a\uFFFF\u001A(\u0004\uFFFF\u0001(\u0001\uFFFF\u001A(\u0005\uFFFFｿ(",
      "",
      "",
      ""
    };
    private static readonly short[] DFA23_eot = DFA.UnpackEncodedString("\u0001\uFFFF\u0015(\u0001\uFFFF\u0001c\u0001e\u0001g\u0001h\u0001j\u0010\uFFFF\u0002(\u0001o\u0002(\u0001r\u0011(\u0001\u008A\u0001\u008B\t(\u0001\u0097\u0001(\u0001\u0099\u0001(\u0001\u009B\u000E(\n\uFFFF\u0001¬\u0001\u00AD\u0001®\u0001°\u0001\uFFFF\u0001±\u0001(\u0001\uFFFF\v(\u0001\u00BE\v(\u0002\uFFFF\u0004(\u0001Î\u0001Ï\u0001(\u0001Ñ\u0001Ò\u0002(\u0001\uFFFF\u0001(\u0001\uFFFF\u0001(\u0001\uFFFF\u0003(\u0001Ú\u0002(\u0001Ý\t(\u0003\uFFFF\u0001(\u0002\uFFFF\u0001(\u0001ê\u0002(\u0001í\u0001(\u0001ð\u0002(\u0001ó\u0002(\u0001\uFFFF\u0003(\u0001ù\u0001ú\u0005(\u0001Ā\u0001ā\u0001Ă\u0001(\u0001Ą\u0002\uFFFF\u0001(\u0002\uFFFF\u0001Ć\u0006(\u0001\uFFFF\u0001č\u0001Ď\u0001\uFFFF\u0001ď\u0001Đ\u0001(\u0001Ē\u0004(\u0001ė\u0001Ę\u0002(\u0001\uFFFF\u0001ě\u0001Ĝ\u0001\uFFFF\u0002(\u0001\uFFFF\u0002(\u0001\uFFFF\u0002(\u0001ģ\u0001Ĥ\u0001ĥ\u0002\uFFFF\u0001Ħ\u0002(\u0001ĩ\u0001(\u0003\uFFFF\u0001(\u0001\uFFFF\u0001(\u0001\uFFFF\u0001ĭ\u0001Į\u0002(\u0001ı\u0001(\u0004\uFFFF\u0001(\u0001\uFFFF\u0001Ĵ\u0002(\u0001ķ\u0002\uFFFF\u0002(\u0002\uFFFF\u0001ĺ\u0003(\u0001ľ\u0001Ŀ\u0004\uFFFF\u0001ŀ\u0001(\u0001\uFFFF\u0001ł\u0001(\u0001ń\u0002\uFFFF\u0001Ņ\u0001(\u0001\uFFFF\u0001Ň\u0001(\u0001\uFFFF\u0001ŉ\u0001(\u0001\uFFFF\u0001(\u0001Ō\u0001\uFFFF\u0003(\u0003\uFFFF\u0001Ő\u0001\uFFFF\u0001ő\u0002\uFFFF\u0001(\u0001\uFFFF\u0001(\u0001\uFFFF\u0002(\u0001\uFFFF\u0001(\u0001ŗ\u0001Ř\u0002\uFFFF\u0001(\u0001Ś\u0001(\u0001Ŝ\u0001(\u0002\uFFFF\u0001(\u0001\uFFFF\u0001ş\u0001\uFFFF\u0001Š\u0001š\u0003\uFFFF");
    private static readonly short[] DFA23_eof = DFA.UnpackEncodedString("Ţ\uFFFF");
    private static readonly char[] DFA23_min = DFA.UnpackEncodedStringToUnsignedChars("\u0001\t\u0001l\u0001e\u0001a\u0001e\u0001l\u0001a\u0001r\u0001a\u0001n\u0001o\u0001e\u0001a\u0001e\u0001b\u0001r\u0001i\u0001e\u0001a\u0001n\u0001e\u0001h\u0001\uFFFF\u0004=\u0001|\u0010\uFFFF\u0001l\u0001d\u0001$\u0001g\u0001t\u0001$\u0001t\u0001a\u0001u\u0001s\u0001l\u0001s\u0001e\u0001c\u0001i\u0001d\u0001p\u0001l\u0001t\u0001o\u0001l\u0001o\u0001v\u0002$\u0001i\u0001a\u0001k\u0001x\u0001n\u0001m\u0001w\u0001t\u0001l\u0001$\u0001t\u0001$\u0001j\u0001$\u0001o\u0001g\u0001l\u0001i\u0002m\u0001k\u0001a\u0001e\u0001i\u0001d\u0001r\u0001e\u0001t\n\uFFFF\u0004$\u0001\uFFFF\u0001$\u0001w\u0001\uFFFF\u0001h\u0001s\u0001n\u0002e\u0001c\u0001t\u0001m\u0001e\u0001a\u0001s\u0001$\u0001t\u0001s\u0001c\u0001m\u0001l\u0001u\u0002i\u0002e\u0001o\u0002\uFFFF\u0001n\u0001t\u0001d\u0001e\u0002$\u0001b\u0002$\u0001l\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001p\u0001h\u0001e\u0001$\u0001p\u0001e\u0001$\u0002e\u0001i\u0001n\u0001o\u0001a\u0001s\u0001n\u0001h\u0003\uFFFF\u0001n\u0002\uFFFF\u0001e\u0001$\u0001s\u0001t\u0001$\u0001t\u0001$\u0001i\u0001e\u0001$\u0001p\u0001t\u0001\uFFFF\u0001y\u0001e\u0001h\u0002$\u0001p\u0001n\u0001c\u0002r\u0003$\u0001i\u0001$\u0002\uFFFF\u0001e\u0002\uFFFF\u0001$\u0002r\u0001c\u0001e\u0001t\u0001c\u0001\uFFFF\u0002$\u0001\uFFFF\u0002$\u0001l\u0001$\u0001n\u0001t\u0001i\u0001e\u0002$\u0001d\u0001e\u0001\uFFFF\u0002$\u0001\uFFFF\u0001e\u0001n\u0001\uFFFF\u0002n\u0001\uFFFF\u0001e\u0001s\u0003$\u0002\uFFFF\u0001$\u0001g\u0001e\u0001$\u0001t\u0003\uFFFF\u0001n\u0001\uFFFF\u0001r\u0001\uFFFF\u0002$\u0001t\u0001r\u0001$\u0001t\u0004\uFFFF\u0001i\u0001\uFFFF\u0001$\u0001e\u0001o\u0001$\u0002\uFFFF\u0001i\u0001n\u0002\uFFFF\u0001$\u0001d\u0001c\u0001t\u0002$\u0004\uFFFF\u0001$\u0001s\u0001\uFFFF\u0001$\u0001g\u0001$\u0002\uFFFF\u0001$\u0001t\u0001\uFFFF\u0001$\u0001n\u0001\uFFFF\u0001$\u0001n\u0001\uFFFF\u0001n\u0001$\u0001\uFFFF\u0001i\u0001t\u0001s\u0003\uFFFF\u0001$\u0001\uFFFF\u0001$\u0002\uFFFF\u0001i\u0001\uFFFF\u0001g\u0001\uFFFF\u0001e\u0001g\u0001\uFFFF\u0001n\u0002$\u0002\uFFFF\u0001e\u0001$\u0001d\u0001$\u0001g\u0002\uFFFF\u0001s\u0001\uFFFF\u0001$\u0001\uFFFF\u0002$\u0003\uFFFF");
    private static readonly char[] DFA23_max = DFA.UnpackEncodedStringToUnsignedChars("\u0001\uFFFE\u0001v\u0001y\u0001o\u0001i\u0001x\u0001u\u0001r\u0001a\u0001s\u0001o\u0002i\u0002u\u0001r\u0001i\u0001u\u0001r\u0001p\u0001e\u0001i\u0001\uFFFF\u0001>\u0003=\u0001|\u0010\uFFFF\u0001l\u0001y\u0001\uFFFE\u0001g\u0001t\u0001\uFFFE\u0001t\u0001a\u0001u\u0004s\u0001c\u0001i\u0001d\u0001p\u0001l\u0001t\u0001o\u0001l\u0001o\u0001v\u0002\uFFFE\u0001i\u0001f\u0001k\u0001x\u0001n\u0001m\u0001w\u0001t\u0001l\u0001\uFFFE\u0001t\u0001\uFFFE\u0001j\u0001\uFFFE\u0001o\u0001g\u0001t\u0001i\u0002m\u0001k\u0001u\u0001e\u0001i\u0001d\u0001r\u0001e\u0001t\n\uFFFF\u0004\uFFFE\u0001\uFFFF\u0001\uFFFE\u0001w\u0001\uFFFF\u0001h\u0001s\u0001n\u0002e\u0001c\u0001t\u0001m\u0001e\u0001a\u0001s\u0001\uFFFE\u0001t\u0001s\u0001c\u0001m\u0001l\u0001u\u0002i\u0002e\u0001o\u0002\uFFFF\u0001n\u0001t\u0001d\u0001e\u0002\uFFFE\u0001b\u0002\uFFFE\u0001l\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001e\u0001\uFFFF\u0001p\u0001h\u0001e\u0001\uFFFE\u0001p\u0001e\u0001\uFFFE\u0002e\u0001i\u0001n\u0001o\u0001a\u0001s\u0001r\u0001h\u0003\uFFFF\u0001n\u0002\uFFFF\u0001e\u0001\uFFFE\u0001s\u0001t\u0001\uFFFE\u0001t\u0001\uFFFE\u0001i\u0001e\u0001\uFFFE\u0001p\u0001t\u0001\uFFFF\u0001y\u0001e\u0001h\u0002\uFFFE\u0001p\u0001n\u0001c\u0002r\u0003\uFFFE\u0001i\u0001\uFFFE\u0002\uFFFF\u0001e\u0002\uFFFF\u0001\uFFFE\u0002r\u0001c\u0001e\u0001t\u0001c\u0001\uFFFF\u0002\uFFFE\u0001\uFFFF\u0002\uFFFE\u0001l\u0001\uFFFE\u0001n\u0001t\u0001i\u0001e\u0002\uFFFE\u0001d\u0001e\u0001\uFFFF\u0002\uFFFE\u0001\uFFFF\u0001e\u0001n\u0001\uFFFF\u0002n\u0001\uFFFF\u0001e\u0001s\u0003\uFFFE\u0002\uFFFF\u0001\uFFFE\u0001g\u0001e\u0001\uFFFE\u0001t\u0003\uFFFF\u0001n\u0001\uFFFF\u0001r\u0001\uFFFF\u0002\uFFFE\u0001t\u0001r\u0001\uFFFE\u0001t\u0004\uFFFF\u0001i\u0001\uFFFF\u0001\uFFFE\u0001e\u0001o\u0001\uFFFE\u0002\uFFFF\u0001i\u0001n\u0002\uFFFF\u0001\uFFFE\u0001d\u0001c\u0001t\u0002\uFFFE\u0004\uFFFF\u0001\uFFFE\u0001s\u0001\uFFFF\u0001\uFFFE\u0001g\u0001\uFFFE\u0002\uFFFF\u0001\uFFFE\u0001t\u0001\uFFFF\u0001\uFFFE\u0001n\u0001\uFFFF\u0001\uFFFE\u0001n\u0001\uFFFF\u0001n\u0001\uFFFE\u0001\uFFFF\u0001i\u0001t\u0001s\u0003\uFFFF\u0001\uFFFE\u0001\uFFFF\u0001\uFFFE\u0002\uFFFF\u0001i\u0001\uFFFF\u0001g\u0001\uFFFF\u0001e\u0001g\u0001\uFFFF\u0001n\u0002\uFFFE\u0002\uFFFF\u0001e\u0001\uFFFE\u0001d\u0001\uFFFE\u0001g\u0002\uFFFF\u0001s\u0001\uFFFF\u0001\uFFFE\u0001\uFFFF\u0002\uFFFE\u0003\uFFFF");
    private static readonly short[] DFA23_accept = DFA.UnpackEncodedString("\u0016\uFFFF\u0001E\u0005\uFFFF\u0001N\u0001P\u0001Q\u0001R\u0001S\u0001T\u0001V\u0001W\u0001X\u0001Y\u0001Z\u0001[\u0001\\\u0001]\u0001^\u0001_5\uFFFF\u0001H\u0001J\u0001F\u0001K\u0001G\u0001I\u0001O\u0001M\u0001U\u0001L\u0004\uFFFF\u0001\u0004\u0002\uFFFF\u00014\u0017\uFFFF\u0001\u0016\u0001\u001B\v\uFFFF\u0001$\u0001\uFFFF\u0001:\u0001\uFFFF\u0001A\u0010\uFFFF\u0001\u0001\u0001\u0002\u0001\u0003\u0001\uFFFF\u0001\u0005\u0001\u0006\f\uFFFF\u00016\u000F\uFFFF\u0001\u001F\u0001 \u0001\uFFFF\u0001!\u0001\"\a\uFFFF\u0001*\u0002\uFFFF\u0001-\f\uFFFF\u0001<\u0002\uFFFF\u00015\u0002\uFFFF\u0001\v\u0002\uFFFF\u00017\u0005\uFFFF\u0001\u0012\u0001\u0013\u0005\uFFFF\u0001\u001A\u0001\u001C\u0001\u001D\u0001\uFFFF\u0001\u001E\u0001\uFFFF\u0001#\u0006\uFFFF\u0001+\u0001,\u0001.\u0001/\u0001\uFFFF\u00018\u0004\uFFFF\u00019\u0001;\u0002\uFFFF\u0001\b\u0001\t\u0006\uFFFF\u0001=\u0001\u0010\u0001\u0011\u0001\u0014\u0002\uFFFF\u0001\u0018\u0003\uFFFF\u0001%\u0001&\u0002\uFFFF\u0001(\u0002\uFFFF\u00010\u0002\uFFFF\u00013\u0002\uFFFF\u0001\n\u0003\uFFFF\u0001\u000E\u0001\u000F\u0001\u0015\u0001\uFFFF\u0001\u0019\u0001\uFFFF\u0001?\u0001@\u0001\uFFFF\u0001)\u0001\uFFFF\u00011\u0002\uFFFF\u0001\a\u0003\uFFFF\u0001\u0017\u0001>\u0005\uFFFF\u0001\f\u0001\r\u0001\uFFFF\u0001B\u0001\uFFFF\u0001C\u0002\uFFFF\u00012\u0001D\u0001'");
    private static readonly short[] DFA23_special = DFA.UnpackEncodedString("Ţ\uFFFF}>");
    private static readonly short[][] DFA23_transition = DFA.UnpackEncodedStringArray(HqlLexer.DFA23_transitionS);

    public HqlLexer() => this.InitializeCyclicDFAs();

    public HqlLexer(ICharStream input)
      : this(input, (RecognizerSharedState) null)
    {
    }

    public HqlLexer(ICharStream input, RecognizerSharedState state)
      : base(input, state)
    {
      this.InitializeCyclicDFAs();
    }

    public override string GrammarFileName => "Hql.g";

    public void mALL()
    {
      int num1 = 4;
      int num2 = 0;
      this.Match("all");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mANY()
    {
      int num1 = 5;
      int num2 = 0;
      this.Match("any");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mAND()
    {
      int num1 = 6;
      int num2 = 0;
      this.Match("and");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mAS()
    {
      int num1 = 7;
      int num2 = 0;
      this.Match("as");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mASCENDING()
    {
      int num1 = 8;
      int num2 = 0;
      this.Match("asc");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mAVG()
    {
      int num1 = 9;
      int num2 = 0;
      this.Match("avg");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mBETWEEN()
    {
      int num1 = 10;
      int num2 = 0;
      this.Match("between");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCLASS()
    {
      int num1 = 11;
      int num2 = 0;
      this.Match("class");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCOUNT()
    {
      int num1 = 12;
      int num2 = 0;
      this.Match("count");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mDELETE()
    {
      int num1 = 13;
      int num2 = 0;
      this.Match("delete");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mDESCENDING()
    {
      int num1 = 14;
      int num2 = 0;
      this.Match("desc");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mDISTINCT()
    {
      int num1 = 16;
      int num2 = 0;
      this.Match("distinct");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mELEMENTS()
    {
      int num1 = 17;
      int num2 = 0;
      this.Match("elements");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mESCAPE()
    {
      int num1 = 18;
      int num2 = 0;
      this.Match("escape");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mEXISTS()
    {
      int num1 = 19;
      int num2 = 0;
      this.Match("exists");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mFALSE()
    {
      int num1 = 20;
      int num2 = 0;
      this.Match("false");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mFETCH()
    {
      int num1 = 21;
      int num2 = 0;
      this.Match("fetch");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mFROM()
    {
      int num1 = 22;
      int num2 = 0;
      this.Match("from");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mFULL()
    {
      int num1 = 23;
      int num2 = 0;
      this.Match("full");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mGROUP()
    {
      int num1 = 24;
      int num2 = 0;
      this.Match("group");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mHAVING()
    {
      int num1 = 25;
      int num2 = 0;
      this.Match("having");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mIN()
    {
      int num1 = 26;
      int num2 = 0;
      this.Match("in");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mINDICES()
    {
      int num1 = 27;
      int num2 = 0;
      this.Match("indices");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mINNER()
    {
      int num1 = 28;
      int num2 = 0;
      this.Match("inner");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mINSERT()
    {
      int num1 = 29;
      int num2 = 0;
      this.Match("insert");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mINTO()
    {
      int num1 = 30;
      int num2 = 0;
      this.Match("into");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mIS()
    {
      int num1 = 31;
      int num2 = 0;
      this.Match("is");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mJOIN()
    {
      int num1 = 32;
      int num2 = 0;
      this.Match("join");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mLEFT()
    {
      int num1 = 33;
      int num2 = 0;
      this.Match("left");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mLIKE()
    {
      int num1 = 34;
      int num2 = 0;
      this.Match("like");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mMAX()
    {
      int num1 = 35;
      int num2 = 0;
      this.Match("max");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mMIN()
    {
      int num1 = 36;
      int num2 = 0;
      this.Match("min");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mNEW()
    {
      int num1 = 37;
      int num2 = 0;
      this.Match("new");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mNOT()
    {
      int num1 = 38;
      int num2 = 0;
      this.Match("not");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mNULL()
    {
      int num1 = 39;
      int num2 = 0;
      this.Match("null");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mOR()
    {
      int num1 = 40;
      int num2 = 0;
      this.Match("or");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mORDER()
    {
      int num1 = 41;
      int num2 = 0;
      this.Match("order");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mOUTER()
    {
      int num1 = 42;
      int num2 = 0;
      this.Match("outer");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mPROPERTIES()
    {
      int num1 = 43;
      int num2 = 0;
      this.Match("properties");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mRIGHT()
    {
      int num1 = 44;
      int num2 = 0;
      this.Match("right");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mSELECT()
    {
      int num1 = 45;
      int num2 = 0;
      this.Match("select");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mSET()
    {
      int num1 = 46;
      int num2 = 0;
      this.Match("set");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mSKIP()
    {
      int num1 = 47;
      int num2 = 0;
      this.Match("skip");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mSOME()
    {
      int num1 = 48;
      int num2 = 0;
      this.Match("some");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mSUM()
    {
      int num1 = 49;
      int num2 = 0;
      this.Match("sum");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mTAKE()
    {
      int num1 = 50;
      int num2 = 0;
      this.Match("take");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mTRUE()
    {
      int num1 = 51;
      int num2 = 0;
      this.Match("true");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mUNION()
    {
      int num1 = 52;
      int num2 = 0;
      this.Match("union");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mUPDATE()
    {
      int num1 = 53;
      int num2 = 0;
      this.Match("update");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mVERSIONED()
    {
      int num1 = 54;
      int num2 = 0;
      this.Match("versioned");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mWHERE()
    {
      int num1 = 55;
      int num2 = 0;
      this.Match("where");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mLITERAL_by()
    {
      int num1 = 56;
      int num2 = 0;
      this.Match("by");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCASE()
    {
      int num1 = 57;
      int num2 = 0;
      this.Match("case");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mEND()
    {
      int num1 = 58;
      int num2 = 0;
      this.Match("end");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mELSE()
    {
      int num1 = 59;
      int num2 = 0;
      this.Match("else");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mTHEN()
    {
      int num1 = 60;
      int num2 = 0;
      this.Match("then");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mWHEN()
    {
      int num1 = 61;
      int num2 = 0;
      this.Match("when");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mON()
    {
      int num1 = 62;
      int num2 = 0;
      this.Match("on");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mWITH()
    {
      int num1 = 63;
      int num2 = 0;
      this.Match("with");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mBOTH()
    {
      int num1 = 64;
      int num2 = 0;
      this.Match("both");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mEMPTY()
    {
      int num1 = 65;
      int num2 = 0;
      this.Match("empty");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mLEADING()
    {
      int num1 = 66;
      int num2 = 0;
      this.Match("leading");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mMEMBER()
    {
      int num1 = 67;
      int num2 = 0;
      this.Match("member");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mOBJECT()
    {
      int num1 = 68;
      int num2 = 0;
      this.Match("object");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mOF()
    {
      int num1 = 69;
      int num2 = 0;
      this.Match("of");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mTRAILING()
    {
      int num1 = 70;
      int num2 = 0;
      this.Match("trailing");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mT__133()
    {
      int num1 = 133;
      int num2 = 0;
      this.Match("ascending");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mT__134()
    {
      int num1 = 134;
      int num2 = 0;
      this.Match("descending");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mEQ()
    {
      int num1 = 102;
      int num2 = 0;
      this.Match(61);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mLT()
    {
      int num1 = 109;
      int num2 = 0;
      this.Match(60);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mGT()
    {
      int num1 = 110;
      int num2 = 0;
      this.Match(62);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mSQL_NE()
    {
      int num1 = 108;
      int num2 = 0;
      this.Match("<>");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mNE()
    {
      int num1 = 107;
      int num2 = 0;
      int num3;
      switch (this.input.LA(1))
      {
        case 33:
          num3 = 1;
          break;
        case 94:
          num3 = 2;
          break;
        default:
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 1, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
      }
      switch (num3)
      {
        case 1:
          this.Match("!=");
          if (this.state.failed)
            return;
          break;
        case 2:
          this.Match("^=");
          if (this.state.failed)
            return;
          break;
      }
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mLE()
    {
      int num1 = 111;
      int num2 = 0;
      this.Match("<=");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mGE()
    {
      int num1 = 112;
      int num2 = 0;
      this.Match(">=");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mBOR()
    {
      int num1 = 115;
      int num2 = 0;
      this.Match(124);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mBXOR()
    {
      int num1 = 116;
      int num2 = 0;
      this.Match(94);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mBAND()
    {
      int num1 = 117;
      int num2 = 0;
      this.Match(38);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mBNOT()
    {
      int num1 = 114;
      int num2 = 0;
      this.Match(33);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCOMMA()
    {
      int num1 = 101;
      int num2 = 0;
      this.Match(44);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mOPEN()
    {
      int num1 = 103;
      int num2 = 0;
      this.Match(40);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCLOSE()
    {
      int num1 = 104;
      int num2 = 0;
      this.Match(41);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mOPEN_BRACKET()
    {
      int num1 = 122;
      int num2 = 0;
      this.Match(91);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCLOSE_BRACKET()
    {
      int num1 = 123;
      int num2 = 0;
      this.Match(93);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCONCAT()
    {
      int num1 = 113;
      int num2 = 0;
      this.Match("||");
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mPLUS()
    {
      int num1 = 118;
      int num2 = 0;
      this.Match(43);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mMINUS()
    {
      int num1 = 119;
      int num2 = 0;
      this.Match(45);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mSTAR()
    {
      int num1 = 120;
      int num2 = 0;
      this.Match(42);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mDIV()
    {
      int num1 = 121;
      int num2 = 0;
      this.Match(47);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mCOLON()
    {
      int num1 = 105;
      int num2 = 0;
      this.Match(58);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mPARAM()
    {
      int num1 = 106;
      int num2 = 0;
      this.Match(63);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mIDENT()
    {
      int num1 = 125;
      int num2 = 0;
      this.mID_START_LETTER();
      if (this.state.failed)
        return;
      do
      {
        int num3 = 2;
        int num4 = this.input.LA(1);
        if (num4 == 36 || num4 >= 48 && num4 <= 57 || num4 >= 65 && num4 <= 90 || num4 == 95 || num4 >= 97 && num4 <= 122 || num4 >= 128 && num4 <= 65534)
          num3 = 1;
        if (num3 == 1)
          this.mID_LETTER();
        else
          goto label_6;
      }
      while (!this.state.failed);
      return;
label_6:
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mID_START_LETTER()
    {
      if (this.input.LA(1) == 36 || this.input.LA(1) >= 65 && this.input.LA(1) <= 90 || this.input.LA(1) == 95 || this.input.LA(1) >= 97 && this.input.LA(1) <= 122 || this.input.LA(1) >= 128 && this.input.LA(1) <= 65534)
      {
        this.input.Consume();
        this.state.failed = false;
      }
      else if (this.state.backtracking > 0)
      {
        this.state.failed = true;
      }
      else
      {
        MismatchedSetException re = new MismatchedSetException((BitSet) null, (IIntStream) this.input);
        this.Recover((RecognitionException) re);
        throw re;
      }
    }

    public void mID_LETTER()
    {
      if (this.input.LA(1) == 36 || this.input.LA(1) >= 48 && this.input.LA(1) <= 57 || this.input.LA(1) >= 65 && this.input.LA(1) <= 90 || this.input.LA(1) == 95 || this.input.LA(1) >= 97 && this.input.LA(1) <= 122 || this.input.LA(1) >= 128 && this.input.LA(1) <= 65534)
      {
        this.input.Consume();
        this.state.failed = false;
      }
      else if (this.state.backtracking > 0)
      {
        this.state.failed = true;
      }
      else
      {
        MismatchedSetException re = new MismatchedSetException((BitSet) null, (IIntStream) this.input);
        this.Recover((RecognitionException) re);
        throw re;
      }
    }

    public void mQUOTED_String()
    {
      int num1 = 124;
      int num2 = 0;
      this.Match(39);
      if (this.state.failed)
        return;
      while (true)
      {
        do
        {
          int num3 = 3;
          int num4 = this.input.LA(1);
          if (num4 == 39)
          {
            if (this.input.LA(2) == 39 && this.synpred1_Hql())
              num3 = 1;
          }
          else if (num4 >= 0 && num4 <= 38 || num4 >= 40 && num4 <= (int) ushort.MaxValue)
            num3 = 2;
          switch (num3)
          {
            case 1:
              this.mESCqs();
              continue;
            case 2:
              goto label_10;
            default:
              goto label_15;
          }
        }
        while (!this.state.failed);
        break;
label_10:
        if (this.input.LA(1) >= 0 && this.input.LA(1) <= 38 || this.input.LA(1) >= 40 && this.input.LA(1) <= (int) ushort.MaxValue)
        {
          this.input.Consume();
          this.state.failed = false;
        }
        else
          goto label_12;
      }
      return;
label_12:
      if (this.state.backtracking > 0)
      {
        this.state.failed = true;
        return;
      }
      MismatchedSetException re = new MismatchedSetException((BitSet) null, (IIntStream) this.input);
      this.Recover((RecognitionException) re);
      throw re;
label_15:
      this.Match(39);
      if (this.state.failed)
        return;
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mESCqs()
    {
      this.Match(39);
      if (this.state.failed)
        return;
      this.Match(39);
      if (!this.state.failed)
        ;
    }

    public void mWS()
    {
      int num1 = 129;
      int num2 = 0;
      int num3;
      switch (this.input.LA(1))
      {
        case 9:
          num3 = 2;
          break;
        case 10:
          num3 = 4;
          break;
        case 13:
          num3 = this.input.LA(2) != 10 ? 5 : 3;
          break;
        case 32:
          num3 = 1;
          break;
        default:
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 4, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
      }
      switch (num3)
      {
        case 1:
          this.Match(32);
          if (this.state.failed)
            return;
          break;
        case 2:
          this.Match(9);
          if (this.state.failed)
            return;
          break;
        case 3:
          this.Match(13);
          if (this.state.failed)
            return;
          this.Match(10);
          if (this.state.failed)
            return;
          break;
        case 4:
          this.Match(10);
          if (this.state.failed)
            return;
          break;
        case 5:
          this.Match(13);
          if (this.state.failed)
            return;
          break;
      }
      if (this.state.backtracking == 0)
        this.Skip();
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mNUM_INT()
    {
      int num1 = 95;
      int num2 = 0;
      bool flag = false;
      IToken token1 = (IToken) null;
      int num3;
      switch (this.input.LA(1))
      {
        case 46:
          num3 = 1;
          break;
        case 48:
        case 49:
        case 50:
        case 51:
        case 52:
        case 53:
        case 54:
        case 55:
        case 56:
        case 57:
          num3 = 2;
          break;
        default:
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 20, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
      }
      switch (num3)
      {
        case 1:
          this.Match(46);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            num1 = 15;
          int num4 = 2;
          switch (this.input.LA(1))
          {
            case 48:
            case 49:
            case 50:
            case 51:
            case 52:
            case 53:
            case 54:
            case 55:
            case 56:
            case 57:
              num4 = 1;
              break;
          }
          if (num4 == 1)
          {
            int num5 = 0;
            while (true)
            {
              int num6 = 2;
              switch (this.input.LA(1))
              {
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                  num6 = 1;
                  break;
              }
              if (num6 == 1)
              {
                this.MatchRange(48, 57);
                if (!this.state.failed)
                  ++num5;
                else
                  break;
              }
              else
                goto label_21;
            }
            return;
label_21:
            if (num5 < 1)
            {
              if (this.state.backtracking <= 0)
                throw new EarlyExitException(5, (IIntStream) this.input);
              this.state.failed = true;
              return;
            }
            int num7 = 2;
            if (this.input.LA(1) == 101)
              num7 = 1;
            if (num7 == 1)
            {
              this.mEXPONENT();
              if (this.state.failed)
                return;
            }
            int num8 = 2;
            switch (this.input.LA(1))
            {
              case 100:
              case 102:
              case 109:
                num8 = 1;
                break;
            }
            if (num8 == 1)
            {
              int charIndex = this.CharIndex;
              this.mFLOAT_SUFFIX();
              if (this.state.failed)
                return;
              IToken token2 = (IToken) new CommonToken(this.input, 0, 0, charIndex, this.CharIndex - 1);
              if (this.state.backtracking == 0)
                token1 = token2;
            }
            if (this.state.backtracking == 0)
            {
              num1 = token1 == null || token1.Text.ToUpperInvariant().IndexOf('F') < 0 ? (token1 == null || token1.Text.ToUpperInvariant().IndexOf('M') < 0 ? 96 : 97) : 98;
              break;
            }
            break;
          }
          break;
        case 2:
          int num9;
          switch (this.input.LA(1))
          {
            case 48:
              num9 = 1;
              break;
            case 49:
            case 50:
            case 51:
            case 52:
            case 53:
            case 54:
            case 55:
            case 56:
            case 57:
              num9 = 2;
              break;
            default:
              if (this.state.backtracking <= 0)
                throw new NoViableAltException("", 13, 0, (IIntStream) this.input);
              this.state.failed = true;
              return;
          }
          switch (num9)
          {
            case 1:
              this.Match(48);
              if (this.state.failed)
                return;
              if (this.state.backtracking == 0)
                flag = true;
              int num10 = 3;
              switch (this.input.LA(1))
              {
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                  num10 = 2;
                  break;
                case 120:
                  num10 = 1;
                  break;
              }
              switch (num10)
              {
                case 1:
                  this.Match(120);
                  if (this.state.failed)
                    return;
                  int num11 = 0;
                  while (true)
                  {
                    int num12 = 2;
                    switch (this.input.LA(1))
                    {
                      case 48:
                      case 49:
                      case 50:
                      case 51:
                      case 52:
                      case 53:
                      case 54:
                      case 55:
                      case 56:
                      case 57:
                      case 97:
                      case 98:
                      case 99:
                        num12 = 1;
                        break;
                      case 100:
                      case 102:
                        this.input.LA(2);
                        if (!flag)
                        {
                          num12 = 1;
                          break;
                        }
                        break;
                      case 101:
                        switch (this.input.LA(2))
                        {
                          case 48:
                          case 49:
                          case 50:
                          case 51:
                          case 52:
                          case 53:
                          case 54:
                          case 55:
                          case 56:
                          case 57:
                            this.input.LA(3);
                            if (!flag)
                            {
                              num12 = 1;
                              break;
                            }
                            break;
                          default:
                            num12 = 1;
                            break;
                        }
                        break;
                    }
                    if (num12 == 1)
                    {
                      this.mHEX_DIGIT();
                      if (!this.state.failed)
                        ++num11;
                      else
                        break;
                    }
                    else
                      goto label_72;
                  }
                  return;
label_72:
                  if (num11 < 1)
                  {
                    if (this.state.backtracking <= 0)
                      throw new EarlyExitException(9, (IIntStream) this.input);
                    this.state.failed = true;
                    return;
                  }
                  break;
                case 2:
                  int num13 = 0;
                  while (true)
                  {
                    int num14 = 2;
                    switch (this.input.LA(1))
                    {
                      case 48:
                      case 49:
                      case 50:
                      case 51:
                      case 52:
                      case 53:
                      case 54:
                      case 55:
                        num14 = 1;
                        break;
                    }
                    if (num14 == 1)
                    {
                      this.MatchRange(48, 55);
                      if (!this.state.failed)
                        ++num13;
                      else
                        break;
                    }
                    else
                      goto label_83;
                  }
                  return;
label_83:
                  if (num13 < 1)
                  {
                    if (this.state.backtracking <= 0)
                      throw new EarlyExitException(10, (IIntStream) this.input);
                    this.state.failed = true;
                    return;
                  }
                  break;
              }
              break;
            case 2:
              this.MatchRange(49, 57);
              if (this.state.failed)
                return;
              do
              {
                int num15 = 2;
                switch (this.input.LA(1))
                {
                  case 48:
                  case 49:
                  case 50:
                  case 51:
                  case 52:
                  case 53:
                  case 54:
                  case 55:
                  case 56:
                  case 57:
                    num15 = 1;
                    break;
                }
                if (num15 == 1)
                  this.MatchRange(48, 57);
                else
                  goto label_95;
              }
              while (!this.state.failed);
              return;
label_95:
              if (this.state.backtracking == 0)
              {
                flag = true;
                break;
              }
              break;
          }
          int num16 = 3;
          int num17 = this.input.LA(1);
          if (num17 == 108)
            num16 = 1;
          else if (num17 == 46 || num17 >= 100 && num17 <= 102 || num17 == 109)
            num16 = 2;
          switch (num16)
          {
            case 1:
              this.Match(108);
              if (this.state.failed)
                return;
              if (this.state.backtracking == 0)
              {
                num1 = 99;
                break;
              }
              break;
            case 2:
              if (!flag)
              {
                if (this.state.backtracking <= 0)
                  throw new FailedPredicateException((IIntStream) this.input, "NUM_INT", "isDecimal");
                this.state.failed = true;
                return;
              }
              int num18;
              switch (this.input.LA(1))
              {
                case 46:
                  num18 = 1;
                  break;
                case 100:
                case 102:
                case 109:
                  num18 = 3;
                  break;
                case 101:
                  num18 = 2;
                  break;
                default:
                  if (this.state.backtracking <= 0)
                    throw new NoViableAltException("", 18, 0, (IIntStream) this.input);
                  this.state.failed = true;
                  return;
              }
              switch (num18)
              {
                case 1:
                  this.Match(46);
                  if (this.state.failed)
                    return;
                  do
                  {
                    int num19 = 2;
                    switch (this.input.LA(1))
                    {
                      case 48:
                      case 49:
                      case 50:
                      case 51:
                      case 52:
                      case 53:
                      case 54:
                      case 55:
                      case 56:
                      case 57:
                        num19 = 1;
                        break;
                    }
                    if (num19 == 1)
                      this.MatchRange(48, 57);
                    else
                      goto label_125;
                  }
                  while (!this.state.failed);
                  return;
label_125:
                  int num20 = 2;
                  if (this.input.LA(1) == 101)
                    num20 = 1;
                  if (num20 == 1)
                  {
                    this.mEXPONENT();
                    if (this.state.failed)
                      return;
                  }
                  int num21 = 2;
                  switch (this.input.LA(1))
                  {
                    case 100:
                    case 102:
                    case 109:
                      num21 = 1;
                      break;
                  }
                  if (num21 == 1)
                  {
                    int charIndex = this.CharIndex;
                    this.mFLOAT_SUFFIX();
                    if (this.state.failed)
                      return;
                    IToken token3 = (IToken) new CommonToken(this.input, 0, 0, charIndex, this.CharIndex - 1);
                    if (this.state.backtracking == 0)
                    {
                      token1 = token3;
                      break;
                    }
                    break;
                  }
                  break;
                case 2:
                  this.mEXPONENT();
                  if (this.state.failed)
                    return;
                  int num22 = 2;
                  switch (this.input.LA(1))
                  {
                    case 100:
                    case 102:
                    case 109:
                      num22 = 1;
                      break;
                  }
                  if (num22 == 1)
                  {
                    int charIndex = this.CharIndex;
                    this.mFLOAT_SUFFIX();
                    if (this.state.failed)
                      return;
                    IToken token4 = (IToken) new CommonToken(this.input, 0, 0, charIndex, this.CharIndex - 1);
                    if (this.state.backtracking == 0)
                    {
                      token1 = token4;
                      break;
                    }
                    break;
                  }
                  break;
                case 3:
                  int charIndex1 = this.CharIndex;
                  this.mFLOAT_SUFFIX();
                  if (this.state.failed)
                    return;
                  IToken token5 = (IToken) new CommonToken(this.input, 0, 0, charIndex1, this.CharIndex - 1);
                  if (this.state.backtracking == 0)
                  {
                    token1 = token5;
                    break;
                  }
                  break;
              }
              if (this.state.backtracking == 0)
              {
                num1 = token1 == null || token1.Text.ToUpperInvariant().IndexOf('F') < 0 ? (token1 == null || token1.Text.ToUpperInvariant().IndexOf('M') < 0 ? 96 : 97) : 98;
                break;
              }
              break;
          }
          break;
      }
      this.state.type = num1;
      this.state.channel = num2;
    }

    public void mHEX_DIGIT()
    {
      if (this.input.LA(1) >= 48 && this.input.LA(1) <= 57 || this.input.LA(1) >= 97 && this.input.LA(1) <= 102)
      {
        this.input.Consume();
        this.state.failed = false;
      }
      else if (this.state.backtracking > 0)
      {
        this.state.failed = true;
      }
      else
      {
        MismatchedSetException re = new MismatchedSetException((BitSet) null, (IIntStream) this.input);
        this.Recover((RecognitionException) re);
        throw re;
      }
    }

    public void mEXPONENT()
    {
      this.Match(101);
      if (this.state.failed)
        return;
      int num1 = 2;
      switch (this.input.LA(1))
      {
        case 43:
        case 45:
          num1 = 1;
          break;
      }
      if (num1 == 1)
      {
        if (this.input.LA(1) == 43 || this.input.LA(1) == 45)
        {
          this.input.Consume();
          this.state.failed = false;
        }
        else
        {
          if (this.state.backtracking > 0)
          {
            this.state.failed = true;
            return;
          }
          MismatchedSetException re = new MismatchedSetException((BitSet) null, (IIntStream) this.input);
          this.Recover((RecognitionException) re);
          throw re;
        }
      }
      int num2 = 0;
      while (true)
      {
        int num3 = 2;
        switch (this.input.LA(1))
        {
          case 48:
          case 49:
          case 50:
          case 51:
          case 52:
          case 53:
          case 54:
          case 55:
          case 56:
          case 57:
            num3 = 1;
            break;
        }
        if (num3 == 1)
        {
          this.MatchRange(48, 57);
          if (!this.state.failed)
            ++num2;
          else
            break;
        }
        else
          goto label_15;
      }
      return;
label_15:
      if (num2 >= 1)
        return;
      if (this.state.backtracking <= 0)
        throw new EarlyExitException(22, (IIntStream) this.input);
      this.state.failed = true;
    }

    public void mFLOAT_SUFFIX()
    {
      if (this.input.LA(1) == 100 || this.input.LA(1) == 102 || this.input.LA(1) == 109)
      {
        this.input.Consume();
        this.state.failed = false;
      }
      else if (this.state.backtracking > 0)
      {
        this.state.failed = true;
      }
      else
      {
        MismatchedSetException re = new MismatchedSetException((BitSet) null, (IIntStream) this.input);
        this.Recover((RecognitionException) re);
        throw re;
      }
    }

    public override void mTokens()
    {
      switch (this.dfa23.Predict((IIntStream) this.input))
      {
        case 1:
          this.mALL();
          if (!this.state.failed)
            break;
          break;
        case 2:
          this.mANY();
          if (!this.state.failed)
            break;
          break;
        case 3:
          this.mAND();
          if (!this.state.failed)
            break;
          break;
        case 4:
          this.mAS();
          if (!this.state.failed)
            break;
          break;
        case 5:
          this.mASCENDING();
          if (!this.state.failed)
            break;
          break;
        case 6:
          this.mAVG();
          if (!this.state.failed)
            break;
          break;
        case 7:
          this.mBETWEEN();
          if (!this.state.failed)
            break;
          break;
        case 8:
          this.mCLASS();
          if (!this.state.failed)
            break;
          break;
        case 9:
          this.mCOUNT();
          if (!this.state.failed)
            break;
          break;
        case 10:
          this.mDELETE();
          if (!this.state.failed)
            break;
          break;
        case 11:
          this.mDESCENDING();
          if (!this.state.failed)
            break;
          break;
        case 12:
          this.mDISTINCT();
          if (!this.state.failed)
            break;
          break;
        case 13:
          this.mELEMENTS();
          if (!this.state.failed)
            break;
          break;
        case 14:
          this.mESCAPE();
          if (!this.state.failed)
            break;
          break;
        case 15:
          this.mEXISTS();
          if (!this.state.failed)
            break;
          break;
        case 16:
          this.mFALSE();
          if (!this.state.failed)
            break;
          break;
        case 17:
          this.mFETCH();
          if (!this.state.failed)
            break;
          break;
        case 18:
          this.mFROM();
          if (!this.state.failed)
            break;
          break;
        case 19:
          this.mFULL();
          if (!this.state.failed)
            break;
          break;
        case 20:
          this.mGROUP();
          if (!this.state.failed)
            break;
          break;
        case 21:
          this.mHAVING();
          if (!this.state.failed)
            break;
          break;
        case 22:
          this.mIN();
          if (!this.state.failed)
            break;
          break;
        case 23:
          this.mINDICES();
          if (!this.state.failed)
            break;
          break;
        case 24:
          this.mINNER();
          if (!this.state.failed)
            break;
          break;
        case 25:
          this.mINSERT();
          if (!this.state.failed)
            break;
          break;
        case 26:
          this.mINTO();
          if (!this.state.failed)
            break;
          break;
        case 27:
          this.mIS();
          if (!this.state.failed)
            break;
          break;
        case 28:
          this.mJOIN();
          if (!this.state.failed)
            break;
          break;
        case 29:
          this.mLEFT();
          if (!this.state.failed)
            break;
          break;
        case 30:
          this.mLIKE();
          if (!this.state.failed)
            break;
          break;
        case 31:
          this.mMAX();
          if (!this.state.failed)
            break;
          break;
        case 32:
          this.mMIN();
          if (!this.state.failed)
            break;
          break;
        case 33:
          this.mNEW();
          if (!this.state.failed)
            break;
          break;
        case 34:
          this.mNOT();
          if (!this.state.failed)
            break;
          break;
        case 35:
          this.mNULL();
          if (!this.state.failed)
            break;
          break;
        case 36:
          this.mOR();
          if (!this.state.failed)
            break;
          break;
        case 37:
          this.mORDER();
          if (!this.state.failed)
            break;
          break;
        case 38:
          this.mOUTER();
          if (!this.state.failed)
            break;
          break;
        case 39:
          this.mPROPERTIES();
          if (!this.state.failed)
            break;
          break;
        case 40:
          this.mRIGHT();
          if (!this.state.failed)
            break;
          break;
        case 41:
          this.mSELECT();
          if (!this.state.failed)
            break;
          break;
        case 42:
          this.mSET();
          if (!this.state.failed)
            break;
          break;
        case 43:
          this.mSKIP();
          if (!this.state.failed)
            break;
          break;
        case 44:
          this.mSOME();
          if (!this.state.failed)
            break;
          break;
        case 45:
          this.mSUM();
          if (!this.state.failed)
            break;
          break;
        case 46:
          this.mTAKE();
          if (!this.state.failed)
            break;
          break;
        case 47:
          this.mTRUE();
          if (!this.state.failed)
            break;
          break;
        case 48:
          this.mUNION();
          if (!this.state.failed)
            break;
          break;
        case 49:
          this.mUPDATE();
          if (!this.state.failed)
            break;
          break;
        case 50:
          this.mVERSIONED();
          if (!this.state.failed)
            break;
          break;
        case 51:
          this.mWHERE();
          if (!this.state.failed)
            break;
          break;
        case 52:
          this.mLITERAL_by();
          if (!this.state.failed)
            break;
          break;
        case 53:
          this.mCASE();
          if (!this.state.failed)
            break;
          break;
        case 54:
          this.mEND();
          if (!this.state.failed)
            break;
          break;
        case 55:
          this.mELSE();
          if (!this.state.failed)
            break;
          break;
        case 56:
          this.mTHEN();
          if (!this.state.failed)
            break;
          break;
        case 57:
          this.mWHEN();
          if (!this.state.failed)
            break;
          break;
        case 58:
          this.mON();
          if (!this.state.failed)
            break;
          break;
        case 59:
          this.mWITH();
          if (!this.state.failed)
            break;
          break;
        case 60:
          this.mBOTH();
          if (!this.state.failed)
            break;
          break;
        case 61:
          this.mEMPTY();
          if (!this.state.failed)
            break;
          break;
        case 62:
          this.mLEADING();
          if (!this.state.failed)
            break;
          break;
        case 63:
          this.mMEMBER();
          if (!this.state.failed)
            break;
          break;
        case 64:
          this.mOBJECT();
          if (!this.state.failed)
            break;
          break;
        case 65:
          this.mOF();
          if (!this.state.failed)
            break;
          break;
        case 66:
          this.mTRAILING();
          if (!this.state.failed)
            break;
          break;
        case 67:
          this.mT__133();
          if (!this.state.failed)
            break;
          break;
        case 68:
          this.mT__134();
          if (!this.state.failed)
            break;
          break;
        case 69:
          this.mEQ();
          if (!this.state.failed)
            break;
          break;
        case 70:
          this.mLT();
          if (!this.state.failed)
            break;
          break;
        case 71:
          this.mGT();
          if (!this.state.failed)
            break;
          break;
        case 72:
          this.mSQL_NE();
          if (!this.state.failed)
            break;
          break;
        case 73:
          this.mNE();
          if (!this.state.failed)
            break;
          break;
        case 74:
          this.mLE();
          if (!this.state.failed)
            break;
          break;
        case 75:
          this.mGE();
          if (!this.state.failed)
            break;
          break;
        case 76:
          this.mBOR();
          if (!this.state.failed)
            break;
          break;
        case 77:
          this.mBXOR();
          if (!this.state.failed)
            break;
          break;
        case 78:
          this.mBAND();
          if (!this.state.failed)
            break;
          break;
        case 79:
          this.mBNOT();
          if (!this.state.failed)
            break;
          break;
        case 80:
          this.mCOMMA();
          if (!this.state.failed)
            break;
          break;
        case 81:
          this.mOPEN();
          if (!this.state.failed)
            break;
          break;
        case 82:
          this.mCLOSE();
          if (!this.state.failed)
            break;
          break;
        case 83:
          this.mOPEN_BRACKET();
          if (!this.state.failed)
            break;
          break;
        case 84:
          this.mCLOSE_BRACKET();
          if (!this.state.failed)
            break;
          break;
        case 85:
          this.mCONCAT();
          if (!this.state.failed)
            break;
          break;
        case 86:
          this.mPLUS();
          if (!this.state.failed)
            break;
          break;
        case 87:
          this.mMINUS();
          if (!this.state.failed)
            break;
          break;
        case 88:
          this.mSTAR();
          if (!this.state.failed)
            break;
          break;
        case 89:
          this.mDIV();
          if (!this.state.failed)
            break;
          break;
        case 90:
          this.mCOLON();
          if (!this.state.failed)
            break;
          break;
        case 91:
          this.mPARAM();
          if (!this.state.failed)
            break;
          break;
        case 92:
          this.mIDENT();
          if (!this.state.failed)
            break;
          break;
        case 93:
          this.mQUOTED_String();
          if (!this.state.failed)
            break;
          break;
        case 94:
          this.mWS();
          if (!this.state.failed)
            break;
          break;
        case 95:
          this.mNUM_INT();
          int num = this.state.failed ? 1 : 0;
          break;
      }
    }

    public void synpred1_Hql_fragment()
    {
      this.mESCqs();
      int num = this.state.failed ? 1 : 0;
    }

    public bool synpred1_Hql()
    {
      ++this.state.backtracking;
      int marker = this.input.Mark();
      try
      {
        this.synpred1_Hql_fragment();
      }
      catch (RecognitionException ex)
      {
        Console.Error.WriteLine("impossible: " + (object) ex);
      }
      bool flag = !this.state.failed;
      this.input.Rewind(marker);
      --this.state.backtracking;
      this.state.failed = false;
      return flag;
    }

    private void InitializeCyclicDFAs() => this.dfa23 = new HqlLexer.DFA23((BaseRecognizer) this);

    public override IToken Emit()
    {
      HqlToken hqlToken1 = new HqlToken(this.input, this.state.type, this.state.channel, this.state.tokenStartCharIndex, this.CharIndex - 1);
      hqlToken1.Line = this.state.tokenStartLine;
      hqlToken1.Text = this.state.text;
      HqlToken hqlToken2 = hqlToken1;
      this.Emit((IToken) hqlToken2);
      return (IToken) hqlToken2;
    }

    protected class DFA23 : DFA
    {
      public DFA23(BaseRecognizer recognizer)
      {
        this.recognizer = recognizer;
        this.decisionNumber = 23;
        this.eot = HqlLexer.DFA23_eot;
        this.eof = HqlLexer.DFA23_eof;
        this.min = HqlLexer.DFA23_min;
        this.max = HqlLexer.DFA23_max;
        this.accept = HqlLexer.DFA23_accept;
        this.special = HqlLexer.DFA23_special;
        this.transition = HqlLexer.DFA23_transition;
      }

      public override string Description
      {
        get
        {
          return "1:1: Tokens : ( ALL | ANY | AND | AS | ASCENDING | AVG | BETWEEN | CLASS | COUNT | DELETE | DESCENDING | DISTINCT | ELEMENTS | ESCAPE | EXISTS | FALSE | FETCH | FROM | FULL | GROUP | HAVING | IN | INDICES | INNER | INSERT | INTO | IS | JOIN | LEFT | LIKE | MAX | MIN | NEW | NOT | NULL | OR | ORDER | OUTER | PROPERTIES | RIGHT | SELECT | SET | SKIP | SOME | SUM | TAKE | TRUE | UNION | UPDATE | VERSIONED | WHERE | LITERAL_by | CASE | END | ELSE | THEN | WHEN | ON | WITH | BOTH | EMPTY | LEADING | MEMBER | OBJECT | OF | TRAILING | T__133 | T__134 | EQ | LT | GT | SQL_NE | NE | LE | GE | BOR | BXOR | BAND | BNOT | COMMA | OPEN | CLOSE | OPEN_BRACKET | CLOSE_BRACKET | CONCAT | PLUS | MINUS | STAR | DIV | COLON | PARAM | IDENT | QUOTED_String | WS | NUM_INT );";
        }
      }
    }
  }
}
