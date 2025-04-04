// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.WebPageRazorHost
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using Microsoft.Internal.Web.Utils;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using System.Web.WebPages.Instrumentation;
using System.Web.WebPages.Razor.Resources;

#nullable disable
namespace System.Web.WebPages.Razor
{
  public class WebPageRazorHost : RazorEngineHost
  {
    internal const string PageClassNamePrefix = "_Page_";
    internal const string ApplicationInstancePropertyName = "ApplicationInstance";
    internal const string ContextPropertyName = "Context";
    internal const string DefineSectionMethodName = "DefineSection";
    internal const string WebDefaultNamespace = "ASP";
    internal const string WriteToMethodName = "WriteTo";
    internal const string WriteLiteralToMethodName = "WriteLiteralTo";
    internal const string BeginContextMethodName = "BeginContext";
    internal const string EndContextMethodName = "EndContext";
    internal const string ResolveUrlMethodName = "Href";
    private const string ApplicationStartFileName = "_AppStart";
    private const string PageStartFileName = "_PageStart";
    internal static readonly string FallbackApplicationTypeName = typeof (HttpApplication).FullName;
    internal static readonly string PageBaseClass = typeof (WebPage).FullName;
    internal static readonly string TemplateTypeName = typeof (HelperResult).FullName;
    private static ConcurrentDictionary<string, object> _importedNamespaces = new ConcurrentDictionary<string, object>();
    private readonly Dictionary<string, string> _specialFileBaseTypes = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private string _className;
    private RazorCodeLanguage _codeLanguage;
    private string _globalAsaxTypeName;
    private bool? _isSpecialPage;
    private string _physicalPath;
    private string _specialFileBaseClass;

    private WebPageRazorHost()
    {
      this.NamespaceImports.Add("System");
      this.NamespaceImports.Add("System.Collections.Generic");
      this.NamespaceImports.Add("System.IO");
      this.NamespaceImports.Add("System.Linq");
      this.NamespaceImports.Add("System.Net");
      this.NamespaceImports.Add("System.Web");
      this.NamespaceImports.Add("System.Web.Helpers");
      this.NamespaceImports.Add("System.Web.Security");
      this.NamespaceImports.Add("System.Web.UI");
      this.NamespaceImports.Add("System.Web.WebPages");
      this.NamespaceImports.Add("System.Web.WebPages.Html");
      this.RegisterSpecialFile("_AppStart", typeof (ApplicationStartPage));
      this.RegisterSpecialFile("_PageStart", typeof (StartPage));
      this.DefaultNamespace = "ASP";
      GeneratedClassContext generatedClassContext;
      // ISSUE: explicit constructor call
      ((GeneratedClassContext) ref generatedClassContext).\u002Ector(GeneratedClassContext.DefaultExecuteMethodName, GeneratedClassContext.DefaultWriteMethodName, GeneratedClassContext.DefaultWriteLiteralMethodName, "WriteTo", "WriteLiteralTo", WebPageRazorHost.TemplateTypeName, "DefineSection", "BeginContext", "EndContext");
      ((GeneratedClassContext) ref generatedClassContext).ResolveUrlMethodName = "Href";
      this.GeneratedClassContext = generatedClassContext;
      this.DefaultPageBaseClass = WebPageRazorHost.PageBaseClass;
      this.DefaultDebugCompilation = true;
      this.EnableInstrumentation = false;
    }

    public WebPageRazorHost(string virtualPath)
      : this(virtualPath, (string) null)
    {
    }

    public WebPageRazorHost(string virtualPath, string physicalPath)
      : this()
    {
      this.VirtualPath = !string.IsNullOrEmpty(virtualPath) ? virtualPath : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, new object[1]
      {
        (object) nameof (virtualPath)
      }), nameof (virtualPath));
      this.PhysicalPath = physicalPath;
      base.DefaultClassName = this.GetClassName(this.VirtualPath);
      base.CodeLanguage = this.GetCodeLanguage();
      this.EnableInstrumentation = new InstrumentationService().IsAvailable;
    }

    public virtual RazorCodeLanguage CodeLanguage
    {
      get
      {
        if (this._codeLanguage == null)
          this._codeLanguage = this.GetCodeLanguage();
        return this._codeLanguage;
      }
      protected set => this._codeLanguage = value;
    }

    public virtual string DefaultBaseClass
    {
      get
      {
        if (base.DefaultBaseClass != null)
          return base.DefaultBaseClass;
        return this.IsSpecialPage ? this.SpecialPageBaseClass : this.DefaultPageBaseClass;
      }
      set => base.DefaultBaseClass = value;
    }

    public virtual string DefaultClassName
    {
      get
      {
        if (this._className == null)
          this._className = this.GetClassName(this.VirtualPath);
        return this._className;
      }
      set => this._className = value;
    }

    public bool DefaultDebugCompilation { get; set; }

    public string DefaultPageBaseClass { get; set; }

    internal string GlobalAsaxTypeName
    {
      get
      {
        string globalAsaxTypeName = this._globalAsaxTypeName;
        if (globalAsaxTypeName != null)
          return globalAsaxTypeName;
        return !HostingEnvironment.IsHosted ? WebPageRazorHost.FallbackApplicationTypeName : BuildManager.GetGlobalAsaxType().FullName;
      }
      set => this._globalAsaxTypeName = value;
    }

    public bool IsSpecialPage
    {
      get
      {
        this.CheckForSpecialPage();
        return this._isSpecialPage.Value;
      }
    }

    public string PhysicalPath
    {
      get
      {
        this.MapPhysicalPath();
        return this._physicalPath;
      }
      set => this._physicalPath = value;
    }

    public virtual string InstrumentedSourceFilePath
    {
      get => this.VirtualPath;
      set => this.VirtualPath = value;
    }

    private string SpecialPageBaseClass
    {
      get
      {
        this.CheckForSpecialPage();
        return this._specialFileBaseClass;
      }
    }

    public string VirtualPath { get; private set; }

    public static void AddGlobalImport(string ns)
    {
      if (string.IsNullOrEmpty(ns))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, new object[1]
        {
          (object) nameof (ns)
        }), nameof (ns));
      WebPageRazorHost._importedNamespaces.TryAdd(ns, (object) null);
    }

    private void CheckForSpecialPage()
    {
      if (this._isSpecialPage.HasValue)
        return;
      string str;
      if (this._specialFileBaseTypes.TryGetValue(Path.GetFileNameWithoutExtension(this.VirtualPath), out str))
      {
        this._isSpecialPage = new bool?(true);
        this._specialFileBaseClass = str;
      }
      else
        this._isSpecialPage = new bool?(false);
    }

    public virtual ParserBase CreateMarkupParser() => (ParserBase) new HtmlMarkupParser();

    private static RazorCodeLanguage DetermineCodeLanguage(string fileName)
    {
      string extension = Path.GetExtension(fileName);
      if (string.IsNullOrEmpty(extension))
        return (RazorCodeLanguage) null;
      if (extension[0] == '.')
        extension = extension.Substring(1);
      return WebPageRazorHost.GetLanguageByExtension(extension);
    }

    protected virtual string GetClassName(string virtualPath)
    {
      return ParserHelpers.SanitizeClassName("_Page_" + virtualPath.TrimStart('~', '/'));
    }

    protected virtual RazorCodeLanguage GetCodeLanguage()
    {
      RazorCodeLanguage codeLanguage = WebPageRazorHost.DetermineCodeLanguage(this.VirtualPath);
      if (codeLanguage == null && !string.IsNullOrEmpty(this.PhysicalPath))
        codeLanguage = WebPageRazorHost.DetermineCodeLanguage(this.PhysicalPath);
      return codeLanguage != null ? codeLanguage : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, RazorWebResources.BuildProvider_No_CodeLanguageService_For_Path, new object[1]
      {
        (object) this.VirtualPath
      }));
    }

    public static IEnumerable<string> GetGlobalImports()
    {
      return ((IEnumerable<KeyValuePair<string, object>>) WebPageRazorHost._importedNamespaces.ToArray()).Select<KeyValuePair<string, object>, string>((Func<KeyValuePair<string, object>, string>) (pair => pair.Key));
    }

    private static RazorCodeLanguage GetLanguageByExtension(string extension)
    {
      return RazorCodeLanguage.GetLanguageByExtension(extension);
    }

    private void MapPhysicalPath()
    {
      if (this._physicalPath != null || !HostingEnvironment.IsHosted)
        return;
      string path = HostingEnvironment.MapPath(this.VirtualPath);
      if (string.IsNullOrEmpty(path) || !File.Exists(path))
        return;
      this._physicalPath = path;
    }

    public virtual void PostProcessGeneratedCode(CodeGeneratorContext context)
    {
      base.PostProcessGeneratedCode(context);
      context.Namespace.Imports.AddRange(WebPageRazorHost.GetGlobalImports().Select<string, CodeNamespaceImport>((Func<string, CodeNamespaceImport>) (s => new CodeNamespaceImport(s))).ToArray<CodeNamespaceImport>());
      CodeMemberProperty codeMemberProperty1 = new CodeMemberProperty();
      codeMemberProperty1.Name = "ApplicationInstance";
      codeMemberProperty1.Type = new CodeTypeReference(this.GlobalAsaxTypeName);
      codeMemberProperty1.HasGet = true;
      codeMemberProperty1.HasSet = false;
      codeMemberProperty1.Attributes = MemberAttributes.Family | MemberAttributes.Final;
      CodeMemberProperty codeMemberProperty2 = codeMemberProperty1;
      codeMemberProperty2.GetStatements.Add((CodeStatement) new CodeMethodReturnStatement((CodeExpression) new CodeCastExpression(new CodeTypeReference(this.GlobalAsaxTypeName), (CodeExpression) new CodePropertyReferenceExpression((CodeExpression) new CodePropertyReferenceExpression((CodeExpression) null, "Context"), "ApplicationInstance"))));
      context.GeneratedClass.Members.Insert(0, (CodeTypeMember) codeMemberProperty2);
    }

    protected void RegisterSpecialFile(string fileName, Type baseType)
    {
      if (baseType == (Type) null)
        throw new ArgumentNullException(nameof (baseType));
      this.RegisterSpecialFile(fileName, baseType.FullName);
    }

    protected void RegisterSpecialFile(string fileName, string baseTypeName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, new object[1]
        {
          (object) nameof (fileName)
        }), nameof (fileName));
      this._specialFileBaseTypes[fileName] = !string.IsNullOrEmpty(baseTypeName) ? baseTypeName : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, new object[1]
      {
        (object) nameof (baseTypeName)
      }), nameof (baseTypeName));
    }
  }
}
