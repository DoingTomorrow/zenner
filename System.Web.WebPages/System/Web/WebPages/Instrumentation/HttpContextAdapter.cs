// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Instrumentation.HttpContextAdapter
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.CSharp.RuntimeBinder;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.WebPages.Instrumentation
{
  [GeneratedCode("Microsoft.Web.CodeGen.DynamicCallerGenerator", "1.0.0.0")]
  internal class HttpContextAdapter
  {
    private static readonly bool _isInstrumentationAvailable = typeof (HttpContext).GetProperty(nameof (PageInstrumentation), BindingFlags.Instance | BindingFlags.Public) != (PropertyInfo) null;
    private static readonly Type _TargetType = typeof (HttpContext);

    internal static bool IsInstrumentationAvailable
    {
      get => HttpContextAdapter._isInstrumentationAvailable;
    }

    internal PageInstrumentationServiceAdapter PageInstrumentation
    {
      get
      {
        if (HttpContextAdapter.\u003Cget_PageInstrumentation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
          HttpContextAdapter.\u003Cget_PageInstrumentation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, nameof (PageInstrumentation), typeof (HttpContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        return new PageInstrumentationServiceAdapter(HttpContextAdapter.\u003Cget_PageInstrumentation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target((CallSite) HttpContextAdapter.\u003Cget_PageInstrumentation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1, this.Adaptee));
      }
    }

    internal object Adaptee { get; private set; }

    internal HttpContextAdapter(object existing) => this.Adaptee = existing;
  }
}
