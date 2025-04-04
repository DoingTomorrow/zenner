// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.ExpressionSerializer
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  public class ExpressionSerializer
  {
    private static readonly Type[] primitiveTypes = new Type[4]
    {
      typeof (string),
      typeof (int),
      typeof (bool),
      typeof (ExpressionType)
    };
    private Dictionary<string, ParameterExpression> parameters = new Dictionary<string, ParameterExpression>();
    private TypeResolver resolver;

    public XElement GenerateXmlFromExpressionCore(Expression e)
    {
      if (e == null)
        return (XElement) null;
      XElement result;
      if (this.TryCustomSerializers(e, out result))
        return result;
      switch (e)
      {
        case BinaryExpression _:
          return this.BinaryExpressionToXElement((BinaryExpression) e);
        case BlockExpression _:
          return this.BlockExpressionToXElement((BlockExpression) e);
        case ConditionalExpression _:
          return this.ConditionalExpressionToXElement((ConditionalExpression) e);
        case ConstantExpression _:
          return this.ConstantExpressionToXElement((ConstantExpression) e);
        case DebugInfoExpression _:
          return this.DebugInfoExpressionToXElement((DebugInfoExpression) e);
        case DefaultExpression _:
          return this.DefaultExpressionToXElement((DefaultExpression) e);
        case DynamicExpression _:
          return this.DynamicExpressionToXElement((DynamicExpression) e);
        case GotoExpression _:
          return this.GotoExpressionToXElement((GotoExpression) e);
        case IndexExpression _:
          return this.IndexExpressionToXElement((IndexExpression) e);
        case InvocationExpression _:
          return this.InvocationExpressionToXElement((InvocationExpression) e);
        case LabelExpression _:
          return this.LabelExpressionToXElement((LabelExpression) e);
        case LambdaExpression _:
          return this.LambdaExpressionToXElement((LambdaExpression) e);
        case ListInitExpression _:
          return this.ListInitExpressionToXElement((ListInitExpression) e);
        case LoopExpression _:
          return this.LoopExpressionToXElement((LoopExpression) e);
        case MemberExpression _:
          return this.MemberExpressionToXElement((MemberExpression) e);
        case MemberInitExpression _:
          return this.MemberInitExpressionToXElement((MemberInitExpression) e);
        case MethodCallExpression _:
          return this.MethodCallExpressionToXElement((MethodCallExpression) e);
        case NewArrayExpression _:
          return this.NewArrayExpressionToXElement((NewArrayExpression) e);
        case NewExpression _:
          return this.NewExpressionToXElement((NewExpression) e);
        case ParameterExpression _:
          return this.ParameterExpressionToXElement((ParameterExpression) e);
        case RuntimeVariablesExpression _:
          return this.RuntimeVariablesExpressionToXElement((RuntimeVariablesExpression) e);
        case SwitchExpression _:
          return this.SwitchExpressionToXElement((SwitchExpression) e);
        case TryExpression _:
          return this.TryExpressionToXElement((TryExpression) e);
        case TypeBinaryExpression _:
          return this.TypeBinaryExpressionToXElement((TypeBinaryExpression) e);
        case UnaryExpression _:
          return this.UnaryExpressionToXElement((UnaryExpression) e);
        default:
          return (XElement) null;
      }
    }

    internal XElement BinaryExpressionToXElement(BinaryExpression e)
    {
      string name = "BinaryExpression";
      object[] objArray = new object[9];
      object canReduce = (object) e.CanReduce;
      objArray[0] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      object right = (object) e.Right;
      objArray[1] = this.GenerateXmlFromProperty(typeof (Expression), "Right", right ?? (object) string.Empty);
      object left = (object) e.Left;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Left", left ?? (object) string.Empty);
      object method = (object) e.Method;
      objArray[3] = this.GenerateXmlFromProperty(typeof (MethodInfo), "Method", method ?? (object) string.Empty);
      object conversion = (object) e.Conversion;
      objArray[4] = this.GenerateXmlFromProperty(typeof (LambdaExpression), "Conversion", conversion ?? (object) string.Empty);
      object isLifted = (object) e.IsLifted;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "IsLifted", isLifted ?? (object) string.Empty);
      object isLiftedToNull = (object) e.IsLiftedToNull;
      objArray[6] = this.GenerateXmlFromProperty(typeof (bool), "IsLiftedToNull", isLiftedToNull ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[7] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object type = (object) e.Type;
      objArray[8] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement BlockExpressionToXElement(BlockExpression e)
    {
      string name = "BlockExpression";
      object[] objArray = new object[6];
      object expressions = (object) e.Expressions;
      objArray[0] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<Expression>), "Expressions", expressions ?? (object) string.Empty);
      object variables = (object) e.Variables;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<ParameterExpression>), "Variables", variables ?? (object) string.Empty);
      object result = (object) e.Result;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Result", result ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[3] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object type = (object) e.Type;
      objArray[4] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement ConditionalExpressionToXElement(ConditionalExpression e)
    {
      string name = "ConditionalExpression";
      object[] objArray = new object[6];
      object nodeType = (object) e.NodeType;
      objArray[0] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object type = (object) e.Type;
      objArray[1] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object test = (object) e.Test;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Test", test ?? (object) string.Empty);
      object ifTrue = (object) e.IfTrue;
      objArray[3] = this.GenerateXmlFromProperty(typeof (Expression), "IfTrue", ifTrue ?? (object) string.Empty);
      object ifFalse = (object) e.IfFalse;
      objArray[4] = this.GenerateXmlFromProperty(typeof (Expression), "IfFalse", ifFalse ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement ConstantExpressionToXElement(ConstantExpression e)
    {
      string name = "ConstantExpression";
      object[] objArray = new object[4];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object obj = e.Value;
      objArray[2] = this.GenerateXmlFromProperty(typeof (object), "Value", obj ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[3] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement DebugInfoExpressionToXElement(DebugInfoExpression e)
    {
      string name = "DebugInfoExpression";
      object[] objArray = new object[9];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object startLine = (object) e.StartLine;
      objArray[2] = this.GenerateXmlFromProperty(typeof (int), "StartLine", startLine ?? (object) string.Empty);
      object startColumn = (object) e.StartColumn;
      objArray[3] = this.GenerateXmlFromProperty(typeof (int), "StartColumn", startColumn ?? (object) string.Empty);
      object endLine = (object) e.EndLine;
      objArray[4] = this.GenerateXmlFromProperty(typeof (int), "EndLine", endLine ?? (object) string.Empty);
      object endColumn = (object) e.EndColumn;
      objArray[5] = this.GenerateXmlFromProperty(typeof (int), "EndColumn", endColumn ?? (object) string.Empty);
      object document = (object) e.Document;
      objArray[6] = this.GenerateXmlFromProperty(typeof (SymbolDocumentInfo), "Document", document ?? (object) string.Empty);
      object isClear = (object) e.IsClear;
      objArray[7] = this.GenerateXmlFromProperty(typeof (bool), "IsClear", isClear ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[8] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement DefaultExpressionToXElement(DefaultExpression e)
    {
      string name = "DefaultExpression";
      object[] objArray = new object[3];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[2] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement DynamicExpressionToXElement(DynamicExpression e)
    {
      string name = "DynamicExpression";
      object[] objArray = new object[6];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object binder = (object) e.Binder;
      objArray[2] = this.GenerateXmlFromProperty(typeof (CallSiteBinder), "Binder", binder ?? (object) string.Empty);
      object delegateType = (object) e.DelegateType;
      objArray[3] = this.GenerateXmlFromProperty(typeof (Type), "DelegateType", delegateType ?? (object) string.Empty);
      object arguments = (object) e.Arguments;
      objArray[4] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<Expression>), "Arguments", arguments ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement GotoExpressionToXElement(GotoExpression e)
    {
      string name = "GotoExpression";
      object[] objArray = new object[6];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object obj = (object) e.Value;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Value", obj ?? (object) string.Empty);
      object target = (object) e.Target;
      objArray[3] = this.GenerateXmlFromProperty(typeof (LabelTarget), "Target", target ?? (object) string.Empty);
      object kind = (object) e.Kind;
      objArray[4] = this.GenerateXmlFromProperty(typeof (GotoExpressionKind), "Kind", kind ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement IndexExpressionToXElement(IndexExpression e)
    {
      string name = "IndexExpression";
      object[] objArray = new object[6];
      object nodeType = (object) e.NodeType;
      objArray[0] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object type = (object) e.Type;
      objArray[1] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object obj = (object) e.Object;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Object", obj ?? (object) string.Empty);
      object indexer = (object) e.Indexer;
      objArray[3] = this.GenerateXmlFromProperty(typeof (PropertyInfo), "Indexer", indexer ?? (object) string.Empty);
      object arguments = (object) e.Arguments;
      objArray[4] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<Expression>), "Arguments", arguments ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement InvocationExpressionToXElement(InvocationExpression e)
    {
      string name = "InvocationExpression";
      object[] objArray = new object[5];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object expression = (object) e.Expression;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Expression", expression ?? (object) string.Empty);
      object arguments = (object) e.Arguments;
      objArray[3] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<Expression>), "Arguments", arguments ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[4] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement LabelExpressionToXElement(LabelExpression e)
    {
      string name = "LabelExpression";
      object[] objArray = new object[5];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object target = (object) e.Target;
      objArray[2] = this.GenerateXmlFromProperty(typeof (LabelTarget), "Target", target ?? (object) string.Empty);
      object defaultValue = (object) e.DefaultValue;
      objArray[3] = this.GenerateXmlFromProperty(typeof (Expression), "DefaultValue", defaultValue ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[4] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement LambdaExpressionToXElement(LambdaExpression e)
    {
      string name1 = "LambdaExpression";
      object[] objArray = new object[8];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object parameters = (object) e.Parameters;
      objArray[2] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<ParameterExpression>), "Parameters", parameters ?? (object) string.Empty);
      object name2 = (object) e.Name;
      objArray[3] = this.GenerateXmlFromProperty(typeof (string), "Name", name2 ?? (object) string.Empty);
      object body = (object) e.Body;
      objArray[4] = this.GenerateXmlFromProperty(typeof (Expression), "Body", body ?? (object) string.Empty);
      object returnType = (object) e.ReturnType;
      objArray[5] = this.GenerateXmlFromProperty(typeof (Type), "ReturnType", returnType ?? (object) string.Empty);
      object tailCall = (object) e.TailCall;
      objArray[6] = this.GenerateXmlFromProperty(typeof (bool), "TailCall", tailCall ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[7] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name1, objArray);
    }

    internal XElement ListInitExpressionToXElement(ListInitExpression e)
    {
      string name = "ListInitExpression";
      object[] objArray = new object[5];
      object nodeType = (object) e.NodeType;
      objArray[0] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object type = (object) e.Type;
      objArray[1] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[2] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      object newExpression = (object) e.NewExpression;
      objArray[3] = this.GenerateXmlFromProperty(typeof (NewExpression), "NewExpression", newExpression ?? (object) string.Empty);
      object initializers = (object) e.Initializers;
      objArray[4] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<ElementInit>), "Initializers", initializers ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement LoopExpressionToXElement(LoopExpression e)
    {
      string name = "LoopExpression";
      object[] objArray = new object[6];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object body = (object) e.Body;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Body", body ?? (object) string.Empty);
      object breakLabel = (object) e.BreakLabel;
      objArray[3] = this.GenerateXmlFromProperty(typeof (LabelTarget), "BreakLabel", breakLabel ?? (object) string.Empty);
      object continueLabel = (object) e.ContinueLabel;
      objArray[4] = this.GenerateXmlFromProperty(typeof (LabelTarget), "ContinueLabel", continueLabel ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement MemberExpressionToXElement(MemberExpression e)
    {
      string name = "MemberExpression";
      object[] objArray = new object[5];
      object member = (object) e.Member;
      objArray[0] = this.GenerateXmlFromProperty(typeof (MemberInfo), "Member", member ?? (object) string.Empty);
      object expression = (object) e.Expression;
      objArray[1] = this.GenerateXmlFromProperty(typeof (Expression), "Expression", expression ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[2] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object type = (object) e.Type;
      objArray[3] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[4] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement MemberInitExpressionToXElement(MemberInitExpression e)
    {
      string name = "MemberInitExpression";
      object[] objArray = new object[5];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[1] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[2] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object newExpression = (object) e.NewExpression;
      objArray[3] = this.GenerateXmlFromProperty(typeof (NewExpression), "NewExpression", newExpression ?? (object) string.Empty);
      object bindings = (object) e.Bindings;
      objArray[4] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<MemberBinding>), "Bindings", bindings ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement MethodCallExpressionToXElement(MethodCallExpression e)
    {
      string name = "MethodCallExpression";
      object[] objArray = new object[6];
      object nodeType = (object) e.NodeType;
      objArray[0] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object type = (object) e.Type;
      objArray[1] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object method = (object) e.Method;
      objArray[2] = this.GenerateXmlFromProperty(typeof (MethodInfo), "Method", method ?? (object) string.Empty);
      object obj = (object) e.Object;
      objArray[3] = this.GenerateXmlFromProperty(typeof (Expression), "Object", obj ?? (object) string.Empty);
      object arguments = (object) e.Arguments;
      objArray[4] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<Expression>), "Arguments", arguments ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement NewArrayExpressionToXElement(NewArrayExpression e)
    {
      string name = "NewArrayExpression";
      object[] objArray = new object[4];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object expressions = (object) e.Expressions;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<Expression>), "Expressions", expressions ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[2] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[3] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement NewExpressionToXElement(NewExpression e)
    {
      string name = "NewExpression";
      object[] objArray = new object[6];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object constructor = (object) e.Constructor;
      objArray[2] = this.GenerateXmlFromProperty(typeof (ConstructorInfo), "Constructor", constructor ?? (object) string.Empty);
      object arguments = (object) e.Arguments;
      objArray[3] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<Expression>), "Arguments", arguments ?? (object) string.Empty);
      object members = (object) e.Members;
      objArray[4] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<MemberInfo>), "Members", members ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement ParameterExpressionToXElement(ParameterExpression e)
    {
      string name1 = "ParameterExpression";
      object[] objArray = new object[5];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object name2 = (object) e.Name;
      objArray[2] = this.GenerateXmlFromProperty(typeof (string), "Name", name2 ?? (object) string.Empty);
      object isByRef = (object) e.IsByRef;
      objArray[3] = this.GenerateXmlFromProperty(typeof (bool), "IsByRef", isByRef ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[4] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name1, objArray);
    }

    internal XElement RuntimeVariablesExpressionToXElement(RuntimeVariablesExpression e)
    {
      string name = "RuntimeVariablesExpression";
      object[] objArray = new object[4];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object variables = (object) e.Variables;
      objArray[2] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<ParameterExpression>), "Variables", variables ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[3] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement SwitchExpressionToXElement(SwitchExpression e)
    {
      string name = "SwitchExpression";
      object[] objArray = new object[7];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object switchValue = (object) e.SwitchValue;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "SwitchValue", switchValue ?? (object) string.Empty);
      object cases = (object) e.Cases;
      objArray[3] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<SwitchCase>), "Cases", cases ?? (object) string.Empty);
      object defaultBody = (object) e.DefaultBody;
      objArray[4] = this.GenerateXmlFromProperty(typeof (Expression), "DefaultBody", defaultBody ?? (object) string.Empty);
      object comparison = (object) e.Comparison;
      objArray[5] = this.GenerateXmlFromProperty(typeof (MethodInfo), "Comparison", comparison ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[6] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement TryExpressionToXElement(TryExpression e)
    {
      string name = "TryExpression";
      object[] objArray = new object[7];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object body = (object) e.Body;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Body", body ?? (object) string.Empty);
      object handlers = (object) e.Handlers;
      objArray[3] = this.GenerateXmlFromProperty(typeof (ReadOnlyCollection<CatchBlock>), "Handlers", handlers ?? (object) string.Empty);
      object obj = (object) e.Finally;
      objArray[4] = this.GenerateXmlFromProperty(typeof (Expression), "Finally", obj ?? (object) string.Empty);
      object fault = (object) e.Fault;
      objArray[5] = this.GenerateXmlFromProperty(typeof (Expression), "Fault", fault ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[6] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement TypeBinaryExpressionToXElement(TypeBinaryExpression e)
    {
      string name = "TypeBinaryExpression";
      object[] objArray = new object[5];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object expression = (object) e.Expression;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Expression", expression ?? (object) string.Empty);
      object typeOperand = (object) e.TypeOperand;
      objArray[3] = this.GenerateXmlFromProperty(typeof (Type), "TypeOperand", typeOperand ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[4] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    internal XElement UnaryExpressionToXElement(UnaryExpression e)
    {
      string name = "UnaryExpression";
      object[] objArray = new object[7];
      object type = (object) e.Type;
      objArray[0] = this.GenerateXmlFromProperty(typeof (Type), "Type", type ?? (object) string.Empty);
      object nodeType = (object) e.NodeType;
      objArray[1] = this.GenerateXmlFromProperty(typeof (ExpressionType), "NodeType", nodeType ?? (object) string.Empty);
      object operand = (object) e.Operand;
      objArray[2] = this.GenerateXmlFromProperty(typeof (Expression), "Operand", operand ?? (object) string.Empty);
      object method = (object) e.Method;
      objArray[3] = this.GenerateXmlFromProperty(typeof (MethodInfo), "Method", method ?? (object) string.Empty);
      object isLifted = (object) e.IsLifted;
      objArray[4] = this.GenerateXmlFromProperty(typeof (bool), "IsLifted", isLifted ?? (object) string.Empty);
      object isLiftedToNull = (object) e.IsLiftedToNull;
      objArray[5] = this.GenerateXmlFromProperty(typeof (bool), "IsLiftedToNull", isLiftedToNull ?? (object) string.Empty);
      object canReduce = (object) e.CanReduce;
      objArray[6] = this.GenerateXmlFromProperty(typeof (bool), "CanReduce", canReduce ?? (object) string.Empty);
      return new XElement((XName) name, objArray);
    }

    public Expression Deserialize(XElement xml)
    {
      this.parameters.Clear();
      return this.ParseExpressionFromXmlNonNull(xml);
    }

    public Expression<TDelegate> Deserialize<TDelegate>(XElement xml)
    {
      Expression expression = this.Deserialize(xml);
      return expression is Expression<TDelegate> ? expression as Expression<TDelegate> : throw new Exception("xml must represent an Expression<TDelegate>");
    }

    private Expression ParseExpressionFromXml(XElement xml)
    {
      return xml.IsEmpty ? (Expression) null : this.ParseExpressionFromXmlNonNull(xml.Elements().First<XElement>());
    }

    private Expression ParseExpressionFromXmlNonNull(XElement xml)
    {
      Expression result;
      if (this.TryCustomDeserializers(xml, out result) || result != null)
        return result;
      switch (xml.Name.LocalName)
      {
        case "BinaryExpression":
          return this.ParseBinaryExpresssionFromXml(xml);
        case "ConditionalExpression":
          return this.ParseConditionalExpressionFromXml(xml);
        case "ConstantExpression":
        case "TypedConstantExpression":
          return this.ParseConstantExpressionFromXml(xml);
        case "FieldExpression":
        case "MemberExpression":
        case "PropertyExpression":
          return this.ParseMemberExpressionFromXml(xml);
        case "InvocationExpression":
          return this.ParseInvocationExpressionFromXml(xml);
        case "LambdaExpression":
          return this.ParseLambdaExpressionFromXml(xml);
        case "ListInitExpression":
          return this.ParseListInitExpressionFromXml(xml);
        case "MemberInitExpression":
          return this.ParseMemberInitExpressionFromXml(xml);
        case "MethodCallExpression":
          return this.ParseMethodCallExpressionFromXml(xml);
        case "NewArrayExpression":
          return this.ParseNewArrayExpressionFromXml(xml);
        case "NewExpression":
          return this.ParseNewExpressionFromXml(xml);
        case "ParameterExpression":
          return this.ParseParameterExpressionFromXml(xml);
        case "TypeBinaryExpression":
          return this.ParseTypeBinaryExpressionFromXml(xml);
        case "UnaryExpression":
          return this.ParseUnaryExpressionFromXml(xml);
        default:
          throw new NotSupportedException(xml.Name.LocalName);
      }
    }

    private bool TryCustomDeserializers(XElement xml, out Expression result)
    {
      result = (Expression) null;
      for (int index = 0; index < this.Converters.Count; ++index)
      {
        if (this.Converters[index].TryDeserialize(xml, out result))
          return true;
      }
      return false;
    }

    private Expression ParseInvocationExpressionFromXml(XElement xml)
    {
      return (Expression) Expression.Invoke(this.ParseExpressionFromXml(xml.Element((XName) "Expression")), this.ParseExpressionListFromXml<Expression>(xml, "Arguments"));
    }

    private Expression ParseTypeBinaryExpressionFromXml(XElement xml)
    {
      return (Expression) Expression.TypeIs(this.ParseExpressionFromXml(xml.Element((XName) "Expression")), this.ParseTypeFromXml(xml.Element((XName) "TypeOperand")));
    }

    private Expression ParseNewArrayExpressionFromXml(XElement xml)
    {
      Type typeFromXml = this.ParseTypeFromXml(xml.Element((XName) "Type"));
      Type type = typeFromXml.IsArray ? typeFromXml.GetElementType() : throw new Exception("Expected array type");
      IEnumerable<Expression> expressionListFromXml = this.ParseExpressionListFromXml<Expression>(xml, "Expressions");
      switch (xml.Attribute((XName) "NodeType").Value)
      {
        case "NewArrayInit":
          return (Expression) Expression.NewArrayInit(type, expressionListFromXml);
        case "NewArrayBounds":
          return (Expression) Expression.NewArrayBounds(type, expressionListFromXml);
        default:
          throw new Exception("Expected NewArrayInit or NewArrayBounds");
      }
    }

    private Expression ParseConditionalExpressionFromXml(XElement xml)
    {
      return (Expression) Expression.Condition(this.ParseExpressionFromXml(xml.Element((XName) "Test")), this.ParseExpressionFromXml(xml.Element((XName) "IfTrue")), this.ParseExpressionFromXml(xml.Element((XName) "IfFalse")));
    }

    private Expression ParseMemberInitExpressionFromXml(XElement xml)
    {
      return (Expression) Expression.MemberInit(this.ParseNewExpressionFromXml(xml.Element((XName) "NewExpression").Element((XName) "NewExpression")) as NewExpression, this.ParseBindingListFromXml(xml, "Bindings").ToArray<MemberBinding>());
    }

    private Expression ParseListInitExpressionFromXml(XElement xml)
    {
      ElementInit[] elementInitArray = this.ParseExpressionFromXml(xml.Element((XName) "NewExpression")) is NewExpression expressionFromXml ? this.ParseElementInitListFromXml(xml, "Initializers").ToArray<ElementInit>() : throw new Exception("Expceted a NewExpression");
      return (Expression) Expression.ListInit(expressionFromXml, elementInitArray);
    }

    private Expression ParseNewExpressionFromXml(XElement xml)
    {
      ConstructorInfo constructorInfoFromXml = this.ParseConstructorInfoFromXml(xml.Element((XName) "Constructor"));
      Expression[] array1 = this.ParseExpressionListFromXml<Expression>(xml, "Arguments").ToArray<Expression>();
      MemberInfo[] array2 = this.ParseMemberInfoListFromXml<MemberInfo>(xml, "Members").ToArray<MemberInfo>();
      return array2.Length == 0 ? (Expression) Expression.New(constructorInfoFromXml, array1) : (Expression) Expression.New(constructorInfoFromXml, (IEnumerable<Expression>) array1, array2);
    }

    private Expression ParseMemberExpressionFromXml(XElement xml)
    {
      return (Expression) Expression.MakeMemberAccess(this.ParseExpressionFromXml(xml.Element((XName) "Expression")), this.ParseMemberInfoFromXml(xml.Element((XName) "Member")));
    }

    private MemberInfo ParseMemberInfoFromXml(XElement xml)
    {
      MemberTypes constantFromAttribute = (MemberTypes) this.ParseConstantFromAttribute<MemberTypes>(xml, "MemberType");
      switch (constantFromAttribute)
      {
        case MemberTypes.Constructor:
          return (MemberInfo) this.ParseConstructorInfoFromXml(xml);
        case MemberTypes.Field:
          return this.ParseFieldInfoFromXml(xml);
        case MemberTypes.Method:
          return (MemberInfo) this.ParseMethodInfoFromXml(xml);
        case MemberTypes.Property:
          return this.ParsePropertyInfoFromXml(xml);
        default:
          throw new NotSupportedException(string.Format("MEmberType {0} not supported", (object) constantFromAttribute));
      }
    }

    private MemberInfo ParseFieldInfoFromXml(XElement xml)
    {
      string constantFromAttribute = (string) this.ParseConstantFromAttribute<string>(xml, "FieldName");
      return (MemberInfo) this.ParseTypeFromXml(xml.Element((XName) "DeclaringType")).GetField(constantFromAttribute);
    }

    private MemberInfo ParsePropertyInfoFromXml(XElement xml)
    {
      string constantFromAttribute = (string) this.ParseConstantFromAttribute<string>(xml, "PropertyName");
      Type typeFromXml = this.ParseTypeFromXml(xml.Element((XName) "DeclaringType"));
      xml.Element((XName) "IndexParameters").Elements().Select<XElement, Type>((Func<XElement, Type>) (paramXml => this.ParseTypeFromXml(paramXml)));
      return (MemberInfo) typeFromXml.GetProperty(constantFromAttribute);
    }

    private Expression ParseUnaryExpressionFromXml(XElement xml)
    {
      Expression expressionFromXml = this.ParseExpressionFromXml(xml.Element((XName) "Operand"));
      MethodInfo methodInfoFromXml = this.ParseMethodInfoFromXml(xml.Element((XName) "Method"));
      bool constantFromAttribute1 = (bool) this.ParseConstantFromAttribute<bool>(xml, "IsLifted");
      bool constantFromAttribute2 = (bool) this.ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
      ExpressionType constantFromAttribute3 = (ExpressionType) this.ParseConstantFromAttribute<ExpressionType>(xml, "NodeType");
      Type typeFromXml = this.ParseTypeFromXml(xml.Element((XName) "Type"));
      return (Expression) Expression.MakeUnary(constantFromAttribute3, expressionFromXml, typeFromXml, methodInfoFromXml);
    }

    private Expression ParseMethodCallExpressionFromXml(XElement xml)
    {
      Expression expressionFromXml = this.ParseExpressionFromXml(xml.Element((XName) "Object"));
      MethodInfo methodInfoFromXml = this.ParseMethodInfoFromXml(xml.Element((XName) "Method"));
      IEnumerable<Expression> expressions = this.ParseExpressionListFromXml<Expression>(xml, "Arguments");
      if (expressions == null || expressions.Count<Expression>() == 0)
        expressions = (IEnumerable<Expression>) new Expression[0];
      return expressionFromXml == null ? (Expression) Expression.Call(methodInfoFromXml, expressions) : (Expression) Expression.Call(expressionFromXml, methodInfoFromXml, expressions);
    }

    private Expression ParseLambdaExpressionFromXml(XElement xml)
    {
      Expression expressionFromXml = this.ParseExpressionFromXml(xml.Element((XName) "Body"));
      IEnumerable<ParameterExpression> expressionListFromXml = this.ParseExpressionListFromXml<ParameterExpression>(xml, "Parameters");
      return (Expression) Expression.Lambda(this.ParseTypeFromXml(xml.Element((XName) "Type")), expressionFromXml, expressionListFromXml);
    }

    private IEnumerable<T> ParseExpressionListFromXml<T>(XElement xml, string elemName) where T : Expression
    {
      IEnumerable<XElement> xelements = xml.Elements((XName) elemName).Elements<XElement>();
      List<T> expressionListFromXml = new List<T>();
      foreach (XElement xml1 in xelements)
      {
        object expressionFromXmlNonNull = (object) this.ParseExpressionFromXmlNonNull(xml1);
        expressionListFromXml.Add((T) expressionFromXmlNonNull);
      }
      return (IEnumerable<T>) expressionListFromXml;
    }

    private IEnumerable<T> ParseMemberInfoListFromXml<T>(XElement xml, string elemName) where T : MemberInfo
    {
      return xml.Element((XName) elemName).Elements().Select<XElement, T>((Func<XElement, T>) (tXml => (T) this.ParseMemberInfoFromXml(tXml)));
    }

    private IEnumerable<ElementInit> ParseElementInitListFromXml(XElement xml, string elemName)
    {
      return xml.Element((XName) elemName).Elements().Select<XElement, ElementInit>((Func<XElement, ElementInit>) (tXml => this.ParseElementInitFromXml(tXml)));
    }

    private ElementInit ParseElementInitFromXml(XElement xml)
    {
      return Expression.ElementInit(this.ParseMethodInfoFromXml(xml.Element((XName) "AddMethod")), this.ParseExpressionListFromXml<Expression>(xml, "Arguments"));
    }

    private IEnumerable<MemberBinding> ParseBindingListFromXml(XElement xml, string elemName)
    {
      return xml.Element((XName) elemName).Elements().Select<XElement, MemberBinding>((Func<XElement, MemberBinding>) (tXml => this.ParseBindingFromXml(tXml)));
    }

    private MemberBinding ParseBindingFromXml(XElement tXml)
    {
      MemberInfo memberInfoFromXml = this.ParseMemberInfoFromXml(tXml.Element((XName) "Member"));
      switch (tXml.Name.LocalName)
      {
        case "MemberAssignment":
          Expression expressionFromXml = this.ParseExpressionFromXml(tXml.Element((XName) "Expression"));
          return (MemberBinding) Expression.Bind(memberInfoFromXml, expressionFromXml);
        case "MemberMemberBinding":
          IEnumerable<MemberBinding> bindingListFromXml = this.ParseBindingListFromXml(tXml, "Bindings");
          return (MemberBinding) Expression.MemberBind(memberInfoFromXml, bindingListFromXml);
        case "MemberListBinding":
          IEnumerable<ElementInit> elementInitListFromXml = this.ParseElementInitListFromXml(tXml, "Initializers");
          return (MemberBinding) Expression.ListBind(memberInfoFromXml, elementInitListFromXml);
        default:
          throw new NotImplementedException();
      }
    }

    private Expression ParseParameterExpressionFromXml(XElement xml)
    {
      Type typeFromXml = this.ParseTypeFromXml(xml.Element((XName) "Type"));
      string constantFromAttribute = (string) this.ParseConstantFromAttribute<string>(xml, "Name");
      string key = constantFromAttribute + typeFromXml.FullName;
      if (!this.parameters.ContainsKey(key))
        this.parameters.Add(key, Expression.Parameter(typeFromXml, constantFromAttribute));
      return (Expression) this.parameters[key];
    }

    private Expression ParseConstantExpressionFromXml(XElement xml)
    {
      Type typeFromXml = this.ParseTypeFromXml(xml.Element((XName) "Type"));
      object constantFromElement = this.ParseConstantFromElement(xml, "Value", typeFromXml);
      // ISSUE: reference to a compiler-generated field
      if (ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, Expression>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (Expression), typeof (ExpressionSerializer)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Expression> target1 = ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Expression>> p2 = ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Constant", (IEnumerable<Type>) null, typeof (ExpressionSerializer), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object, object> target2 = ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object, object>> p1 = ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__1;
      Type type = typeof (Expression);
      object obj1 = constantFromElement;
      // ISSUE: reference to a compiler-generated field
      if (ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, typeof (ExpressionSerializer), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__0.Target((CallSite) ExpressionSerializer.\u003C\u003Eo__52.\u003C\u003Ep__0, constantFromElement);
      object obj3 = target2((CallSite) p1, type, obj1, obj2);
      return target1((CallSite) p2, obj3);
    }

    private Type ParseTypeFromXml(XElement xml)
    {
      Debug.Assert(xml.Elements().Count<XElement>() == 1);
      return this.ParseTypeFromXmlCore(xml.Elements().First<XElement>());
    }

    private Type ParseTypeFromXmlCore(XElement xml)
    {
      switch (xml.Name.ToString())
      {
        case "Type":
          return this.ParseNormalTypeFromXmlCore(xml);
        case "AnonymousType":
          return this.ParseAnonymousTypeFromXmlCore(xml);
        default:
          throw new ArgumentException("Expected 'Type' or 'AnonymousType'");
      }
    }

    private Type ParseNormalTypeFromXmlCore(XElement xml)
    {
      if (!xml.HasElements)
        return this.resolver.GetType(xml.Attribute((XName) "Name").Value);
      IEnumerable<Type> genericArgumentTypes = xml.Elements().Select<XElement, Type>((Func<XElement, Type>) (genArgXml => this.ParseTypeFromXmlCore(genArgXml)));
      return this.resolver.GetType(xml.Attribute((XName) "Name").Value, genericArgumentTypes);
    }

    private Type ParseAnonymousTypeFromXmlCore(XElement xElement)
    {
      string name = xElement.Attribute((XName) "Name").Value;
      IEnumerable<TypeResolver.NameTypePair> source1 = xElement.Elements((XName) "Property").Select<XElement, TypeResolver.NameTypePair>((Func<XElement, TypeResolver.NameTypePair>) (propXml => new TypeResolver.NameTypePair()
      {
        Name = propXml.Attribute((XName) "Name").Value,
        Type = this.ParseTypeFromXml(propXml)
      }));
      IEnumerable<TypeResolver.NameTypePair> source2 = xElement.Elements((XName) "Constructor").Elements<XElement>((XName) "Parameter").Select<XElement, TypeResolver.NameTypePair>((Func<XElement, TypeResolver.NameTypePair>) (propXml => new TypeResolver.NameTypePair()
      {
        Name = propXml.Attribute((XName) "Name").Value,
        Type = this.ParseTypeFromXml(propXml)
      }));
      return this.resolver.GetOrCreateAnonymousTypeFor(name, source1.ToArray<TypeResolver.NameTypePair>(), source2.ToArray<TypeResolver.NameTypePair>());
    }

    private Expression ParseBinaryExpresssionFromXml(XElement xml)
    {
      ExpressionType constantFromAttribute1 = (ExpressionType) this.ParseConstantFromAttribute<ExpressionType>(xml, "NodeType");
      Expression expressionFromXml1 = this.ParseExpressionFromXml(xml.Element((XName) "Left"));
      Expression expressionFromXml2 = this.ParseExpressionFromXml(xml.Element((XName) "Right"));
      if (expressionFromXml1.Type != expressionFromXml2.Type)
        this.ParseBinaryExpressionConvert(ref expressionFromXml1, ref expressionFromXml2);
      bool constantFromAttribute2 = (bool) this.ParseConstantFromAttribute<bool>(xml, "IsLifted");
      bool constantFromAttribute3 = (bool) this.ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
      this.ParseTypeFromXml(xml.Element((XName) "Type"));
      MethodInfo methodInfoFromXml = this.ParseMethodInfoFromXml(xml.Element((XName) "Method"));
      LambdaExpression expressionFromXml3 = this.ParseExpressionFromXml(xml.Element((XName) "Conversion")) as LambdaExpression;
      return constantFromAttribute1 == ExpressionType.Coalesce ? (Expression) Expression.Coalesce(expressionFromXml1, expressionFromXml2, expressionFromXml3) : (Expression) Expression.MakeBinary(constantFromAttribute1, expressionFromXml1, expressionFromXml2, constantFromAttribute3, methodInfoFromXml);
    }

    private void ParseBinaryExpressionConvert(ref Expression left, ref Expression right)
    {
      if (!(left.Type != right.Type))
        return;
      if (right is ConstantExpression)
      {
        UnaryExpression unaryExpression = Expression.Convert(left, right.Type);
        left = (Expression) unaryExpression;
      }
      else
      {
        UnaryExpression unaryExpression = Expression.Convert(right, left.Type);
        right = (Expression) unaryExpression;
      }
    }

    private MethodInfo ParseMethodInfoFromXml(XElement xml)
    {
      if (xml.IsEmpty)
        return (MethodInfo) null;
      string constantFromAttribute = (string) this.ParseConstantFromAttribute<string>(xml, "MethodName");
      Type typeFromXml = this.ParseTypeFromXml(xml.Element((XName) "DeclaringType"));
      IEnumerable<Type> source1 = xml.Element((XName) "Parameters").Elements().Select<XElement, Type>((Func<XElement, Type>) (paramXml => this.ParseTypeFromXml(paramXml)));
      IEnumerable<Type> source2 = xml.Element((XName) "GenericArgTypes").Elements().Select<XElement, Type>((Func<XElement, Type>) (argXml => this.ParseTypeFromXml(argXml)));
      return this.resolver.GetMethod(typeFromXml, constantFromAttribute, source1.ToArray<Type>(), source2.ToArray<Type>());
    }

    private ConstructorInfo ParseConstructorInfoFromXml(XElement xml)
    {
      return xml.IsEmpty ? (ConstructorInfo) null : this.ParseTypeFromXml(xml.Element((XName) "DeclaringType")).GetConstructor(xml.Element((XName) "Parameters").Elements().Select<XElement, Type>((Func<XElement, Type>) (paramXml => this.ParseParameterFromXml(paramXml))).ToArray<Type>());
    }

    private Type ParseParameterFromXml(XElement xml)
    {
      string constantFromAttribute = (string) this.ParseConstantFromAttribute<string>(xml, "Name");
      return this.ParseTypeFromXml(xml.Element((XName) "Type"));
    }

    private object ParseConstantFromAttribute<T>(XElement xml, string attrName)
    {
      string str = xml.Attribute((XName) attrName).Value;
      if (typeof (Type).IsAssignableFrom(typeof (T)))
        throw new Exception("We should never be encoding Types in attributes now.");
      return typeof (Enum).IsAssignableFrom(typeof (T)) ? Enum.Parse(typeof (T), str, false) : Convert.ChangeType((object) str, typeof (T), (IFormatProvider) null);
    }

    private object ParseConstantFromAttribute(XElement xml, string attrName, Type type)
    {
      string str = xml.Attribute((XName) attrName).Value;
      if (typeof (Type).IsAssignableFrom(type))
        throw new Exception("We should never be encoding Types in attributes now.");
      return typeof (Enum).IsAssignableFrom(type) ? Enum.Parse(type, str, false) : Convert.ChangeType((object) str, type, (IFormatProvider) null);
    }

    private object ParseConstantFromElement(XElement xml, string elemName, Type expectedType)
    {
      string str = xml.Element((XName) elemName).Value;
      if (typeof (Type).IsAssignableFrom(expectedType))
        return (object) this.ParseTypeFromXml(xml.Element((XName) "Value"));
      return typeof (Enum).IsAssignableFrom(expectedType) ? Enum.Parse(expectedType, str, false) : Convert.ChangeType((object) str, expectedType, (IFormatProvider) null);
    }

    public List<CustomExpressionXmlConverter> Converters { get; private set; }

    public ExpressionSerializer(
      TypeResolver resolver,
      IEnumerable<CustomExpressionXmlConverter> converters = null)
    {
      this.resolver = resolver;
      if (converters != null)
        this.Converters = new List<CustomExpressionXmlConverter>(converters);
      else
        this.Converters = new List<CustomExpressionXmlConverter>();
    }

    public ExpressionSerializer()
    {
      this.resolver = new TypeResolver();
      this.Converters = new List<CustomExpressionXmlConverter>();
    }

    public XElement Serialize(Expression e)
    {
      if (e.NodeType != ExpressionType.Lambda)
        e = Evaluator.PartialEval(e);
      return this.GenerateXmlFromExpressionCore(e);
    }

    private bool TryCustomSerializers(Expression e, out XElement result)
    {
      result = (XElement) null;
      for (int index = 0; index < this.Converters.Count; ++index)
      {
        if (this.Converters[index].TrySerialize(e, out result))
          return true;
      }
      return false;
    }

    private object GenerateXmlFromProperty(Type propType, string propName, object value)
    {
      if (((IEnumerable<Type>) ExpressionSerializer.primitiveTypes).Contains<Type>(propType))
        return this.GenerateXmlFromPrimitive(propName, value);
      if (propType.Equals(typeof (object)))
        return this.GenerateXmlFromObject(propName, value);
      if (typeof (Expression).IsAssignableFrom(propType))
        return (object) this.GenerateXmlFromExpression(propName, value as Expression);
      if ((object) (value as MethodInfo) != null || propType.Equals(typeof (MethodInfo)))
        return this.GenerateXmlFromMethodInfo(propName, value as MethodInfo);
      if ((object) (value as PropertyInfo) != null || propType.Equals(typeof (PropertyInfo)))
        return this.GenerateXmlFromPropertyInfo(propName, value as PropertyInfo);
      if ((object) (value as FieldInfo) != null || propType.Equals(typeof (FieldInfo)))
        return this.GenerateXmlFromFieldInfo(propName, value as FieldInfo);
      if ((object) (value as ConstructorInfo) != null || propType.Equals(typeof (ConstructorInfo)))
        return this.GenerateXmlFromConstructorInfo(propName, value as ConstructorInfo);
      if (propType.Equals(typeof (Type)))
        return this.GenerateXmlFromType(propName, value as Type);
      if (this.IsIEnumerableOf<Expression>(propType))
        return this.GenerateXmlFromExpressionList(propName, this.AsIEnumerableOf<Expression>(value));
      if (this.IsIEnumerableOf<MemberInfo>(propType))
        return this.GenerateXmlFromMemberInfoList(propName, this.AsIEnumerableOf<MemberInfo>(value));
      if (this.IsIEnumerableOf<ElementInit>(propType))
        return this.GenerateXmlFromElementInitList(propName, this.AsIEnumerableOf<ElementInit>(value));
      if (this.IsIEnumerableOf<MemberBinding>(propType))
        return this.GenerateXmlFromBindingList(propName, this.AsIEnumerableOf<MemberBinding>(value));
      throw new NotSupportedException(propName);
    }

    private object GenerateXmlFromObject(string propName, object value)
    {
      Assembly assembly = typeof (string).Assembly;
      object content = !(value is Type) ? (!((IEnumerable<Type>) assembly.GetTypes()).Any<Type>((Func<Type, bool>) (t => t == value.GetType())) ? (object) value.ToString() : (object) value.ToString()) : (object) this.GenerateXmlFromTypeCore((Type) value);
      return (object) new XElement((XName) propName, content);
    }

    private object GenerateXmlFromKnownTypes(string xName, object instance, Type knownType)
    {
      object obj1 = instance;
      if (typeof (IQueryable).IsAssignableFrom(instance.GetType()))
      {
        if (typeof (Query<>).MakeGenericType(knownType).IsAssignableFrom(instance.GetType()))
          return (object) instance.ToString();
        object genericEnumerable = (object) LinqHelper.CastToGenericEnumerable((IEnumerable) instance, knownType);
        // ISSUE: reference to a compiler-generated field
        if (ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToArray", (IEnumerable<Type>) null, typeof (ExpressionSerializer), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        obj1 = ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__0.Target((CallSite) ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__0, typeof (Enumerable), genericEnumerable);
      }
      // ISSUE: reference to a compiler-generated field
      if (ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, Type>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (Type), typeof (ExpressionSerializer)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Type> target = ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Type>> p2 = ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, typeof (ExpressionSerializer), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__1.Target((CallSite) ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__1, obj1);
      DataContractSerializer contractSerializer = new DataContractSerializer(target((CallSite) p2, obj2), (IEnumerable<Type>) this.resolver.knownTypes);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        // ISSUE: reference to a compiler-generated field
        if (ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__3 = CallSite<Action<CallSite, DataContractSerializer, MemoryStream, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteObject", (IEnumerable<Type>) null, typeof (ExpressionSerializer), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__3.Target((CallSite) ExpressionSerializer.\u003C\u003Eo__78.\u003C\u003Ep__3, contractSerializer, memoryStream, obj1);
        memoryStream.Position = 0L;
        string end = new StreamReader((Stream) memoryStream, Encoding.UTF8).ReadToEnd();
        return (object) new XElement((XName) xName, (object) end);
      }
    }

    private bool IsIEnumerableOf<T>(Type propType)
    {
      if (!propType.IsGenericType)
        return false;
      Type[] genericArguments = propType.GetGenericArguments();
      return genericArguments.Length == 1 && typeof (T).IsAssignableFrom(genericArguments[0]) && typeof (IEnumerable<>).MakeGenericType(genericArguments).IsAssignableFrom(propType);
    }

    private bool IsIEnumerableOf(Type enumerableType, Type elementType)
    {
      if (!enumerableType.IsGenericType)
        return false;
      Type[] genericArguments = enumerableType.GetGenericArguments();
      return genericArguments.Length == 1 && elementType.IsAssignableFrom(genericArguments[0]) && typeof (IEnumerable<>).MakeGenericType(genericArguments).IsAssignableFrom(enumerableType);
    }

    private IEnumerable<T> AsIEnumerableOf<T>(object value)
    {
      return value == null ? (IEnumerable<T>) null : (value as IEnumerable).Cast<T>();
    }

    private object GenerateXmlFromElementInitList(
      string propName,
      IEnumerable<ElementInit> initializers)
    {
      if (initializers == null)
        initializers = (IEnumerable<ElementInit>) new ElementInit[0];
      return (object) new XElement((XName) propName, (object) initializers.Select<ElementInit, object>((Func<ElementInit, object>) (elementInit => this.GenerateXmlFromElementInitializer(elementInit))));
    }

    private object GenerateXmlFromElementInitializer(ElementInit elementInit)
    {
      return (object) new XElement((XName) "ElementInit", new object[2]
      {
        this.GenerateXmlFromMethodInfo("AddMethod", elementInit.AddMethod),
        this.GenerateXmlFromExpressionList("Arguments", (IEnumerable<Expression>) elementInit.Arguments)
      });
    }

    private object GenerateXmlFromExpressionList(
      string propName,
      IEnumerable<Expression> expressions)
    {
      return (object) new XElement((XName) propName, (object) expressions.Select<Expression, XElement>((Func<Expression, XElement>) (expression => this.GenerateXmlFromExpressionCore(expression))));
    }

    private object GenerateXmlFromMemberInfoList(string propName, IEnumerable<MemberInfo> members)
    {
      if (members == null)
        members = (IEnumerable<MemberInfo>) new MemberInfo[0];
      return (object) new XElement((XName) propName, (object) members.Select<MemberInfo, object>((Func<MemberInfo, object>) (member => this.GenerateXmlFromProperty(member.GetType(), "Info", (object) member))));
    }

    private object GenerateXmlFromBindingList(string propName, IEnumerable<MemberBinding> bindings)
    {
      if (bindings == null)
        bindings = (IEnumerable<MemberBinding>) new MemberBinding[0];
      return (object) new XElement((XName) propName, (object) bindings.Select<MemberBinding, object>((Func<MemberBinding, object>) (binding => this.GenerateXmlFromBinding(binding))));
    }

    private object GenerateXmlFromBinding(MemberBinding binding)
    {
      switch (binding.BindingType)
      {
        case MemberBindingType.Assignment:
          return this.GenerateXmlFromAssignment(binding as MemberAssignment);
        case MemberBindingType.MemberBinding:
          return this.GenerateXmlFromMemberBinding(binding as MemberMemberBinding);
        case MemberBindingType.ListBinding:
          return this.GenerateXmlFromListBinding(binding as MemberListBinding);
        default:
          throw new NotSupportedException(string.Format("Binding type {0} not supported.", (object) binding.BindingType));
      }
    }

    private object GenerateXmlFromMemberBinding(MemberMemberBinding memberMemberBinding)
    {
      return (object) new XElement((XName) "MemberMemberBinding", new object[2]
      {
        this.GenerateXmlFromProperty(memberMemberBinding.Member.GetType(), "Member", (object) memberMemberBinding.Member),
        this.GenerateXmlFromBindingList("Bindings", (IEnumerable<MemberBinding>) memberMemberBinding.Bindings)
      });
    }

    private object GenerateXmlFromListBinding(MemberListBinding memberListBinding)
    {
      return (object) new XElement((XName) "MemberListBinding", new object[2]
      {
        this.GenerateXmlFromProperty(memberListBinding.Member.GetType(), "Member", (object) memberListBinding.Member),
        this.GenerateXmlFromProperty(memberListBinding.Initializers.GetType(), "Initializers", (object) memberListBinding.Initializers)
      });
    }

    private object GenerateXmlFromAssignment(MemberAssignment memberAssignment)
    {
      return (object) new XElement((XName) "MemberAssignment", new object[2]
      {
        this.GenerateXmlFromProperty(memberAssignment.Member.GetType(), "Member", (object) memberAssignment.Member),
        this.GenerateXmlFromProperty(memberAssignment.Expression.GetType(), "Expression", (object) memberAssignment.Expression)
      });
    }

    private XElement GenerateXmlFromExpression(string propName, Expression e)
    {
      return new XElement((XName) propName, (object) this.GenerateXmlFromExpressionCore(e));
    }

    private object GenerateXmlFromType(string propName, Type type)
    {
      return (object) new XElement((XName) propName, (object) this.GenerateXmlFromTypeCore(type));
    }

    private XElement GenerateXmlFromTypeCore(Type type)
    {
      if (type.Name.StartsWith("<>f__") || type.Name.StartsWith("VB$AnonymousType"))
        return new XElement((XName) "AnonymousType", new object[3]
        {
          (object) new XAttribute((XName) "Name", (object) type.FullName),
          (object) ((IEnumerable<PropertyInfo>) type.GetProperties()).Select<PropertyInfo, XElement>((Func<PropertyInfo, XElement>) (property => new XElement((XName) "Property", new object[2]
          {
            (object) new XAttribute((XName) "Name", (object) property.Name),
            (object) this.GenerateXmlFromTypeCore(property.PropertyType)
          }))),
          (object) new XElement((XName) "Constructor", (object) ((IEnumerable<ParameterInfo>) ((IEnumerable<ConstructorInfo>) type.GetConstructors()).First<ConstructorInfo>().GetParameters()).Select<ParameterInfo, XElement>((Func<ParameterInfo, XElement>) (parameter => new XElement((XName) "Parameter", new object[2]
          {
            (object) new XAttribute((XName) "Name", (object) parameter.Name),
            (object) this.GenerateXmlFromTypeCore(parameter.ParameterType)
          }))))
        });
      if (!type.IsGenericType)
        return new XElement((XName) "Type", (object) new XAttribute((XName) "Name", (object) type.FullName));
      return new XElement((XName) "Type", new object[2]
      {
        (object) new XAttribute((XName) "Name", (object) type.GetGenericTypeDefinition().FullName),
        (object) ((IEnumerable<Type>) type.GetGenericArguments()).Select<Type, XElement>((Func<Type, XElement>) (genArgType => this.GenerateXmlFromTypeCore(genArgType)))
      });
    }

    private object GenerateXmlFromPrimitive(string propName, object value)
    {
      return (object) new XAttribute((XName) propName, value);
    }

    private object GenerateXmlFromMethodInfo(string propName, MethodInfo methodInfo)
    {
      if (methodInfo == (MethodInfo) null)
        return (object) new XElement((XName) propName);
      return (object) new XElement((XName) propName, new object[5]
      {
        (object) new XAttribute((XName) "MemberType", (object) methodInfo.MemberType),
        (object) new XAttribute((XName) "MethodName", (object) methodInfo.Name),
        this.GenerateXmlFromType("DeclaringType", methodInfo.DeclaringType),
        (object) new XElement((XName) "Parameters", (object) ((IEnumerable<ParameterInfo>) methodInfo.GetParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (param => this.GenerateXmlFromType("Type", param.ParameterType)))),
        (object) new XElement((XName) "GenericArgTypes", (object) ((IEnumerable<Type>) methodInfo.GetGenericArguments()).Select<Type, object>((Func<Type, object>) (argType => this.GenerateXmlFromType("Type", argType))))
      });
    }

    private object GenerateXmlFromPropertyInfo(string propName, PropertyInfo propertyInfo)
    {
      if (propertyInfo == (PropertyInfo) null)
        return (object) new XElement((XName) propName);
      return (object) new XElement((XName) propName, new object[4]
      {
        (object) new XAttribute((XName) "MemberType", (object) propertyInfo.MemberType),
        (object) new XAttribute((XName) "PropertyName", (object) propertyInfo.Name),
        this.GenerateXmlFromType("DeclaringType", propertyInfo.DeclaringType),
        (object) new XElement((XName) "IndexParameters", (object) ((IEnumerable<ParameterInfo>) propertyInfo.GetIndexParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (param => this.GenerateXmlFromType("Type", param.ParameterType))))
      });
    }

    private object GenerateXmlFromFieldInfo(string propName, FieldInfo fieldInfo)
    {
      if (fieldInfo == (FieldInfo) null)
        return (object) new XElement((XName) propName);
      return (object) new XElement((XName) propName, new object[3]
      {
        (object) new XAttribute((XName) "MemberType", (object) fieldInfo.MemberType),
        (object) new XAttribute((XName) "FieldName", (object) fieldInfo.Name),
        this.GenerateXmlFromType("DeclaringType", fieldInfo.DeclaringType)
      });
    }

    private object GenerateXmlFromConstructorInfo(string propName, ConstructorInfo constructorInfo)
    {
      if (constructorInfo == (ConstructorInfo) null)
        return (object) new XElement((XName) propName);
      return (object) new XElement((XName) propName, new object[4]
      {
        (object) new XAttribute((XName) "MemberType", (object) constructorInfo.MemberType),
        (object) new XAttribute((XName) "MethodName", (object) constructorInfo.Name),
        this.GenerateXmlFromType("DeclaringType", constructorInfo.DeclaringType),
        (object) new XElement((XName) "Parameters", (object) ((IEnumerable<ParameterInfo>) constructorInfo.GetParameters()).Select<ParameterInfo, XElement>((Func<ParameterInfo, XElement>) (param => new XElement((XName) "Parameter", new object[2]
        {
          (object) new XAttribute((XName) "Name", (object) param.Name),
          this.GenerateXmlFromType("Type", param.ParameterType)
        }))))
      });
    }
  }
}
