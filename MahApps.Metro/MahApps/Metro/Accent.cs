// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Accent
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Diagnostics;
using System.Windows;

#nullable disable
namespace MahApps.Metro
{
  [DebuggerDisplay("accent={Name}, res={Resources.Source}")]
  public class Accent
  {
    public ResourceDictionary Resources;

    public string Name { get; set; }

    public Accent()
    {
    }

    public Accent(string name, Uri resourceAddress)
    {
      if (name == null)
        throw new ArgumentException(nameof (name));
      if (resourceAddress == (Uri) null)
        throw new ArgumentNullException(nameof (resourceAddress));
      this.Name = name;
      this.Resources = new ResourceDictionary()
      {
        Source = resourceAddress
      };
    }
  }
}
