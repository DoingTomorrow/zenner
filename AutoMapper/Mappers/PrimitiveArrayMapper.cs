// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.PrimitiveArrayMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper.Mappers
{
  public class PrimitiveArrayMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      if (context.IsSourceValueNull && mapper.ShouldMapSourceCollectionAsNull(context))
        return (object) null;
      Type elementType1 = TypeHelper.GetElementType(context.SourceType);
      Type elementType2 = TypeHelper.GetElementType(context.DestinationType);
      Array sourceArray = (Array) context.SourceValue ?? ObjectCreator.CreateArray(elementType1, 0);
      int length = sourceArray.Length;
      Array array = ObjectCreator.CreateArray(elementType2, length);
      Array.Copy(sourceArray, array, length);
      return (object) array;
    }

    private bool IsPrimitiveArrayType(Type type)
    {
      if (!type.IsArray)
        return false;
      Type elementType = TypeHelper.GetElementType(type);
      return elementType.IsPrimitive || elementType.Equals(typeof (string));
    }

    public bool IsMatch(ResolutionContext context)
    {
      return this.IsPrimitiveArrayType(context.DestinationType) && this.IsPrimitiveArrayType(context.SourceType) && TypeHelper.GetElementType(context.DestinationType).Equals(TypeHelper.GetElementType(context.SourceType));
    }
  }
}
