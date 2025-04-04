// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.TypeNameParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Util
{
  public class TypeNameParser
  {
    private readonly string defaultNamespace;
    private readonly string defaultAssembly;
    private static readonly Regex WhiteSpaces = new Regex("[\\t\\r\\n]", RegexOptions.Compiled);
    private static readonly Regex MultipleSpaces = new Regex("[ ]+", RegexOptions.Compiled);

    public TypeNameParser(string defaultNamespace, string defaultAssembly)
    {
      this.defaultNamespace = defaultNamespace;
      this.defaultAssembly = defaultAssembly;
    }

    public static AssemblyQualifiedTypeName Parse(string type)
    {
      return TypeNameParser.Parse(type, (string) null, (string) null);
    }

    public static AssemblyQualifiedTypeName Parse(
      string type,
      string defaultNamespace,
      string defaultAssembly)
    {
      return new TypeNameParser(defaultNamespace, defaultAssembly).ParseTypeName(type);
    }

    public AssemblyQualifiedTypeName ParseTypeName(string typeName)
    {
      string input = typeName != null ? TypeNameParser.WhiteSpaces.Replace(typeName, " ") : throw new ArgumentNullException(nameof (typeName));
      string typeName1 = TypeNameParser.MultipleSpaces.Replace(input, " ").Replace(", [", ",[").Replace("[ [", "[[").Replace("] ]", "]]");
      int num1 = !(typeName1.Trim(' ', '[', ']', '\\', ',') == string.Empty) ? typeName1.IndexOf('[') : throw new ArgumentException(string.Format("The type to parse is not a type name:{0}", (object) typeName), nameof (typeName));
      int num2 = typeName1.LastIndexOf(']');
      int num3 = -1;
      if (num1 >= 0)
        num3 = typeName1.IndexOf('`', 0, num1);
      if (num1 == -1 || num3 == -1)
        return this.ParseNonGenericType(typeName1);
      bool isArrayType = typeName1.EndsWith("[]");
      int cardinality = num3 >= 0 ? int.Parse(typeName1.Substring(num3 + 1, 1)) : throw new ParserException("Invalid generic fully-qualified type name:" + typeName1);
      string typeName2 = typeName1.Substring(0, num1);
      if (typeName1.Length - num2 - 1 > 0)
        typeName2 += typeName1.Substring(num2 + 1, typeName1.Length - num2 - 1);
      List<AssemblyQualifiedTypeName> qualifiedTypeNameList = new List<AssemblyQualifiedTypeName>();
      foreach (string genericTypesArgument in TypeNameParser.GenericTypesArguments(typeName1.Substring(num1 + 1, num2 - num1 - 1), cardinality))
      {
        AssemblyQualifiedTypeName typeName3 = this.ParseTypeName(genericTypesArgument);
        qualifiedTypeNameList.Add(typeName3);
      }
      return this.MakeGenericType(this.ParseNonGenericType(typeName2), isArrayType, qualifiedTypeNameList.ToArray());
    }

    public AssemblyQualifiedTypeName MakeGenericType(
      AssemblyQualifiedTypeName qualifiedName,
      bool isArrayType,
      AssemblyQualifiedTypeName[] typeArguments)
    {
      string type = qualifiedName.Type;
      StringBuilder stringBuilder = new StringBuilder(typeArguments.Length * 200);
      stringBuilder.Append(type);
      stringBuilder.Append('[');
      for (int index = 0; index < typeArguments.Length; ++index)
      {
        if (index > 0)
          stringBuilder.Append(",");
        stringBuilder.Append('[').Append(typeArguments[index].ToString()).Append(']');
      }
      stringBuilder.Append(']');
      if (isArrayType)
        stringBuilder.Append("[]");
      return new AssemblyQualifiedTypeName(stringBuilder.ToString(), qualifiedName.Assembly);
    }

    private static IEnumerable<string> GenericTypesArguments(string s, int cardinality)
    {
      int startIndex = 0;
      for (; cardinality > 0; --cardinality)
      {
        StringBuilder sb = new StringBuilder(s.Length);
        int bracketCount = 0;
        for (int i = startIndex; i < s.Length; ++i)
        {
          switch (s[i])
          {
            case '[':
              ++bracketCount;
              break;
            case ']':
              if (--bracketCount == 0)
              {
                string item = s.Substring(startIndex + 1, i - startIndex - 1);
                yield return item;
                sb = new StringBuilder(s.Length);
                startIndex = i + 2;
                break;
              }
              break;
            default:
              sb.Append(s[i]);
              break;
          }
        }
        if (bracketCount != 0)
          throw new ParserException(string.Format("The brackets are unbalanced in the type name: {0}", (object) s));
        if (sb.Length > 0)
        {
          string result = sb.ToString();
          startIndex += result.Length;
          yield return result.TrimStart(' ', ',');
        }
      }
    }

    private AssemblyQualifiedTypeName ParseNonGenericType(string typeName)
    {
      string assembly;
      string str;
      if (TypeNameParser.NeedDefaultAssembly(typeName))
      {
        assembly = this.defaultAssembly;
        str = typeName;
      }
      else
      {
        int qualifiedNameStartIndex = TypeNameParser.FindAssemblyQualifiedNameStartIndex(typeName);
        if (qualifiedNameStartIndex > 0)
        {
          assembly = typeName.Substring(qualifiedNameStartIndex + 1, typeName.Length - qualifiedNameStartIndex - 1).Trim();
          str = typeName.Substring(0, qualifiedNameStartIndex).Trim();
        }
        else
        {
          assembly = (string) null;
          str = typeName.Trim();
        }
      }
      if (TypeNameParser.NeedDefaultNamespace(str) && !string.IsNullOrEmpty(this.defaultNamespace))
        str = this.defaultNamespace + "." + str;
      return new AssemblyQualifiedTypeName(str, assembly);
    }

    private static int FindAssemblyQualifiedNameStartIndex(string typeName)
    {
      for (int index = 0; index < typeName.Length; ++index)
      {
        if (typeName[index] == ',' && typeName[index - 1] != '\\')
          return index;
      }
      return -1;
    }

    private static bool NeedDefaultNamespaceOrDefaultAssembly(string typeFullName)
    {
      return !typeFullName.StartsWith("System.");
    }

    private static bool NeedDefaultNamespace(string typeFullName)
    {
      if (!TypeNameParser.NeedDefaultNamespaceOrDefaultAssembly(typeFullName))
        return false;
      int num1 = typeFullName.IndexOf(',');
      int num2 = typeFullName.IndexOf('.');
      if (num2 < 0)
        return true;
      return num2 > num1 && num1 > 0;
    }

    private static bool NeedDefaultAssembly(string typeFullName)
    {
      return TypeNameParser.NeedDefaultNamespaceOrDefaultAssembly(typeFullName) && typeFullName.IndexOf(',') < 0;
    }
  }
}
