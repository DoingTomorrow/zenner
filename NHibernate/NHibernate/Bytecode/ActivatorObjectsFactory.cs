// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.ActivatorObjectsFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Bytecode
{
  public class ActivatorObjectsFactory : IObjectsFactory
  {
    public object CreateInstance(Type type) => Activator.CreateInstance(type);

    public object CreateInstance(Type type, bool nonPublic)
    {
      return Activator.CreateInstance(type, nonPublic);
    }

    public object CreateInstance(Type type, params object[] ctorArgs)
    {
      return Activator.CreateInstance(type, ctorArgs);
    }
  }
}
