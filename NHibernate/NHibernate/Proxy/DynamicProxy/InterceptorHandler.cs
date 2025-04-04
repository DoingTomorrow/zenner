// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.InterceptorHandler
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public delegate object InterceptorHandler(
    object proxy,
    MethodInfo targetMethod,
    StackTrace trace,
    Type[] genericTypeArgs,
    object[] args);
}
