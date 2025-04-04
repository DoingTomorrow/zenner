// Decompiled with JetBrains decompiler
// Type: Ionic.TypeCriterion
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Ionic
{
  internal class TypeCriterion : SelectionCriterion
  {
    private char ObjectType;
    internal ComparisonOperator Operator;

    internal string AttributeString
    {
      get => this.ObjectType.ToString();
      set
      {
        this.ObjectType = value.Length == 1 && (value[0] == 'D' || value[0] == 'F') ? value[0] : throw new ArgumentException("Specify a single character: either D or F");
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("type ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.AttributeString);
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(string filename)
    {
      bool flag = this.ObjectType == 'D' ? Directory.Exists(filename) : File.Exists(filename);
      if (this.Operator != ComparisonOperator.EqualTo)
        flag = !flag;
      return flag;
    }

    internal override bool Evaluate(ZipEntry entry)
    {
      bool flag = this.ObjectType == 'D' ? entry.IsDirectory : !entry.IsDirectory;
      if (this.Operator != ComparisonOperator.EqualTo)
        flag = !flag;
      return flag;
    }
  }
}
