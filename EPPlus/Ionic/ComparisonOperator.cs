// Decompiled with JetBrains decompiler
// Type: Ionic.ComparisonOperator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.ComponentModel;

#nullable disable
namespace Ionic
{
  internal enum ComparisonOperator
  {
    [Description(">")] GreaterThan,
    [Description(">=")] GreaterThanOrEqualTo,
    [Description("<")] LesserThan,
    [Description("<=")] LesserThanOrEqualTo,
    [Description("=")] EqualTo,
    [Description("!=")] NotEqualTo,
  }
}
