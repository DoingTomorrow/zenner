// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IColumnInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IColumnInspector : IInspector
  {
    string Name { get; }

    string Check { get; }

    string Index { get; }

    int Length { get; }

    bool NotNull { get; }

    string SqlType { get; }

    bool Unique { get; }

    string UniqueKey { get; }

    int Precision { get; }

    int Scale { get; }

    string Default { get; }
  }
}
