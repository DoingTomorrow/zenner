// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.CacheControllerException
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class CacheControllerException : Exception
  {
    public CacheControllerException(string message)
      : base(message)
    {
    }

    public CacheControllerException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public CacheControllerException()
    {
    }

    internal static CacheControllerException CreateCacheBusyException()
    {
      return new CacheControllerException("Cannot complete WebRequest as another Refresh() operation is in progress.");
    }
  }
}
