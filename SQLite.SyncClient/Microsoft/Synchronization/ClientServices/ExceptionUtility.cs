// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.ExceptionUtility
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  internal static class ExceptionUtility
  {
    internal static bool IsFatal(Exception exception)
    {
      for (; exception != null; exception = exception.InnerException)
      {
        if (exception is OutOfMemoryException || exception is SEHException)
          return true;
      }
      return false;
    }
  }
}
