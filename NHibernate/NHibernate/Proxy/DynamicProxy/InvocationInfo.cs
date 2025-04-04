// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.InvocationInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public class InvocationInfo
  {
    private readonly object[] args;
    private readonly object proxy;
    private readonly MethodInfo targetMethod;
    private readonly StackTrace trace;
    private readonly Type[] typeArgs;

    public InvocationInfo(
      object proxy,
      MethodInfo targetMethod,
      StackTrace trace,
      Type[] genericTypeArgs,
      object[] args)
    {
      this.proxy = proxy;
      this.targetMethod = targetMethod;
      this.typeArgs = genericTypeArgs;
      this.args = args;
      this.trace = trace;
    }

    public object Target => this.proxy;

    public MethodInfo TargetMethod => this.targetMethod;

    public StackTrace StackTrace => this.trace;

    public Type[] TypeArguments => this.typeArgs;

    public object[] Arguments => this.args;

    public void SetArgument(int position, object arg) => this.args[position] = arg;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("Target Method:{0,30:G}\n", (object) this.GetMethodName(this.targetMethod));
      stringBuilder.AppendLine("Arguments:");
      foreach (ParameterInfo parameter in this.targetMethod.GetParameters())
      {
        object obj = this.args[parameter.Position] ?? (object) "(null)";
        stringBuilder.AppendFormat("\t{0,10:G}: {1}\n", (object) parameter.Name, obj);
      }
      stringBuilder.AppendLine();
      return stringBuilder.ToString();
    }

    private string GetMethodName(MethodInfo method)
    {
      StringBuilder stringBuilder = new StringBuilder(512);
      stringBuilder.AppendFormat("{0}.{1}", (object) method.DeclaringType.Name, (object) method.Name);
      stringBuilder.Append("(");
      ParameterInfo[] parameters = method.GetParameters();
      int length = parameters.Length;
      int num = 0;
      foreach (ParameterInfo parameterInfo in parameters)
      {
        ++num;
        stringBuilder.AppendFormat("{0} {1}", (object) parameterInfo.ParameterType.Name, (object) parameterInfo.Name);
        if (num < length)
          stringBuilder.Append(", ");
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }
  }
}
