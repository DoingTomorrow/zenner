// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Instrumentation.PageExecutionContextAdapter
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.CSharp.RuntimeBinder;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.WebPages.Instrumentation
{
  [GeneratedCode("Microsoft.Web.CodeGen.DynamicCallerGenerator", "1.0.0.0")]
  internal class PageExecutionContextAdapter
  {
    private static readonly Type _TargetType = typeof (HttpContext).Assembly.GetType("System.Web.Instrumentation.PageExecutionContext");

    internal bool IsLiteral
    {
      get
      {
        if (PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
          PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (PageExecutionContextAdapter)));
        Func<CallSite, object, bool> target = PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
        CallSite<Func<CallSite, object, bool>> pSite1 = PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
        if (PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
          PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (IsLiteral), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) PageExecutionContextAdapter.\u003Cget_IsLiteral\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, this.Adaptee);
        return target((CallSite) pSite1, obj);
      }
      set
      {
        if (PageExecutionContextAdapter.\u003Cset_IsLiteral\u003Eo__SiteContainer3.\u003C\u003Ep__Site4 == null)
          PageExecutionContextAdapter.\u003Cset_IsLiteral\u003Eo__SiteContainer3.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, nameof (IsLiteral), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cset_IsLiteral\u003Eo__SiteContainer3.\u003C\u003Ep__Site4.Target((CallSite) PageExecutionContextAdapter.\u003Cset_IsLiteral\u003Eo__SiteContainer3.\u003C\u003Ep__Site4, this.Adaptee, value);
      }
    }

    internal int Length
    {
      get
      {
        if (PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site6 == null)
          PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site6 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (PageExecutionContextAdapter)));
        Func<CallSite, object, int> target = PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site6.Target;
        CallSite<Func<CallSite, object, int>> pSite6 = PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site6;
        if (PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site7 == null)
          PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (Length), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site7.Target((CallSite) PageExecutionContextAdapter.\u003Cget_Length\u003Eo__SiteContainer5.\u003C\u003Ep__Site7, this.Adaptee);
        return target((CallSite) pSite6, obj);
      }
      set
      {
        if (PageExecutionContextAdapter.\u003Cset_Length\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 == null)
          PageExecutionContextAdapter.\u003Cset_Length\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, nameof (Length), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cset_Length\u003Eo__SiteContainer8.\u003C\u003Ep__Site9.Target((CallSite) PageExecutionContextAdapter.\u003Cset_Length\u003Eo__SiteContainer8.\u003C\u003Ep__Site9, this.Adaptee, value);
      }
    }

    internal int StartPosition
    {
      get
      {
        if (PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Siteb == null)
          PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (PageExecutionContextAdapter)));
        Func<CallSite, object, int> target = PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Siteb.Target;
        CallSite<Func<CallSite, object, int>> pSiteb = PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Siteb;
        if (PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Sitec == null)
          PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Sitec = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (StartPosition), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Sitec.Target((CallSite) PageExecutionContextAdapter.\u003Cget_StartPosition\u003Eo__SiteContainera.\u003C\u003Ep__Sitec, this.Adaptee);
        return target((CallSite) pSiteb, obj);
      }
      set
      {
        if (PageExecutionContextAdapter.\u003Cset_StartPosition\u003Eo__SiteContainerd.\u003C\u003Ep__Sitee == null)
          PageExecutionContextAdapter.\u003Cset_StartPosition\u003Eo__SiteContainerd.\u003C\u003Ep__Sitee = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, nameof (StartPosition), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cset_StartPosition\u003Eo__SiteContainerd.\u003C\u003Ep__Sitee.Target((CallSite) PageExecutionContextAdapter.\u003Cset_StartPosition\u003Eo__SiteContainerd.\u003C\u003Ep__Sitee, this.Adaptee, value);
      }
    }

    internal TextWriter TextWriter
    {
      get
      {
        if (PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site10 == null)
          PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site10 = CallSite<Func<CallSite, object, TextWriter>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TextWriter), typeof (PageExecutionContextAdapter)));
        Func<CallSite, object, TextWriter> target = PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site10.Target;
        CallSite<Func<CallSite, object, TextWriter>> pSite10 = PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site10;
        if (PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site11 == null)
          PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (TextWriter), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site11.Target((CallSite) PageExecutionContextAdapter.\u003Cget_TextWriter\u003Eo__SiteContainerf.\u003C\u003Ep__Site11, this.Adaptee);
        return target((CallSite) pSite10, obj);
      }
      set
      {
        if (PageExecutionContextAdapter.\u003Cset_TextWriter\u003Eo__SiteContainer12.\u003C\u003Ep__Site13 == null)
          PageExecutionContextAdapter.\u003Cset_TextWriter\u003Eo__SiteContainer12.\u003C\u003Ep__Site13 = CallSite<Func<CallSite, object, TextWriter, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, nameof (TextWriter), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cset_TextWriter\u003Eo__SiteContainer12.\u003C\u003Ep__Site13.Target((CallSite) PageExecutionContextAdapter.\u003Cset_TextWriter\u003Eo__SiteContainer12.\u003C\u003Ep__Site13, this.Adaptee, value);
      }
    }

    internal string VirtualPath
    {
      get
      {
        if (PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site15 == null)
          PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (PageExecutionContextAdapter)));
        Func<CallSite, object, string> target = PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site15.Target;
        CallSite<Func<CallSite, object, string>> pSite15 = PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site15;
        if (PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site16 == null)
          PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (VirtualPath), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site16.Target((CallSite) PageExecutionContextAdapter.\u003Cget_VirtualPath\u003Eo__SiteContainer14.\u003C\u003Ep__Site16, this.Adaptee);
        return target((CallSite) pSite15, obj);
      }
      set
      {
        if (PageExecutionContextAdapter.\u003Cset_VirtualPath\u003Eo__SiteContainer17.\u003C\u003Ep__Site18 == null)
          PageExecutionContextAdapter.\u003Cset_VirtualPath\u003Eo__SiteContainer17.\u003C\u003Ep__Site18 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, nameof (VirtualPath), typeof (PageExecutionContextAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        object obj = PageExecutionContextAdapter.\u003Cset_VirtualPath\u003Eo__SiteContainer17.\u003C\u003Ep__Site18.Target((CallSite) PageExecutionContextAdapter.\u003Cset_VirtualPath\u003Eo__SiteContainer17.\u003C\u003Ep__Site18, this.Adaptee, value);
      }
    }

    internal PageExecutionContextAdapter()
    {
      this.Adaptee = PageExecutionContextAdapter._CallSite_ctor_1.Site();
    }

    internal object Adaptee { get; private set; }

    internal PageExecutionContextAdapter(object existing) => this.Adaptee = existing;

    private static class _CallSite_ctor_1
    {
      public static Func<object> Site = ((Expression<Func<object>>) (() => Expression.New(PageExecutionContextAdapter._TargetType.GetConstructor(new Type[0])))).Compile();
    }
  }
}
