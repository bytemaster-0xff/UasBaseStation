﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
{
    public class Compass : HudControlBase
    {
        Canvas _compass;
        TextBlock _headingLabel;
        RotateTransform _compassTransform;

        public Compass()
        {
            Width = 200;
            Height = 60;
            Clip = new RectangleGeometry() { Rect = new Windows.Foundation.Rect() { Height = 60, Width = 250 } };
            
            _compass = new Canvas()
            {
                Width = 200,
                Height = 200,
                Margin = new Thickness(0, 0, 0, 0)
            };

            _compassTransform = new RotateTransform()
            {
                Angle = -90,
                CenterX = 100,
                CenterY = 100,
            };

            _compass.RenderTransform = _compassTransform;

            _compass.Children.Add(new Ellipse()
            {
                Stroke = ForegroundBrush,
                StrokeThickness = 2,
                Width = 200,
                Height = 200,
            });

            for (var idx = 0; idx < 360; idx += 15)
            {
                var line = new Line()
                {
                    Stroke = ForegroundBrush,
                    StrokeThickness = 2,
                    Height = 200,
                    Width = 200,
                    X1 = 100 + Math.Cos((idx * Math.PI) / 180.0f) * ((idx % 30 == 0) ? 93 : 80),
                    Y1 = 100 + Math.Sin((idx * Math.PI) / 180.0f) * ((idx % 30 == 0) ? 93 : 80),
                    X2 = 100 + Math.Cos((idx * Math.PI) / 180.0f) * 100,
                    Y2 = 100 + Math.Sin((idx * Math.PI) / 180.0f) * 100,
                };

                _compass.Children.Add(line);

                if (idx % 30 == 0)
                {
                    var indicatorText = idx.ToString();
                    var fontSize = 12;
                    var top = 80;
                    switch (idx)
                    {
                        case 0: indicatorText = "N"; fontSize = 14; top = 85; break;
                        case 90: indicatorText = "E"; fontSize = 14; top = 85; break;
                        case 180: indicatorText = "S"; fontSize = 14; top = 85; break;
                        case 270: indicatorText = "W"; fontSize = 14; top = 85; break;
                    }

                    var txt = new TextBlock()
                    {
                        Text = indicatorText,
                        FontSize = fontSize,
                        Width = 30,
                        Height = 20,
                        Margin = new Thickness(-15, -10, 0, 0),

                        TextAlignment = TextAlignment.Center,
                        Foreground = ForegroundBrush,
                        RenderTransform = new RotateTransform()
                        {
                            Angle = idx + 90,
                            CenterX = 15,
                            CenterY = 10,
                        }
                    };

                    txt.SetValue(Canvas.LeftProperty, 100 + (Math.Cos((idx * Math.PI) / 180.0f) * 85));
                    txt.SetValue(Canvas.TopProperty, 100 + (Math.Sin((idx * Math.PI) / 180.0f) * 85));
                    _compass.Children.Add(txt);
                }
            }

            _headingLabel = new TextBlock
            {
                FontSize = 20,
                Foreground = ForegroundBrush,
                Margin = new Thickness(0, 30, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Children.Add(_headingLabel);
            Children.Add(_compass);
        }

        public static DependencyProperty HeadingProperty = DependencyProperty.Register(nameof(Heading), typeof(float), typeof(Compass), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as Compass).Heading = Convert.ToSingle(value.NewValue))));
        public Single Heading
        {
            get { return Convert.ToSingle(GetValue(HeadingProperty)); }
            set
            {
                SetValue(HeadingProperty, value);
                RunOnUIThread(() =>
                {
                    _headingLabel.Text = $"{value:0.0}°";
                    _compassTransform.Angle = 270 - value;
                });
            }
        }
    }
}
