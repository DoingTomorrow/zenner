// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ConstructorNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class ConstructorNode(IToken token) : SelectExpressionList(token), ISelectExpression
  {
    private IType[] _constructorArgumentTypes;
    private ConstructorInfo _constructor;
    private bool _isMap;
    private bool _isList;

    public IList<IType> ConstructorArgumentTypeList
    {
      get => (IList<IType>) new List<IType>((IEnumerable<IType>) this._constructorArgumentTypes);
    }

    public string[] GetAliases()
    {
      ISelectExpression[] selectExpressionArray = this.CollectSelectExpressions();
      string[] aliases = new string[selectExpressionArray.Length];
      for (int index = 0; index < selectExpressionArray.Length; ++index)
      {
        string alias = selectExpressionArray[index].Alias;
        aliases[index] = alias ?? index.ToString();
      }
      return aliases;
    }

    protected internal override IASTNode GetFirstSelectExpression() => this.GetChild(1);

    public void SetScalarColumnText(int i)
    {
      ISelectExpression[] selectExpressionArray = this.CollectSelectExpressions();
      for (int i1 = 0; i1 < selectExpressionArray.Length; ++i1)
        selectExpressionArray[i1].SetScalarColumnText(i1);
    }

    public FromElement FromElement => (FromElement) null;

    public bool IsConstructor => true;

    public bool IsReturnableEntity => false;

    public bool IsScalar => true;

    public string Alias
    {
      get => throw new InvalidOperationException("constructor may not be aliased");
      set => throw new InvalidOperationException("constructor may not be aliased");
    }

    public ConstructorInfo Constructor => this._constructor;

    public bool IsMap => this._isMap;

    public bool IsList => this._isList;

    public void Prepare()
    {
      this._constructorArgumentTypes = this.ResolveConstructorArgumentTypes();
      string path = ((IPathNode) this.GetChild(0)).Path;
      if (path.ToLowerInvariant() == "map")
        this._isMap = true;
      else if (path.ToLowerInvariant() == "list")
        this._isList = true;
      else
        this._constructor = this.ResolveConstructor(path);
    }

    private IType[] ResolveConstructorArgumentTypes()
    {
      ISelectExpression[] selectExpressionArray = this.CollectSelectExpressions();
      if (selectExpressionArray == null)
        return new IType[0];
      IType[] typeArray = new IType[selectExpressionArray.Length];
      for (int index = 0; index < selectExpressionArray.Length; ++index)
        typeArray[index] = selectExpressionArray[index].DataType;
      return typeArray;
    }

    private ConstructorInfo ResolveConstructor(string path)
    {
      string importedClassName = this.SessionFactoryHelper.GetImportedClassName(path);
      string name = StringHelper.IsEmpty(importedClassName) ? path : importedClassName;
      if (name == null)
        throw new SemanticException("Unable to locate class [" + path + "]");
      try
      {
        return ReflectHelper.GetConstructor(ReflectHelper.ClassForName(name), this._constructorArgumentTypes);
      }
      catch (TypeLoadException ex)
      {
        throw new QueryException("Unable to locate class [" + name + "]", (Exception) ex);
      }
      catch (InstantiationException ex)
      {
        throw new QueryException("Unable to locate appropriate constructor on class [" + name + "]", (Exception) ex);
      }
    }
  }
}
