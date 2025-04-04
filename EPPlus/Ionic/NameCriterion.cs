// Decompiled with JetBrains decompiler
// Type: Ionic.NameCriterion
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Ionic
{
  internal class NameCriterion : SelectionCriterion
  {
    private Regex _re;
    private string _regexString;
    internal ComparisonOperator Operator;
    private string _MatchingFileSpec;

    internal virtual string MatchingFileSpec
    {
      set
      {
        this._MatchingFileSpec = !Directory.Exists(value) ? value : ".\\" + value + "\\*.*";
        this._regexString = "^" + Regex.Escape(this._MatchingFileSpec).Replace("\\\\\\*\\.\\*", "\\\\([^\\.]+|.*\\.[^\\\\\\.]*)").Replace("\\.\\*", "\\.[^\\\\\\.]*").Replace("\\*", ".*").Replace("\\?", "[^\\\\\\.]") + "$";
        this._re = new Regex(this._regexString, RegexOptions.IgnoreCase);
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("name ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" '").Append(this._MatchingFileSpec).Append("'");
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(string filename) => this._Evaluate(filename);

    private bool _Evaluate(string fullpath)
    {
      bool flag = this._re.IsMatch(this._MatchingFileSpec.IndexOf('\\') == -1 ? Path.GetFileName(fullpath) : fullpath);
      if (this.Operator != ComparisonOperator.EqualTo)
        flag = !flag;
      return flag;
    }

    internal override bool Evaluate(ZipEntry entry)
    {
      return this._Evaluate(entry.FileName.Replace("/", "\\"));
    }
  }
}
