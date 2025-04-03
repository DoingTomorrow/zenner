// Decompiled with JetBrains decompiler
// Type: HandlerLib.DescendingComparer
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections;

#nullable disable
namespace HandlerLib
{
  internal class DescendingComparer : IComparer
  {
    public int Compare(object x, object y)
    {
      try
      {
        return Convert.ToInt32(x).CompareTo(Convert.ToInt32(y)) * -1;
      }
      catch
      {
        return x.ToString().CompareTo(y.ToString());
      }
    }
  }
}
