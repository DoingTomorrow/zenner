// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Structures.TenantDataTemplateSelector
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Business.DTO;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Structures
{
  public class TenantDataTemplateSelector : DataTemplateSelector
  {
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      return container is FrameworkElement frameworkElement && item is StructureNodeDTO structureNodeDto && structureNodeDto.NodeType.Name == "Tenant" ? frameworkElement.FindResource((object) "TenantPositionTemplate") as DataTemplate : (DataTemplate) null;
    }
  }
}
