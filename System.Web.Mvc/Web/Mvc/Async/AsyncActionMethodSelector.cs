// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.AsyncActionMethodSelector
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc.Async
{
  internal sealed class AsyncActionMethodSelector
  {
    private bool _allowLegacyAsyncActions;

    public AsyncActionMethodSelector(Type controllerType, bool allowLegacyAsyncActions = true)
    {
      this._allowLegacyAsyncActions = allowLegacyAsyncActions;
      this.ControllerType = controllerType;
      this.PopulateLookupTables();
    }

    public Type ControllerType { get; private set; }

    public MethodInfo[] AliasedMethods { get; private set; }

    public ILookup<string, MethodInfo> NonAliasedMethods { get; private set; }

    private AmbiguousMatchException CreateAmbiguousActionMatchException(
      IEnumerable<MethodInfo> ambiguousMethods,
      string actionName)
    {
      string ambiguousMatchList = AsyncActionMethodSelector.CreateAmbiguousMatchList(ambiguousMethods);
      return new AmbiguousMatchException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ActionMethodSelector_AmbiguousMatch, new object[3]
      {
        (object) actionName,
        (object) this.ControllerType.Name,
        (object) ambiguousMatchList
      }));
    }

    private AmbiguousMatchException CreateAmbiguousMethodMatchException(
      IEnumerable<MethodInfo> ambiguousMethods,
      string methodName)
    {
      string ambiguousMatchList = AsyncActionMethodSelector.CreateAmbiguousMatchList(ambiguousMethods);
      return new AmbiguousMatchException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.AsyncActionMethodSelector_AmbiguousMethodMatch, new object[3]
      {
        (object) methodName,
        (object) this.ControllerType.Name,
        (object) ambiguousMatchList
      }));
    }

    private static string CreateAmbiguousMatchList(IEnumerable<MethodInfo> ambiguousMethods)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (MethodInfo ambiguousMethod in ambiguousMethods)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ActionMethodSelector_AmbiguousMatchType, new object[2]
        {
          (object) ambiguousMethod,
          (object) ambiguousMethod.DeclaringType.FullName
        });
      }
      return stringBuilder.ToString();
    }

    public ActionDescriptorCreator FindAction(
      ControllerContext controllerContext,
      string actionName)
    {
      List<MethodInfo> matchingAliasedMethods = this.GetMatchingAliasedMethods(controllerContext, actionName);
      matchingAliasedMethods.AddRange(this.NonAliasedMethods[actionName]);
      List<MethodInfo> ambiguousMethods = AsyncActionMethodSelector.RunSelectionFilters(controllerContext, matchingAliasedMethods);
      switch (ambiguousMethods.Count)
      {
        case 0:
          return (ActionDescriptorCreator) null;
        case 1:
          return this.GetActionDescriptorDelegate(ambiguousMethods[0]);
        default:
          throw this.CreateAmbiguousActionMatchException((IEnumerable<MethodInfo>) ambiguousMethods, actionName);
      }
    }

    private ActionDescriptorCreator GetActionDescriptorDelegate(MethodInfo entryMethod)
    {
      if (entryMethod.ReturnType != (Type) null && typeof (Task).IsAssignableFrom(entryMethod.ReturnType))
        return (ActionDescriptorCreator) ((actionName, controllerDescriptor) => (ActionDescriptor) new TaskAsyncActionDescriptor(entryMethod, actionName, controllerDescriptor));
      if (!this.IsAsyncSuffixedMethod(entryMethod))
        return (ActionDescriptorCreator) ((actionName, controllerDescriptor) => (ActionDescriptor) new ReflectedActionDescriptor(entryMethod, actionName, controllerDescriptor));
      string methodName = entryMethod.Name.Substring(0, entryMethod.Name.Length - "Async".Length) + "Completed";
      MethodInfo completionMethod = this.GetMethodByName(methodName);
      if (completionMethod != (MethodInfo) null)
        return (ActionDescriptorCreator) ((actionName, controllerDescriptor) => (ActionDescriptor) new ReflectedAsyncActionDescriptor(entryMethod, completionMethod, actionName, controllerDescriptor));
      throw Error.AsyncActionMethodSelector_CouldNotFindMethod(methodName, this.ControllerType);
    }

    private string GetCanonicalMethodName(MethodInfo methodInfo)
    {
      string name = methodInfo.Name;
      return !this.IsAsyncSuffixedMethod(methodInfo) ? name : name.Substring(0, name.Length - "Async".Length);
    }

    internal List<MethodInfo> GetMatchingAliasedMethods(
      ControllerContext controllerContext,
      string actionName)
    {
      return ((IEnumerable<MethodInfo>) this.AliasedMethods).Select(methodInfo => new
      {
        methodInfo = methodInfo,
        attrs = ReflectedAttributeCache.GetActionNameSelectorAttributes(methodInfo)
      }).Where(_param1 => _param1.attrs.All<ActionNameSelectorAttribute>((Func<ActionNameSelectorAttribute, bool>) (attr => attr.IsValidName(controllerContext, actionName, _param1.methodInfo)))).Select(_param0 => _param0.methodInfo).ToList<MethodInfo>();
    }

    private bool IsAsyncSuffixedMethod(MethodInfo methodInfo)
    {
      return this._allowLegacyAsyncActions && methodInfo.Name.EndsWith("Async", StringComparison.OrdinalIgnoreCase);
    }

    private bool IsCompletedSuffixedMethod(MethodInfo methodInfo)
    {
      return this._allowLegacyAsyncActions && methodInfo.Name.EndsWith("Completed", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsMethodDecoratedWithAliasingAttribute(MethodInfo methodInfo)
    {
      return methodInfo.IsDefined(typeof (ActionNameSelectorAttribute), true);
    }

    private MethodInfo GetMethodByName(string methodName)
    {
      List<MethodInfo> list = this.ControllerType.GetMember(methodName, MemberTypes.Method, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod).Cast<MethodInfo>().Where<MethodInfo>((Func<MethodInfo, bool>) (methodInfo => this.IsValidActionMethod(methodInfo, false))).ToList<MethodInfo>();
      switch (list.Count)
      {
        case 0:
          return (MethodInfo) null;
        case 1:
          return list[0];
        default:
          throw this.CreateAmbiguousMethodMatchException((IEnumerable<MethodInfo>) list, methodName);
      }
    }

    private bool IsValidActionMethod(MethodInfo methodInfo)
    {
      return this.IsValidActionMethod(methodInfo, true);
    }

    private bool IsValidActionMethod(MethodInfo methodInfo, bool stripInfrastructureMethods)
    {
      return !methodInfo.IsSpecialName && !methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof (AsyncController)) && (!stripInfrastructureMethods || !this.IsCompletedSuffixedMethod(methodInfo));
    }

    private void PopulateLookupTables()
    {
      MethodInfo[] all = Array.FindAll<MethodInfo>(this.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod), new Predicate<MethodInfo>(this.IsValidActionMethod));
      this.AliasedMethods = Array.FindAll<MethodInfo>(all, new Predicate<MethodInfo>(AsyncActionMethodSelector.IsMethodDecoratedWithAliasingAttribute));
      this.NonAliasedMethods = ((IEnumerable<MethodInfo>) all).Except<MethodInfo>((IEnumerable<MethodInfo>) this.AliasedMethods).ToLookup<MethodInfo, string>(new Func<MethodInfo, string>(this.GetCanonicalMethodName), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    private static List<MethodInfo> RunSelectionFilters(
      ControllerContext controllerContext,
      List<MethodInfo> methodInfos)
    {
      List<MethodInfo> methodInfoList1 = new List<MethodInfo>();
      List<MethodInfo> methodInfoList2 = new List<MethodInfo>();
      foreach (MethodInfo methodInfo1 in methodInfos)
      {
        MethodInfo methodInfo = methodInfo1;
        ICollection<ActionMethodSelectorAttribute> selectorAttributes = ReflectedAttributeCache.GetActionMethodSelectorAttributes(methodInfo);
        if (selectorAttributes.Count == 0)
          methodInfoList2.Add(methodInfo);
        else if (selectorAttributes.All<ActionMethodSelectorAttribute>((Func<ActionMethodSelectorAttribute, bool>) (attr => attr.IsValidForRequest(controllerContext, methodInfo))))
          methodInfoList1.Add(methodInfo);
      }
      return methodInfoList1.Count <= 0 ? methodInfoList2 : methodInfoList1;
    }
  }
}
