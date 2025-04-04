// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.IAbstractComponentType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Reflection;

#nullable disable
namespace NHibernate.Type
{
  public interface IAbstractComponentType : IType, ICacheAssembler
  {
    IType[] Subtypes { get; }

    string[] PropertyNames { get; }

    bool[] PropertyNullability { get; }

    object[] GetPropertyValues(object component, ISessionImplementor session);

    object[] GetPropertyValues(object component, EntityMode entityMode);

    void SetPropertyValues(object component, object[] values, EntityMode entityMode);

    object GetPropertyValue(object component, int i, ISessionImplementor session);

    CascadeStyle GetCascadeStyle(int i);

    FetchMode GetFetchMode(int i);

    bool IsEmbedded { get; }

    bool IsMethodOf(MethodBase method);
  }
}
