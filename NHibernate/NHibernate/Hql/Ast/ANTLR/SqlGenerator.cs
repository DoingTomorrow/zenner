// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.SqlGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  public class SqlGenerator : TreeParser, IErrorReporter
  {
    public const int SELECT_COLUMNS = 144;
    public const int EXPONENT = 130;
    public const int LT = 109;
    public const int STAR = 120;
    public const int FLOAT_SUFFIX = 131;
    public const int FILTERS = 147;
    public const int LITERAL_by = 56;
    public const int PROPERTY_REF = 142;
    public const int THETA_JOINS = 146;
    public const int CASE = 57;
    public const int NEW = 37;
    public const int FILTER_ENTITY = 76;
    public const int PARAM = 106;
    public const int COUNT = 12;
    public const int NOT = 38;
    public const int EOF = -1;
    public const int UNARY_PLUS = 91;
    public const int QUOTED_String = 124;
    public const int WEIRD_IDENT = 93;
    public const int ESCqs = 128;
    public const int OPEN_BRACKET = 122;
    public const int FULL = 23;
    public const int ORDER_ELEMENT = 85;
    public const int INSERT = 29;
    public const int ESCAPE = 18;
    public const int IS_NULL = 80;
    public const int FROM_FRAGMENT = 135;
    public const int NAMED_PARAM = 149;
    public const int BOTH = 64;
    public const int SELECT_CLAUSE = 138;
    public const int NUM_DECIMAL = 97;
    public const int EQ = 102;
    public const int VERSIONED = 54;
    public const int SELECT = 45;
    public const int INTO = 30;
    public const int NE = 107;
    public const int GE = 112;
    public const int TAKE = 50;
    public const int ID_LETTER = 127;
    public const int CONCAT = 113;
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
    public const int METHOD_NAME = 148;
    public const int SKIP = 47;
    public const int EMPTY = 65;
    public const int GROUP = 24;
    public const int WS = 129;
    public const int FETCH = 21;
    public const int VECTOR_EXPR = 92;
    public const int NOT_IN = 83;
    public const int SELECT_EXPR = 145;
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
    public const int BOGUS = 150;
    public const int ORDER = 41;
    public const int MAX = 35;
    public const int UPDATE = 53;
    public const int JOIN_FRAGMENT = 137;
    public const int SUM = 49;
    public const int AND = 6;
    public const int SQL_NE = 108;
    public const int ASCENDING = 8;
    public const int EXPR_LIST = 75;
    public const int AS = 7;
    public const int THEN = 60;
    public const int IN = 26;
    public const int OBJECT = 68;
    public const int COMMA = 101;
    public const int IS = 31;
    public const int SQL_TOKEN = 143;
    public const int LEFT = 33;
    public const int AVG = 9;
    public const int SOME = 48;
    public const int BOR = 115;
    public const int ALL = 4;
    public const int IMPLIED_FROM = 136;
    public const int IDENT = 125;
    public const int PLUS = 118;
    public const int BXOR = 116;
    public const int CASE2 = 74;
    public const int EXISTS = 19;
    public const int DOT = 15;
    public const int LIKE = 34;
    public const int WITH = 63;
    public const int OUTER = 42;
    public const int ID_START_LETTER = 126;
    public const int LEFT_OUTER = 139;
    public const int ROW_STAR = 88;
    public const int NOT_LIKE = 84;
    public const int HEX_DIGIT = 132;
    public const int NOT_BETWEEN = 82;
    public const int RANGE = 87;
    public const int RIGHT_OUTER = 140;
    public const int RIGHT = 44;
    public const int SET = 46;
    public const int HAVING = 25;
    public const int MIN = 36;
    public const int MINUS = 119;
    public const int IS_NOT_NULL = 79;
    public const int BAND = 117;
    public const int ELEMENTS = 17;
    public const int TRUE = 51;
    public const int JOIN = 32;
    public const int UNION = 52;
    public const int IN_LIST = 77;
    public const int COLON = 105;
    public const int OPEN = 103;
    public const int ANY = 5;
    public const int CLOSE = 104;
    public const int WHEN = 61;
    public const int ALIAS_REF = 141;
    public const int DIV = 121;
    public const int DESCENDING = 14;
    public const int BETWEEN = 10;
    public const int AGGREGATE = 71;
    public const int LE = 111;
    private const string DFA59_eotS = "\u001F\uFFFF";
    private const string DFA59_eofS = "\u001F\uFFFF";
    private const string DFA59_minS = "\u0001\u0004\u0006\0\u0018\uFFFF";
    private const string DFA59_maxS = "\u0001\u0095\u0006\0\u0018\uFFFF";
    private const string DFA59_acceptS = "\a\uFFFF\u0001\u0003\u0015\uFFFF\u0001\u0001\u0001\u0002";
    private const string DFA59_specialS = "\u0001\uFFFF\u0001\0\u0001\u0001\u0001\u0002\u0001\u0003\u0001\u0004\u0001\u0005\u0018\uFFFF}>";
    private const string DFA60_eotS = "\u001E\uFFFF";
    private const string DFA60_eofS = "\u001E\uFFFF";
    private const string DFA60_minS = "\u0001\u0004\v\0\u0012\uFFFF";
    private const string DFA60_maxS = "\u0001\u0095\v\0\u0012\uFFFF";
    private const string DFA60_acceptS = "\f\uFFFF\u0001\u0002\u0010\uFFFF\u0001\u0001";
    private const string DFA60_specialS = "\u0001\uFFFF\u0001\0\u0001\u0001\u0001\u0002\u0001\u0003\u0001\u0004\u0001\u0005\u0001\u0006\u0001\a\u0001\b\u0001\t\u0001\n\u0012\uFFFF}>";
    public static readonly string[] tokenNames = new string[151]
    {
      "<invalid>",
      "<EOR>",
      "<DOWN>",
      "<UP>",
      nameof (ALL),
      nameof (ANY),
      nameof (AND),
      nameof (AS),
      nameof (ASCENDING),
      nameof (AVG),
      nameof (BETWEEN),
      nameof (CLASS),
      nameof (COUNT),
      nameof (DELETE),
      nameof (DESCENDING),
      nameof (DOT),
      nameof (DISTINCT),
      nameof (ELEMENTS),
      nameof (ESCAPE),
      nameof (EXISTS),
      nameof (FALSE),
      nameof (FETCH),
      nameof (FROM),
      nameof (FULL),
      nameof (GROUP),
      nameof (HAVING),
      nameof (IN),
      nameof (INDICES),
      nameof (INNER),
      nameof (INSERT),
      nameof (INTO),
      nameof (IS),
      nameof (JOIN),
      nameof (LEFT),
      nameof (LIKE),
      nameof (MAX),
      nameof (MIN),
      nameof (NEW),
      nameof (NOT),
      nameof (NULL),
      nameof (OR),
      nameof (ORDER),
      nameof (OUTER),
      nameof (PROPERTIES),
      nameof (RIGHT),
      nameof (SELECT),
      nameof (SET),
      nameof (SKIP),
      nameof (SOME),
      nameof (SUM),
      nameof (TAKE),
      nameof (TRUE),
      nameof (UNION),
      nameof (UPDATE),
      nameof (VERSIONED),
      nameof (WHERE),
      nameof (LITERAL_by),
      nameof (CASE),
      nameof (END),
      nameof (ELSE),
      nameof (THEN),
      nameof (WHEN),
      nameof (ON),
      nameof (WITH),
      nameof (BOTH),
      nameof (EMPTY),
      nameof (LEADING),
      nameof (MEMBER),
      nameof (OBJECT),
      nameof (OF),
      nameof (TRAILING),
      nameof (AGGREGATE),
      nameof (ALIAS),
      nameof (CONSTRUCTOR),
      nameof (CASE2),
      nameof (EXPR_LIST),
      nameof (FILTER_ENTITY),
      nameof (IN_LIST),
      nameof (INDEX_OP),
      nameof (IS_NOT_NULL),
      nameof (IS_NULL),
      nameof (METHOD_CALL),
      nameof (NOT_BETWEEN),
      nameof (NOT_IN),
      nameof (NOT_LIKE),
      nameof (ORDER_ELEMENT),
      nameof (QUERY),
      nameof (RANGE),
      nameof (ROW_STAR),
      nameof (SELECT_FROM),
      nameof (UNARY_MINUS),
      nameof (UNARY_PLUS),
      nameof (VECTOR_EXPR),
      nameof (WEIRD_IDENT),
      nameof (CONSTANT),
      nameof (NUM_INT),
      nameof (NUM_DOUBLE),
      nameof (NUM_DECIMAL),
      nameof (NUM_FLOAT),
      nameof (NUM_LONG),
      nameof (JAVA_CONSTANT),
      nameof (COMMA),
      nameof (EQ),
      nameof (OPEN),
      nameof (CLOSE),
      nameof (COLON),
      nameof (PARAM),
      nameof (NE),
      nameof (SQL_NE),
      nameof (LT),
      nameof (GT),
      nameof (LE),
      nameof (GE),
      nameof (CONCAT),
      nameof (BNOT),
      nameof (BOR),
      nameof (BXOR),
      nameof (BAND),
      nameof (PLUS),
      nameof (MINUS),
      nameof (STAR),
      nameof (DIV),
      nameof (OPEN_BRACKET),
      nameof (CLOSE_BRACKET),
      nameof (QUOTED_String),
      nameof (IDENT),
      nameof (ID_START_LETTER),
      nameof (ID_LETTER),
      nameof (ESCqs),
      nameof (WS),
      nameof (EXPONENT),
      nameof (FLOAT_SUFFIX),
      nameof (HEX_DIGIT),
      "'ascending'",
      "'descending'",
      nameof (FROM_FRAGMENT),
      nameof (IMPLIED_FROM),
      nameof (JOIN_FRAGMENT),
      nameof (SELECT_CLAUSE),
      nameof (LEFT_OUTER),
      nameof (RIGHT_OUTER),
      nameof (ALIAS_REF),
      nameof (PROPERTY_REF),
      nameof (SQL_TOKEN),
      nameof (SELECT_COLUMNS),
      nameof (SELECT_EXPR),
      nameof (THETA_JOINS),
      nameof (FILTERS),
      nameof (METHOD_NAME),
      nameof (NAMED_PARAM),
      nameof (BOGUS)
    };
    protected SqlGenerator.DFA59 dfa59;
    protected SqlGenerator.DFA60 dfa60;
    private static readonly string[] DFA59_transitionS = new string[31]
    {
      "\u0002\a\u0006\uFFFF\u0001\a\u0002\uFFFF\u0001\a\u0004\uFFFF\u0001\a\u0012\uFFFF\u0001\a\u0005\uFFFF\u0001\a\u0002\uFFFF\u0001\a\u0002\uFFFF\u0002\a\u0004\uFFFF\u0001\a\r\uFFFF\u0001\a\u0002\uFFFF\u0001\a\u0003\uFFFF\u0001\a\u0002\uFFFF\u0001\a\b\uFFFF\u0001\a\u0001\uFFFF\u0001\a\u0001\uFFFF\a\a\u0005\uFFFF\u0001\a\a\uFFFF\u0001\u0006\u0001\u0004\u0001\u0005\u0001\u0003\u0001\u0001\u0001\u0002\u0002\a\u0002\uFFFF\u0002\a\u000F\uFFFF\u0001\a\u0001\uFFFF\u0001\a\u0005\uFFFF\u0001\a",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
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
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      ""
    };
    private static readonly short[] DFA59_eot = DFA.UnpackEncodedString("\u001F\uFFFF");
    private static readonly short[] DFA59_eof = DFA.UnpackEncodedString("\u001F\uFFFF");
    private static readonly char[] DFA59_min = DFA.UnpackEncodedStringToUnsignedChars("\u0001\u0004\u0006\0\u0018\uFFFF");
    private static readonly char[] DFA59_max = DFA.UnpackEncodedStringToUnsignedChars("\u0001\u0095\u0006\0\u0018\uFFFF");
    private static readonly short[] DFA59_accept = DFA.UnpackEncodedString("\a\uFFFF\u0001\u0003\u0015\uFFFF\u0001\u0001\u0001\u0002");
    private static readonly short[] DFA59_special = DFA.UnpackEncodedString("\u0001\uFFFF\u0001\0\u0001\u0001\u0001\u0002\u0001\u0003\u0001\u0004\u0001\u0005\u0018\uFFFF}>");
    private static readonly short[][] DFA59_transition = DFA.UnpackEncodedStringArray(SqlGenerator.DFA59_transitionS);
    private static readonly string[] DFA60_transitionS = new string[30]
    {
      "\u0002\f\u0006\uFFFF\u0001\f\u0002\uFFFF\u0001\f\u0004\uFFFF\u0001\f\u0012\uFFFF\u0001\f\u0005\uFFFF\u0001\f\u0002\uFFFF\u0001\f\u0002\uFFFF\u0002\f\u0004\uFFFF\u0001\n\r\uFFFF\u0001\f\u0002\uFFFF\u0001\v\u0003\uFFFF\u0001\f\u0002\uFFFF\u0001\f\b\uFFFF\u0001\t\u0001\uFFFF\u0001\f\u0001\uFFFF\a\f\u0005\uFFFF\u0001\f\a\uFFFF\u0001\u0006\u0001\u0004\u0001\u0005\u0001\u0003\u0001\u0001\u0001\u0002\u0001\a\u0001\b\u0002\uFFFF\u0002\f\u000F\uFFFF\u0001\f\u0001\uFFFF\u0001\f\u0005\uFFFF\u0001\f",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
      "\u0001\uFFFF",
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
      "",
      ""
    };
    private static readonly short[] DFA60_eot = DFA.UnpackEncodedString("\u001E\uFFFF");
    private static readonly short[] DFA60_eof = DFA.UnpackEncodedString("\u001E\uFFFF");
    private static readonly char[] DFA60_min = DFA.UnpackEncodedStringToUnsignedChars("\u0001\u0004\v\0\u0012\uFFFF");
    private static readonly char[] DFA60_max = DFA.UnpackEncodedStringToUnsignedChars("\u0001\u0095\v\0\u0012\uFFFF");
    private static readonly short[] DFA60_accept = DFA.UnpackEncodedString("\f\uFFFF\u0001\u0002\u0010\uFFFF\u0001\u0001");
    private static readonly short[] DFA60_special = DFA.UnpackEncodedString("\u0001\uFFFF\u0001\0\u0001\u0001\u0001\u0002\u0001\u0003\u0001\u0004\u0001\u0005\u0001\u0006\u0001\a\u0001\b\u0001\t\u0001\n\u0012\uFFFF}>");
    private static readonly short[][] DFA60_transition = DFA.UnpackEncodedStringArray(SqlGenerator.DFA60_transitionS);
    public static readonly BitSet FOLLOW_selectStatement_in_statement57 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_updateStatement_in_statement62 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_deleteStatement_in_statement67 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_insertStatement_in_statement72 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SELECT_in_selectStatement84 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_selectClause_in_selectStatement90 = new BitSet(new ulong[1]
    {
      4194304UL
    });
    public static readonly BitSet FOLLOW_from_in_selectStatement94 = new BitSet(new ulong[1]
    {
      37297633487749128UL
    });
    public static readonly BitSet FOLLOW_WHERE_in_selectStatement101 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_whereExpr_in_selectStatement105 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_GROUP_in_selectStatement117 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_groupExprs_in_selectStatement121 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_HAVING_in_selectStatement133 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_selectStatement137 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ORDER_in_selectStatement149 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_orderExprs_in_selectStatement153 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_SKIP_in_selectStatement165 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_limitValue_in_selectStatement169 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_TAKE_in_selectStatement181 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_limitValue_in_selectStatement185 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_UPDATE_in_updateStatement212 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_FROM_in_updateStatement220 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_fromTable_in_updateStatement222 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_setClause_in_updateStatement228 = new BitSet(new ulong[1]
    {
      36028797018963976UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_updateStatement233 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_DELETE_in_deleteStatement252 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_from_in_deleteStatement258 = new BitSet(new ulong[1]
    {
      36028797018963976UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_deleteStatement263 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_INSERT_in_insertStatement280 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_INTO_in_insertStatement289 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_selectStatement_in_insertStatement299 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_SET_in_setClause319 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_comparisonExpr_in_setClause323 = new BitSet(new ulong[2]
    {
      17247503368UL,
      536836554194944UL
    });
    public static readonly BitSet FOLLOW_comparisonExpr_in_setClause330 = new BitSet(new ulong[2]
    {
      17247503368UL,
      536836554194944UL
    });
    public static readonly BitSet FOLLOW_WHERE_in_whereClause348 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_whereClauseExpr_in_whereClause352 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_conditionList_in_whereClauseExpr371 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_whereClauseExpr376 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_expr_in_orderExprs392 = new BitSet(new ulong[3]
    {
      151187796622627122UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_orderDirection_in_orderExprs399 = new BitSet(new ulong[3]
    {
      151187796622610482UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_orderExprs_in_orderExprs409 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_expr_in_groupExprs424 = new BitSet(new ulong[3]
    {
      151187796622610482UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_groupExprs_in_groupExprs430 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_orderDirection0 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_filters_in_whereExpr465 = new BitSet(new ulong[3]
    {
      1391637038146UL,
      536836554326016UL,
      294912UL
    });
    public static readonly BitSet FOLLOW_thetaJoins_in_whereExpr473 = new BitSet(new ulong[3]
    {
      1391637038146UL,
      536836554326016UL,
      32768UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_whereExpr484 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_thetaJoins_in_whereExpr494 = new BitSet(new ulong[3]
    {
      1391637038146UL,
      536836554326016UL,
      32768UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_whereExpr502 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_whereExpr513 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FILTERS_in_filters526 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_conditionList_in_filters528 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_THETA_JOINS_in_thetaJoins542 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_conditionList_in_thetaJoins544 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_sqlToken_in_conditionList557 = new BitSet(new ulong[3]
    {
      2UL,
      0UL,
      32768UL
    });
    public static readonly BitSet FOLLOW_conditionList_in_conditionList563 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SELECT_CLAUSE_in_selectClause578 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_distinctOrAll_in_selectClause581 = new BitSet(new ulong[3]
    {
      146402722018529280UL,
      3745873524544390784UL,
      2269184UL
    });
    public static readonly BitSet FOLLOW_selectColumn_in_selectClause587 = new BitSet(new ulong[3]
    {
      146402722018529288UL,
      3745873524544390784UL,
      2269184UL
    });
    public static readonly BitSet FOLLOW_selectExpr_in_selectColumn605 = new BitSet(new ulong[3]
    {
      2UL,
      0UL,
      65536UL
    });
    public static readonly BitSet FOLLOW_SELECT_COLUMNS_in_selectColumn610 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_selectAtom_in_selectExpr630 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_count_in_selectExpr637 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_CONSTRUCTOR_in_selectExpr643 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_set_in_selectExpr645 = new BitSet(new ulong[3]
    {
      146402722018529280UL,
      3745873524544390784UL,
      2269184UL
    });
    public static readonly BitSet FOLLOW_selectColumn_in_selectExpr655 = new BitSet(new ulong[3]
    {
      146402722018529288UL,
      3745873524544390784UL,
      2269184UL
    });
    public static readonly BitSet FOLLOW_methodCall_in_selectExpr665 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_aggregate_in_selectExpr670 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_constant_in_selectExpr677 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_arithmeticExpr_in_selectExpr684 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_parameter_in_selectExpr689 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_selectStatement_in_selectExpr698 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_COUNT_in_count712 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_distinctOrAll_in_count719 = new BitSet(new ulong[3]
    {
      146367537646440448UL,
      3745873524561167488UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_countExpr_in_count725 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_DISTINCT_in_distinctOrAll740 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ALL_in_distinctOrAll748 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_ROW_STAR_in_countExpr767 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_simpleExpr_in_countExpr774 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_DOT_in_selectAtom786 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_SQL_TOKEN_in_selectAtom796 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_ALIAS_REF_in_selectAtom806 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_SELECT_EXPR_in_selectAtom816 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_FROM_in_from839 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_fromTable_in_from846 = new BitSet(new ulong[3]
    {
      8UL,
      0UL,
      640UL
    });
    public static readonly BitSet FOLLOW_FROM_FRAGMENT_in_fromTable872 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_tableJoin_in_fromTable878 = new BitSet(new ulong[3]
    {
      8UL,
      0UL,
      640UL
    });
    public static readonly BitSet FOLLOW_JOIN_FRAGMENT_in_fromTable893 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_tableJoin_in_fromTable899 = new BitSet(new ulong[3]
    {
      8UL,
      0UL,
      640UL
    });
    public static readonly BitSet FOLLOW_JOIN_FRAGMENT_in_tableJoin922 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_tableJoin_in_tableJoin927 = new BitSet(new ulong[3]
    {
      8UL,
      0UL,
      640UL
    });
    public static readonly BitSet FOLLOW_FROM_FRAGMENT_in_tableJoin943 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_tableJoin_in_tableJoin948 = new BitSet(new ulong[3]
    {
      8UL,
      0UL,
      640UL
    });
    public static readonly BitSet FOLLOW_AND_in_booleanOp968 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_booleanOp970 = new BitSet(new ulong[3]
    {
      1391637038144UL,
      536836554326016UL,
      32768UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_booleanOp975 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_OR_in_booleanOp983 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_booleanOp987 = new BitSet(new ulong[3]
    {
      1391637038144UL,
      536836554326016UL,
      32768UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_booleanOp992 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_in_booleanOp1002 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_booleanOp1006 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_booleanOp_in_booleanExpr1023 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_comparisonExpr_in_booleanExpr1030 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_methodCall_in_booleanExpr1037 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SQL_TOKEN_in_booleanExpr1044 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_binaryComparisonExpression_in_comparisonExpr1060 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_exoticComparisonExpression_in_comparisonExpr1067 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_EQ_in_binaryComparisonExpression1082 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1084 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1088 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NE_in_binaryComparisonExpression1095 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1097 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1101 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_GT_in_binaryComparisonExpression1108 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1110 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1114 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_GE_in_binaryComparisonExpression1121 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1123 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1127 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_LT_in_binaryComparisonExpression1134 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1136 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1140 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_LE_in_binaryComparisonExpression1147 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1149 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_binaryComparisonExpression1153 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_LIKE_in_exoticComparisonExpression1167 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1169 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1173 = new BitSet(new ulong[1]
    {
      262152UL
    });
    public static readonly BitSet FOLLOW_likeEscape_in_exoticComparisonExpression1175 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_LIKE_in_exoticComparisonExpression1183 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1185 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1189 = new BitSet(new ulong[1]
    {
      262152UL
    });
    public static readonly BitSet FOLLOW_likeEscape_in_exoticComparisonExpression1191 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BETWEEN_in_exoticComparisonExpression1198 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1200 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1204 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1208 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_BETWEEN_in_exoticComparisonExpression1215 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1217 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1221 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1225 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IN_in_exoticComparisonExpression1232 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1234 = new BitSet(new ulong[2]
    {
      0UL,
      8192UL
    });
    public static readonly BitSet FOLLOW_inList_in_exoticComparisonExpression1238 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_IN_in_exoticComparisonExpression1246 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1248 = new BitSet(new ulong[2]
    {
      0UL,
      8192UL
    });
    public static readonly BitSet FOLLOW_inList_in_exoticComparisonExpression1252 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_EXISTS_in_exoticComparisonExpression1260 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_quantified_in_exoticComparisonExpression1264 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IS_NULL_in_exoticComparisonExpression1272 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1274 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IS_NOT_NULL_in_exoticComparisonExpression1283 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_exoticComparisonExpression1285 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ESCAPE_in_likeEscape1302 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_likeEscape1306 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IN_LIST_in_inList1322 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_parenSelect_in_inList1328 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_simpleExprList_in_inList1332 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_simpleExpr_in_simpleExprList1353 = new BitSet(new ulong[3]
    {
      146367537646440450UL,
      3745873524544390272UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_simpleExpr_in_expr1372 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_VECTOR_EXPR_in_expr1379 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_expr1386 = new BitSet(new ulong[3]
    {
      151187796622610488UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_parenSelect_in_expr1401 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ANY_in_expr1407 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_quantified_in_expr1411 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ALL_in_expr1419 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_quantified_in_expr1423 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_SOME_in_expr1431 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_quantified_in_expr1435 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_sqlToken_in_quantified1453 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_selectStatement_in_quantified1457 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_selectStatement_in_parenSelect1476 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_UNION_in_parenSelect1485 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_selectStatement_in_parenSelect1489 = new BitSet(new ulong[1]
    {
      4538783999459328UL
    });
    public static readonly BitSet FOLLOW_parenSelect_in_parenSelect1493 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_constant_in_simpleExpr1510 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_NULL_in_simpleExpr1517 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_addrExpr_in_simpleExpr1524 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_sqlToken_in_simpleExpr1529 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_aggregate_in_simpleExpr1534 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_methodCall_in_simpleExpr1539 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_count_in_simpleExpr1544 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_parameter_in_simpleExpr1549 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_arithmeticExpr_in_simpleExpr1554 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_constant0 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_additiveExpr_in_arithmeticExpr1628 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_bitwiseExpr_in_arithmeticExpr1633 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_arithmeticExpr1638 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_UNARY_MINUS_in_arithmeticExpr1645 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_arithmeticExpr1649 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_caseExpr_in_arithmeticExpr1655 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_PLUS_in_additiveExpr1667 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_additiveExpr1669 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_additiveExpr1673 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_MINUS_in_additiveExpr1680 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_additiveExpr1682 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_nestedExprAfterMinusDiv_in_additiveExpr1686 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BAND_in_bitwiseExpr1699 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_bitwiseExpr1701 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_nestedExpr_in_bitwiseExpr1705 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BOR_in_bitwiseExpr1712 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_bitwiseExpr1714 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_nestedExpr_in_bitwiseExpr1718 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BXOR_in_bitwiseExpr1725 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_bitwiseExpr1727 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_nestedExpr_in_bitwiseExpr1731 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BNOT_in_bitwiseExpr1738 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_nestedExpr_in_bitwiseExpr1742 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_STAR_in_multiplicativeExpr1756 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_nestedExpr_in_multiplicativeExpr1758 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_nestedExpr_in_multiplicativeExpr1762 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_DIV_in_multiplicativeExpr1769 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_nestedExpr_in_multiplicativeExpr1771 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_nestedExprAfterMinusDiv_in_multiplicativeExpr1775 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_additiveExpr_in_nestedExpr1797 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_bitwiseExpr_in_nestedExpr1812 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_expr_in_nestedExpr1819 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_arithmeticExpr_in_nestedExprAfterMinusDiv1841 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_expr_in_nestedExprAfterMinusDiv1848 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_CASE_in_caseExpr1860 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_WHEN_in_caseExpr1870 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_booleanExpr_in_caseExpr1874 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr1879 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ELSE_in_caseExpr1891 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr1895 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_CASE2_in_caseExpr1911 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr1915 = new BitSet(new ulong[1]
    {
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_WHEN_in_caseExpr1922 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr1926 = new BitSet(new ulong[3]
    {
      151187796622610480UL,
      3745873524812825728UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr1930 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ELSE_in_caseExpr1942 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr1946 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_AGGREGATE_in_aggregate1970 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_aggregate1975 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_METHOD_CALL_in_methodCall1994 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_METHOD_NAME_in_methodCall1998 = new BitSet(new ulong[2]
    {
      8UL,
      2048UL
    });
    public static readonly BitSet FOLLOW_EXPR_LIST_in_methodCall2007 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_arguments_in_methodCall2010 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_expr_in_arguments2035 = new BitSet(new ulong[3]
    {
      151187813870113842UL,
      3746410361367020672UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_comparisonExpr_in_arguments2039 = new BitSet(new ulong[3]
    {
      151187813870113842UL,
      3746410361367020672UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_expr_in_arguments2048 = new BitSet(new ulong[3]
    {
      151187813870113842UL,
      3746410361367020672UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_comparisonExpr_in_arguments2052 = new BitSet(new ulong[3]
    {
      151187813870113842UL,
      3746410361367020672UL,
      2138112UL
    });
    public static readonly BitSet FOLLOW_NAMED_PARAM_in_parameter2070 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_PARAM_in_parameter2079 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_limitValue0 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_DOT_in_addrExpr2116 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_ALIAS_REF_in_addrExpr2130 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_INDEX_OP_in_addrExpr2140 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_SQL_TOKEN_in_sqlToken2160 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_SQL_TOKEN_in_synpred1_SqlGenerator366 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_additiveExpr_in_synpred2_SqlGenerator1790 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_bitwiseExpr_in_synpred3_SqlGenerator1805 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_arithmeticExpr_in_synpred4_SqlGenerator1834 = new BitSet(new ulong[1]
    {
      2UL
    });
    private readonly List<IParameterSpecification> collectedParameters = new List<IParameterSpecification>();
    private readonly List<SqlGenerator.ISqlWriter> outputStack = new List<SqlGenerator.ISqlWriter>();
    private readonly IParseErrorHandler parseErrorHandler;
    private readonly ISessionFactoryImplementor sessionFactory;
    private readonly SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
    private SqlGenerator.ISqlWriter writer;

    public SqlGenerator(ITreeNodeStream input)
      : this(input, new RecognizerSharedState())
    {
    }

    public SqlGenerator(ITreeNodeStream input, RecognizerSharedState state)
      : base(input, state)
    {
      this.InitializeCyclicDFAs();
    }

    public override string[] TokenNames => SqlGenerator.tokenNames;

    public override string GrammarFileName => "SqlGenerator.g";

    public void statement()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 13:
            num = 3;
            break;
          case 29:
            num = 4;
            break;
          case 45:
            num = 1;
            break;
          case 53:
            num = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 1, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_selectStatement_in_statement57);
            this.selectStatement();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_updateStatement_in_statement62);
            this.updateStatement();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.PushFollow(SqlGenerator.FOLLOW_deleteStatement_in_statement67);
            this.deleteStatement();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 4:
            this.PushFollow(SqlGenerator.FOLLOW_insertStatement_in_statement72);
            this.insertStatement();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void selectStatement()
    {
      try
      {
        this.Match((IIntStream) this.input, 45, SqlGenerator.FOLLOW_SELECT_in_selectStatement84);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
        {
          this.StartQuery();
          this.Out("select ");
        }
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_selectClause_in_selectStatement90);
        this.selectClause();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_from_in_selectStatement94);
        this.from();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        int num1 = 2;
        if (this.input.LA(1) == 55)
          num1 = 1;
        if (num1 == 1)
        {
          this.Match((IIntStream) this.input, 55, SqlGenerator.FOLLOW_WHERE_in_selectStatement101);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            this.Out(" where ");
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          if (this.state.failed)
            return;
          this.PushFollow(SqlGenerator.FOLLOW_whereExpr_in_selectStatement105);
          this.whereExpr();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (this.state.failed)
            return;
        }
        int num2 = 2;
        if (this.input.LA(1) == 24)
          num2 = 1;
        if (num2 == 1)
        {
          this.Match((IIntStream) this.input, 24, SqlGenerator.FOLLOW_GROUP_in_selectStatement117);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            this.Out(" group by ");
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          if (this.state.failed)
            return;
          this.PushFollow(SqlGenerator.FOLLOW_groupExprs_in_selectStatement121);
          this.groupExprs();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (this.state.failed)
            return;
        }
        int num3 = 2;
        if (this.input.LA(1) == 25)
          num3 = 1;
        if (num3 == 1)
        {
          this.Match((IIntStream) this.input, 25, SqlGenerator.FOLLOW_HAVING_in_selectStatement133);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            this.Out(" having ");
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          if (this.state.failed)
            return;
          this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_selectStatement137);
          this.booleanExpr(false);
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (this.state.failed)
            return;
        }
        int num4 = 2;
        if (this.input.LA(1) == 41)
          num4 = 1;
        if (num4 == 1)
        {
          this.Match((IIntStream) this.input, 41, SqlGenerator.FOLLOW_ORDER_in_selectStatement149);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            this.Out(" order by ");
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          if (this.state.failed)
            return;
          this.PushFollow(SqlGenerator.FOLLOW_orderExprs_in_selectStatement153);
          this.orderExprs();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (this.state.failed)
            return;
        }
        int num5 = 2;
        if (this.input.LA(1) == 47)
          num5 = 1;
        if (num5 == 1)
        {
          this.Match((IIntStream) this.input, 47, SqlGenerator.FOLLOW_SKIP_in_selectStatement165);
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          if (this.state.failed)
            return;
          this.PushFollow(SqlGenerator.FOLLOW_limitValue_in_selectStatement169);
          SqlGenerator.limitValue_return limitValueReturn = this.limitValue();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            this.Skip(limitValueReturn != null ? (IASTNode) limitValueReturn.Start : (IASTNode) null);
        }
        int num6 = 2;
        if (this.input.LA(1) == 50)
          num6 = 1;
        if (num6 == 1)
        {
          this.Match((IIntStream) this.input, 50, SqlGenerator.FOLLOW_TAKE_in_selectStatement181);
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          if (this.state.failed)
            return;
          this.PushFollow(SqlGenerator.FOLLOW_limitValue_in_selectStatement185);
          SqlGenerator.limitValue_return limitValueReturn = this.limitValue();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            this.Take(limitValueReturn != null ? (IASTNode) limitValueReturn.Start : (IASTNode) null);
        }
        if (this.state.backtracking == 0)
          this.EndQuery();
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void updateStatement()
    {
      try
      {
        this.Match((IIntStream) this.input, 53, SqlGenerator.FOLLOW_UPDATE_in_updateStatement212);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out("update ");
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 22, SqlGenerator.FOLLOW_FROM_in_updateStatement220);
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_fromTable_in_updateStatement222);
        this.fromTable();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_setClause_in_updateStatement228);
        this.setClause();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        int num = 2;
        if (this.input.LA(1) == 55)
          num = 1;
        if (num == 1)
        {
          this.PushFollow(SqlGenerator.FOLLOW_whereClause_in_updateStatement233);
          this.whereClause();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void deleteStatement()
    {
      try
      {
        this.Match((IIntStream) this.input, 13, SqlGenerator.FOLLOW_DELETE_in_deleteStatement252);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out("delete");
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_from_in_deleteStatement258);
        this.from();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        int num = 2;
        if (this.input.LA(1) == 55)
          num = 1;
        if (num == 1)
        {
          this.PushFollow(SqlGenerator.FOLLOW_whereClause_in_deleteStatement263);
          this.whereClause();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void insertStatement()
    {
      try
      {
        this.Match((IIntStream) this.input, 29, SqlGenerator.FOLLOW_INSERT_in_insertStatement280);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out("insert ");
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        IASTNode n = (IASTNode) this.Match((IIntStream) this.input, 30, SqlGenerator.FOLLOW_INTO_in_insertStatement289);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
        {
          this.Out(n);
          this.Out(" ");
        }
        if (this.input.LA(1) == 2)
        {
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          if (this.state.failed)
            return;
          do
          {
            int num1 = 2;
            int num2 = this.input.LA(1);
            if (num2 >= 4 && num2 <= 150)
              num1 = 1;
            else if (num2 == 3)
              num1 = 2;
            if (num1 == 1)
              this.MatchAny((IIntStream) this.input);
            else
              goto label_21;
          }
          while (!this.state.failed);
          return;
label_21:
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (this.state.failed)
            return;
        }
        this.PushFollow(SqlGenerator.FOLLOW_selectStatement_in_insertStatement299);
        this.selectStatement();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void setClause()
    {
      try
      {
        this.Match((IIntStream) this.input, 46, SqlGenerator.FOLLOW_SET_in_setClause319);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(" set ");
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_comparisonExpr_in_setClause323);
        this.comparisonExpr(false);
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        do
        {
          int num1 = 2;
          int num2 = this.input.LA(1);
          if (num2 == 10 || num2 == 19 || num2 == 26 || num2 == 34 || num2 >= 79 && num2 <= 80 || num2 >= 82 && num2 <= 84 || num2 == 102 || num2 == 107 || num2 >= 109 && num2 <= 112)
            num1 = 1;
          if (num1 == 1)
          {
            if (this.state.backtracking == 0)
              this.Out(", ");
            this.PushFollow(SqlGenerator.FOLLOW_comparisonExpr_in_setClause330);
            this.comparisonExpr(false);
            --this.state.followingStackPointer;
          }
          else
            goto label_15;
        }
        while (!this.state.failed);
        return;
label_15:
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void whereClause()
    {
      try
      {
        this.Match((IIntStream) this.input, 55, SqlGenerator.FOLLOW_WHERE_in_whereClause348);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(" where ");
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_whereClauseExpr_in_whereClause352);
        this.whereClauseExpr();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void whereClauseExpr()
    {
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 143)
        {
          int num3 = this.input.LA(2);
          if (num3 == 2 && this.synpred1_SqlGenerator())
            num2 = 1;
          else if (num3 == 3)
          {
            num2 = 2;
          }
          else
          {
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 12, 1, (IIntStream) this.input);
            this.state.failed = true;
            return;
          }
        }
        else if (num1 == 6 || num1 == 10 || num1 == 19 || num1 == 26 || num1 == 34 || num1 == 38 || num1 == 40 || num1 >= 79 && num1 <= 84 || num1 == 102 || num1 == 107 || num1 >= 109 && num1 <= 112)
        {
          num2 = 2;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 12, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
        }
        switch (num2)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_conditionList_in_whereClauseExpr371);
            this.conditionList();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_whereClauseExpr376);
            this.booleanExpr(false);
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void orderExprs()
    {
      try
      {
        this.PushFollow(SqlGenerator.FOLLOW_expr_in_orderExprs392);
        this.expr();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        int num1 = 2;
        switch (this.input.LA(1))
        {
          case 8:
          case 14:
            num1 = 1;
            break;
        }
        if (num1 == 1)
        {
          this.PushFollow(SqlGenerator.FOLLOW_orderDirection_in_orderExprs399);
          SqlGenerator.orderDirection_return orderDirectionReturn = this.orderDirection();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
          {
            this.Out(" ");
            this.Out(orderDirectionReturn != null ? (IASTNode) orderDirectionReturn.Start : (IASTNode) null);
          }
        }
        int num2 = 2;
        int num3 = this.input.LA(1);
        if (num3 >= 4 && num3 <= 5 || num3 == 12 || num3 == 15 || num3 == 20 || num3 == 39 || num3 == 45 || num3 == 48 || num3 >= 51 && num3 <= 52 || num3 == 57 || num3 == 71 || num3 == 74 || num3 == 78 || num3 == 81 || num3 == 90 || num3 == 92 || num3 >= 94 && num3 <= 100 || num3 == 106 || num3 >= 114 && num3 <= 121 || num3 >= 124 && num3 <= 125 || num3 == 141 || num3 == 143 || num3 == 149)
          num2 = 1;
        if (num2 != 1)
          return;
        if (this.state.backtracking == 0)
          this.Out(", ");
        this.PushFollow(SqlGenerator.FOLLOW_orderExprs_in_orderExprs409);
        this.orderExprs();
        --this.state.followingStackPointer;
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void groupExprs()
    {
      try
      {
        this.PushFollow(SqlGenerator.FOLLOW_expr_in_groupExprs424);
        this.expr();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        int num1 = 2;
        int num2 = this.input.LA(1);
        if (num2 >= 4 && num2 <= 5 || num2 == 12 || num2 == 15 || num2 == 20 || num2 == 39 || num2 == 45 || num2 == 48 || num2 >= 51 && num2 <= 52 || num2 == 57 || num2 == 71 || num2 == 74 || num2 == 78 || num2 == 81 || num2 == 90 || num2 == 92 || num2 >= 94 && num2 <= 100 || num2 == 106 || num2 >= 114 && num2 <= 121 || num2 >= 124 && num2 <= 125 || num2 == 141 || num2 == 143 || num2 == 149)
          num1 = 1;
        if (num1 != 1)
          return;
        if (this.state.backtracking == 0)
          this.Out(" , ");
        this.PushFollow(SqlGenerator.FOLLOW_groupExprs_in_groupExprs430);
        this.groupExprs();
        --this.state.followingStackPointer;
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public SqlGenerator.orderDirection_return orderDirection()
    {
      SqlGenerator.orderDirection_return orderDirectionReturn = new SqlGenerator.orderDirection_return();
      orderDirectionReturn.Start = this.input.LT(1);
      try
      {
        if (this.input.LA(1) == 8 || this.input.LA(1) == 14)
        {
          this.input.Consume();
          this.state.errorRecovery = false;
          this.state.failed = false;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
          this.state.failed = true;
          return orderDirectionReturn;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return orderDirectionReturn;
    }

    public void whereExpr()
    {
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 6:
          case 10:
          case 19:
          case 26:
          case 34:
          case 38:
          case 40:
          case 79:
          case 80:
          case 81:
          case 82:
          case 83:
          case 84:
          case 102:
          case 107:
          case 109:
          case 110:
          case 111:
          case 112:
          case 143:
            num1 = 3;
            break;
          case 146:
            num1 = 2;
            break;
          case 147:
            num1 = 1;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 19, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num1)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_filters_in_whereExpr465);
            this.filters();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            int num2 = 2;
            if (this.input.LA(1) == 146)
              num2 = 1;
            if (num2 == 1)
            {
              if (this.state.backtracking == 0)
                this.Out(" and ");
              this.PushFollow(SqlGenerator.FOLLOW_thetaJoins_in_whereExpr473);
              this.thetaJoins();
              --this.state.followingStackPointer;
              if (this.state.failed)
                break;
            }
            int num3 = 2;
            int num4 = this.input.LA(1);
            if (num4 == 6 || num4 == 10 || num4 == 19 || num4 == 26 || num4 == 34 || num4 == 38 || num4 == 40 || num4 >= 79 && num4 <= 84 || num4 == 102 || num4 == 107 || num4 >= 109 && num4 <= 112 || num4 == 143)
              num3 = 1;
            if (num3 != 1)
              break;
            if (this.state.backtracking == 0)
              this.Out(" and ");
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_whereExpr484);
            this.booleanExpr(true);
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_thetaJoins_in_whereExpr494);
            this.thetaJoins();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            int num5 = 2;
            int num6 = this.input.LA(1);
            if (num6 == 6 || num6 == 10 || num6 == 19 || num6 == 26 || num6 == 34 || num6 == 38 || num6 == 40 || num6 >= 79 && num6 <= 84 || num6 == 102 || num6 == 107 || num6 >= 109 && num6 <= 112 || num6 == 143)
              num5 = 1;
            if (num5 != 1)
              break;
            if (this.state.backtracking == 0)
              this.Out(" and ");
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_whereExpr502);
            this.booleanExpr(true);
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_whereExpr513);
            this.booleanExpr(false);
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void filters()
    {
      try
      {
        this.Match((IIntStream) this.input, 147, SqlGenerator.FOLLOW_FILTERS_in_filters526);
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_conditionList_in_filters528);
        this.conditionList();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void thetaJoins()
    {
      try
      {
        this.Match((IIntStream) this.input, 146, SqlGenerator.FOLLOW_THETA_JOINS_in_thetaJoins542);
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_conditionList_in_thetaJoins544);
        this.conditionList();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void conditionList()
    {
      try
      {
        this.PushFollow(SqlGenerator.FOLLOW_sqlToken_in_conditionList557);
        this.sqlToken();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        int num = 2;
        if (this.input.LA(1) == 143)
          num = 1;
        if (num != 1)
          return;
        if (this.state.backtracking == 0)
          this.Out(" and ");
        this.PushFollow(SqlGenerator.FOLLOW_conditionList_in_conditionList563);
        this.conditionList();
        --this.state.followingStackPointer;
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void selectClause()
    {
      try
      {
        this.Match((IIntStream) this.input, 138, SqlGenerator.FOLLOW_SELECT_CLAUSE_in_selectClause578);
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        int num1 = 2;
        switch (this.input.LA(1))
        {
          case 4:
          case 16:
            num1 = 1;
            break;
        }
        if (num1 == 1)
        {
          this.PushFollow(SqlGenerator.FOLLOW_distinctOrAll_in_selectClause581);
          this.distinctOrAll();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
        }
        int num2 = 0;
        while (true)
        {
          int num3 = 2;
          int num4 = this.input.LA(1);
          if (num4 == 12 || num4 == 15 || num4 == 20 || num4 == 45 || num4 == 51 || num4 == 57 || num4 == 71 || num4 >= 73 && num4 <= 74 || num4 == 81 || num4 == 90 || num4 >= 94 && num4 <= 100 || num4 == 106 || num4 >= 114 && num4 <= 121 || num4 >= 124 && num4 <= 125 || num4 == 141 || num4 == 143 || num4 == 145 || num4 == 149)
            num3 = 1;
          if (num3 == 1)
          {
            this.PushFollow(SqlGenerator.FOLLOW_selectColumn_in_selectClause587);
            this.selectColumn();
            --this.state.followingStackPointer;
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
        if (num2 < 1)
        {
          if (this.state.backtracking <= 0)
            throw new EarlyExitException(22, (IIntStream) this.input);
          this.state.failed = true;
        }
        else
        {
          this.Match((IIntStream) this.input, 3, (BitSet) null);
          if (!this.state.failed)
            ;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void selectColumn()
    {
      IASTNode n = (IASTNode) null;
      try
      {
        this.PushFollow(SqlGenerator.FOLLOW_selectExpr_in_selectColumn605);
        SqlGenerator.selectExpr_return selectExprReturn = this.selectExpr();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        int num = 2;
        if (this.input.LA(1) == 144)
          num = 1;
        if (num == 1)
        {
          n = (IASTNode) this.Match((IIntStream) this.input, 144, SqlGenerator.FOLLOW_SELECT_COLUMNS_in_selectColumn610);
          if (this.state.failed)
            return;
          if (this.state.backtracking == 0)
            this.Out(n);
        }
        if (this.state.backtracking != 0)
          return;
        this.Separator(n ?? (selectExprReturn != null ? (IASTNode) selectExprReturn.Start : (IASTNode) null), ", ");
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public SqlGenerator.selectExpr_return selectExpr()
    {
      SqlGenerator.selectExpr_return selectExprReturn = new SqlGenerator.selectExpr_return();
      selectExprReturn.Start = this.input.LT(1);
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 12:
            num1 = 2;
            break;
          case 15:
          case 141:
          case 143:
          case 145:
            num1 = 1;
            break;
          case 20:
          case 51:
          case 94:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 100:
          case 124:
          case 125:
            num1 = 6;
            break;
          case 45:
            num1 = 9;
            break;
          case 57:
          case 74:
          case 90:
          case 114:
          case 115:
          case 116:
          case 117:
          case 118:
          case 119:
          case 120:
          case 121:
            num1 = 7;
            break;
          case 71:
            num1 = 5;
            break;
          case 73:
            num1 = 3;
            break;
          case 81:
            num1 = 4;
            break;
          case 106:
          case 149:
            num1 = 8;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 25, 0, (IIntStream) this.input);
            this.state.failed = true;
            return selectExprReturn;
        }
        switch (num1)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_selectAtom_in_selectExpr630);
            SqlGenerator.selectAtom_return selectAtomReturn = this.selectAtom();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              return selectExprReturn;
            this.Out(selectAtomReturn != null ? (IASTNode) selectAtomReturn.Start : (IASTNode) null);
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_count_in_selectExpr637);
            this.count();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return selectExprReturn;
            break;
          case 3:
            this.Match((IIntStream) this.input, 73, SqlGenerator.FOLLOW_CONSTRUCTOR_in_selectExpr643);
            if (this.state.failed)
              return selectExprReturn;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return selectExprReturn;
            if (this.input.LA(1) == 15 || this.input.LA(1) == 125)
            {
              this.input.Consume();
              this.state.errorRecovery = false;
              this.state.failed = false;
              int num2 = 0;
              while (true)
              {
                int num3 = 2;
                int num4 = this.input.LA(1);
                if (num4 == 12 || num4 == 15 || num4 == 20 || num4 == 45 || num4 == 51 || num4 == 57 || num4 == 71 || num4 >= 73 && num4 <= 74 || num4 == 81 || num4 == 90 || num4 >= 94 && num4 <= 100 || num4 == 106 || num4 >= 114 && num4 <= 121 || num4 >= 124 && num4 <= 125 || num4 == 141 || num4 == 143 || num4 == 145 || num4 == 149)
                  num3 = 1;
                if (num3 == 1)
                {
                  this.PushFollow(SqlGenerator.FOLLOW_selectColumn_in_selectExpr655);
                  this.selectColumn();
                  --this.state.followingStackPointer;
                  if (!this.state.failed)
                    ++num2;
                  else
                    break;
                }
                else
                  goto label_34;
              }
              return selectExprReturn;
label_34:
              if (num2 < 1)
              {
                if (this.state.backtracking <= 0)
                  throw new EarlyExitException(24, (IIntStream) this.input);
                this.state.failed = true;
                return selectExprReturn;
              }
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                return selectExprReturn;
              break;
            }
            if (this.state.backtracking <= 0)
              throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
            this.state.failed = true;
            return selectExprReturn;
          case 4:
            this.PushFollow(SqlGenerator.FOLLOW_methodCall_in_selectExpr665);
            this.methodCall();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return selectExprReturn;
            break;
          case 5:
            this.PushFollow(SqlGenerator.FOLLOW_aggregate_in_selectExpr670);
            this.aggregate();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return selectExprReturn;
            break;
          case 6:
            this.PushFollow(SqlGenerator.FOLLOW_constant_in_selectExpr677);
            SqlGenerator.constant_return constantReturn = this.constant();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              return selectExprReturn;
            this.Out(constantReturn != null ? (IASTNode) constantReturn.Start : (IASTNode) null);
            break;
          case 7:
            this.PushFollow(SqlGenerator.FOLLOW_arithmeticExpr_in_selectExpr684);
            this.arithmeticExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return selectExprReturn;
            break;
          case 8:
            this.PushFollow(SqlGenerator.FOLLOW_parameter_in_selectExpr689);
            this.parameter();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return selectExprReturn;
            break;
          case 9:
            if (this.state.backtracking == 0)
              this.Out("(");
            this.PushFollow(SqlGenerator.FOLLOW_selectStatement_in_selectExpr698);
            this.selectStatement();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              return selectExprReturn;
            this.Out(")");
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return selectExprReturn;
    }

    public void count()
    {
      try
      {
        this.Match((IIntStream) this.input, 12, SqlGenerator.FOLLOW_COUNT_in_count712);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out("count(");
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        int num = 2;
        switch (this.input.LA(1))
        {
          case 4:
          case 16:
            num = 1;
            break;
        }
        if (num == 1)
        {
          this.PushFollow(SqlGenerator.FOLLOW_distinctOrAll_in_count719);
          this.distinctOrAll();
          --this.state.followingStackPointer;
          if (this.state.failed)
            return;
        }
        this.PushFollow(SqlGenerator.FOLLOW_countExpr_in_count725);
        this.countExpr();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(")");
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void distinctOrAll()
    {
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 4:
            num1 = 2;
            break;
          case 16:
            num1 = 1;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 28, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num1)
        {
          case 1:
            this.Match((IIntStream) this.input, 16, SqlGenerator.FOLLOW_DISTINCT_in_distinctOrAll740);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out("distinct ");
            break;
          case 2:
            this.Match((IIntStream) this.input, 4, SqlGenerator.FOLLOW_ALL_in_distinctOrAll748);
            if (this.state.failed)
              break;
            if (this.input.LA(1) == 2)
            {
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              if (this.state.failed)
                break;
              do
              {
                int num2 = 2;
                int num3 = this.input.LA(1);
                if (num3 >= 4 && num3 <= 150)
                  num2 = 1;
                else if (num3 == 3)
                  num2 = 2;
                if (num2 == 1)
                  this.MatchAny((IIntStream) this.input);
                else
                  goto label_23;
              }
              while (!this.state.failed);
              break;
label_23:
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                break;
            }
            if (this.state.backtracking != 0)
              break;
            this.Out("all ");
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void countExpr()
    {
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 88)
          num2 = 1;
        else if (num1 == 12 || num1 == 15 || num1 == 20 || num1 == 39 || num1 == 51 || num1 == 57 || num1 == 71 || num1 == 74 || num1 == 78 || num1 == 81 || num1 == 90 || num1 >= 94 && num1 <= 100 || num1 == 106 || num1 >= 114 && num1 <= 121 || num1 >= 124 && num1 <= 125 || num1 == 141 || num1 == 143 || num1 == 149)
        {
          num2 = 2;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 29, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
        }
        switch (num2)
        {
          case 1:
            this.Match((IIntStream) this.input, 88, SqlGenerator.FOLLOW_ROW_STAR_in_countExpr767);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out("*");
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_simpleExpr_in_countExpr774);
            this.simpleExpr();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public SqlGenerator.selectAtom_return selectAtom()
    {
      SqlGenerator.selectAtom_return selectAtomReturn = new SqlGenerator.selectAtom_return();
      selectAtomReturn.Start = this.input.LT(1);
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 15:
            num1 = 1;
            break;
          case 141:
            num1 = 3;
            break;
          case 143:
            num1 = 2;
            break;
          case 145:
            num1 = 4;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 34, 0, (IIntStream) this.input);
            this.state.failed = true;
            return selectAtomReturn;
        }
        switch (num1)
        {
          case 1:
            this.Match((IIntStream) this.input, 15, SqlGenerator.FOLLOW_DOT_in_selectAtom786);
            if (this.state.failed || this.input.LA(1) != 2)
              return selectAtomReturn;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            do
            {
              int num2 = 2;
              int num3 = this.input.LA(1);
              if (num3 >= 4 && num3 <= 150)
                num2 = 1;
              else if (num3 == 3)
                num2 = 2;
              if (num2 == 1)
                this.MatchAny((IIntStream) this.input);
              else
                goto label_21;
            }
            while (!this.state.failed);
            return selectAtomReturn;
label_21:
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            break;
          case 2:
            this.Match((IIntStream) this.input, 143, SqlGenerator.FOLLOW_SQL_TOKEN_in_selectAtom796);
            if (this.state.failed || this.input.LA(1) != 2)
              return selectAtomReturn;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            do
            {
              int num4 = 2;
              int num5 = this.input.LA(1);
              if (num5 >= 4 && num5 <= 150)
                num4 = 1;
              else if (num5 == 3)
                num4 = 2;
              if (num4 == 1)
                this.MatchAny((IIntStream) this.input);
              else
                goto label_34;
            }
            while (!this.state.failed);
            return selectAtomReturn;
label_34:
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            break;
          case 3:
            this.Match((IIntStream) this.input, 141, SqlGenerator.FOLLOW_ALIAS_REF_in_selectAtom806);
            if (this.state.failed || this.input.LA(1) != 2)
              return selectAtomReturn;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            do
            {
              int num6 = 2;
              int num7 = this.input.LA(1);
              if (num7 >= 4 && num7 <= 150)
                num6 = 1;
              else if (num7 == 3)
                num6 = 2;
              if (num6 == 1)
                this.MatchAny((IIntStream) this.input);
              else
                goto label_47;
            }
            while (!this.state.failed);
            return selectAtomReturn;
label_47:
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            break;
          case 4:
            this.Match((IIntStream) this.input, 145, SqlGenerator.FOLLOW_SELECT_EXPR_in_selectAtom816);
            if (this.state.failed || this.input.LA(1) != 2)
              return selectAtomReturn;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            do
            {
              int num8 = 2;
              int num9 = this.input.LA(1);
              if (num9 >= 4 && num9 <= 150)
                num8 = 1;
              else if (num9 == 3)
                num8 = 2;
              if (num8 == 1)
                this.MatchAny((IIntStream) this.input);
              else
                goto label_60;
            }
            while (!this.state.failed);
            return selectAtomReturn;
label_60:
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return selectAtomReturn;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return selectAtomReturn;
    }

    public void from()
    {
      try
      {
        IASTNode astNode = (IASTNode) this.Match((IIntStream) this.input, 22, SqlGenerator.FOLLOW_FROM_in_from839);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(" from ");
        if (this.input.LA(1) != 2)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        do
        {
          int num = 2;
          switch (this.input.LA(1))
          {
            case 135:
            case 137:
              num = 1;
              break;
          }
          if (num == 1)
          {
            this.PushFollow(SqlGenerator.FOLLOW_fromTable_in_from846);
            this.fromTable();
            --this.state.followingStackPointer;
          }
          else
            goto label_12;
        }
        while (!this.state.failed);
        return;
label_12:
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void fromTable()
    {
      IASTNode astNode = (IASTNode) null;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 135:
            num1 = 1;
            break;
          case 137:
            num1 = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 38, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num1)
        {
          case 1:
            astNode = (IASTNode) this.Match((IIntStream) this.input, 135, SqlGenerator.FOLLOW_FROM_FRAGMENT_in_fromTable872);
            if (this.state.failed)
              return;
            if (this.state.backtracking == 0)
              this.Out(astNode);
            if (this.input.LA(1) == 2)
            {
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              if (this.state.failed)
                return;
              do
              {
                int num2 = 2;
                switch (this.input.LA(1))
                {
                  case 135:
                  case 137:
                    num2 = 1;
                    break;
                }
                if (num2 == 1)
                {
                  this.PushFollow(SqlGenerator.FOLLOW_tableJoin_in_fromTable878);
                  this.tableJoin(astNode);
                  --this.state.followingStackPointer;
                }
                else
                  goto label_21;
              }
              while (!this.state.failed);
              return;
label_21:
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                return;
              break;
            }
            break;
          case 2:
            astNode = (IASTNode) this.Match((IIntStream) this.input, 137, SqlGenerator.FOLLOW_JOIN_FRAGMENT_in_fromTable893);
            if (this.state.failed)
              return;
            if (this.state.backtracking == 0)
              this.Out(astNode);
            if (this.input.LA(1) == 2)
            {
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              if (this.state.failed)
                return;
              do
              {
                int num3 = 2;
                switch (this.input.LA(1))
                {
                  case 135:
                  case 137:
                    num3 = 1;
                    break;
                }
                if (num3 == 1)
                {
                  this.PushFollow(SqlGenerator.FOLLOW_tableJoin_in_fromTable899);
                  this.tableJoin(astNode);
                  --this.state.followingStackPointer;
                }
                else
                  goto label_35;
              }
              while (!this.state.failed);
              return;
label_35:
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                return;
              break;
            }
            break;
        }
        if (this.state.backtracking != 0)
          return;
        this.FromFragmentSeparator(astNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void tableJoin(IASTNode parent)
    {
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 135:
            num1 = 2;
            break;
          case 137:
            num1 = 1;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 41, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num1)
        {
          case 1:
            IASTNode astNode1 = (IASTNode) this.Match((IIntStream) this.input, 137, SqlGenerator.FOLLOW_JOIN_FRAGMENT_in_tableJoin922);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
            {
              this.Out(" ");
              this.Out(astNode1);
            }
            if (this.input.LA(1) != 2)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            do
            {
              int num2 = 2;
              switch (this.input.LA(1))
              {
                case 135:
                case 137:
                  num2 = 1;
                  break;
              }
              if (num2 == 1)
              {
                this.PushFollow(SqlGenerator.FOLLOW_tableJoin_in_tableJoin927);
                this.tableJoin(astNode1);
                --this.state.followingStackPointer;
              }
              else
                goto label_21;
            }
            while (!this.state.failed);
            break;
label_21:
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            IASTNode astNode2 = (IASTNode) this.Match((IIntStream) this.input, 135, SqlGenerator.FOLLOW_FROM_FRAGMENT_in_tableJoin943);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.NestedFromFragment(astNode2, parent);
            if (this.input.LA(1) != 2)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            do
            {
              int num3 = 2;
              switch (this.input.LA(1))
              {
                case 135:
                case 137:
                  num3 = 1;
                  break;
              }
              if (num3 == 1)
              {
                this.PushFollow(SqlGenerator.FOLLOW_tableJoin_in_tableJoin948);
                this.tableJoin(astNode2);
                --this.state.followingStackPointer;
              }
              else
                goto label_35;
            }
            while (!this.state.failed);
            break;
label_35:
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void booleanOp(bool parens)
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 6:
            num = 1;
            break;
          case 38:
            num = 3;
            break;
          case 40:
            num = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 42, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.Match((IIntStream) this.input, 6, SqlGenerator.FOLLOW_AND_in_booleanOp968);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_booleanOp970);
            this.booleanExpr(true);
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" and ");
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_booleanOp975);
            this.booleanExpr(true);
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.Match((IIntStream) this.input, 40, SqlGenerator.FOLLOW_OR_in_booleanOp983);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0 && parens)
              this.Out("(");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_booleanOp987);
            this.booleanExpr(false);
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" or ");
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_booleanOp992);
            this.booleanExpr(false);
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0 && parens)
              this.Out(")");
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.Match((IIntStream) this.input, 38, SqlGenerator.FOLLOW_NOT_in_booleanOp1002);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" not (");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_booleanOp1006);
            this.booleanExpr(false);
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(")");
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void booleanExpr(bool parens)
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 6:
          case 38:
          case 40:
            num = 1;
            break;
          case 10:
          case 19:
          case 26:
          case 34:
          case 79:
          case 80:
          case 82:
          case 83:
          case 84:
          case 102:
          case 107:
          case 109:
          case 110:
          case 111:
          case 112:
            num = 2;
            break;
          case 81:
            num = 3;
            break;
          case 143:
            num = 4;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 43, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_booleanOp_in_booleanExpr1023);
            this.booleanOp(parens);
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_comparisonExpr_in_booleanExpr1030);
            this.comparisonExpr(parens);
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.PushFollow(SqlGenerator.FOLLOW_methodCall_in_booleanExpr1037);
            this.methodCall();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 4:
            IASTNode n = (IASTNode) this.Match((IIntStream) this.input, 143, SqlGenerator.FOLLOW_SQL_TOKEN_in_booleanExpr1044);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(n);
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void comparisonExpr(bool parens)
    {
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 102 || num1 == 107 || num1 >= 109 && num1 <= 112)
          num2 = 1;
        else if (num1 == 10 || num1 == 19 || num1 == 26 || num1 == 34 || num1 >= 79 && num1 <= 80 || num1 >= 82 && num1 <= 84)
        {
          num2 = 2;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 44, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
        }
        switch (num2)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_binaryComparisonExpression_in_comparisonExpr1060);
            this.binaryComparisonExpression();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 2:
            if (this.state.backtracking == 0 && parens)
              this.Out("(");
            this.PushFollow(SqlGenerator.FOLLOW_exoticComparisonExpression_in_comparisonExpr1067);
            this.exoticComparisonExpression();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0 || !parens)
              break;
            this.Out(")");
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void binaryComparisonExpression()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 102:
            num = 1;
            break;
          case 107:
            num = 2;
            break;
          case 109:
            num = 5;
            break;
          case 110:
            num = 3;
            break;
          case 111:
            num = 6;
            break;
          case 112:
            num = 4;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 45, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.Match((IIntStream) this.input, 102, SqlGenerator.FOLLOW_EQ_in_binaryComparisonExpression1082);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1084);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("=");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1088);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.Match((IIntStream) this.input, 107, SqlGenerator.FOLLOW_NE_in_binaryComparisonExpression1095);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1097);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("<>");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1101);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.Match((IIntStream) this.input, 110, SqlGenerator.FOLLOW_GT_in_binaryComparisonExpression1108);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1110);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(">");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1114);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 4:
            this.Match((IIntStream) this.input, 112, SqlGenerator.FOLLOW_GE_in_binaryComparisonExpression1121);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1123);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(">=");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1127);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 5:
            this.Match((IIntStream) this.input, 109, SqlGenerator.FOLLOW_LT_in_binaryComparisonExpression1134);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1136);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("<");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1140);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 6:
            this.Match((IIntStream) this.input, 111, SqlGenerator.FOLLOW_LE_in_binaryComparisonExpression1147);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1149);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("<=");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_binaryComparisonExpression1153);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void exoticComparisonExpression()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 10:
            num = 3;
            break;
          case 19:
            num = 7;
            break;
          case 26:
            num = 5;
            break;
          case 34:
            num = 1;
            break;
          case 79:
            num = 9;
            break;
          case 80:
            num = 8;
            break;
          case 82:
            num = 4;
            break;
          case 83:
            num = 6;
            break;
          case 84:
            num = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 46, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.Match((IIntStream) this.input, 34, SqlGenerator.FOLLOW_LIKE_in_exoticComparisonExpression1167);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1169);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" like ");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1173);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_likeEscape_in_exoticComparisonExpression1175);
            this.likeEscape();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.Match((IIntStream) this.input, 84, SqlGenerator.FOLLOW_NOT_LIKE_in_exoticComparisonExpression1183);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1185);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" not like ");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1189);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_likeEscape_in_exoticComparisonExpression1191);
            this.likeEscape();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.Match((IIntStream) this.input, 10, SqlGenerator.FOLLOW_BETWEEN_in_exoticComparisonExpression1198);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1200);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" between ");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1204);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" and ");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1208);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 4:
            this.Match((IIntStream) this.input, 82, SqlGenerator.FOLLOW_NOT_BETWEEN_in_exoticComparisonExpression1215);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1217);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" not between ");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1221);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" and ");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1225);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 5:
            this.Match((IIntStream) this.input, 26, SqlGenerator.FOLLOW_IN_in_exoticComparisonExpression1232);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1234);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" in");
            this.PushFollow(SqlGenerator.FOLLOW_inList_in_exoticComparisonExpression1238);
            this.inList();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 6:
            this.Match((IIntStream) this.input, 83, SqlGenerator.FOLLOW_NOT_IN_in_exoticComparisonExpression1246);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1248);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(" not in ");
            this.PushFollow(SqlGenerator.FOLLOW_inList_in_exoticComparisonExpression1252);
            this.inList();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 7:
            this.Match((IIntStream) this.input, 19, SqlGenerator.FOLLOW_EXISTS_in_exoticComparisonExpression1260);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
            {
              this.OptionalSpace();
              this.Out("exists ");
            }
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_quantified_in_exoticComparisonExpression1264);
            this.quantified();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 8:
            this.Match((IIntStream) this.input, 80, SqlGenerator.FOLLOW_IS_NULL_in_exoticComparisonExpression1272);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1274);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(" is null");
            break;
          case 9:
            this.Match((IIntStream) this.input, 79, SqlGenerator.FOLLOW_IS_NOT_NULL_in_exoticComparisonExpression1283);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_exoticComparisonExpression1285);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(" is not null");
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void likeEscape()
    {
      try
      {
        int num = 2;
        if (this.input.LA(1) == 18)
          num = 1;
        if (num != 1)
          return;
        this.Match((IIntStream) this.input, 18, SqlGenerator.FOLLOW_ESCAPE_in_likeEscape1302);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(" escape ");
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_expr_in_likeEscape1306);
        this.expr();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void inList()
    {
      try
      {
        this.Match((IIntStream) this.input, 77, SqlGenerator.FOLLOW_IN_LIST_in_inList1322);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(" ");
        if (this.input.LA(1) != 2)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 45 || num1 == 52)
          num2 = 1;
        else if (num1 == 3 || num1 == 12 || num1 == 15 || num1 == 20 || num1 == 39 || num1 == 51 || num1 == 57 || num1 == 71 || num1 == 74 || num1 == 78 || num1 == 81 || num1 == 90 || num1 >= 94 && num1 <= 100 || num1 == 106 || num1 >= 114 && num1 <= 121 || num1 >= 124 && num1 <= 125 || num1 == 141 || num1 == 143 || num1 == 149)
        {
          num2 = 2;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 48, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
        }
        switch (num2)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_parenSelect_in_inList1328);
            this.parenSelect();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_simpleExprList_in_inList1332);
            this.simpleExprList();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return;
            break;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void simpleExprList()
    {
      try
      {
        if (this.state.backtracking == 0)
          this.Out("(");
        while (true)
        {
          SqlGenerator.simpleExpr_return simpleExprReturn;
          do
          {
            int num1 = 2;
            int num2 = this.input.LA(1);
            if (num2 == 12 || num2 == 15 || num2 == 20 || num2 == 39 || num2 == 51 || num2 == 57 || num2 == 71 || num2 == 74 || num2 == 78 || num2 == 81 || num2 == 90 || num2 >= 94 && num2 <= 100 || num2 == 106 || num2 >= 114 && num2 <= 121 || num2 >= 124 && num2 <= 125 || num2 == 141 || num2 == 143 || num2 == 149)
              num1 = 1;
            if (num1 == 1)
            {
              this.PushFollow(SqlGenerator.FOLLOW_simpleExpr_in_simpleExprList1353);
              simpleExprReturn = this.simpleExpr();
              --this.state.followingStackPointer;
              if (this.state.failed)
                goto label_7;
            }
            else
              goto label_10;
          }
          while (this.state.backtracking != 0);
          this.Separator(simpleExprReturn != null ? (IASTNode) simpleExprReturn.Start : (IASTNode) null, " , ");
        }
label_7:
        return;
label_10:
        if (this.state.backtracking != 0)
          return;
        this.Out(")");
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public SqlGenerator.expr_return expr()
    {
      SqlGenerator.expr_return exprReturn1 = new SqlGenerator.expr_return();
      exprReturn1.Start = this.input.LT(1);
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 4:
            num1 = 5;
            break;
          case 5:
            num1 = 4;
            break;
          case 12:
          case 15:
          case 20:
          case 39:
          case 51:
          case 57:
          case 71:
          case 74:
          case 78:
          case 81:
          case 90:
          case 94:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 100:
          case 106:
          case 114:
          case 115:
          case 116:
          case 117:
          case 118:
          case 119:
          case 120:
          case 121:
          case 124:
          case 125:
          case 141:
          case 143:
          case 149:
            num1 = 1;
            break;
          case 45:
          case 52:
            num1 = 3;
            break;
          case 48:
            num1 = 6;
            break;
          case 92:
            num1 = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 51, 0, (IIntStream) this.input);
            this.state.failed = true;
            return exprReturn1;
        }
        switch (num1)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_simpleExpr_in_expr1372);
            this.simpleExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return exprReturn1;
            break;
          case 2:
            this.Match((IIntStream) this.input, 92, SqlGenerator.FOLLOW_VECTOR_EXPR_in_expr1379);
            if (this.state.failed)
              return exprReturn1;
            if (this.state.backtracking == 0)
              this.Out("(");
            if (this.input.LA(1) == 2)
            {
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              if (this.state.failed)
                return exprReturn1;
              while (true)
              {
                SqlGenerator.expr_return exprReturn2;
                do
                {
                  int num2 = 2;
                  int num3 = this.input.LA(1);
                  if (num3 >= 4 && num3 <= 5 || num3 == 12 || num3 == 15 || num3 == 20 || num3 == 39 || num3 == 45 || num3 == 48 || num3 >= 51 && num3 <= 52 || num3 == 57 || num3 == 71 || num3 == 74 || num3 == 78 || num3 == 81 || num3 == 90 || num3 == 92 || num3 >= 94 && num3 <= 100 || num3 == 106 || num3 >= 114 && num3 <= 121 || num3 >= 124 && num3 <= 125 || num3 == 141 || num3 == 143 || num3 == 149)
                    num2 = 1;
                  if (num2 == 1)
                  {
                    this.PushFollow(SqlGenerator.FOLLOW_expr_in_expr1386);
                    exprReturn2 = this.expr();
                    --this.state.followingStackPointer;
                    if (this.state.failed)
                      return exprReturn1;
                  }
                  else
                    goto label_28;
                }
                while (this.state.backtracking != 0);
                this.Separator(exprReturn2 != null ? (IASTNode) exprReturn2.Start : (IASTNode) null, " , ");
              }
label_28:
              if (this.state.backtracking == 0)
                this.Out(")");
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                return exprReturn1;
              break;
            }
            break;
          case 3:
            this.PushFollow(SqlGenerator.FOLLOW_parenSelect_in_expr1401);
            this.parenSelect();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return exprReturn1;
            break;
          case 4:
            this.Match((IIntStream) this.input, 5, SqlGenerator.FOLLOW_ANY_in_expr1407);
            if (this.state.failed)
              return exprReturn1;
            if (this.state.backtracking == 0)
              this.Out("any ");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return exprReturn1;
            this.PushFollow(SqlGenerator.FOLLOW_quantified_in_expr1411);
            this.quantified();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return exprReturn1;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return exprReturn1;
            break;
          case 5:
            this.Match((IIntStream) this.input, 4, SqlGenerator.FOLLOW_ALL_in_expr1419);
            if (this.state.failed)
              return exprReturn1;
            if (this.state.backtracking == 0)
              this.Out("all ");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return exprReturn1;
            this.PushFollow(SqlGenerator.FOLLOW_quantified_in_expr1423);
            this.quantified();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return exprReturn1;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return exprReturn1;
            break;
          case 6:
            this.Match((IIntStream) this.input, 48, SqlGenerator.FOLLOW_SOME_in_expr1431);
            if (this.state.failed)
              return exprReturn1;
            if (this.state.backtracking == 0)
              this.Out("some ");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return exprReturn1;
            this.PushFollow(SqlGenerator.FOLLOW_quantified_in_expr1435);
            this.quantified();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return exprReturn1;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return exprReturn1;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return exprReturn1;
    }

    public void quantified()
    {
      try
      {
        if (this.state.backtracking == 0)
          this.Out("(");
        int num;
        switch (this.input.LA(1))
        {
          case 45:
            num = 2;
            break;
          case 143:
            num = 1;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 52, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_sqlToken_in_quantified1453);
            this.sqlToken();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_selectStatement_in_quantified1457);
            this.selectStatement();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return;
            break;
        }
        if (this.state.backtracking != 0)
          return;
        this.Out(")");
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void parenSelect()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 45:
            num = 1;
            break;
          case 52:
            num = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 53, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            if (this.state.backtracking == 0)
              this.Out("(");
            this.PushFollow(SqlGenerator.FOLLOW_selectStatement_in_parenSelect1476);
            this.selectStatement();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(")");
            break;
          case 2:
            this.Match((IIntStream) this.input, 52, SqlGenerator.FOLLOW_UNION_in_parenSelect1485);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("(");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_selectStatement_in_parenSelect1489);
            this.selectStatement();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out(") union ");
            this.PushFollow(SqlGenerator.FOLLOW_parenSelect_in_parenSelect1493);
            this.parenSelect();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public SqlGenerator.simpleExpr_return simpleExpr()
    {
      SqlGenerator.simpleExpr_return simpleExprReturn = new SqlGenerator.simpleExpr_return();
      simpleExprReturn.Start = this.input.LT(1);
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 12:
            num = 7;
            break;
          case 15:
          case 78:
          case 141:
            num = 3;
            break;
          case 20:
          case 51:
          case 94:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 100:
          case 124:
          case 125:
            num = 1;
            break;
          case 39:
            num = 2;
            break;
          case 57:
          case 74:
          case 90:
          case 114:
          case 115:
          case 116:
          case 117:
          case 118:
          case 119:
          case 120:
          case 121:
            num = 9;
            break;
          case 71:
            num = 5;
            break;
          case 81:
            num = 6;
            break;
          case 106:
          case 149:
            num = 8;
            break;
          case 143:
            num = 4;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 54, 0, (IIntStream) this.input);
            this.state.failed = true;
            return simpleExprReturn;
        }
        switch (num)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_constant_in_simpleExpr1510);
            SqlGenerator.constant_return constantReturn = this.constant();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              return simpleExprReturn;
            this.Out(constantReturn != null ? (IASTNode) constantReturn.Start : (IASTNode) null);
            break;
          case 2:
            this.Match((IIntStream) this.input, 39, SqlGenerator.FOLLOW_NULL_in_simpleExpr1517);
            if (this.state.failed || this.state.backtracking != 0)
              return simpleExprReturn;
            this.Out("null");
            break;
          case 3:
            this.PushFollow(SqlGenerator.FOLLOW_addrExpr_in_simpleExpr1524);
            this.addrExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return simpleExprReturn;
            break;
          case 4:
            this.PushFollow(SqlGenerator.FOLLOW_sqlToken_in_simpleExpr1529);
            this.sqlToken();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return simpleExprReturn;
            break;
          case 5:
            this.PushFollow(SqlGenerator.FOLLOW_aggregate_in_simpleExpr1534);
            this.aggregate();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return simpleExprReturn;
            break;
          case 6:
            this.PushFollow(SqlGenerator.FOLLOW_methodCall_in_simpleExpr1539);
            this.methodCall();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return simpleExprReturn;
            break;
          case 7:
            this.PushFollow(SqlGenerator.FOLLOW_count_in_simpleExpr1544);
            this.count();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return simpleExprReturn;
            break;
          case 8:
            this.PushFollow(SqlGenerator.FOLLOW_parameter_in_simpleExpr1549);
            this.parameter();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return simpleExprReturn;
            break;
          case 9:
            this.PushFollow(SqlGenerator.FOLLOW_arithmeticExpr_in_simpleExpr1554);
            this.arithmeticExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return simpleExprReturn;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return simpleExprReturn;
    }

    public SqlGenerator.constant_return constant()
    {
      SqlGenerator.constant_return constantReturn = new SqlGenerator.constant_return();
      constantReturn.Start = this.input.LT(1);
      try
      {
        if (this.input.LA(1) == 20 || this.input.LA(1) == 51 || this.input.LA(1) >= 94 && this.input.LA(1) <= 100 || this.input.LA(1) >= 124 && this.input.LA(1) <= 125)
        {
          this.input.Consume();
          this.state.errorRecovery = false;
          this.state.failed = false;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
          this.state.failed = true;
          return constantReturn;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return constantReturn;
    }

    public void arithmeticExpr()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 57:
          case 74:
            num = 5;
            break;
          case 90:
            num = 4;
            break;
          case 114:
          case 115:
          case 116:
          case 117:
            num = 2;
            break;
          case 118:
          case 119:
            num = 1;
            break;
          case 120:
          case 121:
            num = 3;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 55, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_additiveExpr_in_arithmeticExpr1628);
            this.additiveExpr();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_bitwiseExpr_in_arithmeticExpr1633);
            this.bitwiseExpr();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.PushFollow(SqlGenerator.FOLLOW_multiplicativeExpr_in_arithmeticExpr1638);
            this.multiplicativeExpr();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
          case 4:
            this.Match((IIntStream) this.input, 90, SqlGenerator.FOLLOW_UNARY_MINUS_in_arithmeticExpr1645);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("-");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_arithmeticExpr1649);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 5:
            this.PushFollow(SqlGenerator.FOLLOW_caseExpr_in_arithmeticExpr1655);
            this.caseExpr();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void additiveExpr()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 118:
            num = 1;
            break;
          case 119:
            num = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 56, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.Match((IIntStream) this.input, 118, SqlGenerator.FOLLOW_PLUS_in_additiveExpr1667);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_additiveExpr1669);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("+");
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_additiveExpr1673);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.Match((IIntStream) this.input, 119, SqlGenerator.FOLLOW_MINUS_in_additiveExpr1680);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_additiveExpr1682);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("-");
            this.PushFollow(SqlGenerator.FOLLOW_nestedExprAfterMinusDiv_in_additiveExpr1686);
            this.nestedExprAfterMinusDiv();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void bitwiseExpr()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 114:
            num = 4;
            break;
          case 115:
            num = 2;
            break;
          case 116:
            num = 3;
            break;
          case 117:
            num = 1;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 57, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.Match((IIntStream) this.input, 117, SqlGenerator.FOLLOW_BAND_in_bitwiseExpr1699);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_bitwiseExpr1701);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("&");
            this.PushFollow(SqlGenerator.FOLLOW_nestedExpr_in_bitwiseExpr1705);
            this.nestedExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.Match((IIntStream) this.input, 115, SqlGenerator.FOLLOW_BOR_in_bitwiseExpr1712);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_bitwiseExpr1714);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("|");
            this.PushFollow(SqlGenerator.FOLLOW_nestedExpr_in_bitwiseExpr1718);
            this.nestedExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 3:
            this.Match((IIntStream) this.input, 116, SqlGenerator.FOLLOW_BXOR_in_bitwiseExpr1725);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_bitwiseExpr1727);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("^");
            this.PushFollow(SqlGenerator.FOLLOW_nestedExpr_in_bitwiseExpr1731);
            this.nestedExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 4:
            this.Match((IIntStream) this.input, 114, SqlGenerator.FOLLOW_BNOT_in_bitwiseExpr1738);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("~");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_nestedExpr_in_bitwiseExpr1742);
            this.nestedExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void multiplicativeExpr()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 120:
            num = 1;
            break;
          case 121:
            num = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 58, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            this.Match((IIntStream) this.input, 120, SqlGenerator.FOLLOW_STAR_in_multiplicativeExpr1756);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_nestedExpr_in_multiplicativeExpr1758);
            this.nestedExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("*");
            this.PushFollow(SqlGenerator.FOLLOW_nestedExpr_in_multiplicativeExpr1762);
            this.nestedExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.Match((IIntStream) this.input, 121, SqlGenerator.FOLLOW_DIV_in_multiplicativeExpr1769);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_nestedExpr_in_multiplicativeExpr1771);
            this.nestedExpr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("/");
            this.PushFollow(SqlGenerator.FOLLOW_nestedExprAfterMinusDiv_in_multiplicativeExpr1775);
            this.nestedExprAfterMinusDiv();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void nestedExpr()
    {
      try
      {
        switch (this.dfa59.Predict((IIntStream) this.input))
        {
          case 1:
            if (this.state.backtracking == 0)
              this.Out("(");
            this.PushFollow(SqlGenerator.FOLLOW_additiveExpr_in_nestedExpr1797);
            this.additiveExpr();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(")");
            break;
          case 2:
            if (this.state.backtracking == 0)
              this.Out("(");
            this.PushFollow(SqlGenerator.FOLLOW_bitwiseExpr_in_nestedExpr1812);
            this.bitwiseExpr();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(")");
            break;
          case 3:
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_nestedExpr1819);
            this.expr();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void nestedExprAfterMinusDiv()
    {
      try
      {
        switch (this.dfa60.Predict((IIntStream) this.input))
        {
          case 1:
            if (this.state.backtracking == 0)
              this.Out("(");
            this.PushFollow(SqlGenerator.FOLLOW_arithmeticExpr_in_nestedExprAfterMinusDiv1841);
            this.arithmeticExpr();
            --this.state.followingStackPointer;
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(")");
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_nestedExprAfterMinusDiv1848);
            this.expr();
            --this.state.followingStackPointer;
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void caseExpr()
    {
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 57:
            num1 = 1;
            break;
          case 74:
            num1 = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 65, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num1)
        {
          case 1:
            this.Match((IIntStream) this.input, 57, SqlGenerator.FOLLOW_CASE_in_caseExpr1860);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("case");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            int num2 = 0;
            while (true)
            {
              int num3 = 2;
              if (this.input.LA(1) == 61)
                num3 = 1;
              if (num3 == 1)
              {
                this.Match((IIntStream) this.input, 61, SqlGenerator.FOLLOW_WHEN_in_caseExpr1870);
                if (!this.state.failed)
                {
                  if (this.state.backtracking == 0)
                    this.Out(" when ");
                  this.Match((IIntStream) this.input, 2, (BitSet) null);
                  if (!this.state.failed)
                  {
                    this.PushFollow(SqlGenerator.FOLLOW_booleanExpr_in_caseExpr1874);
                    this.booleanExpr(false);
                    --this.state.followingStackPointer;
                    if (!this.state.failed)
                    {
                      if (this.state.backtracking == 0)
                        this.Out(" then ");
                      this.PushFollow(SqlGenerator.FOLLOW_expr_in_caseExpr1879);
                      this.expr();
                      --this.state.followingStackPointer;
                      if (!this.state.failed)
                      {
                        this.Match((IIntStream) this.input, 3, (BitSet) null);
                        if (!this.state.failed)
                          ++num2;
                        else
                          goto label_67;
                      }
                      else
                        goto label_71;
                    }
                    else
                      goto label_73;
                  }
                  else
                    goto label_77;
                }
                else
                  break;
              }
              else
                goto label_32;
            }
            break;
label_77:
            break;
label_73:
            break;
label_71:
            break;
label_67:
            break;
label_32:
            if (num2 < 1)
            {
              if (this.state.backtracking <= 0)
                throw new EarlyExitException(61, (IIntStream) this.input);
              this.state.failed = true;
              break;
            }
            int num4 = 2;
            if (this.input.LA(1) == 59)
              num4 = 1;
            if (num4 == 1)
            {
              this.Match((IIntStream) this.input, 59, SqlGenerator.FOLLOW_ELSE_in_caseExpr1891);
              if (this.state.failed)
                break;
              if (this.state.backtracking == 0)
                this.Out(" else ");
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              if (this.state.failed)
                break;
              this.PushFollow(SqlGenerator.FOLLOW_expr_in_caseExpr1895);
              this.expr();
              --this.state.followingStackPointer;
              if (this.state.failed)
                break;
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                break;
            }
            if (this.state.backtracking == 0)
              this.Out(" end");
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
          case 2:
            this.Match((IIntStream) this.input, 74, SqlGenerator.FOLLOW_CASE2_in_caseExpr1911);
            if (this.state.failed)
              break;
            if (this.state.backtracking == 0)
              this.Out("case ");
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_caseExpr1915);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              break;
            int num5 = 0;
            while (true)
            {
              int num6 = 2;
              if (this.input.LA(1) == 61)
                num6 = 1;
              if (num6 == 1)
              {
                this.Match((IIntStream) this.input, 61, SqlGenerator.FOLLOW_WHEN_in_caseExpr1922);
                if (!this.state.failed)
                {
                  if (this.state.backtracking == 0)
                    this.Out(" when ");
                  this.Match((IIntStream) this.input, 2, (BitSet) null);
                  if (!this.state.failed)
                  {
                    this.PushFollow(SqlGenerator.FOLLOW_expr_in_caseExpr1926);
                    this.expr();
                    --this.state.followingStackPointer;
                    if (!this.state.failed)
                    {
                      if (this.state.backtracking == 0)
                        this.Out(" then ");
                      this.PushFollow(SqlGenerator.FOLLOW_expr_in_caseExpr1930);
                      this.expr();
                      --this.state.followingStackPointer;
                      if (!this.state.failed)
                      {
                        this.Match((IIntStream) this.input, 3, (BitSet) null);
                        if (!this.state.failed)
                          ++num5;
                        else
                          goto label_19;
                      }
                      else
                        goto label_23;
                    }
                    else
                      goto label_25;
                  }
                  else
                    goto label_29;
                }
                else
                  break;
              }
              else
                goto label_80;
            }
            break;
label_29:
            break;
label_25:
            break;
label_23:
            break;
label_19:
            break;
label_80:
            if (num5 < 1)
            {
              if (this.state.backtracking <= 0)
                throw new EarlyExitException(63, (IIntStream) this.input);
              this.state.failed = true;
              break;
            }
            int num7 = 2;
            if (this.input.LA(1) == 59)
              num7 = 1;
            if (num7 == 1)
            {
              this.Match((IIntStream) this.input, 59, SqlGenerator.FOLLOW_ELSE_in_caseExpr1942);
              if (this.state.failed)
                break;
              if (this.state.backtracking == 0)
                this.Out(" else ");
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              if (this.state.failed)
                break;
              this.PushFollow(SqlGenerator.FOLLOW_expr_in_caseExpr1946);
              this.expr();
              --this.state.followingStackPointer;
              if (this.state.failed)
                break;
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                break;
            }
            if (this.state.backtracking == 0)
              this.Out(" end");
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (!this.state.failed)
              break;
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void aggregate()
    {
      try
      {
        IASTNode n = (IASTNode) this.Match((IIntStream) this.input, 71, SqlGenerator.FOLLOW_AGGREGATE_in_aggregate1970);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
        {
          this.Out(n);
          this.Out("(");
        }
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        this.PushFollow(SqlGenerator.FOLLOW_expr_in_aggregate1975);
        this.expr();
        --this.state.followingStackPointer;
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(")");
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void methodCall()
    {
      try
      {
        IASTNode m = (IASTNode) this.Match((IIntStream) this.input, 81, SqlGenerator.FOLLOW_METHOD_CALL_in_methodCall1994);
        if (this.state.failed)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        IASTNode i = (IASTNode) this.Match((IIntStream) this.input, 148, SqlGenerator.FOLLOW_METHOD_NAME_in_methodCall1998);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.BeginFunctionTemplate(m, i);
        int num1 = 2;
        if (this.input.LA(1) == 75)
          num1 = 1;
        if (num1 == 1)
        {
          this.Match((IIntStream) this.input, 75, SqlGenerator.FOLLOW_EXPR_LIST_in_methodCall2007);
          if (this.state.failed)
            return;
          if (this.input.LA(1) == 2)
          {
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              return;
            int num2 = 2;
            int num3 = this.input.LA(1);
            if (num3 >= 4 && num3 <= 5 || num3 == 10 || num3 == 12 || num3 == 15 || num3 >= 19 && num3 <= 20 || num3 == 26 || num3 == 34 || num3 == 39 || num3 == 45 || num3 == 48 || num3 >= 51 && num3 <= 52 || num3 == 57 || num3 == 71 || num3 == 74 || num3 >= 78 && num3 <= 84 || num3 == 90 || num3 == 92 || num3 >= 94 && num3 <= 100 || num3 == 102 || num3 >= 106 && num3 <= 107 || num3 >= 109 && num3 <= 112 || num3 >= 114 && num3 <= 121 || num3 >= 124 && num3 <= 125 || num3 == 141 || num3 == 143 || num3 == 149)
              num2 = 1;
            if (num2 == 1)
            {
              this.PushFollow(SqlGenerator.FOLLOW_arguments_in_methodCall2010);
              this.arguments();
              --this.state.followingStackPointer;
              if (this.state.failed)
                return;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed)
              return;
          }
        }
        if (this.state.backtracking == 0)
          this.EndFunctionTemplate(m);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void arguments()
    {
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 >= 4 && num1 <= 5 || num1 == 12 || num1 == 15 || num1 == 20 || num1 == 39 || num1 == 45 || num1 == 48 || num1 >= 51 && num1 <= 52 || num1 == 57 || num1 == 71 || num1 == 74 || num1 == 78 || num1 == 81 || num1 == 90 || num1 == 92 || num1 >= 94 && num1 <= 100 || num1 == 106 || num1 >= 114 && num1 <= 121 || num1 >= 124 && num1 <= 125 || num1 == 141 || num1 == 143 || num1 == 149)
          num2 = 1;
        else if (num1 == 10 || num1 == 19 || num1 == 26 || num1 == 34 || num1 >= 79 && num1 <= 80 || num1 >= 82 && num1 <= 84 || num1 == 102 || num1 == 107 || num1 >= 109 && num1 <= 112)
        {
          num2 = 2;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new NoViableAltException("", 68, 0, (IIntStream) this.input);
          this.state.failed = true;
          return;
        }
        switch (num2)
        {
          case 1:
            this.PushFollow(SqlGenerator.FOLLOW_expr_in_arguments2035);
            this.expr();
            --this.state.followingStackPointer;
            if (this.state.failed)
              return;
            break;
          case 2:
            this.PushFollow(SqlGenerator.FOLLOW_comparisonExpr_in_arguments2039);
            this.comparisonExpr(true);
            --this.state.followingStackPointer;
            if (this.state.failed)
              return;
            break;
        }
        do
        {
          do
          {
            int num3 = 2;
            int num4 = this.input.LA(1);
            if (num4 >= 4 && num4 <= 5 || num4 == 10 || num4 == 12 || num4 == 15 || num4 >= 19 && num4 <= 20 || num4 == 26 || num4 == 34 || num4 == 39 || num4 == 45 || num4 == 48 || num4 >= 51 && num4 <= 52 || num4 == 57 || num4 == 71 || num4 == 74 || num4 >= 78 && num4 <= 84 || num4 == 90 || num4 == 92 || num4 >= 94 && num4 <= 100 || num4 == 102 || num4 >= 106 && num4 <= 107 || num4 >= 109 && num4 <= 112 || num4 >= 114 && num4 <= 121 || num4 >= 124 && num4 <= 125 || num4 == 141 || num4 == 143 || num4 == 149)
              num3 = 1;
            if (num3 == 1)
            {
              if (this.state.backtracking == 0)
                this.CommaBetweenParameters(", ");
              int num5 = this.input.LA(1);
              int num6;
              if (num5 >= 4 && num5 <= 5 || num5 == 12 || num5 == 15 || num5 == 20 || num5 == 39 || num5 == 45 || num5 == 48 || num5 >= 51 && num5 <= 52 || num5 == 57 || num5 == 71 || num5 == 74 || num5 == 78 || num5 == 81 || num5 == 90 || num5 == 92 || num5 >= 94 && num5 <= 100 || num5 == 106 || num5 >= 114 && num5 <= 121 || num5 >= 124 && num5 <= 125 || num5 == 141 || num5 == 143 || num5 == 149)
                num6 = 1;
              else if (num5 == 10 || num5 == 19 || num5 == 26 || num5 == 34 || num5 >= 79 && num5 <= 80 || num5 >= 82 && num5 <= 84 || num5 == 102 || num5 == 107 || num5 >= 109 && num5 <= 112)
              {
                num6 = 2;
              }
              else
              {
                if (this.state.backtracking <= 0)
                  throw new NoViableAltException("", 69, 0, (IIntStream) this.input);
                this.state.failed = true;
                return;
              }
              switch (num6)
              {
                case 1:
                  this.PushFollow(SqlGenerator.FOLLOW_expr_in_arguments2048);
                  this.expr();
                  --this.state.followingStackPointer;
                  continue;
                case 2:
                  goto label_28;
                default:
                  continue;
              }
            }
            else
              goto label_30;
          }
          while (!this.state.failed);
          goto label_10;
label_28:
          this.PushFollow(SqlGenerator.FOLLOW_comparisonExpr_in_arguments2052);
          this.comparisonExpr(true);
          --this.state.followingStackPointer;
        }
        while (!this.state.failed);
        goto label_8;
label_30:
        return;
label_10:
        return;
label_8:;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void parameter()
    {
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 106:
            num = 2;
            break;
          case 149:
            num = 1;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 71, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num)
        {
          case 1:
            IASTNode n1 = (IASTNode) this.Match((IIntStream) this.input, 149, SqlGenerator.FOLLOW_NAMED_PARAM_in_parameter2070);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(n1);
            break;
          case 2:
            IASTNode n2 = (IASTNode) this.Match((IIntStream) this.input, 106, SqlGenerator.FOLLOW_PARAM_in_parameter2079);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(n2);
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public SqlGenerator.limitValue_return limitValue()
    {
      SqlGenerator.limitValue_return limitValueReturn = new SqlGenerator.limitValue_return();
      limitValueReturn.Start = this.input.LT(1);
      try
      {
        if (this.input.LA(1) == 95 || this.input.LA(1) == 106 || this.input.LA(1) == 149)
        {
          this.input.Consume();
          this.state.errorRecovery = false;
          this.state.failed = false;
        }
        else
        {
          if (this.state.backtracking <= 0)
            throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
          this.state.failed = true;
          return limitValueReturn;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return limitValueReturn;
    }

    public void addrExpr()
    {
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 15:
            num1 = 1;
            break;
          case 78:
            num1 = 3;
            break;
          case 141:
            num1 = 2;
            break;
          default:
            if (this.state.backtracking <= 0)
              throw new NoViableAltException("", 73, 0, (IIntStream) this.input);
            this.state.failed = true;
            return;
        }
        switch (num1)
        {
          case 1:
            IASTNode n1 = (IASTNode) this.Match((IIntStream) this.input, 15, SqlGenerator.FOLLOW_DOT_in_addrExpr2116);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            if (this.state.failed)
              break;
            this.MatchAny((IIntStream) this.input);
            if (this.state.failed)
              break;
            this.MatchAny((IIntStream) this.input);
            if (this.state.failed)
              break;
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(n1);
            break;
          case 2:
            IASTNode n2 = (IASTNode) this.Match((IIntStream) this.input, 141, SqlGenerator.FOLLOW_ALIAS_REF_in_addrExpr2130);
            if (this.state.failed || this.state.backtracking != 0)
              break;
            this.Out(n2);
            break;
          case 3:
            IASTNode n3 = (IASTNode) this.Match((IIntStream) this.input, 78, SqlGenerator.FOLLOW_INDEX_OP_in_addrExpr2140);
            if (this.state.failed)
              break;
            if (this.input.LA(1) == 2)
            {
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              if (this.state.failed)
                break;
              do
              {
                int num2 = 2;
                int num3 = this.input.LA(1);
                if (num3 >= 4 && num3 <= 150)
                  num2 = 1;
                else if (num3 == 3)
                  num2 = 2;
                if (num2 == 1)
                  this.MatchAny((IIntStream) this.input);
                else
                  goto label_36;
              }
              while (!this.state.failed);
              break;
label_36:
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              if (this.state.failed)
                break;
            }
            if (this.state.backtracking != 0)
              break;
            this.Out(n3);
            break;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void sqlToken()
    {
      try
      {
        IASTNode n = (IASTNode) this.Match((IIntStream) this.input, 143, SqlGenerator.FOLLOW_SQL_TOKEN_in_sqlToken2160);
        if (this.state.failed)
          return;
        if (this.state.backtracking == 0)
          this.Out(n);
        if (this.input.LA(1) != 2)
          return;
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        if (this.state.failed)
          return;
        do
        {
          int num1 = 2;
          int num2 = this.input.LA(1);
          if (num2 >= 4 && num2 <= 150)
            num1 = 1;
          else if (num2 == 3)
            num1 = 2;
          if (num1 == 1)
            this.MatchAny((IIntStream) this.input);
          else
            goto label_15;
        }
        while (!this.state.failed);
        return;
label_15:
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        if (!this.state.failed)
          ;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
    }

    public void synpred1_SqlGenerator_fragment()
    {
      this.Match((IIntStream) this.input, 143, SqlGenerator.FOLLOW_SQL_TOKEN_in_synpred1_SqlGenerator366);
      int num = this.state.failed ? 1 : 0;
    }

    public void synpred2_SqlGenerator_fragment()
    {
      this.PushFollow(SqlGenerator.FOLLOW_additiveExpr_in_synpred2_SqlGenerator1790);
      this.additiveExpr();
      --this.state.followingStackPointer;
      int num = this.state.failed ? 1 : 0;
    }

    public void synpred3_SqlGenerator_fragment()
    {
      this.PushFollow(SqlGenerator.FOLLOW_bitwiseExpr_in_synpred3_SqlGenerator1805);
      this.bitwiseExpr();
      --this.state.followingStackPointer;
      int num = this.state.failed ? 1 : 0;
    }

    public void synpred4_SqlGenerator_fragment()
    {
      this.PushFollow(SqlGenerator.FOLLOW_arithmeticExpr_in_synpred4_SqlGenerator1834);
      this.arithmeticExpr();
      --this.state.followingStackPointer;
      int num = this.state.failed ? 1 : 0;
    }

    public bool synpred2_SqlGenerator()
    {
      ++this.state.backtracking;
      int marker = this.input.Mark();
      try
      {
        this.synpred2_SqlGenerator_fragment();
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

    public bool synpred3_SqlGenerator()
    {
      ++this.state.backtracking;
      int marker = this.input.Mark();
      try
      {
        this.synpred3_SqlGenerator_fragment();
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

    public bool synpred4_SqlGenerator()
    {
      ++this.state.backtracking;
      int marker = this.input.Mark();
      try
      {
        this.synpred4_SqlGenerator_fragment();
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

    public bool synpred1_SqlGenerator()
    {
      ++this.state.backtracking;
      int marker = this.input.Mark();
      try
      {
        this.synpred1_SqlGenerator_fragment();
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

    private void InitializeCyclicDFAs()
    {
      this.dfa59 = new SqlGenerator.DFA59((BaseRecognizer) this);
      this.dfa60 = new SqlGenerator.DFA60((BaseRecognizer) this);
      this.dfa59.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(this.DFA59_SpecialStateTransition);
      this.dfa60.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(this.DFA60_SpecialStateTransition);
    }

    protected internal int DFA59_SpecialStateTransition(DFA dfa, int s, IIntStream _input)
    {
      ITreeNodeStream input = (ITreeNodeStream) _input;
      int stateNumber = s;
      switch (s)
      {
        case 0:
          input.LA(1);
          int index1 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred2_SqlGenerator() ? 7 : 29;
          input.Seek(index1);
          if (s >= 0)
            return s;
          break;
        case 1:
          input.LA(1);
          int index2 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred2_SqlGenerator() ? 7 : 29;
          input.Seek(index2);
          if (s >= 0)
            return s;
          break;
        case 2:
          input.LA(1);
          int index3 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred3_SqlGenerator() ? 7 : 30;
          input.Seek(index3);
          if (s >= 0)
            return s;
          break;
        case 3:
          input.LA(1);
          int index4 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred3_SqlGenerator() ? 7 : 30;
          input.Seek(index4);
          if (s >= 0)
            return s;
          break;
        case 4:
          input.LA(1);
          int index5 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred3_SqlGenerator() ? 7 : 30;
          input.Seek(index5);
          if (s >= 0)
            return s;
          break;
        case 5:
          input.LA(1);
          int index6 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred3_SqlGenerator() ? 7 : 30;
          input.Seek(index6);
          if (s >= 0)
            return s;
          break;
      }
      if (this.state.backtracking > 0)
      {
        this.state.failed = true;
        return -1;
      }
      NoViableAltException nvae = new NoViableAltException(dfa.Description, 59, stateNumber, (IIntStream) input);
      dfa.Error(nvae);
      throw nvae;
    }

    protected internal int DFA60_SpecialStateTransition(DFA dfa, int s, IIntStream _input)
    {
      ITreeNodeStream input = (ITreeNodeStream) _input;
      int stateNumber = s;
      switch (s)
      {
        case 0:
          input.LA(1);
          int index1 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index1);
          if (s >= 0)
            return s;
          break;
        case 1:
          input.LA(1);
          int index2 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index2);
          if (s >= 0)
            return s;
          break;
        case 2:
          input.LA(1);
          int index3 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index3);
          if (s >= 0)
            return s;
          break;
        case 3:
          input.LA(1);
          int index4 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index4);
          if (s >= 0)
            return s;
          break;
        case 4:
          input.LA(1);
          int index5 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index5);
          if (s >= 0)
            return s;
          break;
        case 5:
          input.LA(1);
          int index6 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index6);
          if (s >= 0)
            return s;
          break;
        case 6:
          input.LA(1);
          int index7 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index7);
          if (s >= 0)
            return s;
          break;
        case 7:
          input.LA(1);
          int index8 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index8);
          if (s >= 0)
            return s;
          break;
        case 8:
          input.LA(1);
          int index9 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index9);
          if (s >= 0)
            return s;
          break;
        case 9:
          input.LA(1);
          int index10 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index10);
          if (s >= 0)
            return s;
          break;
        case 10:
          input.LA(1);
          int index11 = input.Index();
          input.Rewind();
          s = -1;
          s = !this.synpred4_SqlGenerator() ? 12 : 29;
          input.Seek(index11);
          if (s >= 0)
            return s;
          break;
      }
      if (this.state.backtracking > 0)
      {
        this.state.failed = true;
        return -1;
      }
      NoViableAltException nvae = new NoViableAltException(dfa.Description, 60, stateNumber, (IIntStream) input);
      dfa.Error(nvae);
      throw nvae;
    }

    public SqlGenerator(ISessionFactoryImplementor sfi, ITreeNodeStream input)
      : this(input)
    {
      this.parseErrorHandler = (IParseErrorHandler) new ErrorCounter();
      this.sessionFactory = sfi;
      this.writer = (SqlGenerator.ISqlWriter) new SqlGenerator.DefaultWriter(this);
    }

    public IParseErrorHandler ParseErrorHandler => this.parseErrorHandler;

    public override void ReportError(RecognitionException e)
    {
      this.parseErrorHandler.ReportError(e);
    }

    public void ReportError(string s) => this.parseErrorHandler.ReportError(s);

    public void ReportWarning(string s) => this.parseErrorHandler.ReportWarning(s);

    public SqlString GetSQL() => this.sqlStringBuilder.ToSqlString();

    public IList<IParameterSpecification> GetCollectedParameters()
    {
      return (IList<IParameterSpecification>) this.collectedParameters;
    }

    private void Out(string s) => this.writer.Clause(s);

    private void Out(SqlString s) => this.writer.Clause(s);

    private void OptionalSpace()
    {
      int count = this.sqlStringBuilder.Count;
      if (count == 0)
        return;
      string str = this.sqlStringBuilder[count - 1].ToString();
      if (str.Length == 0)
        return;
      switch (str[str.Length - 1])
      {
        case ' ':
          break;
        case '(':
          break;
        case ')':
          break;
        default:
          this.Out(" ");
          break;
      }
    }

    private void Out(IASTNode n)
    {
      switch (n)
      {
        case ParameterNode parameterNode:
          Parameter placeholder = Parameter.Placeholder;
          placeholder.BackTrack = (object) parameterNode.HqlParameterSpecification.GetIdsForBackTrack((IMapping) this.sessionFactory).Single<string>();
          this.writer.PushParameter(placeholder);
          break;
        case SqlNode _:
          this.Out(((SqlNode) n).RenderText(this.sessionFactory));
          break;
        default:
          this.Out(n.Text);
          break;
      }
      if (parameterNode != null)
      {
        this.collectedParameters.Add(parameterNode.HqlParameterSpecification);
      }
      else
      {
        if (!(n is IParameterContainer))
          return;
        IParameterContainer parameterContainer = (IParameterContainer) n;
        if (!parameterContainer.HasEmbeddedParameters)
          return;
        this.collectedParameters.AddRange((IEnumerable<IParameterSpecification>) parameterContainer.GetEmbeddedParameters());
      }
    }

    private void Separator(IASTNode n, string sep)
    {
      if (n.NextSibling == null)
        return;
      this.Out(sep);
    }

    private static bool HasText(IASTNode a) => !string.IsNullOrEmpty(a.Text);

    protected virtual void FromFragmentSeparator(IASTNode a)
    {
      IASTNode nextSibling = a.NextSibling;
      if (nextSibling == null || !SqlGenerator.HasText(a))
        return;
      FromElement fromElement = (FromElement) a;
      FromElement a1 = (FromElement) nextSibling;
      while (a1 != null && !SqlGenerator.HasText((IASTNode) a1))
        a1 = (FromElement) a1.NextSibling;
      if (a1 == null || !SqlGenerator.HasText((IASTNode) a1))
        return;
      if (a1.RealOrigin == fromElement || a1.RealOrigin != null && a1.RealOrigin == fromElement.RealOrigin)
      {
        if (a1.JoinSequence != null && a1.JoinSequence.IsThetaStyle)
          this.Out(", ");
        else
          this.Out(" ");
      }
      else
        this.Out(", ");
    }

    protected virtual void NestedFromFragment(IASTNode d, IASTNode parent)
    {
      if (d == null || !SqlGenerator.HasText(d))
        return;
      if (parent != null && SqlGenerator.HasText(parent))
      {
        FromElement fromElement1 = (FromElement) parent;
        FromElement fromElement2 = (FromElement) d;
        if (fromElement2.RealOrigin == fromElement1)
        {
          if (fromElement2.JoinSequence != null && fromElement2.JoinSequence.IsThetaStyle)
            this.Out(", ");
          else
            this.Out(" ");
        }
        else
          this.Out(", ");
      }
      this.Out(d);
    }

    private SqlStringBuilder GetStringBuilder() => this.sqlStringBuilder;

    private void BeginFunctionTemplate(IASTNode m, IASTNode i)
    {
      if (((MethodNode) m).SQLFunction == null)
      {
        this.Out(i);
        this.Out("(");
      }
      else
      {
        this.outputStack.Insert(0, this.writer);
        this.writer = (SqlGenerator.ISqlWriter) new SqlGenerator.FunctionArguments();
      }
    }

    private void EndFunctionTemplate(IASTNode m)
    {
      ISQLFunction sqlFunction = ((MethodNode) m).SQLFunction;
      if (sqlFunction == null)
      {
        this.Out(")");
      }
      else
      {
        SqlGenerator.FunctionArguments writer = (SqlGenerator.FunctionArguments) this.writer;
        this.writer = this.outputStack[0];
        this.outputStack.RemoveAt(0);
        this.Out(sqlFunction.Render(writer.Args, this.sessionFactory));
      }
    }

    private void CommaBetweenParameters(string comma) => this.writer.CommaBetweenParameters(comma);

    private void StartQuery()
    {
      this.outputStack.Insert(0, this.writer);
      this.writer = (SqlGenerator.ISqlWriter) new SqlGenerator.QueryWriter();
    }

    private void EndQuery()
    {
      SqlString withLimitsIfNeeded = this.GetSqlStringWithLimitsIfNeeded((SqlGenerator.QueryWriter) this.writer);
      this.writer = this.outputStack[0];
      this.outputStack.RemoveAt(0);
      this.Out(withLimitsIfNeeded);
    }

    private SqlString GetSqlStringWithLimitsIfNeeded(SqlGenerator.QueryWriter queryWriter)
    {
      Parameter offsetParameter = (Parameter) null;
      Parameter limitParameter = (Parameter) null;
      if (queryWriter.SkipParameter != null)
      {
        queryWriter.SkipParameter.ExpectedType = (IType) NHibernateUtil.Int32;
        queryWriter.SkipParameter.IsSkipParameter();
        offsetParameter = Parameter.Placeholder;
        offsetParameter.BackTrack = (object) queryWriter.SkipParameter.GetIdsForBackTrack((IMapping) this.sessionFactory).First<string>();
      }
      if (queryWriter.TakeParameter != null)
      {
        queryWriter.TakeParameter.ExpectedType = (IType) NHibernateUtil.Int32;
        queryWriter.TakeParameter.IsTakeParameterWithSkipParameter(queryWriter.SkipParameter);
        limitParameter = Parameter.Placeholder;
        limitParameter.BackTrack = (object) queryWriter.TakeParameter.GetIdsForBackTrack((IMapping) this.sessionFactory).First<string>();
      }
      NHibernate.Dialect.Dialect dialect = this.sessionFactory.Dialect;
      return dialect.GetLimitString(queryWriter.ToSqlString(), queryWriter.Skip.HasValue ? new int?(dialect.GetOffsetValue(queryWriter.Skip.Value)) : new int?(), queryWriter.Take.HasValue ? new int?(dialect.GetLimitValue(queryWriter.Skip ?? 0, queryWriter.Take.Value)) : new int?(), offsetParameter, limitParameter);
    }

    private void Skip(IASTNode node)
    {
      SqlGenerator.QueryWriter writer = (SqlGenerator.QueryWriter) this.writer;
      if (node is ParameterNode parameterNode)
      {
        writer.SkipParameter = (IPageableParameterSpecification) parameterNode.HqlParameterSpecification;
        this.collectedParameters.Add(parameterNode.HqlParameterSpecification);
      }
      else
        writer.Skip = new int?(Convert.ToInt32(node.Text));
    }

    private void Take(IASTNode node)
    {
      SqlGenerator.QueryWriter writer = (SqlGenerator.QueryWriter) this.writer;
      if (node is ParameterNode parameterNode)
      {
        writer.TakeParameter = (IPageableParameterSpecification) parameterNode.HqlParameterSpecification;
        this.collectedParameters.Add(parameterNode.HqlParameterSpecification);
      }
      else
        writer.Take = new int?(Convert.ToInt32(node.Text));
    }

    public class orderDirection_return : TreeRuleReturnScope
    {
    }

    public class selectExpr_return : TreeRuleReturnScope
    {
    }

    public class selectAtom_return : TreeRuleReturnScope
    {
    }

    public class expr_return : TreeRuleReturnScope
    {
    }

    public class simpleExpr_return : TreeRuleReturnScope
    {
    }

    public class constant_return : TreeRuleReturnScope
    {
    }

    public class limitValue_return : TreeRuleReturnScope
    {
    }

    protected class DFA59 : DFA
    {
      public DFA59(BaseRecognizer recognizer)
      {
        this.recognizer = recognizer;
        this.decisionNumber = 59;
        this.eot = SqlGenerator.DFA59_eot;
        this.eof = SqlGenerator.DFA59_eof;
        this.min = SqlGenerator.DFA59_min;
        this.max = SqlGenerator.DFA59_max;
        this.accept = SqlGenerator.DFA59_accept;
        this.special = SqlGenerator.DFA59_special;
        this.transition = SqlGenerator.DFA59_transition;
      }

      public override string Description
      {
        get
        {
          return "319:1: nestedExpr : ( ( additiveExpr )=> additiveExpr | ( bitwiseExpr )=> bitwiseExpr | expr );";
        }
      }
    }

    protected class DFA60 : DFA
    {
      public DFA60(BaseRecognizer recognizer)
      {
        this.recognizer = recognizer;
        this.decisionNumber = 60;
        this.eot = SqlGenerator.DFA60_eot;
        this.eof = SqlGenerator.DFA60_eof;
        this.min = SqlGenerator.DFA60_min;
        this.max = SqlGenerator.DFA60_max;
        this.accept = SqlGenerator.DFA60_accept;
        this.special = SqlGenerator.DFA60_special;
        this.transition = SqlGenerator.DFA60_transition;
      }

      public override string Description
      {
        get => "326:1: nestedExprAfterMinusDiv : ( ( arithmeticExpr )=> arithmeticExpr | expr );";
      }
    }

    private interface ISqlWriter
    {
      void Clause(string clause);

      void Clause(SqlString clause);

      void PushParameter(Parameter parameter);

      void CommaBetweenParameters(string comma);
    }

    private class DefaultWriter : SqlGenerator.ISqlWriter
    {
      private readonly SqlGenerator generator;

      internal DefaultWriter(SqlGenerator generator) => this.generator = generator;

      public void Clause(string clause) => this.generator.GetStringBuilder().Add(clause);

      public void Clause(SqlString clause) => this.generator.GetStringBuilder().Add(clause);

      public void PushParameter(Parameter parameter)
      {
        this.generator.GetStringBuilder().Add(parameter);
      }

      public void CommaBetweenParameters(string comma)
      {
        this.generator.GetStringBuilder().Add(comma);
      }
    }

    private class QueryWriter : SqlGenerator.ISqlWriter
    {
      private readonly SqlStringBuilder builder = new SqlStringBuilder();

      public IPageableParameterSpecification TakeParameter { get; set; }

      public IPageableParameterSpecification SkipParameter { get; set; }

      public int? Skip { get; set; }

      public int? Take { get; set; }

      public void Clause(string clause) => this.builder.Add(clause);

      public void Clause(SqlString clause) => this.builder.Add(clause);

      public void PushParameter(Parameter parameter) => this.builder.Add(parameter);

      public void CommaBetweenParameters(string comma) => this.builder.Add(comma);

      public SqlString ToSqlString() => this.builder.ToSqlString();
    }

    private class FunctionArguments : SqlGenerator.ISqlWriter
    {
      private readonly List<SqlString> args = new List<SqlString>();
      private int argInd;

      public IList Args => (IList) this.args;

      public void Clause(string clause) => this.Clause(SqlString.Parse(clause));

      public void Clause(SqlString clause)
      {
        if (this.argInd == this.args.Count)
          this.args.Add(clause);
        else
          this.args[this.argInd] = this.args[this.argInd].Append(clause);
      }

      public void PushParameter(Parameter parameter)
      {
        if (this.argInd == this.args.Count)
          this.args.Add(new SqlString(parameter));
        else
          this.args[this.argInd] = this.args[this.argInd].Append(new SqlString(parameter));
      }

      public void CommaBetweenParameters(string comma) => ++this.argInd;
    }
  }
}
