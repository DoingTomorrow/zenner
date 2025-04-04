// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.GenericDictionaryRuntimeMethodHqlGeneratorBase`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public abstract class GenericDictionaryRuntimeMethodHqlGeneratorBase<TGenerator> : 
    IRuntimeMethodHqlGenerator
    where TGenerator : IHqlGeneratorForMethod, new()
  {
    private readonly IHqlGeneratorForMethod generator = (IHqlGeneratorForMethod) new TGenerator();

    protected abstract string MethodName { get; }

    public bool SupportsMethod(MethodInfo method)
    {
      return method != null && method.Name == this.MethodName && method.IsMethodOf(typeof (IDictionary<,>));
    }

    public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method) => this.generator;
  }
}
