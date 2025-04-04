// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.WebCodeRazorHost
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;

#nullable disable
namespace System.Web.WebPages.Razor
{
  public class WebCodeRazorHost : WebPageRazorHost
  {
    private const string AppCodeDir = "App_Code";
    private const string HttpContextAccessorName = "Context";
    private static readonly string _helperPageBaseType = typeof (HelperPage).FullName;

    public WebCodeRazorHost(string virtualPath)
      : base(virtualPath)
    {
      base.DefaultBaseClass = WebCodeRazorHost._helperPageBaseType;
      this.DefaultNamespace = WebCodeRazorHost.DetermineNamespace(virtualPath);
      this.DefaultDebugCompilation = false;
      this.StaticHelpers = true;
    }

    public WebCodeRazorHost(string virtualPath, string physicalPath)
      : base(virtualPath, physicalPath)
    {
      base.DefaultBaseClass = WebCodeRazorHost._helperPageBaseType;
      this.DefaultNamespace = WebCodeRazorHost.DetermineNamespace(virtualPath);
      this.DefaultDebugCompilation = false;
      this.StaticHelpers = true;
    }

    public override void PostProcessGeneratedCode(CodeGeneratorContext context)
    {
      base.PostProcessGeneratedCode(context);
      context.GeneratedClass.Members.Remove((CodeTypeMember) context.TargetMethod);
      CodeMemberProperty codeMemberProperty = context.GeneratedClass.Members.OfType<CodeMemberProperty>().Where<CodeMemberProperty>((Func<CodeMemberProperty, bool>) (p => "ApplicationInstance".Equals(p.Name))).SingleOrDefault<CodeMemberProperty>();
      if (codeMemberProperty == null)
        return;
      codeMemberProperty.Attributes |= MemberAttributes.Static;
    }

    protected override string GetClassName(string virtualPath)
    {
      return ParserHelpers.SanitizeClassName(Path.GetFileNameWithoutExtension(virtualPath));
    }

    private static string DetermineNamespace(string virtualPath)
    {
      virtualPath = virtualPath.Replace(Path.DirectorySeparatorChar, '/');
      virtualPath = WebCodeRazorHost.GetDirectory(virtualPath);
      int num = virtualPath.IndexOf("App_Code", StringComparison.OrdinalIgnoreCase);
      if (num != -1)
        virtualPath = virtualPath.Substring(num + "App_Code".Length);
      IEnumerable<string> strings = (IEnumerable<string>) virtualPath.Split(new char[1]
      {
        '/'
      }, StringSplitOptions.RemoveEmptyEntries);
      return !strings.Any<string>() ? "ASP" : "ASP." + string.Join(".", strings);
    }

    private static string GetDirectory(string virtualPath)
    {
      int length = virtualPath.LastIndexOf('/');
      return length != -1 ? virtualPath.Substring(0, length) : string.Empty;
    }
  }
}
