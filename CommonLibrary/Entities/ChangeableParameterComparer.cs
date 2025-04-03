// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ChangeableParameterComparer
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public class ChangeableParameterComparer : IComparer<ChangeableParameter>
  {
    public int Compare(ChangeableParameter x, ChangeableParameter y) => x.Key.CompareTo(y.Key);
  }
}
