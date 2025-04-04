// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Ensure
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;

#nullable disable
namespace Ninject.Infrastructure
{
  internal static class Ensure
  {
    public static void ArgumentNotNull(object argument, string name)
    {
      if (argument == null)
        throw new ArgumentNullException(name, "Cannot be null");
    }

    public static void ArgumentNotNullOrEmpty(string argument, string name)
    {
      if (string.IsNullOrEmpty(argument))
        throw new ArgumentException("Cannot be null or empty", name);
    }
  }
}
