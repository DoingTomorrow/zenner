// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.NoThrowGetBinderMember
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Dynamic;

#nullable disable
namespace Newtonsoft.Json.Utilities
{
  internal class NoThrowGetBinderMember : GetMemberBinder
  {
    private readonly GetMemberBinder _innerBinder;

    public NoThrowGetBinderMember(GetMemberBinder innerBinder)
      : base(innerBinder.Name, innerBinder.IgnoreCase)
    {
      this._innerBinder = innerBinder;
    }

    public override DynamicMetaObject FallbackGetMember(
      DynamicMetaObject target,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[0]);
      return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
    }
  }
}
