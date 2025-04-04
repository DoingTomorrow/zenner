// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.TreeHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class TreeHelper
  {
    public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
    {
      DependencyObject parentObject = child.GetParentObject();
      if (parentObject == null)
        return default (T);
      return parentObject is T obj ? obj : parentObject.TryFindParent<T>();
    }

    public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
    {
      if (parent == null)
        return default (T);
      T child1 = default (T);
      int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child2 = VisualTreeHelper.GetChild(parent, childIndex);
        if ((object) (child2 as T) == null)
        {
          child1 = child2.FindChild<T>(childName);
          if ((object) child1 != null)
            break;
        }
        else if (!string.IsNullOrEmpty(childName))
        {
          if (child2 is FrameworkElement frameworkElement && frameworkElement.Name == childName)
          {
            child1 = (T) child2;
            break;
          }
        }
        else
        {
          child1 = (T) child2;
          break;
        }
      }
      return child1;
    }

    public static DependencyObject GetParentObject(this DependencyObject child)
    {
      switch (child)
      {
        case null:
          return (DependencyObject) null;
        case ContentElement reference:
          DependencyObject parent1 = ContentOperations.GetParent(reference);
          if (parent1 != null)
            return parent1;
          return !(reference is FrameworkContentElement frameworkContentElement) ? (DependencyObject) null : frameworkContentElement.Parent;
        case FrameworkElement frameworkElement:
          DependencyObject parent2 = frameworkElement.Parent;
          if (parent2 != null)
            return parent2;
          break;
      }
      return VisualTreeHelper.GetParent(child);
    }

    public static IEnumerable<T> FindChildren<T>(
      this DependencyObject source,
      bool forceUsingTheVisualTreeHelper = false)
      where T : DependencyObject
    {
      if (source != null)
      {
        foreach (DependencyObject child1 in source.GetChildObjects(forceUsingTheVisualTreeHelper))
        {
          if (child1 != null && child1 is T)
            yield return (T) child1;
          foreach (T child2 in child1.FindChildren<T>())
            yield return child2;
        }
      }
    }

    public static IEnumerable<DependencyObject> GetChildObjects(
      this DependencyObject parent,
      bool forceUsingTheVisualTreeHelper = false)
    {
      if (parent != null)
      {
        if (!forceUsingTheVisualTreeHelper && (parent is ContentElement || parent is FrameworkElement))
        {
          foreach (object child in LogicalTreeHelper.GetChildren(parent))
          {
            if (child is DependencyObject)
              yield return (DependencyObject) child;
          }
        }
        else
        {
          int count = VisualTreeHelper.GetChildrenCount(parent);
          for (int i = 0; i < count; ++i)
            yield return VisualTreeHelper.GetChild(parent, i);
        }
      }
    }

    public static T TryFindFromPoint<T>(UIElement reference, Point point) where T : DependencyObject
    {
      if (!(reference.InputHitTest(point) is DependencyObject child))
        return default (T);
      return child is T obj ? obj : child.TryFindParent<T>();
    }
  }
}
