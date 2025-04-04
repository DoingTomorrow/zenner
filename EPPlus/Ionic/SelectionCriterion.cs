// Decompiled with JetBrains decompiler
// Type: Ionic.SelectionCriterion
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using System.Diagnostics;

#nullable disable
namespace Ionic
{
  internal abstract class SelectionCriterion
  {
    internal virtual bool Verbose { get; set; }

    internal abstract bool Evaluate(string filename);

    [Conditional("SelectorTrace")]
    protected static void CriterionTrace(string format, params object[] args)
    {
    }

    internal abstract bool Evaluate(ZipEntry entry);
  }
}
