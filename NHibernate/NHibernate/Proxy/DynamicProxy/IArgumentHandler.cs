// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.IArgumentHandler
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public interface IArgumentHandler
  {
    void PushArguments(ParameterInfo[] parameters, ILGenerator IL, bool isStatic);
  }
}
