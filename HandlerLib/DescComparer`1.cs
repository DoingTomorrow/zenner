// Decompiled with JetBrains decompiler
// Type: HandlerLib.DescComparer`1
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Collections.Generic;

#nullable disable
namespace HandlerLib
{
  internal class DescComparer<T> : IComparer<T>
  {
    public int Compare(T x, T y) => Comparer<T>.Default.Compare(y, x);
  }
}
