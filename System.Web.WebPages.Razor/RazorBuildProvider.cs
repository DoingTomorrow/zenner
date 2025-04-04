// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.RazorBuildProvider
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Web.Compilation;
using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;

#nullable disable
namespace System.Web.WebPages.Razor
{
  [BuildProviderAppliesTo(BuildProviderAppliesTo.Web | BuildProviderAppliesTo.Code)]
  public class RazorBuildProvider : BuildProvider
  {
    private static bool? _isFullTrust;
    private CodeCompileUnit _generatedCode;
    private WebPageRazorHost _host;
    private IList _virtualPathDependencies;
    private IAssemblyBuilder _assemblyBuilder;

    public static event EventHandler<CodeGenerationCompleteEventArgs> CodeGenerationCompleted;

    internal event EventHandler<CodeGenerationCompleteEventArgs> CodeGenerationCompletedInternal
    {
      add => this._codeGenerationCompletedInternal += value;
      remove => this._codeGenerationCompletedInternal -= value;
    }

    public static event EventHandler CodeGenerationStarted;

    internal event EventHandler CodeGenerationStartedInternal
    {
      add => this._codeGenerationStartedInternal += value;
      remove => this._codeGenerationStartedInternal -= value;
    }

    public static event EventHandler<CompilingPathEventArgs> CompilingPath;

    private event EventHandler<CodeGenerationCompleteEventArgs> _codeGenerationCompletedInternal;

    private event EventHandler _codeGenerationStartedInternal;

    internal WebPageRazorHost Host
    {
      get
      {
        if (this._host == null)
          this._host = this.CreateHost();
        return this._host;
      }
      set => this._host = value;
    }

    public override ICollection VirtualPathDependencies
    {
      get
      {
        return this._virtualPathDependencies != null ? (ICollection) ArrayList.ReadOnly(this._virtualPathDependencies) : base.VirtualPathDependencies;
      }
    }

    public new string VirtualPath => base.VirtualPath;

    public AssemblyBuilder AssemblyBuilder
    {
      get
      {
        return this._assemblyBuilder is AssemblyBuilderWrapper assemblyBuilder ? assemblyBuilder.InnerBuilder : (AssemblyBuilder) null;
      }
    }

    internal IAssemblyBuilder AssemblyBuilderInternal => this._assemblyBuilder;

    internal CodeCompileUnit GeneratedCode
    {
      get
      {
        this.EnsureGeneratedCode();
        return this._generatedCode;
      }
      set => this._generatedCode = value;
    }

    public override CompilerType CodeCompilerType
    {
      get
      {
        this.EnsureGeneratedCode();
        CompilerType compilerTypeForLanguage = this.GetDefaultCompilerTypeForLanguage(((RazorEngineHost) this.Host).CodeLanguage.LanguageName);
        bool? isFullTrust = RazorBuildProvider._isFullTrust;
        if ((isFullTrust.GetValueOrDefault() ? 1 : (!isFullTrust.HasValue ? 1 : 0)) != 0)
        {
          if (this.Host.DefaultDebugCompilation)
          {
            try
            {
              RazorBuildProvider.SetIncludeDebugInfoFlag(compilerTypeForLanguage);
              RazorBuildProvider._isFullTrust = new bool?(true);
            }
            catch (SecurityException ex)
            {
              RazorBuildProvider._isFullTrust = new bool?(false);
            }
          }
        }
        return compilerTypeForLanguage;
      }
    }

    public void AddVirtualPathDependency(string dependency)
    {
      if (this._virtualPathDependencies == null)
        this._virtualPathDependencies = (IList) new ArrayList(base.VirtualPathDependencies);
      this._virtualPathDependencies.Add((object) dependency);
    }

    public override Type GetGeneratedType(CompilerResults results)
    {
      return results.CompiledAssembly.GetType(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}.{1}", new object[2]
      {
        (object) this.Host.DefaultNamespace,
        (object) ((RazorEngineHost) this.Host).DefaultClassName
      }));
    }

    public override void GenerateCode(AssemblyBuilder assemblyBuilder)
    {
      this.GenerateCodeCore((IAssemblyBuilder) new AssemblyBuilderWrapper(assemblyBuilder));
    }

    internal virtual void GenerateCodeCore(IAssemblyBuilder assemblyBuilder)
    {
      this.OnCodeGenerationStarted(assemblyBuilder);
      assemblyBuilder.AddCodeCompileUnit((BuildProvider) this, this.GeneratedCode);
      assemblyBuilder.GenerateTypeFactory(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}.{1}", new object[2]
      {
        (object) this.Host.DefaultNamespace,
        (object) ((RazorEngineHost) this.Host).DefaultClassName
      }));
    }

    protected internal virtual TextReader InternalOpenReader() => this.OpenReader();

    protected internal virtual WebPageRazorHost CreateHost()
    {
      CompilingPathEventArgs args = new CompilingPathEventArgs(this.VirtualPath, this.GetHostFromConfig());
      this.OnBeforeCompilePath(args);
      return args.Host;
    }

    protected internal virtual WebPageRazorHost GetHostFromConfig()
    {
      return WebRazorHostFactory.CreateHostFromConfig(this.VirtualPath);
    }

    protected virtual void OnBeforeCompilePath(CompilingPathEventArgs args)
    {
      EventHandler<CompilingPathEventArgs> compilingPath = RazorBuildProvider.CompilingPath;
      if (compilingPath == null)
        return;
      compilingPath((object) this, args);
    }

    private void OnCodeGenerationStarted(IAssemblyBuilder assemblyBuilder)
    {
      this._assemblyBuilder = assemblyBuilder;
      EventHandler eventHandler = this._codeGenerationStartedInternal ?? RazorBuildProvider.CodeGenerationStarted;
      if (eventHandler == null)
        return;
      eventHandler((object) this, (EventArgs) null);
    }

    private void OnCodeGenerationCompleted(CodeCompileUnit generatedCode)
    {
      EventHandler<CodeGenerationCompleteEventArgs> eventHandler = this._codeGenerationCompletedInternal ?? RazorBuildProvider.CodeGenerationCompleted;
      if (eventHandler == null)
        return;
      eventHandler((object) this, new CodeGenerationCompleteEventArgs(this.Host.VirtualPath, this.Host.PhysicalPath, generatedCode));
    }

    private void EnsureGeneratedCode()
    {
      if (this._generatedCode != null)
        return;
      RazorTemplateEngine razorTemplateEngine1 = new RazorTemplateEngine((RazorEngineHost) this.Host);
      GeneratorResults generatorResults = (GeneratorResults) null;
      using (TextReader textReader1 = this.InternalOpenReader())
      {
        RazorTemplateEngine razorTemplateEngine2 = razorTemplateEngine1;
        string str1 = (string) null;
        string str2 = (string) null;
        string physicalPath = this.Host.PhysicalPath;
        TextReader textReader2 = textReader1;
        string str3 = str1;
        string str4 = str2;
        string str5 = physicalPath;
        generatorResults = razorTemplateEngine2.GenerateCode(textReader2, str3, str4, str5);
      }
      this._generatedCode = ((ParserResults) generatorResults).Success ? generatorResults.GeneratedCode : throw RazorBuildProvider.CreateExceptionFromParserError(((ParserResults) generatorResults).ParserErrors.Last<RazorError>(), this.VirtualPath);
      this.OnCodeGenerationCompleted(this._generatedCode);
    }

    private static HttpParseException CreateExceptionFromParserError(
      RazorError error,
      string virtualPath)
    {
      string message = error.Message + Environment.NewLine;
      string virtualPath1 = virtualPath;
      SourceLocation location = error.Location;
      int line = ((SourceLocation) ref location).LineIndex + 1;
      return new HttpParseException(message, (Exception) null, virtualPath1, (string) null, line);
    }

    private static void SetIncludeDebugInfoFlag(CompilerType compilerType)
    {
      compilerType.CompilerParameters.IncludeDebugInformation = true;
    }
  }
}
