// Decompiled with JetBrains decompiler
// Type: NLog.MappedDiagnosticsContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog
{
  public static class MappedDiagnosticsContext
  {
    private static readonly object dataSlot = ThreadLocalStorageHelper.AllocateDataSlot();
    private static readonly IDictionary<string, object> EmptyDefaultDictionary = (IDictionary<string, object>) new SortHelpers.ReadOnlySingleBucketDictionary<string, object>();

    private static IDictionary<string, object> GetThreadDictionary(bool create = true)
    {
      Dictionary<string, object> dataForSlot = ThreadLocalStorageHelper.GetDataForSlot<Dictionary<string, object>>(MappedDiagnosticsContext.dataSlot, create);
      return dataForSlot == null && !create ? MappedDiagnosticsContext.EmptyDefaultDictionary : (IDictionary<string, object>) dataForSlot;
    }

    public static IDisposable SetScoped(string item, string value)
    {
      MappedDiagnosticsContext.Set(item, value);
      return (IDisposable) new MappedDiagnosticsContext.ItemRemover(item);
    }

    public static IDisposable SetScoped(string item, object value)
    {
      MappedDiagnosticsContext.Set(item, value);
      return (IDisposable) new MappedDiagnosticsContext.ItemRemover(item);
    }

    public static void Set(string item, string value)
    {
      MappedDiagnosticsContext.GetThreadDictionary()[item] = (object) value;
    }

    public static void Set(string item, object value)
    {
      MappedDiagnosticsContext.GetThreadDictionary()[item] = value;
    }

    public static string Get(string item)
    {
      return MappedDiagnosticsContext.Get(item, (IFormatProvider) null);
    }

    public static string Get(string item, IFormatProvider formatProvider)
    {
      return FormatHelper.ConvertToString(MappedDiagnosticsContext.GetObject(item), formatProvider);
    }

    public static object GetObject(string item)
    {
      object obj;
      if (!MappedDiagnosticsContext.GetThreadDictionary(false).TryGetValue(item, out obj))
        obj = (object) null;
      return obj;
    }

    public static ICollection<string> GetNames()
    {
      return MappedDiagnosticsContext.GetThreadDictionary(false).Keys;
    }

    public static bool Contains(string item)
    {
      return MappedDiagnosticsContext.GetThreadDictionary(false).ContainsKey(item);
    }

    public static void Remove(string item)
    {
      MappedDiagnosticsContext.GetThreadDictionary().Remove(item);
    }

    public static void Clear() => MappedDiagnosticsContext.GetThreadDictionary().Clear();

    private class ItemRemover : IDisposable
    {
      private readonly string _item;
      private bool _disposed;

      public ItemRemover(string item) => this._item = item;

      public void Dispose()
      {
        if (this._disposed)
          return;
        this._disposed = true;
        MappedDiagnosticsContext.Remove(this._item);
      }
    }
  }
}
