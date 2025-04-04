// Decompiled with JetBrains decompiler
// Type: Fluent.ColorGradientItemTemplateSelector
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public class ColorGradientItemTemplateSelector : DataTemplateSelector
  {
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      listBox = (ListBox) null;
      DependencyObject reference = container;
      while (reference != null)
      {
        reference = VisualTreeHelper.GetParent(reference);
        if (reference is ListBox listBox)
          break;
      }
      if (listBox != null)
      {
        colorGallery = (ColorGallery) null;
        while (reference != null)
        {
          reference = VisualTreeHelper.GetParent(reference);
          if (reference is ColorGallery colorGallery)
            break;
        }
        if (colorGallery != null)
        {
          int num = listBox.Items.IndexOf(item);
          if (num < colorGallery.Columns)
            return listBox.TryFindResource((object) "GradientColorTopDataTemplate") as DataTemplate;
          return num >= listBox.Items.Count - colorGallery.Columns ? listBox.TryFindResource((object) "GradientColorBottomDataTemplate") as DataTemplate : listBox.TryFindResource((object) "GradientColorCenterDataTemplate") as DataTemplate;
        }
      }
      return (DataTemplate) null;
    }
  }
}
