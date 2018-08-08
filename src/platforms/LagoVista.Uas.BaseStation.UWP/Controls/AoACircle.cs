using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LagoVista.Uas.BaseStation.UWP.Controls
{
    public class AoACircle : HudControlBase
    {
        RotateTransform _rollTransform;
        TranslateTransform _pitchTransform;

        public AoACircle()
        {
            Width = 480;
            Height = 480;
            _rollTransform = new RotateTransform()
            {
                Angle = 0,
                CenterX = 240,
                CenterY = 240
            };

            RenderTransform = _rollTransform;

            Children.Add(new Ellipse()
            {
                Width = 300,
                Height = 300,
                Stroke = ForegroundBrush,
                StrokeThickness = 2,
            });

            _pitchTransform = new TranslateTransform();

            for (var idx = -50; idx <= 50; idx += 10)
            {
                var len = idx == 0 ? 240 : (idx % 20 == 0) ? 120 : 90;
                Children.Add(new Line()
                {
                    Stroke = ForegroundBrush,
                    StrokeThickness = 2,
                    RenderTransform = _pitchTransform,
                    Height = 4,
                    Width = len,
                    X1 = 0,
                    X2 = len,
                    Y1 = 0,
                    Y2 = 0,
                    Margin = new Thickness(0, idx * 5, 0, 0),
                    StrokeDashArray = new DoubleCollection() { 5, 2 },
                });
            }
        }

        double _roll;
        public double Roll
        {
            get { return _roll; }
            set
            {
                _roll = value;
                _rollTransform.Angle = Roll;
            }
        }

        double _pitch;
        public double Pitch
        {
            get { return _pitch; }
            set
            {
                _pitch = value;

            }
        }

    }
}
