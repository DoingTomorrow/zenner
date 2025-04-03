// Decompiled with JetBrains decompiler
// Type: AutoMapper.TypeConverter`2
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

#nullable disable
namespace AutoMapper
{
  public abstract class TypeConverter<TSource, TDestination> : ITypeConverter<TSource, TDestination>
  {
    public TDestination Convert(ResolutionContext context)
    {
      return context.SourceValue == null || context.SourceValue is TSource ? this.ConvertCore((TSource) context.SourceValue) : throw new AutoMapperMappingException(context, string.Format("Value supplied is of type {0} but expected {1}.\nChange the type converter source type, or redirect the source value supplied to the value resolver using FromMember.", new object[2]
      {
        (object) typeof (TSource),
        (object) context.SourceValue.GetType()
      }));
    }

    protected abstract TDestination ConvertCore(TSource source);
  }
}
