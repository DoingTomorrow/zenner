// Decompiled with JetBrains decompiler
// Type: AutoMapper.ResolutionResult
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public class ResolutionResult
  {
    private readonly object _value;
    private readonly ResolutionContext _context;
    private readonly Type _type;
    private readonly Type _memberType;

    public ResolutionResult(ResolutionContext context)
      : this(context.SourceValue, context, context.SourceType)
    {
    }

    private ResolutionResult(object value, ResolutionContext context, Type memberType)
    {
      this._value = value;
      this._context = context;
      this._type = this.ResolveType(value, memberType);
      this._memberType = memberType;
    }

    private ResolutionResult(object value, ResolutionContext context)
    {
      this._value = value;
      this._context = context;
      this._type = this.ResolveType(value, typeof (object));
      this._memberType = this._type;
    }

    public object Value => this._value;

    public Type Type => this._type;

    public Type MemberType => this._memberType;

    public ResolutionContext Context => this._context;

    public bool ShouldIgnore { get; set; }

    private Type ResolveType(object value, Type memberType)
    {
      return value == null ? memberType : value.GetType();
    }

    public ResolutionResult Ignore()
    {
      return new ResolutionResult(this.Context)
      {
        ShouldIgnore = true
      };
    }

    public ResolutionResult New(object value) => new ResolutionResult(value, this.Context);

    public ResolutionResult New(object value, Type memberType)
    {
      return new ResolutionResult(value, this.Context, memberType);
    }
  }
}
