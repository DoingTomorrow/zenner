// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Introspection.ExceptionFormatter
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Modules;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Infrastructure.Introspection
{
  public static class ExceptionFormatter
  {
    public static string ModulesWithNullOrEmptyNamesAreNotSupported()
    {
      return "Modules with null or empty names are not supported";
    }

    public static string TargetDoesNotHaveADefaultValue(ITarget target)
    {
      return string.Format("Target '{0}' at site '{1}' does not have a default value.", (object) target.Member, (object) target.Name);
    }

    public static string ModuleWithSameNameIsAlreadyLoaded(
      INinjectModule newModule,
      INinjectModule existingModule)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error loading module '{0}' of type {1}", (object) newModule.Name, (object) newModule.GetType().Format());
        stringWriter.WriteLine("Another module (of type {0}) with the same name has already been loaded", (object) existingModule.GetType().Format());
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that you have not accidentally loaded the same module twice.");
        stringWriter.WriteLine("  2) If you are using automatic module loading, ensure you have not manually loaded a module");
        stringWriter.WriteLine("     that may be found by the module loader.");
        return stringWriter.ToString();
      }
    }

    public static string NoModuleLoadedWithTheSpecifiedName(string name)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error unloading module '{0}': no such module has been loaded", (object) name);
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure you have previously loaded the module and the name is spelled correctly.");
        stringWriter.WriteLine("  2) Ensure you have not accidentally created more than one kernel.");
        return stringWriter.ToString();
      }
    }

    public static string CouldNotUniquelyResolveBinding(IRequest request)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error activating {0}", (object) request.Service.Format());
        stringWriter.WriteLine("More than one matching bindings are available.");
        stringWriter.WriteLine("Activation path:");
        stringWriter.WriteLine(request.FormatActivationPath());
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that you have defined a binding for {0} only once.", (object) request.Service.Format());
        return stringWriter.ToString();
      }
    }

    public static string CouldNotResolveBinding(IRequest request)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error activating {0}", (object) request.Service.Format());
        stringWriter.WriteLine("No matching bindings are available, and the type is not self-bindable.");
        stringWriter.WriteLine("Activation path:");
        stringWriter.WriteLine(request.FormatActivationPath());
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that you have defined a binding for {0}.", (object) request.Service.Format());
        stringWriter.WriteLine("  2) If the binding was defined in a module, ensure that the module has been loaded into the kernel.");
        stringWriter.WriteLine("  3) Ensure you have not accidentally created more than one kernel.");
        stringWriter.WriteLine("  4) If you are using constructor arguments, ensure that the parameter name matches the constructors parameter name.");
        stringWriter.WriteLine("  5) If you are using automatic module loading, ensure the search path and filters are correct.");
        return stringWriter.ToString();
      }
    }

    public static string CyclicalDependenciesDetected(IContext context)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error activating {0} using {1}", (object) context.Request.Service.Format(), (object) context.Binding.Format(context));
        stringWriter.WriteLine("A cyclical dependency was detected between the constructors of two services.");
        stringWriter.WriteLine();
        stringWriter.WriteLine("Activation path:");
        stringWriter.WriteLine(context.Request.FormatActivationPath());
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that you have not declared a dependency for {0} on any implementations of the service.", (object) context.Request.Service.Format());
        stringWriter.WriteLine("  2) Consider combining the services into a single one to remove the cycle.");
        stringWriter.WriteLine("  3) Use property injection instead of constructor injection, and implement IInitializable");
        stringWriter.WriteLine("     if you need initialization logic to be run after property values have been injected.");
        return stringWriter.ToString();
      }
    }

    public static string InvalidAttributeTypeUsedInBindingCondition(
      string serviceNames,
      string methodName,
      Type type)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error registering binding(s) for {0}", (object) serviceNames);
        stringWriter.WriteLine("The type {0} used in a call to {1}() is not a valid attribute.", (object) type.Format(), (object) methodName);
        stringWriter.WriteLine();
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that you have passed the correct type.");
        stringWriter.WriteLine("  2) If you have defined your own attribute type, ensure that it extends System.Attribute.");
        stringWriter.WriteLine("  3) To avoid problems with type-safety, use the generic version of the the method instead,");
        stringWriter.WriteLine("     such as {0}<SomeAttribute>().", (object) methodName);
        return stringWriter.ToString();
      }
    }

    public static string NoConstructorsAvailable(IContext context)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error activating {0} using {1}", (object) context.Request.Service.Format(), (object) context.Binding.Format(context));
        stringWriter.WriteLine("No constructor was available to create an instance of the implementation type.");
        stringWriter.WriteLine();
        stringWriter.WriteLine("Activation path:");
        stringWriter.WriteLine(context.Request.FormatActivationPath());
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that the implementation type has a public constructor.");
        stringWriter.WriteLine("  2) If you have implemented the Singleton pattern, use a binding with InSingletonScope() instead.");
        return stringWriter.ToString();
      }
    }

    public static string NoConstructorsAvailableForComponent(Type component, Type implementation)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error loading Ninject component {0}", (object) component.Format());
        stringWriter.WriteLine("No constructor was available to create an instance of the registered implementation type {0}.", (object) implementation.Format());
        stringWriter.WriteLine();
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that the implementation type has a public constructor.");
        return stringWriter.ToString();
      }
    }

    public static string NoSuchComponentRegistered(Type component)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error loading Ninject component {0}", (object) component.Format());
        stringWriter.WriteLine("No such component has been registered in the kernel's component container.");
        stringWriter.WriteLine();
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) If you have created a custom subclass for KernelBase, ensure that you have properly");
        stringWriter.WriteLine("     implemented the AddComponents() method.");
        stringWriter.WriteLine("  2) Ensure that you have not removed the component from the container via a call to RemoveAll().");
        stringWriter.WriteLine("  3) Ensure you have not accidentally created more than one kernel.");
        return stringWriter.ToString();
      }
    }

    public static string CouldNotResolvePropertyForValueInjection(
      IRequest request,
      string propertyName)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error activating {0}", (object) request.Service.Format());
        stringWriter.WriteLine("No matching property {0}.", (object) propertyName);
        stringWriter.WriteLine("Activation path:");
        stringWriter.WriteLine(request.FormatActivationPath());
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that you have the correct property name.");
        return stringWriter.ToString();
      }
    }

    public static string ProviderReturnedNull(IContext context)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        stringWriter.WriteLine("Error activating {0} using {1}", (object) context.Request.Service.Format(), (object) context.Binding.Format(context));
        stringWriter.WriteLine("Provider returned null.");
        stringWriter.WriteLine("Activation path:");
        stringWriter.WriteLine(context.Request.FormatActivationPath());
        stringWriter.WriteLine("Suggestions:");
        stringWriter.WriteLine("  1) Ensure that the provider handles creation requests properly.");
        return stringWriter.ToString();
      }
    }

    public static string ConstructorsAmbiguous(
      IContext context,
      IGrouping<int, ConstructorInjectionDirective> bestDirectives)
    {
      using (StringWriter sw = new StringWriter())
      {
        sw.WriteLine("Error activating {0} using {1}", (object) context.Request.Service.Format(), (object) context.Binding.Format(context));
        sw.WriteLine("Several constructors have the same priority. Please specify the constructor using ToConstructor syntax or add an Inject attribute.");
        sw.WriteLine();
        sw.WriteLine("Constructors:");
        foreach (ConstructorInjectionDirective bestDirective in (IEnumerable<ConstructorInjectionDirective>) bestDirectives)
          ExceptionFormatter.FormatConstructor(bestDirective.Constructor, sw);
        sw.WriteLine();
        sw.WriteLine("Activation path:");
        sw.WriteLine(context.Request.FormatActivationPath());
        sw.WriteLine("Suggestions:");
        sw.WriteLine("  1) Ensure that the implementation type has a public constructor.");
        sw.WriteLine("  2) If you have implemented the Singleton pattern, use a binding with InSingletonScope() instead.");
        return sw.ToString();
      }
    }

    private static void FormatConstructor(ConstructorInfo constructor, StringWriter sw)
    {
      foreach (Attribute customAttribute in constructor.GetCustomAttributes(false))
        ExceptionFormatter.FormatAttribute(sw, customAttribute);
      sw.Write(constructor.DeclaringType.Name);
      sw.Write("(");
      foreach (ParameterInfo parameter in constructor.GetParameters())
      {
        foreach (Attribute customAttribute in parameter.GetCustomAttributes(false))
          ExceptionFormatter.FormatAttribute(sw, customAttribute);
        sw.Write(parameter.ParameterType.Format());
        sw.Write(" ");
        sw.Write(parameter.Name);
      }
      sw.WriteLine(")");
    }

    private static void FormatAttribute(StringWriter sw, Attribute attribute)
    {
      sw.Write("[");
      string str1 = attribute.GetType().Format();
      string str2 = str1.EndsWith("Attribute") ? str1.Substring(0, str1.Length - 9) : str1;
      sw.Write(str2);
      sw.Write("]");
    }
  }
}
