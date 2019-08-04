using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
{
    public class ArtificialHorizon : HudControlBase
    {
        Polygon _sky;
        Polygon _ground;

        public ArtificialHorizon()
        {

            _sky = new Polygon();
            _sky.Points = new PointCollection()
            {
                new Windows.Foundation.Point(0, 0),
                new Windows.Foundation.Point(640, 0),
                new Windows.Foundation.Point(640, 240),
                new Windows.Foundation.Point(0, 240),
                new Windows.Foundation.Point(0, 0),
            };

            _sky.Fill = new LinearGradientBrush(new GradientStopCollection()
                {
                    new GradientStop()
                    {
                         Color = Colors.SkyBlue,
                         Offset = 0
                    },
                    new GradientStop()
                    {
                        Color = Colors.LightBlue,
                        Offset = 1
                    }
                }, 90);

            _ground = new Polygon();
            _ground.Fill = new LinearGradientBrush(new GradientStopCollection()
                {
                    new GradientStop()
                    {
                         Color = Colors.LightGreen,
                         Offset = 0
                    },
                    new GradientStop()
                    {
                        Color = Colors.DarkOliveGreen,
                        Offset = 1
                    }
                }, 90);

            _ground.Points = new PointCollection()
            {
                new Windows.Foundation.Point(0, 240),
                new Windows.Foundation.Point(640, 240),
                new Windows.Foundation.Point(640, 480),
                new Windows.Foundation.Point(0, 480),
                new Windows.Foundation.Point(0, 240),
            };

            Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, 640, 580)
            };

            Children.Add(_sky);
            Children.Add(_ground);
        }

        public Point P1
        {
            set
            {
                _sky.Points[3] = value;

                _ground.Points[0] = value;
                _ground.Points[4] = value;
            }
        }

        public Point P2
        {
            set
            {
                _sky.Points[2] = value;
                _ground.Points[1] = value;
            }
        }

        private void SetHorizon()
        {
            var pitch = 240 + Pitch * 5;

            var y1 = pitch - (Math.Sin((Roll * Math.PI) / 180.0f) * 320);
            var y2 = pitch + (Math.Sin((Roll * Math.PI) / 180.0f) * 320);

            P1 = new Point(0, y1);
            P2 = new Point(640, y2);
        }

        public static DependencyProperty RollProperty = DependencyProperty.Register(nameof(Roll), typeof(float), typeof(ArtificialHorizon), new PropertyMetadata(null, new PropertyChangedCallback(OnRollChanged)));

        private static void OnRollChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as ArtificialHorizon).Roll = Convert.ToSingle(args.NewValue);
        }

        public float Roll
        {
            get { return Convert.ToSingle(GetValue(RollProperty)); }
            set
            {
                SetValue(RollProperty, value);
                RunOnUIThread(() => SetHorizon());

            }
        }

        public static DependencyProperty PitchProperty = DependencyProperty.Register(nameof(Pitch), typeof(float), typeof(ArtificialHorizon), new PropertyMetadata(null, new PropertyChangedCallback(OnPitchChanged)));

        private static void OnPitchChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as ArtificialHorizon).Pitch = Convert.ToSingle(args.NewValue);
        }

        public float Pitch
        {
            get { return Convert.ToSingle(GetValue(PitchProperty)); }
            set
            {
                SetValue(PitchProperty, value);
                RunOnUIThread(() => SetHorizon());
            }
        }
    }
}
