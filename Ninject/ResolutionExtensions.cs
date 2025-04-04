// Decompiled with JetBrains decompiler
// Type: Ninject.ResolutionExtensions
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject
{
  public static class ResolutionExtensions
  {
    public static T Get<T>(this IResolutionRoot root, params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, typeof (T), (Func<IBindingMetadata, bool>) null, (IEnumerable<IParameter>) parameters, false, true).Cast<T>().Single<T>();
    }

    public static T Get<T>(this IResolutionRoot root, string name, params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, typeof (T), (Func<IBindingMetadata, bool>) (b => b.Name == name), (IEnumerable<IParameter>) parameters, false, true).Cast<T>().Single<T>();
    }

    public static T Get<T>(
      this IResolutionRoot root,
      Func<IBindingMetadata, bool> constraint,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, typeof (T), constraint, (IEnumerable<IParameter>) parameters, false, true).Cast<T>().Single<T>();
    }

    public static T TryGet<T>(this IResolutionRoot root, params IParameter[] parameters)
    {
      return ResolutionExtensions.TryGet<T>(ResolutionExtensions.GetResolutionIterator(root, typeof (T), (Func<IBindingMetadata, bool>) null, (IEnumerable<IParameter>) parameters, true, true).Cast<T>());
    }

    public static T TryGet<T>(
      this IResolutionRoot root,
      string name,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.TryGet<T>(ResolutionExtensions.GetResolutionIterator(root, typeof (T), (Func<IBindingMetadata, bool>) (b => b.Name == name), (IEnumerable<IParameter>) parameters, true, true).Cast<T>());
    }

    public static T TryGet<T>(
      this IResolutionRoot root,
      Func<IBindingMetadata, bool> constraint,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.TryGet<T>(ResolutionExtensions.GetResolutionIterator(root, typeof (T), constraint, (IEnumerable<IParameter>) parameters, true, true).Cast<T>());
    }

    public static IEnumerable<T> GetAll<T>(
      this IResolutionRoot root,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, typeof (T), (Func<IBindingMetadata, bool>) null, (IEnumerable<IParameter>) parameters, true, false).Cast<T>();
    }

    public static IEnumerable<T> GetAll<T>(
      this IResolutionRoot root,
      string name,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, typeof (T), (Func<IBindingMetadata, bool>) (b => b.Name == name), (IEnumerable<IParameter>) parameters, true, false).Cast<T>();
    }

    public static IEnumerable<T> GetAll<T>(
      this IResolutionRoot root,
      Func<IBindingMetadata, bool> constraint,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, typeof (T), constraint, (IEnumerable<IParameter>) parameters, true, false).Cast<T>();
    }

    public static object Get(
      this IResolutionRoot root,
      Type service,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, service, (Func<IBindingMetadata, bool>) null, (IEnumerable<IParameter>) parameters, false, true).Single<object>();
    }

    public static object Get(
      this IResolutionRoot root,
      Type service,
      string name,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, service, (Func<IBindingMetadata, bool>) (b => b.Name == name), (IEnumerable<IParameter>) parameters, false, true).Single<object>();
    }

    public static object Get(
      this IResolutionRoot root,
      Type service,
      Func<IBindingMetadata, bool> constraint,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, service, constraint, (IEnumerable<IParameter>) parameters, false, true).Single<object>();
    }

    public static object TryGet(
      this IResolutionRoot root,
      Type service,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.TryGet<object>(ResolutionExtensions.GetResolutionIterator(root, service, (Func<IBindingMetadata, bool>) null, (IEnumerable<IParameter>) parameters, true, true));
    }

    public static object TryGet(
      this IResolutionRoot root,
      Type service,
      string name,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.TryGet<object>(ResolutionExtensions.GetResolutionIterator(root, service, (Func<IBindingMetadata, bool>) (b => b.Name == name), (IEnumerable<IParameter>) parameters, true, false));
    }

    public static object TryGet(
      this IResolutionRoot root,
      Type service,
      Func<IBindingMetadata, bool> constraint,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.TryGet<object>(ResolutionExtensions.GetResolutionIterator(root, service, constraint, (IEnumerable<IParameter>) parameters, true, false));
    }

    public static IEnumerable<object> GetAll(
      this IResolutionRoot root,
      Type service,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, service, (Func<IBindingMetadata, bool>) null, (IEnumerable<IParameter>) parameters, true, false);
    }

    public static IEnumerable<object> GetAll(
      this IResolutionRoot root,
      Type service,
      string name,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, service, (Func<IBindingMetadata, bool>) (b => b.Name == name), (IEnumerable<IParameter>) parameters, true, false);
    }

    public static IEnumerable<object> GetAll(
      this IResolutionRoot root,
      Type service,
      Func<IBindingMetadata, bool> constraint,
      params IParameter[] parameters)
    {
      return ResolutionExtensions.GetResolutionIterator(root, service, constraint, (IEnumerable<IParameter>) parameters, true, false);
    }

    private static IEnumerable<object> GetResolutionIterator(
      IResolutionRoot root,
      Type service,
      Func<IBindingMetadata, bool> constraint,
      IEnumerable<IParameter> parameters,
      bool isOptional,
      bool isUnique)
    {
      Ensure.ArgumentNotNull((object) root, nameof (root));
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) parameters, nameof (parameters));
      IRequest request = root.CreateRequest(service, constraint, parameters, isOptional, isUnique);
      return root.Resolve(request);
    }

    private static T TryGet<T>(IEnumerable<T> iterator)
    {
      try
      {
        return iterator.SingleOrDefault<T>();
      }
      catch (ActivationException ex)
      {
        return default (T);
      }
    }
  }
}
