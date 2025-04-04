// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.ToStringRuntimeMethodHqlGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public class ToStringRuntimeMethodHqlGenerator : IRuntimeMethodHqlGenerator
  {
    private readonly ToStringHqlGeneratorForMethod generator = new ToStringHqlGeneratorForMethod();

    public bool SupportsMethod(MethodInfo method)
    {
      return method != null && method.Name == "ToString" && method.GetBaseDefinition().DeclaringType == typeof (object);
    }

    public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method)
    {
      return (IHqlGeneratorForMethod) this.generator;
    }
  }
}
