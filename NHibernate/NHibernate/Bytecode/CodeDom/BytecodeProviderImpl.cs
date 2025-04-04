// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.CodeDom.BytecodeProviderImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Microsoft.CSharp;
using NHibernate.Properties;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

#nullable disable
namespace NHibernate.Bytecode.CodeDom
{
  public class BytecodeProviderImpl : AbstractBytecodeProvider
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (BytecodeProviderImpl));

    public override IReflectionOptimizer GetReflectionOptimizer(
      Type clazz,
      IGetter[] getters,
      ISetter[] setters)
    {
      if (!clazz.IsValueType)
        return new BytecodeProviderImpl.Generator(clazz, getters, setters).CreateReflectionOptimizer();
      BytecodeProviderImpl.log.Info((object) ("Disabling reflection optimizer for value type " + clazz.FullName));
      return (IReflectionOptimizer) null;
    }

    public class Generator
    {
      private const string classDef = "public class GetSetHelper_{0} : IReflectionOptimizer, IAccessOptimizer {{\n\t\t\t\t\tISetter[] setters;\n\t\t\t\t\tIGetter[] getters;\n\t\t\t\t\t\n\t\t\t\t\tpublic GetSetHelper_{0}(ISetter[] setters, IGetter[] getters) {{\n\t\t\t\t\t\tthis.setters = setters;\n\t\t\t\t\t\tthis.getters = getters;\n\t\t\t\t\t}}\n\n\t\t\t\t\tpublic IInstantiationOptimizer InstantiationOptimizer {{\n\t\t\t\t\t\tget {{ return null; }}\n\t\t\t\t\t}}\n\n\t\t\t\t\tpublic IAccessOptimizer AccessOptimizer {{\n\t\t\t\t\t\tget {{ return this; }}\n\t\t\t\t\t}}\n\t\t\t\t\t";
      private const string closeGetMethod = "  return ret;\n}\n";
      private const string closeSetMethod = "}\n";
      private const string header = "using System;\nusing NHibernate.Property;\nnamespace NHibernate.Bytecode.CodeDom {\n";
      private const string startGetMethod = "public object[] GetPropertyValues(object obj) {{\n  {0} t = ({0})obj;\n  object[] ret = new object[{1}];\n";
      private const string startSetMethod = "public void SetPropertyValues(object obj, object[] values) {{\n  {0} t = ({0})obj;\n";
      private readonly CompilerParameters cp = new CompilerParameters();
      private readonly IGetter[] getters;
      private readonly Type mappedClass;
      private readonly ISetter[] setters;

      public Generator(Type mappedClass, IGetter[] getters, ISetter[] setters)
      {
        this.mappedClass = mappedClass;
        this.getters = getters;
        this.setters = setters;
      }

      public IReflectionOptimizer CreateReflectionOptimizer()
      {
        try
        {
          this.InitCompiler();
          return this.Build(this.GenerateCode());
        }
        catch (Exception ex)
        {
          BytecodeProviderImpl.log.Info((object) ("Disabling reflection optimizer for class " + this.mappedClass.FullName));
          BytecodeProviderImpl.log.Debug((object) "CodeDOM compilation failed", ex);
          return (IReflectionOptimizer) null;
        }
      }

      private void InitCompiler()
      {
        if (BytecodeProviderImpl.log.IsDebugEnabled)
          BytecodeProviderImpl.log.Debug((object) ("Init compiler for class " + this.mappedClass.FullName));
        this.cp.GenerateInMemory = true;
        this.cp.TreatWarningsAsErrors = true;
        this.cp.CompilerOptions = "/optimize";
        this.AddAssembly(Assembly.GetExecutingAssembly().Location);
        Assembly assembly = this.mappedClass.Assembly;
        this.AddAssembly(assembly.Location);
        foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
          this.AddAssembly(Assembly.Load(referencedAssembly).Location);
      }

      private void AddAssembly(string name)
      {
        if (name.StartsWith("System.") || this.cp.ReferencedAssemblies.Contains(name))
          return;
        if (BytecodeProviderImpl.log.IsDebugEnabled)
          BytecodeProviderImpl.log.Debug((object) ("Adding referenced assembly " + name));
        this.cp.ReferencedAssemblies.Add(name);
      }

      private IReflectionOptimizer Build(string code)
      {
        CompilerResults compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(this.cp, code);
        if (compilerResults.Errors.HasErrors)
        {
          BytecodeProviderImpl.log.Debug((object) ("Compiled with error:\n" + code));
          foreach (CompilerError error in (CollectionBase) compilerResults.Errors)
            BytecodeProviderImpl.log.Debug((object) string.Format("Line:{0}, Column:{1} Message:{2}", (object) error.Line, (object) error.Column, (object) error.ErrorText));
          throw new InvalidOperationException(compilerResults.Errors[0].ErrorText);
        }
        if (BytecodeProviderImpl.log.IsDebugEnabled)
          BytecodeProviderImpl.log.Debug((object) ("Compiled ok:\n" + code));
        Assembly compiledAssembly = compilerResults.CompiledAssembly;
        Type[] types = compiledAssembly.GetTypes();
        return (IReflectionOptimizer) compiledAssembly.CreateInstance(types[0].FullName, false, BindingFlags.CreateInstance, (Binder) null, new object[2]
        {
          (object) this.setters,
          (object) this.getters
        }, (CultureInfo) null, (object[]) null);
      }

      private bool IsPublic(string propertyName)
      {
        return this.mappedClass.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public) != null;
      }

      private string GenerateCode()
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("using System;\nusing NHibernate.Property;\nnamespace NHibernate.Bytecode.CodeDom {\n");
        stringBuilder.AppendFormat("public class GetSetHelper_{0} : IReflectionOptimizer, IAccessOptimizer {{\n\t\t\t\t\tISetter[] setters;\n\t\t\t\t\tIGetter[] getters;\n\t\t\t\t\t\n\t\t\t\t\tpublic GetSetHelper_{0}(ISetter[] setters, IGetter[] getters) {{\n\t\t\t\t\t\tthis.setters = setters;\n\t\t\t\t\t\tthis.getters = getters;\n\t\t\t\t\t}}\n\n\t\t\t\t\tpublic IInstantiationOptimizer InstantiationOptimizer {{\n\t\t\t\t\t\tget {{ return null; }}\n\t\t\t\t\t}}\n\n\t\t\t\t\tpublic IAccessOptimizer AccessOptimizer {{\n\t\t\t\t\t\tget {{ return this; }}\n\t\t\t\t\t}}\n\t\t\t\t\t", (object) this.mappedClass.FullName.Replace('.', '_').Replace("+", "__"));
        stringBuilder.AppendFormat("public void SetPropertyValues(object obj, object[] values) {{\n  {0} t = ({0})obj;\n", (object) this.mappedClass.FullName.Replace('+', '.'));
        for (int index = 0; index < this.setters.Length; ++index)
        {
          ISetter setter = this.setters[index];
          if (setter is BasicPropertyAccessor.BasicSetter && this.IsPublic(setter.PropertyName))
          {
            Type returnType = this.getters[index].ReturnType;
            if (returnType.IsValueType)
              stringBuilder.AppendFormat("  t.{0} = values[{2}] == null ? new {1}() : ({1})values[{2}];\n", (object) setter.PropertyName, (object) returnType.FullName.Replace('+', '.'), (object) index);
            else
              stringBuilder.AppendFormat("  t.{0} = ({1})values[{2}];\n", (object) setter.PropertyName, (object) returnType.FullName.Replace('+', '.'), (object) index);
          }
          else
            stringBuilder.AppendFormat("  setters[{0}].Set(obj, values[{0}]);\n", (object) index);
        }
        stringBuilder.Append("}\n");
        stringBuilder.AppendFormat("public object[] GetPropertyValues(object obj) {{\n  {0} t = ({0})obj;\n  object[] ret = new object[{1}];\n", (object) this.mappedClass.FullName.Replace('+', '.'), (object) this.getters.Length);
        for (int index = 0; index < this.getters.Length; ++index)
        {
          IGetter getter = this.getters[index];
          if (getter is BasicPropertyAccessor.BasicGetter && this.IsPublic(getter.PropertyName))
            stringBuilder.AppendFormat("  ret[{0}] = t.{1};\n", (object) index, (object) getter.PropertyName);
          else
            stringBuilder.AppendFormat("  ret[{0}] = getters[{0}].Get(obj);\n", (object) index);
        }
        stringBuilder.Append("  return ret;\n}\n");
        stringBuilder.Append("}\n");
        stringBuilder.Append("}\n");
        return stringBuilder.ToString();
      }
    }
  }
}
