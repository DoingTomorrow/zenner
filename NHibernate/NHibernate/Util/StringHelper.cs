// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.StringHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace NHibernate.Util
{
  public static class StringHelper
  {
    public const string WhiteSpace = " \n\r\f\t";
    public const char Dot = '.';
    public const char Underscore = '_';
    public const string CommaSpace = ", ";
    public const string Comma = ",";
    public const string OpenParen = "(";
    public const string ClosedParen = ")";
    public const char SingleQuote = '\'';
    public const string SqlParameter = "?";
    public const int AliasTruncateLength = 10;

    public static string Join(string separator, IEnumerable objects)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (object obj in objects)
      {
        if (!flag)
          stringBuilder.Append(separator);
        flag = false;
        stringBuilder.Append(obj);
      }
      return stringBuilder.ToString();
    }

    public static SqlString Join(SqlString separator, IEnumerable objects)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      bool flag = true;
      foreach (object part in objects)
      {
        if (!flag)
          sqlStringBuilder.Add(separator);
        flag = false;
        sqlStringBuilder.AddObject(part);
      }
      return sqlStringBuilder.ToSqlString();
    }

    public static SqlString[] Add(SqlString[] x, string sep, SqlString[] y)
    {
      SqlString[] sqlStringArray = new SqlString[x.Length];
      for (int index = 0; index < x.Length; ++index)
        sqlStringArray[index] = new SqlStringBuilder(3).Add(x[index]).Add(sep).Add(y[index]).ToSqlString();
      return sqlStringArray;
    }

    public static string Repeat(string str, int times)
    {
      StringBuilder stringBuilder = new StringBuilder(str.Length * times);
      for (int index = 0; index < times; ++index)
        stringBuilder.Append(str);
      return stringBuilder.ToString();
    }

    public static string Replace(string template, string placeholder, string replacement)
    {
      return StringHelper.Replace(template, placeholder, replacement, false);
    }

    public static string Replace(
      string template,
      string placeholder,
      string replacement,
      bool wholeWords)
    {
      if (template == null)
        return (string) null;
      int length = template.IndexOf(placeholder);
      if (length < 0)
        return template;
      string str1 = replacement;
      if (length + placeholder.Length < template.Length)
      {
        string str2 = template[length + placeholder.Length].ToString();
        if (wholeWords && !" \n\r\f\t".Contains(str2) && !")".Equals(str2) && !",".Equals(str2))
          str1 = placeholder;
      }
      return new StringBuilder(template.Substring(0, length)).Append(str1).Append(StringHelper.Replace(template.Substring(length + placeholder.Length), placeholder, replacement, wholeWords)).ToString();
    }

    public static string ReplaceOnce(string template, string placeholder, string replacement)
    {
      int length = template.IndexOf(placeholder);
      return length < 0 ? template : new StringBuilder(template.Substring(0, length)).Append(replacement).Append(template.Substring(length + placeholder.Length)).ToString();
    }

    public static string[] Split(string separators, string list)
    {
      return list.Split(separators.ToCharArray());
    }

    public static string[] Split(string separators, string list, bool include)
    {
      StringTokenizer stringTokenizer = new StringTokenizer(list, separators, include);
      ArrayList arrayList = new ArrayList();
      foreach (string str in stringTokenizer)
        arrayList.Add((object) str);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public static string Unqualify(string qualifiedName)
    {
      return qualifiedName.IndexOf('`') > 0 ? StringHelper.GetClassname(qualifiedName) : StringHelper.Unqualify(qualifiedName, ".");
    }

    public static string Unqualify(string qualifiedName, string seperator)
    {
      return qualifiedName.Substring(qualifiedName.LastIndexOf(seperator) + 1);
    }

    public static string GetFullClassname(string typeName)
    {
      return new TypeNameParser((string) null, (string) null).ParseTypeName(typeName).Type;
    }

    public static string GetClassname(string typeName)
    {
      string fullClassname = StringHelper.GetFullClassname(typeName);
      int length = fullClassname.IndexOf('`');
      if (length != -1)
      {
        int num = fullClassname.Substring(0, length).LastIndexOf('.');
        return num == -1 ? fullClassname : fullClassname.Substring(num + 1);
      }
      string[] strArray = fullClassname.Split('.');
      return strArray[strArray.Length - 1];
    }

    public static string Qualifier(string qualifiedName)
    {
      int length = qualifiedName.LastIndexOf(".");
      return length < 0 ? string.Empty : qualifiedName.Substring(0, length);
    }

    public static string[] Suffix(string[] columns, string suffix)
    {
      if (suffix == null)
        return columns;
      string[] strArray = new string[columns.Length];
      for (int index = 0; index < columns.Length; ++index)
        strArray[index] = StringHelper.Suffix(columns[index], suffix);
      return strArray;
    }

    public static string Suffix(string name, string suffix)
    {
      return suffix != null ? name + suffix : name;
    }

    public static string[] Prefix(string[] columns, string prefix)
    {
      if (prefix == null)
        return columns;
      string[] strArray = new string[columns.Length];
      for (int index = 0; index < columns.Length; ++index)
        strArray[index] = prefix + columns[index];
      return strArray;
    }

    public static string Root(string qualifiedName)
    {
      int length = qualifiedName.IndexOf(".");
      return length >= 0 ? qualifiedName.Substring(0, length) : qualifiedName;
    }

    public static bool BooleanValue(string value)
    {
      string lowerInvariant = value.Trim().ToLowerInvariant();
      return lowerInvariant.Equals("true") || lowerInvariant.Equals("t");
    }

    private static string NullSafeToString(object obj) => obj != null ? obj.ToString() : "(null)";

    public static string ToString(object[] array)
    {
      int length = array.Length;
      if (length == 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(length * 12);
      for (int index = 0; index < length - 1; ++index)
        stringBuilder.Append(StringHelper.NullSafeToString(array[index])).Append(", ");
      return stringBuilder.Append(StringHelper.NullSafeToString(array[length - 1])).ToString();
    }

    public static string LinesToString(this string[] text)
    {
      if (text == null)
        return (string) null;
      if (text.Length == 1)
        return text[0];
      StringBuilder sb = new StringBuilder(200);
      Array.ForEach<string>(text, (Action<string>) (t => sb.AppendLine(t)));
      return sb.ToString();
    }

    public static string[] Multiply(string str, IEnumerator placeholders, IEnumerator replacements)
    {
      string[] strings = new string[1]{ str };
      while (placeholders.MoveNext())
      {
        replacements.MoveNext();
        strings = StringHelper.Multiply(strings, placeholders.Current as string, replacements.Current as string[]);
      }
      return strings;
    }

    public static string[] Multiply(string[] strings, string placeholder, string[] replacements)
    {
      string[] strArray = new string[replacements.Length * strings.Length];
      int num = 0;
      for (int index1 = 0; index1 < replacements.Length; ++index1)
      {
        for (int index2 = 0; index2 < strings.Length; ++index2)
          strArray[num++] = StringHelper.ReplaceOnce(strings[index2], placeholder, replacements[index1]);
      }
      return strArray;
    }

    public static int CountUnquoted(string str, char character)
    {
      if ('\'' == character)
        throw new ArgumentOutOfRangeException(nameof (character), "Unquoted count of quotes is invalid");
      int num = 0;
      char[] charArray = str.ToCharArray();
      int length = string.IsNullOrEmpty(str) ? 0 : charArray.Length;
      bool flag = false;
      for (int index = 0; index < length; ++index)
      {
        if (flag)
        {
          if ('\'' == charArray[index])
            flag = false;
        }
        else if ('\'' == charArray[index])
          flag = true;
        else if ((int) charArray[index] == (int) character)
          ++num;
      }
      return num;
    }

    public static bool IsEmpty(string str) => string.IsNullOrEmpty(str);

    public static bool IsNotEmpty(string str) => !StringHelper.IsEmpty(str);

    public static bool IsNotEmpty(SqlString str) => !StringHelper.IsEmpty(str);

    public static bool IsEmpty(SqlString str) => str == null || str.Count == 0;

    public static string Qualify(string prefix, string name)
    {
      char c = name[0];
      return !string.IsNullOrEmpty(prefix) && c != '\'' && !char.IsDigit(c) ? prefix + (object) '.' + name : name;
    }

    public static string[] Qualify(string prefix, string[] names)
    {
      if (string.IsNullOrEmpty(prefix))
        return names;
      int length = names.Length;
      string[] strArray = new string[length];
      for (int index = 0; index < length; ++index)
        strArray[index] = StringHelper.Qualify(prefix, names[index]);
      return strArray;
    }

    public static int FirstIndexOfChar(string sqlString, string str, int startIndex)
    {
      return sqlString.IndexOfAny(str.ToCharArray(), startIndex);
    }

    public static string Truncate(string str, int length)
    {
      return str.Length <= length ? str : str.Substring(0, length);
    }

    public static int LastIndexOfLetter(string str)
    {
      for (int index = 0; index < str.Length; ++index)
      {
        if (!char.IsLetter(str, index))
          return index - 1;
      }
      return str.Length - 1;
    }

    public static string UnqualifyEntityName(string entityName)
    {
      string str = StringHelper.Unqualify(entityName);
      int num = str.IndexOf('/');
      if (num > 0)
        str = str.Substring(0, num - 1);
      return str;
    }

    public static string GenerateAlias(string description)
    {
      return StringHelper.GenerateAliasRoot(description) + (object) '_';
    }

    public static string GenerateAlias(string description, int unique)
    {
      return StringHelper.GenerateAliasRoot(description) + (object) unique + (object) '_';
    }

    private static string GenerateAliasRoot(string description)
    {
      int length = description.IndexOf('`');
      if (length > 0)
        description = StringHelper.Truncate(description, length);
      string s = StringHelper.Truncate(StringHelper.UnqualifyEntityName(description), 10).ToLowerInvariant().Replace('/', '_').Replace('+', '_').Replace('[', '_').Replace(']', '_').Replace('`', '_').TrimStart('_');
      if (char.IsDigit(s, s.Length - 1))
        return s + "x";
      return char.IsLetter(s[0]) || '_' == s[0] ? s : "alias_" + s;
    }

    public static string MoveAndToBeginning(string filter)
    {
      if (filter.Trim().Length > 0)
      {
        filter += " and ";
        if (filter.StartsWith(" and "))
          filter = filter.Substring(4);
      }
      return filter;
    }

    public static string Unroot(string qualifiedName)
    {
      int num = qualifiedName.IndexOf(".");
      return num >= 0 ? qualifiedName.Substring(num + 1) : qualifiedName;
    }

    public static bool EqualsCaseInsensitive(string a, string b)
    {
      return StringComparer.InvariantCultureIgnoreCase.Compare(a, b) == 0;
    }

    public static int IndexOfCaseInsensitive(string source, string value)
    {
      return source.IndexOf(value, StringComparison.InvariantCultureIgnoreCase);
    }

    public static int IndexOfCaseInsensitive(string source, string value, int startIndex)
    {
      return source.IndexOf(value, startIndex, StringComparison.InvariantCultureIgnoreCase);
    }

    public static int IndexOfCaseInsensitive(
      string source,
      string value,
      int startIndex,
      int count)
    {
      return source.IndexOf(value, startIndex, count, StringComparison.InvariantCultureIgnoreCase);
    }

    public static int LastIndexOfCaseInsensitive(string source, string value)
    {
      return source.LastIndexOf(value, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool StartsWithCaseInsensitive(string source, string prefix)
    {
      return source.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase);
    }

    public static string InternedIfPossible(string str)
    {
      return str == null ? (string) null : string.IsInterned(str) ?? str;
    }

    public static string CollectionToString(ICollection keys)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object key in (IEnumerable) keys)
      {
        stringBuilder.Append(key);
        stringBuilder.Append(", ");
      }
      if (stringBuilder.Length != 0)
        stringBuilder.Remove(stringBuilder.Length - 2, 2);
      return stringBuilder.ToString();
    }

    public static SqlString RemoveAsAliasesFromSql(SqlString sql)
    {
      return sql.Substring(0, sql.LastIndexOfCaseInsensitive(" as "));
    }

    public static string ToUpperCase(string str) => str?.ToUpperInvariant();

    public static string ToLowerCase(string str) => str?.ToLowerInvariant();

    public static bool IsBackticksEnclosed(string identifier)
    {
      return !string.IsNullOrEmpty(identifier) && identifier.StartsWith("`") && identifier.EndsWith("`");
    }

    public static string PurgeBackticksEnclosing(string identifier)
    {
      return StringHelper.IsBackticksEnclosed(identifier) ? identifier.Substring(1, identifier.Length - 2) : identifier;
    }

    public static string[] ParseFilterParameterName(string filterParameterName)
    {
      int length = filterParameterName.IndexOf(".");
      return length > 0 ? new string[2]
      {
        filterParameterName.Substring(0, length),
        filterParameterName.Substring(length + 1)
      } : throw new ArgumentException("Invalid filter-parameter name format; the name should be a property path.", nameof (filterParameterName));
    }
  }
}
