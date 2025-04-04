// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.QPPARAMINFO
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SqlServerCe
{
  [StructLayout(LayoutKind.Sequential)]
  internal class QPPARAMINFO
  {
    public IntPtr pwszParam = IntPtr.Zero;
    public uint cchMax;
    public uint iOrdinal;
    public QPPARAMTYPE paramType;
    public SETYPE type;
    public uint ulSize;
    public byte bPrecision;
    public byte bScale;
    public short padding;
    public int fIsTyped;
  }
}
