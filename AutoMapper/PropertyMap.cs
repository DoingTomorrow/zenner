// Decompiled with JetBrains decompiler
// Type: AutoMapper.PropertyMap
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class PropertyMap
  {
    private readonly LinkedList<IValueResolver> _sourceValueResolvers = new LinkedList<IValueResolver>();
    private readonly IList<Type> _valueFormattersToSkip = (IList<Type>) new List<Type>();
    private readonly IList<IValueFormatter> _valueFormatters = (IList<IValueFormatter>) new List<IValueFormatter>();
    private bool _ignored;
    private int _mappingOrder;
    private bool _hasCustomValueResolver;
    private IValueResolver _customResolver;
    private IValueResolver _customMemberResolver;
    private object _nullSubstitute;
    private bool _sealed;
    private IValueResolver[] _cachedResolvers;
    private Func<ResolutionContext, bool> _condition;
    private MemberInfo _sourceMember;

    public PropertyMap(IMemberAccessor destinationProperty)
    {
      this.UseDestinationValue = true;
      this.DestinationProperty = destinationProperty;
    }

    public PropertyMap(PropertyMap inheritedMappedProperty)
      : this(inheritedMappedProperty.DestinationProperty)
    {
      if (inheritedMappedProperty.IsIgnored())
      {
        this.Ignore();
      }
      else
      {
        foreach (IValueResolver sourceValueResolver in inheritedMappedProperty.GetSourceValueResolvers())
          this.ChainResolver(sourceValueResolver);
      }
      this.ApplyCondition(inheritedMappedProperty._condition);
      this.SetNullSubstitute(inheritedMappedProperty._nullSubstitute);
      this.SetMappingOrder(inheritedMappedProperty._mappingOrder);
    }

    public IMemberAccessor DestinationProperty { get; private set; }

    public Type DestinationPropertyType => this.DestinationProperty.MemberType;

    public LambdaExpression CustomExpression { get; private set; }

    public MemberInfo SourceMember
    {
      get
      {
        if ((object) this._sourceMember != null)
          return this._sourceMember;
        return this.GetSourceValueResolvers().OfType<IMemberGetter>().LastOrDefault<IMemberGetter>()?.MemberInfo;
      }
      internal set => this._sourceMember = value;
    }

    public bool CanBeSet
    {
      get
      {
        return !(this.DestinationProperty is PropertyAccessor) || ((PropertyAccessor) this.DestinationProperty).HasSetter;
      }
    }

    public bool UseDestinationValue { get; set; }

    internal bool HasCustomValueResolver => this._hasCustomValueResolver;

    public IEnumerable<IValueResolver> GetSourceValueResolvers()
    {
      if (this._customMemberResolver != null)
        yield return this._customMemberResolver;
      if (this._customResolver != null)
        yield return this._customResolver;
      foreach (IValueResolver resolver in this._sourceValueResolvers)
        yield return resolver;
      if (this._nullSubstitute != null)
        yield return (IValueResolver) new NullReplacementMethod(this._nullSubstitute);
    }

    public void RemoveLastResolver() => this._sourceValueResolvers.RemoveLast();

    public ResolutionResult ResolveValue(ResolutionContext context)
    {
      this.Seal();
      return ((IEnumerable<IValueResolver>) this._cachedResolvers).Aggregate<IValueResolver, ResolutionResult>(new ResolutionResult(context), (Func<ResolutionResult, IValueResolver, ResolutionResult>) ((current, resolver) => resolver.Resolve(current)));
    }

    internal void Seal()
    {
      if (this._sealed)
        return;
      this._cachedResolvers = this.GetSourceValueResolvers().ToArray<IValueResolver>();
      this._sealed = true;
    }

    public void ChainResolver(IValueResolver IValueResolver)
    {
      this._sourceValueResolvers.AddLast(IValueResolver);
    }

    public void AddFormatterToSkip<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      this._valueFormattersToSkip.Add(typeof (TValueFormatter));
    }

    public bool FormattersToSkipContains(Type valueFormatterType)
    {
      return this._valueFormattersToSkip.Contains(valueFormatterType);
    }

    public void AddFormatter(IValueFormatter valueFormatter)
    {
      this._valueFormatters.Add(valueFormatter);
    }

    public IValueFormatter[] GetFormatters() => this._valueFormatters.ToArray<IValueFormatter>();

    public void AssignCustomValueResolver(IValueResolver valueResolver)
    {
      this._ignored = false;
      this._customResolver = valueResolver;
      this.ResetSourceMemberChain();
      this._hasCustomValueResolver = true;
    }

    public void ChainTypeMemberForResolver(IValueResolver valueResolver)
    {
      this.ResetSourceMemberChain();
      this._customMemberResolver = valueResolver;
    }

    public void ChainConstructorForResolver(IValueResolver valueResolver)
    {
      this._customResolver = valueResolver;
    }

    public void Ignore() => this._ignored = true;

    public bool IsIgnored() => this._ignored;

    public void SetMappingOrder(int mappingOrder) => this._mappingOrder = mappingOrder;

    public int GetMappingOrder() => this._mappingOrder;

    public bool IsMapped()
    {
      return this._sourceValueResolvers.Count > 0 || this._hasCustomValueResolver || this._ignored;
    }

    public bool CanResolveValue()
    {
      return (this._sourceValueResolvers.Count > 0 || this._hasCustomValueResolver || this.UseDestinationValue) && !this._ignored;
    }

    public void RemoveLastFormatter()
    {
      this._valueFormatters.RemoveAt(this._valueFormatters.Count - 1);
    }

    public void SetNullSubstitute(object nullSubstitute) => this._nullSubstitute = nullSubstitute;

    private void ResetSourceMemberChain() => this._sourceValueResolvers.Clear();

    public bool Equals(PropertyMap other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.DestinationProperty, (object) this.DestinationProperty);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) obj.GetType() == (object) typeof (PropertyMap) && this.Equals((PropertyMap) obj);
    }

    public override int GetHashCode() => this.DestinationProperty.GetHashCode();

    public void ApplyCondition(Func<ResolutionContext, bool> condition)
    {
      this._condition = condition;
    }

    public bool ShouldAssignValue(ResolutionContext context)
    {
      return this._condition == null || this._condition(context);
    }

    public void SetCustomValueResolverExpression<TSource, TMember>(
      Expression<Func<TSource, TMember>> sourceMember)
    {
      if (sourceMember.Body is MemberExpression)
        this.SourceMember = ((MemberExpression) sourceMember.Body).Member;
      this.CustomExpression = (LambdaExpression) sourceMember;
      this.AssignCustomValueResolver((IValueResolver) new NullReferenceExceptionSwallowingResolver((IMemberResolver) new DelegateBasedResolver<TSource, TMember>(sourceMember.Compile())));
    }

    public object GetDestinationValue(object mappedObject)
    {
      return !this.UseDestinationValue ? (object) null : this.DestinationProperty.GetValue(mappedObject);
    }

    public ExpressionResolutionResult ResolveExpression(
      Type currentType,
      Expression instanceParameter)
    {
      Expression expression = instanceParameter;
      Type type = currentType;
      foreach (IValueResolver sourceValueResolver in this.GetSourceValueResolvers())
      {
        if (sourceValueResolver is IMemberGetter memberGetter)
        {
          PropertyInfo memberInfo = memberGetter.MemberInfo as PropertyInfo;
          expression = (object) memberInfo != null ? (Expression) Expression.Property(expression, memberInfo) : throw new NotImplementedException("Expressions mapping from methods not supported yet.");
          type = memberInfo.PropertyType;
        }
        else
        {
          ParameterExpression oldParameter = this.CustomExpression.Parameters.Single<ParameterExpression>();
          expression = new PropertyMap.ConversionVisitor(instanceParameter, oldParameter).Visit(this.CustomExpression.Body);
          type = expression.Type;
        }
      }
      return new ExpressionResolutionResult(expression, type);
    }

    private class ConversionVisitor : ExpressionVisitor
    {
      private readonly Expression newParameter;
      private readonly ParameterExpression oldParameter;

      public ConversionVisitor(Expression newParameter, ParameterExpression oldParameter)
      {
        this.newParameter = newParameter;
        this.oldParameter = oldParameter;
      }

      protected override Expression VisitParameter(ParameterExpression node)
      {
        return (object) node.Type != (object) this.oldParameter.Type ? (Expression) node : this.newParameter;
      }

      protected override Expression VisitMember(MemberExpression node)
      {
        return node.Expression != this.oldParameter ? base.VisitMember(node) : (Expression) Expression.MakeMemberAccess(this.Visit(node.Expression), ((IEnumerable<MemberInfo>) this.newParameter.Type.GetMember(node.Member.Name)).First<MemberInfo>());
      }
    }
  }
}
