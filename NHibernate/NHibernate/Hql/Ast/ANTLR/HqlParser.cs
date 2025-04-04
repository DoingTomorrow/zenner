// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.HqlParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  public class HqlParser : Parser
  {
    public const int EXPONENT = 130;
    public const int LT = 109;
    public const int FLOAT_SUFFIX = 131;
    public const int STAR = 120;
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
    public const int INSERT = 29;
    public const int ESCAPE = 18;
    public const int IS_NULL = 80;
    public const int BOTH = 64;
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
    public const int NUM_LONG = 99;
    public const int ON = 62;
    public const int TRAILING = 70;
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
    public const int LEFT = 33;
    public const int AVG = 9;
    public const int SOME = 48;
    public const int BOR = 115;
    public const int ALL = 4;
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
    public const int ROW_STAR = 88;
    public const int NOT_LIKE = 84;
    public const int HEX_DIGIT = 132;
    public const int NOT_BETWEEN = 82;
    public const int RANGE = 87;
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
    public const int DIV = 121;
    public const int DESCENDING = 14;
    public const int BETWEEN = 10;
    public const int AGGREGATE = 71;
    public const int LE = 111;
    public static readonly string[] tokenNames = new string[135]
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
      "'descending'"
    };
    protected ITreeAdaptor adaptor = (ITreeAdaptor) new CommonTreeAdaptor();
    public static readonly BitSet FOLLOW_updateStatement_in_statement611 = new BitSet(new ulong[1]);
    public static readonly BitSet FOLLOW_deleteStatement_in_statement615 = new BitSet(new ulong[1]);
    public static readonly BitSet FOLLOW_selectStatement_in_statement619 = new BitSet(new ulong[1]);
    public static readonly BitSet FOLLOW_insertStatement_in_statement623 = new BitSet(new ulong[1]);
    public static readonly BitSet FOLLOW_EOF_in_statement627 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_UPDATE_in_updateStatement639 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_VERSIONED_in_updateStatement643 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_optionalFromTokenFromClause_in_updateStatement649 = new BitSet(new ulong[1]
    {
      70368744177664UL
    });
    public static readonly BitSet FOLLOW_setClause_in_updateStatement653 = new BitSet(new ulong[1]
    {
      36028797018963970UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_updateStatement658 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SET_in_setClause672 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_assignment_in_setClause675 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_setClause678 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_assignment_in_setClause681 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_stateField_in_assignment695 = new BitSet(new ulong[2]
    {
      0UL,
      274877906944UL
    });
    public static readonly BitSet FOLLOW_EQ_in_assignment697 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_newValue_in_assignment700 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_path_in_stateField713 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_concatenation_in_newValue726 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_DELETE_in_deleteStatement737 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_optionalFromTokenFromClause_in_deleteStatement743 = new BitSet(new ulong[1]
    {
      36028797018963970UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_deleteStatement749 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_optionalFromTokenFromClause2_in_optionalFromTokenFromClause764 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_optionalFromTokenFromClause766 = new BitSet(new ulong[2]
    {
      18014398513676418UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_asAlias_in_optionalFromTokenFromClause769 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FROM_in_optionalFromTokenFromClause2800 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_queryRule_in_selectStatement814 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_INSERT_in_insertStatement843 = new BitSet(new ulong[1]
    {
      1073741824UL
    });
    public static readonly BitSet FOLLOW_intoClause_in_insertStatement846 = new BitSet(new ulong[1]
    {
      37332817864032256UL
    });
    public static readonly BitSet FOLLOW_selectStatement_in_insertStatement848 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_INTO_in_intoClause859 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_intoClause862 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_insertablePropertySpec_in_intoClause866 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_insertablePropertySpec877 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_primaryExpression_in_insertablePropertySpec879 = new BitSet(new ulong[2]
    {
      0UL,
      1236950581248UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_insertablePropertySpec883 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_primaryExpression_in_insertablePropertySpec885 = new BitSet(new ulong[2]
    {
      0UL,
      1236950581248UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_insertablePropertySpec890 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_selectFrom_in_queryRule916 = new BitSet(new ulong[1]
    {
      37297633487749122UL
    });
    public static readonly BitSet FOLLOW_whereClause_in_queryRule921 = new BitSet(new ulong[1]
    {
      1268836468785154UL
    });
    public static readonly BitSet FOLLOW_groupByClause_in_queryRule928 = new BitSet(new ulong[1]
    {
      1268836452007938UL
    });
    public static readonly BitSet FOLLOW_havingClause_in_queryRule935 = new BitSet(new ulong[1]
    {
      1268836418453506UL
    });
    public static readonly BitSet FOLLOW_orderByClause_in_queryRule942 = new BitSet(new ulong[1]
    {
      1266637395197954UL
    });
    public static readonly BitSet FOLLOW_skipClause_in_queryRule949 = new BitSet(new ulong[1]
    {
      1125899906842626UL
    });
    public static readonly BitSet FOLLOW_takeClause_in_queryRule956 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_selectClause_in_selectFrom974 = new BitSet(new ulong[1]
    {
      4194306UL
    });
    public static readonly BitSet FOLLOW_fromClause_in_selectFrom981 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SELECT_in_selectClause1030 = new BitSet(new ulong[2]
    {
      165226876621230640UL,
      3513940822653403154UL
    });
    public static readonly BitSet FOLLOW_DISTINCT_in_selectClause1042 = new BitSet(new ulong[2]
    {
      165226876621230640UL,
      3513940822653403154UL
    });
    public static readonly BitSet FOLLOW_selectedPropertiesList_in_selectClause1048 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_newExpression_in_selectClause1052 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_selectObject_in_selectClause1056 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_NEW_in_newExpression1070 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_newExpression1072 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_newExpression1077 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_selectedPropertiesList_in_newExpression1079 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_newExpression1081 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_OBJECT_in_selectObject1107 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_selectObject1110 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_identifier_in_selectObject1113 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_selectObject1115 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FROM_in_fromClause1133 = new BitSet(new ulong[2]
    {
      18014398580916352UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_fromRange_in_fromClause1138 = new BitSet(new ulong[2]
    {
      17605347770370UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_fromJoin_in_fromClause1142 = new BitSet(new ulong[2]
    {
      17605347770370UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_fromClause1146 = new BitSet(new ulong[2]
    {
      18014398580916352UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_fromRange_in_fromClause1151 = new BitSet(new ulong[2]
    {
      17605347770370UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_set_in_fromJoin1169 = new BitSet(new ulong[1]
    {
      4402341478400UL
    });
    public static readonly BitSet FOLLOW_OUTER_in_fromJoin1180 = new BitSet(new ulong[1]
    {
      4294967296UL
    });
    public static readonly BitSet FOLLOW_FULL_in_fromJoin1188 = new BitSet(new ulong[1]
    {
      4294967296UL
    });
    public static readonly BitSet FOLLOW_INNER_in_fromJoin1192 = new BitSet(new ulong[1]
    {
      4294967296UL
    });
    public static readonly BitSet FOLLOW_JOIN_in_fromJoin1197 = new BitSet(new ulong[2]
    {
      18014398515773440UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_FETCH_in_fromJoin1201 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_fromJoin1205 = new BitSet(new ulong[2]
    {
      9241386435370549378UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_asAlias_in_fromJoin1208 = new BitSet(new ulong[1]
    {
      9223372036856872962UL
    });
    public static readonly BitSet FOLLOW_propertyFetch_in_fromJoin1213 = new BitSet(new ulong[1]
    {
      9223372036854775810UL
    });
    public static readonly BitSet FOLLOW_withClause_in_fromJoin1218 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_fromJoin1229 = new BitSet(new ulong[1]
    {
      4402341478400UL
    });
    public static readonly BitSet FOLLOW_OUTER_in_fromJoin1240 = new BitSet(new ulong[1]
    {
      4294967296UL
    });
    public static readonly BitSet FOLLOW_FULL_in_fromJoin1248 = new BitSet(new ulong[1]
    {
      4294967296UL
    });
    public static readonly BitSet FOLLOW_INNER_in_fromJoin1252 = new BitSet(new ulong[1]
    {
      4294967296UL
    });
    public static readonly BitSet FOLLOW_JOIN_in_fromJoin1257 = new BitSet(new ulong[1]
    {
      2228224UL
    });
    public static readonly BitSet FOLLOW_FETCH_in_fromJoin1261 = new BitSet(new ulong[1]
    {
      131072UL
    });
    public static readonly BitSet FOLLOW_ELEMENTS_in_fromJoin1265 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_fromJoin1268 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_fromJoin1271 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_fromJoin1273 = new BitSet(new ulong[2]
    {
      9241386435370549378UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_asAlias_in_fromJoin1277 = new BitSet(new ulong[1]
    {
      9223372036856872962UL
    });
    public static readonly BitSet FOLLOW_propertyFetch_in_fromJoin1282 = new BitSet(new ulong[1]
    {
      9223372036854775810UL
    });
    public static readonly BitSet FOLLOW_withClause_in_fromJoin1287 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_WITH_in_withClause1300 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_logicalExpression_in_withClause1303 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_fromClassOrOuterQueryPath_in_fromRange1314 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_inClassDeclaration_in_fromRange1319 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_inCollectionDeclaration_in_fromRange1324 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_inCollectionElementsDeclaration_in_fromRange1329 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_path_in_fromClassOrOuterQueryPath1341 = new BitSet(new ulong[2]
    {
      18014398515773570UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_asAlias_in_fromClassOrOuterQueryPath1346 = new BitSet(new ulong[1]
    {
      2097154UL
    });
    public static readonly BitSet FOLLOW_propertyFetch_in_fromClassOrOuterQueryPath1351 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_alias_in_inClassDeclaration1381 = new BitSet(new ulong[1]
    {
      67108864UL
    });
    public static readonly BitSet FOLLOW_IN_in_inClassDeclaration1383 = new BitSet(new ulong[2]
    {
      18014398513678336UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_CLASS_in_inClassDeclaration1385 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_inClassDeclaration1388 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_IN_in_inCollectionDeclaration1416 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_inCollectionDeclaration1418 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_inCollectionDeclaration1420 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_inCollectionDeclaration1422 = new BitSet(new ulong[2]
    {
      18014398513676416UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_alias_in_inCollectionDeclaration1424 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_alias_in_inCollectionElementsDeclaration1458 = new BitSet(new ulong[1]
    {
      67108864UL
    });
    public static readonly BitSet FOLLOW_IN_in_inCollectionElementsDeclaration1460 = new BitSet(new ulong[1]
    {
      131072UL
    });
    public static readonly BitSet FOLLOW_ELEMENTS_in_inCollectionElementsDeclaration1462 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_inCollectionElementsDeclaration1464 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_inCollectionElementsDeclaration1466 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_inCollectionElementsDeclaration1468 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ELEMENTS_in_inCollectionElementsDeclaration1490 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_inCollectionElementsDeclaration1492 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_inCollectionElementsDeclaration1494 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_inCollectionElementsDeclaration1496 = new BitSet(new ulong[1]
    {
      128UL
    });
    public static readonly BitSet FOLLOW_AS_in_inCollectionElementsDeclaration1498 = new BitSet(new ulong[2]
    {
      18014398513676416UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_alias_in_inCollectionElementsDeclaration1500 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_AS_in_asAlias1532 = new BitSet(new ulong[2]
    {
      18014398513676416UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_alias_in_asAlias1537 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_identifier_in_alias1549 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FETCH_in_propertyFetch1568 = new BitSet(new ulong[1]
    {
      16UL
    });
    public static readonly BitSet FOLLOW_ALL_in_propertyFetch1570 = new BitSet(new ulong[1]
    {
      8796093022208UL
    });
    public static readonly BitSet FOLLOW_PROPERTIES_in_propertyFetch1573 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_GROUP_in_groupByClause1585 = new BitSet(new ulong[1]
    {
      72057594037927936UL
    });
    public static readonly BitSet FOLLOW_LITERAL_by_in_groupByClause1591 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_groupByClause1594 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_groupByClause1598 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_groupByClause1601 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_ORDER_in_orderByClause1615 = new BitSet(new ulong[1]
    {
      72057594037927936UL
    });
    public static readonly BitSet FOLLOW_LITERAL_by_in_orderByClause1618 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_orderElement_in_orderByClause1621 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_orderByClause1625 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_orderElement_in_orderByClause1628 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_SKIP_in_skipClause1642 = new BitSet(new ulong[2]
    {
      0UL,
      6599217250304UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_skipClause1646 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_parameter_in_skipClause1650 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_TAKE_in_takeClause1662 = new BitSet(new ulong[2]
    {
      0UL,
      6599217250304UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_takeClause1666 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_parameter_in_takeClause1670 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_COLON_in_parameter1682 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_identifier_in_parameter1685 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_PARAM_in_parameter1690 = new BitSet(new ulong[2]
    {
      2UL,
      2147483648UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_parameter1694 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_expression_in_orderElement1707 = new BitSet(new ulong[3]
    {
      16642UL,
      0UL,
      96UL
    });
    public static readonly BitSet FOLLOW_ascendingOrDescending_in_orderElement1711 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ASCENDING_in_ascendingOrDescending1729 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_133_in_ascendingOrDescending1735 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_DESCENDING_in_ascendingOrDescending1755 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_134_in_ascendingOrDescending1761 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_HAVING_in_havingClause1782 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_logicalExpression_in_havingClause1785 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_WHERE_in_whereClause1796 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_logicalExpression_in_whereClause1799 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_aliasedExpression_in_selectedPropertiesList1810 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_selectedPropertiesList1814 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_aliasedExpression_in_selectedPropertiesList1817 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_expression_in_aliasedExpression1832 = new BitSet(new ulong[1]
    {
      130UL
    });
    public static readonly BitSet FOLLOW_AS_in_aliasedExpression1836 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_identifier_in_aliasedExpression1839 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_expression_in_logicalExpression1878 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_logicalOrExpression_in_expression1890 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_logicalAndExpression_in_logicalOrExpression1902 = new BitSet(new ulong[1]
    {
      1099511627778UL
    });
    public static readonly BitSet FOLLOW_OR_in_logicalOrExpression1906 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_logicalAndExpression_in_logicalOrExpression1909 = new BitSet(new ulong[1]
    {
      1099511627778UL
    });
    public static readonly BitSet FOLLOW_negatedExpression_in_logicalAndExpression1924 = new BitSet(new ulong[1]
    {
      66UL
    });
    public static readonly BitSet FOLLOW_AND_in_logicalAndExpression1928 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_negatedExpression_in_logicalAndExpression1931 = new BitSet(new ulong[1]
    {
      66UL
    });
    public static readonly BitSet FOLLOW_NOT_in_negatedExpression1952 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_negatedExpression_in_negatedExpression1956 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_equalityExpression_in_negatedExpression1969 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_relationalExpression_in_equalityExpression1999 = new BitSet(new ulong[2]
    {
      2147483650UL,
      26663156973568UL
    });
    public static readonly BitSet FOLLOW_EQ_in_equalityExpression2007 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_IS_in_equalityExpression2016 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_NOT_in_equalityExpression2022 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_NE_in_equalityExpression2034 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_SQL_NE_in_equalityExpression2043 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_relationalExpression_in_equalityExpression2054 = new BitSet(new ulong[2]
    {
      2147483650UL,
      26663156973568UL
    });
    public static readonly BitSet FOLLOW_concatenation_in_relationalExpression2071 = new BitSet(new ulong[2]
    {
      292124886018UL,
      527765581332488UL
    });
    public static readonly BitSet FOLLOW_LT_in_relationalExpression2083 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_GT_in_relationalExpression2088 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_LE_in_relationalExpression2093 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_GE_in_relationalExpression2098 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_bitwiseNotExpression_in_relationalExpression2103 = new BitSet(new ulong[2]
    {
      2UL,
      527765581332480UL
    });
    public static readonly BitSet FOLLOW_NOT_in_relationalExpression2120 = new BitSet(new ulong[2]
    {
      17246979072UL,
      8UL
    });
    public static readonly BitSet FOLLOW_IN_in_relationalExpression2141 = new BitSet(new ulong[2]
    {
      18577451680666112UL,
      2305843558969507840UL
    });
    public static readonly BitSet FOLLOW_inList_in_relationalExpression2150 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_BETWEEN_in_relationalExpression2161 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_betweenList_in_relationalExpression2170 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_LIKE_in_relationalExpression2182 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_concatenation_in_relationalExpression2191 = new BitSet(new ulong[1]
    {
      262144UL
    });
    public static readonly BitSet FOLLOW_likeEscape_in_relationalExpression2193 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_MEMBER_in_relationalExpression2202 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693984UL
    });
    public static readonly BitSet FOLLOW_OF_in_relationalExpression2206 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_relationalExpression2213 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ESCAPE_in_likeEscape2240 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_concatenation_in_likeEscape2243 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_compoundExpr_in_inList2256 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_concatenation_in_betweenList2277 = new BitSet(new ulong[1]
    {
      64UL
    });
    public static readonly BitSet FOLLOW_AND_in_betweenList2279 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_concatenation_in_betweenList2282 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_bitwiseNotExpression_in_concatenation2301 = new BitSet(new ulong[2]
    {
      2UL,
      562949953421312UL
    });
    public static readonly BitSet FOLLOW_CONCAT_in_concatenation2309 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_bitwiseNotExpression_in_concatenation2318 = new BitSet(new ulong[2]
    {
      2UL,
      562949953421312UL
    });
    public static readonly BitSet FOLLOW_CONCAT_in_concatenation2325 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_bitwiseNotExpression_in_concatenation2328 = new BitSet(new ulong[2]
    {
      2UL,
      562949953421312UL
    });
    public static readonly BitSet FOLLOW_BNOT_in_bitwiseNotExpression2352 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_bitwiseOrExpression_in_bitwiseNotExpression2355 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_bitwiseOrExpression_in_bitwiseNotExpression2361 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_bitwiseXOrExpression_in_bitwiseOrExpression2373 = new BitSet(new ulong[2]
    {
      2UL,
      2251799813685248UL
    });
    public static readonly BitSet FOLLOW_BOR_in_bitwiseOrExpression2376 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_bitwiseXOrExpression_in_bitwiseOrExpression2379 = new BitSet(new ulong[2]
    {
      2UL,
      2251799813685248UL
    });
    public static readonly BitSet FOLLOW_bitwiseAndExpression_in_bitwiseXOrExpression2393 = new BitSet(new ulong[2]
    {
      2UL,
      4503599627370496UL
    });
    public static readonly BitSet FOLLOW_BXOR_in_bitwiseXOrExpression2396 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_bitwiseAndExpression_in_bitwiseXOrExpression2399 = new BitSet(new ulong[2]
    {
      2UL,
      4503599627370496UL
    });
    public static readonly BitSet FOLLOW_additiveExpression_in_bitwiseAndExpression2413 = new BitSet(new ulong[2]
    {
      2UL,
      9007199254740992UL
    });
    public static readonly BitSet FOLLOW_BAND_in_bitwiseAndExpression2416 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_additiveExpression_in_bitwiseAndExpression2419 = new BitSet(new ulong[2]
    {
      2UL,
      9007199254740992UL
    });
    public static readonly BitSet FOLLOW_multiplyExpression_in_additiveExpression2433 = new BitSet(new ulong[2]
    {
      2UL,
      54043195528445952UL
    });
    public static readonly BitSet FOLLOW_PLUS_in_additiveExpression2439 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_MINUS_in_additiveExpression2444 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_multiplyExpression_in_additiveExpression2449 = new BitSet(new ulong[2]
    {
      2UL,
      54043195528445952UL
    });
    public static readonly BitSet FOLLOW_unaryExpression_in_multiplyExpression2464 = new BitSet(new ulong[2]
    {
      2UL,
      216172782113783808UL
    });
    public static readonly BitSet FOLLOW_STAR_in_multiplyExpression2470 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_DIV_in_multiplyExpression2475 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_unaryExpression_in_multiplyExpression2480 = new BitSet(new ulong[2]
    {
      2UL,
      216172782113783808UL
    });
    public static readonly BitSet FOLLOW_MINUS_in_unaryExpression2498 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpression2502 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_PLUS_in_unaryExpression2519 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpression2523 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_caseExpression_in_unaryExpression2540 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_quantifiedExpression_in_unaryExpression2554 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_atom_in_unaryExpression2569 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_CASE_in_caseExpression2588 = new BitSet(new ulong[1]
    {
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_whenClause_in_caseExpression2591 = new BitSet(new ulong[1]
    {
      3170534137668829184UL
    });
    public static readonly BitSet FOLLOW_elseClause_in_caseExpression2596 = new BitSet(new ulong[1]
    {
      288230376151711744UL
    });
    public static readonly BitSet FOLLOW_END_in_caseExpression2600 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_CASE_in_caseExpression2620 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_unaryExpression_in_caseExpression2622 = new BitSet(new ulong[1]
    {
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_altWhenClause_in_caseExpression2625 = new BitSet(new ulong[1]
    {
      3170534137668829184UL
    });
    public static readonly BitSet FOLLOW_elseClause_in_caseExpression2630 = new BitSet(new ulong[1]
    {
      288230376151711744UL
    });
    public static readonly BitSet FOLLOW_END_in_caseExpression2634 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_WHEN_in_whenClause2663 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_logicalExpression_in_whenClause2666 = new BitSet(new ulong[1]
    {
      1152921504606846976UL
    });
    public static readonly BitSet FOLLOW_THEN_in_whenClause2668 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_whenClause2671 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_WHEN_in_altWhenClause2685 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_unaryExpression_in_altWhenClause2688 = new BitSet(new ulong[1]
    {
      1152921504606846976UL
    });
    public static readonly BitSet FOLLOW_THEN_in_altWhenClause2690 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_altWhenClause2693 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ELSE_in_elseClause2707 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_elseClause2710 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SOME_in_quantifiedExpression2725 = new BitSet(new ulong[2]
    {
      18577451680666112UL,
      2305843558969507840UL
    });
    public static readonly BitSet FOLLOW_EXISTS_in_quantifiedExpression2730 = new BitSet(new ulong[2]
    {
      18577451680666112UL,
      2305843558969507840UL
    });
    public static readonly BitSet FOLLOW_ALL_in_quantifiedExpression2735 = new BitSet(new ulong[2]
    {
      18577451680666112UL,
      2305843558969507840UL
    });
    public static readonly BitSet FOLLOW_ANY_in_quantifiedExpression2740 = new BitSet(new ulong[2]
    {
      18577451680666112UL,
      2305843558969507840UL
    });
    public static readonly BitSet FOLLOW_identifier_in_quantifiedExpression2749 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_collectionExpr_in_quantifiedExpression2753 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_quantifiedExpression2758 = new BitSet(new ulong[1]
    {
      37332817864032256UL
    });
    public static readonly BitSet FOLLOW_subQuery_in_quantifiedExpression2763 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_quantifiedExpression2767 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_primaryExpression_in_atom2786 = new BitSet(new ulong[2]
    {
      32770UL,
      288230376151711744UL
    });
    public static readonly BitSet FOLLOW_DOT_in_atom2795 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_identifier_in_atom2798 = new BitSet(new ulong[2]
    {
      32770UL,
      288230925907525632UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_atom2826 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513941922165030983UL
    });
    public static readonly BitSet FOLLOW_exprList_in_atom2831 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_atom2833 = new BitSet(new ulong[2]
    {
      32770UL,
      288230376151711744UL
    });
    public static readonly BitSet FOLLOW_OPEN_BRACKET_in_atom2847 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_atom2852 = new BitSet(new ulong[2]
    {
      0UL,
      576460752303423488UL
    });
    public static readonly BitSet FOLLOW_CLOSE_BRACKET_in_atom2854 = new BitSet(new ulong[2]
    {
      32770UL,
      288230376151711744UL
    });
    public static readonly BitSet FOLLOW_identPrimary_in_primaryExpression2874 = new BitSet(new ulong[1]
    {
      32770UL
    });
    public static readonly BitSet FOLLOW_DOT_in_primaryExpression2887 = new BitSet(new ulong[1]
    {
      2048UL
    });
    public static readonly BitSet FOLLOW_CLASS_in_primaryExpression2890 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_constant_in_primaryExpression2900 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_COLON_in_primaryExpression2907 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_identifier_in_primaryExpression2910 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_primaryExpression2919 = new BitSet(new ulong[2]
    {
      202559557042049584UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expressionOrVector_in_primaryExpression2923 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_subQuery_in_primaryExpression2927 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_primaryExpression2930 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_PARAM_in_primaryExpression2938 = new BitSet(new ulong[2]
    {
      2UL,
      2147483648UL
    });
    public static readonly BitSet FOLLOW_NUM_INT_in_primaryExpression2942 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_expression_in_expressionOrVector2960 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_vectorExpr_in_expressionOrVector2966 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_vectorExpr3005 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_vectorExpr3008 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_vectorExpr3011 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_vectorExpr3014 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_identifier_in_identPrimary3030 = new BitSet(new ulong[2]
    {
      32770UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_DOT_in_identPrimary3048 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693968UL
    });
    public static readonly BitSet FOLLOW_identifier_in_identPrimary3053 = new BitSet(new ulong[2]
    {
      32770UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OBJECT_in_identPrimary3059 = new BitSet(new ulong[2]
    {
      32770UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_identPrimary3077 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513941922165030983UL
    });
    public static readonly BitSet FOLLOW_exprList_in_identPrimary3082 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_identPrimary3084 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_aggregate_in_identPrimary3100 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_SUM_in_aggregate3121 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_AVG_in_aggregate3127 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_MAX_in_aggregate3133 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_MIN_in_aggregate3139 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_aggregate3143 = new BitSet(new ulong[2]
    {
      165226464304304688UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_additiveExpression_in_aggregate3145 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_aggregate3147 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_COUNT_in_aggregate3166 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_aggregate3168 = new BitSet(new ulong[2]
    {
      18577451680731664UL,
      2377900603251621888UL
    });
    public static readonly BitSet FOLLOW_STAR_in_aggregate3174 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_aggregateDistinctAll_in_aggregate3180 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_aggregate3184 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_collectionExpr_in_aggregate3216 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_aggregateDistinctAll3229 = new BitSet(new ulong[2]
    {
      18577451680666112UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_aggregateDistinctAll3242 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_collectionExpr_in_aggregateDistinctAll3246 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_ELEMENTS_in_collectionExpr3265 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_INDICES_in_collectionExpr3270 = new BitSet(new ulong[2]
    {
      0UL,
      549755813888UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_collectionExpr3274 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_path_in_collectionExpr3277 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_collectionExpr3279 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_collectionExpr_in_compoundExpr3334 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_path_in_compoundExpr3339 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_OPEN_in_compoundExpr3345 = new BitSet(new ulong[2]
    {
      202559557042049584UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_subQuery_in_compoundExpr3350 = new BitSet(new ulong[2]
    {
      0UL,
      1099511627776UL
    });
    public static readonly BitSet FOLLOW_expression_in_compoundExpr3355 = new BitSet(new ulong[2]
    {
      0UL,
      1236950581248UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_compoundExpr3358 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_compoundExpr3361 = new BitSet(new ulong[2]
    {
      0UL,
      1236950581248UL
    });
    public static readonly BitSet FOLLOW_CLOSE_in_compoundExpr3368 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_TRAILING_in_exprList3387 = new BitSet(new ulong[2]
    {
      165226739182211634UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_LEADING_in_exprList3400 = new BitSet(new ulong[2]
    {
      165226739182211634UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_BOTH_in_exprList3413 = new BitSet(new ulong[2]
    {
      165226739182211634UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_exprList3437 = new BitSet(new ulong[2]
    {
      4194434UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_COMMA_in_exprList3442 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_exprList3445 = new BitSet(new ulong[2]
    {
      2UL,
      137438953472UL
    });
    public static readonly BitSet FOLLOW_FROM_in_exprList3460 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_exprList3462 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_AS_in_exprList3474 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_identifier_in_exprList3477 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_FROM_in_exprList3491 = new BitSet(new ulong[2]
    {
      165226739182211632UL,
      3513940822653403138UL
    });
    public static readonly BitSet FOLLOW_expression_in_exprList3493 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_innerSubQuery_in_subQuery3513 = new BitSet(new ulong[1]
    {
      4503599627370498UL
    });
    public static readonly BitSet FOLLOW_UNION_in_subQuery3516 = new BitSet(new ulong[1]
    {
      37332817864032256UL
    });
    public static readonly BitSet FOLLOW_innerSubQuery_in_subQuery3519 = new BitSet(new ulong[1]
    {
      4503599627370498UL
    });
    public static readonly BitSet FOLLOW_queryRule_in_innerSubQuery3533 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_set_in_constant0 = new BitSet(new ulong[1]
    {
      2UL
    });
    public static readonly BitSet FOLLOW_identifier_in_path3621 = new BitSet(new ulong[1]
    {
      32770UL
    });
    public static readonly BitSet FOLLOW_DOT_in_path3625 = new BitSet(new ulong[2]
    {
      18014398513676288UL,
      2305843009213693952UL
    });
    public static readonly BitSet FOLLOW_identifier_in_path3630 = new BitSet(new ulong[1]
    {
      32770UL
    });
    public static readonly BitSet FOLLOW_IDENT_in_identifier3646 = new BitSet(new ulong[1]
    {
      2UL
    });
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (HqlParser));
    internal static readonly bool[] possibleIds = new bool[HqlParser.tokenNames.Length];
    private bool filter;
    private IParseErrorHandler _parseErrorHandler = (IParseErrorHandler) new ErrorCounter();

    public HqlParser(ITokenStream input)
      : this(input, new RecognizerSharedState())
    {
    }

    public HqlParser(ITokenStream input, RecognizerSharedState state)
      : base(input, state)
    {
      this.InitializeCyclicDFAs();
    }

    public ITreeAdaptor TreeAdaptor
    {
      get => this.adaptor;
      set => this.adaptor = value;
    }

    public override string[] TokenNames => HqlParser.tokenNames;

    public override string GrammarFileName => "Hql.g";

    public HqlParser.statement_return statement()
    {
      HqlParser.statement_return statementReturn = new HqlParser.statement_return();
      statementReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        int num;
        switch (this.input.LA(1))
        {
          case -1:
          case 22:
          case 24:
          case 25:
          case 41:
          case 45:
          case 47:
          case 50:
          case 52:
          case 55:
          case 104:
            num = 3;
            break;
          case 13:
            num = 2;
            break;
          case 29:
            num = 4;
            break;
          case 53:
            num = 1;
            break;
          default:
            throw new NoViableAltException("", 1, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            this.PushFollow(HqlParser.FOLLOW_updateStatement_in_statement611);
            HqlParser.updateStatement_return updateStatementReturn = this.updateStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, updateStatementReturn.Tree);
            break;
          case 2:
            this.PushFollow(HqlParser.FOLLOW_deleteStatement_in_statement615);
            HqlParser.deleteStatement_return deleteStatementReturn = this.deleteStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, deleteStatementReturn.Tree);
            break;
          case 3:
            this.PushFollow(HqlParser.FOLLOW_selectStatement_in_statement619);
            HqlParser.selectStatement_return selectStatementReturn = this.selectStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, selectStatementReturn.Tree);
            break;
          case 4:
            this.PushFollow(HqlParser.FOLLOW_insertStatement_in_statement623);
            HqlParser.insertStatement_return insertStatementReturn = this.insertStatement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, insertStatementReturn.Tree);
            break;
        }
        IToken token = (IToken) this.Match((IIntStream) this.input, -1, HqlParser.FOLLOW_EOF_in_statement627);
        statementReturn.Stop = (object) this.input.LT(-1);
        statementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(statementReturn.Tree, (IToken) statementReturn.Start, (IToken) statementReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        statementReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) statementReturn.Start, this.input.LT(-1), ex);
      }
      return statementReturn;
    }

    public HqlParser.updateStatement_return updateStatement()
    {
      HqlParser.updateStatement_return updateStatementReturn = new HqlParser.updateStatement_return();
      updateStatementReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 53, HqlParser.FOLLOW_UPDATE_in_updateStatement639)), (object) nilNode);
        int num1 = 2;
        if (this.input.LA(1) == 54)
          num1 = 1;
        if (num1 == 1)
        {
          IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 54, HqlParser.FOLLOW_VERSIONED_in_updateStatement643));
          this.adaptor.AddChild((object) astNode, (object) child);
        }
        this.PushFollow(HqlParser.FOLLOW_optionalFromTokenFromClause_in_updateStatement649);
        HqlParser.optionalFromTokenFromClause_return fromClauseReturn = this.optionalFromTokenFromClause();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, fromClauseReturn.Tree);
        this.PushFollow(HqlParser.FOLLOW_setClause_in_updateStatement653);
        HqlParser.setClause_return setClauseReturn = this.setClause();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, setClauseReturn.Tree);
        int num2 = 2;
        if (this.input.LA(1) == 55)
          num2 = 1;
        if (num2 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_whereClause_in_updateStatement658);
          HqlParser.whereClause_return whereClauseReturn = this.whereClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode, whereClauseReturn.Tree);
        }
        updateStatementReturn.Stop = (object) this.input.LT(-1);
        updateStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(updateStatementReturn.Tree, (IToken) updateStatementReturn.Start, (IToken) updateStatementReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        updateStatementReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) updateStatementReturn.Start, this.input.LT(-1), ex);
      }
      return updateStatementReturn;
    }

    public HqlParser.setClause_return setClause()
    {
      HqlParser.setClause_return setClauseReturn = new HqlParser.setClause_return();
      setClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 46, HqlParser.FOLLOW_SET_in_setClause672)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_assignment_in_setClause675);
        HqlParser.assignment_return assignmentReturn1 = this.assignment();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, assignmentReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 101)
            num = 1;
          if (num == 1)
          {
            IToken token = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_setClause678);
            this.PushFollow(HqlParser.FOLLOW_assignment_in_setClause681);
            HqlParser.assignment_return assignmentReturn2 = this.assignment();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, assignmentReturn2.Tree);
          }
          else
            break;
        }
        setClauseReturn.Stop = (object) this.input.LT(-1);
        setClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(setClauseReturn.Tree, (IToken) setClauseReturn.Start, (IToken) setClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        setClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) setClauseReturn.Start, this.input.LT(-1), ex);
      }
      return setClauseReturn;
    }

    public HqlParser.assignment_return assignment()
    {
      HqlParser.assignment_return assignmentReturn = new HqlParser.assignment_return();
      assignmentReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_stateField_in_assignment695);
        HqlParser.stateField_return stateFieldReturn = this.stateField();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, stateFieldReturn.Tree);
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 102, HqlParser.FOLLOW_EQ_in_assignment697)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_newValue_in_assignment700);
        HqlParser.newValue_return newValueReturn = this.newValue();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, newValueReturn.Tree);
        assignmentReturn.Stop = (object) this.input.LT(-1);
        assignmentReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(assignmentReturn.Tree, (IToken) assignmentReturn.Start, (IToken) assignmentReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        assignmentReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) assignmentReturn.Start, this.input.LT(-1), ex);
      }
      return assignmentReturn;
    }

    public HqlParser.stateField_return stateField()
    {
      HqlParser.stateField_return stateFieldReturn = new HqlParser.stateField_return();
      stateFieldReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_path_in_stateField713);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, pathReturn.Tree);
        stateFieldReturn.Stop = (object) this.input.LT(-1);
        stateFieldReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(stateFieldReturn.Tree, (IToken) stateFieldReturn.Start, (IToken) stateFieldReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        stateFieldReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) stateFieldReturn.Start, this.input.LT(-1), ex);
      }
      return stateFieldReturn;
    }

    public HqlParser.newValue_return newValue()
    {
      HqlParser.newValue_return newValueReturn = new HqlParser.newValue_return();
      newValueReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_concatenation_in_newValue726);
        HqlParser.concatenation_return concatenationReturn = this.concatenation();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, concatenationReturn.Tree);
        newValueReturn.Stop = (object) this.input.LT(-1);
        newValueReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(newValueReturn.Tree, (IToken) newValueReturn.Start, (IToken) newValueReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        newValueReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) newValueReturn.Start, this.input.LT(-1), ex);
      }
      return newValueReturn;
    }

    public HqlParser.deleteStatement_return deleteStatement()
    {
      HqlParser.deleteStatement_return deleteStatementReturn = new HqlParser.deleteStatement_return();
      deleteStatementReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 13, HqlParser.FOLLOW_DELETE_in_deleteStatement737)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_optionalFromTokenFromClause_in_deleteStatement743);
        HqlParser.optionalFromTokenFromClause_return fromClauseReturn = this.optionalFromTokenFromClause();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, fromClauseReturn.Tree);
        int num = 2;
        if (this.input.LA(1) == 55)
          num = 1;
        if (num == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_whereClause_in_deleteStatement749);
          HqlParser.whereClause_return whereClauseReturn = this.whereClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode, whereClauseReturn.Tree);
        }
        deleteStatementReturn.Stop = (object) this.input.LT(-1);
        deleteStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(deleteStatementReturn.Tree, (IToken) deleteStatementReturn.Start, (IToken) deleteStatementReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        deleteStatementReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) deleteStatementReturn.Start, this.input.LT(-1), ex);
      }
      return deleteStatementReturn;
    }

    public HqlParser.optionalFromTokenFromClause_return optionalFromTokenFromClause()
    {
      HqlParser.optionalFromTokenFromClause_return fromClauseReturn = new HqlParser.optionalFromTokenFromClause_return();
      fromClauseReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule optionalFromTokenFromClause2");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule asAlias");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_optionalFromTokenFromClause2_in_optionalFromTokenFromClause764);
        HqlParser.optionalFromTokenFromClause2_return fromClause2Return = this.optionalFromTokenFromClause2();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(fromClause2Return.Tree);
        this.PushFollow(HqlParser.FOLLOW_path_in_optionalFromTokenFromClause766);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(pathReturn.Tree);
        int num = 2;
        switch (this.input.LA(1))
        {
          case 7:
          case 125:
            num = 1;
            break;
        }
        if (num == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_asAlias_in_optionalFromTokenFromClause769);
          HqlParser.asAlias_return asAliasReturn = this.asAlias();
          --this.state.followingStackPointer;
          ruleSubtreeStream3.Add(asAliasReturn.Tree);
        }
        fromClauseReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", fromClauseReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(22, "FROM"), (object) nilNode2);
        IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(87, "RANGE"), (object) nilNode3);
        this.adaptor.AddChild((object) astNode3, ruleSubtreeStream2.NextTree());
        if (ruleSubtreeStream3.HasNext())
          this.adaptor.AddChild((object) astNode3, ruleSubtreeStream3.NextTree());
        ruleSubtreeStream3.Reset();
        this.adaptor.AddChild((object) astNode2, (object) astNode3);
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        fromClauseReturn.Tree = (object) nilNode1;
        fromClauseReturn.Tree = (object) nilNode1;
        fromClauseReturn.Stop = (object) this.input.LT(-1);
        fromClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(fromClauseReturn.Tree, (IToken) fromClauseReturn.Start, (IToken) fromClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        fromClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) fromClauseReturn.Start, this.input.LT(-1), ex);
      }
      return fromClauseReturn;
    }

    public HqlParser.optionalFromTokenFromClause2_return optionalFromTokenFromClause2()
    {
      HqlParser.optionalFromTokenFromClause2_return fromClause2Return = new HqlParser.optionalFromTokenFromClause2_return();
      fromClause2Return.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        int num = 2;
        if (this.input.LA(1) == 22)
          num = 1;
        if (num == 1)
        {
          IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 22, HqlParser.FOLLOW_FROM_in_optionalFromTokenFromClause2800));
          this.adaptor.AddChild((object) nilNode, (object) child);
        }
        fromClause2Return.Stop = (object) this.input.LT(-1);
        fromClause2Return.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(fromClause2Return.Tree, (IToken) fromClause2Return.Start, (IToken) fromClause2Return.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        fromClause2Return.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) fromClause2Return.Start, this.input.LT(-1), ex);
      }
      return fromClause2Return;
    }

    public HqlParser.selectStatement_return selectStatement()
    {
      HqlParser.selectStatement_return selectStatementReturn = new HqlParser.selectStatement_return();
      selectStatementReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule queryRule");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_queryRule_in_selectStatement814);
        HqlParser.queryRule_return queryRuleReturn = this.queryRule();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(queryRuleReturn.Tree);
        selectStatementReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", selectStatementReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule q", queryRuleReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(86, "query"), (object) nilNode2);
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream3.NextTree());
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        selectStatementReturn.Tree = (object) nilNode1;
        selectStatementReturn.Tree = (object) nilNode1;
        selectStatementReturn.Stop = (object) this.input.LT(-1);
        selectStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(selectStatementReturn.Tree, (IToken) selectStatementReturn.Start, (IToken) selectStatementReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        selectStatementReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) selectStatementReturn.Start, this.input.LT(-1), ex);
      }
      return selectStatementReturn;
    }

    public HqlParser.insertStatement_return insertStatement()
    {
      HqlParser.insertStatement_return insertStatementReturn = new HqlParser.insertStatement_return();
      insertStatementReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 29, HqlParser.FOLLOW_INSERT_in_insertStatement843)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_intoClause_in_insertStatement846);
        HqlParser.intoClause_return intoClauseReturn = this.intoClause();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, intoClauseReturn.Tree);
        this.PushFollow(HqlParser.FOLLOW_selectStatement_in_insertStatement848);
        HqlParser.selectStatement_return selectStatementReturn = this.selectStatement();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, selectStatementReturn.Tree);
        insertStatementReturn.Stop = (object) this.input.LT(-1);
        insertStatementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(insertStatementReturn.Tree, (IToken) insertStatementReturn.Start, (IToken) insertStatementReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        insertStatementReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) insertStatementReturn.Start, this.input.LT(-1), ex);
      }
      return insertStatementReturn;
    }

    public HqlParser.intoClause_return intoClause()
    {
      HqlParser.intoClause_return intoClauseReturn = new HqlParser.intoClause_return();
      intoClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 30, HqlParser.FOLLOW_INTO_in_intoClause859)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_path_in_intoClause862);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, pathReturn.Tree);
        this.WeakKeywords();
        this.PushFollow(HqlParser.FOLLOW_insertablePropertySpec_in_intoClause866);
        HqlParser.insertablePropertySpec_return propertySpecReturn = this.insertablePropertySpec();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, propertySpecReturn.Tree);
        intoClauseReturn.Stop = (object) this.input.LT(-1);
        intoClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(intoClauseReturn.Tree, (IToken) intoClauseReturn.Start, (IToken) intoClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        intoClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) intoClauseReturn.Start, this.input.LT(-1), ex);
      }
      return intoClauseReturn;
    }

    public HqlParser.insertablePropertySpec_return insertablePropertySpec()
    {
      HqlParser.insertablePropertySpec_return propertySpecReturn = new HqlParser.insertablePropertySpec_return();
      propertySpecReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token OPEN");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token CLOSE");
      RewriteRuleTokenStream rewriteRuleTokenStream3 = new RewriteRuleTokenStream(this.adaptor, "token COMMA");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule primaryExpression");
      try
      {
        IToken el1 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_insertablePropertySpec877);
        rewriteRuleTokenStream1.Add(el1);
        this.PushFollow(HqlParser.FOLLOW_primaryExpression_in_insertablePropertySpec879);
        HqlParser.primaryExpression_return expressionReturn1 = this.primaryExpression();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(expressionReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 101)
            num = 1;
          if (num == 1)
          {
            IToken el2 = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_insertablePropertySpec883);
            rewriteRuleTokenStream3.Add(el2);
            this.PushFollow(HqlParser.FOLLOW_primaryExpression_in_insertablePropertySpec885);
            HqlParser.primaryExpression_return expressionReturn2 = this.primaryExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream1.Add(expressionReturn2.Tree);
          }
          else
            break;
        }
        IToken el3 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_insertablePropertySpec890);
        rewriteRuleTokenStream2.Add(el3);
        propertySpecReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", propertySpecReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(87, "column-spec"), (object) nilNode2);
        while (ruleSubtreeStream1.HasNext())
          this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
        ruleSubtreeStream1.Reset();
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        propertySpecReturn.Tree = (object) nilNode1;
        propertySpecReturn.Tree = (object) nilNode1;
        propertySpecReturn.Stop = (object) this.input.LT(-1);
        propertySpecReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(propertySpecReturn.Tree, (IToken) propertySpecReturn.Start, (IToken) propertySpecReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        propertySpecReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) propertySpecReturn.Start, this.input.LT(-1), ex);
      }
      return propertySpecReturn;
    }

    public HqlParser.queryRule_return queryRule()
    {
      HqlParser.queryRule_return queryRuleReturn = new HqlParser.queryRule_return();
      queryRuleReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_selectFrom_in_queryRule916);
        HqlParser.selectFrom_return selectFromReturn = this.selectFrom();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, selectFromReturn.Tree);
        int num1 = 2;
        if (this.input.LA(1) == 55)
          num1 = 1;
        if (num1 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_whereClause_in_queryRule921);
          HqlParser.whereClause_return whereClauseReturn = this.whereClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, whereClauseReturn.Tree);
        }
        int num2 = 2;
        if (this.input.LA(1) == 24)
          num2 = 1;
        if (num2 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_groupByClause_in_queryRule928);
          HqlParser.groupByClause_return groupByClauseReturn = this.groupByClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, groupByClauseReturn.Tree);
        }
        int num3 = 2;
        if (this.input.LA(1) == 25)
          num3 = 1;
        if (num3 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_havingClause_in_queryRule935);
          HqlParser.havingClause_return havingClauseReturn = this.havingClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, havingClauseReturn.Tree);
        }
        int num4 = 2;
        if (this.input.LA(1) == 41)
          num4 = 1;
        if (num4 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_orderByClause_in_queryRule942);
          HqlParser.orderByClause_return orderByClauseReturn = this.orderByClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, orderByClauseReturn.Tree);
        }
        int num5 = 2;
        if (this.input.LA(1) == 47)
          num5 = 1;
        if (num5 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_skipClause_in_queryRule949);
          HqlParser.skipClause_return skipClauseReturn = this.skipClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, skipClauseReturn.Tree);
        }
        int num6 = 2;
        if (this.input.LA(1) == 50)
          num6 = 1;
        if (num6 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_takeClause_in_queryRule956);
          HqlParser.takeClause_return clause = this.takeClause();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, clause.Tree);
        }
        queryRuleReturn.Stop = (object) this.input.LT(-1);
        queryRuleReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(queryRuleReturn.Tree, (IToken) queryRuleReturn.Start, (IToken) queryRuleReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        queryRuleReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) queryRuleReturn.Start, this.input.LT(-1), ex);
      }
      return queryRuleReturn;
    }

    public HqlParser.selectFrom_return selectFrom()
    {
      HqlParser.selectFrom_return selectFromReturn = new HqlParser.selectFrom_return();
      selectFromReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      HqlParser.fromClause_return fromClauseReturn = (HqlParser.fromClause_return) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule selectClause");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule fromClause");
      try
      {
        int num1 = 2;
        if (this.input.LA(1) == 45)
          num1 = 1;
        if (num1 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_selectClause_in_selectFrom974);
          HqlParser.selectClause_return selectClauseReturn = this.selectClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream1.Add(selectClauseReturn.Tree);
        }
        int num2 = 2;
        if (this.input.LA(1) == 22)
          num2 = 1;
        if (num2 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_fromClause_in_selectFrom981);
          fromClauseReturn = this.fromClause();
          --this.state.followingStackPointer;
          ruleSubtreeStream2.Add(fromClauseReturn.Tree);
        }
        if ((fromClauseReturn != null ? (IASTNode) fromClauseReturn.Tree : (IASTNode) null) == null && !this.filter)
          throw new RecognitionException("FROM expected (non-filter queries must contain a FROM clause)");
        selectFromReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", selectFromReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        if ((fromClauseReturn != null ? (IASTNode) fromClauseReturn.Tree : (IASTNode) null) == null && this.filter)
        {
          IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
          IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(89, "SELECT_FROM"), (object) nilNode2);
          this.adaptor.AddChild((object) astNode2, (object) (IASTNode) this.adaptor.Create(22, "{filter-implied FROM}"));
          if (ruleSubtreeStream1.HasNext())
            this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
          ruleSubtreeStream1.Reset();
          this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        }
        else
        {
          IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
          IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(89, "SELECT_FROM"), (object) nilNode3);
          if (ruleSubtreeStream2.HasNext())
            this.adaptor.AddChild((object) astNode3, ruleSubtreeStream2.NextTree());
          ruleSubtreeStream2.Reset();
          if (ruleSubtreeStream1.HasNext())
            this.adaptor.AddChild((object) astNode3, ruleSubtreeStream1.NextTree());
          ruleSubtreeStream1.Reset();
          this.adaptor.AddChild((object) nilNode1, (object) astNode3);
        }
        selectFromReturn.Tree = (object) nilNode1;
        selectFromReturn.Tree = (object) nilNode1;
        selectFromReturn.Stop = (object) this.input.LT(-1);
        selectFromReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(selectFromReturn.Tree, (IToken) selectFromReturn.Start, (IToken) selectFromReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        selectFromReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) selectFromReturn.Start, this.input.LT(-1), ex);
      }
      return selectFromReturn;
    }

    public HqlParser.selectClause_return selectClause()
    {
      HqlParser.selectClause_return selectClauseReturn = new HqlParser.selectClause_return();
      selectClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 45, HqlParser.FOLLOW_SELECT_in_selectClause1030)), (object) nilNode);
        this.WeakKeywords();
        int num1 = 2;
        if (this.input.LA(1) == 16)
          num1 = 1;
        if (num1 == 1)
        {
          IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 16, HqlParser.FOLLOW_DISTINCT_in_selectClause1042));
          this.adaptor.AddChild((object) astNode, (object) child);
        }
        int num2;
        switch (this.input.LA(1))
        {
          case 4:
          case 5:
          case 9:
          case 12:
          case 17:
          case 19:
          case 20:
          case 27:
          case 35:
          case 36:
          case 38:
          case 39:
          case 48:
          case 49:
          case 51:
          case 57:
          case 65:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 103:
          case 105:
          case 106:
          case 114:
          case 118:
          case 119:
          case 124:
          case 125:
            num2 = 1;
            break;
          case 37:
            num2 = 2;
            break;
          case 68:
            num2 = 3;
            break;
          default:
            throw new NoViableAltException("", 18, 0, (IIntStream) this.input);
        }
        switch (num2)
        {
          case 1:
            this.PushFollow(HqlParser.FOLLOW_selectedPropertiesList_in_selectClause1048);
            HqlParser.selectedPropertiesList_return propertiesListReturn = this.selectedPropertiesList();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, propertiesListReturn.Tree);
            break;
          case 2:
            this.PushFollow(HqlParser.FOLLOW_newExpression_in_selectClause1052);
            HqlParser.newExpression_return expressionReturn = this.newExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn.Tree);
            break;
          case 3:
            this.PushFollow(HqlParser.FOLLOW_selectObject_in_selectClause1056);
            HqlParser.selectObject_return selectObjectReturn = this.selectObject();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, selectObjectReturn.Tree);
            break;
        }
        selectClauseReturn.Stop = (object) this.input.LT(-1);
        selectClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(selectClauseReturn.Tree, (IToken) selectClauseReturn.Start, (IToken) selectClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        selectClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) selectClauseReturn.Start, this.input.LT(-1), ex);
      }
      return selectClauseReturn;
    }

    public HqlParser.newExpression_return newExpression()
    {
      HqlParser.newExpression_return expressionReturn = new HqlParser.newExpression_return();
      expressionReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token NEW");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token OPEN");
      RewriteRuleTokenStream rewriteRuleTokenStream3 = new RewriteRuleTokenStream(this.adaptor, "token CLOSE");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule selectedPropertiesList");
      try
      {
        IToken el1 = (IToken) this.Match((IIntStream) this.input, 37, HqlParser.FOLLOW_NEW_in_newExpression1070);
        rewriteRuleTokenStream1.Add(el1);
        this.PushFollow(HqlParser.FOLLOW_path_in_newExpression1072);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(pathReturn.Tree);
        IToken token = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_newExpression1077);
        rewriteRuleTokenStream2.Add(token);
        this.PushFollow(HqlParser.FOLLOW_selectedPropertiesList_in_newExpression1079);
        HqlParser.selectedPropertiesList_return propertiesListReturn = this.selectedPropertiesList();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(propertiesListReturn.Tree);
        IToken el2 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_newExpression1081);
        rewriteRuleTokenStream3.Add(el2);
        expressionReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(73, token), (object) nilNode2);
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream2.NextTree());
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        expressionReturn.Tree = (object) nilNode1;
        expressionReturn.Tree = (object) nilNode1;
        expressionReturn.Stop = (object) this.input.LT(-1);
        expressionReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(expressionReturn.Tree, (IToken) expressionReturn.Start, (IToken) expressionReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn.Start, this.input.LT(-1), ex);
      }
      return expressionReturn;
    }

    public HqlParser.selectObject_return selectObject()
    {
      HqlParser.selectObject_return selectObjectReturn = new HqlParser.selectObject_return();
      selectObjectReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 68, HqlParser.FOLLOW_OBJECT_in_selectObject1107)), (object) nilNode);
        IToken token1 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_selectObject1110);
        this.PushFollow(HqlParser.FOLLOW_identifier_in_selectObject1113);
        HqlParser.identifier_return identifierReturn = this.identifier();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, identifierReturn.Tree);
        IToken token2 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_selectObject1115);
        selectObjectReturn.Stop = (object) this.input.LT(-1);
        selectObjectReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(selectObjectReturn.Tree, (IToken) selectObjectReturn.Start, (IToken) selectObjectReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        selectObjectReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) selectObjectReturn.Start, this.input.LT(-1), ex);
      }
      return selectObjectReturn;
    }

    public HqlParser.fromClause_return fromClause()
    {
      HqlParser.fromClause_return fromClauseReturn = new HqlParser.fromClause_return();
      fromClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 22, HqlParser.FOLLOW_FROM_in_fromClause1133)), (object) nilNode);
        this.WeakKeywords();
        this.PushFollow(HqlParser.FOLLOW_fromRange_in_fromClause1138);
        HqlParser.fromRange_return fromRangeReturn1 = this.fromRange();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, fromRangeReturn1.Tree);
        while (true)
        {
          int num1 = 3;
          int num2 = this.input.LA(1);
          if (num2 == 23 || num2 == 28 || num2 >= 32 && num2 <= 33 || num2 == 44)
            num1 = 1;
          else if (num2 == 101)
            num1 = 2;
          switch (num1)
          {
            case 1:
              this.PushFollow(HqlParser.FOLLOW_fromJoin_in_fromClause1142);
              HqlParser.fromJoin_return fromJoinReturn = this.fromJoin();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, fromJoinReturn.Tree);
              continue;
            case 2:
              IToken token = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_fromClause1146);
              this.WeakKeywords();
              this.PushFollow(HqlParser.FOLLOW_fromRange_in_fromClause1151);
              HqlParser.fromRange_return fromRangeReturn2 = this.fromRange();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, fromRangeReturn2.Tree);
              continue;
            default:
              goto label_9;
          }
        }
label_9:
        fromClauseReturn.Stop = (object) this.input.LT(-1);
        fromClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(fromClauseReturn.Tree, (IToken) fromClauseReturn.Start, (IToken) fromClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        fromClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) fromClauseReturn.Start, this.input.LT(-1), ex);
      }
      return fromClauseReturn;
    }

    public HqlParser.fromJoin_return fromJoin()
    {
      HqlParser.fromJoin_return fromJoinReturn = new HqlParser.fromJoin_return();
      fromJoinReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 23:
            if (this.input.LA(2) != 32)
              throw new NoViableAltException("", 32, 2, (IIntStream) this.input);
            switch (this.input.LA(3))
            {
              case 17:
                num1 = 2;
                break;
              case 21:
                switch (this.input.LA(4))
                {
                  case 17:
                    num1 = 2;
                    break;
                  case 125:
                    num1 = 1;
                    break;
                  default:
                    throw new NoViableAltException("", 32, 6, (IIntStream) this.input);
                }
                break;
              case 125:
                num1 = 1;
                break;
              default:
                throw new NoViableAltException("", 32, 4, (IIntStream) this.input);
            }
            break;
          case 28:
            if (this.input.LA(2) != 32)
              throw new NoViableAltException("", 32, 3, (IIntStream) this.input);
            switch (this.input.LA(3))
            {
              case 17:
                num1 = 2;
                break;
              case 21:
                switch (this.input.LA(4))
                {
                  case 17:
                    num1 = 2;
                    break;
                  case 125:
                    num1 = 1;
                    break;
                  default:
                    throw new NoViableAltException("", 32, 6, (IIntStream) this.input);
                }
                break;
              case 125:
                num1 = 1;
                break;
              default:
                throw new NoViableAltException("", 32, 4, (IIntStream) this.input);
            }
            break;
          case 32:
            switch (this.input.LA(2))
            {
              case 17:
                num1 = 2;
                break;
              case 21:
                switch (this.input.LA(3))
                {
                  case 17:
                    num1 = 2;
                    break;
                  case 125:
                    num1 = 1;
                    break;
                  default:
                    throw new NoViableAltException("", 32, 6, (IIntStream) this.input);
                }
                break;
              case 125:
                num1 = 1;
                break;
              default:
                throw new NoViableAltException("", 32, 4, (IIntStream) this.input);
            }
            break;
          case 33:
          case 44:
            switch (this.input.LA(2))
            {
              case 32:
                switch (this.input.LA(3))
                {
                  case 17:
                    num1 = 2;
                    break;
                  case 21:
                    switch (this.input.LA(4))
                    {
                      case 17:
                        num1 = 2;
                        break;
                      case 125:
                        num1 = 1;
                        break;
                      default:
                        throw new NoViableAltException("", 32, 6, (IIntStream) this.input);
                    }
                    break;
                  case 125:
                    num1 = 1;
                    break;
                  default:
                    throw new NoViableAltException("", 32, 4, (IIntStream) this.input);
                }
                break;
              case 42:
                if (this.input.LA(3) != 32)
                  throw new NoViableAltException("", 32, 5, (IIntStream) this.input);
                switch (this.input.LA(4))
                {
                  case 17:
                    num1 = 2;
                    break;
                  case 21:
                    switch (this.input.LA(5))
                    {
                      case 17:
                        num1 = 2;
                        break;
                      case 125:
                        num1 = 1;
                        break;
                      default:
                        throw new NoViableAltException("", 32, 6, (IIntStream) this.input);
                    }
                    break;
                  case 125:
                    num1 = 1;
                    break;
                  default:
                    throw new NoViableAltException("", 32, 4, (IIntStream) this.input);
                }
                break;
              default:
                throw new NoViableAltException("", 32, 1, (IIntStream) this.input);
            }
          default:
            throw new NoViableAltException("", 32, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            int num2 = 4;
            switch (this.input.LA(1))
            {
              case 23:
                num2 = 2;
                break;
              case 28:
                num2 = 3;
                break;
              case 33:
              case 44:
                num2 = 1;
                break;
            }
            switch (num2)
            {
              case 1:
                IToken payload1 = this.input.LT(1);
                if (this.input.LA(1) != 33 && this.input.LA(1) != 44)
                  throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
                this.input.Consume();
                this.adaptor.AddChild((object) nilNode1, (object) (IASTNode) this.adaptor.Create(payload1));
                this.state.errorRecovery = false;
                int num3 = 2;
                if (this.input.LA(1) == 42)
                  num3 = 1;
                if (num3 == 1)
                {
                  IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 42, HqlParser.FOLLOW_OUTER_in_fromJoin1180));
                  this.adaptor.AddChild((object) nilNode1, (object) child);
                  break;
                }
                break;
              case 2:
                IASTNode child1 = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 23, HqlParser.FOLLOW_FULL_in_fromJoin1188));
                this.adaptor.AddChild((object) nilNode1, (object) child1);
                break;
              case 3:
                IASTNode child2 = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 28, HqlParser.FOLLOW_INNER_in_fromJoin1192));
                this.adaptor.AddChild((object) nilNode1, (object) child2);
                break;
            }
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 32, HqlParser.FOLLOW_JOIN_in_fromJoin1197)), (object) nilNode1);
            int num4 = 2;
            if (this.input.LA(1) == 21)
              num4 = 1;
            if (num4 == 1)
            {
              IASTNode child3 = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 21, HqlParser.FOLLOW_FETCH_in_fromJoin1201));
              this.adaptor.AddChild((object) astNode, (object) child3);
            }
            this.PushFollow(HqlParser.FOLLOW_path_in_fromJoin1205);
            HqlParser.path_return pathReturn1 = this.path();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, pathReturn1.Tree);
            int num5 = 2;
            switch (this.input.LA(1))
            {
              case 7:
              case 125:
                num5 = 1;
                break;
            }
            if (num5 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_asAlias_in_fromJoin1208);
              HqlParser.asAlias_return asAliasReturn = this.asAlias();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, asAliasReturn.Tree);
            }
            int num6 = 2;
            if (this.input.LA(1) == 21)
              num6 = 1;
            if (num6 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_propertyFetch_in_fromJoin1213);
              HqlParser.propertyFetch_return propertyFetchReturn = this.propertyFetch();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, propertyFetchReturn.Tree);
            }
            int num7 = 2;
            if (this.input.LA(1) == 63)
              num7 = 1;
            if (num7 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_withClause_in_fromJoin1218);
              HqlParser.withClause_return withClauseReturn = this.withClause();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, withClauseReturn.Tree);
              break;
            }
            break;
          case 2:
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            int num8 = 4;
            switch (this.input.LA(1))
            {
              case 23:
                num8 = 2;
                break;
              case 28:
                num8 = 3;
                break;
              case 33:
              case 44:
                num8 = 1;
                break;
            }
            switch (num8)
            {
              case 1:
                IToken payload2 = this.input.LT(1);
                if (this.input.LA(1) != 33 && this.input.LA(1) != 44)
                  throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
                this.input.Consume();
                this.adaptor.AddChild((object) nilNode2, (object) (IASTNode) this.adaptor.Create(payload2));
                this.state.errorRecovery = false;
                int num9 = 2;
                if (this.input.LA(1) == 42)
                  num9 = 1;
                if (num9 == 1)
                {
                  IASTNode child4 = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 42, HqlParser.FOLLOW_OUTER_in_fromJoin1240));
                  this.adaptor.AddChild((object) nilNode2, (object) child4);
                  break;
                }
                break;
              case 2:
                IASTNode child5 = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 23, HqlParser.FOLLOW_FULL_in_fromJoin1248));
                this.adaptor.AddChild((object) nilNode2, (object) child5);
                break;
              case 3:
                IASTNode child6 = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 28, HqlParser.FOLLOW_INNER_in_fromJoin1252));
                this.adaptor.AddChild((object) nilNode2, (object) child6);
                break;
            }
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 32, HqlParser.FOLLOW_JOIN_in_fromJoin1257)), (object) nilNode2);
            int num10 = 2;
            if (this.input.LA(1) == 21)
              num10 = 1;
            if (num10 == 1)
            {
              IASTNode child7 = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 21, HqlParser.FOLLOW_FETCH_in_fromJoin1261));
              this.adaptor.AddChild((object) astNode, (object) child7);
            }
            IToken token1 = (IToken) this.Match((IIntStream) this.input, 17, HqlParser.FOLLOW_ELEMENTS_in_fromJoin1265);
            IToken token2 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_fromJoin1268);
            this.PushFollow(HqlParser.FOLLOW_path_in_fromJoin1271);
            HqlParser.path_return pathReturn2 = this.path();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, pathReturn2.Tree);
            IToken token3 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_fromJoin1273);
            int num11 = 2;
            switch (this.input.LA(1))
            {
              case 7:
              case 125:
                num11 = 1;
                break;
            }
            if (num11 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_asAlias_in_fromJoin1277);
              HqlParser.asAlias_return asAliasReturn = this.asAlias();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, asAliasReturn.Tree);
            }
            int num12 = 2;
            if (this.input.LA(1) == 21)
              num12 = 1;
            if (num12 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_propertyFetch_in_fromJoin1282);
              HqlParser.propertyFetch_return propertyFetchReturn = this.propertyFetch();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, propertyFetchReturn.Tree);
            }
            int num13 = 2;
            if (this.input.LA(1) == 63)
              num13 = 1;
            if (num13 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_withClause_in_fromJoin1287);
              HqlParser.withClause_return withClauseReturn = this.withClause();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, withClauseReturn.Tree);
              break;
            }
            break;
        }
        fromJoinReturn.Stop = (object) this.input.LT(-1);
        fromJoinReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(fromJoinReturn.Tree, (IToken) fromJoinReturn.Start, (IToken) fromJoinReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        fromJoinReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) fromJoinReturn.Start, this.input.LT(-1), ex);
      }
      return fromJoinReturn;
    }

    public HqlParser.withClause_return withClause()
    {
      HqlParser.withClause_return withClauseReturn = new HqlParser.withClause_return();
      withClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 63, HqlParser.FOLLOW_WITH_in_withClause1300)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_logicalExpression_in_withClause1303);
        HqlParser.logicalExpression_return expressionReturn = this.logicalExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn.Tree);
        withClauseReturn.Stop = (object) this.input.LT(-1);
        withClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(withClauseReturn.Tree, (IToken) withClauseReturn.Start, (IToken) withClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        withClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) withClauseReturn.Start, this.input.LT(-1), ex);
      }
      return withClauseReturn;
    }

    public HqlParser.fromRange_return fromRange()
    {
      HqlParser.fromRange_return fromRangeReturn = new HqlParser.fromRange_return();
      fromRangeReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 17:
            num1 = 4;
            break;
          case 26:
            num1 = 3;
            break;
          case 125:
            int num2 = this.input.LA(2);
            if (num2 == 26)
            {
              switch (this.input.LA(3))
              {
                case 11:
                case 125:
                  num1 = 2;
                  break;
                case 17:
                  num1 = 4;
                  break;
                default:
                  throw new NoViableAltException("", 33, 4, (IIntStream) this.input);
              }
            }
            else
            {
              if (num2 != -1 && num2 != 7 && num2 != 15 && num2 != 21 && (num2 < 23 || num2 > 25) && num2 != 28 && (num2 < 32 || num2 > 33) && num2 != 41 && num2 != 44 && num2 != 47 && num2 != 50 && num2 != 52 && num2 != 55 && num2 != 101 && num2 != 104 && num2 != 125)
                throw new NoViableAltException("", 33, 1, (IIntStream) this.input);
              num1 = 1;
              break;
            }
            break;
          default:
            throw new NoViableAltException("", 33, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_fromClassOrOuterQueryPath_in_fromRange1314);
            HqlParser.fromClassOrOuterQueryPath_return outerQueryPathReturn = this.fromClassOrOuterQueryPath();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, outerQueryPathReturn.Tree);
            break;
          case 2:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_inClassDeclaration_in_fromRange1319);
            HqlParser.inClassDeclaration_return declarationReturn1 = this.inClassDeclaration();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, declarationReturn1.Tree);
            break;
          case 3:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_inCollectionDeclaration_in_fromRange1324);
            HqlParser.inCollectionDeclaration_return declarationReturn2 = this.inCollectionDeclaration();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, declarationReturn2.Tree);
            break;
          case 4:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_inCollectionElementsDeclaration_in_fromRange1329);
            HqlParser.inCollectionElementsDeclaration_return declarationReturn3 = this.inCollectionElementsDeclaration();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, declarationReturn3.Tree);
            break;
        }
        fromRangeReturn.Stop = (object) this.input.LT(-1);
        fromRangeReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(fromRangeReturn.Tree, (IToken) fromRangeReturn.Start, (IToken) fromRangeReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        fromRangeReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) fromRangeReturn.Start, this.input.LT(-1), ex);
      }
      return fromRangeReturn;
    }

    public HqlParser.fromClassOrOuterQueryPath_return fromClassOrOuterQueryPath()
    {
      HqlParser.fromClassOrOuterQueryPath_return outerQueryPathReturn = new HqlParser.fromClassOrOuterQueryPath_return();
      outerQueryPathReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule propertyFetch");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule asAlias");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_path_in_fromClassOrOuterQueryPath1341);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(pathReturn.Tree);
        this.WeakKeywords();
        int num1 = 2;
        switch (this.input.LA(1))
        {
          case 7:
          case 125:
            num1 = 1;
            break;
        }
        if (num1 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_asAlias_in_fromClassOrOuterQueryPath1346);
          HqlParser.asAlias_return asAliasReturn = this.asAlias();
          --this.state.followingStackPointer;
          ruleSubtreeStream3.Add(asAliasReturn.Tree);
        }
        int num2 = 2;
        if (this.input.LA(1) == 21)
          num2 = 1;
        if (num2 == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_propertyFetch_in_fromClassOrOuterQueryPath1351);
          HqlParser.propertyFetch_return propertyFetchReturn = this.propertyFetch();
          --this.state.followingStackPointer;
          ruleSubtreeStream1.Add(propertyFetchReturn.Tree);
        }
        outerQueryPathReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", outerQueryPathReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(87, "RANGE"), (object) nilNode2);
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream2.NextTree());
        if (ruleSubtreeStream3.HasNext())
          this.adaptor.AddChild((object) astNode2, ruleSubtreeStream3.NextTree());
        ruleSubtreeStream3.Reset();
        if (ruleSubtreeStream1.HasNext())
          this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
        ruleSubtreeStream1.Reset();
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        outerQueryPathReturn.Tree = (object) nilNode1;
        outerQueryPathReturn.Tree = (object) nilNode1;
        outerQueryPathReturn.Stop = (object) this.input.LT(-1);
        outerQueryPathReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(outerQueryPathReturn.Tree, (IToken) outerQueryPathReturn.Start, (IToken) outerQueryPathReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        outerQueryPathReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) outerQueryPathReturn.Start, this.input.LT(-1), ex);
      }
      return outerQueryPathReturn;
    }

    public HqlParser.inClassDeclaration_return inClassDeclaration()
    {
      HqlParser.inClassDeclaration_return declarationReturn = new HqlParser.inClassDeclaration_return();
      declarationReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token CLASS");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token IN");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule alias");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_alias_in_inClassDeclaration1381);
        HqlParser.alias_return aliasReturn = this.alias();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(aliasReturn.Tree);
        IToken el1 = (IToken) this.Match((IIntStream) this.input, 26, HqlParser.FOLLOW_IN_in_inClassDeclaration1383);
        rewriteRuleTokenStream2.Add(el1);
        int num = 2;
        if (this.input.LA(1) == 11)
          num = 1;
        if (num == 1)
        {
          IToken el2 = (IToken) this.Match((IIntStream) this.input, 11, HqlParser.FOLLOW_CLASS_in_inClassDeclaration1385);
          rewriteRuleTokenStream1.Add(el2);
        }
        this.PushFollow(HqlParser.FOLLOW_path_in_inClassDeclaration1388);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(pathReturn.Tree);
        declarationReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", declarationReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(87, "RANGE"), (object) nilNode2);
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream2.NextTree());
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        declarationReturn.Tree = (object) nilNode1;
        declarationReturn.Tree = (object) nilNode1;
        declarationReturn.Stop = (object) this.input.LT(-1);
        declarationReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(declarationReturn.Tree, (IToken) declarationReturn.Start, (IToken) declarationReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        declarationReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) declarationReturn.Start, this.input.LT(-1), ex);
      }
      return declarationReturn;
    }

    public HqlParser.inCollectionDeclaration_return inCollectionDeclaration()
    {
      HqlParser.inCollectionDeclaration_return declarationReturn = new HqlParser.inCollectionDeclaration_return();
      declarationReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token OPEN");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token IN");
      RewriteRuleTokenStream rewriteRuleTokenStream3 = new RewriteRuleTokenStream(this.adaptor, "token CLOSE");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule alias");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      try
      {
        IToken el1 = (IToken) this.Match((IIntStream) this.input, 26, HqlParser.FOLLOW_IN_in_inCollectionDeclaration1416);
        rewriteRuleTokenStream2.Add(el1);
        IToken el2 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_inCollectionDeclaration1418);
        rewriteRuleTokenStream1.Add(el2);
        this.PushFollow(HqlParser.FOLLOW_path_in_inCollectionDeclaration1420);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        ruleSubtreeStream2.Add(pathReturn.Tree);
        IToken el3 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_inCollectionDeclaration1422);
        rewriteRuleTokenStream3.Add(el3);
        this.PushFollow(HqlParser.FOLLOW_alias_in_inCollectionDeclaration1424);
        HqlParser.alias_return aliasReturn = this.alias();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(aliasReturn.Tree);
        declarationReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", declarationReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(32, "join"), (object) nilNode2);
        this.adaptor.AddChild((object) astNode2, (object) (IASTNode) this.adaptor.Create(28, "inner"));
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream2.NextTree());
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        declarationReturn.Tree = (object) nilNode1;
        declarationReturn.Tree = (object) nilNode1;
        declarationReturn.Stop = (object) this.input.LT(-1);
        declarationReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(declarationReturn.Tree, (IToken) declarationReturn.Start, (IToken) declarationReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        declarationReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) declarationReturn.Start, this.input.LT(-1), ex);
      }
      return declarationReturn;
    }

    public HqlParser.inCollectionElementsDeclaration_return inCollectionElementsDeclaration()
    {
      HqlParser.inCollectionElementsDeclaration_return declarationReturn = new HqlParser.inCollectionElementsDeclaration_return();
      declarationReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token OPEN");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token AS");
      RewriteRuleTokenStream rewriteRuleTokenStream3 = new RewriteRuleTokenStream(this.adaptor, "token IN");
      RewriteRuleTokenStream rewriteRuleTokenStream4 = new RewriteRuleTokenStream(this.adaptor, "token CLOSE");
      RewriteRuleTokenStream rewriteRuleTokenStream5 = new RewriteRuleTokenStream(this.adaptor, "token ELEMENTS");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule alias");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule path");
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 17:
            num = 2;
            break;
          case 125:
            num = 1;
            break;
          default:
            throw new NoViableAltException("", 37, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            this.PushFollow(HqlParser.FOLLOW_alias_in_inCollectionElementsDeclaration1458);
            HqlParser.alias_return aliasReturn1 = this.alias();
            --this.state.followingStackPointer;
            ruleSubtreeStream1.Add(aliasReturn1.Tree);
            IToken el1 = (IToken) this.Match((IIntStream) this.input, 26, HqlParser.FOLLOW_IN_in_inCollectionElementsDeclaration1460);
            rewriteRuleTokenStream3.Add(el1);
            IToken el2 = (IToken) this.Match((IIntStream) this.input, 17, HqlParser.FOLLOW_ELEMENTS_in_inCollectionElementsDeclaration1462);
            rewriteRuleTokenStream5.Add(el2);
            IToken el3 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_inCollectionElementsDeclaration1464);
            rewriteRuleTokenStream1.Add(el3);
            this.PushFollow(HqlParser.FOLLOW_path_in_inCollectionElementsDeclaration1466);
            HqlParser.path_return pathReturn1 = this.path();
            --this.state.followingStackPointer;
            ruleSubtreeStream2.Add(pathReturn1.Tree);
            IToken el4 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_inCollectionElementsDeclaration1468);
            rewriteRuleTokenStream4.Add(el4);
            declarationReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", declarationReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(32, "join"), (object) nilNode1);
            this.adaptor.AddChild((object) astNode2, (object) (IASTNode) this.adaptor.Create(28, "inner"));
            this.adaptor.AddChild((object) astNode2, ruleSubtreeStream2.NextTree());
            this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
            this.adaptor.AddChild((object) astNode1, (object) astNode2);
            declarationReturn.Tree = (object) astNode1;
            declarationReturn.Tree = (object) astNode1;
            break;
          case 2:
            IToken el5 = (IToken) this.Match((IIntStream) this.input, 17, HqlParser.FOLLOW_ELEMENTS_in_inCollectionElementsDeclaration1490);
            rewriteRuleTokenStream5.Add(el5);
            IToken el6 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_inCollectionElementsDeclaration1492);
            rewriteRuleTokenStream1.Add(el6);
            this.PushFollow(HqlParser.FOLLOW_path_in_inCollectionElementsDeclaration1494);
            HqlParser.path_return pathReturn2 = this.path();
            --this.state.followingStackPointer;
            ruleSubtreeStream2.Add(pathReturn2.Tree);
            IToken el7 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_inCollectionElementsDeclaration1496);
            rewriteRuleTokenStream4.Add(el7);
            IToken el8 = (IToken) this.Match((IIntStream) this.input, 7, HqlParser.FOLLOW_AS_in_inCollectionElementsDeclaration1498);
            rewriteRuleTokenStream2.Add(el8);
            this.PushFollow(HqlParser.FOLLOW_alias_in_inCollectionElementsDeclaration1500);
            HqlParser.alias_return aliasReturn2 = this.alias();
            --this.state.followingStackPointer;
            ruleSubtreeStream1.Add(aliasReturn2.Tree);
            declarationReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", declarationReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(32, "join"), (object) nilNode2);
            this.adaptor.AddChild((object) astNode3, (object) (IASTNode) this.adaptor.Create(28, "inner"));
            this.adaptor.AddChild((object) astNode3, ruleSubtreeStream2.NextTree());
            this.adaptor.AddChild((object) astNode3, ruleSubtreeStream1.NextTree());
            this.adaptor.AddChild((object) astNode1, (object) astNode3);
            declarationReturn.Tree = (object) astNode1;
            declarationReturn.Tree = (object) astNode1;
            break;
        }
        declarationReturn.Stop = (object) this.input.LT(-1);
        declarationReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
        this.adaptor.SetTokenBoundaries(declarationReturn.Tree, (IToken) declarationReturn.Start, (IToken) declarationReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        declarationReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) declarationReturn.Start, this.input.LT(-1), ex);
      }
      return declarationReturn;
    }

    public HqlParser.asAlias_return asAlias()
    {
      HqlParser.asAlias_return asAliasReturn = new HqlParser.asAlias_return();
      asAliasReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        int num = 2;
        if (this.input.LA(1) == 7)
          num = 1;
        if (num == 1)
        {
          IToken token = (IToken) this.Match((IIntStream) this.input, 7, HqlParser.FOLLOW_AS_in_asAlias1532);
        }
        this.PushFollow(HqlParser.FOLLOW_alias_in_asAlias1537);
        HqlParser.alias_return aliasReturn = this.alias();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, aliasReturn.Tree);
        asAliasReturn.Stop = (object) this.input.LT(-1);
        asAliasReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(asAliasReturn.Tree, (IToken) asAliasReturn.Start, (IToken) asAliasReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        asAliasReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) asAliasReturn.Start, this.input.LT(-1), ex);
      }
      return asAliasReturn;
    }

    public HqlParser.alias_return alias()
    {
      HqlParser.alias_return aliasReturn = new HqlParser.alias_return();
      aliasReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule identifier");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_identifier_in_alias1549);
        HqlParser.identifier_return identifierReturn = this.identifier();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(identifierReturn.Tree);
        aliasReturn.Tree = (object) astNode;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", aliasReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode child = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(72, identifierReturn != null ? (IToken) identifierReturn.Start : (IToken) null), (object) nilNode2);
        this.adaptor.AddChild((object) nilNode1, (object) child);
        aliasReturn.Tree = (object) nilNode1;
        aliasReturn.Tree = (object) nilNode1;
        aliasReturn.Stop = (object) this.input.LT(-1);
        aliasReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(aliasReturn.Tree, (IToken) aliasReturn.Start, (IToken) aliasReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        aliasReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) aliasReturn.Start, this.input.LT(-1), ex);
      }
      return aliasReturn;
    }

    public HqlParser.propertyFetch_return propertyFetch()
    {
      HqlParser.propertyFetch_return propertyFetchReturn = new HqlParser.propertyFetch_return();
      propertyFetchReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 21, HqlParser.FOLLOW_FETCH_in_propertyFetch1568));
        this.adaptor.AddChild((object) nilNode, (object) child);
        IToken token1 = (IToken) this.Match((IIntStream) this.input, 4, HqlParser.FOLLOW_ALL_in_propertyFetch1570);
        IToken token2 = (IToken) this.Match((IIntStream) this.input, 43, HqlParser.FOLLOW_PROPERTIES_in_propertyFetch1573);
        propertyFetchReturn.Stop = (object) this.input.LT(-1);
        propertyFetchReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(propertyFetchReturn.Tree, (IToken) propertyFetchReturn.Start, (IToken) propertyFetchReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        propertyFetchReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) propertyFetchReturn.Start, this.input.LT(-1), ex);
      }
      return propertyFetchReturn;
    }

    public HqlParser.groupByClause_return groupByClause()
    {
      HqlParser.groupByClause_return groupByClauseReturn = new HqlParser.groupByClause_return();
      groupByClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 24, HqlParser.FOLLOW_GROUP_in_groupByClause1585)), (object) nilNode);
        IToken token1 = (IToken) this.Match((IIntStream) this.input, 56, HqlParser.FOLLOW_LITERAL_by_in_groupByClause1591);
        this.PushFollow(HqlParser.FOLLOW_expression_in_groupByClause1594);
        HqlParser.expression_return expressionReturn1 = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 101)
            num = 1;
          if (num == 1)
          {
            IToken token2 = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_groupByClause1598);
            this.PushFollow(HqlParser.FOLLOW_expression_in_groupByClause1601);
            HqlParser.expression_return expressionReturn2 = this.expression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
          }
          else
            break;
        }
        groupByClauseReturn.Stop = (object) this.input.LT(-1);
        groupByClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(groupByClauseReturn.Tree, (IToken) groupByClauseReturn.Start, (IToken) groupByClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        groupByClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) groupByClauseReturn.Start, this.input.LT(-1), ex);
      }
      return groupByClauseReturn;
    }

    public HqlParser.orderByClause_return orderByClause()
    {
      HqlParser.orderByClause_return orderByClauseReturn = new HqlParser.orderByClause_return();
      orderByClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 41, HqlParser.FOLLOW_ORDER_in_orderByClause1615)), (object) nilNode);
        IToken token1 = (IToken) this.Match((IIntStream) this.input, 56, HqlParser.FOLLOW_LITERAL_by_in_orderByClause1618);
        this.PushFollow(HqlParser.FOLLOW_orderElement_in_orderByClause1621);
        HqlParser.orderElement_return orderElementReturn1 = this.orderElement();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, orderElementReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 101)
            num = 1;
          if (num == 1)
          {
            IToken token2 = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_orderByClause1625);
            this.PushFollow(HqlParser.FOLLOW_orderElement_in_orderByClause1628);
            HqlParser.orderElement_return orderElementReturn2 = this.orderElement();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, orderElementReturn2.Tree);
          }
          else
            break;
        }
        orderByClauseReturn.Stop = (object) this.input.LT(-1);
        orderByClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(orderByClauseReturn.Tree, (IToken) orderByClauseReturn.Start, (IToken) orderByClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        orderByClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) orderByClauseReturn.Start, this.input.LT(-1), ex);
      }
      return orderByClauseReturn;
    }

    public HqlParser.skipClause_return skipClause()
    {
      HqlParser.skipClause_return skipClauseReturn = new HqlParser.skipClause_return();
      skipClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 47, HqlParser.FOLLOW_SKIP_in_skipClause1642)), (object) nilNode);
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
            throw new NoViableAltException("", 41, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 95, HqlParser.FOLLOW_NUM_INT_in_skipClause1646));
            this.adaptor.AddChild((object) astNode, (object) child);
            break;
          case 2:
            this.PushFollow(HqlParser.FOLLOW_parameter_in_skipClause1650);
            HqlParser.parameter_return parameterReturn = this.parameter();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, parameterReturn.Tree);
            break;
        }
        skipClauseReturn.Stop = (object) this.input.LT(-1);
        skipClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(skipClauseReturn.Tree, (IToken) skipClauseReturn.Start, (IToken) skipClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        skipClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) skipClauseReturn.Start, this.input.LT(-1), ex);
      }
      return skipClauseReturn;
    }

    public HqlParser.takeClause_return takeClause()
    {
      HqlParser.takeClause_return clause = new HqlParser.takeClause_return();
      clause.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 50, HqlParser.FOLLOW_TAKE_in_takeClause1662)), (object) nilNode);
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
            throw new NoViableAltException("", 42, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 95, HqlParser.FOLLOW_NUM_INT_in_takeClause1666));
            this.adaptor.AddChild((object) astNode, (object) child);
            break;
          case 2:
            this.PushFollow(HqlParser.FOLLOW_parameter_in_takeClause1670);
            HqlParser.parameter_return parameterReturn = this.parameter();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, parameterReturn.Tree);
            break;
        }
        clause.Stop = (object) this.input.LT(-1);
        clause.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(clause.Tree, (IToken) clause.Start, (IToken) clause.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        clause.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) clause.Start, this.input.LT(-1), ex);
      }
      return clause;
    }

    public HqlParser.parameter_return parameter()
    {
      HqlParser.parameter_return parameterReturn = new HqlParser.parameter_return();
      parameterReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
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
            throw new NoViableAltException("", 44, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 105, HqlParser.FOLLOW_COLON_in_parameter1682)), (object) nilNode1);
            this.PushFollow(HqlParser.FOLLOW_identifier_in_parameter1685);
            HqlParser.identifier_return identifierReturn = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, identifierReturn.Tree);
            break;
          case 2:
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 106, HqlParser.FOLLOW_PARAM_in_parameter1690)), (object) nilNode2);
            int num2 = 2;
            if (this.input.LA(1) == 95)
              num2 = 1;
            if (num2 == 1)
            {
              IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 95, HqlParser.FOLLOW_NUM_INT_in_parameter1694));
              this.adaptor.AddChild((object) astNode, (object) child);
              break;
            }
            break;
        }
        parameterReturn.Stop = (object) this.input.LT(-1);
        parameterReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(parameterReturn.Tree, (IToken) parameterReturn.Start, (IToken) parameterReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        parameterReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) parameterReturn.Start, this.input.LT(-1), ex);
      }
      return parameterReturn;
    }

    public HqlParser.orderElement_return orderElement()
    {
      HqlParser.orderElement_return orderElementReturn = new HqlParser.orderElement_return();
      orderElementReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_expression_in_orderElement1707);
        HqlParser.expression_return expressionReturn = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, expressionReturn.Tree);
        int num = 2;
        switch (this.input.LA(1))
        {
          case 8:
          case 14:
          case 133:
          case 134:
            num = 1;
            break;
        }
        if (num == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_ascendingOrDescending_in_orderElement1711);
          HqlParser.ascendingOrDescending_return descendingReturn = this.ascendingOrDescending();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) nilNode, descendingReturn.Tree);
        }
        orderElementReturn.Stop = (object) this.input.LT(-1);
        orderElementReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(orderElementReturn.Tree, (IToken) orderElementReturn.Start, (IToken) orderElementReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        orderElementReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) orderElementReturn.Start, this.input.LT(-1), ex);
      }
      return orderElementReturn;
    }

    public HqlParser.ascendingOrDescending_return ascendingOrDescending()
    {
      HqlParser.ascendingOrDescending_return descendingReturn = new HqlParser.ascendingOrDescending_return();
      descendingReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      IToken el1 = (IToken) null;
      IToken el2 = (IToken) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token 134");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token 133");
      RewriteRuleTokenStream rewriteRuleTokenStream3 = new RewriteRuleTokenStream(this.adaptor, "token DESCENDING");
      RewriteRuleTokenStream rewriteRuleTokenStream4 = new RewriteRuleTokenStream(this.adaptor, "token ASCENDING");
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 8:
          case 133:
            num1 = 1;
            break;
          case 14:
          case 134:
            num1 = 2;
            break;
          default:
            throw new NoViableAltException("", 48, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            int num2;
            switch (this.input.LA(1))
            {
              case 8:
                num2 = 1;
                break;
              case 133:
                num2 = 2;
                break;
              default:
                throw new NoViableAltException("", 46, 0, (IIntStream) this.input);
            }
            switch (num2)
            {
              case 1:
                el1 = (IToken) this.Match((IIntStream) this.input, 8, HqlParser.FOLLOW_ASCENDING_in_ascendingOrDescending1729);
                rewriteRuleTokenStream4.Add(el1);
                break;
              case 2:
                el1 = (IToken) this.Match((IIntStream) this.input, 133, HqlParser.FOLLOW_133_in_ascendingOrDescending1735);
                rewriteRuleTokenStream2.Add(el1);
                break;
            }
            descendingReturn.Tree = (object) astNode;
            RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", descendingReturn?.Tree);
            astNode = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child1 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(8, el1.Text), (object) nilNode1);
            this.adaptor.AddChild((object) astNode, (object) child1);
            descendingReturn.Tree = (object) astNode;
            descendingReturn.Tree = (object) astNode;
            break;
          case 2:
            int num3;
            switch (this.input.LA(1))
            {
              case 14:
                num3 = 1;
                break;
              case 134:
                num3 = 2;
                break;
              default:
                throw new NoViableAltException("", 47, 0, (IIntStream) this.input);
            }
            switch (num3)
            {
              case 1:
                el2 = (IToken) this.Match((IIntStream) this.input, 14, HqlParser.FOLLOW_DESCENDING_in_ascendingOrDescending1755);
                rewriteRuleTokenStream3.Add(el2);
                break;
              case 2:
                el2 = (IToken) this.Match((IIntStream) this.input, 134, HqlParser.FOLLOW_134_in_ascendingOrDescending1761);
                rewriteRuleTokenStream1.Add(el2);
                break;
            }
            descendingReturn.Tree = (object) astNode;
            RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", descendingReturn?.Tree);
            astNode = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(14, el2.Text), (object) nilNode2);
            this.adaptor.AddChild((object) astNode, (object) child2);
            descendingReturn.Tree = (object) astNode;
            descendingReturn.Tree = (object) astNode;
            break;
        }
        descendingReturn.Stop = (object) this.input.LT(-1);
        descendingReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(descendingReturn.Tree, (IToken) descendingReturn.Start, (IToken) descendingReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        descendingReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) descendingReturn.Start, this.input.LT(-1), ex);
      }
      return descendingReturn;
    }

    public HqlParser.havingClause_return havingClause()
    {
      HqlParser.havingClause_return havingClauseReturn = new HqlParser.havingClause_return();
      havingClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 25, HqlParser.FOLLOW_HAVING_in_havingClause1782)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_logicalExpression_in_havingClause1785);
        HqlParser.logicalExpression_return expressionReturn = this.logicalExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn.Tree);
        havingClauseReturn.Stop = (object) this.input.LT(-1);
        havingClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(havingClauseReturn.Tree, (IToken) havingClauseReturn.Start, (IToken) havingClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        havingClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) havingClauseReturn.Start, this.input.LT(-1), ex);
      }
      return havingClauseReturn;
    }

    public HqlParser.whereClause_return whereClause()
    {
      HqlParser.whereClause_return whereClauseReturn = new HqlParser.whereClause_return();
      whereClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 55, HqlParser.FOLLOW_WHERE_in_whereClause1796)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_logicalExpression_in_whereClause1799);
        HqlParser.logicalExpression_return expressionReturn = this.logicalExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn.Tree);
        whereClauseReturn.Stop = (object) this.input.LT(-1);
        whereClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(whereClauseReturn.Tree, (IToken) whereClauseReturn.Start, (IToken) whereClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        whereClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) whereClauseReturn.Start, this.input.LT(-1), ex);
      }
      return whereClauseReturn;
    }

    public HqlParser.selectedPropertiesList_return selectedPropertiesList()
    {
      HqlParser.selectedPropertiesList_return propertiesListReturn = new HqlParser.selectedPropertiesList_return();
      propertiesListReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_aliasedExpression_in_selectedPropertiesList1810);
        HqlParser.aliasedExpression_return expressionReturn1 = this.aliasedExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, expressionReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 101)
            num = 1;
          if (num == 1)
          {
            IToken token = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_selectedPropertiesList1814);
            this.PushFollow(HqlParser.FOLLOW_aliasedExpression_in_selectedPropertiesList1817);
            HqlParser.aliasedExpression_return expressionReturn2 = this.aliasedExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, expressionReturn2.Tree);
          }
          else
            break;
        }
        propertiesListReturn.Stop = (object) this.input.LT(-1);
        propertiesListReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(propertiesListReturn.Tree, (IToken) propertiesListReturn.Start, (IToken) propertiesListReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        propertiesListReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) propertiesListReturn.Start, this.input.LT(-1), ex);
      }
      return propertiesListReturn;
    }

    public HqlParser.aliasedExpression_return aliasedExpression()
    {
      HqlParser.aliasedExpression_return expressionReturn1 = new HqlParser.aliasedExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_expression_in_aliasedExpression1832);
        HqlParser.expression_return expressionReturn2 = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        int num = 2;
        if (this.input.LA(1) == 7)
          num = 1;
        if (num == 1)
        {
          astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 7, HqlParser.FOLLOW_AS_in_aliasedExpression1836)), (object) astNode);
          this.PushFollow(HqlParser.FOLLOW_identifier_in_aliasedExpression1839);
          HqlParser.identifier_return identifierReturn = this.identifier();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode, identifierReturn.Tree);
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.logicalExpression_return logicalExpression()
    {
      HqlParser.logicalExpression_return expressionReturn1 = new HqlParser.logicalExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_expression_in_logicalExpression1878);
        HqlParser.expression_return expressionReturn2 = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, expressionReturn2.Tree);
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.expression_return expression()
    {
      HqlParser.expression_return expressionReturn1 = new HqlParser.expression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_logicalOrExpression_in_expression1890);
        HqlParser.logicalOrExpression_return expressionReturn2 = this.logicalOrExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, expressionReturn2.Tree);
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.logicalOrExpression_return logicalOrExpression()
    {
      HqlParser.logicalOrExpression_return expressionReturn1 = new HqlParser.logicalOrExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_logicalAndExpression_in_logicalOrExpression1902);
        HqlParser.logicalAndExpression_return expressionReturn2 = this.logicalAndExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 40)
            num = 1;
          if (num == 1)
          {
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 40, HqlParser.FOLLOW_OR_in_logicalOrExpression1906)), (object) astNode);
            this.PushFollow(HqlParser.FOLLOW_logicalAndExpression_in_logicalOrExpression1909);
            HqlParser.logicalAndExpression_return expressionReturn3 = this.logicalAndExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.logicalAndExpression_return logicalAndExpression()
    {
      HqlParser.logicalAndExpression_return expressionReturn1 = new HqlParser.logicalAndExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_negatedExpression_in_logicalAndExpression1924);
        HqlParser.negatedExpression_return expressionReturn2 = this.negatedExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 6)
            num = 1;
          if (num == 1)
          {
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 6, HqlParser.FOLLOW_AND_in_logicalAndExpression1928)), (object) astNode);
            this.PushFollow(HqlParser.FOLLOW_negatedExpression_in_logicalAndExpression1931);
            HqlParser.negatedExpression_return expressionReturn3 = this.negatedExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.negatedExpression_return negatedExpression()
    {
      HqlParser.negatedExpression_return expressionReturn1 = new HqlParser.negatedExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream = new RewriteRuleTokenStream(this.adaptor, "token NOT");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule equalityExpression");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule negatedExpression");
      this.WeakKeywords();
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 38)
        {
          num2 = 1;
        }
        else
        {
          if ((num1 < 4 || num1 > 5) && num1 != 9 && num1 != 12 && num1 != 17 && (num1 < 19 || num1 > 20) && num1 != 27 && (num1 < 35 || num1 > 36) && num1 != 39 && (num1 < 48 || num1 > 49) && num1 != 51 && num1 != 57 && num1 != 65 && (num1 < 95 || num1 > 99) && num1 != 103 && (num1 < 105 || num1 > 106) && num1 != 114 && (num1 < 118 || num1 > 119) && (num1 < 124 || num1 > 125))
            throw new NoViableAltException("", 53, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            IToken el = (IToken) this.Match((IIntStream) this.input, 38, HqlParser.FOLLOW_NOT_in_negatedExpression1952);
            rewriteRuleTokenStream.Add(el);
            this.PushFollow(HqlParser.FOLLOW_negatedExpression_in_negatedExpression1956);
            HqlParser.negatedExpression_return expressionReturn2 = this.negatedExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream2.Add(expressionReturn2.Tree);
            expressionReturn1.Tree = (object) astNode;
            RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            astNode = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child1 = (IASTNode) this.adaptor.BecomeRoot((object) this.NegateNode(expressionReturn2 != null ? (IASTNode) expressionReturn2.Tree : (IASTNode) null), (object) nilNode1);
            this.adaptor.AddChild((object) astNode, (object) child1);
            expressionReturn1.Tree = (object) astNode;
            expressionReturn1.Tree = (object) astNode;
            break;
          case 2:
            this.PushFollow(HqlParser.FOLLOW_equalityExpression_in_negatedExpression1969);
            HqlParser.equalityExpression_return expressionReturn3 = this.equalityExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream1.Add(expressionReturn3.Tree);
            expressionReturn1.Tree = (object) astNode;
            RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            astNode = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child2 = (IASTNode) this.adaptor.BecomeRoot(ruleSubtreeStream1.NextNode(), (object) nilNode2);
            this.adaptor.AddChild((object) astNode, (object) child2);
            expressionReturn1.Tree = (object) astNode;
            expressionReturn1.Tree = (object) astNode;
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.equalityExpression_return equalityExpression()
    {
      HqlParser.equalityExpression_return expressionReturn1 = new HqlParser.equalityExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_relationalExpression_in_equalityExpression1999);
        HqlParser.relationalExpression_return expressionReturn2 = this.relationalExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num1 = 2;
          switch (this.input.LA(1))
          {
            case 31:
            case 102:
            case 107:
            case 108:
              num1 = 1;
              break;
          }
          if (num1 == 1)
          {
            int num2;
            switch (this.input.LA(1))
            {
              case 31:
                num2 = 2;
                break;
              case 102:
                num2 = 1;
                break;
              case 107:
                num2 = 3;
                break;
              case 108:
                num2 = 4;
                break;
              default:
                goto label_10;
            }
            switch (num2)
            {
              case 1:
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 102, HqlParser.FOLLOW_EQ_in_equalityExpression2007)), (object) astNode);
                break;
              case 2:
                IToken payload1 = (IToken) this.Match((IIntStream) this.input, 31, HqlParser.FOLLOW_IS_in_equalityExpression2016);
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload1), (object) astNode);
                payload1.Type = 102;
                int num3 = 2;
                if (this.input.LA(1) == 38)
                  num3 = 1;
                if (num3 == 1)
                {
                  IToken token = (IToken) this.Match((IIntStream) this.input, 38, HqlParser.FOLLOW_NOT_in_equalityExpression2022);
                  payload1.Type = 107;
                  break;
                }
                break;
              case 3:
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 107, HqlParser.FOLLOW_NE_in_equalityExpression2034)), (object) astNode);
                break;
              case 4:
                IToken payload2 = (IToken) this.Match((IIntStream) this.input, 108, HqlParser.FOLLOW_SQL_NE_in_equalityExpression2043);
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload2), (object) astNode);
                payload2.Type = 107;
                break;
            }
            this.PushFollow(HqlParser.FOLLOW_relationalExpression_in_equalityExpression2054);
            HqlParser.relationalExpression_return expressionReturn3 = this.relationalExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            goto label_20;
        }
label_10:
        throw new NoViableAltException("", 55, 0, (IIntStream) this.input);
label_20:
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
        expressionReturn1.Tree = (object) this.ProcessEqualityExpression((object) (IASTNode) expressionReturn1.Tree);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.relationalExpression_return relationalExpression()
    {
      HqlParser.relationalExpression_return expressionReturn1 = new HqlParser.relationalExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      IToken n = (IToken) null;
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_concatenation_in_relationalExpression2071);
        HqlParser.concatenation_return concatenationReturn1 = this.concatenation();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, concatenationReturn1.Tree);
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == -1 || num1 >= 6 && num1 <= 8 || num1 == 14 || num1 >= 22 && num1 <= 25 || num1 == 28 || num1 >= 31 && num1 <= 33 || num1 >= 40 && num1 <= 41 || num1 == 44 || num1 == 47 || num1 == 50 || num1 == 52 || num1 == 55 || num1 >= 58 && num1 <= 61 || num1 >= 101 && num1 <= 102 || num1 == 104 || num1 >= 107 && num1 <= 112 || num1 == 123 || num1 >= 133 && num1 <= 134)
        {
          num2 = 1;
        }
        else
        {
          if (num1 != 10 && num1 != 26 && num1 != 34 && num1 != 38 && num1 != 67)
            throw new NoViableAltException("", 62, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            while (true)
            {
              int num3 = 2;
              switch (this.input.LA(1))
              {
                case 109:
                case 110:
                case 111:
                case 112:
                  num3 = 1;
                  break;
              }
              if (num3 == 1)
              {
                int num4;
                switch (this.input.LA(1))
                {
                  case 109:
                    num4 = 1;
                    break;
                  case 110:
                    num4 = 2;
                    break;
                  case 111:
                    num4 = 3;
                    break;
                  case 112:
                    num4 = 4;
                    break;
                  default:
                    goto label_15;
                }
                switch (num4)
                {
                  case 1:
                    astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 109, HqlParser.FOLLOW_LT_in_relationalExpression2083)), (object) astNode);
                    break;
                  case 2:
                    astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 110, HqlParser.FOLLOW_GT_in_relationalExpression2088)), (object) astNode);
                    break;
                  case 3:
                    astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 111, HqlParser.FOLLOW_LE_in_relationalExpression2093)), (object) astNode);
                    break;
                  case 4:
                    astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 112, HqlParser.FOLLOW_GE_in_relationalExpression2098)), (object) astNode);
                    break;
                }
                this.PushFollow(HqlParser.FOLLOW_bitwiseNotExpression_in_relationalExpression2103);
                HqlParser.bitwiseNotExpression_return expressionReturn2 = this.bitwiseNotExpression();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
              }
              else
                goto label_41;
            }
label_15:
            throw new NoViableAltException("", 57, 0, (IIntStream) this.input);
          case 2:
            int num5 = 2;
            if (this.input.LA(1) == 38)
              num5 = 1;
            if (num5 == 1)
              n = (IToken) this.Match((IIntStream) this.input, 38, HqlParser.FOLLOW_NOT_in_relationalExpression2120);
            int num6;
            switch (this.input.LA(1))
            {
              case 10:
                num6 = 2;
                break;
              case 26:
                num6 = 1;
                break;
              case 34:
                num6 = 3;
                break;
              case 67:
                num6 = 4;
                break;
              default:
                throw new NoViableAltException("", 61, 0, (IIntStream) this.input);
            }
            switch (num6)
            {
              case 1:
                IToken payload1 = (IToken) this.Match((IIntStream) this.input, 26, HqlParser.FOLLOW_IN_in_relationalExpression2141);
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload1), (object) astNode);
                payload1.Type = n == null ? 26 : 83;
                payload1.Text = n == null ? "in" : "not in";
                this.PushFollow(HqlParser.FOLLOW_inList_in_relationalExpression2150);
                HqlParser.inList_return inListReturn = this.inList();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, inListReturn.Tree);
                break;
              case 2:
                IToken payload2 = (IToken) this.Match((IIntStream) this.input, 10, HqlParser.FOLLOW_BETWEEN_in_relationalExpression2161);
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload2), (object) astNode);
                payload2.Type = n == null ? 10 : 82;
                payload2.Text = n == null ? "between" : "not between";
                this.PushFollow(HqlParser.FOLLOW_betweenList_in_relationalExpression2170);
                HqlParser.betweenList_return betweenListReturn = this.betweenList();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, betweenListReturn.Tree);
                break;
              case 3:
                IToken payload3 = (IToken) this.Match((IIntStream) this.input, 34, HqlParser.FOLLOW_LIKE_in_relationalExpression2182);
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload3), (object) astNode);
                payload3.Type = n == null ? 34 : 84;
                payload3.Text = n == null ? "like" : "not like";
                this.PushFollow(HqlParser.FOLLOW_concatenation_in_relationalExpression2191);
                HqlParser.concatenation_return concatenationReturn2 = this.concatenation();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, concatenationReturn2.Tree);
                this.PushFollow(HqlParser.FOLLOW_likeEscape_in_relationalExpression2193);
                HqlParser.likeEscape_return likeEscapeReturn = this.likeEscape();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, likeEscapeReturn.Tree);
                break;
              case 4:
                IToken token1 = (IToken) this.Match((IIntStream) this.input, 67, HqlParser.FOLLOW_MEMBER_in_relationalExpression2202);
                int num7 = 2;
                if (this.input.LA(1) == 69)
                  num7 = 1;
                if (num7 == 1)
                {
                  IToken token2 = (IToken) this.Match((IIntStream) this.input, 69, HqlParser.FOLLOW_OF_in_relationalExpression2206);
                }
                this.PushFollow(HqlParser.FOLLOW_path_in_relationalExpression2213);
                HqlParser.path_return pathReturn = this.path();
                --this.state.followingStackPointer;
                astNode = this.ProcessMemberOf(n, pathReturn != null ? (IASTNode) pathReturn.Tree : (IASTNode) null, astNode);
                break;
            }
            break;
        }
label_41:
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.likeEscape_return likeEscape()
    {
      HqlParser.likeEscape_return likeEscapeReturn = new HqlParser.likeEscape_return();
      likeEscapeReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        int num = 2;
        if (this.input.LA(1) == 18)
          num = 1;
        if (num == 1)
        {
          astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 18, HqlParser.FOLLOW_ESCAPE_in_likeEscape2240)), (object) astNode);
          this.PushFollow(HqlParser.FOLLOW_concatenation_in_likeEscape2243);
          HqlParser.concatenation_return concatenationReturn = this.concatenation();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode, concatenationReturn.Tree);
        }
        likeEscapeReturn.Stop = (object) this.input.LT(-1);
        likeEscapeReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(likeEscapeReturn.Tree, (IToken) likeEscapeReturn.Start, (IToken) likeEscapeReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        likeEscapeReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) likeEscapeReturn.Start, this.input.LT(-1), ex);
      }
      return likeEscapeReturn;
    }

    public HqlParser.inList_return inList()
    {
      HqlParser.inList_return inListReturn = new HqlParser.inList_return();
      inListReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule compoundExpr");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_compoundExpr_in_inList2256);
        HqlParser.compoundExpr_return compoundExprReturn = this.compoundExpr();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(compoundExprReturn.Tree);
        inListReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", inListReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(77, nameof (inList)), (object) nilNode2);
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        inListReturn.Tree = (object) nilNode1;
        inListReturn.Tree = (object) nilNode1;
        inListReturn.Stop = (object) this.input.LT(-1);
        inListReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(inListReturn.Tree, (IToken) inListReturn.Start, (IToken) inListReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        inListReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) inListReturn.Start, this.input.LT(-1), ex);
      }
      return inListReturn;
    }

    public HqlParser.betweenList_return betweenList()
    {
      HqlParser.betweenList_return betweenListReturn = new HqlParser.betweenList_return();
      betweenListReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_concatenation_in_betweenList2277);
        HqlParser.concatenation_return concatenationReturn1 = this.concatenation();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, concatenationReturn1.Tree);
        IToken token = (IToken) this.Match((IIntStream) this.input, 6, HqlParser.FOLLOW_AND_in_betweenList2279);
        this.PushFollow(HqlParser.FOLLOW_concatenation_in_betweenList2282);
        HqlParser.concatenation_return concatenationReturn2 = this.concatenation();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, concatenationReturn2.Tree);
        betweenListReturn.Stop = (object) this.input.LT(-1);
        betweenListReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(betweenListReturn.Tree, (IToken) betweenListReturn.Start, (IToken) betweenListReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        betweenListReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) betweenListReturn.Start, this.input.LT(-1), ex);
      }
      return betweenListReturn;
    }

    public HqlParser.concatenation_return concatenation()
    {
      HqlParser.concatenation_return concatenationReturn = new HqlParser.concatenation_return();
      concatenationReturn.Start = (object) this.input.LT(1);
      IToken payload = (IToken) null;
      try
      {
        IASTNode astNode1 = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_bitwiseNotExpression_in_concatenation2301);
        HqlParser.bitwiseNotExpression_return expressionReturn1 = this.bitwiseNotExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode1, expressionReturn1.Tree);
        int num1 = 2;
        if (this.input.LA(1) == 113)
          num1 = 1;
        if (num1 == 1)
        {
          payload = (IToken) this.Match((IIntStream) this.input, 113, HqlParser.FOLLOW_CONCAT_in_concatenation2309);
          astNode1 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload), (object) astNode1);
          payload.Type = 75;
          payload.Text = "concatList";
          this.PushFollow(HqlParser.FOLLOW_bitwiseNotExpression_in_concatenation2318);
          HqlParser.bitwiseNotExpression_return expressionReturn2 = this.bitwiseNotExpression();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode1, expressionReturn2.Tree);
          while (true)
          {
            int num2 = 2;
            if (this.input.LA(1) == 113)
              num2 = 1;
            if (num2 == 1)
            {
              IToken token = (IToken) this.Match((IIntStream) this.input, 113, HqlParser.FOLLOW_CONCAT_in_concatenation2325);
              this.PushFollow(HqlParser.FOLLOW_bitwiseNotExpression_in_concatenation2328);
              HqlParser.bitwiseNotExpression_return expressionReturn3 = this.bitwiseNotExpression();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode1, expressionReturn3.Tree);
            }
            else
              break;
          }
        }
        concatenationReturn.Stop = (object) this.input.LT(-1);
        concatenationReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
        this.adaptor.SetTokenBoundaries(concatenationReturn.Tree, (IToken) concatenationReturn.Start, (IToken) concatenationReturn.Stop);
        if (payload != null)
        {
          IASTNode astNode2 = (IASTNode) this.adaptor.Create(81, "||");
          IASTNode childNode = (IASTNode) this.adaptor.Create(125, "concat");
          astNode2.AddChild(childNode);
          astNode2.AddChild((IASTNode) concatenationReturn.Tree);
          concatenationReturn.Tree = (object) astNode2;
        }
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        concatenationReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) concatenationReturn.Start, this.input.LT(-1), ex);
      }
      return concatenationReturn;
    }

    public HqlParser.bitwiseNotExpression_return bitwiseNotExpression()
    {
      HqlParser.bitwiseNotExpression_return expressionReturn1 = new HqlParser.bitwiseNotExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 114)
        {
          num2 = 1;
        }
        else
        {
          if ((num1 < 4 || num1 > 5) && num1 != 9 && num1 != 12 && num1 != 17 && (num1 < 19 || num1 > 20) && num1 != 27 && (num1 < 35 || num1 > 36) && num1 != 39 && (num1 < 48 || num1 > 49) && num1 != 51 && num1 != 57 && num1 != 65 && (num1 < 95 || num1 > 99) && num1 != 103 && (num1 < 105 || num1 > 106) && (num1 < 118 || num1 > 119) && (num1 < 124 || num1 > 125))
            throw new NoViableAltException("", 66, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 114, HqlParser.FOLLOW_BNOT_in_bitwiseNotExpression2352)), (object) nilNode);
            this.PushFollow(HqlParser.FOLLOW_bitwiseOrExpression_in_bitwiseNotExpression2355);
            HqlParser.bitwiseOrExpression_return expressionReturn2 = this.bitwiseOrExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
            break;
          case 2:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_bitwiseOrExpression_in_bitwiseNotExpression2361);
            HqlParser.bitwiseOrExpression_return expressionReturn3 = this.bitwiseOrExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.bitwiseOrExpression_return bitwiseOrExpression()
    {
      HqlParser.bitwiseOrExpression_return expressionReturn1 = new HqlParser.bitwiseOrExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_bitwiseXOrExpression_in_bitwiseOrExpression2373);
        HqlParser.bitwiseXOrExpression_return expressionReturn2 = this.bitwiseXOrExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 115)
            num = 1;
          if (num == 1)
          {
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 115, HqlParser.FOLLOW_BOR_in_bitwiseOrExpression2376)), (object) astNode);
            this.PushFollow(HqlParser.FOLLOW_bitwiseXOrExpression_in_bitwiseOrExpression2379);
            HqlParser.bitwiseXOrExpression_return expressionReturn3 = this.bitwiseXOrExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.bitwiseXOrExpression_return bitwiseXOrExpression()
    {
      HqlParser.bitwiseXOrExpression_return expressionReturn1 = new HqlParser.bitwiseXOrExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_bitwiseAndExpression_in_bitwiseXOrExpression2393);
        HqlParser.bitwiseAndExpression_return expressionReturn2 = this.bitwiseAndExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 116)
            num = 1;
          if (num == 1)
          {
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 116, HqlParser.FOLLOW_BXOR_in_bitwiseXOrExpression2396)), (object) astNode);
            this.PushFollow(HqlParser.FOLLOW_bitwiseAndExpression_in_bitwiseXOrExpression2399);
            HqlParser.bitwiseAndExpression_return expressionReturn3 = this.bitwiseAndExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.bitwiseAndExpression_return bitwiseAndExpression()
    {
      HqlParser.bitwiseAndExpression_return expressionReturn1 = new HqlParser.bitwiseAndExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_additiveExpression_in_bitwiseAndExpression2413);
        HqlParser.additiveExpression_return expressionReturn2 = this.additiveExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 117)
            num = 1;
          if (num == 1)
          {
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 117, HqlParser.FOLLOW_BAND_in_bitwiseAndExpression2416)), (object) astNode);
            this.PushFollow(HqlParser.FOLLOW_additiveExpression_in_bitwiseAndExpression2419);
            HqlParser.additiveExpression_return expressionReturn3 = this.additiveExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.additiveExpression_return additiveExpression()
    {
      HqlParser.additiveExpression_return expressionReturn1 = new HqlParser.additiveExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_multiplyExpression_in_additiveExpression2433);
        HqlParser.multiplyExpression_return expressionReturn2 = this.multiplyExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num1 = 2;
          switch (this.input.LA(1))
          {
            case 118:
            case 119:
              num1 = 1;
              break;
          }
          if (num1 == 1)
          {
            int num2;
            switch (this.input.LA(1))
            {
              case 118:
                num2 = 1;
                break;
              case 119:
                num2 = 2;
                break;
              default:
                goto label_8;
            }
            switch (num2)
            {
              case 1:
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 118, HqlParser.FOLLOW_PLUS_in_additiveExpression2439)), (object) astNode);
                break;
              case 2:
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 119, HqlParser.FOLLOW_MINUS_in_additiveExpression2444)), (object) astNode);
                break;
            }
            this.PushFollow(HqlParser.FOLLOW_multiplyExpression_in_additiveExpression2449);
            HqlParser.multiplyExpression_return expressionReturn3 = this.multiplyExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            goto label_13;
        }
label_8:
        throw new NoViableAltException("", 70, 0, (IIntStream) this.input);
label_13:
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.multiplyExpression_return multiplyExpression()
    {
      HqlParser.multiplyExpression_return expressionReturn1 = new HqlParser.multiplyExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_unaryExpression_in_multiplyExpression2464);
        HqlParser.unaryExpression_return expressionReturn2 = this.unaryExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        while (true)
        {
          int num1 = 2;
          switch (this.input.LA(1))
          {
            case 120:
            case 121:
              num1 = 1;
              break;
          }
          if (num1 == 1)
          {
            int num2;
            switch (this.input.LA(1))
            {
              case 120:
                num2 = 1;
                break;
              case 121:
                num2 = 2;
                break;
              default:
                goto label_8;
            }
            switch (num2)
            {
              case 1:
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 120, HqlParser.FOLLOW_STAR_in_multiplyExpression2470)), (object) astNode);
                break;
              case 2:
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 121, HqlParser.FOLLOW_DIV_in_multiplyExpression2475)), (object) astNode);
                break;
            }
            this.PushFollow(HqlParser.FOLLOW_unaryExpression_in_multiplyExpression2480);
            HqlParser.unaryExpression_return expressionReturn3 = this.unaryExpression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, expressionReturn3.Tree);
          }
          else
            goto label_13;
        }
label_8:
        throw new NoViableAltException("", 72, 0, (IIntStream) this.input);
label_13:
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.unaryExpression_return unaryExpression()
    {
      HqlParser.unaryExpression_return expressionReturn1 = new HqlParser.unaryExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token PLUS");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token MINUS");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule atom");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule caseExpression");
      RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule quantifiedExpression");
      RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule unaryExpression");
      try
      {
        int num;
        switch (this.input.LA(1))
        {
          case 4:
          case 5:
          case 19:
          case 48:
            num = 4;
            break;
          case 9:
          case 12:
          case 17:
          case 20:
          case 27:
          case 35:
          case 36:
          case 39:
          case 49:
          case 51:
          case 65:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 103:
          case 105:
          case 106:
          case 124:
          case 125:
            num = 5;
            break;
          case 57:
            num = 3;
            break;
          case 118:
            num = 2;
            break;
          case 119:
            num = 1;
            break;
          default:
            throw new NoViableAltException("", 74, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            IToken token1 = (IToken) this.Match((IIntStream) this.input, 119, HqlParser.FOLLOW_MINUS_in_unaryExpression2498);
            rewriteRuleTokenStream2.Add(token1);
            this.PushFollow(HqlParser.FOLLOW_unaryExpression_in_unaryExpression2502);
            HqlParser.unaryExpression_return expressionReturn2 = this.unaryExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream4.Add(expressionReturn2.Tree);
            expressionReturn1.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            RewriteRuleSubtreeStream ruleSubtreeStream6 = new RewriteRuleSubtreeStream(this.adaptor, "rule mu", expressionReturn2?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(90, token1), (object) nilNode1);
            this.adaptor.AddChild((object) astNode2, ruleSubtreeStream6.NextTree());
            this.adaptor.AddChild((object) astNode1, (object) astNode2);
            expressionReturn1.Tree = (object) astNode1;
            expressionReturn1.Tree = (object) astNode1;
            break;
          case 2:
            IToken token2 = (IToken) this.Match((IIntStream) this.input, 118, HqlParser.FOLLOW_PLUS_in_unaryExpression2519);
            rewriteRuleTokenStream1.Add(token2);
            this.PushFollow(HqlParser.FOLLOW_unaryExpression_in_unaryExpression2523);
            HqlParser.unaryExpression_return expressionReturn3 = this.unaryExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream4.Add(expressionReturn3.Tree);
            expressionReturn1.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream7 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            RewriteRuleSubtreeStream ruleSubtreeStream8 = new RewriteRuleSubtreeStream(this.adaptor, "rule pu", expressionReturn3?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(91, token2), (object) nilNode2);
            this.adaptor.AddChild((object) astNode3, ruleSubtreeStream8.NextTree());
            this.adaptor.AddChild((object) astNode1, (object) astNode3);
            expressionReturn1.Tree = (object) astNode1;
            expressionReturn1.Tree = (object) astNode1;
            break;
          case 3:
            this.PushFollow(HqlParser.FOLLOW_caseExpression_in_unaryExpression2540);
            HqlParser.caseExpression_return expressionReturn4 = this.caseExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream2.Add(expressionReturn4.Tree);
            expressionReturn1.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream9 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            RewriteRuleSubtreeStream ruleSubtreeStream10 = new RewriteRuleSubtreeStream(this.adaptor, "rule c", expressionReturn4?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child1 = (IASTNode) this.adaptor.BecomeRoot(ruleSubtreeStream10.NextNode(), (object) nilNode3);
            this.adaptor.AddChild((object) astNode1, (object) child1);
            expressionReturn1.Tree = (object) astNode1;
            expressionReturn1.Tree = (object) astNode1;
            break;
          case 4:
            this.PushFollow(HqlParser.FOLLOW_quantifiedExpression_in_unaryExpression2554);
            HqlParser.quantifiedExpression_return expressionReturn5 = this.quantifiedExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream3.Add(expressionReturn5.Tree);
            expressionReturn1.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream11 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            RewriteRuleSubtreeStream ruleSubtreeStream12 = new RewriteRuleSubtreeStream(this.adaptor, "rule q", expressionReturn5?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child2 = (IASTNode) this.adaptor.BecomeRoot(ruleSubtreeStream12.NextNode(), (object) nilNode4);
            this.adaptor.AddChild((object) astNode1, (object) child2);
            expressionReturn1.Tree = (object) astNode1;
            expressionReturn1.Tree = (object) astNode1;
            break;
          case 5:
            this.PushFollow(HqlParser.FOLLOW_atom_in_unaryExpression2569);
            HqlParser.atom_return atomReturn = this.atom();
            --this.state.followingStackPointer;
            ruleSubtreeStream1.Add(atomReturn.Tree);
            expressionReturn1.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream13 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            RewriteRuleSubtreeStream ruleSubtreeStream14 = new RewriteRuleSubtreeStream(this.adaptor, "rule a", atomReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode5 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode child3 = (IASTNode) this.adaptor.BecomeRoot(ruleSubtreeStream14.NextNode(), (object) nilNode5);
            this.adaptor.AddChild((object) astNode1, (object) child3);
            expressionReturn1.Tree = (object) astNode1;
            expressionReturn1.Tree = (object) astNode1;
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.caseExpression_return caseExpression()
    {
      HqlParser.caseExpression_return expressionReturn1 = new HqlParser.caseExpression_return();
      expressionReturn1.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token END");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token CASE");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule whenClause");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule unaryExpression");
      RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule altWhenClause");
      RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule elseClause");
      try
      {
        int num1 = this.input.LA(1) == 57 ? this.input.LA(2) : throw new NoViableAltException("", 79, 0, (IIntStream) this.input);
        int num2;
        if (num1 >= 4 && num1 <= 5 || num1 == 9 || num1 == 12 || num1 == 17 || num1 >= 19 && num1 <= 20 || num1 == 27 || num1 >= 35 && num1 <= 36 || num1 == 39 || num1 >= 48 && num1 <= 49 || num1 == 51 || num1 == 57 || num1 == 65 || num1 >= 95 && num1 <= 99 || num1 == 103 || num1 >= 105 && num1 <= 106 || num1 >= 118 && num1 <= 119 || num1 >= 124 && num1 <= 125)
        {
          num2 = 2;
        }
        else
        {
          if (num1 != 61)
            throw new NoViableAltException("", 79, 1, (IIntStream) this.input);
          num2 = 1;
        }
        switch (num2)
        {
          case 1:
            IToken el1 = (IToken) this.Match((IIntStream) this.input, 57, HqlParser.FOLLOW_CASE_in_caseExpression2588);
            rewriteRuleTokenStream2.Add(el1);
            int num3 = 0;
            while (true)
            {
              int num4 = 2;
              if (this.input.LA(1) == 61)
                num4 = 1;
              if (num4 == 1)
              {
                this.PushFollow(HqlParser.FOLLOW_whenClause_in_caseExpression2591);
                HqlParser.whenClause_return whenClauseReturn = this.whenClause();
                --this.state.followingStackPointer;
                ruleSubtreeStream1.Add(whenClauseReturn.Tree);
                ++num3;
              }
              else
                break;
            }
            if (num3 < 1)
              throw new EarlyExitException(75, (IIntStream) this.input);
            int num5 = 2;
            if (this.input.LA(1) == 59)
              num5 = 1;
            if (num5 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_elseClause_in_caseExpression2596);
              HqlParser.elseClause_return elseClauseReturn = this.elseClause();
              --this.state.followingStackPointer;
              ruleSubtreeStream4.Add(elseClauseReturn.Tree);
            }
            IToken el2 = (IToken) this.Match((IIntStream) this.input, 58, HqlParser.FOLLOW_END_in_caseExpression2600);
            rewriteRuleTokenStream1.Add(el2);
            expressionReturn1.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleTokenStream2.NextNode(), (object) nilNode1);
            if (!ruleSubtreeStream1.HasNext())
              throw new RewriteEarlyExitException();
            while (ruleSubtreeStream1.HasNext())
              this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
            ruleSubtreeStream1.Reset();
            if (ruleSubtreeStream4.HasNext())
              this.adaptor.AddChild((object) astNode2, ruleSubtreeStream4.NextTree());
            ruleSubtreeStream4.Reset();
            this.adaptor.AddChild((object) astNode1, (object) astNode2);
            expressionReturn1.Tree = (object) astNode1;
            expressionReturn1.Tree = (object) astNode1;
            break;
          case 2:
            IToken el3 = (IToken) this.Match((IIntStream) this.input, 57, HqlParser.FOLLOW_CASE_in_caseExpression2620);
            rewriteRuleTokenStream2.Add(el3);
            this.PushFollow(HqlParser.FOLLOW_unaryExpression_in_caseExpression2622);
            HqlParser.unaryExpression_return expressionReturn2 = this.unaryExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream2.Add(expressionReturn2.Tree);
            int num6 = 0;
            while (true)
            {
              int num7 = 2;
              if (this.input.LA(1) == 61)
                num7 = 1;
              if (num7 == 1)
              {
                this.PushFollow(HqlParser.FOLLOW_altWhenClause_in_caseExpression2625);
                HqlParser.altWhenClause_return whenClauseReturn = this.altWhenClause();
                --this.state.followingStackPointer;
                ruleSubtreeStream3.Add(whenClauseReturn.Tree);
                ++num6;
              }
              else
                break;
            }
            if (num6 < 1)
              throw new EarlyExitException(77, (IIntStream) this.input);
            int num8 = 2;
            if (this.input.LA(1) == 59)
              num8 = 1;
            if (num8 == 1)
            {
              this.PushFollow(HqlParser.FOLLOW_elseClause_in_caseExpression2630);
              HqlParser.elseClause_return elseClauseReturn = this.elseClause();
              --this.state.followingStackPointer;
              ruleSubtreeStream4.Add(elseClauseReturn.Tree);
            }
            IToken el4 = (IToken) this.Match((IIntStream) this.input, 58, HqlParser.FOLLOW_END_in_caseExpression2634);
            rewriteRuleTokenStream1.Add(el4);
            expressionReturn1.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream6 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionReturn1?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(74, "CASE2"), (object) nilNode2);
            this.adaptor.AddChild((object) astNode3, ruleSubtreeStream2.NextTree());
            if (!ruleSubtreeStream3.HasNext())
              throw new RewriteEarlyExitException();
            while (ruleSubtreeStream3.HasNext())
              this.adaptor.AddChild((object) astNode3, ruleSubtreeStream3.NextTree());
            ruleSubtreeStream3.Reset();
            if (ruleSubtreeStream4.HasNext())
              this.adaptor.AddChild((object) astNode3, ruleSubtreeStream4.NextTree());
            ruleSubtreeStream4.Reset();
            this.adaptor.AddChild((object) astNode1, (object) astNode3);
            expressionReturn1.Tree = (object) astNode1;
            expressionReturn1.Tree = (object) astNode1;
            break;
        }
        expressionReturn1.Stop = (object) this.input.LT(-1);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
        this.adaptor.SetTokenBoundaries(expressionReturn1.Tree, (IToken) expressionReturn1.Start, (IToken) expressionReturn1.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn1.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn1.Start, this.input.LT(-1), ex);
      }
      return expressionReturn1;
    }

    public HqlParser.whenClause_return whenClause()
    {
      HqlParser.whenClause_return whenClauseReturn = new HqlParser.whenClause_return();
      whenClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 61, HqlParser.FOLLOW_WHEN_in_whenClause2663)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_logicalExpression_in_whenClause2666);
        HqlParser.logicalExpression_return expressionReturn1 = this.logicalExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn1.Tree);
        IToken token = (IToken) this.Match((IIntStream) this.input, 60, HqlParser.FOLLOW_THEN_in_whenClause2668);
        this.PushFollow(HqlParser.FOLLOW_expression_in_whenClause2671);
        HqlParser.expression_return expressionReturn2 = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        whenClauseReturn.Stop = (object) this.input.LT(-1);
        whenClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(whenClauseReturn.Tree, (IToken) whenClauseReturn.Start, (IToken) whenClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        whenClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) whenClauseReturn.Start, this.input.LT(-1), ex);
      }
      return whenClauseReturn;
    }

    public HqlParser.altWhenClause_return altWhenClause()
    {
      HqlParser.altWhenClause_return whenClauseReturn = new HqlParser.altWhenClause_return();
      whenClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 61, HqlParser.FOLLOW_WHEN_in_altWhenClause2685)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_unaryExpression_in_altWhenClause2688);
        HqlParser.unaryExpression_return expressionReturn1 = this.unaryExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn1.Tree);
        IToken token = (IToken) this.Match((IIntStream) this.input, 60, HqlParser.FOLLOW_THEN_in_altWhenClause2690);
        this.PushFollow(HqlParser.FOLLOW_expression_in_altWhenClause2693);
        HqlParser.expression_return expressionReturn2 = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
        whenClauseReturn.Stop = (object) this.input.LT(-1);
        whenClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(whenClauseReturn.Tree, (IToken) whenClauseReturn.Start, (IToken) whenClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        whenClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) whenClauseReturn.Start, this.input.LT(-1), ex);
      }
      return whenClauseReturn;
    }

    public HqlParser.elseClause_return elseClause()
    {
      HqlParser.elseClause_return elseClauseReturn = new HqlParser.elseClause_return();
      elseClauseReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 59, HqlParser.FOLLOW_ELSE_in_elseClause2707)), (object) nilNode);
        this.PushFollow(HqlParser.FOLLOW_expression_in_elseClause2710);
        HqlParser.expression_return expressionReturn = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn.Tree);
        elseClauseReturn.Stop = (object) this.input.LT(-1);
        elseClauseReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(elseClauseReturn.Tree, (IToken) elseClauseReturn.Start, (IToken) elseClauseReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        elseClauseReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) elseClauseReturn.Start, this.input.LT(-1), ex);
      }
      return elseClauseReturn;
    }

    public HqlParser.quantifiedExpression_return quantifiedExpression()
    {
      HqlParser.quantifiedExpression_return expressionReturn = new HqlParser.quantifiedExpression_return();
      expressionReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        int num1;
        switch (this.input.LA(1))
        {
          case 4:
            num1 = 3;
            break;
          case 5:
            num1 = 4;
            break;
          case 19:
            num1 = 2;
            break;
          case 48:
            num1 = 1;
            break;
          default:
            throw new NoViableAltException("", 80, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 48, HqlParser.FOLLOW_SOME_in_quantifiedExpression2725)), (object) astNode);
            break;
          case 2:
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 19, HqlParser.FOLLOW_EXISTS_in_quantifiedExpression2730)), (object) astNode);
            break;
          case 3:
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 4, HqlParser.FOLLOW_ALL_in_quantifiedExpression2735)), (object) astNode);
            break;
          case 4:
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 5, HqlParser.FOLLOW_ANY_in_quantifiedExpression2740)), (object) astNode);
            break;
        }
        int num2;
        switch (this.input.LA(1))
        {
          case 17:
          case 27:
            num2 = 2;
            break;
          case 103:
            num2 = 3;
            break;
          case 125:
            num2 = 1;
            break;
          default:
            throw new NoViableAltException("", 81, 0, (IIntStream) this.input);
        }
        switch (num2)
        {
          case 1:
            this.PushFollow(HqlParser.FOLLOW_identifier_in_quantifiedExpression2749);
            HqlParser.identifier_return identifierReturn = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, identifierReturn.Tree);
            break;
          case 2:
            this.PushFollow(HqlParser.FOLLOW_collectionExpr_in_quantifiedExpression2753);
            HqlParser.collectionExpr_return collectionExprReturn = this.collectionExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, collectionExprReturn.Tree);
            break;
          case 3:
            IToken token1 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_quantifiedExpression2758);
            this.PushFollow(HqlParser.FOLLOW_subQuery_in_quantifiedExpression2763);
            HqlParser.subQuery_return subQueryReturn = this.subQuery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, subQueryReturn.Tree);
            IToken token2 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_quantifiedExpression2767);
            break;
        }
        expressionReturn.Stop = (object) this.input.LT(-1);
        expressionReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn.Tree, (IToken) expressionReturn.Start, (IToken) expressionReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn.Start, this.input.LT(-1), ex);
      }
      return expressionReturn;
    }

    public HqlParser.atom_return atom()
    {
      HqlParser.atom_return atomReturn = new HqlParser.atom_return();
      atomReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_primaryExpression_in_atom2786);
        HqlParser.primaryExpression_return expressionReturn1 = this.primaryExpression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, expressionReturn1.Tree);
        while (true)
        {
          int num1;
          do
          {
            int num2 = 3;
            switch (this.input.LA(1))
            {
              case 15:
                num2 = 1;
                break;
              case 122:
                num2 = 2;
                break;
            }
            switch (num2)
            {
              case 1:
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 15, HqlParser.FOLLOW_DOT_in_atom2795)), (object) astNode);
                this.PushFollow(HqlParser.FOLLOW_identifier_in_atom2798);
                HqlParser.identifier_return identifierReturn = this.identifier();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, identifierReturn.Tree);
                num1 = 2;
                if (this.input.LA(1) == 103)
                  num1 = 1;
                continue;
              case 2:
                goto label_10;
              default:
                goto label_11;
            }
          }
          while (num1 != 1);
          IToken payload1 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_atom2826);
          astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload1), (object) astNode);
          payload1.Type = 81;
          this.PushFollow(HqlParser.FOLLOW_exprList_in_atom2831);
          HqlParser.exprList_return exprListReturn = this.exprList();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode, exprListReturn.Tree);
          IToken token1 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_atom2833);
          continue;
label_10:
          IToken payload2 = (IToken) this.Match((IIntStream) this.input, 122, HqlParser.FOLLOW_OPEN_BRACKET_in_atom2847);
          astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload2), (object) astNode);
          payload2.Type = 78;
          this.PushFollow(HqlParser.FOLLOW_expression_in_atom2852);
          HqlParser.expression_return expressionReturn2 = this.expression();
          --this.state.followingStackPointer;
          this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
          IToken token2 = (IToken) this.Match((IIntStream) this.input, 123, HqlParser.FOLLOW_CLOSE_BRACKET_in_atom2854);
        }
label_11:
        atomReturn.Stop = (object) this.input.LT(-1);
        atomReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(atomReturn.Tree, (IToken) atomReturn.Start, (IToken) atomReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        atomReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) atomReturn.Start, this.input.LT(-1), ex);
      }
      return atomReturn;
    }

    public HqlParser.primaryExpression_return primaryExpression()
    {
      HqlParser.primaryExpression_return expressionReturn = new HqlParser.primaryExpression_return();
      expressionReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 9:
          case 12:
          case 17:
          case 27:
          case 35:
          case 36:
          case 49:
          case 125:
            num1 = 1;
            break;
          case 20:
          case 39:
          case 51:
          case 65:
          case 95:
          case 96:
          case 97:
          case 98:
          case 99:
          case 124:
            num1 = 2;
            break;
          case 103:
            num1 = 4;
            break;
          case 105:
            num1 = 3;
            break;
          case 106:
            num1 = 5;
            break;
          default:
            throw new NoViableAltException("", 87, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_identPrimary_in_primaryExpression2874);
            HqlParser.identPrimary_return identPrimaryReturn = this.identPrimary();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, identPrimaryReturn.Tree);
            int num2 = 2;
            if (this.input.LA(1) == 15 && this.input.LA(2) == 11)
              num2 = 1;
            if (num2 == 1)
            {
              astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 15, HqlParser.FOLLOW_DOT_in_primaryExpression2887)), (object) astNode);
              IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 11, HqlParser.FOLLOW_CLASS_in_primaryExpression2890));
              this.adaptor.AddChild((object) astNode, (object) child);
              break;
            }
            break;
          case 2:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_constant_in_primaryExpression2900);
            HqlParser.constant_return constantReturn = this.constant();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, constantReturn.Tree);
            break;
          case 3:
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 105, HqlParser.FOLLOW_COLON_in_primaryExpression2907)), (object) nilNode1);
            this.PushFollow(HqlParser.FOLLOW_identifier_in_primaryExpression2910);
            HqlParser.identifier_return identifierReturn = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, identifierReturn.Tree);
            break;
          case 4:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            IToken token1 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_primaryExpression2919);
            int num3 = this.input.LA(1);
            int num4;
            if (num3 >= 4 && num3 <= 5 || num3 == 9 || num3 == 12 || num3 == 17 || num3 >= 19 && num3 <= 20 || num3 == 27 || num3 >= 35 && num3 <= 36 || num3 >= 38 && num3 <= 39 || num3 >= 48 && num3 <= 49 || num3 == 51 || num3 == 57 || num3 == 65 || num3 >= 95 && num3 <= 99 || num3 == 103 || num3 >= 105 && num3 <= 106 || num3 == 114 || num3 >= 118 && num3 <= 119 || num3 >= 124 && num3 <= 125)
            {
              num4 = 1;
            }
            else
            {
              if (num3 != -1 && num3 != 22 && (num3 < 24 || num3 > 25) && num3 != 41 && num3 != 45 && num3 != 47 && num3 != 50 && num3 != 52 && num3 != 55 && num3 != 104)
                throw new NoViableAltException("", 85, 0, (IIntStream) this.input);
              num4 = 2;
            }
            switch (num4)
            {
              case 1:
                this.PushFollow(HqlParser.FOLLOW_expressionOrVector_in_primaryExpression2923);
                HqlParser.expressionOrVector_return expressionOrVectorReturn = this.expressionOrVector();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, expressionOrVectorReturn.Tree);
                break;
              case 2:
                this.PushFollow(HqlParser.FOLLOW_subQuery_in_primaryExpression2927);
                HqlParser.subQuery_return subQueryReturn = this.subQuery();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, subQueryReturn.Tree);
                break;
            }
            IToken token2 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_primaryExpression2930);
            break;
          case 5:
            IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 106, HqlParser.FOLLOW_PARAM_in_primaryExpression2938)), (object) nilNode2);
            int num5 = 2;
            if (this.input.LA(1) == 95)
              num5 = 1;
            if (num5 == 1)
            {
              IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 95, HqlParser.FOLLOW_NUM_INT_in_primaryExpression2942));
              this.adaptor.AddChild((object) astNode, (object) child);
              break;
            }
            break;
        }
        expressionReturn.Stop = (object) this.input.LT(-1);
        expressionReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(expressionReturn.Tree, (IToken) expressionReturn.Start, (IToken) expressionReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionReturn.Start, this.input.LT(-1), ex);
      }
      return expressionReturn;
    }

    public HqlParser.expressionOrVector_return expressionOrVector()
    {
      HqlParser.expressionOrVector_return expressionOrVectorReturn = new HqlParser.expressionOrVector_return();
      expressionOrVectorReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      HqlParser.vectorExpr_return vectorExprReturn = (HqlParser.vectorExpr_return) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule expression");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule vectorExpr");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_expression_in_expressionOrVector2960);
        HqlParser.expression_return expressionReturn = this.expression();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(expressionReturn.Tree);
        int num = 2;
        if (this.input.LA(1) == 101)
          num = 1;
        if (num == 1)
        {
          this.PushFollow(HqlParser.FOLLOW_vectorExpr_in_expressionOrVector2966);
          vectorExprReturn = this.vectorExpr();
          --this.state.followingStackPointer;
          ruleSubtreeStream2.Add(vectorExprReturn.Tree);
        }
        expressionOrVectorReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule v", vectorExprReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", expressionOrVectorReturn?.Tree);
        RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule e", expressionReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        if (vectorExprReturn != null)
        {
          IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
          IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(92, "{vector}"), (object) nilNode2);
          this.adaptor.AddChild((object) astNode2, ruleSubtreeStream5.NextTree());
          this.adaptor.AddChild((object) astNode2, ruleSubtreeStream3.NextTree());
          this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        }
        else
        {
          IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
          IASTNode child = (IASTNode) this.adaptor.BecomeRoot(ruleSubtreeStream5.NextNode(), (object) nilNode3);
          this.adaptor.AddChild((object) nilNode1, (object) child);
        }
        expressionOrVectorReturn.Tree = (object) nilNode1;
        expressionOrVectorReturn.Tree = (object) nilNode1;
        expressionOrVectorReturn.Stop = (object) this.input.LT(-1);
        expressionOrVectorReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(expressionOrVectorReturn.Tree, (IToken) expressionOrVectorReturn.Start, (IToken) expressionOrVectorReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        expressionOrVectorReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) expressionOrVectorReturn.Start, this.input.LT(-1), ex);
      }
      return expressionOrVectorReturn;
    }

    public HqlParser.vectorExpr_return vectorExpr()
    {
      HqlParser.vectorExpr_return vectorExprReturn = new HqlParser.vectorExpr_return();
      vectorExprReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IToken token1 = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_vectorExpr3005);
        this.PushFollow(HqlParser.FOLLOW_expression_in_vectorExpr3008);
        HqlParser.expression_return expressionReturn1 = this.expression();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) nilNode, expressionReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 101)
            num = 1;
          if (num == 1)
          {
            IToken token2 = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_vectorExpr3011);
            this.PushFollow(HqlParser.FOLLOW_expression_in_vectorExpr3014);
            HqlParser.expression_return expressionReturn2 = this.expression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, expressionReturn2.Tree);
          }
          else
            break;
        }
        vectorExprReturn.Stop = (object) this.input.LT(-1);
        vectorExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(vectorExprReturn.Tree, (IToken) vectorExprReturn.Start, (IToken) vectorExprReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        vectorExprReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) vectorExprReturn.Start, this.input.LT(-1), ex);
      }
      return vectorExprReturn;
    }

    public HqlParser.identPrimary_return identPrimary()
    {
      HqlParser.identPrimary_return identPrimaryReturn = new HqlParser.identPrimary_return();
      identPrimaryReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      try
      {
        int num1 = this.input.LA(1);
        int num2;
        if (num1 == 125)
        {
          num2 = 1;
        }
        else
        {
          if (num1 != 9 && num1 != 12 && num1 != 17 && num1 != 27 && (num1 < 35 || num1 > 36) && num1 != 49)
            throw new NoViableAltException("", 93, 0, (IIntStream) this.input);
          num2 = 2;
        }
        switch (num2)
        {
          case 1:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_identifier_in_identPrimary3030);
            HqlParser.identifier_return identifierReturn1 = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, identifierReturn1.Tree);
            this.HandleDotIdent();
            while (true)
            {
              int num3 = 2;
              if (this.input.LA(1) == 15)
              {
                switch (this.input.LA(2))
                {
                  case 68:
                  case 125:
                    num3 = 1;
                    break;
                }
              }
              if (num3 == 1)
              {
                astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 15, HqlParser.FOLLOW_DOT_in_identPrimary3048)), (object) astNode);
                int num4;
                switch (this.input.LA(1))
                {
                  case 68:
                    num4 = 2;
                    break;
                  case 125:
                    num4 = 1;
                    break;
                  default:
                    goto label_15;
                }
                switch (num4)
                {
                  case 1:
                    this.PushFollow(HqlParser.FOLLOW_identifier_in_identPrimary3053);
                    HqlParser.identifier_return identifierReturn2 = this.identifier();
                    --this.state.followingStackPointer;
                    this.adaptor.AddChild((object) astNode, identifierReturn2.Tree);
                    continue;
                  case 2:
                    IToken payload = (IToken) this.Match((IIntStream) this.input, 68, HqlParser.FOLLOW_OBJECT_in_identPrimary3059);
                    IASTNode child = (IASTNode) this.adaptor.Create(payload);
                    this.adaptor.AddChild((object) astNode, (object) child);
                    payload.Type = 125;
                    continue;
                  default:
                    continue;
                }
              }
              else
                goto label_19;
            }
label_15:
            throw new NoViableAltException("", 90, 0, (IIntStream) this.input);
label_19:
            int num5 = 2;
            if (this.input.LA(1) == 103)
              num5 = 1;
            if (num5 == 1)
            {
              IToken payload = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_identPrimary3077);
              astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(payload), (object) astNode);
              payload.Type = 81;
              this.PushFollow(HqlParser.FOLLOW_exprList_in_identPrimary3082);
              HqlParser.exprList_return exprListReturn = this.exprList();
              --this.state.followingStackPointer;
              this.adaptor.AddChild((object) astNode, exprListReturn.Tree);
              IToken token = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_identPrimary3084);
              break;
            }
            break;
          case 2:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_aggregate_in_identPrimary3100);
            HqlParser.aggregate_return aggregateReturn = this.aggregate();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, aggregateReturn.Tree);
            break;
        }
        identPrimaryReturn.Stop = (object) this.input.LT(-1);
        identPrimaryReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(identPrimaryReturn.Tree, (IToken) identPrimaryReturn.Start, (IToken) identPrimaryReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        identPrimaryReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) identPrimaryReturn.Start, this.input.LT(-1), ex);
      }
      return identPrimaryReturn;
    }

    public HqlParser.aggregate_return aggregate()
    {
      HqlParser.aggregate_return aggregateReturn = new HqlParser.aggregate_return();
      aggregateReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      IToken token = (IToken) null;
      IToken el1 = (IToken) null;
      HqlParser.aggregateDistinctAll_return distinctAllReturn = (HqlParser.aggregateDistinctAll_return) null;
      RewriteRuleTokenStream rewriteRuleTokenStream1 = new RewriteRuleTokenStream(this.adaptor, "token OPEN");
      RewriteRuleTokenStream rewriteRuleTokenStream2 = new RewriteRuleTokenStream(this.adaptor, "token MAX");
      RewriteRuleTokenStream rewriteRuleTokenStream3 = new RewriteRuleTokenStream(this.adaptor, "token COUNT");
      RewriteRuleTokenStream rewriteRuleTokenStream4 = new RewriteRuleTokenStream(this.adaptor, "token STAR");
      RewriteRuleTokenStream rewriteRuleTokenStream5 = new RewriteRuleTokenStream(this.adaptor, "token MIN");
      RewriteRuleTokenStream rewriteRuleTokenStream6 = new RewriteRuleTokenStream(this.adaptor, "token CLOSE");
      RewriteRuleTokenStream rewriteRuleTokenStream7 = new RewriteRuleTokenStream(this.adaptor, "token SUM");
      RewriteRuleTokenStream rewriteRuleTokenStream8 = new RewriteRuleTokenStream(this.adaptor, "token AVG");
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule aggregateDistinctAll");
      RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule additiveExpression");
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 9:
          case 35:
          case 36:
          case 49:
            num1 = 1;
            break;
          case 12:
            num1 = 2;
            break;
          case 17:
          case 27:
            num1 = 3;
            break;
          default:
            throw new NoViableAltException("", 96, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            int num2;
            switch (this.input.LA(1))
            {
              case 9:
                num2 = 2;
                break;
              case 35:
                num2 = 3;
                break;
              case 36:
                num2 = 4;
                break;
              case 49:
                num2 = 1;
                break;
              default:
                throw new NoViableAltException("", 94, 0, (IIntStream) this.input);
            }
            switch (num2)
            {
              case 1:
                token = (IToken) this.Match((IIntStream) this.input, 49, HqlParser.FOLLOW_SUM_in_aggregate3121);
                rewriteRuleTokenStream7.Add(token);
                break;
              case 2:
                token = (IToken) this.Match((IIntStream) this.input, 9, HqlParser.FOLLOW_AVG_in_aggregate3127);
                rewriteRuleTokenStream8.Add(token);
                break;
              case 3:
                token = (IToken) this.Match((IIntStream) this.input, 35, HqlParser.FOLLOW_MAX_in_aggregate3133);
                rewriteRuleTokenStream2.Add(token);
                break;
              case 4:
                token = (IToken) this.Match((IIntStream) this.input, 36, HqlParser.FOLLOW_MIN_in_aggregate3139);
                rewriteRuleTokenStream5.Add(token);
                break;
            }
            IToken el2 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_aggregate3143);
            rewriteRuleTokenStream1.Add(el2);
            this.PushFollow(HqlParser.FOLLOW_additiveExpression_in_aggregate3145);
            HqlParser.additiveExpression_return expressionReturn = this.additiveExpression();
            --this.state.followingStackPointer;
            ruleSubtreeStream2.Add(expressionReturn.Tree);
            IToken el3 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_aggregate3147);
            rewriteRuleTokenStream6.Add(el3);
            aggregateReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream3 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", aggregateReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
            IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(71, token), (object) nilNode1);
            this.adaptor.AddChild((object) astNode2, ruleSubtreeStream2.NextTree());
            this.adaptor.AddChild((object) astNode1, (object) astNode2);
            aggregateReturn.Tree = (object) astNode1;
            aggregateReturn.Tree = (object) astNode1;
            break;
          case 2:
            IToken el4 = (IToken) this.Match((IIntStream) this.input, 12, HqlParser.FOLLOW_COUNT_in_aggregate3166);
            rewriteRuleTokenStream3.Add(el4);
            IToken el5 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_aggregate3168);
            rewriteRuleTokenStream1.Add(el5);
            int num3 = this.input.LA(1);
            int num4;
            if (num3 == 120)
            {
              num4 = 1;
            }
            else
            {
              if (num3 != 4 && (num3 < 16 || num3 > 17) && num3 != 27 && num3 != 125)
                throw new NoViableAltException("", 95, 0, (IIntStream) this.input);
              num4 = 2;
            }
            switch (num4)
            {
              case 1:
                el1 = (IToken) this.Match((IIntStream) this.input, 120, HqlParser.FOLLOW_STAR_in_aggregate3174);
                rewriteRuleTokenStream4.Add(el1);
                break;
              case 2:
                this.PushFollow(HqlParser.FOLLOW_aggregateDistinctAll_in_aggregate3180);
                distinctAllReturn = this.aggregateDistinctAll();
                --this.state.followingStackPointer;
                ruleSubtreeStream1.Add(distinctAllReturn.Tree);
                break;
            }
            IToken el6 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_aggregate3184);
            rewriteRuleTokenStream6.Add(el6);
            aggregateReturn.Tree = (object) astNode1;
            RewriteRuleSubtreeStream ruleSubtreeStream4 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", aggregateReturn?.Tree);
            RewriteRuleSubtreeStream ruleSubtreeStream5 = new RewriteRuleSubtreeStream(this.adaptor, "rule p", distinctAllReturn?.Tree);
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            if (el1 == null)
            {
              IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
              IASTNode astNode3 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleTokenStream3.NextNode(), (object) nilNode2);
              this.adaptor.AddChild((object) astNode3, ruleSubtreeStream5.NextTree());
              this.adaptor.AddChild((object) astNode1, (object) astNode3);
            }
            else
            {
              IASTNode nilNode3 = (IASTNode) this.adaptor.GetNilNode();
              IASTNode astNode4 = (IASTNode) this.adaptor.BecomeRoot(rewriteRuleTokenStream3.NextNode(), (object) nilNode3);
              IASTNode nilNode4 = (IASTNode) this.adaptor.GetNilNode();
              IASTNode child = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(88, "*"), (object) nilNode4);
              this.adaptor.AddChild((object) astNode4, (object) child);
              this.adaptor.AddChild((object) astNode1, (object) astNode4);
            }
            aggregateReturn.Tree = (object) astNode1;
            aggregateReturn.Tree = (object) astNode1;
            break;
          case 3:
            astNode1 = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_collectionExpr_in_aggregate3216);
            HqlParser.collectionExpr_return collectionExprReturn = this.collectionExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode1, collectionExprReturn.Tree);
            break;
        }
        aggregateReturn.Stop = (object) this.input.LT(-1);
        aggregateReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode1);
        this.adaptor.SetTokenBoundaries(aggregateReturn.Tree, (IToken) aggregateReturn.Start, (IToken) aggregateReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        aggregateReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) aggregateReturn.Start, this.input.LT(-1), ex);
      }
      return aggregateReturn;
    }

    public HqlParser.aggregateDistinctAll_return aggregateDistinctAll()
    {
      HqlParser.aggregateDistinctAll_return distinctAllReturn = new HqlParser.aggregateDistinctAll_return();
      distinctAllReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
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
          IToken payload = this.input.LT(1);
          if (this.input.LA(1) != 4 && this.input.LA(1) != 16)
            throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
          this.input.Consume();
          this.adaptor.AddChild((object) nilNode, (object) (IASTNode) this.adaptor.Create(payload));
          this.state.errorRecovery = false;
        }
        int num2;
        switch (this.input.LA(1))
        {
          case 17:
          case 27:
            num2 = 2;
            break;
          case 125:
            num2 = 1;
            break;
          default:
            throw new NoViableAltException("", 98, 0, (IIntStream) this.input);
        }
        switch (num2)
        {
          case 1:
            this.PushFollow(HqlParser.FOLLOW_path_in_aggregateDistinctAll3242);
            HqlParser.path_return pathReturn = this.path();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, pathReturn.Tree);
            break;
          case 2:
            this.PushFollow(HqlParser.FOLLOW_collectionExpr_in_aggregateDistinctAll3246);
            HqlParser.collectionExpr_return collectionExprReturn = this.collectionExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, collectionExprReturn.Tree);
            break;
        }
        distinctAllReturn.Stop = (object) this.input.LT(-1);
        distinctAllReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(distinctAllReturn.Tree, (IToken) distinctAllReturn.Start, (IToken) distinctAllReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        distinctAllReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) distinctAllReturn.Start, this.input.LT(-1), ex);
      }
      return distinctAllReturn;
    }

    public HqlParser.collectionExpr_return collectionExpr()
    {
      HqlParser.collectionExpr_return collectionExprReturn = new HqlParser.collectionExpr_return();
      collectionExprReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
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
            throw new NoViableAltException("", 99, 0, (IIntStream) this.input);
        }
        switch (num)
        {
          case 1:
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 17, HqlParser.FOLLOW_ELEMENTS_in_collectionExpr3265)), (object) astNode);
            break;
          case 2:
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 27, HqlParser.FOLLOW_INDICES_in_collectionExpr3270)), (object) astNode);
            break;
        }
        IToken token1 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_collectionExpr3274);
        this.PushFollow(HqlParser.FOLLOW_path_in_collectionExpr3277);
        HqlParser.path_return pathReturn = this.path();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, pathReturn.Tree);
        IToken token2 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_collectionExpr3279);
        collectionExprReturn.Stop = (object) this.input.LT(-1);
        collectionExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(collectionExprReturn.Tree, (IToken) collectionExprReturn.Start, (IToken) collectionExprReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        collectionExprReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) collectionExprReturn.Start, this.input.LT(-1), ex);
      }
      return collectionExprReturn;
    }

    public HqlParser.compoundExpr_return compoundExpr()
    {
      HqlParser.compoundExpr_return compoundExprReturn = new HqlParser.compoundExpr_return();
      compoundExprReturn.Start = (object) this.input.LT(1);
      IASTNode astNode = (IASTNode) null;
      try
      {
        int num1;
        switch (this.input.LA(1))
        {
          case 17:
          case 27:
            num1 = 1;
            break;
          case 103:
            num1 = 3;
            break;
          case 125:
            num1 = 2;
            break;
          default:
            throw new NoViableAltException("", 102, 0, (IIntStream) this.input);
        }
        switch (num1)
        {
          case 1:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_collectionExpr_in_compoundExpr3334);
            HqlParser.collectionExpr_return collectionExprReturn = this.collectionExpr();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, collectionExprReturn.Tree);
            break;
          case 2:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            this.PushFollow(HqlParser.FOLLOW_path_in_compoundExpr3339);
            HqlParser.path_return pathReturn = this.path();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, pathReturn.Tree);
            break;
          case 3:
            astNode = (IASTNode) this.adaptor.GetNilNode();
            IToken token1 = (IToken) this.Match((IIntStream) this.input, 103, HqlParser.FOLLOW_OPEN_in_compoundExpr3345);
            int num2 = this.input.LA(1);
            int num3;
            if (num2 == -1 || num2 == 22 || num2 >= 24 && num2 <= 25 || num2 == 41 || num2 == 45 || num2 == 47 || num2 == 50 || num2 == 52 || num2 == 55 || num2 == 104)
            {
              num3 = 1;
            }
            else
            {
              if ((num2 < 4 || num2 > 5) && num2 != 9 && num2 != 12 && num2 != 17 && (num2 < 19 || num2 > 20) && num2 != 27 && (num2 < 35 || num2 > 36) && (num2 < 38 || num2 > 39) && (num2 < 48 || num2 > 49) && num2 != 51 && num2 != 57 && num2 != 65 && (num2 < 95 || num2 > 99) && num2 != 103 && (num2 < 105 || num2 > 106) && num2 != 114 && (num2 < 118 || num2 > 119) && (num2 < 124 || num2 > 125))
                throw new NoViableAltException("", 101, 0, (IIntStream) this.input);
              num3 = 2;
            }
            switch (num3)
            {
              case 1:
                this.PushFollow(HqlParser.FOLLOW_subQuery_in_compoundExpr3350);
                HqlParser.subQuery_return subQueryReturn = this.subQuery();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, subQueryReturn.Tree);
                break;
              case 2:
                this.PushFollow(HqlParser.FOLLOW_expression_in_compoundExpr3355);
                HqlParser.expression_return expressionReturn1 = this.expression();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) astNode, expressionReturn1.Tree);
                while (true)
                {
                  int num4 = 2;
                  if (this.input.LA(1) == 101)
                    num4 = 1;
                  if (num4 == 1)
                  {
                    IToken token2 = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_compoundExpr3358);
                    this.PushFollow(HqlParser.FOLLOW_expression_in_compoundExpr3361);
                    HqlParser.expression_return expressionReturn2 = this.expression();
                    --this.state.followingStackPointer;
                    this.adaptor.AddChild((object) astNode, expressionReturn2.Tree);
                  }
                  else
                    break;
                }
            }
            IToken token3 = (IToken) this.Match((IIntStream) this.input, 104, HqlParser.FOLLOW_CLOSE_in_compoundExpr3368);
            break;
        }
        compoundExprReturn.Stop = (object) this.input.LT(-1);
        compoundExprReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(compoundExprReturn.Tree, (IToken) compoundExprReturn.Start, (IToken) compoundExprReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        compoundExprReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) compoundExprReturn.Start, this.input.LT(-1), ex);
      }
      return compoundExprReturn;
    }

    public HqlParser.exprList_return exprList()
    {
      HqlParser.exprList_return exprListReturn = new HqlParser.exprList_return();
      exprListReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        int num1 = 4;
        switch (this.input.LA(1))
        {
          case 64:
            num1 = 3;
            break;
          case 66:
            num1 = 2;
            break;
          case 70:
            num1 = 1;
            break;
        }
        switch (num1)
        {
          case 1:
            IToken payload1 = (IToken) this.Match((IIntStream) this.input, 70, HqlParser.FOLLOW_TRAILING_in_exprList3387);
            IASTNode child1 = (IASTNode) this.adaptor.Create(payload1);
            this.adaptor.AddChild((object) nilNode, (object) child1);
            payload1.Type = 125;
            break;
          case 2:
            IToken payload2 = (IToken) this.Match((IIntStream) this.input, 66, HqlParser.FOLLOW_LEADING_in_exprList3400);
            IASTNode child2 = (IASTNode) this.adaptor.Create(payload2);
            this.adaptor.AddChild((object) nilNode, (object) child2);
            payload2.Type = 125;
            break;
          case 3:
            IToken payload3 = (IToken) this.Match((IIntStream) this.input, 64, HqlParser.FOLLOW_BOTH_in_exprList3413);
            IASTNode child3 = (IASTNode) this.adaptor.Create(payload3);
            this.adaptor.AddChild((object) nilNode, (object) child3);
            payload3.Type = 125;
            break;
        }
        int num2 = 3;
        int num3 = this.input.LA(1);
        if (num3 >= 4 && num3 <= 5 || num3 == 9 || num3 == 12 || num3 == 17 || num3 >= 19 && num3 <= 20 || num3 == 27 || num3 >= 35 && num3 <= 36 || num3 >= 38 && num3 <= 39 || num3 >= 48 && num3 <= 49 || num3 == 51 || num3 == 57 || num3 == 65 || num3 >= 95 && num3 <= 99 || num3 == 103 || num3 >= 105 && num3 <= 106 || num3 == 114 || num3 >= 118 && num3 <= 119 || num3 >= 124 && num3 <= 125)
          num2 = 1;
        else if (num3 == 22)
          num2 = 2;
        switch (num2)
        {
          case 1:
            this.PushFollow(HqlParser.FOLLOW_expression_in_exprList3437);
            HqlParser.expression_return expressionReturn1 = this.expression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, expressionReturn1.Tree);
            int num4 = 4;
            switch (this.input.LA(1))
            {
              case 7:
                num4 = 3;
                break;
              case 22:
                num4 = 2;
                break;
              case 101:
                num4 = 1;
                break;
            }
            switch (num4)
            {
              case 1:
                int num5 = 0;
                while (true)
                {
                  int num6 = 2;
                  if (this.input.LA(1) == 101)
                    num6 = 1;
                  if (num6 == 1)
                  {
                    IToken token = (IToken) this.Match((IIntStream) this.input, 101, HqlParser.FOLLOW_COMMA_in_exprList3442);
                    this.PushFollow(HqlParser.FOLLOW_expression_in_exprList3445);
                    HqlParser.expression_return expressionReturn2 = this.expression();
                    --this.state.followingStackPointer;
                    this.adaptor.AddChild((object) nilNode, expressionReturn2.Tree);
                    ++num5;
                  }
                  else
                    break;
                }
                if (num5 < 1)
                  throw new EarlyExitException(104, (IIntStream) this.input);
                break;
              case 2:
                IToken payload4 = (IToken) this.Match((IIntStream) this.input, 22, HqlParser.FOLLOW_FROM_in_exprList3460);
                IASTNode child4 = (IASTNode) this.adaptor.Create(payload4);
                this.adaptor.AddChild((object) nilNode, (object) child4);
                this.PushFollow(HqlParser.FOLLOW_expression_in_exprList3462);
                HqlParser.expression_return expressionReturn3 = this.expression();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) nilNode, expressionReturn3.Tree);
                payload4.Type = 125;
                break;
              case 3:
                IToken token1 = (IToken) this.Match((IIntStream) this.input, 7, HqlParser.FOLLOW_AS_in_exprList3474);
                this.PushFollow(HqlParser.FOLLOW_identifier_in_exprList3477);
                HqlParser.identifier_return identifierReturn = this.identifier();
                --this.state.followingStackPointer;
                this.adaptor.AddChild((object) nilNode, identifierReturn.Tree);
                break;
            }
            break;
          case 2:
            IToken payload5 = (IToken) this.Match((IIntStream) this.input, 22, HqlParser.FOLLOW_FROM_in_exprList3491);
            IASTNode child5 = (IASTNode) this.adaptor.Create(payload5);
            this.adaptor.AddChild((object) nilNode, (object) child5);
            this.PushFollow(HqlParser.FOLLOW_expression_in_exprList3493);
            HqlParser.expression_return expressionReturn4 = this.expression();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) nilNode, expressionReturn4.Tree);
            payload5.Type = 125;
            break;
        }
        exprListReturn.Stop = (object) this.input.LT(-1);
        exprListReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(exprListReturn.Tree, (IToken) exprListReturn.Start, (IToken) exprListReturn.Stop);
        IASTNode astNode = (IASTNode) this.adaptor.Create(75, nameof (exprList));
        astNode.AddChild((IASTNode) exprListReturn.Tree);
        exprListReturn.Tree = (object) astNode;
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        exprListReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) exprListReturn.Start, this.input.LT(-1), ex);
      }
      return exprListReturn;
    }

    public HqlParser.subQuery_return subQuery()
    {
      HqlParser.subQuery_return subQueryReturn = new HqlParser.subQuery_return();
      subQueryReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_innerSubQuery_in_subQuery3513);
        HqlParser.innerSubQuery_return innerSubQueryReturn1 = this.innerSubQuery();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, innerSubQueryReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 52)
            num = 1;
          if (num == 1)
          {
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 52, HqlParser.FOLLOW_UNION_in_subQuery3516)), (object) astNode);
            this.PushFollow(HqlParser.FOLLOW_innerSubQuery_in_subQuery3519);
            HqlParser.innerSubQuery_return innerSubQueryReturn2 = this.innerSubQuery();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, innerSubQueryReturn2.Tree);
          }
          else
            break;
        }
        subQueryReturn.Stop = (object) this.input.LT(-1);
        subQueryReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(subQueryReturn.Tree, (IToken) subQueryReturn.Start, (IToken) subQueryReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        subQueryReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) subQueryReturn.Start, this.input.LT(-1), ex);
      }
      return subQueryReturn;
    }

    public HqlParser.innerSubQuery_return innerSubQuery()
    {
      HqlParser.innerSubQuery_return innerSubQueryReturn = new HqlParser.innerSubQuery_return();
      innerSubQueryReturn.Start = (object) this.input.LT(1);
      IASTNode astNode1 = (IASTNode) null;
      RewriteRuleSubtreeStream ruleSubtreeStream1 = new RewriteRuleSubtreeStream(this.adaptor, "rule queryRule");
      try
      {
        this.PushFollow(HqlParser.FOLLOW_queryRule_in_innerSubQuery3533);
        HqlParser.queryRule_return queryRuleReturn = this.queryRule();
        --this.state.followingStackPointer;
        ruleSubtreeStream1.Add(queryRuleReturn.Tree);
        innerSubQueryReturn.Tree = (object) astNode1;
        RewriteRuleSubtreeStream ruleSubtreeStream2 = new RewriteRuleSubtreeStream(this.adaptor, "rule retval", innerSubQueryReturn?.Tree);
        IASTNode nilNode1 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode nilNode2 = (IASTNode) this.adaptor.GetNilNode();
        IASTNode astNode2 = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create(86, "query"), (object) nilNode2);
        this.adaptor.AddChild((object) astNode2, ruleSubtreeStream1.NextTree());
        this.adaptor.AddChild((object) nilNode1, (object) astNode2);
        innerSubQueryReturn.Tree = (object) nilNode1;
        innerSubQueryReturn.Tree = (object) nilNode1;
        innerSubQueryReturn.Stop = (object) this.input.LT(-1);
        innerSubQueryReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode1);
        this.adaptor.SetTokenBoundaries(innerSubQueryReturn.Tree, (IToken) innerSubQueryReturn.Start, (IToken) innerSubQueryReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        innerSubQueryReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) innerSubQueryReturn.Start, this.input.LT(-1), ex);
      }
      return innerSubQueryReturn;
    }

    public HqlParser.constant_return constant()
    {
      HqlParser.constant_return constantReturn = new HqlParser.constant_return();
      constantReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IToken payload = this.input.LT(1);
        if (this.input.LA(1) != 20 && this.input.LA(1) != 39 && this.input.LA(1) != 51 && this.input.LA(1) != 65 && (this.input.LA(1) < 95 || this.input.LA(1) > 99) && this.input.LA(1) != 124)
          throw new MismatchedSetException((BitSet) null, (IIntStream) this.input);
        this.input.Consume();
        this.adaptor.AddChild((object) nilNode, (object) (IASTNode) this.adaptor.Create(payload));
        this.state.errorRecovery = false;
        constantReturn.Stop = (object) this.input.LT(-1);
        constantReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(constantReturn.Tree, (IToken) constantReturn.Start, (IToken) constantReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        constantReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) constantReturn.Start, this.input.LT(-1), ex);
      }
      return constantReturn;
    }

    public HqlParser.path_return path()
    {
      HqlParser.path_return pathReturn = new HqlParser.path_return();
      pathReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode astNode = (IASTNode) this.adaptor.GetNilNode();
        this.PushFollow(HqlParser.FOLLOW_identifier_in_path3621);
        HqlParser.identifier_return identifierReturn1 = this.identifier();
        --this.state.followingStackPointer;
        this.adaptor.AddChild((object) astNode, identifierReturn1.Tree);
        while (true)
        {
          int num = 2;
          if (this.input.LA(1) == 15)
            num = 1;
          if (num == 1)
          {
            astNode = (IASTNode) this.adaptor.BecomeRoot((object) (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 15, HqlParser.FOLLOW_DOT_in_path3625)), (object) astNode);
            this.WeakKeywords();
            this.PushFollow(HqlParser.FOLLOW_identifier_in_path3630);
            HqlParser.identifier_return identifierReturn2 = this.identifier();
            --this.state.followingStackPointer;
            this.adaptor.AddChild((object) astNode, identifierReturn2.Tree);
          }
          else
            break;
        }
        pathReturn.Stop = (object) this.input.LT(-1);
        pathReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) astNode);
        this.adaptor.SetTokenBoundaries(pathReturn.Tree, (IToken) pathReturn.Start, (IToken) pathReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        this.ReportError(ex);
        this.Recover((IIntStream) this.input, ex);
        pathReturn.Tree = (object) (IASTNode) this.adaptor.ErrorNode(this.input, (IToken) pathReturn.Start, this.input.LT(-1), ex);
      }
      return pathReturn;
    }

    public HqlParser.identifier_return identifier()
    {
      HqlParser.identifier_return identifierReturn = new HqlParser.identifier_return();
      identifierReturn.Start = (object) this.input.LT(1);
      try
      {
        IASTNode nilNode = (IASTNode) this.adaptor.GetNilNode();
        IASTNode child = (IASTNode) this.adaptor.Create((IToken) this.Match((IIntStream) this.input, 125, HqlParser.FOLLOW_IDENT_in_identifier3646));
        this.adaptor.AddChild((object) nilNode, (object) child);
        identifierReturn.Stop = (object) this.input.LT(-1);
        identifierReturn.Tree = (object) (IASTNode) this.adaptor.RulePostProcessing((object) nilNode);
        this.adaptor.SetTokenBoundaries(identifierReturn.Tree, (IToken) identifierReturn.Start, (IToken) identifierReturn.Stop);
      }
      catch (RecognitionException ex)
      {
        identifierReturn.Tree = (object) this.HandleIdentifierError(this.input.LT(1), ex);
      }
      return identifierReturn;
    }

    private void InitializeCyclicDFAs()
    {
    }

    static HqlParser()
    {
      HqlParser.possibleIds[12] = true;
      HqlParser.possibleIds[4] = true;
      HqlParser.possibleIds[5] = true;
      HqlParser.possibleIds[6] = true;
      HqlParser.possibleIds[7] = true;
      HqlParser.possibleIds[8] = true;
      HqlParser.possibleIds[9] = true;
      HqlParser.possibleIds[10] = true;
      HqlParser.possibleIds[11] = true;
      HqlParser.possibleIds[13] = true;
      HqlParser.possibleIds[14] = true;
      HqlParser.possibleIds[16] = true;
      HqlParser.possibleIds[17] = true;
      HqlParser.possibleIds[18] = true;
      HqlParser.possibleIds[19] = true;
      HqlParser.possibleIds[20] = true;
      HqlParser.possibleIds[21] = true;
      HqlParser.possibleIds[22] = true;
      HqlParser.possibleIds[23] = true;
      HqlParser.possibleIds[24] = true;
      HqlParser.possibleIds[25] = true;
      HqlParser.possibleIds[26] = true;
      HqlParser.possibleIds[27] = true;
      HqlParser.possibleIds[28] = true;
      HqlParser.possibleIds[29] = true;
      HqlParser.possibleIds[30] = true;
      HqlParser.possibleIds[31] = true;
      HqlParser.possibleIds[32] = true;
      HqlParser.possibleIds[33] = true;
      HqlParser.possibleIds[34] = true;
      HqlParser.possibleIds[35] = true;
      HqlParser.possibleIds[36] = true;
      HqlParser.possibleIds[37] = true;
      HqlParser.possibleIds[38] = true;
      HqlParser.possibleIds[39] = true;
      HqlParser.possibleIds[40] = true;
      HqlParser.possibleIds[41] = true;
      HqlParser.possibleIds[42] = true;
      HqlParser.possibleIds[43] = true;
      HqlParser.possibleIds[44] = true;
      HqlParser.possibleIds[45] = true;
      HqlParser.possibleIds[46] = true;
      HqlParser.possibleIds[48] = true;
      HqlParser.possibleIds[49] = true;
      HqlParser.possibleIds[51] = true;
      HqlParser.possibleIds[52] = true;
      HqlParser.possibleIds[53] = true;
      HqlParser.possibleIds[54] = true;
      HqlParser.possibleIds[55] = true;
      HqlParser.possibleIds[56] = true;
      HqlParser.possibleIds[57] = true;
      HqlParser.possibleIds[58] = true;
      HqlParser.possibleIds[59] = true;
      HqlParser.possibleIds[60] = true;
      HqlParser.possibleIds[61] = true;
      HqlParser.possibleIds[62] = true;
      HqlParser.possibleIds[63] = true;
      HqlParser.possibleIds[64] = true;
      HqlParser.possibleIds[65] = true;
      HqlParser.possibleIds[66] = true;
      HqlParser.possibleIds[67] = true;
      HqlParser.possibleIds[68] = true;
      HqlParser.possibleIds[69] = true;
      HqlParser.possibleIds[70] = true;
    }

    public IParseErrorHandler ParseErrorHandler
    {
      get => this._parseErrorHandler;
      set => this._parseErrorHandler = value;
    }

    public bool Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public override void ReportError(RecognitionException e)
    {
      this._parseErrorHandler.ReportError(e);
    }

    protected internal override object RecoverFromMismatchedToken(
      IIntStream input,
      int ttype,
      BitSet follow)
    {
      throw new MismatchedTokenException(ttype, input);
    }

    public void WeakKeywords()
    {
      int num = this.input.LA(1);
      switch (num)
      {
        case 24:
        case 41:
          if (this.input.LA(2) == 56)
            break;
          this.input.LT(1).Type = 125;
          if (!HqlParser.log.IsDebugEnabled)
            break;
          HqlParser.log.Debug((object) ("weakKeywords() : new LT(1) token - " + (object) this.input.LT(1)));
          break;
        default:
          if (num == 125 || this.input.LA(-1) != 22 || this.input.LA(2) != 15 && this.input.LA(2) != 125 && this.input.LA(2) != -1 || !(this.input.LT(1) is HqlToken hqlToken) || !hqlToken.PossibleId)
            break;
          hqlToken.Type = 125;
          if (!HqlParser.log.IsDebugEnabled)
            break;
          HqlParser.log.Debug((object) ("weakKeywords() : new LT(1) token - " + (object) this.input.LT(1)));
          break;
      }
    }

    public void WeakKeywords2()
    {
      int num = this.input.LA(1);
      switch (num)
      {
        case 24:
        case 41:
          if (this.input.LA(2) == 56)
            break;
          this.input.LT(1).Type = 125;
          if (!HqlParser.log.IsDebugEnabled)
            break;
          HqlParser.log.Debug((object) ("weakKeywords() : new LT(1) token - " + (object) this.input.LT(1)));
          break;
        default:
          if (num == 125 || this.input.LA(-1) != 22 || this.input.LA(2) != 15)
            break;
          HqlToken hqlToken = (HqlToken) this.input.LT(1);
          if (!hqlToken.PossibleId)
            break;
          hqlToken.Type = 125;
          if (!HqlParser.log.IsDebugEnabled)
            break;
          HqlParser.log.Debug((object) ("weakKeywords() : new LT(1) token - " + (object) this.input.LT(1)));
          break;
      }
    }

    public IASTNode NegateNode(IASTNode node)
    {
      switch (node.Type)
      {
        case 6:
          node.Type = 40;
          node.Text = "{or}";
          this.NegateNode(node.GetChild(0));
          this.NegateNode(node.GetChild(1));
          return node;
        case 10:
          node.Type = 82;
          node.Text = "{not}" + node.Text;
          return node;
        case 26:
          node.Type = 83;
          node.Text = "{not}" + node.Text;
          return node;
        case 34:
          node.Type = 84;
          node.Text = "{not}" + node.Text;
          return node;
        case 40:
          node.Type = 6;
          node.Text = "{and}";
          this.NegateNode(node.GetChild(0));
          this.NegateNode(node.GetChild(1));
          return node;
        case 79:
          node.Type = 80;
          node.Text = "{not}" + node.Text;
          return node;
        case 80:
          node.Type = 79;
          node.Text = "{not}" + node.Text;
          return node;
        case 82:
          node.Type = 10;
          node.Text = "{not}" + node.Text;
          return node;
        case 83:
          node.Type = 26;
          node.Text = "{not}" + node.Text;
          return node;
        case 84:
          node.Type = 34;
          node.Text = "{not}" + node.Text;
          return node;
        case 102:
          node.Type = 107;
          node.Text = "{not}" + node.Text;
          return node;
        case 107:
          node.Type = 102;
          node.Text = "{not}" + node.Text;
          return node;
        case 109:
          node.Type = 112;
          node.Text = "{not}" + node.Text;
          return node;
        case 110:
          node.Type = 111;
          node.Text = "{not}" + node.Text;
          return node;
        case 111:
          node.Type = 110;
          node.Text = "{not}" + node.Text;
          return node;
        case 112:
          node.Type = 109;
          node.Text = "{not}" + node.Text;
          return node;
        default:
          IASTNode astNode = (IASTNode) this.TreeAdaptor.Create(38, "not");
          astNode.AddChild(node);
          return astNode;
      }
    }

    public IASTNode ProcessEqualityExpression(object o)
    {
      if (!(o is IASTNode astNode))
      {
        HqlParser.log.Warn((object) "processEqualityExpression() : No expression to process!");
        return (IASTNode) null;
      }
      int type = astNode.Type;
      switch (type)
      {
        case 102:
        case 107:
          bool negated = type == 107;
          if (astNode.ChildCount == 2)
          {
            IASTNode child1 = astNode.GetChild(0);
            IASTNode child2 = astNode.GetChild(1);
            if (child1.Type == 39 && child2.Type != 39)
              return this.CreateIsNullParent(child2, negated);
            if (child2.Type == 39 && child1.Type != 39)
              return this.CreateIsNullParent(child1, negated);
            if (child2.Type == 65)
              return this.ProcessIsEmpty(child1, negated);
            break;
          }
          break;
      }
      return astNode;
    }

    public void HandleDotIdent()
    {
      if (this.input.LA(1) != 15 || this.input.LA(2) == 125 || !(this.input.LT(2) is HqlToken hqlToken) || !hqlToken.PossibleId)
        return;
      this.input.LT(2).Type = 125;
      if (!HqlParser.log.IsDebugEnabled)
        return;
      HqlParser.log.Debug((object) ("handleDotIdent() : new LT(2) token - " + (object) this.input.LT(1)));
    }

    private IASTNode CreateIsNullParent(IASTNode node, bool negated)
    {
      return (IASTNode) this.adaptor.BecomeRoot(this.adaptor.Create(negated ? 79 : 80, negated ? "is not null" : "is null"), (object) node);
    }

    private IASTNode ProcessIsEmpty(IASTNode node, bool negated)
    {
      IASTNode subquery = this.CreateSubquery(node);
      IASTNode oldRoot = (IASTNode) this.adaptor.BecomeRoot(this.adaptor.Create(19, "exists"), (object) subquery);
      if (!negated)
        oldRoot = (IASTNode) this.adaptor.BecomeRoot(this.adaptor.Create(38, "not"), (object) oldRoot);
      return oldRoot;
    }

    private IASTNode CreateSubquery(IASTNode node)
    {
      return (IASTNode) this.adaptor.BecomeRoot(this.adaptor.Create(86, "QUERY"), this.adaptor.BecomeRoot(this.adaptor.Create(89, "SELECT_FROM"), this.adaptor.BecomeRoot(this.adaptor.Create(22, "from"), this.adaptor.BecomeRoot(this.adaptor.Create(87, "RANGE"), (object) node))));
    }

    public IASTNode ProcessMemberOf(IToken n, IASTNode p, IASTNode root)
    {
      ASTFactory astFactory = new ASTFactory(this.adaptor);
      return astFactory.CreateNode(n == null ? 26 : 83, n == null ? "in" : "not in", new IASTNode[2]
      {
        !root.IsNil || root.ChildCount != 1 ? root : root.GetChild(0),
        astFactory.CreateNode(77, "inList", new IASTNode[1]
        {
          this.CreateSubquery(p)
        })
      });
    }

    public IASTNode HandleIdentifierError(IToken token, RecognitionException ex)
    {
      if (token is HqlToken && ((HqlToken) token).PossibleId && ex is MismatchedTokenException)
      {
        MismatchedTokenException mismatchedTokenException = (MismatchedTokenException) ex;
        if (mismatchedTokenException.Expecting == 125)
        {
          this._parseErrorHandler.ReportWarning("Keyword  '" + token.Text + "' is being interpreted as an identifier due to: " + mismatchedTokenException.Message);
          token.Type = 93;
          this.input.Consume();
          return (IASTNode) this.adaptor.Create(token);
        }
      }
      throw ex;
    }

    public class statement_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class updateStatement_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class setClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class assignment_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class stateField_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class newValue_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class deleteStatement_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class optionalFromTokenFromClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class optionalFromTokenFromClause2_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectStatement_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class insertStatement_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class intoClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class insertablePropertySpec_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class queryRule_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectFrom_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class newExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectObject_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class fromClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class fromJoin_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class withClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class fromRange_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class fromClassOrOuterQueryPath_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class inClassDeclaration_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class inCollectionDeclaration_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class inCollectionElementsDeclaration_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class asAlias_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class alias_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class propertyFetch_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class groupByClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class orderByClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class skipClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class takeClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class parameter_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class orderElement_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class ascendingOrDescending_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class havingClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class whereClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class selectedPropertiesList_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class aliasedExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class logicalExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class expression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class logicalOrExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class logicalAndExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class negatedExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class equalityExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class relationalExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class likeEscape_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class inList_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class betweenList_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class concatenation_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class bitwiseNotExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class bitwiseOrExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class bitwiseXOrExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class bitwiseAndExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class additiveExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class multiplyExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class unaryExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class caseExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class whenClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class altWhenClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class elseClause_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class quantifiedExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class atom_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class primaryExpression_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class expressionOrVector_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class vectorExpr_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class identPrimary_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class aggregate_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class aggregateDistinctAll_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class collectionExpr_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class compoundExpr_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class exprList_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class subQuery_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class innerSubQuery_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class constant_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class path_return : ParserRuleReturnScope
    {
      private IASTNode tree;

      public override object Tree
      {
        get => (object) this.tree;
        set => this.tree = (IASTNode) value;
      }
    }

    public class identifier_return : ParserRuleReturnScope
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
