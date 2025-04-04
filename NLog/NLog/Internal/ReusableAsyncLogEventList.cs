// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReusableAsyncLogEventList
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal
{
  internal class ReusableAsyncLogEventList(int capacity) : 
    ReusableObjectCreator<IList<AsyncLogEventInfo>>((IList<AsyncLogEventInfo>) new List<AsyncLogEventInfo>(capacity), (Action<IList<AsyncLogEventInfo>>) (l => l.Clear()))
  {
  }
}
