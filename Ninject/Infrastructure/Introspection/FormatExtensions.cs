// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Introspection.FormatExtensions
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Planning.Bindings;
using Ninject.Planning.Targets;
using System;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace Ninject.Infrastructure.Introspection
{
  public static class FormatExtensions
  {
    public static string FormatActivationPath(this IRequest request)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        for (IRequest request1 = request; request1 != null; request1 = request1.ParentRequest)
          stringWriter.WriteLine("{0,3}) {1}", (object) (request1.Depth + 1), (object) request1.Format());
        return stringWriter.ToString();
      }
    }

    public static string Format(this IBinding binding, IContext context)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        if (binding.Condition != null)
          stringWriter.Write("conditional ");
        if (binding.IsImplicit)
          stringWriter.Write("implicit ");
        IProvider provider = binding.GetProvider(context);
        switch (binding.Target)
        {
          case BindingTarget.Self:
            stringWriter.Write("self-binding of {0}", (object) binding.Service.Format());
            break;
          case BindingTarget.Type:
            stringWriter.Write("binding from {0} to {1}", (object) binding.Service.Format(), (object) provider.Type.Format());
            break;
          case BindingTarget.Provider:
            stringWriter.Write("provider binding from {0} to {1} (via {2})", (object) binding.Service.Format(), (object) provider.Type.Format(), (object) provider.GetType().Format());
            break;
          case BindingTarget.Method:
            stringWriter.Write("binding from {0} to method", (object) binding.Service.Format());
            break;
          case BindingTarget.Constant:
            stringWriter.Write("binding from {0} to constant value", (object) binding.Service.Format());
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        return stringWriter.ToString();
      }
    }

    public static string Format(this IRequest request)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        if (request.Target == null)
          stringWriter.Write("Request for {0}", (object) request.Service.Format());
        else
          stringWriter.Write("Injection of dependency {0} into {1}", (object) request.Service.Format(), (object) request.Target.Format());
        return stringWriter.ToString();
      }
    }

    public static string Format(this ITarget target)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        switch (target.Member.MemberType)
        {
          case MemberTypes.Constructor:
            stringWriter.Write("parameter {0} of constructor", (object) target.Name);
            break;
          case MemberTypes.Method:
            stringWriter.Write("parameter {0} of method {1}", (object) target.Name, (object) target.Member.Name);
            break;
          case MemberTypes.Property:
            stringWriter.Write("property {0}", (object) target.Name);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        stringWriter.Write(" of type {0}", (object) target.Member.ReflectedType.Format());
        return stringWriter.ToString();
      }
    }

    public static string Format(this Type type)
    {
      if (type.IsGenericType)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(type.Name.Substring(0, type.Name.LastIndexOf('`')));
        stringBuilder.Append("{");
        foreach (Type genericArgument in type.GetGenericArguments())
        {
          stringBuilder.Append(genericArgument.Format());
          stringBuilder.Append(", ");
        }
        stringBuilder.Remove(stringBuilder.Length - 2, 2);
        stringBuilder.Append("}");
        return stringBuilder.ToString();
      }
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Boolean:
          return "bool";
        case TypeCode.Char:
          return "char";
        case TypeCode.SByte:
          return "sbyte";
        case TypeCode.Byte:
          return "byte";
        case TypeCode.Int16:
          return "short";
        case TypeCode.UInt16:
          return "ushort";
        case TypeCode.Int32:
          return "int";
        case TypeCode.UInt32:
          return "uint";
        case TypeCode.Int64:
          return "long";
        case TypeCode.UInt64:
          return "ulong";
        case TypeCode.Single:
          return "float";
        case TypeCode.Double:
          return "double";
        case TypeCode.Decimal:
          return "decimal";
        case TypeCode.DateTime:
          return "DateTime";
        case TypeCode.String:
          return "string";
        default:
          return type.Name;
      }
    }
  }
}
