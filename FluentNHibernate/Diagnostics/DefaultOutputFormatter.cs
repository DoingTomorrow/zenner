// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.DefaultOutputFormatter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class DefaultOutputFormatter : IDiagnosticResultsFormatter
  {
    public string Format(DiagnosticResults results)
    {
      StringBuilder sb = new StringBuilder();
      this.OutputFluentMappings(sb, results);
      sb.AppendLine();
      this.OutputConventions(sb, results);
      sb.AppendLine();
      this.OutputAutomappings(sb, results);
      return sb.ToString();
    }

    private void OutputAutomappings(StringBuilder sb, DiagnosticResults results)
    {
      this.Title(sb, "Automapping");
      sb.AppendLine();
      sb.AppendLine("Skipped types:");
      sb.AppendLine();
      SkippedAutomappingType[] array1 = results.AutomappingSkippedTypes.OrderBy<SkippedAutomappingType, string>((Func<SkippedAutomappingType, string>) (x => x.Type.Name)).ToArray<SkippedAutomappingType>();
      if (((IEnumerable<SkippedAutomappingType>) array1).Any<SkippedAutomappingType>())
        this.Table(sb, ((IEnumerable<SkippedAutomappingType>) array1).Select<SkippedAutomappingType, string>((Func<SkippedAutomappingType, string>) (x => x.Type.Name)), ((IEnumerable<SkippedAutomappingType>) array1).Select<SkippedAutomappingType, string>((Func<SkippedAutomappingType, string>) (x => x.Reason)), ((IEnumerable<SkippedAutomappingType>) array1).Select<SkippedAutomappingType, string>((Func<SkippedAutomappingType, string>) (x => x.Type.AssemblyQualifiedName)));
      else
        sb.AppendLine("  None found");
      sb.AppendLine();
      sb.AppendLine("Candidate types:");
      sb.AppendLine();
      Type[] array2 = results.AutomappingCandidateTypes.OrderBy<Type, string>((Func<Type, string>) (x => x.Name)).ToArray<Type>();
      if (((IEnumerable<Type>) array2).Any<Type>())
      {
        this.Table(sb, ((IEnumerable<Type>) array2).Select<Type, string>((Func<Type, string>) (x => x.Name)), ((IEnumerable<Type>) array2).Select<Type, string>((Func<Type, string>) (x => x.AssemblyQualifiedName)));
        sb.AppendLine();
        sb.AppendLine("Mapped types:");
        sb.AppendLine();
        IEnumerable<Type> source = results.AutomappedTypes.Select<AutomappingType, Type>((Func<AutomappingType, Type>) (x => x.Type));
        if (source.Any<Type>())
          this.Table(sb, source.Select<Type, string>((Func<Type, string>) (x => x.Name)), source.Select<Type, string>((Func<Type, string>) (x => x.AssemblyQualifiedName)));
        else
          sb.AppendLine("  None found");
      }
      else
        sb.AppendLine("  None found");
    }

    private void OutputConventions(StringBuilder sb, DiagnosticResults results)
    {
      this.Title(sb, "Conventions");
      sb.AppendLine();
      sb.AppendLine("Sources scanned:");
      sb.AppendLine();
      string[] array1 = results.ScannedSources.Where<ScannedSource>((Func<ScannedSource, bool>) (x => x.Phase == ScanPhase.Conventions)).OrderBy<ScannedSource, string>((Func<ScannedSource, string>) (x => x.Identifier)).Select<ScannedSource, string>((Func<ScannedSource, string>) (x => x.Identifier)).ToArray<string>();
      if (((IEnumerable<string>) array1).Any<string>())
      {
        this.List(sb, (IEnumerable<string>) array1);
        sb.AppendLine();
        sb.AppendLine("Conventions discovered:");
        sb.AppendLine();
        if (results.Conventions.Any<Type>())
        {
          Type[] array2 = results.Conventions.OrderBy<Type, string>((Func<Type, string>) (x => x.Name)).ToArray<Type>();
          this.Table(sb, ((IEnumerable<Type>) array2).Select<Type, string>((Func<Type, string>) (x => x.Name)), ((IEnumerable<Type>) array2).Select<Type, string>((Func<Type, string>) (x => x.AssemblyQualifiedName)));
        }
        else
          sb.AppendLine("  None found");
      }
      else
        sb.AppendLine("  None found");
    }

    private void OutputFluentMappings(StringBuilder sb, DiagnosticResults results)
    {
      this.Title(sb, "Fluent Mappings");
      sb.AppendLine();
      sb.AppendLine("Sources scanned:");
      sb.AppendLine();
      string[] array1 = results.ScannedSources.Where<ScannedSource>((Func<ScannedSource, bool>) (x => x.Phase == ScanPhase.FluentMappings)).OrderBy<ScannedSource, string>((Func<ScannedSource, string>) (x => x.Identifier)).Select<ScannedSource, string>((Func<ScannedSource, string>) (x => x.Identifier)).ToArray<string>();
      if (((IEnumerable<string>) array1).Any<string>())
      {
        this.List(sb, (IEnumerable<string>) array1);
        sb.AppendLine();
        sb.AppendLine("Mappings discovered:");
        sb.AppendLine();
        if (results.FluentMappings.Any<Type>())
        {
          Type[] array2 = results.FluentMappings.OrderBy<Type, string>((Func<Type, string>) (x => x.Name)).ToArray<Type>();
          this.Table(sb, ((IEnumerable<Type>) array2).Select<Type, string>((Func<Type, string>) (x => x.Name)), ((IEnumerable<Type>) array2).Select<Type, string>((Func<Type, string>) (x => x.AssemblyQualifiedName)));
        }
        else
          sb.AppendLine("  None found");
      }
      else
        sb.AppendLine("  None found");
    }

    private void Table(StringBuilder sb, params IEnumerable<string>[] columns)
    {
      int[] array = ((IEnumerable<IEnumerable<string>>) columns).Select<IEnumerable<string>, int>((Func<IEnumerable<string>, int>) (x => x.Max<string>((Func<string, int>) (val => val.Length)))).ToArray<int>();
      int num = ((IEnumerable<IEnumerable<string>>) columns).First<IEnumerable<string>>().Count<string>();
      for (int index1 = 0; index1 < num; ++index1)
      {
        sb.Append("  ");
        for (int index2 = 0; index2 < columns.Length; ++index2)
        {
          IEnumerable<string> column = columns[index2];
          int totalWidth = array[index2];
          string str = column.ElementAt<string>(index1);
          sb.Append(str.PadRight(totalWidth));
          sb.Append(" | ");
        }
        sb.Length -= 3;
        sb.AppendLine();
      }
    }

    private void List(StringBuilder sb, IEnumerable<string> items)
    {
      foreach (string str in items)
        sb.AppendLine("  " + str);
    }

    private void Title(StringBuilder sb, string title)
    {
      sb.AppendLine(title);
      sb.AppendLine("".PadLeft(title.Length, '-'));
    }
  }
}
