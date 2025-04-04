// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlString
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.SqlCommand
{
  [Serializable]
  public class SqlString : ICollection, IEnumerable<object>, IEnumerable
  {
    public static readonly SqlString Empty = new SqlString(Enumerable.Empty<object>());
    private readonly List<SqlString.Part> _parts;
    private readonly SortedList<int, Parameter> _parameters;
    private readonly int _firstPartIndex;
    private readonly int _lastPartIndex;
    private readonly int _sqlStartIndex;
    private readonly int _length;

    private SqlString(SqlString other)
    {
      this._parts = other._parts;
      this._sqlStartIndex = other._sqlStartIndex;
      this._length = other._length;
      this._firstPartIndex = other._firstPartIndex;
      this._lastPartIndex = other._lastPartIndex;
      if (other._parameters.Count > 0)
      {
        this._parameters = new SortedList<int, Parameter>(other._parameters.Count);
        foreach (KeyValuePair<int, Parameter> parameter1 in other._parameters)
        {
          Parameter parameter2 = parameter1.Value;
          Parameter parameter3 = parameter2.Clone();
          int? parameterPosition = parameter2.ParameterPosition;
          if ((parameterPosition.GetValueOrDefault() >= 0 ? 0 : (parameterPosition.HasValue ? 1 : 0)) != 0)
            parameter3.ParameterPosition = parameter2.ParameterPosition;
          this._parameters.Add(parameter1.Key, parameter3);
        }
      }
      else
        this._parameters = SqlString.Empty._parameters;
    }

    private SqlString(SqlString other, int sqlStartIndex, int length)
    {
      this._parts = other._parts;
      this._sqlStartIndex = sqlStartIndex;
      this._length = length;
      this._firstPartIndex = other.GetPartIndexForSqlIndex(sqlStartIndex);
      this._lastPartIndex = other.GetPartIndexForSqlIndex(this._sqlStartIndex + this._length - 1);
      if (this._firstPartIndex != this._lastPartIndex || this._parts[this._firstPartIndex].IsParameter)
      {
        this._parameters = new SortedList<int, Parameter>(other._parameters.Count);
        using (IEnumerator<KeyValuePair<int, Parameter>> enumerator = other._parameters.GetEnumerator())
        {
          do
            ;
          while (enumerator.MoveNext() && enumerator.Current.Key < this._sqlStartIndex);
          int num = this._sqlStartIndex + this._length;
          while (enumerator.Current.Key <= num)
          {
            this._parameters.Add(enumerator.Current.Key, enumerator.Current.Value);
            if (!enumerator.MoveNext())
              break;
          }
        }
      }
      else
        this._parameters = SqlString.Empty._parameters;
    }

    public SqlString(string sql)
    {
      this._parts = sql != null ? new List<SqlString.Part>(1)
      {
        new SqlString.Part(0, sql)
      } : throw new ArgumentNullException(nameof (sql));
      this._parameters = SqlString.Empty._parameters;
      this._length = sql.Length;
    }

    public SqlString(Parameter parameter)
    {
      if (parameter == (Parameter) null)
        throw new ArgumentNullException(nameof (parameter));
      this._parts = new List<SqlString.Part>(1)
      {
        new SqlString.Part(0)
      };
      this._parameters = new SortedList<int, Parameter>(1)
      {
        {
          0,
          parameter
        }
      };
      this._length = this._parts[0].Length;
    }

    public SqlString(params object[] parts)
      : this((IEnumerable<object>) parts)
    {
    }

    private SqlString(IEnumerable<object> parts)
    {
      this._parts = new List<SqlString.Part>();
      this._parameters = new SortedList<int, Parameter>();
      int sqlIndex = 0;
      StringBuilder pendingContent = new StringBuilder();
      foreach (object part in parts)
        this.Add(part, pendingContent, ref sqlIndex);
      this.AppendAndResetPendingContent(pendingContent, ref sqlIndex);
      this._firstPartIndex = this._parts.Count > 0 ? 0 : -1;
      this._lastPartIndex = this._parts.Count - 1;
      this._length = sqlIndex;
    }

    public static SqlString Parse(string sql)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (char ch in sql)
      {
        switch (ch)
        {
          case '\'':
            flag = !flag;
            stringBuilder.Append(ch);
            break;
          case '?':
            if (flag)
            {
              stringBuilder.Append(ch);
              break;
            }
            if (stringBuilder.Length > 0)
            {
              sqlStringBuilder.Add(stringBuilder.ToString());
              stringBuilder.Length = 0;
            }
            sqlStringBuilder.AddParameter();
            break;
          default:
            stringBuilder.Append(ch);
            break;
        }
      }
      if (stringBuilder.Length > 0)
        sqlStringBuilder.Add(stringBuilder.ToString());
      return sqlStringBuilder.ToSqlString();
    }

    public int Count => this._length <= 0 ? 0 : this._lastPartIndex - this._firstPartIndex + 1;

    public int Length => this._length;

    [Obsolete("Use SqlString.Count and SqlString.GetEnumerator properties")]
    public ICollection Parts => (ICollection) this;

    public static SqlString operator +(SqlString lhs, SqlString rhs) => lhs.Append(rhs);

    public SqlString Append(SqlString sql)
    {
      if (sql == null || sql._length == 0)
        return this;
      if (this._length == 0)
        return sql;
      return new SqlString(new object[2]
      {
        (object) this,
        (object) sql
      });
    }

    public SqlString Append(string text)
    {
      if (string.IsNullOrEmpty(text))
        return this;
      if (this._length == 0)
        return new SqlString(text);
      return new SqlString(new object[2]
      {
        (object) this,
        (object) text
      });
    }

    public SqlString Compact() => this;

    public SqlString Copy() => new SqlString(this);

    public bool EndsWith(string value)
    {
      return value != null && value.Length <= this._length && this.IndexOf(value, this._length - value.Length, value.Length, StringComparison.InvariantCulture) >= 0;
    }

    public bool EndsWithCaseInsensitive(string value)
    {
      return value != null && value.Length <= this._length && this.IndexOf(value, this._length - value.Length, value.Length, StringComparison.CurrentCultureIgnoreCase) >= 0;
    }

    public IEnumerable<Parameter> GetParameters()
    {
      return (IEnumerable<Parameter>) this._parameters.Values;
    }

    public int GetParameterCount() => this._parameters.Count;

    public int IndexOfCaseInsensitive(string text)
    {
      return this.IndexOf(text, 0, this._length, StringComparison.InvariantCultureIgnoreCase);
    }

    public int IndexOf(string text, int startIndex, int length, StringComparison stringComparison)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      int sqlIndex = this._sqlStartIndex + startIndex;
      int val1 = Math.Min(length, this._sqlStartIndex + this._length - sqlIndex);
      if (val1 >= text.Length)
      {
        int indexForSqlIndex = this.GetPartIndexForSqlIndex(sqlIndex);
        if (indexForSqlIndex >= 0)
        {
          for (; val1 > 0 && indexForSqlIndex <= this._lastPartIndex; ++indexForSqlIndex)
          {
            SqlString.Part part = this._parts[indexForSqlIndex];
            int startIndex1 = sqlIndex - part.SqlIndex;
            int count = Math.Min(val1, part.Length - startIndex1);
            int num = part.Content.IndexOf(text, startIndex1, count, stringComparison);
            if (num >= 0)
              return part.SqlIndex + num - this._sqlStartIndex;
            sqlIndex += count;
            val1 -= count;
          }
        }
      }
      return -1;
    }

    public SqlString Insert(int index, string text)
    {
      if (string.IsNullOrEmpty(text))
        return this;
      return new SqlString(new object[3]
      {
        (object) this.Substring(0, index),
        (object) text,
        (object) this.Substring(index, this._length - index)
      });
    }

    public SqlString Insert(int index, SqlString sql)
    {
      if (sql == null || sql._length == 0)
        return this;
      return new SqlString(new object[3]
      {
        (object) this.Substring(0, index),
        (object) sql,
        (object) this.Substring(index, this._length - index)
      });
    }

    public int LastIndexOfCaseInsensitive(string text)
    {
      return this.LastIndexOf(text, 0, this._length, StringComparison.InvariantCultureIgnoreCase);
    }

    private int LastIndexOf(
      string text,
      int startIndex,
      int length,
      StringComparison stringComparison)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      int num1 = this._sqlStartIndex + Math.Min(this._length, startIndex + length);
      int val1 = num1 - this._sqlStartIndex - startIndex;
      if (val1 > text.Length)
      {
        int indexForSqlIndex = this.GetPartIndexForSqlIndex(num1 - 1);
        if (indexForSqlIndex >= 0)
        {
          for (; val1 > 0 && indexForSqlIndex >= this._firstPartIndex; --indexForSqlIndex)
          {
            SqlString.Part part = this._parts[indexForSqlIndex];
            int val2 = num1 - part.SqlIndex;
            int count = Math.Min(val1, val2);
            int num2 = part.Content.LastIndexOf(text, val2 - 1, count, stringComparison);
            if (num2 >= 0)
              return part.SqlIndex + num2 - this._sqlStartIndex;
            num1 -= count;
            val1 -= count;
          }
        }
      }
      return -1;
    }

    public SqlString Replace(string oldValue, string newValue)
    {
      return new SqlString(this.ReplaceParts(oldValue, newValue));
    }

    private IEnumerable<object> ReplaceParts(string oldValue, string newValue)
    {
      foreach (object part in this)
      {
        string content = part as string;
        yield return content != null ? (object) content.Replace(oldValue, newValue) : part;
      }
    }

    public SqlString[] Split(string splitter) => this.SplitParts(splitter).ToArray<SqlString>();

    internal SqlString[] SplitWithRegex(string pattern)
    {
      SqlString[] array = ((IEnumerable<string>) Regex.Split(this.ToString(), pattern)).Select<string, SqlString>((Func<string, SqlString>) (s => SqlString.Parse(s))).ToArray<SqlString>();
      IList<Parameter> values = this._parameters.Values;
      int index = 0;
      foreach (Parameter parameter in ((IEnumerable<SqlString>) array).SelectMany<SqlString, Parameter>((Func<SqlString, IEnumerable<Parameter>>) (s => s.GetParameters())))
      {
        parameter.BackTrack = values[index].BackTrack;
        ++index;
      }
      return array;
    }

    private IEnumerable<SqlString> SplitParts(string splitter)
    {
      int startIndex;
      int splitterIndex;
      for (startIndex = 0; startIndex < this._length; startIndex = splitterIndex + splitter.Length)
      {
        splitterIndex = this.IndexOf(splitter, startIndex, this._length - startIndex, StringComparison.InvariantCultureIgnoreCase);
        if (splitterIndex >= 0)
          yield return new SqlString(this, this._sqlStartIndex + startIndex, splitterIndex - startIndex);
        else
          break;
      }
      if (startIndex < this._length)
        yield return new SqlString(this, this._sqlStartIndex + startIndex, this._length - startIndex);
    }

    public bool StartsWithCaseInsensitive(string value)
    {
      return value != null && value.Length <= this._length && this.IndexOf(value, 0, value.Length, StringComparison.InvariantCultureIgnoreCase) >= 0;
    }

    public SqlString Substring(int startIndex)
    {
      return this.Substring(startIndex, this._length - startIndex);
    }

    public SqlString Substring(int startIndex, int length)
    {
      if (startIndex == 0 && length == this._length)
        return this;
      length = Math.Min(this._length - startIndex, length);
      return length <= 0 ? SqlString.Empty : new SqlString(this, this._sqlStartIndex + startIndex, length);
    }

    public SqlString SubstringStartingWithLast(string text)
    {
      int startIndex = this.LastIndexOfCaseInsensitive(text);
      return startIndex < 0 ? SqlString.Empty : this.Substring(startIndex);
    }

    public SqlString Trim()
    {
      if (this._firstPartIndex < 0)
        return this;
      SqlString.Part part1 = this._parts[this._firstPartIndex];
      int index1 = this._sqlStartIndex - part1.SqlIndex;
      for (int index2 = Math.Min(part1.Length - index1, this._length); index2 > 0 && char.IsWhiteSpace(part1.Content[index1]); --index2)
        ++index1;
      SqlString.Part part2 = this._parts[this._lastPartIndex];
      int index3 = this._sqlStartIndex + this._length - 1 - part2.SqlIndex;
      for (int index4 = Math.Min(index3 + 1, this._length); index4 > 0 && char.IsWhiteSpace(part2.Content[index3]); --index4)
        --index3;
      int sqlStartIndex = part1.SqlIndex + index1;
      int length = part2.SqlIndex + index3 + 1 - sqlStartIndex;
      return length <= 0 ? SqlString.Empty : new SqlString(this, sqlStartIndex, length);
    }

    public void Visit(ISqlStringVisitor visitor)
    {
      foreach (object obj in this)
      {
        switch (obj)
        {
          case string text:
            visitor.String(text);
            continue;
          case SqlString sqlString:
            visitor.String(sqlString);
            continue;
          default:
            Parameter parameter = obj as Parameter;
            if (parameter != (Parameter) null)
            {
              visitor.Parameter(parameter);
              continue;
            }
            continue;
        }
      }
    }

    private int GetPartIndexForSqlIndex(int sqlIndex)
    {
      if (sqlIndex < this._sqlStartIndex || sqlIndex >= this._sqlStartIndex + this._length)
        return -1;
      int index1 = this._firstPartIndex;
      int num = this._lastPartIndex;
      while (index1 < num)
      {
        int index2 = (index1 + num + 1) / 2;
        if (sqlIndex < this._parts[index2].SqlIndex)
          num = index2 - 1;
        else
          index1 = index2;
      }
      SqlString.Part part = this._parts[index1];
      return sqlIndex < part.SqlIndex || sqlIndex >= part.SqlIndex + part.Length ? -1 : index1;
    }

    private void Add(object part, StringBuilder pendingContent, ref int sqlIndex)
    {
      if (part is string str)
      {
        pendingContent.Append(str);
      }
      else
      {
        Parameter parameter = part as Parameter;
        if (parameter != (Parameter) null)
        {
          this.AppendAndResetPendingContent(pendingContent, ref sqlIndex);
          this._parts.Add(new SqlString.Part(sqlIndex));
          this._parameters.Add(sqlIndex, parameter);
          ++sqlIndex;
        }
        else
        {
          if (!(part is SqlString sqlString))
            throw new ArgumentException("Only string, Parameter or SqlString values are supported as SqlString parts.");
          foreach (object part1 in sqlString)
            this.Add(part1, pendingContent, ref sqlIndex);
        }
      }
    }

    private void AppendAndResetPendingContent(StringBuilder pendingContent, ref int sqlIndex)
    {
      if (pendingContent.Length <= 0)
        return;
      this._parts.Add(new SqlString.Part(sqlIndex, pendingContent.ToString()));
      sqlIndex += pendingContent.Length;
      pendingContent.Length = 0;
    }

    void ICollection.CopyTo(Array array, int index)
    {
      foreach (object obj in this)
        array.SetValue(obj, index++);
    }

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => (object) null;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public IEnumerator<object> GetEnumerator()
    {
      if (this._firstPartIndex >= 0)
      {
        int partIndex = this._firstPartIndex;
        SqlString.Part part = this._parts[partIndex++];
        if (part.IsParameter)
        {
          yield return (object) this._parameters[part.SqlIndex];
        }
        else
        {
          int firstPartOffset = this._sqlStartIndex - part.SqlIndex;
          int firstPartLength = Math.Min(part.Length - firstPartOffset, this._length);
          yield return (object) part.Content.Substring(firstPartOffset, firstPartLength);
        }
        if (this._firstPartIndex != this._lastPartIndex)
        {
          while (partIndex < this._lastPartIndex)
          {
            part = this._parts[partIndex++];
            yield return part.IsParameter ? (object) this._parameters[part.SqlIndex] : (object) part.Content;
          }
          part = this._parts[partIndex];
          if (part.IsParameter)
          {
            yield return (object) this._parameters[part.SqlIndex];
          }
          else
          {
            int lastPartLength = this._sqlStartIndex + this._length - part.SqlIndex;
            yield return (object) part.Content.Substring(0, lastPartLength);
          }
        }
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj is SqlString sqlString))
        return false;
      if (sqlString == this)
        return true;
      if (this._length != sqlString._length || this._lastPartIndex - this._firstPartIndex != sqlString._lastPartIndex - sqlString._firstPartIndex || this._parameters.Count != sqlString._parameters.Count)
        return false;
      using (IEnumerator<object> enumerator1 = this.GetEnumerator())
      {
        using (IEnumerator<object> enumerator2 = sqlString.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            if (!enumerator2.MoveNext() || !object.Equals(enumerator1.Current, enumerator2.Current))
              return false;
          }
          if (enumerator2.MoveNext())
            return false;
        }
      }
      return true;
    }

    public override int GetHashCode()
    {
      uint hashCode = 2166136261;
      foreach (object obj in this)
      {
        hashCode ^= (uint) obj.GetHashCode();
        hashCode *= 16777619U;
      }
      return (int) hashCode;
    }

    public override string ToString() => this.ToString(0, this._length);

    public string ToString(int startIndex, int length)
    {
      int sqlIndex = this._sqlStartIndex + startIndex;
      int indexForSqlIndex = this.GetPartIndexForSqlIndex(sqlIndex);
      if (indexForSqlIndex < 0)
        return string.Empty;
      int val2 = Math.Min(this._sqlStartIndex + this._length - sqlIndex, length);
      List<SqlString.Part> parts1 = this._parts;
      int index1 = indexForSqlIndex;
      int num1 = index1 + 1;
      SqlString.Part part = parts1[index1];
      int startIndex1 = sqlIndex - part.SqlIndex;
      int num2 = Math.Min(part.Length - startIndex1, val2);
      if (num2 == val2)
        return part.Length != num2 ? part.Content.Substring(startIndex1, num2) : part.Content;
      StringBuilder stringBuilder = new StringBuilder(length);
      stringBuilder.Append(part.Content, startIndex1, num2);
      int count = val2 - num2;
      List<SqlString.Part> parts2 = this._parts;
      int index2 = num1;
      int num3 = index2 + 1;
      for (part = parts2[index2]; num3 <= this._lastPartIndex && part.Length <= count; part = this._parts[num3++])
      {
        stringBuilder.Append(part.Content);
        count -= part.Length;
      }
      if (count > 0)
        stringBuilder.Append(part.Content, 0, count);
      return stringBuilder.ToString();
    }

    public SqlString GetSubselectString() => new SubselectClauseExtractor(this).GetSqlString();

    [Serializable]
    private struct Part(int sqlIndex, string content) : IEquatable<SqlString.Part>
    {
      public readonly int SqlIndex = sqlIndex;
      public readonly string Content = content;
      public readonly bool IsParameter = false;

      public Part(int sqlIndex)
        : this(sqlIndex, "?")
      {
        this.IsParameter = true;
      }

      public int Length => this.Content.Length;

      public bool Equals(SqlString.Part other)
      {
        return this.IsParameter == other.IsParameter && this.Content == other.Content;
      }

      public override bool Equals(object obj) => obj is SqlString.Part other && this.Equals(other);

      public override int GetHashCode() => this.Content.GetHashCode();

      public override string ToString() => this.Content;
    }
  }
}
