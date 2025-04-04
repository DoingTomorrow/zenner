﻿// Decompiled with JetBrains decompiler
// Type: Ionic.TimeCriterion
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
  internal class TimeCriterion : SelectionCriterion
  {
    internal ComparisonOperator Operator;
    internal WhichTime Which;
    internal DateTime Time;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Which.ToString()).Append(" ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.Time.ToString("yyyy-MM-dd-HH:mm:ss"));
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(string filename)
    {
      DateTime universalTime;
      switch (this.Which)
      {
        case WhichTime.atime:
          universalTime = File.GetLastAccessTime(filename).ToUniversalTime();
          break;
        case WhichTime.mtime:
          universalTime = File.GetLastWriteTime(filename).ToUniversalTime();
          break;
        case WhichTime.ctime:
          universalTime = File.GetCreationTime(filename).ToUniversalTime();
          break;
        default:
          throw new ArgumentException("Operator");
      }
      return this._Evaluate(universalTime);
    }

    private bool _Evaluate(DateTime x)
    {
      switch (this.Operator)
      {
        case ComparisonOperator.GreaterThan:
          return x > this.Time;
        case ComparisonOperator.GreaterThanOrEqualTo:
          return x >= this.Time;
        case ComparisonOperator.LesserThan:
          return x < this.Time;
        case ComparisonOperator.LesserThanOrEqualTo:
          return x <= this.Time;
        case ComparisonOperator.EqualTo:
          return x == this.Time;
        case ComparisonOperator.NotEqualTo:
          return x != this.Time;
        default:
          throw new ArgumentException("Operator");
      }
    }

    internal override bool Evaluate(ZipEntry entry)
    {
      DateTime x;
      switch (this.Which)
      {
        case WhichTime.atime:
          x = entry.AccessedTime;
          break;
        case WhichTime.mtime:
          x = entry.ModifiedTime;
          break;
        case WhichTime.ctime:
          x = entry.CreationTime;
          break;
        default:
          throw new ArgumentException("??time");
      }
      return this._Evaluate(x);
    }
  }
}
