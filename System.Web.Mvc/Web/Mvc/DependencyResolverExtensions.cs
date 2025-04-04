// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DependencyResolverExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public static class DependencyResolverExtensions
  {
    public static TService GetService<TService>(this IDependencyResolver resolver)
    {
      return (TService) resolver.GetService(typeof (TService));
    }

    public static IEnumerable<TService> GetServices<TService>(this IDependencyResolver resolver)
    {
      return resolver.GetServices(typeof (TService)).Cast<TService>();
    }
  }
}
