// Decompiled with JetBrains decompiler
// Type: AutoMapper.QueryableExtensions.ProjectionExpression`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.Linq;

#nullable disable
namespace AutoMapper.QueryableExtensions
{
  public class ProjectionExpression<TSource> : IProjectionExpression
  {
    private readonly IQueryable<TSource> _source;
    private readonly IMappingEngine _mappingEngine;

    public ProjectionExpression(IQueryable<TSource> source, IMappingEngine mappingEngine)
    {
      this._source = source;
      this._mappingEngine = mappingEngine;
    }

    public IQueryable<TResult> To<TResult>()
    {
      return this._source.Select<TSource, TResult>(this._mappingEngine.CreateMapExpression<TSource, TResult>());
    }
  }
}
