using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
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
                CenterX = 320,
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

        public static readonly DependencyProperty RollProperty = DependencyProperty.Register(nameof(Roll), typeof(float), typeof(AoACircle), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) =>  (obj as AoACircle).Roll = Convert.ToSingle(value.NewValue))));

        public float Roll
        {
            get { return Convert.ToSingle(GetValue(RollProperty)); }
            set
            {
                SetValue(RollProperty, value);
                RunOnUIThread(() => _rollTransform.Angle = value);
            }
        }

        public static readonly DependencyProperty PitchProperty = DependencyProperty.Register(nameof(Pitch), typeof(float), typeof(AoACircle), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as AoACircle).Pitch = Convert.ToSingle(value.NewValue))));
        public float Pitch
        {
            get { return Convert.ToSingle(GetValue(PitchProperty)); }
            set {
                SetValue(PitchProperty, value);
                RunOnUIThread(() => _pitchTransform.Y = value);
            }
        }
    }
}
