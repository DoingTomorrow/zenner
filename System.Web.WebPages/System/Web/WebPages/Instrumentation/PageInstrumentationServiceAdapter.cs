// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Instrumentation.PageInstrumentationServiceAdapter
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Web.WebPages.Instrumentation
{
  internal class PageInstrumentationServiceAdapter
  {
    private static readonly Type _targetType = typeof (HttpContext).Assembly.GetType("System.Web.Instrumentation.PageInstrumentationService");

    internal PageInstrumentationServiceAdapter()
    {
      this.Adaptee = PageInstrumentationServiceAdapter._CallSite_ctor_2.Site();
    }

    internal PageInstrumentationServiceAdapter(object existing) => this.Adaptee = existing;

    internal IEnumerable<PageExecutionListenerAdapter> ExecutionListeners
    {
      get
      {
        if (PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
          PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, IEnumerable<object>>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable<object>), typeof (PageInstrumentationServiceAdapter)));
        Func<CallSite, object, IEnumerable<object>> target = PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
        CallSite<Func<CallSite, object, IEnumerable<object>>> pSite1 = PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
        if (PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
          PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, nameof (ExecutionListeners), typeof (PageInstrumentationServiceAdapter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj = PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) PageInstrumentationServiceAdapter.\u003Cget_ExecutionListeners\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, this.Adaptee);
        return target((CallSite) pSite1, obj).Select<object, PageExecutionListenerAdapter>((Func<object, PageExecutionListenerAdapter>) (listener => new PageExecutionListenerAdapter(listener)));
      }
    }

    internal static bool IsEnabled
    {
      get => PageInstrumentationServiceAdapter._CallSite_IsEnabled_1.Getter();
      set => PageInstrumentationServiceAdapter._CallSite_IsEnabled_1.Setter(value);
    }

    internal object Adaptee { get; private set; }

    private static class _CallSite_IsEnabled_1
    {
      public static Func<bool> Getter;
      public static Action<bool> Setter;

      static _CallSite_IsEnabled_1()
      {
        PropertyInfo property = (PropertyInfo) null;
        if (PageInstrumentationServiceAdapter._targetType != (Type) null)
          property = PageInstrumentationServiceAdapter._targetType.GetProperty("IsEnabled", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, typeof (bool), Type.EmptyTypes, new ParameterModifier[0]);
        if (property != (PropertyInfo) null)
        {
          PageInstrumentationServiceAdapter._CallSite_IsEnabled_1.Getter = Expression.Lambda<Func<bool>>((Expression) Expression.Property((Expression) null, property)).Compile();
          PageInstrumentationServiceAdapter._CallSite_IsEnabled_1.Setter = ((Expression<Action<bool>>) (flag => Expression.Assign((Expression) Expression.Property((Expression) null, property), flag))).Compile();
        }
        else
        {
          PageInstrumentationServiceAdapter._CallSite_IsEnabled_1.Getter = (Func<bool>) (() => false);
          PageInstrumentationServiceAdapter._CallSite_IsEnabled_1.Setter = (Action<bool>) (_ => { });
        }
      }
    }

    private static class _CallSite_ctor_2
    {
      public static Func<object> Site;

      static _CallSite_ctor_2()
      {
        if (PageInstrumentationServiceAdapter._targetType != (Type) null)
          PageInstrumentationServiceAdapter._CallSite_ctor_2.Site = ((Expression<Func<object>>) (() => Expression.New(PageInstrumentationServiceAdapter._targetType.GetConstructor(new Type[0])))).Compile();
        else
          PageInstrumentationServiceAdapter._CallSite_ctor_2.Site = (Func<object>) (() => (object) null);
      }
    }
  }
}
