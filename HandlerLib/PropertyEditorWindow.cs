// Decompiled with JetBrains decompiler
// Type: HandlerLib.PropertyEditorWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class PropertyEditorWindow : Window, IComponentConnector
  {
    private object obj;
    internal Button ButtonSave;
    private bool _contentLoaded;

    public List<ParameterItem> Propertys { get; set; }

    public PropertyEditorWindow(Window owner, object obj)
    {
      this.InitializeComponent();
      this.Owner = owner;
      this.obj = obj;
      this.Propertys = new List<ParameterItem>();
      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
      {
        string name = property.Name;
        bool flag = property.Attributes.Contains((Attribute) ReadOnlyAttribute.Yes);
        object buffer = property.GetValue(obj);
        string str = string.Empty;
        if (buffer != null)
        {
          Type type = buffer.GetType();
          if (type == typeof (byte[]))
            str = Utility.ByteArrayToHexString((byte[]) buffer);
          else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Dictionary<,>))
          {
            Type genericArgument1 = type.GetGenericArguments()[0];
            Type genericArgument2 = type.GetGenericArguments()[1];
            IDictionary dictionary = buffer as IDictionary;
            IEnumerator enumerator = dictionary.Keys.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                object current = enumerator.Current;
                this.Propertys.Add(new ParameterItem()
                {
                  Key = current.ToString(),
                  Value = dictionary[current] != null ? dictionary[current].ToString() : "",
                  IsReadOnly = flag
                });
              }
              continue;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          }
          else
            str = buffer.ToString();
        }
        this.Propertys.Add(new ParameterItem()
        {
          Key = name,
          Value = str,
          IsReadOnly = flag
        });
      }
      this.DataContext = (object) this;
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
      foreach (ParameterItem property1 in this.Propertys)
      {
        if (!property1.IsReadOnly)
        {
          PropertyInfo property2 = this.obj.GetType().GetProperty(property1.Key);
          property2.SetValue(this.obj, Convert.ChangeType((object) property1.Value, property2.PropertyType), (object[]) null);
        }
      }
      this.DialogResult = new bool?(true);
      this.Close();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/propertyeditorwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        this.ButtonSave = (Button) target;
        this.ButtonSave.Click += new RoutedEventHandler(this.ButtonSave_Click);
      }
      else
        this._contentLoaded = true;
    }
  }
}
