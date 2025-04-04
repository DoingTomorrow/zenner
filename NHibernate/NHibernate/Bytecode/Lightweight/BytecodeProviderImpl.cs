// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.Lightweight.BytecodeProviderImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using System;

#nullable disable
namespace NHibernate.Bytecode.Lightweight
{
  public class BytecodeProviderImpl : AbstractBytecodeProvider
  {
    public override IReflectionOptimizer GetReflectionOptimizer(
      Type mappedClass,
      IGetter[] getters,
      ISetter[] setters)
    {
      return (IReflectionOptimizer) new ReflectionOptimizer(mappedClass, getters, setters);
    }
  }
}
