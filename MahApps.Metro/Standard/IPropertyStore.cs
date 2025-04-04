// Decompiled with JetBrains decompiler
// Type: Standard.IPropertyStore
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
  [ComImport]
  internal interface IPropertyStore
  {
    uint GetCount();

    PKEY GetAt(uint iProp);

    void GetValue([In] ref PKEY pkey, [In, Out] PROPVARIANT pv);

    void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

    void Commit();
  }
}
