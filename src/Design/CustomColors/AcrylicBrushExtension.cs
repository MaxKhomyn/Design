using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Data;
using Design.Controls;
using System.Windows;
using System;

namespace Design.CustomColors
{
    public class AcrylicBrushExtension : MarkupExtension
    {
        #region Properties

        public string TargetName { get; set; }
        public Color TintColor { get; set; }/* = Colors.White;*/
        public double TintOpacity { get; set; } = 0.0;
        public double NoiseOpacity { get; set; } = 0.03;

        #endregion

        #region Constructor

        public AcrylicBrushExtension() { }

        #endregion

        #region Methods

        public AcrylicBrushExtension(string TargetName)
        {
            this.TargetName = TargetName;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var dst = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var target = dst.TargetObject as FrameworkElement;

            var AcrylicPanel = new AcrylicPanel()
            {
                TintColor = this.TintColor,
                TintOpacity = this.TintOpacity,
                NoiseOpacity = this.NoiseOpacity,
                Width = target.Width,
                Height = target.Height
            };

            BindingOperations.SetBinding(AcrylicPanel, AcrylicPanel.TargetProperty, new Binding() { ElementName = this.TargetName });
            BindingOperations.SetBinding(AcrylicPanel, AcrylicPanel.SourceProperty, new Binding() { Source = target });
            
            return new VisualBrush(AcrylicPanel)
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                ViewboxUnits = BrushMappingMode.Absolute,
            };
        }

        #endregion
    }
}