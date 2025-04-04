// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SETABLEINFO
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SqlServerCe
{
  [StructLayout(LayoutKind.Sequential)]
  internal class SETABLEINFO
  {
    public IntPtr pwszTable = IntPtr.Zero;
    public uint cchMax;
    public bool fIsSystem;
    public bool fReadOnly;
    public ulong uhVersion;
    public bool fTemporary;
    public bool fOrdered;
    public long lNextIdentity;
    public bool fIdentityOverflow;
    public ushort wTracking;
    public int lTableNick;
    public uint dwCedbType;
    public int hBookmark;
    public uint cPages;
    public uint cLvPages;
    public uint dwGrantedOps;
    public bool fHasDefaults;
    public bool fCompressed;
    public uint cStatMods;
  }
}
