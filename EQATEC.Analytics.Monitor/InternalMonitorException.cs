// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.InternalMonitorException
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class InternalMonitorException : Exception
  {
    public InternalMonitorException()
    {
    }

    public InternalMonitorException(string message)
      : base(message)
    {
    }

    public InternalMonitorException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
