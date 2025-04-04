// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.BTHNS_BLOB
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal abstract class BTHNS_BLOB : IDisposable
  {
    internal byte[] m_data;

    internal int Length => this.m_data.Length;

    internal byte[] ToByteArray() => this.m_data;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.m_data = (byte[]) null;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~BTHNS_BLOB() => this.Dispose(false);
  }
}
