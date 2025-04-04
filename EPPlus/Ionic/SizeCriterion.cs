// Decompiled with JetBrains decompiler
// Type: Ionic.SizeCriterion
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
  internal class SizeCriterion : SelectionCriterion
  {
    internal ComparisonOperator Operator;
    internal long Size;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("size ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.Size.ToString());
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(string filename)
    {
      return this._Evaluate(new FileInfo(filename).Length);
    }

    private bool _Evaluate(long Length)
    {
      switch (this.Operator)
      {
        case ComparisonOperator.GreaterThan:
          return Length > this.Size;
        case ComparisonOperator.GreaterThanOrEqualTo:
          return Length >= this.Size;
        case ComparisonOperator.LesserThan:
          return Length < this.Size;
        case ComparisonOperator.LesserThanOrEqualTo:
          return Length <= this.Size;
        case ComparisonOperator.EqualTo:
          return Length == this.Size;
        case ComparisonOperator.NotEqualTo:
          return Length != this.Size;
        default:
          throw new ArgumentException("Operator");
      }
    }

    internal override bool Evaluate(ZipEntry entry) => this._Evaluate(entry.UncompressedSize);
  }
}
