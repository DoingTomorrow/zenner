// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReusableStreamCreator
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.IO;

#nullable disable
namespace NLog.Internal
{
  internal class ReusableStreamCreator(int capacity) : 
    ReusableObjectCreator<MemoryStream>(new MemoryStream(capacity), (Action<MemoryStream>) (m =>
    {
      m.Position = 0L;
      m.SetLength(0L);
    })),
    IDisposable
  {
    void IDisposable.Dispose() => this._reusableObject.Dispose();
  }
}
