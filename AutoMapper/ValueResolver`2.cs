// Decompiled with JetBrains decompiler
// Type: AutoMapper.ValueResolver`2
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

#nullable disable
namespace AutoMapper
{
  public abstract class ValueResolver<TSource, TDestination> : IValueResolver
  {
    public ResolutionResult Resolve(ResolutionResult source)
    {
      return source.Value == null || source.Value is TSource ? source.New((object) this.ResolveCore((TSource) source.Value), typeof (TDestination)) : throw new AutoMapperMappingException(string.Format("Value supplied is of type {0} but expected {1}.\nChange the value resolver source type, or redirect the source value supplied to the value resolver using FromMember.", new object[2]
      {
        (object) source.Value.GetType(),
        (object) typeof (TSource)
      }));
    }

    protected abstract TDestination ResolveCore(TSource source);
  }
}
