// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.ILinqToHqlGeneratorsRegistry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public interface ILinqToHqlGeneratorsRegistry
  {
    bool TryGetGenerator(MethodInfo method, out IHqlGeneratorForMethod generator);

    bool TryGetGenerator(MemberInfo property, out IHqlGeneratorForProperty generator);

    void RegisterGenerator(MethodInfo method, IHqlGeneratorForMethod generator);

    void RegisterGenerator(MemberInfo property, IHqlGeneratorForProperty generator);

    void RegisterGenerator(IRuntimeMethodHqlGenerator generator);
  }
}
