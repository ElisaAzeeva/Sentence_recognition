using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows
{
    public static class MyTextDecorations
    {

        static MyTextDecorations()
        {
            {
                // Init Underline
                TextDecoration td = new TextDecoration();
                td.Location = TextDecorationLocation.Underline;
                Underline = new TextDecorationCollection();
                Underline.Add(td);
                Underline.Freeze();
            }

            {
                // Init DoubleUnderline
                DoubleUnderline = new TextDecorationCollection
                {
                    new TextDecoration
                    {
                        Location = TextDecorationLocation.Underline
                    }
                };
                DoubleUnderline.Freeze();
            }

            // Init WavyUnderline
            {
                var h = 2;
                var w = 2;

                WavyUnderline = new TextDecorationCollection {
                    new TextDecoration {
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(new VisualBrush
                        {
                            Stretch = Stretch.None,
                            TileMode = TileMode.Tile,
                            ViewportUnits = BrushMappingMode.Absolute,
                            Viewport = new Rect(0, 0, 3 * w, 3 * h),
                            Visual = new Path
                            {
                                Stroke = Brushes.Black,
                                StrokeThickness = 1/2d,
                                StrokeEndLineCap = PenLineCap.Square,
                                StrokeStartLineCap = PenLineCap.Square,
                                Data = new PathGeometry(
                                    new List<PathFigure>{
                                        new PathFigure(
                                            new Point(0,h),
                                            new List<BezierSegment>{
                                                new BezierSegment(
                                                    new Point(w,0),
                                                    new Point(2*w,2*h),
                                                    new Point(3*w,h),
                                                    true)
                                            },
                                            false
                                            )
                                    })
                            }
                        }, 6*h)
                    }
                };

                //WavyUnderline.Freeze();
            }

            // Init DashedUnderline
            {
                DashedUnderline = new TextDecorationCollection {
                    new TextDecoration
                    {
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(Brushes.Black, 1)
                        {
                            DashStyle = new DashStyle(new List<double> { 4, 4 }, 0)
                        }
                    }
                };
                DashedUnderline.Freeze();
            }

            // Init DashDotedUnderline
            {
                DashDotedUnderline = new TextDecorationCollection {
                    new TextDecoration
                    {
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(Brushes.Black, 1)
                        {
                            DashStyle = new DashStyle(new List<double> { 4, 4, 0, 4 }, 0)
                        }
                    }
                };
                DashDotedUnderline.Freeze();
            }
        }


        public static TextDecorationCollection Underline { get; }

        public static TextDecorationCollection DoubleUnderline { get; }

        public static TextDecorationCollection WavyUnderline { get; }

        public static TextDecorationCollection DashedUnderline { get; }

        public static TextDecorationCollection DashDotedUnderline { get; }
    }
}