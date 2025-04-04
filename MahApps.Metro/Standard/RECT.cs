// Decompiled with JetBrains decompiler
// Type: Standard.RECT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace Standard
{
  internal struct RECT
  {
    private int _left;
    private int _top;
    private int _right;
    private int _bottom;

    public void Offset(int dx, int dy)
    {
      this._left += dx;
      this._top += dy;
      this._right += dx;
      this._bottom += dy;
    }

    public int Left
    {
      get => this._left;
      set => this._left = value;
    }

    public int Right
    {
      get => this._right;
      set => this._right = value;
    }

    public int Top
    {
      get => this._top;
      set => this._top = value;
    }

    public int Bottom
    {
      get => this._bottom;
      set => this._bottom = value;
    }

    public int Width => this._right - this._left;

    public int Height => this._bottom - this._top;

    public POINT Position
    {
      get => new POINT() { x = this._left, y = this._top };
    }

    public SIZE Size
    {
      get
      {
        return new SIZE()
        {
          cx = this.Width,
          cy = this.Height
        };
      }
    }

    public static RECT Union(RECT rect1, RECT rect2)
    {
      return new RECT()
      {
        Left = Math.Min(rect1.Left, rect2.Left),
        Top = Math.Min(rect1.Top, rect2.Top),
        Right = Math.Max(rect1.Right, rect2.Right),
        Bottom = Math.Max(rect1.Bottom, rect2.Bottom)
      };
    }

    public override bool Equals(object obj)
    {
      try
      {
        RECT rect = (RECT) obj;
        return rect._bottom == this._bottom && rect._left == this._left && rect._right == this._right && rect._top == this._top;
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return (this._left << 16 | Utility.LOWORD(this._right)) ^ (this._top << 16 | Utility.LOWORD(this._bottom));
    }
  }
}
