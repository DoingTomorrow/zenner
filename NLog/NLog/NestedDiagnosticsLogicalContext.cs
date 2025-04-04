﻿// Decompiled with JetBrains decompiler
// Type: NLog.NestedDiagnosticsLogicalContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;

#nullable disable
namespace NLog
{
  public static class NestedDiagnosticsLogicalContext
  {
    private const string NestedDiagnosticsContextKey = "NLog.AsyncableNestedDiagnosticsContext";

    public static IDisposable Push<T>(T value)
    {
      NestedDiagnosticsLogicalContext.NestedContext<T> newValue = new NestedDiagnosticsLogicalContext.NestedContext<T>(NestedDiagnosticsLogicalContext.GetThreadLocal(), value);
      NestedDiagnosticsLogicalContext.SetThreadLocal((NestedDiagnosticsLogicalContext.INestedContext) newValue);
      return (IDisposable) newValue;
    }

    public static object Pop() => NestedDiagnosticsLogicalContext.PopObject();

    public static string Pop(IFormatProvider formatProvider)
    {
      return FormatHelper.ConvertToString(NestedDiagnosticsLogicalContext.PopObject() ?? (object) string.Empty, formatProvider);
    }

    public static object PopObject()
    {
      NestedDiagnosticsLogicalContext.INestedContext threadLocal = NestedDiagnosticsLogicalContext.GetThreadLocal();
      if (threadLocal != null)
        NestedDiagnosticsLogicalContext.SetThreadLocal(threadLocal.Parent);
      return threadLocal?.Value;
    }

    public static object PeekObject() => NestedDiagnosticsLogicalContext.PeekContext(false)?.Value;

    internal static DateTime PeekTopScopeBeginTime()
    {
      NestedDiagnosticsLogicalContext.INestedContext nestedContext = NestedDiagnosticsLogicalContext.PeekContext(false);
      return new DateTime(nestedContext != null ? nestedContext.CreatedTimeUtcTicks : DateTime.MinValue.Ticks, DateTimeKind.Utc);
    }

    internal static DateTime PeekBottomScopeBeginTime()
    {
      NestedDiagnosticsLogicalContext.INestedContext nestedContext = NestedDiagnosticsLogicalContext.PeekContext(true);
      return new DateTime(nestedContext != null ? nestedContext.CreatedTimeUtcTicks : DateTime.MinValue.Ticks, DateTimeKind.Utc);
    }

    private static NestedDiagnosticsLogicalContext.INestedContext PeekContext(bool bottomScope)
    {
      NestedDiagnosticsLogicalContext.INestedContext nestedContext = NestedDiagnosticsLogicalContext.GetThreadLocal();
      if (nestedContext == null)
        return (NestedDiagnosticsLogicalContext.INestedContext) null;
      if (bottomScope)
      {
        while (nestedContext.Parent != null)
          nestedContext = nestedContext.Parent;
      }
      return nestedContext;
    }

    public static void Clear()
    {
      NestedDiagnosticsLogicalContext.SetThreadLocal((NestedDiagnosticsLogicalContext.INestedContext) null);
    }

    public static string[] GetAllMessages()
    {
      return NestedDiagnosticsLogicalContext.GetAllMessages((IFormatProvider) null);
    }

    public static string[] GetAllMessages(IFormatProvider formatProvider)
    {
      return ((IEnumerable<object>) NestedDiagnosticsLogicalContext.GetAllObjects()).Select<object, string>((Func<object, string>) (o => FormatHelper.ConvertToString(o, formatProvider))).ToArray<string>();
    }

    public static object[] GetAllObjects()
    {
      NestedDiagnosticsLogicalContext.INestedContext nestedContext = NestedDiagnosticsLogicalContext.GetThreadLocal();
      if (nestedContext == null)
        return ArrayHelper.Empty<object>();
      int num = 0;
      object[] allObjects = new object[nestedContext.FrameLevel];
      for (; nestedContext != null; nestedContext = nestedContext.Parent)
        allObjects[num++] = nestedContext.Value;
      return allObjects;
    }

    private static void SetThreadLocal(
      NestedDiagnosticsLogicalContext.INestedContext newValue)
    {
      if (newValue == null)
        CallContext.FreeNamedDataSlot("NLog.AsyncableNestedDiagnosticsContext");
      else
        CallContext.LogicalSetData("NLog.AsyncableNestedDiagnosticsContext", (object) newValue);
    }

    private static NestedDiagnosticsLogicalContext.INestedContext GetThreadLocal()
    {
      return CallContext.LogicalGetData("NLog.AsyncableNestedDiagnosticsContext") as NestedDiagnosticsLogicalContext.INestedContext;
    }

    private interface INestedContext : IDisposable
    {
      NestedDiagnosticsLogicalContext.INestedContext Parent { get; }

      int FrameLevel { get; }

      object Value { get; }

      long CreatedTimeUtcTicks { get; }
    }

    [Serializable]
    private class NestedContext<T> : NestedDiagnosticsLogicalContext.INestedContext, IDisposable
    {
      private int _disposed;

      public NestedDiagnosticsLogicalContext.INestedContext Parent { get; }

      public T Value { get; }

      object NestedDiagnosticsLogicalContext.INestedContext.Value => (object) this.Value;

      public long CreatedTimeUtcTicks { get; }

      public int FrameLevel { get; }

      public NestedContext(
        NestedDiagnosticsLogicalContext.INestedContext parent,
        T value)
      {
        this.Parent = parent;
        this.Value = value;
        this.CreatedTimeUtcTicks = DateTime.UtcNow.Ticks;
        this.FrameLevel = parent != null ? parent.FrameLevel + 1 : 1;
      }

      void IDisposable.Dispose()
      {
        if (Interlocked.Exchange(ref this._disposed, 1) == 1)
          return;
        NestedDiagnosticsLogicalContext.PopObject();
      }

      public override string ToString() => ((object) this.Value)?.ToString() ?? "null";
    }
  }
}
