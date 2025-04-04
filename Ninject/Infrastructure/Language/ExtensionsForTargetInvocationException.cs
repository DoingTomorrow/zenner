// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Language.ExtensionsForTargetInvocationException
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Reflection;

#nullable disable
namespace Ninject.Infrastructure.Language
{
  internal static class ExtensionsForTargetInvocationException
  {
    public static void RethrowInnerException(this TargetInvocationException exception)
    {
      Exception innerException = exception.InnerException;
      typeof (Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) innerException, (object) innerException.StackTrace);
      throw innerException;
    }
  }
}
