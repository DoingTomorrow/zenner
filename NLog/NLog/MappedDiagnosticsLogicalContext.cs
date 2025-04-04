// Decompiled with JetBrains decompiler
// Type: NLog.MappedDiagnosticsLogicalContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading;

#nullable disable
namespace NLog
{
  public static class MappedDiagnosticsLogicalContext
  {
    private const string LogicalThreadDictionaryKey = "NLog.AsyncableMappedDiagnosticsContext";
    private static readonly IDictionary<string, object> EmptyDefaultDictionary = (IDictionary<string, object>) new SortHelpers.ReadOnlySingleBucketDictionary<string, object>();

    private static IDictionary<string, object> GetLogicalThreadDictionary(bool clone = false)
    {
      IDictionary<string, object> threadDictionary = MappedDiagnosticsLogicalContext.GetThreadLocal();
      if (threadDictionary == null)
      {
        if (!clone)
          return MappedDiagnosticsLogicalContext.EmptyDefaultDictionary;
        threadDictionary = (IDictionary<string, object>) new Dictionary<string, object>();
        MappedDiagnosticsLogicalContext.SetThreadLocal(threadDictionary);
      }
      else if (clone)
      {
        threadDictionary = (IDictionary<string, object>) new Dictionary<string, object>(threadDictionary);
        MappedDiagnosticsLogicalContext.SetThreadLocal(threadDictionary);
      }
      return threadDictionary;
    }

    public static string Get(string item)
    {
      return MappedDiagnosticsLogicalContext.Get(item, (IFormatProvider) null);
    }

    public static string Get(string item, IFormatProvider formatProvider)
    {
      return FormatHelper.ConvertToString(MappedDiagnosticsLogicalContext.GetObject(item), formatProvider);
    }

    public static object GetObject(string item)
    {
      object obj;
      MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary().TryGetValue(item, out obj);
      return obj;
    }

    public static IDisposable SetScoped(string item, string value)
    {
      MappedDiagnosticsLogicalContext.Set(item, value);
      return (IDisposable) new MappedDiagnosticsLogicalContext.ItemRemover(item);
    }

    public static IDisposable SetScoped(string item, object value)
    {
      MappedDiagnosticsLogicalContext.Set(item, value);
      return (IDisposable) new MappedDiagnosticsLogicalContext.ItemRemover(item);
    }

    public static void Set(string item, string value)
    {
      MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true)[item] = (object) value;
    }

    public static void Set(string item, object value)
    {
      MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true)[item] = value;
    }

    public static ICollection<string> GetNames()
    {
      return MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary().Keys;
    }

    public static bool Contains(string item)
    {
      return MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary().ContainsKey(item);
    }

    public static void Remove(string item)
    {
      MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true).Remove(item);
    }

    public static void Clear() => MappedDiagnosticsLogicalContext.Clear(false);

    public static void Clear(bool free)
    {
      if (free)
        MappedDiagnosticsLogicalContext.SetThreadLocal((IDictionary<string, object>) null);
      else
        MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true).Clear();
    }

    private static void SetThreadLocal(IDictionary<string, object> newValue)
    {
      if (newValue == null)
        CallContext.FreeNamedDataSlot("NLog.AsyncableMappedDiagnosticsContext");
      else
        CallContext.LogicalSetData("NLog.AsyncableMappedDiagnosticsContext", (object) newValue);
    }

    private static IDictionary<string, object> GetThreadLocal()
    {
      return (IDictionary<string, object>) (CallContext.LogicalGetData("NLog.AsyncableMappedDiagnosticsContext") as Dictionary<string, object>);
    }

    private class ItemRemover : IDisposable
    {
      private readonly string _item;
      private int _disposed;

      public ItemRemover(string item) => this._item = item;

      public void Dispose()
      {
        if (Interlocked.Exchange(ref this._disposed, 1) != 0)
          return;
        MappedDiagnosticsLogicalContext.Remove(this._item);
      }
    }
  }
}
