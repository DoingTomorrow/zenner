// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.CollectionContainsRuntimeHqlGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public class CollectionContainsRuntimeHqlGenerator : IRuntimeMethodHqlGenerator
  {
    private readonly IHqlGeneratorForMethod containsGenerator = (IHqlGeneratorForMethod) new CollectionContainsGenerator();

    public bool SupportsMethod(MethodInfo method)
    {
      return method != null && method.Name == "Contains" && method.IsMethodOf(typeof (ICollection<>));
    }

    public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method) => this.containsGenerator;
  }
}
