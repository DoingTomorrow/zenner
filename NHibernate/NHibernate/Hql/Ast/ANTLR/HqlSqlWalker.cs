// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.HqlSqlWalker
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Id;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.UserTypes;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  public class HqlSqlWalker : TreeParser
  {
    public const int SELECT_COLUMNS = 144;
    public const int LT = 109;
    public const int EXPONENT = 130;
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
    public const int ESCqs = 128;
    public const int WEIRD_IDENT = 93;
    public const int OPEN_BRACKET = 122;
    public const int FULL = 23;
    public const int ORDER_ELEMENT = 85;
    public const int IS_NULL = 80;
    public const int ESCAPE = 18;
    public const int INSERT = 29;
    public const int FROM_FRAGMENT = 135;
    public const int NAMED_PARAM = 149;
    public const int BOTH = 64;
    public const int SELECT_CLAUSE = 138;
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
    public const int SQL_TOKEN = 143;
    public const int IS = 31;
    public const int AVG = 9;
    public const int LEFT = 33;
    public const int SOME = 48;
    public const int ALL = 4;
    public const int BOR = 115;
    public const int IMPLIED_FROM = 136;
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
    public const int LEFT_OUTER = 139;
    public const int ROW_STAR = 88;
    public const int NOT_LIKE = 84;
    public const int RIGHT_OUTER = 140;
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
    public const int ALIAS_REF = 141;
    public const int DIV = 121;
    public const int DESCENDING = 14;
    public const int AGGREGATE = 71;
    public const int BETWEEN = 10;
    public const int LE = 111;
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
    protected ITreeAdaptor adaptor = (ITreeAdaptor) new CommonTreeAdaptor();
    public static readonly BitSet FOLLOW_selectStatement_in_statement168 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_updateStatement_in_statement172 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_deleteStatement_in_statement176 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_insertStatement_in_statement180 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_query_in_selectStatement191 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_UPDATE_in_updateStatement215 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_VERSIONED_in_updateStatement222 = new BitSet(new ulong[1]
    {
      4194304UL
    });
    public static readonly BitSet FOLLOW_fromClause_in_updateStatement228 = new BitSet(new ulong[1]
    {
      70368744177664UL
    });
    public static readonly BitSet FOLLOW_setClause_in_updateStatement232 = new BitSet(new ulong[1]
    {
      36028797018963976UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_updateStatement237 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_DELETE_in_deleteStatement280 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_fromClause_in_deleteStatement284 = new BitSet(new ulong[1]
    {
      36028797018963976UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_deleteStatement287 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_INSERT_in_insertStatement317 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_intoClause_in_insertStatement321 = new BitSet(new ulong[2]
    {
      4503599627370496UL,
      4194304UL
    });
    public static readonly BitSet FOLLOW_query_in_insertStatement323 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_INTO_in_intoClause347 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_path_in_intoClause354 = new BitSet(new ulong[2]
    {
      0UL,
      8388608UL
    });
    public static readonly BitSet FOLLOW_insertablePropertySpec_in_intoClause359 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_RANGE_in_insertablePropertySpec375 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_IDENT_in_insertablePropertySpec378 = new BitSet(new ulong[2]
    {
      8UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_SET_in_setClause395 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_assignment_in_setClause400 = new BitSet(new ulong[2]
    {
      8UL,
      274877906944UL
    });
    public static readonly BitSet FOLLOW_EQ_in_assignment427 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_propertyRef_in_assignment432 = new BitSet(new ulong[2]
    {
      150871137273810944UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_newValue_in_assignment438 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_expr_in_newValue454 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_query_in_newValue458 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_unionedQuery_in_query469 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_UNION_in_query476 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_unionedQuery_in_query478 = new BitSet(new ulong[2]
    {
      4503599627370496UL,
      4194304UL
    });
    public static readonly BitSet FOLLOW_query_in_query480 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_QUERY_in_unionedQuery503 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_SELECT_FROM_in_unionedQuery515 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_fromClause_in_unionedQuery523 = new BitSet(new ulong[1]
    {
      35184372088840UL
    });
    public static readonly BitSet FOLLOW_selectClause_in_unionedQuery532 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_unionedQuery547 = new BitSet(new ulong[1]
    {
      1268836468785160UL
    });
    public static readonly BitSet FOLLOW_groupClause_in_unionedQuery557 = new BitSet(new ulong[1]
    {
      1268836452007944UL
    });
    public static readonly BitSet FOLLOW_havingClause_in_unionedQuery567 = new BitSet(new ulong[1]
    {
      1268836418453512UL
    });
    public static readonly BitSet FOLLOW_orderClause_in_unionedQuery577 = new BitSet(new ulong[1]
    {
      1266637395197960UL
    });
    public static readonly BitSet FOLLOW_skipClause_in_unionedQuery587 = new BitSet(new ulong[1]
    {
      1125899906842632UL
    });
    public static readonly BitSet FOLLOW_takeClause_in_unionedQuery597 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ORDER_in_orderClause654 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_orderExprs_in_orderClause659 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_query_in_orderClause663 = new BitSet(new ulong[1]
    {
      16648UL
    });
    public static readonly BitSet FOLLOW_set_in_orderClause665 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_expr_in_orderExprs686 = new BitSet(new ulong[2]
    {
      146367537646457090UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_set_in_orderExprs688 = new BitSet(new ulong[2]
    {
      146367537646440450UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_orderExprs_in_orderExprs700 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SKIP_in_skipClause714 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_skipClause717 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_parameter_in_skipClause721 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_TAKE_in_takeClause735 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_takeClause738 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_parameter_in_takeClause742 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_GROUP_in_groupClause756 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_groupClause761 = new BitSet(new ulong[2]
    {
      146367537646440456UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_HAVING_in_havingClause777 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_havingClause779 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_SELECT_in_selectClause793 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_DISTINCT_in_selectClause800 = new BitSet(new ulong[2]
    {
      150871137408159888UL,
      3745875723303405200UL
    });
    public static readonly BitSet FOLLOW_selectExprList_in_selectClause806 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_selectExpr_in_selectExprList841 = new BitSet(new ulong[2]
    {
      150871137408159890UL,
      3745875723303405200UL
    });
    public static readonly BitSet FOLLOW_aliasedSelectExpr_in_selectExprList845 = new BitSet(new ulong[2]
    {
      150871137408159890UL,
      3745875723303405200UL
    });
    public static readonly BitSet FOLLOW_AS_in_aliasedSelectExpr869 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_selectExpr_in_aliasedSelectExpr873 = new BitSet(new ulong[2]
    {
      32768UL,
      2305843009750581248UL
    });
    public static readonly BitSet FOLLOW_identifier_in_aliasedSelectExpr877 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_propertyRef_in_selectExpr892 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ALL_in_selectExpr904 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_aliasRef_in_selectExpr908 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_OBJECT_in_selectExpr920 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_aliasRef_in_selectExpr924 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_constructor_in_selectExpr935 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_functionCall_in_selectExpr946 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_parameter_in_selectExpr951 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_count_in_selectExpr956 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_collectionFunction_in_selectExpr961 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_literal_in_selectExpr969 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_arithmeticExpr_in_selectExpr974 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_query_in_selectExpr979 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_COUNT_in_count991 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_set_in_count993 = new BitSet(new ulong[2]
    {
      146367537780789248UL,
      3745875723315987584UL
    });
    public static readonly BitSet FOLLOW_aggregateExpr_in_count1006 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ROW_STAR_in_count1010 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_CONSTRUCTOR_in_constructor1026 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_path_in_constructor1028 = new BitSet(new ulong[2]
    {
      150871137408159896UL,
      3745875723303405200UL
    });
    public static readonly BitSet FOLLOW_selectExpr_in_constructor1032 = new BitSet(new ulong[2]
    {
      150871137408159896UL,
      3745875723303405200UL
    });
    public static readonly BitSet FOLLOW_aliasedSelectExpr_in_constructor1036 = new BitSet(new ulong[2]
    {
      150871137408159896UL,
      3745875723303405200UL
    });
    public static readonly BitSet FOLLOW_expr_in_aggregateExpr1052 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_collectionFunction_in_aggregateExpr1058 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FROM_in_fromClause1078 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_fromElementList_in_fromClause1082 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_fromElement_in_fromElementList1100 = new BitSet(new ulong[2]
    {
      4294967298UL,
      8392704UL
    });
    public static readonly BitSet FOLLOW_RANGE_in_fromElement1125 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_path_in_fromElement1129 = new BitSet(new ulong[2]
    {
      2097160UL,
      256UL
    });
    public static readonly BitSet FOLLOW_ALIAS_in_fromElement1134 = new BitSet(new ulong[1]
    {
      2097160UL
    });
    public static readonly BitSet FOLLOW_FETCH_in_fromElement1141 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_joinElement_in_fromElement1168 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FILTER_ENTITY_in_fromElement1183 = new BitSet(new ulong[2]
    {
      0UL,
      256UL
    });
    public static readonly BitSet FOLLOW_ALIAS_in_fromElement1187 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_JOIN_in_joinElement1216 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_joinType_in_joinElement1221 = new BitSet(new ulong[2]
    {
      2129920UL,
      2305843009750581248UL
    });
    public static readonly BitSet FOLLOW_FETCH_in_joinElement1231 = new BitSet(new ulong[2]
    {
      32768UL,
      2305843009750581248UL
    });
    public static readonly BitSet FOLLOW_propertyRef_in_joinElement1237 = new BitSet(new ulong[2]
    {
      9223372036856872968UL,
      256UL
    });
    public static readonly BitSet FOLLOW_ALIAS_in_joinElement1242 = new BitSet(new ulong[1]
    {
      9223372036856872968UL
    });
    public static readonly BitSet FOLLOW_FETCH_in_joinElement1249 = new BitSet(new ulong[1]
    {
      9223372036854775816UL
    });
    public static readonly BitSet FOLLOW_WITH_in_joinElement1258 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_LEFT_in_joinType1299 = new BitSet(new ulong[1]
    {
      4398046511106UL
    });
    public static readonly BitSet FOLLOW_RIGHT_in_joinType1305 = new BitSet(new ulong[1]
    {
      4398046511106UL
    });
    public static readonly BitSet FOLLOW_OUTER_in_joinType1311 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FULL_in_joinType1325 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_INNER_in_joinType1332 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_identifier_in_path1354 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_DOT_in_path1362 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_path_in_path1366 = new BitSet(new ulong[2]
    {
      32768UL,
      2305843009750581248UL
    });
    public static readonly BitSet FOLLOW_identifier_in_path1370 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_path_in_pathAsIdent1389 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_WITH_in_withClause1430 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_withClause1436 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_WHERE_in_whereClause1464 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_whereClause1470 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_AND_in_logicalExpr1496 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_logicalExpr1498 = new BitSet(new ulong[2]
    {
      1391637070912UL,
      2306379846304907392UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_logicalExpr1500 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_OR_in_logicalExpr1507 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_logicalExpr1509 = new BitSet(new ulong[2]
    {
      1391637070912UL,
      2306379846304907392UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_logicalExpr1511 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_in_logicalExpr1518 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_logicalExpr1520 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_comparisonExpr_in_logicalExpr1526 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_functionCall_in_logicalExpr1531 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_logicalPath_in_logicalExpr1536 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_addrExpr_in_logicalPath1555 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_EQ_in_comparisonExpr1593 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1595 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1597 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NE_in_comparisonExpr1604 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1606 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1608 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_LT_in_comparisonExpr1615 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1617 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1619 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_GT_in_comparisonExpr1626 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1628 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1630 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_LE_in_comparisonExpr1637 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1639 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1641 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_GE_in_comparisonExpr1648 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1650 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1652 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_LIKE_in_comparisonExpr1659 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1661 = new BitSet(new ulong[2]
    {
      146367537646440448UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_expr_in_comparisonExpr1663 = new BitSet(new ulong[1]
    {
      262152UL
    });
    public static readonly BitSet FOLLOW_ESCAPE_in_comparisonExpr1668 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_comparisonExpr1670 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_LIKE_in_comparisonExpr1682 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1684 = new BitSet(new ulong[2]
    {
      146367537646440448UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_expr_in_comparisonExpr1686 = new BitSet(new ulong[1]
    {
      262152UL
    });
    public static readonly BitSet FOLLOW_ESCAPE_in_comparisonExpr1691 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_comparisonExpr1693 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BETWEEN_in_comparisonExpr1705 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1707 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1709 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1711 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_BETWEEN_in_comparisonExpr1718 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1720 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1722 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1724 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IN_in_comparisonExpr1731 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1733 = new BitSet(new ulong[2]
    {
      0UL,
      8192UL
    });
    public static readonly BitSet FOLLOW_inRhs_in_comparisonExpr1735 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NOT_IN_in_comparisonExpr1743 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1745 = new BitSet(new ulong[2]
    {
      0UL,
      8192UL
    });
    public static readonly BitSet FOLLOW_inRhs_in_comparisonExpr1747 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IS_NULL_in_comparisonExpr1755 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1757 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IS_NOT_NULL_in_comparisonExpr1764 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_comparisonExpr1766 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_EXISTS_in_comparisonExpr1775 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_comparisonExpr1779 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_collectionFunctionOrSubselect_in_comparisonExpr1783 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_IN_LIST_in_inRhs1807 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_collectionFunctionOrSubselect_in_inRhs1811 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_expr_in_inRhs1815 = new BitSet(new ulong[2]
    {
      146367537646440456UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_expr_in_exprOrSubquery1831 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_query_in_exprOrSubquery1836 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ANY_in_exprOrSubquery1842 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_collectionFunctionOrSubselect_in_exprOrSubquery1844 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ALL_in_exprOrSubquery1851 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_collectionFunctionOrSubselect_in_exprOrSubquery1853 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_SOME_in_exprOrSubquery1860 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_collectionFunctionOrSubselect_in_exprOrSubquery1862 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_collectionFunction_in_collectionFunctionOrSubselect1875 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_query_in_collectionFunctionOrSubselect1880 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_addrExpr_in_expr1894 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_VECTOR_EXPR_in_expr1906 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_expr1909 = new BitSet(new ulong[2]
    {
      146367537646440456UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_constant_in_expr1918 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_arithmeticExpr_in_expr1923 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_functionCall_in_expr1928 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_parameter_in_expr1940 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_count_in_expr1945 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_PLUS_in_arithmeticExpr1973 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr1975 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr1977 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_MINUS_in_arithmeticExpr1984 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr1986 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr1988 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_DIV_in_arithmeticExpr1995 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr1997 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr1999 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_STAR_in_arithmeticExpr2006 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2008 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2010 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BNOT_in_arithmeticExpr2017 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2019 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BAND_in_arithmeticExpr2026 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2028 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2030 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BOR_in_arithmeticExpr2037 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2039 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2041 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_BXOR_in_arithmeticExpr2048 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2050 = new BitSet(new ulong[2]
    {
      151152612250521648UL,
      3745875723303404672UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2052 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_UNARY_MINUS_in_arithmeticExpr2060 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_exprOrSubquery_in_arithmeticExpr2062 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_caseExpr_in_arithmeticExpr2070 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_CASE_in_caseExpr2082 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_WHEN_in_caseExpr2088 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_logicalExpr_in_caseExpr2090 = new BitSet(new ulong[2]
    {
      146367537646440448UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr2092 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ELSE_in_caseExpr2099 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr2101 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_CASE2_in_caseExpr2113 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr2117 = new BitSet(new ulong[1]
    {
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_WHEN_in_caseExpr2121 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr2123 = new BitSet(new ulong[2]
    {
      146367537646440448UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr2125 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ELSE_in_caseExpr2132 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_caseExpr2134 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_ELEMENTS_in_collectionFunction2156 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_propertyRef_in_collectionFunction2162 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_INDICES_in_collectionFunction2181 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_propertyRef_in_collectionFunction2187 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_METHOD_CALL_in_functionCall2212 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_pathAsIdent_in_functionCall2217 = new BitSet(new ulong[2]
    {
      8UL,
      2048UL
    });
    public static readonly BitSet FOLLOW_EXPR_LIST_in_functionCall2222 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_expr_in_functionCall2225 = new BitSet(new ulong[2]
    {
      150871154521314312UL,
      3746412559857599616UL
    });
    public static readonly BitSet FOLLOW_query_in_functionCall2229 = new BitSet(new ulong[2]
    {
      150871154521314312UL,
      3746412559857599616UL
    });
    public static readonly BitSet FOLLOW_comparisonExpr_in_functionCall2233 = new BitSet(new ulong[2]
    {
      150871154521314312UL,
      3746412559857599616UL
    });
    public static readonly BitSet FOLLOW_AGGREGATE_in_functionCall2252 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_aggregateExpr_in_functionCall2254 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_literal_in_constant2267 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_NULL_in_constant2272 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_TRUE_in_constant2279 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FALSE_in_constant2289 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_JAVA_CONSTANT_in_constant2296 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_numericLiteral_in_literal2307 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_stringLiteral_in_literal2312 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_numericLiteral0 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_QUOTED_String_in_stringLiteral2359 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_identifier2370 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_addrExprDot_in_addrExpr2389 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_addrExprIndex_in_addrExpr2396 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_addrExprIdent_in_addrExpr2403 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_DOT_in_addrExprDot2427 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_addrExprLhs_in_addrExprDot2431 = new BitSet(new ulong[2]
    {
      134383616UL,
      2305843009750581248UL
    });
    public static readonly BitSet FOLLOW_propertyName_in_addrExprDot2435 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_INDEX_OP_in_addrExprIndex2474 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_addrExprLhs_in_addrExprIndex2478 = new BitSet(new ulong[2]
    {
      146367537646440448UL,
      3745875723299210368UL
    });
    public static readonly BitSet FOLLOW_expr_in_addrExprIndex2482 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_identifier_in_addrExprIdent2514 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_addrExpr_in_addrExprLhs2542 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_identifier_in_propertyName2555 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_CLASS_in_propertyName2560 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ELEMENTS_in_propertyName2565 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_INDICES_in_propertyName2570 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_propertyRefPath_in_propertyRef2582 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_propertyRefIdent_in_propertyRef2587 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_DOT_in_propertyRefPath2607 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_propertyRefLhs_in_propertyRefPath2611 = new BitSet(new ulong[2]
    {
      134383616UL,
      2305843009750581248UL
    });
    public static readonly BitSet FOLLOW_propertyName_in_propertyRefPath2615 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_identifier_in_propertyRefIdent2652 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_propertyRef_in_propertyRefLhs2664 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_identifier_in_aliasRef2685 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_COLON_in_parameter2703 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_identifier_in_parameter2707 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_PARAM_in_parameter2728 = new BitSet(new ulong[1]
    {
      4UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_parameter2733 = new BitSet(new ulong[1]
    {
      8UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_numericInteger2766 = new BitSet(new ulong[1]
    {
      2UL
    });
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (HqlSqlWalker));
    private readonly string _collectionFilterRole;
    private readonly SessionFactoryHelperExtensions _sessionFactoryHelper;
    private readonly QueryTranslatorImpl _qti;
    private int _currentClauseType;
    private int _level;
    private bool _inSelect;
    private bool _inFrom;
    private bool _inFunctionCall;
    private bool _inCase;
    private int _statementType;
    private int _currentStatementType;
    private string _statementTypeName;
    private int _positionalParameterCount;
    private int _parameterCount;
    private readonly NullableDictionary<string, object> _namedParameters = new NullableDictionary<string, object>();
    private readonly List<IParameterSpecification> _parameters = new List<IParameterSpecification>();
    private FromClause _currentFromClause;
    private SelectClause _selectClause;
    private readonly AliasGenerator _aliasGenerator = new AliasGenerator();
    private readonly ASTPrinter _printer = new ASTPrinter();
    private readonly Set<string> _querySpaces = (Set<string>) new HashedSet<string>();
    private readonly LiteralProcessor _literalProcessor;
    private readonly IDictionary<string, string> _tokenReplacements;
    private JoinType _impliedJoinType;
    private IParseErrorHandler _parseErrorHandler = (IParseErrorHandler) new ErrorCounter();
    private IASTFactory _nodeFactory;
    private readonly List<AssignmentSpecification> assignmentSpecifications = new List<AssignmentSpecification>();
    private int numberOfParametersInSetClause;

    public HqlSqlWalker(ITreeNodeStream input)
      : this(input, new RecognizerSharedState())
    {
    }

    public HqlSqlWalker(ITreeNodeStream input, RecognizerSharedState state)
      : base(input, state)
    {
      this.InitializeCyclicDFAs();
    }

    public ITreeAdaptor TreeAdaptor
    {
      get => this.adaptor;
      set => this.adaptor = value;
    }

    public override string[] TokenNames => HqlSqlWalker.tokenNames;

    public override string GrammarFileName => "HqlSqlWalker.g";

    public HqlSqlWalker.statement_return statement()
    {
      HqlSqlWalker.statement_return statementReturn = new HqlSqlWalker.statement_return();
      statementReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
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
          case 52:
          case 86:
            num = 1;
            break;
          case 53:
            num = 2;
            break;
          default:
            throw new NoViableAltException("", 1, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_selectStatement_in_statement168);
            HqlSqlWalker.selectStatement_return selectStatementReturn = this.selectStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, selectStatementReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_updateStatement_in_statement172);
            HqlSqlWalker.updateStatement_return updateStatementReturn = this.updateStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, updateStatementReturn.Tree);
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode4 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_deleteStatement_in_statement176);
            HqlSqlWalker.deleteStatement_return deleteStatementReturn = this.deleteStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, deleteStatementReturn.Tree);
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_insertStatement_in_statement180);
            HqlSqlWalker.insertStatement_return insertStatementReturn = this.insertStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, insertStatementReturn.Tree);
            break;
        }
        statementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return statementReturn;
    }

    public HqlSqlWalker.selectStatement_return selectStatement()
    {
      HqlSqlWalker.selectStatement_return selectStatementReturn = new HqlSqlWalker.selectStatement_return();
      selectStatementReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_query_in_selectStatement191);
        HqlSqlWalker.query_return queryReturn = this.query();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, queryReturn.Tree);
        selectStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return selectStatementReturn;
    }

    public HqlSqlWalker.updateStatement_return updateStatement()
    {
      HqlSqlWalker.updateStatement_return updateStatementReturn = new HqlSqlWalker.updateStatement_return();
      updateStatementReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      HqlSqlWalker.whereClause_return whereClauseReturn = (HqlSqlWalker.whereClause_return) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token UPDATE");
      RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token VERSIONED");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule whereClause");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule setClause");
      RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule fromClause");
      try
      {
        IASTNode astNode3 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode4 = (IASTNode) this.Match((IIntStream) this.input, 53, HqlSqlWalker.FOLLOW_UPDATE_in_updateStatement215);
        rewriteRuleNodeStream1.Add((object) astNode4);
        this.BeforeStatement("update", 53);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num1 = 2;
        if (this.input.LA(1) == 54)
          num1 = 1;
        if (num1 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          astNode2 = (IASTNode) this.Match((IIntStream) this.input, 54, HqlSqlWalker.FOLLOW_VERSIONED_in_updateStatement222);
          rewriteRuleNodeStream2.Add((object) astNode2);
        }
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_fromClause_in_updateStatement228);
        HqlSqlWalker.fromClause_return fromClauseReturn = this.fromClause();
        --this.state.followingStackPointer;
        ruleSubtreeStream3.Add(fromClauseReturn.Tree);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_setClause_in_updateStatement232);
        HqlSqlWalker.setClause_return setClauseReturn = this.setClause();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(setClauseReturn.Tree);
        int num2 = 2;
        if (this.input.LA(1) == 55)
          num2 = 1;
        if (num2 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_whereClause_in_updateStatement237);
          whereClauseReturn = this.whereClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream1.Add(whereClauseReturn.Tree);
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode3;
        updateStatementReturn.Tree = (object) t;
        RewriteRuleNodeStream rewriteRuleNodeStream3 = new RewriteRuleNodeStream(this.adaptor, "token u", (object) astNode4);
        RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule f", fromClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule w", whereClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream6 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", updateStatementReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream7 = new RewriteRuleSubtreeStream(this.adaptor, "rule s", setClauseReturn?.Tree);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode5 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleNodeStream3.NextNode(), (object) nilNode3);
        this.adaptor.AddChild((object) astNode5, ruleSubtreeStream4.NextTree());
        this.adaptor.AddChild((object) astNode5, ruleSubtreeStream7.NextTree());
        if (ruleSubtreeStream5.HasNext())
          this.adaptor.AddChild((object) astNode5, ruleSubtreeStream5.NextTree());
        ruleSubtreeStream5.Reset();
        this.adaptor.AddChild((object) nilNode2, (object) astNode5);
        updateStatementReturn.Tree = (object) nilNode2;
        updateStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode2);
        this.BeforeStatementCompletion("update");
        this.PrepareVersioned((IASTNode) updateStatementReturn.Tree, astNode2);
        this.PostProcessUpdate((IASTNode) updateStatementReturn.Tree);
        this.AfterStatementCompletion("update");
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return updateStatementReturn;
    }

    public HqlSqlWalker.deleteStatement_return deleteStatement()
    {
      HqlSqlWalker.deleteStatement_return deleteStatementReturn = new HqlSqlWalker.deleteStatement_return();
      deleteStatementReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 13, HqlSqlWalker.FOLLOW_DELETE_in_deleteStatement280)), (object) nilNode2);
        this.BeforeStatement("delete", 13);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_fromClause_in_deleteStatement284);
        HqlSqlWalker.fromClause_return fromClauseReturn = this.fromClause();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, fromClauseReturn.Tree);
        int num = 2;
        if (this.input.LA(1) == 55)
          num = 1;
        if (num == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_whereClause_in_deleteStatement287);
          HqlSqlWalker.whereClause_return whereClauseReturn = this.whereClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode3, whereClauseReturn.Tree);
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        deleteStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.BeforeStatementCompletion("delete");
        this.PostProcessDelete((IASTNode) deleteStatementReturn.Tree);
        this.AfterStatementCompletion("delete");
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return deleteStatementReturn;
    }

    public HqlSqlWalker.insertStatement_return insertStatement()
    {
      HqlSqlWalker.insertStatement_return insertStatementReturn = new HqlSqlWalker.insertStatement_return();
      insertStatementReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 29, HqlSqlWalker.FOLLOW_INSERT_in_insertStatement317)), (object) nilNode2);
        this.BeforeStatement("insert", 29);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_intoClause_in_insertStatement321);
        HqlSqlWalker.intoClause_return intoClauseReturn = this.intoClause();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, intoClauseReturn.Tree);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_query_in_insertStatement323);
        HqlSqlWalker.query_return queryReturn = this.query();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, queryReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        insertStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.BeforeStatementCompletion("insert");
        this.PostProcessInsert((IASTNode) insertStatementReturn.Tree);
        this.AfterStatementCompletion("insert");
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return insertStatementReturn;
    }

    public HqlSqlWalker.intoClause_return intoClause()
    {
      HqlSqlWalker.intoClause_return intoClauseReturn = new HqlSqlWalker.intoClause_return();
      intoClauseReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 30, HqlSqlWalker.FOLLOW_INTO_in_intoClause347)), (object) nilNode2);
        this.HandleClauseStart(30);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_path_in_intoClause354);
        HqlSqlWalker.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, pathReturn.Tree);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_insertablePropertySpec_in_intoClause359);
        HqlSqlWalker.insertablePropertySpec_return propertySpecReturn = this.insertablePropertySpec();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, propertySpecReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        intoClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        intoClauseReturn.Tree = (object) this.CreateIntoClause(pathReturn?.p, propertySpecReturn != null ? (IASTNode) propertySpecReturn.Tree : (IASTNode) null);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return intoClauseReturn;
    }

    public HqlSqlWalker.insertablePropertySpec_return insertablePropertySpec()
    {
      HqlSqlWalker.insertablePropertySpec_return propertySpecReturn = new HqlSqlWalker.insertablePropertySpec_return();
      propertySpecReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 87, HqlSqlWalker.FOLLOW_RANGE_in_insertablePropertySpec375)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num1 = 0;
        while (true)
        {
          int num2 = 2;
          if (this.input.LA(1) == 125)
            num2 = 1;
          if (num2 == 1)
          {
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode child = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 125, HqlSqlWalker.FOLLOW_IDENT_in_insertablePropertySpec378));
            this.adaptor.AddChild((object) astNode3, (object) child);
            ++num1;
          }
          else
            break;
        }
        if (num1 < 1)
          throw new EarlyExitException(5, (IIntStream) this.input);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        propertySpecReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return propertySpecReturn;
    }

    public HqlSqlWalker.setClause_return setClause()
    {
      HqlSqlWalker.setClause_return setClauseReturn = new HqlSqlWalker.setClause_return();
      setClauseReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 46, HqlSqlWalker.FOLLOW_SET_in_setClause395)), (object) nilNode2);
        this.HandleClauseStart(46);
        if (this.input.LA(1) == 2)
        {
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          while (true)
          {
            int num = 2;
            if (this.input.LA(1) == 102)
              num = 1;
            if (num == 1)
            {
              astNode1 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_assignment_in_setClause400);
              HqlSqlWalker.assignment_return assignmentReturn = this.assignment();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode3, assignmentReturn.Tree);
            }
            else
              break;
          }
          this.Match((IIntStream) this.input, 3, (BitSet) null);
        }
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        setClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return setClauseReturn;
    }

    public HqlSqlWalker.assignment_return assignment()
    {
      HqlSqlWalker.assignment_return assignmentReturn = new HqlSqlWalker.assignment_return();
      assignmentReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 102, HqlSqlWalker.FOLLOW_EQ_in_assignment427)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_propertyRef_in_assignment432);
        HqlSqlWalker.propertyRef_return propertyRefReturn = this.propertyRef();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, propertyRefReturn.Tree);
        this.Resolve(propertyRefReturn != null ? (IASTNode) propertyRefReturn.Tree : (IASTNode) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_newValue_in_assignment438);
        HqlSqlWalker.newValue_return newValueReturn = this.newValue();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, newValueReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        assignmentReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.EvaluateAssignment((IASTNode) assignmentReturn.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return assignmentReturn;
    }

    public HqlSqlWalker.newValue_return newValue()
    {
      HqlSqlWalker.newValue_return newValueReturn = new HqlSqlWalker.newValue_return();
      newValueReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 12 || num1 == 15 || num1 == 20 || num1 == 39 || num1 == 51 || num1 == 57 || num1 == 71 || num1 == 74 || num1 == 78 || num1 == 81 || num1 == 90 || num1 >= 92 && num1 <= 93 || num1 >= 95 && num1 <= 100 || num1 >= 105 && num1 <= 106 || num1 >= 114 && num1 <= 121 || num1 >= 124 && num1 <= 125)
        {
          num2 = 1;
        }
        else
        {
          if (num1 != 52 && num1 != 86)
            throw new NoViableAltException("", 7, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_newValue454);
            HqlSqlWalker.expr_return exprReturn = this.expr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, exprReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_query_in_newValue458);
            HqlSqlWalker.query_return queryReturn = this.query();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, queryReturn.Tree);
            break;
        }
        newValueReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return newValueReturn;
    }

    public HqlSqlWalker.query_return query()
    {
      HqlSqlWalker.query_return queryReturn1 = new HqlSqlWalker.query_return();
      queryReturn1.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 52:
            num = 2;
            break;
          case 86:
            num = 1;
            break;
          default:
            throw new NoViableAltException("", 8, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_unionedQuery_in_query469);
            HqlSqlWalker.unionedQuery_return unionedQueryReturn1 = this.unionedQuery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, unionedQueryReturn1.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 52, HqlSqlWalker.FOLLOW_UNION_in_query476)), (object) nilNode);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_unionedQuery_in_query478);
            HqlSqlWalker.unionedQuery_return unionedQueryReturn2 = this.unionedQuery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, unionedQueryReturn2.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_query_in_query480);
            HqlSqlWalker.query_return queryReturn2 = this.query();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, queryReturn2.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            break;
        }
        queryReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return queryReturn1;
    }

    public HqlSqlWalker.unionedQuery_return unionedQuery()
    {
      HqlSqlWalker.unionedQuery_return unionedQueryReturn = new HqlSqlWalker.unionedQuery_return();
      unionedQueryReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      HqlSqlWalker.selectClause_return selectClauseReturn = (HqlSqlWalker.selectClause_return) null;
      HqlSqlWalker.whereClause_return whereClauseReturn = (HqlSqlWalker.whereClause_return) null;
      HqlSqlWalker.groupClause_return groupClauseReturn = (HqlSqlWalker.groupClause_return) null;
      HqlSqlWalker.havingClause_return havingClauseReturn = (HqlSqlWalker.havingClause_return) null;
      HqlSqlWalker.orderClause_return orderClauseReturn = (HqlSqlWalker.orderClause_return) null;
      HqlSqlWalker.skipClause_return skipClauseReturn = (HqlSqlWalker.skipClause_return) null;
      HqlSqlWalker.takeClause_return takeClauseReturn = (HqlSqlWalker.takeClause_return) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token SELECT_FROM");
      RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token QUERY");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule whereClause");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule skipClause");
      RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule orderClause");
      RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule groupClause");
      RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule havingClause");
      RewriteRuleSubtreeStream ruleSubtreeStream6 = new RewriteRuleSubtreeStream(this.adaptor, "rule fromClause");
      RewriteRuleSubtreeStream ruleSubtreeStream7 = new RewriteRuleSubtreeStream(this.adaptor, "rule selectClause");
      RewriteRuleSubtreeStream ruleSubtreeStream8 = new RewriteRuleSubtreeStream(this.adaptor, "rule takeClause");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode el1 = (IASTNode) this.Match((IIntStream) this.input, 86, HqlSqlWalker.FOLLOW_QUERY_in_unionedQuery503);
        rewriteRuleNodeStream2.Add((object) el1);
        this.BeforeStatement("select", 45);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        IASTNode astNode3 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode el2 = (IASTNode) this.Match((IIntStream) this.input, 89, HqlSqlWalker.FOLLOW_SELECT_FROM_in_unionedQuery515);
        rewriteRuleNodeStream1.Add((object) el2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_fromClause_in_unionedQuery523);
        HqlSqlWalker.fromClause_return fromClauseReturn = this.fromClause();
        --this.state.followingStackPointer;
        ruleSubtreeStream6.Add(fromClauseReturn.Tree);
        int num1 = 2;
        if (this.input.LA(1) == 45)
          num1 = 1;
        if (num1 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_selectClause_in_unionedQuery532);
          selectClauseReturn = this.selectClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream7.Add(selectClauseReturn.Tree);
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) nilNode2);
        astNode1 = astNode3;
        int num2 = 2;
        if (this.input.LA(1) == 55)
          num2 = 1;
        if (num2 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_whereClause_in_unionedQuery547);
          whereClauseReturn = this.whereClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream1.Add(whereClauseReturn.Tree);
        }
        int num3 = 2;
        if (this.input.LA(1) == 24)
          num3 = 1;
        if (num3 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_groupClause_in_unionedQuery557);
          groupClauseReturn = this.groupClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream4.Add(groupClauseReturn.Tree);
        }
        int num4 = 2;
        if (this.input.LA(1) == 25)
          num4 = 1;
        if (num4 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_havingClause_in_unionedQuery567);
          havingClauseReturn = this.havingClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream5.Add(havingClauseReturn.Tree);
        }
        int num5 = 2;
        if (this.input.LA(1) == 41)
          num5 = 1;
        if (num5 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_orderClause_in_unionedQuery577);
          orderClauseReturn = this.orderClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream3.Add(orderClauseReturn.Tree);
        }
        int num6 = 2;
        if (this.input.LA(1) == 47)
          num6 = 1;
        if (num6 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_skipClause_in_unionedQuery587);
          skipClauseReturn = this.skipClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream2.Add(skipClauseReturn.Tree);
        }
        int num7 = 2;
        if (this.input.LA(1) == 50)
          num7 = 1;
        if (num7 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_takeClause_in_unionedQuery597);
          takeClauseReturn = this.takeClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream8.Add(takeClauseReturn.Tree);
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode2;
        unionedQueryReturn.Tree = (object) t;
        RewriteRuleSubtreeStream ruleSubtreeStream9 = new RewriteRuleSubtreeStream(this.adaptor, "rule f", fromClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream10 = new RewriteRuleSubtreeStream(this.adaptor, "rule w", whereClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream11 = new RewriteRuleSubtreeStream(this.adaptor, "rule g", groupClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream12 = new RewriteRuleSubtreeStream(this.adaptor, "rule sk", skipClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream13 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", unionedQueryReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream14 = new RewriteRuleSubtreeStream(this.adaptor, "rule s", selectClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream15 = new RewriteRuleSubtreeStream(this.adaptor, "rule o", orderClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream16 = new RewriteRuleSubtreeStream(this.adaptor, "rule tk", takeClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream17 = new RewriteRuleSubtreeStream(this.adaptor, "rule h", havingClauseReturn?.Tree);
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(45, "SELECT"), (object) nilNode4);
        if (ruleSubtreeStream14.HasNext())
          this.adaptor.AddChild((object) astNode4, ruleSubtreeStream14.NextTree());
        ruleSubtreeStream14.Reset();
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream9.NextTree());
        if (ruleSubtreeStream10.HasNext())
          this.adaptor.AddChild((object) astNode4, ruleSubtreeStream10.NextTree());
        ruleSubtreeStream10.Reset();
        if (ruleSubtreeStream11.HasNext())
          this.adaptor.AddChild((object) astNode4, ruleSubtreeStream11.NextTree());
        ruleSubtreeStream11.Reset();
        if (ruleSubtreeStream17.HasNext())
          this.adaptor.AddChild((object) astNode4, ruleSubtreeStream17.NextTree());
        ruleSubtreeStream17.Reset();
        if (ruleSubtreeStream15.HasNext())
          this.adaptor.AddChild((object) astNode4, ruleSubtreeStream15.NextTree());
        ruleSubtreeStream15.Reset();
        if (ruleSubtreeStream12.HasNext())
          this.adaptor.AddChild((object) astNode4, ruleSubtreeStream12.NextTree());
        ruleSubtreeStream12.Reset();
        if (ruleSubtreeStream16.HasNext())
          this.adaptor.AddChild((object) astNode4, ruleSubtreeStream16.NextTree());
        ruleSubtreeStream16.Reset();
        this.adaptor.AddChild((object) nilNode3, (object) astNode4);
        unionedQueryReturn.Tree = (object) nilNode3;
        unionedQueryReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode3);
        this.BeforeStatementCompletion("select");
        this.ProcessQuery(selectClauseReturn != null ? (IASTNode) selectClauseReturn.Tree : (IASTNode) null, (IASTNode) unionedQueryReturn.Tree);
        this.AfterStatementCompletion("select");
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return unionedQueryReturn;
    }

    public HqlSqlWalker.orderClause_return orderClause()
    {
      HqlSqlWalker.orderClause_return orderClauseReturn = new HqlSqlWalker.orderClause_return();
      orderClauseReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 41, HqlSqlWalker.FOLLOW_ORDER_in_orderClause654)), (object) nilNode2);
        this.HandleClauseStart(41);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 12 || num1 == 15 || num1 == 20 || num1 == 39 || num1 == 51 || num1 == 57 || num1 == 71 || num1 == 74 || num1 == 78 || num1 == 81 || num1 == 90 || num1 >= 92 && num1 <= 93 || num1 >= 95 && num1 <= 100 || num1 >= 105 && num1 <= 106 || num1 >= 114 && num1 <= 121 || num1 >= 124 && num1 <= 125)
        {
          num2 = 1;
        }
        else
        {
          if (num1 != 52 && num1 != 86)
            throw new NoViableAltException("", 17, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_orderExprs_in_orderClause659);
            HqlSqlWalker.orderExprs_return orderExprsReturn = this.orderExprs();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, orderExprsReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_query_in_orderClause663);
            HqlSqlWalker.query_return queryReturn = this.query();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, queryReturn.Tree);
            int num3 = 2;
            switch (this.input.LA(1))
            {
              case 8:
              case 14:
                num3 = 1;
                break;
            }
            if (num3 == 1)
            {
              astNode1 = (IASTNode) this.input.LT(1);
              IASTNode treeNode = (IASTNode) this.input.LT(1);
              if (this.input.LA(1) != 8 && this.input.LA(1) != 14)
                throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
              this.input.Consume();
              IASTNode child = (IASTNode) this.adaptor.DupNode((object) treeNode);
              this.adaptor.AddChild((object) astNode3, (object) child);
              this.state.errorRecovery = false;
              break;
            }
            break;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        orderClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return orderClauseReturn;
    }

    public HqlSqlWalker.orderExprs_return orderExprs()
    {
      HqlSqlWalker.orderExprs_return orderExprsReturn1 = new HqlSqlWalker.orderExprs_return();
      orderExprsReturn1.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_orderExprs686);
        HqlSqlWalker.expr_return exprReturn = this.expr();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, exprReturn.Tree);
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
          IASTNode astNode2 = (IASTNode) this.input.LT(1);
          IASTNode treeNode = (IASTNode) this.input.LT(1);
          if (this.input.LA(1) != 8 && this.input.LA(1) != 14)
            throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
          this.input.Consume();
          IASTNode child = (IASTNode) this.adaptor.DupNode((object) treeNode);
          this.adaptor.AddChild((object) nilNode, (object) child);
          this.state.errorRecovery = false;
        }
        int num2 = 2;
        int num3 = this.input.LA(1);
        if (num3 == 12 || num3 == 15 || num3 == 20 || num3 == 39 || num3 == 51 || num3 == 57 || num3 == 71 || num3 == 74 || num3 == 78 || num3 == 81 || num3 == 90 || num3 >= 92 && num3 <= 93 || num3 >= 95 && num3 <= 100 || num3 >= 105 && num3 <= 106 || num3 >= 114 && num3 <= 121 || num3 >= 124 && num3 <= 125)
          num2 = 1;
        if (num2 == 1)
        {
          IASTNode astNode3 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_orderExprs_in_orderExprs700);
          HqlSqlWalker.orderExprs_return orderExprsReturn2 = this.orderExprs();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, orderExprsReturn2.Tree);
        }
        orderExprsReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return orderExprsReturn1;
    }

    public HqlSqlWalker.skipClause_return skipClause()
    {
      HqlSqlWalker.skipClause_return skipClauseReturn = new HqlSqlWalker.skipClause_return();
      skipClauseReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 47, HqlSqlWalker.FOLLOW_SKIP_in_skipClause714)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num;
        switch (this.input.LA(1))
        {
          case 95:
            num = 1;
            break;
          case 105:
          case 106:
            num = 2;
            break;
          default:
            throw new NoViableAltException("", 20, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode child = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 95, HqlSqlWalker.FOLLOW_NUM_INT_in_skipClause717));
            this.adaptor.AddChild((object) astNode3, (object) child);
            break;
          case 2:
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_parameter_in_skipClause721);
            HqlSqlWalker.parameter_return parameterReturn = this.parameter();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, parameterReturn.Tree);
            break;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        skipClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return skipClauseReturn;
    }

    public HqlSqlWalker.takeClause_return takeClause()
    {
      HqlSqlWalker.takeClause_return clause = new HqlSqlWalker.takeClause_return();
      clause.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 50, HqlSqlWalker.FOLLOW_TAKE_in_takeClause735)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num;
        switch (this.input.LA(1))
        {
          case 95:
            num = 1;
            break;
          case 105:
          case 106:
            num = 2;
            break;
          default:
            throw new NoViableAltException("", 21, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode child = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 95, HqlSqlWalker.FOLLOW_NUM_INT_in_takeClause738));
            this.adaptor.AddChild((object) astNode3, (object) child);
            break;
          case 2:
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_parameter_in_takeClause742);
            HqlSqlWalker.parameter_return parameterReturn = this.parameter();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, parameterReturn.Tree);
            break;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        clause.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return clause;
    }

    public HqlSqlWalker.groupClause_return groupClause()
    {
      HqlSqlWalker.groupClause_return groupClauseReturn = new HqlSqlWalker.groupClause_return();
      groupClauseReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 24, HqlSqlWalker.FOLLOW_GROUP_in_groupClause756)), (object) nilNode2);
        this.HandleClauseStart(24);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num1 = 0;
        while (true)
        {
          int num2 = 2;
          int num3 = this.input.LA(1);
          if (num3 == 12 || num3 == 15 || num3 == 20 || num3 == 39 || num3 == 51 || num3 == 57 || num3 == 71 || num3 == 74 || num3 == 78 || num3 == 81 || num3 == 90 || num3 >= 92 && num3 <= 93 || num3 >= 95 && num3 <= 100 || num3 >= 105 && num3 <= 106 || num3 >= 114 && num3 <= 121 || num3 >= 124 && num3 <= 125)
            num2 = 1;
          if (num2 == 1)
          {
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_groupClause761);
            HqlSqlWalker.expr_return exprReturn = this.expr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, exprReturn.Tree);
            ++num1;
          }
          else
            break;
        }
        if (num1 < 1)
          throw new EarlyExitException(22, (IIntStream) this.input);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        groupClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return groupClauseReturn;
    }

    public HqlSqlWalker.havingClause_return havingClause()
    {
      HqlSqlWalker.havingClause_return havingClauseReturn = new HqlSqlWalker.havingClause_return();
      havingClauseReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 25, HqlSqlWalker.FOLLOW_HAVING_in_havingClause777)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_havingClause779);
        HqlSqlWalker.logicalExpr_return logicalExprReturn = this.logicalExpr();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, logicalExprReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        havingClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return havingClauseReturn;
    }

    public HqlSqlWalker.selectClause_return selectClause()
    {
      HqlSqlWalker.selectClause_return selectClauseReturn = new HqlSqlWalker.selectClause_return();
      selectClauseReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token SELECT");
      RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token DISTINCT");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule selectExprList");
      try
      {
        IASTNode astNode3 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode el = (IASTNode) this.Match((IIntStream) this.input, 45, HqlSqlWalker.FOLLOW_SELECT_in_selectClause793);
        rewriteRuleNodeStream1.Add((object) el);
        this.HandleClauseStart(45);
        this.BeforeSelectClause();
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num = 2;
        if (this.input.LA(1) == 16)
          num = 1;
        if (num == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          astNode2 = (IASTNode) this.Match((IIntStream) this.input, 16, HqlSqlWalker.FOLLOW_DISTINCT_in_selectClause800);
          rewriteRuleNodeStream2.Add((object) astNode2);
        }
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_selectExprList_in_selectClause806);
        HqlSqlWalker.selectExprList_return selectExprListReturn = this.selectExprList();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(selectExprListReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode3;
        selectClauseReturn.Tree = (object) t;
        RewriteRuleNodeStream rewriteRuleNodeStream3 = new RewriteRuleNodeStream(this.adaptor, "token d", (object) astNode2);
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", selectClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule x", selectExprListReturn?.Tree);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(138, "{select clause}"), (object) nilNode3);
        if (rewriteRuleNodeStream3.HasNext())
          this.adaptor.AddChild((object) astNode4, rewriteRuleNodeStream3.NextNode());
        rewriteRuleNodeStream3.Reset();
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream3.NextTree());
        this.adaptor.AddChild((object) nilNode2, (object) astNode4);
        selectClauseReturn.Tree = (object) nilNode2;
        selectClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode2);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return selectClauseReturn;
    }

    public HqlSqlWalker.selectExprList_return selectExprList()
    {
      HqlSqlWalker.selectExprList_return selectExprListReturn = new HqlSqlWalker.selectExprList_return();
      selectExprListReturn.Start = this.input.LT(1);
      bool inSelect = this._inSelect;
      this._inSelect = true;
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        int num1 = 0;
        while (true)
        {
          int num2 = 3;
          int num3 = this.input.LA(1);
          if (num3 == 4 || num3 == 12 || num3 == 15 || num3 == 17 || num3 == 27 || num3 == 52 || num3 == 57 || num3 == 68 || num3 == 71 || num3 >= 73 && num3 <= 74 || num3 == 81 || num3 == 86 || num3 == 90 || num3 == 93 || num3 >= 95 && num3 <= 99 || num3 >= 105 && num3 <= 106 || num3 >= 114 && num3 <= 121 || num3 >= 124 && num3 <= 125)
            num2 = 1;
          else if (num3 == 7)
            num2 = 2;
          switch (num2)
          {
            case 1:
              IASTNode astNode1 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_selectExpr_in_selectExprList841);
              HqlSqlWalker.selectExpr_return selectExprReturn1 = this.selectExpr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) nilNode, selectExprReturn1.Tree);
              break;
            case 2:
              IASTNode astNode2 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_aliasedSelectExpr_in_selectExprList845);
              HqlSqlWalker.aliasedSelectExpr_return selectExprReturn2 = this.aliasedSelectExpr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) nilNode, selectExprReturn2.Tree);
              break;
            default:
              goto label_9;
          }
          ++num1;
        }
label_9:
        if (num1 < 1)
          throw new EarlyExitException(24, (IIntStream) this.input);
        this._inSelect = inSelect;
        selectExprListReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return selectExprListReturn;
    }

    public HqlSqlWalker.aliasedSelectExpr_return aliasedSelectExpr()
    {
      HqlSqlWalker.aliasedSelectExpr_return selectExprReturn1 = new HqlSqlWalker.aliasedSelectExpr_return();
      selectExprReturn1.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 7, HqlSqlWalker.FOLLOW_AS_in_aliasedSelectExpr869)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_selectExpr_in_aliasedSelectExpr873);
        HqlSqlWalker.selectExpr_return selectExprReturn2 = this.selectExpr();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, selectExprReturn2.Tree);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_aliasedSelectExpr877);
        HqlSqlWalker.identifier_return identifierReturn = this.identifier();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, identifierReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        selectExprReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        HqlSqlWalker.SetAlias(selectExprReturn2 != null ? (IASTNode) selectExprReturn2.Tree : (IASTNode) null, identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null);
        selectExprReturn1.Tree = selectExprReturn2 != null ? (object) (IASTNode) selectExprReturn2.Tree : (object) (IASTNode) null;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return selectExprReturn1;
    }

    public HqlSqlWalker.selectExpr_return selectExpr()
    {
      HqlSqlWalker.selectExpr_return selectExprReturn = new HqlSqlWalker.selectExpr_return();
      selectExprReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 4:
            num = 2;
            break;
          case 12:
            num = 7;
            break;
          case 15:
          case 93:
          case 125:
            num = 1;
            break;
          case 17:
          case 27:
            num = 8;
            break;
          case 52:
          case 86:
            num = 11;
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
            num = 10;
            break;
          case 68:
            num = 3;
            break;
          case 71:
          case 81:
            num = 5;
            break;
          case 73:
            num = 4;
            break;
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 124:
            num = 9;
            break;
          case 105:
          case 106:
            num = 6;
            break;
          default:
            throw new NoViableAltException("", 25, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_propertyRef_in_selectExpr892);
            HqlSqlWalker.propertyRef_return propertyRefReturn = this.propertyRef();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, propertyRefReturn.Tree);
            HqlSqlWalker.ResolveSelectExpression(propertyRefReturn != null ? (IASTNode) propertyRefReturn.Tree : (IASTNode) null);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 4, HqlSqlWalker.FOLLOW_ALL_in_selectExpr904)), (object) nilNode1);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_aliasRef_in_selectExpr908);
            HqlSqlWalker.aliasRef_return aliasRefReturn1 = this.aliasRef();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, aliasRefReturn1.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            HqlSqlWalker.ResolveSelectExpression(aliasRefReturn1 != null ? (IASTNode) aliasRefReturn1.Tree : (IASTNode) null);
            selectExprReturn.Tree = aliasRefReturn1 != null ? (object) (IASTNode) aliasRefReturn1.Tree : (object) (IASTNode) null;
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode6 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 68, HqlSqlWalker.FOLLOW_OBJECT_in_selectExpr920)), (object) nilNode2);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_aliasRef_in_selectExpr924);
            HqlSqlWalker.aliasRef_return aliasRefReturn2 = this.aliasRef();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode6, aliasRefReturn2.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode6);
            astNode2 = astNode5;
            HqlSqlWalker.ResolveSelectExpression(aliasRefReturn2 != null ? (IASTNode) aliasRefReturn2.Tree : (IASTNode) null);
            selectExprReturn.Tree = aliasRefReturn2 != null ? (object) (IASTNode) aliasRefReturn2.Tree : (object) (IASTNode) null;
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_constructor_in_selectExpr935);
            HqlSqlWalker.constructor_return constructorReturn = this.constructor();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, constructorReturn.Tree);
            HqlSqlWalker.ProcessConstructor(constructorReturn != null ? (IASTNode) constructorReturn.Tree : (IASTNode) null);
            break;
          case 5:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_functionCall_in_selectExpr946);
            HqlSqlWalker.functionCall_return functionCallReturn = this.functionCall();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, functionCallReturn.Tree);
            break;
          case 6:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_parameter_in_selectExpr951);
            HqlSqlWalker.parameter_return parameterReturn = this.parameter();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, parameterReturn.Tree);
            break;
          case 7:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_count_in_selectExpr956);
            HqlSqlWalker.count_return countReturn = this.count();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, countReturn.Tree);
            break;
          case 8:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunction_in_selectExpr961);
            HqlSqlWalker.collectionFunction_return collectionFunctionReturn = this.collectionFunction();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, collectionFunctionReturn.Tree);
            break;
          case 9:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_literal_in_selectExpr969);
            HqlSqlWalker.literal_return literalReturn = this.literal();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, literalReturn.Tree);
            break;
          case 10:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_arithmeticExpr_in_selectExpr974);
            HqlSqlWalker.arithmeticExpr_return arithmeticExprReturn = this.arithmeticExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, arithmeticExprReturn.Tree);
            break;
          case 11:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_query_in_selectExpr979);
            HqlSqlWalker.query_return queryReturn = this.query();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, queryReturn.Tree);
            break;
        }
        selectExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return selectExprReturn;
    }

    public HqlSqlWalker.count_return count()
    {
      HqlSqlWalker.count_return countReturn = new HqlSqlWalker.count_return();
      countReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 12, HqlSqlWalker.FOLLOW_COUNT_in_count991)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
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
          astNode1 = (IASTNode) this.input.LT(1);
          IASTNode treeNode = (IASTNode) this.input.LT(1);
          if (this.input.LA(1) != 4 && this.input.LA(1) != 16)
            throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
          this.input.Consume();
          IASTNode child = (IASTNode) this.adaptor.DupNode((object) treeNode);
          this.adaptor.AddChild((object) astNode3, (object) child);
          this.state.errorRecovery = false;
        }
        int num2 = this.input.LA(1);
        int num3;
        if (num2 == 12 || num2 == 15 || num2 == 17 || num2 == 20 || num2 == 27 || num2 == 39 || num2 == 51 || num2 == 57 || num2 == 71 || num2 == 74 || num2 == 78 || num2 == 81 || num2 == 90 || num2 >= 92 && num2 <= 93 || num2 >= 95 && num2 <= 100 || num2 >= 105 && num2 <= 106 || num2 >= 114 && num2 <= 121 || num2 >= 124 && num2 <= 125)
        {
          num3 = 1;
        }
        else
        {
          if (num2 != 88)
            throw new NoViableAltException("", 27, 0, (IIntStream) this.input);
          num3 = 2;
        }
        switch (num3)
        {
          case 1:
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_aggregateExpr_in_count1006);
            HqlSqlWalker.aggregateExpr_return aggregateExprReturn = this.aggregateExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, aggregateExprReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode child1 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 88, HqlSqlWalker.FOLLOW_ROW_STAR_in_count1010));
            this.adaptor.AddChild((object) astNode3, (object) child1);
            break;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        countReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return countReturn;
    }

    public HqlSqlWalker.constructor_return constructor()
    {
      HqlSqlWalker.constructor_return constructorReturn = new HqlSqlWalker.constructor_return();
      constructorReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 73, HqlSqlWalker.FOLLOW_CONSTRUCTOR_in_constructor1026)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_path_in_constructor1028);
        HqlSqlWalker.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode3, pathReturn.Tree);
        while (true)
        {
          int num1 = 3;
          int num2 = this.input.LA(1);
          if (num2 == 4 || num2 == 12 || num2 == 15 || num2 == 17 || num2 == 27 || num2 == 52 || num2 == 57 || num2 == 68 || num2 == 71 || num2 >= 73 && num2 <= 74 || num2 == 81 || num2 == 86 || num2 == 90 || num2 == 93 || num2 >= 95 && num2 <= 99 || num2 >= 105 && num2 <= 106 || num2 >= 114 && num2 <= 121 || num2 >= 124 && num2 <= 125)
            num1 = 1;
          else if (num2 == 7)
            num1 = 2;
          switch (num1)
          {
            case 1:
              astNode1 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_selectExpr_in_constructor1032);
              HqlSqlWalker.selectExpr_return selectExprReturn1 = this.selectExpr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode3, selectExprReturn1.Tree);
              continue;
            case 2:
              astNode1 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_aliasedSelectExpr_in_constructor1036);
              HqlSqlWalker.aliasedSelectExpr_return selectExprReturn2 = this.aliasedSelectExpr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode3, selectExprReturn2.Tree);
              continue;
            default:
              goto label_9;
          }
        }
label_9:
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        constructorReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return constructorReturn;
    }

    public HqlSqlWalker.aggregateExpr_return aggregateExpr()
    {
      HqlSqlWalker.aggregateExpr_return aggregateExprReturn = new HqlSqlWalker.aggregateExpr_return();
      aggregateExprReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 12 || num1 == 15 || num1 == 20 || num1 == 39 || num1 == 51 || num1 == 57 || num1 == 71 || num1 == 74 || num1 == 78 || num1 == 81 || num1 == 90 || num1 >= 92 && num1 <= 93 || num1 >= 95 && num1 <= 100 || num1 >= 105 && num1 <= 106 || num1 >= 114 && num1 <= 121 || num1 >= 124 && num1 <= 125)
        {
          num2 = 1;
        }
        else
        {
          if (num1 != 17 && num1 != 27)
            throw new NoViableAltException("", 29, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_aggregateExpr1052);
            HqlSqlWalker.expr_return exprReturn = this.expr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, exprReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunction_in_aggregateExpr1058);
            HqlSqlWalker.collectionFunction_return collectionFunctionReturn = this.collectionFunction();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, collectionFunctionReturn.Tree);
            break;
        }
        aggregateExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return aggregateExprReturn;
    }

    public HqlSqlWalker.fromClause_return fromClause()
    {
      HqlSqlWalker.fromClause_return fromClauseReturn = new HqlSqlWalker.fromClause_return();
      fromClauseReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      this.PrepareFromClauseInputTree((IASTNode) this.input.LT(1), this.input);
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 22, HqlSqlWalker.FOLLOW_FROM_in_fromClause1078));
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) astNode3, (object) nilNode2);
        this.PushFromClause(astNode3);
        this.HandleClauseStart(22);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_fromElementList_in_fromClause1082);
        HqlSqlWalker.fromElementList_return elementListReturn = this.fromElementList();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode4, elementListReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode4);
        astNode1 = astNode2;
        fromClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return fromClauseReturn;
    }

    public HqlSqlWalker.fromElementList_return fromElementList()
    {
      HqlSqlWalker.fromElementList_return elementListReturn = new HqlSqlWalker.fromElementList_return();
      elementListReturn.Start = this.input.LT(1);
      bool inFrom = this._inFrom;
      this._inFrom = true;
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        int num1 = 0;
        while (true)
        {
          int num2 = 2;
          switch (this.input.LA(1))
          {
            case 32:
            case 76:
            case 87:
              num2 = 1;
              break;
          }
          if (num2 == 1)
          {
            IASTNode astNode = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_fromElement_in_fromElementList1100);
            HqlSqlWalker.fromElement_return fromElementReturn = this.fromElement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, fromElementReturn.Tree);
            ++num1;
          }
          else
            break;
        }
        if (num1 < 1)
          throw new EarlyExitException(30, (IIntStream) this.input);
        this._inFrom = inFrom;
        elementListReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return elementListReturn;
    }

    public HqlSqlWalker.fromElement_return fromElement()
    {
      HqlSqlWalker.fromElement_return fromElementReturn = new HqlSqlWalker.fromElement_return();
      fromElementReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      IASTNode astNode3 = (IASTNode) null;
      IASTNode astNode4 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token FILTER_ENTITY");
      RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token RANGE");
      RewriteRuleNodeStream rewriteRuleNodeStream3 = new RewriteRuleNodeStream(this.adaptor, "token FETCH");
      RewriteRuleNodeStream rewriteRuleNodeStream4 = new RewriteRuleNodeStream(this.adaptor, "token ALIAS");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule joinElement");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 32:
            num1 = 2;
            break;
          case 76:
            num1 = 3;
            break;
          case 87:
            num1 = 1;
            break;
          default:
            throw new NoViableAltException("", 33, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode el = (IASTNode) this.Match((IIntStream) this.input, 87, HqlSqlWalker.FOLLOW_RANGE_in_fromElement1125);
            rewriteRuleNodeStream2.Add((object) el);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_path_in_fromElement1129);
            HqlSqlWalker.path_return pathReturn = this.path();
            --this.state.followingStackPointer;
            ruleSubtreeStream2.Add(pathReturn.Tree);
            int num2 = 2;
            if (this.input.LA(1) == 72)
              num2 = 1;
            if (num2 == 1)
            {
              astNode2 = (IASTNode) this.input.LT(1);
              astNode3 = (IASTNode) this.Match((IIntStream) this.input, 72, HqlSqlWalker.FOLLOW_ALIAS_in_fromElement1134);
              rewriteRuleNodeStream4.Add((object) astNode3);
            }
            int num3 = 2;
            if (this.input.LA(1) == 21)
              num3 = 1;
            if (num3 == 1)
            {
              astNode2 = (IASTNode) this.input.LT(1);
              astNode4 = (IASTNode) this.Match((IIntStream) this.input, 21, HqlSqlWalker.FOLLOW_FETCH_in_fromElement1141);
              rewriteRuleNodeStream3.Add((object) astNode4);
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) nilNode1);
            astNode2 = astNode5;
            IASTNode fromElement = this.CreateFromElement(pathReturn?.p, pathReturn != null ? (IASTNode) pathReturn.Tree : (IASTNode) null, astNode3, astNode4);
            fromElementReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", fromElementReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            if (fromElement != null)
            {
              IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
              IASTNode child = (IASTNode) this.adaptor.BecomeRoot((object) fromElement, (object) nilNode2);
              this.adaptor.AddChild((object) astNode1, (object) child);
            }
            else
              astNode1 = (IASTNode) null;
            fromElementReturn.Tree = (object) astNode1;
            break;
          case 2:
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_joinElement_in_fromElement1168);
            HqlSqlWalker.joinElement_return joinElementReturn = this.joinElement();
            --this.state.followingStackPointer;
            ruleSubtreeStream1.Add(joinElementReturn.Tree);
            fromElementReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", fromElementReturn?.Tree);
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) null;
            fromElementReturn.Tree = (object) astNode1;
            break;
          case 3:
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode6 = (IASTNode) this.Match((IIntStream) this.input, 76, HqlSqlWalker.FOLLOW_FILTER_ENTITY_in_fromElement1183);
            rewriteRuleNodeStream1.Add((object) astNode6);
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode7 = (IASTNode) this.Match((IIntStream) this.input, 72, HqlSqlWalker.FOLLOW_ALIAS_in_fromElement1187);
            rewriteRuleNodeStream4.Add((object) astNode7);
            fromElementReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", fromElementReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child1 = (IASTNode) this.adaptor.BecomeRoot((object) this.CreateFromFilterElement(astNode6, astNode7), (object) nilNode4);
            this.adaptor.AddChild((object) astNode1, (object) child1);
            fromElementReturn.Tree = (object) astNode1;
            break;
        }
        fromElementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return fromElementReturn;
    }

    public HqlSqlWalker.joinElement_return joinElement()
    {
      HqlSqlWalker.joinElement_return joinElementReturn = new HqlSqlWalker.joinElement_return();
      joinElementReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      IASTNode astNode3 = (IASTNode) null;
      IASTNode astNode4 = (IASTNode) null;
      IASTNode astNode5 = (IASTNode) null;
      HqlSqlWalker.joinType_return joinTypeReturn = (HqlSqlWalker.joinType_return) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode6 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode7 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 32, HqlSqlWalker.FOLLOW_JOIN_in_joinElement1216)), (object) nilNode2);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        int num1 = 2;
        switch (this.input.LA(1))
        {
          case 23:
          case 28:
          case 33:
          case 44:
            num1 = 1;
            break;
        }
        if (num1 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          this.PushFollow(HqlSqlWalker.FOLLOW_joinType_in_joinElement1221);
          joinTypeReturn = this.joinType();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode7, joinTypeReturn.Tree);
          this.SetImpliedJoinType(joinTypeReturn != null ? joinTypeReturn.j : 0);
        }
        int num2 = 2;
        if (this.input.LA(1) == 21)
          num2 = 1;
        if (num2 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          astNode2 = (IASTNode) this.Match((IIntStream) this.input, 21, HqlSqlWalker.FOLLOW_FETCH_in_joinElement1231);
          IASTNode child = (IASTNode) this.adaptor.DupNode((object) astNode2);
          this.adaptor.AddChild((object) astNode7, (object) child);
        }
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_propertyRef_in_joinElement1237);
        HqlSqlWalker.propertyRef_return propertyRefReturn = this.propertyRef();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode7, propertyRefReturn.Tree);
        int num3 = 2;
        if (this.input.LA(1) == 72)
          num3 = 1;
        if (num3 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          astNode3 = (IASTNode) this.Match((IIntStream) this.input, 72, HqlSqlWalker.FOLLOW_ALIAS_in_joinElement1242);
          IASTNode child = (IASTNode) this.adaptor.DupNode((object) astNode3);
          this.adaptor.AddChild((object) astNode7, (object) child);
        }
        int num4 = 2;
        if (this.input.LA(1) == 21)
          num4 = 1;
        if (num4 == 1)
        {
          astNode1 = (IASTNode) this.input.LT(1);
          astNode4 = (IASTNode) this.Match((IIntStream) this.input, 21, HqlSqlWalker.FOLLOW_FETCH_in_joinElement1249);
          IASTNode child = (IASTNode) this.adaptor.DupNode((object) astNode4);
          this.adaptor.AddChild((object) astNode7, (object) child);
        }
        int num5 = 2;
        if (this.input.LA(1) == 63)
          num5 = 1;
        if (num5 == 1)
        {
          IASTNode astNode8 = (IASTNode) this.input.LT(1);
          IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
          astNode1 = (IASTNode) this.input.LT(1);
          astNode5 = (IASTNode) this.Match((IIntStream) this.input, 63, HqlSqlWalker.FOLLOW_WITH_in_joinElement1258);
          IASTNode child1 = (IASTNode) this.adaptor.DupNode((object) astNode5);
          this.adaptor.AddChild((object) nilNode3, (object) child1);
          if (this.input.LA(1) == 2)
          {
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            while (true)
            {
              int num6 = 2;
              int num7 = this.input.LA(1);
              if (num7 >= 4 && num7 <= 150)
                num6 = 1;
              else if (num7 == 3)
                num6 = 2;
              if (num6 == 1)
              {
                astNode1 = (IASTNode) this.input.LT(1);
                IASTNode tree = (IASTNode) this.input.LT(1);
                this.MatchAny((IIntStream) this.input);
                IASTNode child2 = (IASTNode) this.adaptor.DupTree((object) tree);
                this.adaptor.AddChild((object) nilNode3, (object) child2);
              }
              else
                break;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
          }
          this.adaptor.AddChild((object) astNode7, (object) nilNode3);
          astNode1 = astNode8;
        }
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) nilNode1, (object) astNode7);
        astNode1 = astNode6;
        this.CreateFromJoinElement(propertyRefReturn != null ? (IASTNode) propertyRefReturn.Tree : (IASTNode) null, astNode3, joinTypeReturn != null ? joinTypeReturn.j : 0, astNode2, astNode4, astNode5);
        this.SetImpliedJoinType(28);
        joinElementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return joinElementReturn;
    }

    public HqlSqlWalker.joinType_return joinType()
    {
      HqlSqlWalker.joinType_return joinTypeReturn = new HqlSqlWalker.joinType_return();
      joinTypeReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode treeNode1 = (IASTNode) null;
      IASTNode treeNode2 = (IASTNode) null;
      IASTNode treeNode3 = (IASTNode) null;
      joinTypeReturn.j = 28;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 23:
            num1 = 2;
            break;
          case 28:
            num1 = 3;
            break;
          case 33:
          case 44:
            num1 = 1;
            break;
          default:
            throw new NoViableAltException("", 42, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            int num2;
            switch (this.input.LA(1))
            {
              case 33:
                num2 = 1;
                break;
              case 44:
                num2 = 2;
                break;
              default:
                throw new NoViableAltException("", 40, 0, (IIntStream) this.input);
            }
            switch (num2)
            {
              case 1:
                IASTNode astNode2 = (IASTNode) this.input.LT(1);
                treeNode1 = (IASTNode) this.Match((IIntStream) this.input, 33, HqlSqlWalker.FOLLOW_LEFT_in_joinType1299);
                IASTNode child1 = (IASTNode) this.adaptor.DupNode((object) treeNode1);
                this.adaptor.AddChild((object) astNode1, (object) child1);
                break;
              case 2:
                IASTNode astNode3 = (IASTNode) this.input.LT(1);
                treeNode2 = (IASTNode) this.Match((IIntStream) this.input, 44, HqlSqlWalker.FOLLOW_RIGHT_in_joinType1305);
                IASTNode child2 = (IASTNode) this.adaptor.DupNode((object) treeNode2);
                this.adaptor.AddChild((object) astNode1, (object) child2);
                break;
            }
            int num3 = 2;
            if (this.input.LA(1) == 42)
              num3 = 1;
            if (num3 == 1)
            {
              IASTNode astNode4 = (IASTNode) this.input.LT(1);
              treeNode3 = (IASTNode) this.Match((IIntStream) this.input, 42, HqlSqlWalker.FOLLOW_OUTER_in_joinType1311);
              IASTNode child3 = (IASTNode) this.adaptor.DupNode((object) treeNode3);
              this.adaptor.AddChild((object) astNode1, (object) child3);
            }
            if (treeNode1 != null)
            {
              joinTypeReturn.j = 139;
              break;
            }
            if (treeNode2 != null)
            {
              joinTypeReturn.j = 140;
              break;
            }
            if (treeNode3 != null)
            {
              joinTypeReturn.j = 140;
              break;
            }
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            IASTNode child4 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 23, HqlSqlWalker.FOLLOW_FULL_in_joinType1325));
            this.adaptor.AddChild((object) astNode1, (object) child4);
            joinTypeReturn.j = 23;
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode6 = (IASTNode) this.input.LT(1);
            IASTNode child5 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 28, HqlSqlWalker.FOLLOW_INNER_in_joinType1332));
            this.adaptor.AddChild((object) astNode1, (object) child5);
            joinTypeReturn.j = 28;
            break;
        }
        joinTypeReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return joinTypeReturn;
    }

    public HqlSqlWalker.path_return path()
    {
      HqlSqlWalker.path_return pathReturn1 = new HqlSqlWalker.path_return();
      pathReturn1.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 15:
            num = 2;
            break;
          case 93:
          case 125:
            num = 1;
            break;
          default:
            throw new NoViableAltException("", 43, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_path1354);
            HqlSqlWalker.identifier_return identifierReturn1 = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, identifierReturn1.Tree);
            pathReturn1.p = (identifierReturn1 != null ? (object) (IASTNode) identifierReturn1.Start : (object) (IASTNode) null).ToString();
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 15, HqlSqlWalker.FOLLOW_DOT_in_path1362)), (object) nilNode);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_path_in_path1366);
            HqlSqlWalker.path_return pathReturn2 = this.path();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, pathReturn2.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_path1370);
            HqlSqlWalker.identifier_return identifierReturn2 = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, identifierReturn2.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(pathReturn2?.p).Append('.').Append((identifierReturn2 != null ? (object) (IASTNode) identifierReturn2.Start : (object) (IASTNode) null).ToString());
            pathReturn1.p = stringBuilder.ToString();
            break;
        }
        pathReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return pathReturn1;
    }

    public HqlSqlWalker.pathAsIdent_return pathAsIdent()
    {
      HqlSqlWalker.pathAsIdent_return pathAsIdentReturn = new HqlSqlWalker.pathAsIdent_return();
      pathAsIdentReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_path_in_pathAsIdent1389);
        HqlSqlWalker.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(pathReturn.Tree);
        pathAsIdentReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", pathAsIdentReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode child = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(125, pathReturn?.p), (object) nilNode2);
        this.adaptor.AddChild((object) nilNode1, (object) child);
        pathAsIdentReturn.Tree = (object) nilNode1;
        pathAsIdentReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return pathAsIdentReturn;
    }

    public HqlSqlWalker.withClause_return withClause()
    {
      HqlSqlWalker.withClause_return withClauseReturn = new HqlSqlWalker.withClause_return();
      withClauseReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token WITH");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule logicalExpr");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.Match((IIntStream) this.input, 63, HqlSqlWalker.FOLLOW_WITH_in_withClause1430);
        rewriteRuleNodeStream1.Add((object) astNode3);
        this.HandleClauseStart(63);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_withClause1436);
        HqlSqlWalker.logicalExpr_return logicalExprReturn = this.logicalExpr();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(logicalExprReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode2;
        withClauseReturn.Tree = (object) t;
        RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token w", (object) astNode3);
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", withClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule b", logicalExprReturn?.Tree);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleNodeStream2.NextNode(), (object) nilNode3);
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream3.NextTree());
        this.adaptor.AddChild((object) nilNode2, (object) astNode4);
        withClauseReturn.Tree = (object) nilNode2;
        withClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode2);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return withClauseReturn;
    }

    public HqlSqlWalker.whereClause_return whereClause()
    {
      HqlSqlWalker.whereClause_return whereClauseReturn = new HqlSqlWalker.whereClause_return();
      whereClauseReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token WHERE");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule logicalExpr");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.Match((IIntStream) this.input, 55, HqlSqlWalker.FOLLOW_WHERE_in_whereClause1464);
        rewriteRuleNodeStream1.Add((object) astNode3);
        this.HandleClauseStart(55);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_whereClause1470);
        HqlSqlWalker.logicalExpr_return logicalExprReturn = this.logicalExpr();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(logicalExprReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode2;
        whereClauseReturn.Tree = (object) t;
        RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token w", (object) astNode3);
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", whereClauseReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule b", logicalExprReturn?.Tree);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleNodeStream2.NextNode(), (object) nilNode3);
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream3.NextTree());
        this.adaptor.AddChild((object) nilNode2, (object) astNode4);
        whereClauseReturn.Tree = (object) nilNode2;
        whereClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode2);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return whereClauseReturn;
    }

    public HqlSqlWalker.logicalExpr_return logicalExpr()
    {
      HqlSqlWalker.logicalExpr_return logicalExprReturn1 = new HqlSqlWalker.logicalExpr_return();
      logicalExprReturn1.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 6:
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
            num = 4;
            break;
          case 15:
          case 78:
          case 93:
          case 125:
            num = 6;
            break;
          case 38:
            num = 3;
            break;
          case 40:
            num = 2;
            break;
          case 71:
          case 81:
            num = 5;
            break;
          default:
            throw new NoViableAltException("", 44, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 6, HqlSqlWalker.FOLLOW_AND_in_logicalExpr1496)), (object) nilNode1);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_logicalExpr1498);
            HqlSqlWalker.logicalExpr_return logicalExprReturn2 = this.logicalExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, logicalExprReturn2.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_logicalExpr1500);
            HqlSqlWalker.logicalExpr_return logicalExprReturn3 = this.logicalExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, logicalExprReturn3.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode6 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 40, HqlSqlWalker.FOLLOW_OR_in_logicalExpr1507)), (object) nilNode2);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_logicalExpr1509);
            HqlSqlWalker.logicalExpr_return logicalExprReturn4 = this.logicalExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode6, logicalExprReturn4.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_logicalExpr1511);
            HqlSqlWalker.logicalExpr_return logicalExprReturn5 = this.logicalExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode6, logicalExprReturn5.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode6);
            astNode2 = astNode5;
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode7 = (IASTNode) this.input.LT(1);
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode8 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 38, HqlSqlWalker.FOLLOW_NOT_in_logicalExpr1518)), (object) nilNode3);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_logicalExpr1520);
            HqlSqlWalker.logicalExpr_return logicalExprReturn6 = this.logicalExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode8, logicalExprReturn6.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode8);
            astNode2 = astNode7;
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_comparisonExpr_in_logicalExpr1526);
            HqlSqlWalker.comparisonExpr_return comparisonExprReturn = this.comparisonExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, comparisonExprReturn.Tree);
            break;
          case 5:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_functionCall_in_logicalExpr1531);
            HqlSqlWalker.functionCall_return functionCallReturn = this.functionCall();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, functionCallReturn.Tree);
            break;
          case 6:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_logicalPath_in_logicalExpr1536);
            HqlSqlWalker.logicalPath_return logicalPathReturn = this.logicalPath();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, logicalPathReturn.Tree);
            break;
        }
        logicalExprReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return logicalExprReturn1;
    }

    public HqlSqlWalker.logicalPath_return logicalPath()
    {
      HqlSqlWalker.logicalPath_return logicalPathReturn = new HqlSqlWalker.logicalPath_return();
      logicalPathReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule addrExpr");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_addrExpr_in_logicalPath1555);
        HqlSqlWalker.addrExpr_return addrExprReturn = this.addrExpr(true);
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(addrExprReturn.Tree);
        this.Resolve(addrExprReturn != null ? (IASTNode) addrExprReturn.Tree : (IASTNode) null);
        logicalPathReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", logicalPathReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule p", addrExprReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(102, "EQ"), (object) nilNode2);
        this.adaptor.AddChild((object) astNode3, ruleSubtreeStream3.NextTree());
        this.adaptor.AddChild((object) astNode3, (object) (IASTNode) this.adaptor.Create(51, "TRUE"));
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        logicalPathReturn.Tree = (object) nilNode1;
        logicalPathReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        HqlSqlWalker.PrepareLogicOperator((IASTNode) logicalPathReturn.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return logicalPathReturn;
    }

    public HqlSqlWalker.comparisonExpr_return comparisonExpr()
    {
      HqlSqlWalker.comparisonExpr_return comparisonExprReturn = new HqlSqlWalker.comparisonExpr_return();
      comparisonExprReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        int num1;
        switch (this.input.LA(1))
        {
          case 10:
            num1 = 9;
            break;
          case 19:
            num1 = 15;
            break;
          case 26:
            num1 = 11;
            break;
          case 34:
            num1 = 7;
            break;
          case 79:
            num1 = 14;
            break;
          case 80:
            num1 = 13;
            break;
          case 82:
            num1 = 10;
            break;
          case 83:
            num1 = 12;
            break;
          case 84:
            num1 = 8;
            break;
          case 102:
            num1 = 1;
            break;
          case 107:
            num1 = 2;
            break;
          case 109:
            num1 = 3;
            break;
          case 110:
            num1 = 4;
            break;
          case 111:
            num1 = 5;
            break;
          case 112:
            num1 = 6;
            break;
          default:
            throw new NoViableAltException("", 48, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 102, HqlSqlWalker.FOLLOW_EQ_in_comparisonExpr1593)), (object) nilNode2);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1595);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn1 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, orSubqueryReturn1.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1597);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn2 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode3, orSubqueryReturn2.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode3);
            astNode1 = astNode2;
            break;
          case 2:
            IASTNode astNode4 = (IASTNode) this.input.LT(1);
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode5 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 107, HqlSqlWalker.FOLLOW_NE_in_comparisonExpr1604)), (object) nilNode3);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1606);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn3 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode5, orSubqueryReturn3.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1608);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn4 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode5, orSubqueryReturn4.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode5);
            astNode1 = astNode4;
            break;
          case 3:
            IASTNode astNode6 = (IASTNode) this.input.LT(1);
            IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode7 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 109, HqlSqlWalker.FOLLOW_LT_in_comparisonExpr1615)), (object) nilNode4);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1617);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn5 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode7, orSubqueryReturn5.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1619);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn6 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode7, orSubqueryReturn6.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode7);
            astNode1 = astNode6;
            break;
          case 4:
            IASTNode astNode8 = (IASTNode) this.input.LT(1);
            IASTNode nilNode5 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode9 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 110, HqlSqlWalker.FOLLOW_GT_in_comparisonExpr1626)), (object) nilNode5);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1628);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn7 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode9, orSubqueryReturn7.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1630);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn8 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode9, orSubqueryReturn8.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode9);
            astNode1 = astNode8;
            break;
          case 5:
            IASTNode astNode10 = (IASTNode) this.input.LT(1);
            IASTNode nilNode6 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode11 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 111, HqlSqlWalker.FOLLOW_LE_in_comparisonExpr1637)), (object) nilNode6);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1639);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn9 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode11, orSubqueryReturn9.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1641);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn10 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode11, orSubqueryReturn10.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode11);
            astNode1 = astNode10;
            break;
          case 6:
            IASTNode astNode12 = (IASTNode) this.input.LT(1);
            IASTNode nilNode7 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode13 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 112, HqlSqlWalker.FOLLOW_GE_in_comparisonExpr1648)), (object) nilNode7);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1650);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn11 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode13, orSubqueryReturn11.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1652);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn12 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode13, orSubqueryReturn12.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode13);
            astNode1 = astNode12;
            break;
          case 7:
            IASTNode astNode14 = (IASTNode) this.input.LT(1);
            IASTNode nilNode8 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode15 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 34, HqlSqlWalker.FOLLOW_LIKE_in_comparisonExpr1659)), (object) nilNode8);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1661);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn13 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode15, orSubqueryReturn13.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_comparisonExpr1663);
            HqlSqlWalker.expr_return exprReturn1 = this.expr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode15, exprReturn1.Tree);
            int num2 = 2;
            if (this.input.LA(1) == 18)
              num2 = 1;
            if (num2 == 1)
            {
              IASTNode astNode16 = (IASTNode) this.input.LT(1);
              IASTNode nilNode9 = (IASTNode) this.adaptor.GetNilNode();
              astNode1 = (IASTNode) this.input.LT(1);
              IASTNode astNode17 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 18, HqlSqlWalker.FOLLOW_ESCAPE_in_comparisonExpr1668)), (object) nilNode9);
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              astNode1 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_comparisonExpr1670);
              HqlSqlWalker.expr_return exprReturn2 = this.expr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode17, exprReturn2.Tree);
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              this.adaptor.AddChild((object) astNode15, (object) astNode17);
              astNode1 = astNode16;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode15);
            astNode1 = astNode14;
            break;
          case 8:
            IASTNode astNode18 = (IASTNode) this.input.LT(1);
            IASTNode nilNode10 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode19 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 84, HqlSqlWalker.FOLLOW_NOT_LIKE_in_comparisonExpr1682)), (object) nilNode10);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1684);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn14 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode19, orSubqueryReturn14.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_comparisonExpr1686);
            HqlSqlWalker.expr_return exprReturn3 = this.expr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode19, exprReturn3.Tree);
            int num3 = 2;
            if (this.input.LA(1) == 18)
              num3 = 1;
            if (num3 == 1)
            {
              IASTNode astNode20 = (IASTNode) this.input.LT(1);
              IASTNode nilNode11 = (IASTNode) this.adaptor.GetNilNode();
              astNode1 = (IASTNode) this.input.LT(1);
              IASTNode astNode21 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 18, HqlSqlWalker.FOLLOW_ESCAPE_in_comparisonExpr1691)), (object) nilNode11);
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              astNode1 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_comparisonExpr1693);
              HqlSqlWalker.expr_return exprReturn4 = this.expr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode21, exprReturn4.Tree);
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              this.adaptor.AddChild((object) astNode19, (object) astNode21);
              astNode1 = astNode20;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode19);
            astNode1 = astNode18;
            break;
          case 9:
            IASTNode astNode22 = (IASTNode) this.input.LT(1);
            IASTNode nilNode12 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode23 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 10, HqlSqlWalker.FOLLOW_BETWEEN_in_comparisonExpr1705)), (object) nilNode12);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1707);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn15 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode23, orSubqueryReturn15.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1709);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn16 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode23, orSubqueryReturn16.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1711);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn17 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode23, orSubqueryReturn17.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode23);
            astNode1 = astNode22;
            break;
          case 10:
            IASTNode astNode24 = (IASTNode) this.input.LT(1);
            IASTNode nilNode13 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode25 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 82, HqlSqlWalker.FOLLOW_NOT_BETWEEN_in_comparisonExpr1718)), (object) nilNode13);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1720);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn18 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode25, orSubqueryReturn18.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1722);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn19 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode25, orSubqueryReturn19.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1724);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn20 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode25, orSubqueryReturn20.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode25);
            astNode1 = astNode24;
            break;
          case 11:
            IASTNode astNode26 = (IASTNode) this.input.LT(1);
            IASTNode nilNode14 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode27 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 26, HqlSqlWalker.FOLLOW_IN_in_comparisonExpr1731)), (object) nilNode14);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1733);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn21 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode27, orSubqueryReturn21.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_inRhs_in_comparisonExpr1735);
            HqlSqlWalker.inRhs_return inRhsReturn1 = this.inRhs();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode27, inRhsReturn1.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode27);
            astNode1 = astNode26;
            break;
          case 12:
            IASTNode astNode28 = (IASTNode) this.input.LT(1);
            IASTNode nilNode15 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode29 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 83, HqlSqlWalker.FOLLOW_NOT_IN_in_comparisonExpr1743)), (object) nilNode15);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1745);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn22 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode29, orSubqueryReturn22.Tree);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_inRhs_in_comparisonExpr1747);
            HqlSqlWalker.inRhs_return inRhsReturn2 = this.inRhs();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode29, inRhsReturn2.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode29);
            astNode1 = astNode28;
            break;
          case 13:
            IASTNode astNode30 = (IASTNode) this.input.LT(1);
            IASTNode nilNode16 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode31 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 80, HqlSqlWalker.FOLLOW_IS_NULL_in_comparisonExpr1755)), (object) nilNode16);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1757);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn23 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode31, orSubqueryReturn23.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode31);
            astNode1 = astNode30;
            break;
          case 14:
            IASTNode astNode32 = (IASTNode) this.input.LT(1);
            IASTNode nilNode17 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode33 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 79, HqlSqlWalker.FOLLOW_IS_NOT_NULL_in_comparisonExpr1764)), (object) nilNode17);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode1 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_comparisonExpr1766);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn24 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode33, orSubqueryReturn24.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode33);
            astNode1 = astNode32;
            break;
          case 15:
            IASTNode astNode34 = (IASTNode) this.input.LT(1);
            IASTNode nilNode18 = (IASTNode) this.adaptor.GetNilNode();
            astNode1 = (IASTNode) this.input.LT(1);
            IASTNode astNode35 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 19, HqlSqlWalker.FOLLOW_EXISTS_in_comparisonExpr1775)), (object) nilNode18);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            int num4 = this.input.LA(1);
            int num5;
            if (num4 == 12 || num4 == 15 || num4 == 20 || num4 == 39 || num4 == 51 || num4 == 57 || num4 == 71 || num4 == 74 || num4 == 78 || num4 == 81 || num4 == 90 || num4 >= 92 && num4 <= 93 || num4 >= 95 && num4 <= 100 || num4 >= 105 && num4 <= 106 || num4 >= 114 && num4 <= 121 || num4 >= 124 && num4 <= 125)
            {
              num5 = 1;
            }
            else
            {
              if (num4 != 17 && num4 != 27 && num4 != 52 && num4 != 86)
                throw new NoViableAltException("", 47, 0, (IIntStream) this.input);
              num5 = 2;
            }
            switch (num5)
            {
              case 1:
                astNode1 = (IASTNode) this.input.LT(1);
                this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_comparisonExpr1779);
                HqlSqlWalker.expr_return exprReturn5 = this.expr();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode35, exprReturn5.Tree);
                break;
              case 2:
                astNode1 = (IASTNode) this.input.LT(1);
                this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunctionOrSubselect_in_comparisonExpr1783);
                HqlSqlWalker.collectionFunctionOrSubselect_return orSubselectReturn = this.collectionFunctionOrSubselect();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode35, orSubselectReturn.Tree);
                break;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) nilNode1, (object) astNode35);
            astNode1 = astNode34;
            break;
        }
        comparisonExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        HqlSqlWalker.PrepareLogicOperator((IASTNode) comparisonExprReturn.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return comparisonExprReturn;
    }

    public HqlSqlWalker.inRhs_return inRhs()
    {
      HqlSqlWalker.inRhs_return inRhsReturn = new HqlSqlWalker.inRhs_return();
      inRhsReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      int num1 = 99999;
      try
      {
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 77, HqlSqlWalker.FOLLOW_IN_LIST_in_inRhs1807)), (object) nilNode2);
        if (this.input.LA(1) == 2)
        {
          this.Match((IIntStream) this.input, 2, (BitSet) null);
          int num2 = this.input.LA(1);
          int num3;
          switch (num2)
          {
            case 17:
            case 27:
            case 52:
            case 86:
              num3 = 1;
              break;
            default:
              if (num2 != num1 && num2 != 12 && num2 != 15 && num2 != 20 && num2 != 39 && num2 != 51 && num2 != 57 && num2 != 71 && num2 != 74 && num2 != 78 && num2 != 81 && num2 != 90 && (num2 < 92 || num2 > 93) && (num2 < 95 || num2 > 100) && (num2 < 105 || num2 > 106) && (num2 < 114 || num2 > 121) && (num2 < 124 || num2 > 125))
                throw new NoViableAltException("", 50, 0, (IIntStream) this.input);
              num3 = 2;
              break;
          }
          switch (num3)
          {
            case 1:
              astNode1 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunctionOrSubselect_in_inRhs1811);
              HqlSqlWalker.collectionFunctionOrSubselect_return orSubselectReturn = this.collectionFunctionOrSubselect();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode3, orSubselectReturn.Tree);
              break;
            case 2:
              while (true)
              {
                int num4 = 2;
                int num5 = this.input.LA(1);
                if (num5 == 12 || num5 == 15 || num5 == 20 || num5 == 39 || num5 == 51 || num5 == 57 || num5 == 71 || num5 == 74 || num5 == 78 || num5 == 81 || num5 == 90 || num5 >= 92 && num5 <= 93 || num5 >= 95 && num5 <= 100 || num5 >= 105 && num5 <= 106 || num5 >= 114 && num5 <= 121 || num5 >= 124 && num5 <= 125)
                  num4 = 1;
                if (num4 == 1)
                {
                  astNode1 = (IASTNode) this.input.LT(1);
                  this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_inRhs1815);
                  HqlSqlWalker.expr_return exprReturn = this.expr();
                  --this.state.followingStackPointer;
                  this.adaptor.AddChild((object) astNode3, exprReturn.Tree);
                }
                else
                  break;
              }
          }
          this.Match((IIntStream) this.input, 3, (BitSet) null);
        }
        this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        astNode1 = astNode2;
        inRhsReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return inRhsReturn;
    }

    public HqlSqlWalker.exprOrSubquery_return exprOrSubquery()
    {
      HqlSqlWalker.exprOrSubquery_return orSubqueryReturn = new HqlSqlWalker.exprOrSubquery_return();
      orSubqueryReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 4:
            num = 4;
            break;
          case 5:
            num = 3;
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
          case 92:
          case 93:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 100:
          case 105:
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
            num = 1;
            break;
          case 48:
            num = 5;
            break;
          case 52:
          case 86:
            num = 2;
            break;
          default:
            throw new NoViableAltException("", 51, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_exprOrSubquery1831);
            HqlSqlWalker.expr_return exprReturn = this.expr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, exprReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_query_in_exprOrSubquery1836);
            HqlSqlWalker.query_return queryReturn = this.query();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, queryReturn.Tree);
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 5, HqlSqlWalker.FOLLOW_ANY_in_exprOrSubquery1842)), (object) nilNode1);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunctionOrSubselect_in_exprOrSubquery1844);
            HqlSqlWalker.collectionFunctionOrSubselect_return orSubselectReturn1 = this.collectionFunctionOrSubselect();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, orSubselectReturn1.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode6 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 4, HqlSqlWalker.FOLLOW_ALL_in_exprOrSubquery1851)), (object) nilNode2);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunctionOrSubselect_in_exprOrSubquery1853);
            HqlSqlWalker.collectionFunctionOrSubselect_return orSubselectReturn2 = this.collectionFunctionOrSubselect();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode6, orSubselectReturn2.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode6);
            astNode2 = astNode5;
            break;
          case 5:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode7 = (IASTNode) this.input.LT(1);
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode8 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 48, HqlSqlWalker.FOLLOW_SOME_in_exprOrSubquery1860)), (object) nilNode3);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunctionOrSubselect_in_exprOrSubquery1862);
            HqlSqlWalker.collectionFunctionOrSubselect_return orSubselectReturn3 = this.collectionFunctionOrSubselect();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode8, orSubselectReturn3.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode8);
            astNode2 = astNode7;
            break;
        }
        orSubqueryReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return orSubqueryReturn;
    }

    public HqlSqlWalker.collectionFunctionOrSubselect_return collectionFunctionOrSubselect()
    {
      HqlSqlWalker.collectionFunctionOrSubselect_return orSubselectReturn = new HqlSqlWalker.collectionFunctionOrSubselect_return();
      orSubselectReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 17:
          case 27:
            num = 1;
            break;
          case 52:
          case 86:
            num = 2;
            break;
          default:
            throw new NoViableAltException("", 52, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_collectionFunction_in_collectionFunctionOrSubselect1875);
            HqlSqlWalker.collectionFunction_return collectionFunctionReturn = this.collectionFunction();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, collectionFunctionReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_query_in_collectionFunctionOrSubselect1880);
            HqlSqlWalker.query_return queryReturn = this.query();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, queryReturn.Tree);
            break;
        }
        orSubselectReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return orSubselectReturn;
    }

    public HqlSqlWalker.expr_return expr()
    {
      HqlSqlWalker.expr_return exprReturn1 = new HqlSqlWalker.expr_return();
      exprReturn1.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 12:
            num1 = 7;
            break;
          case 15:
          case 78:
          case 93:
          case 125:
            num1 = 1;
            break;
          case 20:
          case 39:
          case 51:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 100:
          case 124:
            num1 = 3;
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
            num1 = 4;
            break;
          case 71:
          case 81:
            num1 = 5;
            break;
          case 92:
            num1 = 2;
            break;
          case 105:
          case 106:
            num1 = 6;
            break;
          default:
            throw new NoViableAltException("", 54, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_addrExpr_in_expr1894);
            HqlSqlWalker.addrExpr_return addrExprReturn = this.addrExpr(true);
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, addrExprReturn.Tree);
            this.Resolve(addrExprReturn != null ? (IASTNode) addrExprReturn.Tree : (IASTNode) null);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 92, HqlSqlWalker.FOLLOW_VECTOR_EXPR_in_expr1906)), (object) nilNode);
            if (this.input.LA(1) == 2)
            {
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              while (true)
              {
                int num2 = 2;
                int num3 = this.input.LA(1);
                if (num3 == 12 || num3 == 15 || num3 == 20 || num3 == 39 || num3 == 51 || num3 == 57 || num3 == 71 || num3 == 74 || num3 == 78 || num3 == 81 || num3 == 90 || num3 >= 92 && num3 <= 93 || num3 >= 95 && num3 <= 100 || num3 >= 105 && num3 <= 106 || num3 >= 114 && num3 <= 121 || num3 >= 124 && num3 <= 125)
                  num2 = 1;
                if (num2 == 1)
                {
                  astNode2 = (IASTNode) this.input.LT(1);
                  this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_expr1909);
                  HqlSqlWalker.expr_return exprReturn2 = this.expr();
                  --this.state.followingStackPointer;
                  this.adaptor.AddChild((object) astNode4, exprReturn2.Tree);
                }
                else
                  break;
              }
              this.Match((IIntStream) this.input, 3, (BitSet) null);
            }
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_constant_in_expr1918);
            HqlSqlWalker.constant_return constantReturn = this.constant();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, constantReturn.Tree);
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_arithmeticExpr_in_expr1923);
            HqlSqlWalker.arithmeticExpr_return arithmeticExprReturn = this.arithmeticExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, arithmeticExprReturn.Tree);
            break;
          case 5:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_functionCall_in_expr1928);
            HqlSqlWalker.functionCall_return functionCallReturn = this.functionCall();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, functionCallReturn.Tree);
            break;
          case 6:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_parameter_in_expr1940);
            HqlSqlWalker.parameter_return parameterReturn = this.parameter();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, parameterReturn.Tree);
            break;
          case 7:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_count_in_expr1945);
            HqlSqlWalker.count_return countReturn = this.count();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, countReturn.Tree);
            break;
        }
        exprReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return exprReturn1;
    }

    public HqlSqlWalker.arithmeticExpr_return arithmeticExpr()
    {
      HqlSqlWalker.arithmeticExpr_return arithmeticExprReturn = new HqlSqlWalker.arithmeticExpr_return();
      arithmeticExprReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      HqlSqlWalker.caseExpr_return caseExprReturn = (HqlSqlWalker.caseExpr_return) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 57:
          case 74:
            num = 10;
            break;
          case 90:
            num = 9;
            break;
          case 114:
            num = 5;
            break;
          case 115:
            num = 7;
            break;
          case 116:
            num = 8;
            break;
          case 117:
            num = 6;
            break;
          case 118:
            num = 1;
            break;
          case 119:
            num = 2;
            break;
          case 120:
            num = 4;
            break;
          case 121:
            num = 3;
            break;
          default:
            throw new NoViableAltException("", 55, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 118, HqlSqlWalker.FOLLOW_PLUS_in_arithmeticExpr1973)), (object) nilNode1);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr1975);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn1 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, orSubqueryReturn1.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr1977);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn2 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode4, orSubqueryReturn2.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode6 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 119, HqlSqlWalker.FOLLOW_MINUS_in_arithmeticExpr1984)), (object) nilNode2);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr1986);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn3 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode6, orSubqueryReturn3.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr1988);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn4 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode6, orSubqueryReturn4.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode6);
            astNode2 = astNode5;
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode7 = (IASTNode) this.input.LT(1);
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode8 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 121, HqlSqlWalker.FOLLOW_DIV_in_arithmeticExpr1995)), (object) nilNode3);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr1997);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn5 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode8, orSubqueryReturn5.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr1999);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn6 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode8, orSubqueryReturn6.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode8);
            astNode2 = astNode7;
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode9 = (IASTNode) this.input.LT(1);
            IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode10 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 120, HqlSqlWalker.FOLLOW_STAR_in_arithmeticExpr2006)), (object) nilNode4);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2008);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn7 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode10, orSubqueryReturn7.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2010);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn8 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode10, orSubqueryReturn8.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode10);
            astNode2 = astNode9;
            break;
          case 5:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode11 = (IASTNode) this.input.LT(1);
            IASTNode nilNode5 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode12 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 114, HqlSqlWalker.FOLLOW_BNOT_in_arithmeticExpr2017)), (object) nilNode5);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2019);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn9 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode12, orSubqueryReturn9.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode12);
            astNode2 = astNode11;
            break;
          case 6:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode13 = (IASTNode) this.input.LT(1);
            IASTNode nilNode6 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode14 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 117, HqlSqlWalker.FOLLOW_BAND_in_arithmeticExpr2026)), (object) nilNode6);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2028);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn10 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode14, orSubqueryReturn10.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2030);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn11 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode14, orSubqueryReturn11.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode14);
            astNode2 = astNode13;
            break;
          case 7:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode15 = (IASTNode) this.input.LT(1);
            IASTNode nilNode7 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode16 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 115, HqlSqlWalker.FOLLOW_BOR_in_arithmeticExpr2037)), (object) nilNode7);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2039);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn12 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode16, orSubqueryReturn12.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2041);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn13 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode16, orSubqueryReturn13.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode16);
            astNode2 = astNode15;
            break;
          case 8:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode17 = (IASTNode) this.input.LT(1);
            IASTNode nilNode8 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode18 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 116, HqlSqlWalker.FOLLOW_BXOR_in_arithmeticExpr2048)), (object) nilNode8);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2050);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn14 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode18, orSubqueryReturn14.Tree);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2052);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn15 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode18, orSubqueryReturn15.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode18);
            astNode2 = astNode17;
            break;
          case 9:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode19 = (IASTNode) this.input.LT(1);
            IASTNode nilNode9 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode20 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 90, HqlSqlWalker.FOLLOW_UNARY_MINUS_in_arithmeticExpr2060)), (object) nilNode9);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_exprOrSubquery_in_arithmeticExpr2062);
            HqlSqlWalker.exprOrSubquery_return orSubqueryReturn16 = this.exprOrSubquery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode20, orSubqueryReturn16.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode20);
            astNode2 = astNode19;
            break;
          case 10:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_caseExpr_in_arithmeticExpr2070);
            caseExprReturn = this.caseExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, caseExprReturn.Tree);
            break;
        }
        arithmeticExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
        if ((caseExprReturn != null ? (IASTNode) caseExprReturn.Tree : (IASTNode) null) == null)
          HqlSqlWalker.PrepareArithmeticOperator((IASTNode) arithmeticExprReturn.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return arithmeticExprReturn;
    }

    public HqlSqlWalker.caseExpr_return caseExpr()
    {
      HqlSqlWalker.caseExpr_return caseExprReturn = new HqlSqlWalker.caseExpr_return();
      caseExprReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
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
            throw new NoViableAltException("", 60, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 57, HqlSqlWalker.FOLLOW_CASE_in_caseExpr2082)), (object) nilNode1);
            this._inCase = true;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            int num2 = 0;
            while (true)
            {
              int num3 = 2;
              if (this.input.LA(1) == 61)
                num3 = 1;
              if (num3 == 1)
              {
                IASTNode astNode5 = (IASTNode) this.input.LT(1);
                IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
                astNode2 = (IASTNode) this.input.LT(1);
                IASTNode astNode6 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 61, HqlSqlWalker.FOLLOW_WHEN_in_caseExpr2088)), (object) nilNode2);
                this.Match((IIntStream) this.input, 2, (BitSet) null);
                astNode2 = (IASTNode) this.input.LT(1);
                this.PushFollow(HqlSqlWalker.FOLLOW_logicalExpr_in_caseExpr2090);
                HqlSqlWalker.logicalExpr_return logicalExprReturn = this.logicalExpr();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode6, logicalExprReturn.Tree);
                astNode2 = (IASTNode) this.input.LT(1);
                this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_caseExpr2092);
                HqlSqlWalker.expr_return exprReturn = this.expr();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode6, exprReturn.Tree);
                this.Match((IIntStream) this.input, 3, (BitSet) null);
                this.adaptor.AddChild((object) astNode4, (object) astNode6);
                astNode2 = astNode5;
                ++num2;
              }
              else
                break;
            }
            if (num2 < 1)
              throw new EarlyExitException(56, (IIntStream) this.input);
            int num4 = 2;
            if (this.input.LA(1) == 59)
              num4 = 1;
            if (num4 == 1)
            {
              IASTNode astNode7 = (IASTNode) this.input.LT(1);
              IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
              astNode2 = (IASTNode) this.input.LT(1);
              IASTNode astNode8 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 59, HqlSqlWalker.FOLLOW_ELSE_in_caseExpr2099)), (object) nilNode3);
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              astNode2 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_caseExpr2101);
              HqlSqlWalker.expr_return exprReturn = this.expr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode8, exprReturn.Tree);
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              this.adaptor.AddChild((object) astNode4, (object) astNode8);
              astNode2 = astNode7;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode4);
            astNode2 = astNode3;
            this._inCase = false;
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode9 = (IASTNode) this.input.LT(1);
            IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode10 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 74, HqlSqlWalker.FOLLOW_CASE2_in_caseExpr2113)), (object) nilNode4);
            this._inCase = true;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_caseExpr2117);
            HqlSqlWalker.expr_return exprReturn1 = this.expr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode10, exprReturn1.Tree);
            int num5 = 0;
            while (true)
            {
              int num6 = 2;
              if (this.input.LA(1) == 61)
                num6 = 1;
              if (num6 == 1)
              {
                IASTNode astNode11 = (IASTNode) this.input.LT(1);
                IASTNode nilNode5 = (IASTNode) this.adaptor.GetNilNode();
                astNode2 = (IASTNode) this.input.LT(1);
                IASTNode astNode12 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 61, HqlSqlWalker.FOLLOW_WHEN_in_caseExpr2121)), (object) nilNode5);
                this.Match((IIntStream) this.input, 2, (BitSet) null);
                astNode2 = (IASTNode) this.input.LT(1);
                this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_caseExpr2123);
                HqlSqlWalker.expr_return exprReturn2 = this.expr();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode12, exprReturn2.Tree);
                astNode2 = (IASTNode) this.input.LT(1);
                this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_caseExpr2125);
                HqlSqlWalker.expr_return exprReturn3 = this.expr();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode12, exprReturn3.Tree);
                this.Match((IIntStream) this.input, 3, (BitSet) null);
                this.adaptor.AddChild((object) astNode10, (object) astNode12);
                astNode2 = astNode11;
                ++num5;
              }
              else
                break;
            }
            if (num5 < 1)
              throw new EarlyExitException(58, (IIntStream) this.input);
            int num7 = 2;
            if (this.input.LA(1) == 59)
              num7 = 1;
            if (num7 == 1)
            {
              IASTNode astNode13 = (IASTNode) this.input.LT(1);
              IASTNode nilNode6 = (IASTNode) this.adaptor.GetNilNode();
              astNode2 = (IASTNode) this.input.LT(1);
              IASTNode astNode14 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 59, HqlSqlWalker.FOLLOW_ELSE_in_caseExpr2132)), (object) nilNode6);
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              astNode2 = (IASTNode) this.input.LT(1);
              this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_caseExpr2134);
              HqlSqlWalker.expr_return exprReturn4 = this.expr();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode14, exprReturn4.Tree);
              this.Match((IIntStream) this.input, 3, (BitSet) null);
              this.adaptor.AddChild((object) astNode10, (object) astNode14);
              astNode2 = astNode13;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode10);
            astNode2 = astNode9;
            this._inCase = false;
            break;
        }
        caseExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return caseExprReturn;
    }

    public HqlSqlWalker.collectionFunction_return collectionFunction()
    {
      HqlSqlWalker.collectionFunction_return collectionFunctionReturn = new HqlSqlWalker.collectionFunction_return();
      collectionFunctionReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 17:
            num = 1;
            break;
          case 27:
            num = 2;
            break;
          default:
            throw new NoViableAltException("", 61, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 17, HqlSqlWalker.FOLLOW_ELEMENTS_in_collectionFunction2156));
            IASTNode astNode5 = (IASTNode) this.adaptor.BecomeRoot((object) astNode4, (object) nilNode1);
            this._inFunctionCall = true;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_propertyRef_in_collectionFunction2162);
            HqlSqlWalker.propertyRef_return propertyRefReturn1 = this.propertyRef();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode5, propertyRefReturn1.Tree);
            this.Resolve(propertyRefReturn1 != null ? (IASTNode) propertyRefReturn1.Tree : (IASTNode) null);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode5);
            astNode2 = astNode3;
            HqlSqlWalker.ProcessFunction(astNode4, this._inSelect);
            this._inFunctionCall = false;
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode6 = (IASTNode) this.input.LT(1);
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode7 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 27, HqlSqlWalker.FOLLOW_INDICES_in_collectionFunction2181));
            IASTNode astNode8 = (IASTNode) this.adaptor.BecomeRoot((object) astNode7, (object) nilNode2);
            this._inFunctionCall = true;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_propertyRef_in_collectionFunction2187);
            HqlSqlWalker.propertyRef_return propertyRefReturn2 = this.propertyRef();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode8, propertyRefReturn2.Tree);
            this.Resolve(propertyRefReturn2 != null ? (IASTNode) propertyRefReturn2.Tree : (IASTNode) null);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode8);
            astNode2 = astNode6;
            HqlSqlWalker.ProcessFunction(astNode7, this._inSelect);
            this._inFunctionCall = false;
            break;
        }
        collectionFunctionReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return collectionFunctionReturn;
    }

    public HqlSqlWalker.functionCall_return functionCall()
    {
      HqlSqlWalker.functionCall_return functionCallReturn = new HqlSqlWalker.functionCall_return();
      functionCallReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 71:
            num1 = 2;
            break;
          case 81:
            num1 = 1;
            break;
          default:
            throw new NoViableAltException("", 64, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode4 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 81, HqlSqlWalker.FOLLOW_METHOD_CALL_in_functionCall2212));
            IASTNode astNode5 = (IASTNode) this.adaptor.BecomeRoot((object) astNode4, (object) nilNode1);
            this._inFunctionCall = true;
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_pathAsIdent_in_functionCall2217);
            HqlSqlWalker.pathAsIdent_return pathAsIdentReturn = this.pathAsIdent();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode5, pathAsIdentReturn.Tree);
            int num2 = 2;
            if (this.input.LA(1) == 75)
              num2 = 1;
            if (num2 == 1)
            {
              IASTNode astNode6 = (IASTNode) this.input.LT(1);
              IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
              astNode2 = (IASTNode) this.input.LT(1);
              IASTNode astNode7 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 75, HqlSqlWalker.FOLLOW_EXPR_LIST_in_functionCall2222)), (object) nilNode2);
              if (this.input.LA(1) == 2)
              {
                this.Match((IIntStream) this.input, 2, (BitSet) null);
                while (true)
                {
                  int num3 = 4;
                  switch (this.input.LA(1))
                  {
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
                      num3 = 3;
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
                    case 92:
                    case 93:
                    case 95:
                    case 96:
                    case 97:
                    case 98:
                    case 99:
                    case 100:
                    case 105:
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
                      num3 = 1;
                      break;
                    case 52:
                    case 86:
                      num3 = 2;
                      break;
                  }
                  switch (num3)
                  {
                    case 1:
                      astNode2 = (IASTNode) this.input.LT(1);
                      this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_functionCall2225);
                      HqlSqlWalker.expr_return exprReturn = this.expr();
                      --this.state.followingStackPointer;
                      this.adaptor.AddChild((object) astNode7, exprReturn.Tree);
                      continue;
                    case 2:
                      astNode2 = (IASTNode) this.input.LT(1);
                      this.PushFollow(HqlSqlWalker.FOLLOW_query_in_functionCall2229);
                      HqlSqlWalker.query_return queryReturn = this.query();
                      --this.state.followingStackPointer;
                      this.adaptor.AddChild((object) astNode7, queryReturn.Tree);
                      continue;
                    case 3:
                      astNode2 = (IASTNode) this.input.LT(1);
                      this.PushFollow(HqlSqlWalker.FOLLOW_comparisonExpr_in_functionCall2233);
                      HqlSqlWalker.comparisonExpr_return comparisonExprReturn = this.comparisonExpr();
                      --this.state.followingStackPointer;
                      this.adaptor.AddChild((object) astNode7, comparisonExprReturn.Tree);
                      continue;
                    default:
                      goto label_19;
                  }
                }
label_19:
                this.Match((IIntStream) this.input, 3, (BitSet) null);
              }
              this.adaptor.AddChild((object) astNode5, (object) astNode7);
              astNode2 = astNode6;
            }
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode5);
            astNode2 = astNode3;
            HqlSqlWalker.ProcessFunction(astNode4, this._inSelect);
            this._inFunctionCall = false;
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode8 = (IASTNode) this.input.LT(1);
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode9 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 71, HqlSqlWalker.FOLLOW_AGGREGATE_in_functionCall2252)), (object) nilNode3);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_aggregateExpr_in_functionCall2254);
            HqlSqlWalker.aggregateExpr_return aggregateExprReturn = this.aggregateExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode9, aggregateExprReturn.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) astNode9);
            astNode2 = astNode8;
            break;
        }
        functionCallReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return functionCallReturn;
    }

    public HqlSqlWalker.constant_return constant()
    {
      HqlSqlWalker.constant_return constantReturn = new HqlSqlWalker.constant_return();
      constantReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 20:
            num = 4;
            break;
          case 39:
            num = 2;
            break;
          case 51:
            num = 3;
            break;
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 124:
            num = 1;
            break;
          case 100:
            num = 5;
            break;
          default:
            throw new NoViableAltException("", 65, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_literal_in_constant2267);
            HqlSqlWalker.literal_return literalReturn = this.literal();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, literalReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode child1 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 39, HqlSqlWalker.FOLLOW_NULL_in_constant2272));
            this.adaptor.AddChild((object) astNode1, (object) child1);
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode4 = (IASTNode) this.input.LT(1);
            IASTNode astNode5 = (IASTNode) this.Match((IIntStream) this.input, 51, HqlSqlWalker.FOLLOW_TRUE_in_constant2279);
            IASTNode child2 = (IASTNode) this.adaptor.DupNode((object) astNode5);
            this.adaptor.AddChild((object) astNode1, (object) child2);
            this.ProcessBool(astNode5);
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode6 = (IASTNode) this.input.LT(1);
            IASTNode astNode7 = (IASTNode) this.Match((IIntStream) this.input, 20, HqlSqlWalker.FOLLOW_FALSE_in_constant2289);
            IASTNode child3 = (IASTNode) this.adaptor.DupNode((object) astNode7);
            this.adaptor.AddChild((object) astNode1, (object) child3);
            this.ProcessBool(astNode7);
            break;
          case 5:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode8 = (IASTNode) this.input.LT(1);
            IASTNode child4 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 100, HqlSqlWalker.FOLLOW_JAVA_CONSTANT_in_constant2296));
            this.adaptor.AddChild((object) astNode1, (object) child4);
            break;
        }
        constantReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return constantReturn;
    }

    public HqlSqlWalker.literal_return literal()
    {
      HqlSqlWalker.literal_return literalReturn = new HqlSqlWalker.literal_return();
      literalReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 >= 95 && num1 <= 99)
        {
          num2 = 1;
        }
        else
        {
          if (num1 != 124)
            throw new NoViableAltException("", 66, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_numericLiteral_in_literal2307);
            HqlSqlWalker.numericLiteral_return numericLiteralReturn = this.numericLiteral();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, numericLiteralReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_stringLiteral_in_literal2312);
            HqlSqlWalker.stringLiteral_return stringLiteralReturn = this.stringLiteral();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, stringLiteralReturn.Tree);
            break;
        }
        literalReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return literalReturn;
    }

    public HqlSqlWalker.numericLiteral_return numericLiteral()
    {
      HqlSqlWalker.numericLiteral_return numericLiteralReturn = new HqlSqlWalker.numericLiteral_return();
      numericLiteralReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        IASTNode treeNode = (IASTNode) this.input.LT(1);
        if (this.input.LA(1) < 95 || this.input.LA(1) > 99)
          throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
        this.input.Consume();
        IASTNode child = (IASTNode) this.adaptor.DupNode((object) treeNode);
        this.adaptor.AddChild((object) nilNode, (object) child);
        this.state.errorRecovery = false;
        numericLiteralReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.ProcessNumericLiteral((IASTNode) numericLiteralReturn.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return numericLiteralReturn;
    }

    public HqlSqlWalker.stringLiteral_return stringLiteral()
    {
      HqlSqlWalker.stringLiteral_return stringLiteralReturn = new HqlSqlWalker.stringLiteral_return();
      stringLiteralReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        IASTNode child = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 124, HqlSqlWalker.FOLLOW_QUOTED_String_in_stringLiteral2359));
        this.adaptor.AddChild((object) nilNode, (object) child);
        stringLiteralReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return stringLiteralReturn;
    }

    public HqlSqlWalker.identifier_return identifier()
    {
      HqlSqlWalker.identifier_return identifierReturn = new HqlSqlWalker.identifier_return();
      identifierReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        IASTNode treeNode = (IASTNode) this.input.LT(1);
        if (this.input.LA(1) != 93 && this.input.LA(1) != 125)
          throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
        this.input.Consume();
        IASTNode child = (IASTNode) this.adaptor.DupNode((object) treeNode);
        this.adaptor.AddChild((object) nilNode, (object) child);
        this.state.errorRecovery = false;
        identifierReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return identifierReturn;
    }

    public HqlSqlWalker.addrExpr_return addrExpr(bool root)
    {
      HqlSqlWalker.addrExpr_return addrExprReturn = new HqlSqlWalker.addrExpr_return();
      addrExprReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 15:
            num = 1;
            break;
          case 78:
            num = 2;
            break;
          case 93:
          case 125:
            num = 3;
            break;
          default:
            throw new NoViableAltException("", 67, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_addrExprDot_in_addrExpr2389);
            HqlSqlWalker.addrExprDot_return addrExprDotReturn = this.addrExprDot(root);
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, addrExprDotReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_addrExprIndex_in_addrExpr2396);
            HqlSqlWalker.addrExprIndex_return addrExprIndexReturn = this.addrExprIndex(root);
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, addrExprIndexReturn.Tree);
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode4 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_addrExprIdent_in_addrExpr2403);
            HqlSqlWalker.addrExprIdent_return addrExprIdentReturn = this.addrExprIdent(root);
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, addrExprIdentReturn.Tree);
            break;
        }
        addrExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return addrExprReturn;
    }

    public HqlSqlWalker.addrExprDot_return addrExprDot(bool root)
    {
      HqlSqlWalker.addrExprDot_return addrExprDotReturn = new HqlSqlWalker.addrExprDot_return();
      addrExprDotReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token DOT");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule propertyName");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule addrExprLhs");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.Match((IIntStream) this.input, 15, HqlSqlWalker.FOLLOW_DOT_in_addrExprDot2427);
        rewriteRuleNodeStream1.Add((object) astNode3);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_addrExprLhs_in_addrExprDot2431);
        HqlSqlWalker.addrExprLhs_return addrExprLhsReturn = this.addrExprLhs();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(addrExprLhsReturn.Tree);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_propertyName_in_addrExprDot2435);
        HqlSqlWalker.propertyName_return propertyNameReturn = this.propertyName();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(propertyNameReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode2;
        addrExprDotReturn.Tree = (object) t;
        RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token d", (object) astNode3);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", addrExprDotReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule rhs", propertyNameReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule lhs", addrExprLhsReturn?.Tree);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleNodeStream2.NextNode(), (object) nilNode3);
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream5.NextTree());
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream4.NextTree());
        this.adaptor.AddChild((object) nilNode2, (object) astNode4);
        addrExprDotReturn.Tree = (object) nilNode2;
        addrExprDotReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode2);
        this.LookupProperty((IASTNode) addrExprDotReturn.Tree, root, false);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return addrExprDotReturn;
    }

    public HqlSqlWalker.addrExprIndex_return addrExprIndex(bool root)
    {
      HqlSqlWalker.addrExprIndex_return addrExprIndexReturn = new HqlSqlWalker.addrExprIndex_return();
      addrExprIndexReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token INDEX_OP");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule addrExprLhs");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule expr");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.Match((IIntStream) this.input, 78, HqlSqlWalker.FOLLOW_INDEX_OP_in_addrExprIndex2474);
        rewriteRuleNodeStream1.Add((object) astNode3);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_addrExprLhs_in_addrExprIndex2478);
        HqlSqlWalker.addrExprLhs_return addrExprLhsReturn = this.addrExprLhs();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(addrExprLhsReturn.Tree);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_expr_in_addrExprIndex2482);
        HqlSqlWalker.expr_return exprReturn = this.expr();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(exprReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode2;
        addrExprIndexReturn.Tree = (object) t;
        RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token i", (object) astNode3);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", addrExprIndexReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule rhs2", exprReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule lhs2", addrExprLhsReturn?.Tree);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleNodeStream2.NextNode(), (object) nilNode3);
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream5.NextTree());
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream4.NextTree());
        this.adaptor.AddChild((object) nilNode2, (object) astNode4);
        addrExprIndexReturn.Tree = (object) nilNode2;
        addrExprIndexReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode2);
        HqlSqlWalker.ProcessIndex((IASTNode) addrExprIndexReturn.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return addrExprIndexReturn;
    }

    public HqlSqlWalker.addrExprIdent_return addrExprIdent(bool root)
    {
      HqlSqlWalker.addrExprIdent_return addrExprIdentReturn = new HqlSqlWalker.addrExprIdent_return();
      addrExprIdentReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule identifier");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_addrExprIdent2514);
        HqlSqlWalker.identifier_return identifierReturn = this.identifier();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(identifierReturn.Tree);
        addrExprIdentReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", addrExprIdentReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        if (this.IsNonQualifiedPropertyRef(identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null))
        {
          IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
          IASTNode child = (IASTNode) this.adaptor.BecomeRoot((object) this.LookupNonQualifiedProperty(identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null), (object) nilNode2);
          this.adaptor.AddChild((object) nilNode1, (object) child);
        }
        else
        {
          IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
          IASTNode child = (IASTNode) this.adaptor.BecomeRoot((object) this.Resolve(identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null), (object) nilNode3);
          this.adaptor.AddChild((object) nilNode1, (object) child);
        }
        addrExprIdentReturn.Tree = (object) nilNode1;
        addrExprIdentReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return addrExprIdentReturn;
    }

    public HqlSqlWalker.addrExprLhs_return addrExprLhs()
    {
      HqlSqlWalker.addrExprLhs_return addrExprLhsReturn = new HqlSqlWalker.addrExprLhs_return();
      addrExprLhsReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_addrExpr_in_addrExprLhs2542);
        HqlSqlWalker.addrExpr_return addrExprReturn = this.addrExpr(false);
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, addrExprReturn.Tree);
        addrExprLhsReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return addrExprLhsReturn;
    }

    public HqlSqlWalker.propertyName_return propertyName()
    {
      HqlSqlWalker.propertyName_return propertyNameReturn = new HqlSqlWalker.propertyName_return();
      propertyNameReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 11:
            num = 2;
            break;
          case 17:
            num = 3;
            break;
          case 27:
            num = 4;
            break;
          case 93:
          case 125:
            num = 1;
            break;
          default:
            throw new NoViableAltException("", 68, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_propertyName2555);
            HqlSqlWalker.identifier_return identifierReturn = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, identifierReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            IASTNode child1 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 11, HqlSqlWalker.FOLLOW_CLASS_in_propertyName2560));
            this.adaptor.AddChild((object) astNode1, (object) child1);
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode4 = (IASTNode) this.input.LT(1);
            IASTNode child2 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 17, HqlSqlWalker.FOLLOW_ELEMENTS_in_propertyName2565));
            this.adaptor.AddChild((object) astNode1, (object) child2);
            break;
          case 4:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode5 = (IASTNode) this.input.LT(1);
            IASTNode child3 = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 27, HqlSqlWalker.FOLLOW_INDICES_in_propertyName2570));
            this.adaptor.AddChild((object) astNode1, (object) child3);
            break;
        }
        propertyNameReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return propertyNameReturn;
    }

    public HqlSqlWalker.propertyRef_return propertyRef()
    {
      HqlSqlWalker.propertyRef_return propertyRefReturn = new HqlSqlWalker.propertyRef_return();
      propertyRefReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 15:
            num = 1;
            break;
          case 93:
          case 125:
            num = 2;
            break;
          default:
            throw new NoViableAltException("", 69, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_propertyRefPath_in_propertyRef2582);
            HqlSqlWalker.propertyRefPath_return propertyRefPathReturn = this.propertyRefPath();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, propertyRefPathReturn.Tree);
            break;
          case 2:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_propertyRefIdent_in_propertyRef2587);
            HqlSqlWalker.propertyRefIdent_return propertyRefIdentReturn = this.propertyRefIdent();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, propertyRefIdentReturn.Tree);
            break;
        }
        propertyRefReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return propertyRefReturn;
    }

    public HqlSqlWalker.propertyRefPath_return propertyRefPath()
    {
      HqlSqlWalker.propertyRefPath_return propertyRefPathReturn = new HqlSqlWalker.propertyRefPath_return();
      propertyRefPathReturn.Start = this.input.LT(1);
      IASTNode t = (IASTNode) null;
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token DOT");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule propertyName");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule propertyRefLhs");
      try
      {
        IASTNode astNode2 = (IASTNode) this.input.LT(1);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        astNode1 = (IASTNode) this.input.LT(1);
        IASTNode astNode3 = (IASTNode) this.Match((IIntStream) this.input, 15, HqlSqlWalker.FOLLOW_DOT_in_propertyRefPath2607);
        rewriteRuleNodeStream1.Add((object) astNode3);
        this.Match((IIntStream) this.input, 2, (BitSet) null);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_propertyRefLhs_in_propertyRefPath2611);
        HqlSqlWalker.propertyRefLhs_return propertyRefLhsReturn = this.propertyRefLhs();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(propertyRefLhsReturn.Tree);
        astNode1 = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_propertyName_in_propertyRefPath2615);
        HqlSqlWalker.propertyName_return propertyNameReturn = this.propertyName();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(propertyNameReturn.Tree);
        this.Match((IIntStream) this.input, 3, (BitSet) null);
        this.adaptor.AddChild((object) t, (object) nilNode1);
        astNode1 = astNode2;
        propertyRefPathReturn.Tree = (object) t;
        RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token d", (object) astNode3);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", propertyRefPathReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule rhs", propertyNameReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule lhs", propertyRefLhsReturn?.Tree);
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleNodeStream2.NextNode(), (object) nilNode3);
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream5.NextTree());
        this.adaptor.AddChild((object) astNode4, ruleSubtreeStream4.NextTree());
        this.adaptor.AddChild((object) nilNode2, (object) astNode4);
        propertyRefPathReturn.Tree = (object) nilNode2;
        propertyRefPathReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode2);
        propertyRefPathReturn.Tree = (object) this.LookupProperty((IASTNode) propertyRefPathReturn.Tree, false, true);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return propertyRefPathReturn;
    }

    public HqlSqlWalker.propertyRefIdent_return propertyRefIdent()
    {
      HqlSqlWalker.propertyRefIdent_return propertyRefIdentReturn = new HqlSqlWalker.propertyRefIdent_return();
      propertyRefIdentReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_propertyRefIdent2652);
        HqlSqlWalker.identifier_return identifierReturn = this.identifier();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, identifierReturn.Tree);
        propertyRefIdentReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        if (this.IsNonQualifiedPropertyRef(identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null))
        {
          propertyRefIdentReturn.Tree = (object) this.LookupNonQualifiedProperty(identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null);
        }
        else
        {
          this.Resolve(identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null);
          propertyRefIdentReturn.Tree = identifierReturn != null ? (object) (IASTNode) identifierReturn.Tree : (object) (IASTNode) null;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return propertyRefIdentReturn;
    }

    public HqlSqlWalker.propertyRefLhs_return propertyRefLhs()
    {
      HqlSqlWalker.propertyRefLhs_return propertyRefLhsReturn = new HqlSqlWalker.propertyRefLhs_return();
      propertyRefLhsReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_propertyRef_in_propertyRefLhs2664);
        HqlSqlWalker.propertyRef_return propertyRefReturn = this.propertyRef();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, propertyRefReturn.Tree);
        propertyRefLhsReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return propertyRefLhsReturn;
    }

    public HqlSqlWalker.aliasRef_return aliasRef()
    {
      HqlSqlWalker.aliasRef_return aliasRefReturn = new HqlSqlWalker.aliasRef_return();
      aliasRefReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_aliasRef2685);
        HqlSqlWalker.identifier_return identifierReturn = this.identifier();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, identifierReturn.Tree);
        aliasRefReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.LookupAlias((IASTNode) aliasRefReturn.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return aliasRefReturn;
    }

    public HqlSqlWalker.parameter_return parameter()
    {
      HqlSqlWalker.parameter_return parameterReturn = new HqlSqlWalker.parameter_return();
      parameterReturn.Start = this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IASTNode astNode2 = (IASTNode) null;
      IASTNode astNode3 = (IASTNode) null;
      RewriteRuleNodeStream rewriteRuleNodeStream1 = new RewriteRuleNodeStream(this.adaptor, "token COLON");
      RewriteRuleNodeStream rewriteRuleNodeStream2 = new RewriteRuleNodeStream(this.adaptor, "token PARAM");
      RewriteRuleNodeStream rewriteRuleNodeStream3 = new RewriteRuleNodeStream(this.adaptor, "token NUM_INT");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule identifier");
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 105:
            num1 = 1;
            break;
          case 106:
            num1 = 2;
            break;
          default:
            throw new NoViableAltException("", 71, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            IASTNode astNode4 = (IASTNode) this.input.LT(1);
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode5 = (IASTNode) this.Match((IIntStream) this.input, 105, HqlSqlWalker.FOLLOW_COLON_in_parameter2703);
            rewriteRuleNodeStream1.Add((object) astNode5);
            this.Match((IIntStream) this.input, 2, (BitSet) null);
            astNode2 = (IASTNode) this.input.LT(1);
            this.PushFollow(HqlSqlWalker.FOLLOW_identifier_in_parameter2707);
            HqlSqlWalker.identifier_return identifierReturn = this.identifier();
            --this.state.followingStackPointer;
            ruleSubtreeStream1.Add(identifierReturn.Tree);
            this.Match((IIntStream) this.input, 3, (BitSet) null);
            this.adaptor.AddChild((object) astNode1, (object) nilNode1);
            astNode2 = astNode4;
            parameterReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", parameterReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child1 = (IASTNode) this.adaptor.BecomeRoot((object) this.GenerateNamedParameter(astNode5, identifierReturn != null ? (IASTNode) identifierReturn.Tree : (IASTNode) null), (object) nilNode2);
            this.adaptor.AddChild((object) astNode1, (object) child1);
            parameterReturn.Tree = (object) astNode1;
            break;
          case 2:
            IASTNode astNode6 = (IASTNode) this.input.LT(1);
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            astNode2 = (IASTNode) this.input.LT(1);
            IASTNode astNode7 = (IASTNode) this.Match((IIntStream) this.input, 106, HqlSqlWalker.FOLLOW_PARAM_in_parameter2728);
            rewriteRuleNodeStream2.Add((object) astNode7);
            if (this.input.LA(1) == 2)
            {
              this.Match((IIntStream) this.input, 2, (BitSet) null);
              int num2 = 2;
              if (this.input.LA(1) == 95)
                num2 = 1;
              if (num2 == 1)
              {
                astNode2 = (IASTNode) this.input.LT(1);
                astNode3 = (IASTNode) this.Match((IIntStream) this.input, 95, HqlSqlWalker.FOLLOW_NUM_INT_in_parameter2733);
                rewriteRuleNodeStream3.Add((object) astNode3);
              }
              this.Match((IIntStream) this.input, 3, (BitSet) null);
            }
            this.adaptor.AddChild((object) astNode1, (object) nilNode3);
            astNode2 = astNode6;
            parameterReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", parameterReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            if (astNode3 != null)
            {
              IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
              IASTNode child2 = (IASTNode) this.adaptor.BecomeRoot((object) this.GenerateNamedParameter(astNode7, astNode3), (object) nilNode4);
              this.adaptor.AddChild((object) astNode1, (object) child2);
            }
            else
            {
              IASTNode nilNode5 = (IASTNode) this.adaptor.GetNilNode();
              IASTNode child3 = (IASTNode) this.adaptor.BecomeRoot((object) this.GeneratePositionalParameter(astNode7), (object) nilNode5);
              this.adaptor.AddChild((object) astNode1, (object) child3);
            }
            parameterReturn.Tree = (object) astNode1;
            break;
        }
        parameterReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return parameterReturn;
    }

    public HqlSqlWalker.numericInteger_return numericInteger()
    {
      HqlSqlWalker.numericInteger_return numericIntegerReturn = new HqlSqlWalker.numericInteger_return();
      numericIntegerReturn.Start = this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.input.LT(1);
        IASTNode child = (IASTNode) this.adaptor.DupNode((object) (IASTNode) this.Match((IIntStream) this.input, 95, HqlSqlWalker.FOLLOW_NUM_INT_in_numericInteger2766));
        this.adaptor.AddChild((object) nilNode, (object) child);
        numericIntegerReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
      }
      return numericIntegerReturn;
    }

    private void InitializeCyclicDFAs()
    {
    }

    public HqlSqlWalker(
      QueryTranslatorImpl qti,
      ISessionFactoryImplementor sfi,
      ITreeNodeStream input,
      IDictionary<string, string> tokenReplacements,
      string collectionRole)
      : this(input)
    {
      this._sessionFactoryHelper = new SessionFactoryHelperExtensions(sfi);
      this._qti = qti;
      this._literalProcessor = new LiteralProcessor(this);
      this._tokenReplacements = tokenReplacements;
      this._collectionFilterRole = collectionRole;
    }

    public override void ReportError(RecognitionException e)
    {
      this._parseErrorHandler.ReportError(e);
    }

    public IList<AssignmentSpecification> AssignmentSpecifications
    {
      get => (IList<AssignmentSpecification>) this.assignmentSpecifications;
    }

    public int NumberOfParametersInSetClause => this.numberOfParametersInSetClause;

    public IParseErrorHandler ParseErrorHandler
    {
      get => this._parseErrorHandler;
      set => this._parseErrorHandler = value;
    }

    public AliasGenerator AliasGenerator => this._aliasGenerator;

    public Set<string> QuerySpaces => this._querySpaces;

    public IDictionary<string, object> NamedParameters
    {
      get => (IDictionary<string, object>) this._namedParameters;
    }

    internal SessionFactoryHelperExtensions SessionFactoryHelper => this._sessionFactoryHelper;

    public int CurrentStatementType => this._currentStatementType;

    public JoinType ImpliedJoinType => this._impliedJoinType;

    public string[] ReturnAliases => this._selectClause.QueryReturnAliases;

    public IType[] ReturnTypes => this._selectClause.QueryReturnTypes;

    public string CollectionFilterRole => this._collectionFilterRole;

    public SelectClause SelectClause => this._selectClause;

    public IList<IParameterSpecification> Parameters
    {
      get => (IList<IParameterSpecification>) this._parameters;
    }

    private void BeforeStatement(string statementName, int statementType)
    {
      this._inFunctionCall = false;
      ++this._level;
      if (this._level == 1)
      {
        this._statementTypeName = statementName;
        this._statementType = statementType;
      }
      this._currentStatementType = statementType;
      if (!HqlSqlWalker.log.IsDebugEnabled)
        return;
      HqlSqlWalker.log.Debug((object) (statementName + " << begin [level=" + (object) this._level + ", statement=" + this._statementTypeName + "]"));
    }

    private void BeforeStatementCompletion(string statementName)
    {
      if (!HqlSqlWalker.log.IsDebugEnabled)
        return;
      HqlSqlWalker.log.Debug((object) (statementName + " : finishing up [level=" + (object) this._level + ", statement=" + this._statementTypeName + "]"));
    }

    private void PrepareVersioned(IASTNode updateNode, IASTNode versioned)
    {
      UpdateStatement updateStatement = (UpdateStatement) updateNode;
      FromClause fromClause = updateStatement.FromClause;
      if (versioned == null)
        return;
      IQueryable queryable = fromClause.GetFromElement().Queryable;
      IVersionType type = ((IEntityPersister) queryable).IsVersioned ? queryable.VersionType : throw new SemanticException("increment option specified for update of non-versioned entity");
      if (type is IUserVersionType)
        throw new SemanticException("user-defined version types not supported for increment option");
      IASTNode node1 = this.ASTFactory.CreateNode(102, "=");
      IASTNode versionPropertyNode = this.GenerateVersionPropertyNode(queryable);
      node1.SetFirstChild(versionPropertyNode);
      IASTNode node2;
      if (typeof (DateTime).IsAssignableFrom(type.ReturnedClass))
      {
        node2 = this.ASTFactory.CreateNode(106, "?");
        IParameterSpecification parameterSpecification = (IParameterSpecification) new VersionTypeSeedParameterSpecification(type);
        ((ParameterNode) node2).HqlParameterSpecification = parameterSpecification;
        this.Parameters.Insert(0, parameterSpecification);
      }
      else
      {
        node2 = this.ASTFactory.CreateNode(118, "+");
        node2.SetFirstChild(this.GenerateVersionPropertyNode(queryable));
        node2.AddChild(this.ASTFactory.CreateNode(125, "1"));
      }
      node1.AddChild(node2);
      this.EvaluateAssignment(node1, queryable, 0);
      IASTNode setClause = updateStatement.SetClause;
      IASTNode firstChild = setClause.GetFirstChild();
      setClause.SetFirstChild(node1);
      node1.NextSibling = firstChild;
    }

    private IASTNode GenerateVersionPropertyNode(IQueryable persister)
    {
      IASTNode node = this.LookupNonQualifiedProperty(this.ASTFactory.CreateNode(125, persister.PropertyNames[persister.VersionProperty]));
      this.Resolve(node);
      return node;
    }

    private void PostProcessUpdate(IASTNode update)
    {
      this.PostProcessDML((IRestrictableStatement) update);
    }

    private void PostProcessDelete(IASTNode delete)
    {
      this.PostProcessDML((IRestrictableStatement) delete);
    }

    private void PostProcessInsert(IASTNode insert)
    {
      InsertStatement insertStatement = (InsertStatement) insert;
      insertStatement.Validate();
      SelectClause selectClause = insertStatement.SelectClause;
      IQueryable queryable = insertStatement.IntoClause.Queryable;
      if (!insertStatement.IntoClause.IsExplicitIdInsertion)
      {
        IIdentifierGenerator identifierGenerator = queryable.IdentifierGenerator;
        if (!HqlSqlWalker.SupportsIdGenWithBulkInsertion(identifierGenerator))
          throw new QueryException("can only generate ids as part of bulk insert with either sequence or post-insert style generators");
        IASTNode newChild = (IASTNode) null;
        if (identifierGenerator is SequenceGenerator sequenceGenerator)
          newChild = this.ASTFactory.CreateNode(143, this.SessionFactoryHelper.Factory.Dialect.GetSelectSequenceNextValString(sequenceGenerator.GeneratorKey()));
        if (newChild != null)
        {
          IASTNode firstChild = selectClause.GetFirstChild();
          selectClause.SetFirstChild(newChild);
          newChild.NextSibling = firstChild;
          insertStatement.IntoClause.PrependIdColumnSpec();
        }
      }
      if (((IEntityPersister) queryable).IsVersioned && !insertStatement.IntoClause.IsExplicitVersionInsertion && queryable.VersionPropertyInsertable)
      {
        IVersionType versionType = queryable.VersionType;
        IASTNode node;
        if (this.SessionFactoryHelper.Factory.Dialect.SupportsParametersInInsertSelect)
        {
          node = this.ASTFactory.CreateNode(106, "?");
          IParameterSpecification parameterSpecification = (IParameterSpecification) new VersionTypeSeedParameterSpecification(versionType);
          ((ParameterNode) node).HqlParameterSpecification = parameterSpecification;
          this._parameters.Insert(0, parameterSpecification);
        }
        else if (HqlSqlWalker.IsIntegral((IType) versionType))
        {
          try
          {
            node = this.ASTFactory.CreateNode(143, versionType.Seed((ISessionImplementor) null).ToString());
          }
          catch (Exception ex)
          {
            throw new QueryException("could not determine seed value for version on bulk insert [" + (object) versionType + "]", ex);
          }
        }
        else
        {
          if (!HqlSqlWalker.IsDatabaseGeneratedTimestamp((IType) versionType))
            throw new QueryException("cannot handle version type [" + (object) versionType + "] on bulk inserts with dialects not supporting parameters in insert-select statements");
          node = this.ASTFactory.CreateNode(143, this.SessionFactoryHelper.Factory.Dialect.CurrentTimestampSQLFunctionName);
        }
        IASTNode firstChild = selectClause.GetFirstChild();
        selectClause.SetFirstChild(node);
        node.NextSibling = firstChild;
        insertStatement.IntoClause.PrependVersionColumnSpec();
      }
      if (!insertStatement.IntoClause.IsDiscriminated)
        return;
      IASTNode node1 = this.ASTFactory.CreateNode(143, insertStatement.IntoClause.Queryable.DiscriminatorSQLValue);
      insertStatement.SelectClause.AddChild(node1);
    }

    private static bool IsDatabaseGeneratedTimestamp(IType type)
    {
      return typeof (TimestampType).IsAssignableFrom(type.GetType());
    }

    private static bool IsIntegral(IType type)
    {
      return typeof (long).IsAssignableFrom(type.ReturnedClass) || typeof (int).IsAssignableFrom(type.ReturnedClass) || typeof (short).IsAssignableFrom(type.ReturnedClass);
    }

    public static bool SupportsIdGenWithBulkInsertion(IIdentifierGenerator generator)
    {
      return typeof (SequenceGenerator).IsAssignableFrom(generator.GetType()) || typeof (IPostInsertIdentifierGenerator).IsAssignableFrom(generator.GetType());
    }

    private void PostProcessDML(IRestrictableStatement statement)
    {
      statement.FromClause.Resolve();
      FromElement fromElement = (FromElement) statement.FromClause.GetFromElements()[0];
      IQueryable queryable = fromElement.Queryable;
      fromElement.Text = queryable.TableName;
      if (queryable.DiscriminatorType == null)
        return;
      new SyntheticAndFactory(this).AddDiscriminatorWhereFragment(statement, queryable, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>(), fromElement.TableAlias);
    }

    private void AfterStatementCompletion(string statementName)
    {
      if (HqlSqlWalker.log.IsDebugEnabled)
        HqlSqlWalker.log.Debug((object) (statementName + " >> end [level=" + (object) this._level + ", statement=" + this._statementTypeName + "]"));
      --this._level;
    }

    private void HandleClauseStart(int clauseType) => this._currentClauseType = clauseType;

    private IASTNode CreateIntoClause(string path, IASTNode propertySpec)
    {
      IQueryable persister = (IQueryable) this.SessionFactoryHelper.RequireClassPersister(path);
      IntoClause intoClause = (IntoClause) this.adaptor.Create(30, "into");
      intoClause.SetFirstChild(propertySpec);
      intoClause.Initialize(persister);
      this.AddQuerySpaces(persister.QuerySpaces);
      return (IASTNode) intoClause;
    }

    private IASTNode Resolve(IASTNode node)
    {
      if (node != null)
      {
        IResolvableNode resolvableNode = (IResolvableNode) node;
        if (this._inFunctionCall)
          resolvableNode.ResolveInFunctionCall(false, true);
        else
          resolvableNode.Resolve(false, true);
      }
      return node;
    }

    private void ProcessQuery(IASTNode select, IASTNode query)
    {
      if (HqlSqlWalker.log.IsDebugEnabled)
        HqlSqlWalker.log.Debug((object) ("processQuery() : " + query.ToStringTree()));
      try
      {
        QueryNode queryNode = (QueryNode) query;
        if (select == null || select.ChildCount <= 0)
          this.CreateSelectClauseFromFromClause((IASTNode) queryNode);
        else
          this.UseSelectClause(select);
        new JoinProcessor(this).ProcessJoins(queryNode);
        foreach (FromElement projection in (IEnumerable<IASTNode>) queryNode.FromClause.GetProjectionList())
        {
          if (projection.IsFetch && projection.QueryableCollection != null)
          {
            if (projection.QueryableCollection.HasOrdering)
            {
              string sqlOrderByString = projection.QueryableCollection.GetSQLOrderByString(projection.TableAlias);
              queryNode.GetOrderByClause().AddOrderFragment(sqlOrderByString);
            }
            if (projection.QueryableCollection.HasManyToManyOrdering)
            {
              string manyOrderByString = projection.QueryableCollection.GetManyToManyOrderByString(projection.TableAlias);
              queryNode.GetOrderByClause().AddOrderFragment(manyOrderByString);
            }
          }
        }
      }
      finally
      {
        this.PopFromClause();
      }
    }

    private void UseSelectClause(IASTNode select)
    {
      this._selectClause = (SelectClause) select;
      this._selectClause.InitializeExplicitSelectClause(this._currentFromClause);
    }

    private void CreateSelectClauseFromFromClause(IASTNode qn)
    {
      qn.InsertChild(0, (IASTNode) this.adaptor.Create(138, "{derived select clause}"));
      this._selectClause = (SelectClause) qn.GetChild(0);
      this._selectClause.InitializeDerivedSelectClause(this._currentFromClause);
      if (!HqlSqlWalker.log.IsDebugEnabled)
        return;
      HqlSqlWalker.log.Debug((object) "Derived SELECT clause created.");
    }

    private void PopFromClause()
    {
      this._currentFromClause = this._currentFromClause.ParentFromClause;
    }

    private static void ProcessConstructor(IASTNode constructor)
    {
      ((ConstructorNode) constructor).Prepare();
    }

    protected void EvaluateAssignment(IASTNode eq)
    {
      HqlSqlWalker.PrepareLogicOperator(eq);
      IQueryable queryable = this.CurrentFromClause.GetFromElement().Queryable;
      this.EvaluateAssignment(eq, queryable, -1);
    }

    private void EvaluateAssignment(IASTNode eq, IQueryable persister, int targetIndex)
    {
      if (!persister.IsMultiTable)
        return;
      AssignmentSpecification assignmentSpecification = new AssignmentSpecification(eq, persister);
      if (targetIndex >= 0)
        this.assignmentSpecifications.Insert(targetIndex, assignmentSpecification);
      else
        this.assignmentSpecifications.Add(assignmentSpecification);
      this.numberOfParametersInSetClause += assignmentSpecification.Parameters.Length;
    }

    private void BeforeSelectClause()
    {
      foreach (FromElement fromElement in (IEnumerable<IASTNode>) this.CurrentFromClause.GetFromElements())
        fromElement.IncludeSubclasses = false;
    }

    private static void SetAlias(IASTNode selectExpr, IASTNode ident)
    {
      ((ISelectExpression) selectExpr).Alias = ident.Text;
    }

    private static void ResolveSelectExpression(IASTNode node)
    {
      switch (node.Type)
      {
        case 15:
          ((DotNode) node).ResolveSelectExpression();
          break;
        case 141:
          FromReferenceNode fromReferenceNode = (FromReferenceNode) node;
          fromReferenceNode.Resolve(false, false);
          FromElement fromElement = fromReferenceNode.FromElement;
          if (fromElement == null)
            break;
          fromElement.IncludeSubclasses = true;
          break;
      }
    }

    private void PrepareFromClauseInputTree(IASTNode fromClauseInput, ITreeNodeStream input)
    {
      if (!this.IsFilter())
        return;
      IQueryableCollection collectionPersister = this._sessionFactoryHelper.GetCollectionPersister(this._collectionFilterRole);
      if (!collectionPersister.ElementType.IsEntityType)
        throw new QueryException("collection of values in filter: this");
      IASTNode child1 = (IASTNode) this.adaptor.Create(76, collectionPersister.ElementPersister.EntityName);
      IASTNode child2 = (IASTNode) this.adaptor.Create(72, "this");
      ((HqlSqlWalkerTreeNodeStream) input).InsertChild(fromClauseInput, child1);
      ((HqlSqlWalkerTreeNodeStream) input).InsertChild(fromClauseInput, child2);
      if (HqlSqlWalker.log.IsDebugEnabled)
        HqlSqlWalker.log.Debug((object) "prepareFromClauseInputTree() : Filter - Added 'this' as a from element...");
      IType keyType = this._sessionFactoryHelper.RequireQueryableCollection(this._collectionFilterRole).KeyType;
      ParameterNode parameterNode = (ParameterNode) this.adaptor.Create(106, "?");
      CollectionFilterKeyParameterSpecification parameterSpecification = new CollectionFilterKeyParameterSpecification(this._collectionFilterRole, keyType, this._positionalParameterCount++);
      parameterNode.HqlParameterSpecification = (IParameterSpecification) parameterSpecification;
      this._parameters.Add((IParameterSpecification) parameterSpecification);
    }

    private void CreateFromJoinElement(
      IASTNode path,
      IASTNode alias,
      int joinType,
      IASTNode fetchNode,
      IASTNode propertyFetch,
      IASTNode with)
    {
      bool flag = fetchNode != null;
      if (flag && this.IsSubQuery)
        throw new QueryException("fetch not allowed in subquery from-elements");
      DotNode dotNode = path.Type == 15 ? (DotNode) path : throw new SemanticException("Path expected for join!");
      JoinType impliedJoinType = this._impliedJoinType;
      dotNode.JoinType = impliedJoinType;
      dotNode.Fetch = flag;
      dotNode.Resolve(true, false, alias == null ? (string) null : alias.Text);
      FromElement fromElement;
      if (dotNode.DataType != null && dotNode.DataType.IsComponentType)
      {
        fromElement = new FromElementFactory(this.CurrentFromClause, dotNode.GetLhs().FromElement, dotNode.PropertyPath, alias == null ? (string) null : alias.Text, (string[]) null, false).CreateComponentJoin((ComponentType) dotNode.DataType);
      }
      else
      {
        fromElement = dotNode.GetImpliedJoin();
        if (fromElement == null)
          throw new InvalidPathException("Invalid join: " + dotNode.Path);
        fromElement.SetAllPropertyFetch(propertyFetch != null);
        if (with != null)
        {
          if (flag)
            throw new SemanticException("with-clause not allowed on fetched associations; use filters");
          this.HandleWithFragment(fromElement, with);
        }
      }
      if (!HqlSqlWalker.log.IsDebugEnabled)
        return;
      HqlSqlWalker.log.Debug((object) ("createFromJoinElement() : " + this._printer.ShowAsString((IASTNode) fromElement, "-- join tree --")));
    }

    private IASTNode CreateFromElement(
      string path,
      IASTNode pathNode,
      IASTNode alias,
      IASTNode propertyFetch)
    {
      FromElement fromElement = this._currentFromClause.AddFromElement(path, alias);
      fromElement.SetAllPropertyFetch(propertyFetch != null);
      return (IASTNode) fromElement;
    }

    private IASTNode CreateFromFilterElement(IASTNode filterEntity, IASTNode alias)
    {
      FromElement fromFilterElement = this._currentFromClause.AddFromElement(filterEntity.Text, alias);
      FromClause fromClause = fromFilterElement.FromClause;
      IQueryableCollection collectionPersister = this._sessionFactoryHelper.GetCollectionPersister(this._collectionFilterRole);
      string[] keyColumnNames = collectionPersister.KeyColumnNames;
      string alias1 = collectionPersister.IsOneToMany ? fromFilterElement.TableAlias : fromClause.AliasGenerator.CreateName(this._collectionFilterRole);
      JoinSequence joinSequence = this._sessionFactoryHelper.CreateJoinSequence();
      joinSequence.SetRoot((IJoinable) collectionPersister, alias1);
      if (!collectionPersister.IsOneToMany)
        joinSequence.AddJoin((IAssociationType) collectionPersister.ElementType, fromFilterElement.TableAlias, JoinType.InnerJoin, collectionPersister.GetElementColumnNames(alias1));
      joinSequence.AddCondition(alias1, keyColumnNames, " = ", true);
      fromFilterElement.JoinSequence = joinSequence;
      fromFilterElement.Filter = true;
      if (HqlSqlWalker.log.IsDebugEnabled)
        HqlSqlWalker.log.Debug((object) "createFromFilterElement() : processed filter FROM element.");
      return (IASTNode) fromFilterElement;
    }

    private void SetImpliedJoinType(int joinType)
    {
      this._impliedJoinType = JoinProcessor.ToHibernateJoinType(joinType);
    }

    private void PushFromClause(IASTNode fromNode)
    {
      FromClause fromClause = (FromClause) fromNode;
      fromClause.SetParentFromClause(this._currentFromClause);
      this._currentFromClause = fromClause;
    }

    private static void PrepareArithmeticOperator(IASTNode op) => ((IOperatorNode) op).Initialize();

    private static void ProcessFunction(IASTNode functionCall, bool inSelect)
    {
      ((MethodNode) functionCall).Resolve(inSelect);
    }

    private void ProcessBool(IASTNode constant) => this._literalProcessor.ProcessBoolean(constant);

    private static void PrepareLogicOperator(IASTNode operatorNode)
    {
      ((IOperatorNode) operatorNode).Initialize();
    }

    private void ProcessNumericLiteral(IASTNode literal)
    {
      this._literalProcessor.ProcessNumericLiteral((SqlNode) literal);
    }

    protected IASTNode LookupProperty(IASTNode dot, bool root, bool inSelect)
    {
      DotNode dotNode = (DotNode) dot;
      FromReferenceNode lhs = dotNode.GetLhs();
      IASTNode nextSibling = lhs.NextSibling;
      switch (nextSibling.Type)
      {
        case 17:
        case 27:
          if (HqlSqlWalker.log.IsDebugEnabled)
            HqlSqlWalker.log.Debug((object) ("lookupProperty() " + dotNode.Path + " => " + nextSibling.Text + "(" + lhs.Path + ")"));
          CollectionFunction newChild = (CollectionFunction) nextSibling;
          newChild.SetFirstChild((IASTNode) lhs);
          lhs.NextSibling = (IASTNode) null;
          dotNode.SetFirstChild((IASTNode) newChild);
          this.Resolve((IASTNode) lhs);
          newChild.Resolve(inSelect);
          return (IASTNode) newChild;
        default:
          dotNode.ResolveFirstChild();
          return (IASTNode) dotNode;
      }
    }

    private static void ProcessIndex(IASTNode indexOp)
    {
      ((FromReferenceNode) indexOp).Resolve(true, true);
    }

    private bool IsNonQualifiedPropertyRef(IASTNode ident)
    {
      string text = ident.Text;
      if (this._currentFromClause.IsFromElementAlias(text))
        return false;
      IList<IASTNode> explicitFromElements = this._currentFromClause.GetExplicitFromElements();
      if (explicitFromElements.Count != 1)
        return false;
      FromElement fromElement = (FromElement) explicitFromElements[0];
      HqlSqlWalker.log.Info((object) ("attempting to resolve property [" + text + "] as a non-qualified ref"));
      return fromElement.GetPropertyMapping(text).TryToType(text, out IType _);
    }

    private IASTNode LookupNonQualifiedProperty(IASTNode property)
    {
      FromElement explicitFromElement = (FromElement) this._currentFromClause.GetExplicitFromElements()[0];
      return this.LookupProperty(this.GenerateSyntheticDotNodeForNonQualifiedPropertyRef(property, explicitFromElement), false, this._currentClauseType == 45);
    }

    private IASTNode GenerateSyntheticDotNodeForNonQualifiedPropertyRef(
      IASTNode property,
      FromElement fromElement)
    {
      IASTNode qualifiedPropertyRef = (IASTNode) this.adaptor.Create(15, "{non-qualified-property-ref}");
      ((DotNode) qualifiedPropertyRef).PropertyPath = ((FromReferenceNode) property).Path;
      IdentNode newChild = (IdentNode) this.adaptor.Create(125, "{synthetic-alias}");
      newChild.FromElement = fromElement;
      newChild.IsResolved = true;
      qualifiedPropertyRef.SetFirstChild((IASTNode) newChild);
      qualifiedPropertyRef.AddChild(property);
      return qualifiedPropertyRef;
    }

    private void LookupAlias(IASTNode aliasRef)
    {
      FromElement fromElement = this._currentFromClause.GetFromElement(aliasRef.Text);
      ((AbstractSelectExpression) aliasRef).FromElement = fromElement;
    }

    private IASTNode GenerateNamedParameter(IASTNode delimiterNode, IASTNode nameNode)
    {
      string text = nameNode.Text;
      this.TrackNamedParameterPositions(text);
      ParameterNode namedParameter = (ParameterNode) this.adaptor.Create(149, text);
      namedParameter.Text = "?";
      NamedParameterSpecification parameterSpecification = new NamedParameterSpecification(delimiterNode.Line, delimiterNode.CharPositionInLine, text);
      namedParameter.HqlParameterSpecification = (IParameterSpecification) parameterSpecification;
      this._parameters.Add((IParameterSpecification) parameterSpecification);
      return (IASTNode) namedParameter;
    }

    private IASTNode GeneratePositionalParameter(IASTNode inputNode)
    {
      if (this._namedParameters.Count > 0)
        throw new SemanticException("cannot define positional parameter after any named parameters have been defined");
      ParameterNode positionalParameter = (ParameterNode) this.adaptor.Create(106, "?");
      PositionalParameterSpecification parameterSpecification = new PositionalParameterSpecification(inputNode.Line, inputNode.CharPositionInLine, this._positionalParameterCount++);
      positionalParameter.HqlParameterSpecification = (IParameterSpecification) parameterSpecification;
      this._parameters.Add((IParameterSpecification) parameterSpecification);
      return (IASTNode) positionalParameter;
    }

    public FromClause CurrentFromClause => this._currentFromClause;

    public int StatementType => this._statementType;

    public LiteralProcessor LiteralProcessor => this._literalProcessor;

    public int CurrentClauseType => this._currentClauseType;

    public IDictionary<string, IFilter> EnabledFilters => this._qti.EnabledFilters;

    public bool IsSubQuery => this._level > 1;

    public bool IsSelectStatement => this._statementType == 45;

    public bool IsInFrom => this._inFrom;

    public bool IsInSelect => this._inSelect;

    public IDictionary<string, string> TokenReplacements => this._tokenReplacements;

    public bool IsComparativeExpressionClause
    {
      get => this.CurrentClauseType == 55 || this.CurrentClauseType == 63 || this.IsInCase;
    }

    public bool IsInCase => this._inCase;

    public bool IsShallowQuery => this.StatementType == 29 || this._qti.IsShallowQuery;

    public FromClause GetFinalFromClause()
    {
      FromClause finalFromClause = this._currentFromClause;
      while (finalFromClause.ParentFromClause != null)
        finalFromClause = finalFromClause.ParentFromClause;
      return finalFromClause;
    }

    public bool IsFilter() => this._collectionFilterRole != null;

    public IASTFactory ASTFactory
    {
      get
      {
        if (this._nodeFactory == null)
          this._nodeFactory = (IASTFactory) new NHibernate.Hql.Ast.ANTLR.Tree.ASTFactory(this.adaptor);
        return this._nodeFactory;
      }
    }

    public void AddQuerySpaces(string[] spaces)
    {
      for (int index = 0; index < spaces.Length; ++index)
        this._querySpaces.Add(spaces[index]);
    }

    private void TrackNamedParameterPositions(string name)
    {
      int num1 = this._parameterCount++;
      object namedParameter = this._namedParameters[name];
      if (namedParameter == null)
        this._namedParameters.Add(name, (object) num1);
      else if (namedParameter is int num2)
      {
        List<int> intList = new List<int>(4) { num2, num1 };
        this._namedParameters[name] = (object) intList;
      }
      else
        ((List<int>) namedParameter).Add(num1);
    }

    private void HandleWithFragment(FromElement fromElement, IASTNode hqlWithNode)
    {
      try
      {
        ITreeNodeStream input = this.input;
        this.input = (ITreeNodeStream) new CommonTreeNodeStream(this.adaptor, (object) hqlWithNode);
        IASTNode tree = (IASTNode) this.withClause().Tree;
        this.input = input;
        if (HqlSqlWalker.log.IsDebugEnabled)
          HqlSqlWalker.log.Debug((object) ("handleWithFragment() : " + this._printer.ShowAsString(tree, "-- with clause --")));
        WithClauseVisitor visitor = new WithClauseVisitor(fromElement);
        new NodeTraverser((IVisitationStrategy) visitor).TraverseDepthFirst(tree);
        if (visitor.GetReferencedFromElement() != fromElement)
          throw new InvalidWithClauseException("with-clause expressions did not reference from-clause element to which the with-clause was associated");
        SqlGenerator sqlGenerator = new SqlGenerator(this._sessionFactoryHelper.Factory, (ITreeNodeStream) new CommonTreeNodeStream(this.adaptor, (object) tree.GetChild(0)));
        sqlGenerator.whereExpr();
        SqlString withClauseFragment = new SqlString(new object[3]
        {
          (object) "(",
          (object) sqlGenerator.GetSQL(),
          (object) ")"
        });
        fromElement.SetWithClauseFragment(visitor.GetJoinAlias(), withClauseFragment);
      }
      catch (SemanticException ex)
      {
        throw;
      }
      catch (InvalidWithClauseException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new SemanticException(ex.Message);
      }
    }

    public class statement_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectStatement_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class updateStatement_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class deleteStatement_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class insertStatement_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class intoClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class insertablePropertySpec_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class setClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class assignment_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class newValue_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class query_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class unionedQuery_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class orderClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class orderExprs_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class skipClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class takeClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class groupClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class havingClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectExprList_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class aliasedSelectExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class count_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class constructor_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class aggregateExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class fromClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class fromElementList_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class fromElement_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class joinElement_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class joinType_return : TreeRuleReturnScope
    {
      public int j;
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class path_return : TreeRuleReturnScope
    {
      public string p;
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class pathAsIdent_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class withClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class whereClause_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class logicalExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class logicalPath_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class comparisonExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class inRhs_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class exprOrSubquery_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class collectionFunctionOrSubselect_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class expr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class arithmeticExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class caseExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class collectionFunction_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class functionCall_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class constant_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class literal_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class numericLiteral_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class stringLiteral_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class identifier_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class addrExpr_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class addrExprDot_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class addrExprIndex_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class addrExprIdent_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class addrExprLhs_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class propertyName_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class propertyRef_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class propertyRefPath_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class propertyRefIdent_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class propertyRefLhs_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class aliasRef_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class parameter_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class numericInteger_return : TreeRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }
  }
}
