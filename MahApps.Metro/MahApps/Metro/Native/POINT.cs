// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Native.POINT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace MahApps.Metro.Native
{
  [Serializable]
  public struct POINT(int x, int y)
  {
    private int _x = x;
    private int _y = y;

    public int X
    {
      get => this._x;
      set => this._x = value;
    }

    public int Y
    {
      get => this._y;
      set => this._y = value;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is POINT point))
        return base.Equals(obj);
      return point._x == this._x && point._y == this._y;
    }

    public override int GetHashCode() => this._x.GetHashCode() ^ this._y.GetHashCode();

    public static bool operator ==(POINT a, POINT b) => a._x == b._x && a._y == b._y;

    public static bool operator !=(POINT a, POINT b) => !(a == b);
  }
}
