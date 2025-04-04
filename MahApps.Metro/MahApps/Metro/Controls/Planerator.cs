// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Planerator
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#nullable disable
namespace MahApps.Metro.Controls
{
  [ContentProperty("Child")]
  public class Planerator : FrameworkElement
  {
    public static readonly DependencyProperty RotationXProperty = DependencyProperty.Register(nameof (RotationX), typeof (double), typeof (Planerator), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, (PropertyChangedCallback) ((d, args) => ((Planerator) d).UpdateRotation())));
    public static readonly DependencyProperty RotationYProperty = DependencyProperty.Register(nameof (RotationY), typeof (double), typeof (Planerator), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, (PropertyChangedCallback) ((d, args) => ((Planerator) d).UpdateRotation())));
    public static readonly DependencyProperty RotationZProperty = DependencyProperty.Register(nameof (RotationZ), typeof (double), typeof (Planerator), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, (PropertyChangedCallback) ((d, args) => ((Planerator) d).UpdateRotation())));
    public static readonly DependencyProperty FieldOfViewProperty = DependencyProperty.Register(nameof (FieldOfView), typeof (double), typeof (Planerator), (PropertyMetadata) new UIPropertyMetadata((object) 45.0, (PropertyChangedCallback) ((d, args) => ((Planerator) d).Update3D()), (CoerceValueCallback) ((d, val) => (object) Math.Min(Math.Max((double) val, 0.5), 179.9))));
    private static readonly Point3D[] Mesh = new Point3D[4]
    {
      new Point3D(0.0, 0.0, 0.0),
      new Point3D(0.0, 1.0, 0.0),
      new Point3D(1.0, 1.0, 0.0),
      new Point3D(1.0, 0.0, 0.0)
    };
    private static readonly Point[] TexCoords = new Point[4]
    {
      new Point(0.0, 1.0),
      new Point(0.0, 0.0),
      new Point(1.0, 0.0),
      new Point(1.0, 1.0)
    };
    private static readonly int[] Indices = new int[6]
    {
      0,
      2,
      1,
      0,
      3,
      2
    };
    private static readonly Vector3D XAxis = new Vector3D(1.0, 0.0, 0.0);
    private static readonly Vector3D YAxis = new Vector3D(0.0, 1.0, 0.0);
    private static readonly Vector3D ZAxis = new Vector3D(0.0, 0.0, 1.0);
    private readonly QuaternionRotation3D _quaternionRotation = new QuaternionRotation3D();
    private readonly RotateTransform3D _rotationTransform = new RotateTransform3D();
    private readonly ScaleTransform3D _scaleTransform = new ScaleTransform3D();
    private FrameworkElement _logicalChild;
    private FrameworkElement _originalChild;
    private Viewport3D _viewport3D;
    private FrameworkElement _visualChild;
    private Viewport2DVisual3D _frontModel;

    public double RotationX
    {
      get => (double) this.GetValue(Planerator.RotationXProperty);
      set => this.SetValue(Planerator.RotationXProperty, (object) value);
    }

    public double RotationY
    {
      get => (double) this.GetValue(Planerator.RotationYProperty);
      set => this.SetValue(Planerator.RotationYProperty, (object) value);
    }

    public double RotationZ
    {
      get => (double) this.GetValue(Planerator.RotationZProperty);
      set => this.SetValue(Planerator.RotationZProperty, (object) value);
    }

    public double FieldOfView
    {
      get => (double) this.GetValue(Planerator.FieldOfViewProperty);
      set => this.SetValue(Planerator.FieldOfViewProperty, (object) value);
    }

    public FrameworkElement Child
    {
      get => this._originalChild;
      set
      {
        if (this._originalChild == value)
          return;
        this.RemoveVisualChild((Visual) this._visualChild);
        this.RemoveLogicalChild((object) this._logicalChild);
        this._originalChild = value;
        LayoutInvalidationCatcher invalidationCatcher = new LayoutInvalidationCatcher();
        invalidationCatcher.Child = (UIElement) this._originalChild;
        this._logicalChild = (FrameworkElement) invalidationCatcher;
        this._visualChild = this.CreateVisualChild();
        this.AddVisualChild((Visual) this._visualChild);
        this.AddLogicalChild((object) this._logicalChild);
        this.InvalidateMeasure();
      }
    }

    protected override int VisualChildrenCount => this._visualChild != null ? 1 : 0;

    protected override Size MeasureOverride(Size availableSize)
    {
      Size availableSize1;
      if (this._logicalChild != null)
      {
        this._logicalChild.Measure(availableSize);
        availableSize1 = this._logicalChild.DesiredSize;
        this._visualChild.Measure(availableSize1);
      }
      else
        availableSize1 = new Size(0.0, 0.0);
      return availableSize1;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      if (this._logicalChild != null)
      {
        this._logicalChild.Arrange(new Rect(finalSize));
        this._visualChild.Arrange(new Rect(finalSize));
        this.Update3D();
      }
      return base.ArrangeOverride(finalSize);
    }

    protected override Visual GetVisualChild(int index) => (Visual) this._visualChild;

    private FrameworkElement CreateVisualChild()
    {
      MeshGeometry3D meshGeometry3D = new MeshGeometry3D()
      {
        Positions = new Point3DCollection((IEnumerable<Point3D>) Planerator.Mesh),
        TextureCoordinates = new PointCollection((IEnumerable<Point>) Planerator.TexCoords),
        TriangleIndices = new Int32Collection((IEnumerable<int>) Planerator.Indices)
      };
      Material material1 = (Material) new DiffuseMaterial((Brush) Brushes.White);
      material1.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, (object) true);
      VisualBrush d = new VisualBrush((Visual) this._logicalChild);
      this.SetCachingForObject((DependencyObject) d);
      Material material2 = (Material) new DiffuseMaterial((Brush) d);
      this._rotationTransform.Rotation = (Rotation3D) this._quaternionRotation;
      Transform3DGroup transform3Dgroup = new Transform3DGroup()
      {
        Children = {
          (Transform3D) this._scaleTransform,
          (Transform3D) this._rotationTransform
        }
      };
      GeometryModel3D geometryModel3D1 = new GeometryModel3D();
      geometryModel3D1.Geometry = (Geometry3D) meshGeometry3D;
      geometryModel3D1.Transform = (Transform3D) transform3Dgroup;
      geometryModel3D1.BackMaterial = material2;
      GeometryModel3D geometryModel3D2 = geometryModel3D1;
      Model3DGroup model3Dgroup = new Model3DGroup()
      {
        Children = {
          (Model3D) new DirectionalLight(Colors.White, new Vector3D(0.0, 0.0, -1.0)),
          (Model3D) new DirectionalLight(Colors.White, new Vector3D(0.1, -0.1, 1.0)),
          (Model3D) geometryModel3D2
        }
      };
      ModelVisual3D modelVisual3D = new ModelVisual3D()
      {
        Content = (Model3D) model3Dgroup
      };
      if (this._frontModel != null)
        this._frontModel.Visual = (Visual) null;
      Viewport2DVisual3D viewport2Dvisual3D = new Viewport2DVisual3D();
      viewport2Dvisual3D.Geometry = (Geometry3D) meshGeometry3D;
      viewport2Dvisual3D.Visual = (Visual) this._logicalChild;
      viewport2Dvisual3D.Material = material1;
      viewport2Dvisual3D.Transform = (Transform3D) transform3Dgroup;
      this._frontModel = viewport2Dvisual3D;
      this.SetCachingForObject((DependencyObject) this._frontModel);
      Viewport3D viewport3D = new Viewport3D();
      viewport3D.ClipToBounds = false;
      viewport3D.Children.Add((Visual3D) modelVisual3D);
      viewport3D.Children.Add((Visual3D) this._frontModel);
      this._viewport3D = viewport3D;
      this.UpdateRotation();
      return (FrameworkElement) this._viewport3D;
    }

    private void SetCachingForObject(DependencyObject d)
    {
      RenderOptions.SetCachingHint(d, CachingHint.Cache);
      RenderOptions.SetCacheInvalidationThresholdMinimum(d, 0.5);
      RenderOptions.SetCacheInvalidationThresholdMaximum(d, 2.0);
    }

    private void UpdateRotation()
    {
      this._quaternionRotation.Quaternion = new Quaternion(Planerator.XAxis, this.RotationX) * new Quaternion(Planerator.YAxis, this.RotationY) * new Quaternion(Planerator.ZAxis, this.RotationZ);
    }

    private void Update3D()
    {
      Rect descendantBounds = VisualTreeHelper.GetDescendantBounds((Visual) this._logicalChild);
      double width = descendantBounds.Width;
      double height = descendantBounds.Height;
      double num = this.FieldOfView * (Math.PI / 180.0);
      double z = width / Math.Tan(num / 2.0) / 2.0;
      this._viewport3D.Camera = (Camera) new PerspectiveCamera(new Point3D(width / 2.0, height / 2.0, z), -Planerator.ZAxis, Planerator.YAxis, this.FieldOfView);
      this._scaleTransform.ScaleX = width;
      this._scaleTransform.ScaleY = height;
      this._rotationTransform.CenterX = width / 2.0;
      this._rotationTransform.CenterY = height / 2.0;
    }
  }
}
