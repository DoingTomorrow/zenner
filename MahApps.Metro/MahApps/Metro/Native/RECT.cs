// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Native.RECT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Native
{
  [Serializable]
  public struct RECT
  {
    public int left;
    public int top;
    public int right;
    public int bottom;
    public static readonly RECT Empty;

    public int Width => Math.Abs(this.right - this.left);

    public int Height => this.bottom - this.top;

    public RECT(int left, int top, int right, int bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    public RECT(RECT rcSrc)
    {
      this.left = rcSrc.left;
      this.top = rcSrc.top;
      this.right = rcSrc.right;
      this.bottom = rcSrc.bottom;
    }

    public bool IsEmpty => this.left >= this.right || this.top >= this.bottom;

    public override string ToString()
    {
      if (this == RECT.Empty)
        return "RECT {Empty}";
      return "RECT { left : " + (object) this.left + " / top : " + (object) this.top + " / right : " + (object) this.right + " / bottom : " + (object) this.bottom + " }";
    }

    public override bool Equals(object obj) => obj is Rect && this == (RECT) obj;

    public override int GetHashCode()
    {
      return this.left.GetHashCode() + this.top.GetHashCode() + this.right.GetHashCode() + this.bottom.GetHashCode();
    }

    public static bool operator ==(RECT rect1, RECT rect2)
    {
      return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
    }

    public static bool operator !=(RECT rect1, RECT rect2) => !(rect1 == rect2);
  }
}
