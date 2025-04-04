// Decompiled with JetBrains decompiler
// Type: NLog.LogFactory`1
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog
{
  public class LogFactory<T> : LogFactory where T : Logger
  {
    public T GetLogger(string name) => (T) this.GetLogger(name, typeof (T));

    [MethodImpl(MethodImplOptions.NoInlining)]
    public T GetCurrentClassLogger()
    {
      return this.GetLogger(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)));
    }
  }
}
