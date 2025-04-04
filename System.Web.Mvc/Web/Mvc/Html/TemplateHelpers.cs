// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.TemplateHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.Web.UI.WebControls;

#nullable disable
namespace System.Web.Mvc.Html
{
  internal static class TemplateHelpers
  {
    private static readonly Dictionary<DataBoundControlMode, string> _modeViewPaths = new Dictionary<DataBoundControlMode, string>()
    {
      {
        DataBoundControlMode.ReadOnly,
        "DisplayTemplates"
      },
      {
        DataBoundControlMode.Edit,
        "EditorTemplates"
      }
    };
    private static readonly Dictionary<string, Func<HtmlHelper, string>> _defaultDisplayActions = new Dictionary<string, Func<HtmlHelper, string>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "EmailAddress",
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.EmailAddressTemplate)
      },
      {
        "HiddenInput",
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.HiddenInputTemplate)
      },
      {
        "Html",
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.HtmlTemplate)
      },
      {
        "Text",
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.StringTemplate)
      },
      {
        "Url",
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.UrlTemplate)
      },
      {
        "Collection",
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.CollectionTemplate)
      },
      {
        typeof (bool).Name,
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.BooleanTemplate)
      },
      {
        typeof (Decimal).Name,
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.DecimalTemplate)
      },
      {
        typeof (string).Name,
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.StringTemplate)
      },
      {
        typeof (object).Name,
        new Func<HtmlHelper, string>(DefaultDisplayTemplates.ObjectTemplate)
      }
    };
    private static readonly Dictionary<string, Func<HtmlHelper, string>> _defaultEditorActions = new Dictionary<string, Func<HtmlHelper, string>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "HiddenInput",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.HiddenInputTemplate)
      },
      {
        "MultilineText",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.MultilineTextTemplate)
      },
      {
        "Password",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.PasswordTemplate)
      },
      {
        "Text",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.StringTemplate)
      },
      {
        "Collection",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.CollectionTemplate)
      },
      {
        "PhoneNumber",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.PhoneNumberInputTemplate)
      },
      {
        "Url",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.UrlInputTemplate)
      },
      {
        "EmailAddress",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.EmailAddressInputTemplate)
      },
      {
        "DateTime",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.DateTimeInputTemplate)
      },
      {
        "Date",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.DateInputTemplate)
      },
      {
        "Time",
        new Func<HtmlHelper, string>(DefaultEditorTemplates.TimeInputTemplate)
      },
      {
        typeof (byte).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.NumberInputTemplate)
      },
      {
        typeof (sbyte).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.NumberInputTemplate)
      },
      {
        typeof (int).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.NumberInputTemplate)
      },
      {
        typeof (uint).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.NumberInputTemplate)
      },
      {
        typeof (long).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.NumberInputTemplate)
      },
      {
        typeof (ulong).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.NumberInputTemplate)
      },
      {
        typeof (bool).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.BooleanTemplate)
      },
      {
        typeof (Decimal).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.DecimalTemplate)
      },
      {
        typeof (string).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.StringTemplate)
      },
      {
        typeof (object).Name,
        new Func<HtmlHelper, string>(DefaultEditorTemplates.ObjectTemplate)
      }
    };
    internal static string CacheItemId = Guid.NewGuid().ToString();

    internal static string ExecuteTemplate(
      HtmlHelper html,
      ViewDataDictionary viewData,
      string templateName,
      DataBoundControlMode mode,
      TemplateHelpers.GetViewNamesDelegate getViewNames,
      TemplateHelpers.GetDefaultActionsDelegate getDefaultActions)
    {
      Dictionary<string, TemplateHelpers.ActionCacheItem> actionCache = TemplateHelpers.GetActionCache(html);
      Dictionary<string, Func<HtmlHelper, string>> dictionary = getDefaultActions(mode);
      string modeViewPath = TemplateHelpers._modeViewPaths[mode];
      foreach (string key in getViewNames(viewData.ModelMetadata, templateName, viewData.ModelMetadata.TemplateHint, viewData.ModelMetadata.DataTypeName))
      {
        string str = modeViewPath + "/" + key;
        TemplateHelpers.ActionCacheItem actionCacheItem;
        if (actionCache.TryGetValue(str, out actionCacheItem))
        {
          if (actionCacheItem != null)
            return actionCacheItem.Execute(html, viewData);
        }
        else
        {
          ViewEngineResult partialView = ViewEngines.Engines.FindPartialView((ControllerContext) html.ViewContext, str);
          if (partialView.View != null)
          {
            actionCache[str] = (TemplateHelpers.ActionCacheItem) new TemplateHelpers.ActionCacheViewItem()
            {
              ViewName = str
            };
            using (StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
            {
              partialView.View.Render(new ViewContext((ControllerContext) html.ViewContext, partialView.View, viewData, html.ViewContext.TempData, (TextWriter) writer), (TextWriter) writer);
              return writer.ToString();
            }
          }
          else
          {
            Func<HtmlHelper, string> func;
            if (dictionary.TryGetValue(key, out func))
            {
              actionCache[str] = (TemplateHelpers.ActionCacheItem) new TemplateHelpers.ActionCacheCodeItem()
              {
                Action = func
              };
              return func(TemplateHelpers.MakeHtmlHelper(html, viewData));
            }
            actionCache[str] = (TemplateHelpers.ActionCacheItem) null;
          }
        }
      }
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.TemplateHelpers_NoTemplate, new object[1]
      {
        (object) viewData.ModelMetadata.RealModelType.FullName
      }));
    }

    internal static Dictionary<string, TemplateHelpers.ActionCacheItem> GetActionCache(
      HtmlHelper html)
    {
      HttpContextBase httpContext = html.ViewContext.HttpContext;
      Dictionary<string, TemplateHelpers.ActionCacheItem> actionCache;
      if (!httpContext.Items.Contains((object) TemplateHelpers.CacheItemId))
      {
        actionCache = new Dictionary<string, TemplateHelpers.ActionCacheItem>();
        httpContext.Items[(object) TemplateHelpers.CacheItemId] = (object) actionCache;
      }
      else
        actionCache = (Dictionary<string, TemplateHelpers.ActionCacheItem>) httpContext.Items[(object) TemplateHelpers.CacheItemId];
      return actionCache;
    }

    internal static Dictionary<string, Func<HtmlHelper, string>> GetDefaultActions(
      DataBoundControlMode mode)
    {
      return mode != DataBoundControlMode.ReadOnly ? TemplateHelpers._defaultEditorActions : TemplateHelpers._defaultDisplayActions;
    }

    internal static IEnumerable<string> GetViewNames(
      ModelMetadata metadata,
      params string[] templateHints)
    {
      foreach (string templateHint in ((IEnumerable<string>) templateHints).Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))))
        yield return templateHint;
      Type type = Nullable.GetUnderlyingType(metadata.RealModelType);
      if ((object) type == null)
        type = metadata.RealModelType;
      Type fieldType = type;
      yield return fieldType.Name;
      if (!metadata.IsComplexType)
        yield return "String";
      else if (fieldType.IsInterface)
      {
        if (typeof (IEnumerable).IsAssignableFrom(fieldType))
          yield return "Collection";
        yield return "Object";
      }
      else
      {
        bool isEnumerable = typeof (IEnumerable).IsAssignableFrom(fieldType);
        while (true)
        {
          fieldType = fieldType.BaseType;
          if (!(fieldType == (Type) null))
          {
            if (isEnumerable && fieldType == typeof (object))
              yield return "Collection";
            yield return fieldType.Name;
          }
          else
            break;
        }
      }
    }

    internal static MvcHtmlString Template(
      HtmlHelper html,
      string expression,
      string templateName,
      string htmlFieldName,
      DataBoundControlMode mode,
      object additionalViewData)
    {
      return MvcHtmlString.Create(TemplateHelpers.Template(html, expression, templateName, htmlFieldName, mode, additionalViewData, new TemplateHelpers.TemplateHelperDelegate(TemplateHelpers.TemplateHelper)));
    }

    internal static string Template(
      HtmlHelper html,
      string expression,
      string templateName,
      string htmlFieldName,
      DataBoundControlMode mode,
      object additionalViewData,
      TemplateHelpers.TemplateHelperDelegate templateHelper)
    {
      return templateHelper(html, ModelMetadata.FromStringExpression(expression, html.ViewData), htmlFieldName ?? ExpressionHelper.GetExpressionText(expression), templateName, mode, additionalViewData);
    }

    internal static MvcHtmlString TemplateFor<TContainer, TValue>(
      this HtmlHelper<TContainer> html,
      Expression<Func<TContainer, TValue>> expression,
      string templateName,
      string htmlFieldName,
      DataBoundControlMode mode,
      object additionalViewData)
    {
      return MvcHtmlString.Create(html.TemplateFor<TContainer, TValue>(expression, templateName, htmlFieldName, mode, additionalViewData, new TemplateHelpers.TemplateHelperDelegate(TemplateHelpers.TemplateHelper)));
    }

    internal static string TemplateFor<TContainer, TValue>(
      this HtmlHelper<TContainer> html,
      Expression<Func<TContainer, TValue>> expression,
      string templateName,
      string htmlFieldName,
      DataBoundControlMode mode,
      object additionalViewData,
      TemplateHelpers.TemplateHelperDelegate templateHelper)
    {
      return templateHelper((HtmlHelper) html, ModelMetadata.FromLambdaExpression<TContainer, TValue>(expression, html.ViewData), htmlFieldName ?? ExpressionHelper.GetExpressionText((LambdaExpression) expression), templateName, mode, additionalViewData);
    }

    internal static string TemplateHelper(
      HtmlHelper html,
      ModelMetadata metadata,
      string htmlFieldName,
      string templateName,
      DataBoundControlMode mode,
      object additionalViewData)
    {
      return TemplateHelpers.TemplateHelper(html, metadata, htmlFieldName, templateName, mode, additionalViewData, new TemplateHelpers.ExecuteTemplateDelegate(TemplateHelpers.ExecuteTemplate));
    }

    internal static string TemplateHelper(
      HtmlHelper html,
      ModelMetadata metadata,
      string htmlFieldName,
      string templateName,
      DataBoundControlMode mode,
      object additionalViewData,
      TemplateHelpers.ExecuteTemplateDelegate executeTemplate)
    {
      if (metadata.ConvertEmptyStringToNull && string.Empty.Equals(metadata.Model))
        metadata.Model = (object) null;
      object obj1 = metadata.Model;
      if (metadata.Model == null && mode == DataBoundControlMode.ReadOnly)
        obj1 = (object) metadata.NullDisplayText;
      string format = mode == DataBoundControlMode.ReadOnly ? metadata.DisplayFormatString : metadata.EditFormatString;
      if (metadata.Model != null && !string.IsNullOrEmpty(format))
        obj1 = (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, new object[1]
        {
          metadata.Model
        });
      object obj2 = metadata.Model ?? (object) metadata.RealModelType;
      if (html.ViewDataContainer.ViewData.TemplateInfo.VisitedObjects.Contains(obj2))
        return string.Empty;
      ViewDataDictionary viewData = new ViewDataDictionary(html.ViewDataContainer.ViewData)
      {
        Model = metadata.Model,
        ModelMetadata = metadata,
        TemplateInfo = new TemplateInfo()
        {
          FormattedModelValue = obj1,
          HtmlFieldPrefix = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName),
          VisitedObjects = new HashSet<object>((IEnumerable<object>) html.ViewContext.ViewData.TemplateInfo.VisitedObjects)
        }
      };
      if (additionalViewData != null)
      {
        foreach (KeyValuePair<string, object> keyValuePair in new RouteValueDictionary(additionalViewData))
          viewData[keyValuePair.Key] = keyValuePair.Value;
      }
      viewData.TemplateInfo.VisitedObjects.Add(obj2);
      return executeTemplate(html, viewData, templateName, mode, new TemplateHelpers.GetViewNamesDelegate(TemplateHelpers.GetViewNames), new TemplateHelpers.GetDefaultActionsDelegate(TemplateHelpers.GetDefaultActions));
    }

    private static HtmlHelper MakeHtmlHelper(HtmlHelper html, ViewDataDictionary viewData)
    {
      return new HtmlHelper(new ViewContext((ControllerContext) html.ViewContext, html.ViewContext.View, viewData, html.ViewContext.TempData, html.ViewContext.Writer), (IViewDataContainer) new TemplateHelpers.ViewDataContainer(viewData));
    }

    internal delegate string ExecuteTemplateDelegate(
      HtmlHelper html,
      ViewDataDictionary viewData,
      string templateName,
      DataBoundControlMode mode,
      TemplateHelpers.GetViewNamesDelegate getViewNames,
      TemplateHelpers.GetDefaultActionsDelegate getDefaultActions);

    internal delegate Dictionary<string, Func<HtmlHelper, string>> GetDefaultActionsDelegate(
      DataBoundControlMode mode);

    internal delegate IEnumerable<string> GetViewNamesDelegate(
      ModelMetadata metadata,
      params string[] templateHints);

    internal delegate string TemplateHelperDelegate(
      HtmlHelper html,
      ModelMetadata metadata,
      string htmlFieldName,
      string templateName,
      DataBoundControlMode mode,
      object additionalViewData);

    internal abstract class ActionCacheItem
    {
      public abstract string Execute(HtmlHelper html, ViewDataDictionary viewData);
    }

    internal class ActionCacheCodeItem : TemplateHelpers.ActionCacheItem
    {
      public Func<HtmlHelper, string> Action { get; set; }

      public override string Execute(HtmlHelper html, ViewDataDictionary viewData)
      {
        return this.Action(TemplateHelpers.MakeHtmlHelper(html, viewData));
      }
    }

    internal class ActionCacheViewItem : TemplateHelpers.ActionCacheItem
    {
      public string ViewName { get; set; }

      public override string Execute(HtmlHelper html, ViewDataDictionary viewData)
      {
        ViewEngineResult partialView = ViewEngines.Engines.FindPartialView((ControllerContext) html.ViewContext, this.ViewName);
        using (StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
        {
          partialView.View.Render(new ViewContext((ControllerContext) html.ViewContext, partialView.View, viewData, html.ViewContext.TempData, (TextWriter) writer), (TextWriter) writer);
          return writer.ToString();
        }
      }
    }

    private class ViewDataContainer : IViewDataContainer
    {
      public ViewDataContainer(ViewDataDictionary viewData) => this.ViewData = viewData;

      public ViewDataDictionary ViewData { get; set; }
    }
  }
}
