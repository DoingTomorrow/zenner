// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Instrumentation.PageExecutionListenerAdapter
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.CSharp.RuntimeBinder;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.WebPages.Instrumentation
{
  [GeneratedCode("Microsoft.Web.CodeGen.DynamicCallerGenerator", "1.0.0.0")]
  internal class PageExecutionListenerAdapter
  {
    private static readonly Type _TargetType = typeof (HttpContext).Assembly.GetType("System.Web.Instrumentation.PageExecutionListener");

    internal void BeginContext(PageExecutionContextAdapter context)
    {
      // ISSUE: reference to a compiler-generated field
      if (PageExecutionListenerAdapter.\u003CBeginContext\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PageExecutionListenerAdapter.\u003CBeginContext\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (BeginContext), (IEnumerable<Type>) null, typeof (PageExecutionListenerAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PageExecutionListenerAdapter.\u003CBeginContext\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target((CallSite) PageExecutionListenerAdapter.\u003CBeginContext\u003Eo__SiteContainer0.\u003C\u003Ep__Site1, this.Adaptee, context.Adaptee);
    }

    internal void EndContext(PageExecutionContextAdapter context)
    {
      // ISSUE: reference to a compiler-generated field
      if (PageExecutionListenerAdapter.\u003CEndContext\u003Eo__SiteContainer2.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PageExecutionListenerAdapter.\u003CEndContext\u003Eo__SiteContainer2.\u003C\u003Ep__Site3 = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (EndContext), (IEnumerable<Type>) null, typeof (PageExecutionListenerAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PageExecutionListenerAdapter.\u003CEndContext\u003Eo__SiteContainer2.\u003C\u003Ep__Site3.Target((CallSite) PageExecutionListenerAdapter.\u003CEndContext\u003Eo__SiteContainer2.\u003C\u003Ep__Site3, this.Adaptee, context.Adaptee);
    }

    internal object Adaptee { get; private set; }

    internal PageExecutionListenerAdapter(object existing) => this.Adaptee = existing;
  }
}
