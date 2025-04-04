// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.CascadeStyle
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public abstract class CascadeStyle
  {
    internal static readonly Dictionary<string, CascadeStyle> Styles = new Dictionary<string, CascadeStyle>();
    public static readonly CascadeStyle AllDeleteOrphan = (CascadeStyle) new CascadeStyle.AllDeleteOrphanCascadeStyle();
    public static readonly CascadeStyle All = (CascadeStyle) new CascadeStyle.AllCascadeStyle();
    public static readonly CascadeStyle Update = (CascadeStyle) new CascadeStyle.UpdateCascadeStyle();
    public static readonly CascadeStyle Lock = (CascadeStyle) new CascadeStyle.LockCascadeStyle();
    public static readonly CascadeStyle Refresh = (CascadeStyle) new CascadeStyle.RefreshCascadeStyle();
    public static readonly CascadeStyle Evict = (CascadeStyle) new CascadeStyle.EvictCascadeStyle();
    public static readonly CascadeStyle Replicate = (CascadeStyle) new CascadeStyle.ReplicateCascadeStyle();
    public static readonly CascadeStyle Merge = (CascadeStyle) new CascadeStyle.MergeCascadeStyle();
    public static readonly CascadeStyle Persist = (CascadeStyle) new CascadeStyle.PersistCascadeStyle();
    public static readonly CascadeStyle Delete = (CascadeStyle) new CascadeStyle.DeleteCascadeStyle();
    public static readonly CascadeStyle DeleteOrphan = (CascadeStyle) new CascadeStyle.DeleteOrphanCascadeStyle();
    public static readonly CascadeStyle None = (CascadeStyle) new CascadeStyle.NoneCascadeStyle();

    internal CascadeStyle()
    {
    }

    static CascadeStyle()
    {
      CascadeStyle.Styles["all"] = CascadeStyle.All;
      CascadeStyle.Styles["all-delete-orphan"] = CascadeStyle.AllDeleteOrphan;
      CascadeStyle.Styles["save-update"] = CascadeStyle.Update;
      CascadeStyle.Styles["persist"] = CascadeStyle.Persist;
      CascadeStyle.Styles["merge"] = CascadeStyle.Merge;
      CascadeStyle.Styles["lock"] = CascadeStyle.Lock;
      CascadeStyle.Styles["refresh"] = CascadeStyle.Refresh;
      CascadeStyle.Styles["replicate"] = CascadeStyle.Replicate;
      CascadeStyle.Styles["evict"] = CascadeStyle.Evict;
      CascadeStyle.Styles["delete"] = CascadeStyle.Delete;
      CascadeStyle.Styles["remove"] = CascadeStyle.Delete;
      CascadeStyle.Styles["delete-orphan"] = CascadeStyle.DeleteOrphan;
      CascadeStyle.Styles["none"] = CascadeStyle.None;
    }

    public abstract bool DoCascade(CascadingAction action);

    public virtual bool ReallyDoCascade(CascadingAction action) => this.DoCascade(action);

    public virtual bool HasOrphanDelete => false;

    public static CascadeStyle GetCascadeStyle(string cascade)
    {
      CascadeStyle cascadeStyle;
      if (!CascadeStyle.Styles.TryGetValue(cascade, out cascadeStyle))
        throw new MappingException("Unsupported cascade style: " + cascade);
      return cascadeStyle;
    }

    [Serializable]
    private class AllDeleteOrphanCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => true;

      public override bool HasOrphanDelete => true;
    }

    [Serializable]
    private class AllCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => true;
    }

    [Serializable]
    private class UpdateCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action)
      {
        return action == CascadingAction.SaveUpdate || action == CascadingAction.SaveUpdateCopy;
      }
    }

    [Serializable]
    private class LockCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => action == CascadingAction.Lock;
    }

    [Serializable]
    private class RefreshCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => action == CascadingAction.Refresh;
    }

    [Serializable]
    private class EvictCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => action == CascadingAction.Evict;
    }

    [Serializable]
    private class ReplicateCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => action == CascadingAction.Replicate;
    }

    [Serializable]
    private class MergeCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action)
      {
        return action == CascadingAction.Merge || action == CascadingAction.SaveUpdateCopy;
      }
    }

    [Serializable]
    private class PersistCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action)
      {
        return action == CascadingAction.Persist || action == CascadingAction.PersistOnFlush;
      }
    }

    [Serializable]
    private class DeleteCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => action == CascadingAction.Delete;
    }

    [Serializable]
    private class DeleteOrphanCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action)
      {
        return action == CascadingAction.Delete || action == CascadingAction.SaveUpdate;
      }

      public override bool ReallyDoCascade(CascadingAction action)
      {
        return action == CascadingAction.Delete;
      }

      public override bool HasOrphanDelete => true;
    }

    [Serializable]
    private class NoneCascadeStyle : CascadeStyle
    {
      public override bool DoCascade(CascadingAction action) => false;
    }

    [Serializable]
    public sealed class MultipleCascadeStyle : CascadeStyle
    {
      private readonly CascadeStyle[] styles;

      public MultipleCascadeStyle(CascadeStyle[] styles) => this.styles = styles;

      public override bool DoCascade(CascadingAction action)
      {
        for (int index = 0; index < this.styles.Length; ++index)
        {
          if (this.styles[index].DoCascade(action))
            return true;
        }
        return false;
      }

      public override bool ReallyDoCascade(CascadingAction action)
      {
        for (int index = 0; index < this.styles.Length; ++index)
        {
          if (this.styles[index].ReallyDoCascade(action))
            return true;
        }
        return false;
      }

      public override bool HasOrphanDelete
      {
        get
        {
          for (int index = 0; index < this.styles.Length; ++index)
          {
            if (this.styles[index].HasOrphanDelete)
              return true;
          }
          return false;
        }
      }

      public override string ToString() => ArrayHelper.ToString((object[]) this.styles);
    }
  }
}
