// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.ISetter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Reflection;

#nullable disable
namespace NHibernate.Properties
{
  public interface ISetter
  {
    void Set(object target, object value);

    string PropertyName { get; }

    MethodInfo Method { get; }
  }
}
