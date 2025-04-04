// Decompiled with JetBrains decompiler
// Type: Standard.RefRECT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [StructLayout(LayoutKind.Sequential)]
  internal class RefRECT
  {
    private int _left;
    private int _top;
    private int _right;
    private int _bottom;

    public RefRECT(int left, int top, int right, int bottom)
    {
      this._left = left;
      this._top = top;
      this._right = right;
      this._bottom = bottom;
    }

    public int Width => this._right - this._left;

    public int Height => this._bottom - this._top;

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

    public void Offset(int dx, int dy)
    {
      this._left += dx;
      this._top += dy;
      this._right += dx;
      this._bottom += dy;
    }
  }
}
