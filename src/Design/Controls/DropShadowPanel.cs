using System.Windows.Media.Effects;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Data;
using Design.Converters;
using System.Windows;
using System;

namespace Design.Controls
{
    public class DropShadowPanel : Decorator
    {
        #region Fields

        private ContainerVisual _internalVisual;       
        protected override int VisualChildrenCount => 1; // Always return 1

        #endregion Fields

        #region Properties

        private ContainerVisual InternalVisual
        {
            get
            {
                if (this._internalVisual is null)
                {
                    this._internalVisual = new ContainerVisual();
                    AddVisualChild(this._internalVisual);
                }
                return this._internalVisual;
            }
        }

        private UIElement InternalChild
        {
            get
            {
                var children = this.InternalVisual.Children;
                if (children.Count > 0) return children[0] as UIElement;
                else return null;
            }
            set
            {
                var children = this.InternalVisual.Children;
                if (children.Count > 0) children.Clear();
                children.Add(value);
            }
        }

        public override UIElement Child
        {
            get { return this.InternalChild; }
            set
            {
                var old = this.InternalChild;

                if (old != value)
                {
                    //Remove old elements from LogicalTree
                    RemoveLogicalChild(old);

                    var ic = this.CreateInternalVisual(value);

                    if (ic != null)
                    {
                        AddLogicalChild(ic);
                    }

                    this.InternalChild = ic;

                    InvalidateMeasure();
                }
            }
        }

        #endregion Properties

        #region DependencyPropertys

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register
        (
            "Background", typeof(Brush),
            typeof(DropShadowPanel), new PropertyMetadata(null)
        );
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty BlurRadiusProperty = DependencyProperty.Register
        (
            "BlurRadius", typeof(double),
            typeof(DropShadowPanel), new PropertyMetadata(20.0)
        );
        public double BlurRadius
        {
            get { return (double)GetValue(BlurRadiusProperty); }
            set { SetValue(BlurRadiusProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register
        (
            "Color", typeof(Color),
            typeof(DropShadowPanel), new PropertyMetadata(Colors.Dark)
        );
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register
        (
            "Direction", typeof(double),
            typeof(DropShadowPanel), new PropertyMetadata(315.0)
        );
        public double Direction
        {
            get { return (double)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty ShadowOpacityProperty = DependencyProperty.Register
        (
            "ShadowOpacity", typeof(double),
            typeof(DropShadowPanel), new PropertyMetadata(0.8)
        );
        public double ShadowOpacity
        {
            get { return (double)GetValue(ShadowOpacityProperty); }
            set { SetValue(ShadowOpacityProperty, value); }
        }
        
        public static readonly DependencyProperty RenderingBiasProperty = DependencyProperty.Register
        (
            "RenderingBias", typeof(RenderingBias),
            typeof(DropShadowPanel), new PropertyMetadata(RenderingBias.Performance)
        );
        public RenderingBias RenderingBias
        {
            get { return (RenderingBias)GetValue(RenderingBiasProperty); }
            set { SetValue(RenderingBiasProperty, value); }
        }
        
        public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.Register
        (
            "ShadowDepth", typeof(double),
            typeof(DropShadowPanel), new PropertyMetadata(0.0)
        );
        public double ShadowDepth
        {
            get { return (double)GetValue(ShadowDepthProperty); }
            set { SetValue(ShadowDepthProperty, value); }
        }

        public static readonly DependencyProperty ShadowModeProperty = DependencyProperty.Register
        (
            "ShadowMode", typeof(ShadowMode),
            typeof(DropShadowPanel), new PropertyMetadata(ShadowMode.Content)
        );
        public ShadowMode ShadowMode
        {
            get { return (ShadowMode)GetValue(ShadowModeProperty); }
            set { SetValue(ShadowModeProperty, value); }
        }

        #endregion DependencyPropertys

        #region Constructor

        public DropShadowPanel() { }

        #endregion Constructor

        #region Methods

        protected internal UIElement CreateInternalVisual(UIElement value)
        {
            var effect = new DropShadowEffect();
            BindingOperations.SetBinding(effect, DropShadowEffect.BlurRadiusProperty, new Binding("BlurRadius") { Source = this });
            BindingOperations.SetBinding(effect, DropShadowEffect.ColorProperty, new Binding("Color") { Source = this });
            BindingOperations.SetBinding(effect, DropShadowEffect.DirectionProperty, new Binding("Direction") { Source = this });
            BindingOperations.SetBinding(effect, DropShadowEffect.OpacityProperty, new Binding("ShadowOpacity") { Source = this });
            BindingOperations.SetBinding(effect, DropShadowEffect.RenderingBiasProperty, new Binding("RenderingBias") { Source = this });
            BindingOperations.SetBinding(effect, DropShadowEffect.ShadowDepthProperty, new Binding("ShadowDepth") { Source = this });

            // Set trigger for mode switching of DropShadow
            var st = new Style();

            //In the case of ShadowMode.Content, treat the content as VisualBrush and create a shadow according to the shape of Content
            var brush = new VisualBrush(value)
            {
                TileMode = TileMode.None,
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                ViewboxUnits = BrushMappingMode.Absolute
            };
            var contentTrigger = new DataTrigger()
            {
                Binding = new Binding("ShadowMode") { Source = this },
                Value = ShadowMode.Content,
            };
            contentTrigger.Setters.Add(new Setter()
            {
                Property = Rectangle.FillProperty,
                Value = brush
            });
            st.Triggers.Add(contentTrigger);


            // For ShadowMode.Outer, the shadow is displayed only outside the control
            var outerTrigger = new DataTrigger()
            {
                Binding = new Binding("ShadowMode") { Source = this },
                Value = ShadowMode.Outer,
            };
            outerTrigger.Setters.Add(new Setter()
            {
                Property = Rectangle.ClipProperty,
                Value = new MultiBinding()
                {
                    Bindings =
                    {
                        new Binding("ActualWidth") { Source = this },
                        new Binding("ActualHeight") { Source = this },
                        new Binding("BlurRadius") { Source = this },
                    },
                    Converter = new ClipInnerRectConverter()
                }
            });
            outerTrigger.Setters.Add(new Setter()
            {
                Property = Rectangle.FillProperty,
                Value = Brushes.White
            });
            st.Triggers.Add(outerTrigger);

            // For ShadowMode.Inner, the shadow is displayed only inside the control
            var innerTrigger = new DataTrigger()
            {
                Binding = new Binding("ShadowMode") { Source = this },
                Value = ShadowMode.Inner,
            };
            innerTrigger.Setters.Add(new Setter()
            {
                Property = Rectangle.StrokeProperty,
                Value = Brushes.White
            });
            innerTrigger.Setters.Add(new Setter()
            {
                Property = Rectangle.StrokeThicknessProperty,
                Value = new Binding("BlurRadius") { Source = this }
            });
            innerTrigger.Setters.Add(new Setter()
            {
                Property = Rectangle.MarginProperty,
                Value = new Binding("BlurRadius") { Source = this, Converter = new NegativeMarginConverter() }
            });
            innerTrigger.Setters.Add(new Setter()
            {
                Property = Rectangle.ClipProperty,
                Value = new MultiBinding()
                {
                    Bindings =
                    {
                        new Binding("ActualWidth") { Source = this },
                        new Binding("ActualHeight") { Source = this },
                        new Binding("BlurRadius") { Source = this },
                    },
                    Converter = new ClipOuterRectConverter()
                }
            });
            st.Triggers.Add(innerTrigger);

            var border = new Rectangle()
            {
                Effect = effect,
                Style = st,
            };

            var grid = new Grid();
            BindingOperations.SetBinding(grid, Grid.BackgroundProperty, new Binding("Background") { Source = this });
            grid.Children.Add(border);
            if (value != null)
            {
                grid.Children.Add(value);
            }

            return grid;
        }
        
        protected override Visual GetVisualChild(int index)
        {
            if (index != 0) throw new ArgumentOutOfRangeException();
            return this.InternalVisual;
        }

        #endregion Methods
    }
}