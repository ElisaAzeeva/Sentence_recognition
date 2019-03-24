using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CommonLib
{
    public static class MyTextDecorations
    {
        // Возможно лучше переписать это как цвета
        // Т.к. подчеркивания плохо видно.
        // Также все это не очень хороший код 
        // который работает только при 15 шрифте.

        // TODO: Добавить константу цвета.
        // TODO: Исправить ошибки с отображением волнистой и
        // двойного подчеркивания при изменении шрифта.

        static MyTextDecorations()
        {
            // Black magic.

            // Init Underline
            {
                Underline = new TextDecorationCollection
                {
                    new TextDecoration
                    {
                        PenThicknessUnit = TextDecorationUnit.FontRenderingEmSize,
                        PenOffset = 1,
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(Brushes.Black, 0.1) 
                    }
                };
                Underline.Freeze();
            }

            // Init DoubleUnderline
            {

                var sp = new StackPanel();
                sp.Children.Add(new Rectangle { Width = 1, Height = 1, Fill = Brushes.Black});
                sp.Children.Add(new Rectangle { Width = 1, Height = 4});
                sp.Children.Add(new Rectangle { Width = 1, Height = 1, Fill = Brushes.Black});

                DoubleUnderline = new TextDecorationCollection
                {
                    new TextDecoration
                    {
                        PenThicknessUnit = TextDecorationUnit.FontRenderingEmSize,
                        PenOffset = 1,
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(new VisualBrush
                        {
                            TileMode = TileMode.Tile,
                            Visual = sp,
                        }, 0.25)
                    }
                };
            }

            // Init WavyUnderline
            {
                Pen path_pen = new Pen(new SolidColorBrush(Colors.Black), 0.2);
                path_pen.EndLineCap = PenLineCap.Square;
                path_pen.StartLineCap = PenLineCap.Square;

                Point path_start = new Point(0, 1);
                BezierSegment path_segment = new BezierSegment(new Point(1, 0), new Point(2, 2), new Point(3, 1), true);
                PathFigure path_figure = new PathFigure(path_start, new PathSegment[] { path_segment }, false);
                PathGeometry path_geometry = new PathGeometry(new PathFigure[] { path_figure });

                DrawingBrush squiggly_brush = new DrawingBrush();
                squiggly_brush.Viewport = new Rect(0, 0, 6, 5);
                squiggly_brush.ViewportUnits = BrushMappingMode.Absolute;
                squiggly_brush.TileMode = TileMode.Tile;
                squiggly_brush.Drawing = new GeometryDrawing(null, path_pen, path_geometry);

                TextDecoration squiggly = new TextDecoration();
                squiggly.Pen = new Pen(squiggly_brush, 5);


                WavyUnderline = new TextDecorationCollection {
                    squiggly
                };
            }

            // Init DashedUnderline
            {
                DashedUnderline = new TextDecorationCollection {
                    new TextDecoration
                    {
                        PenThicknessUnit = TextDecorationUnit.FontRenderingEmSize,
                        PenOffset = 1,
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(Brushes.Black, 0.1)
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
                        PenThicknessUnit = TextDecorationUnit.FontRenderingEmSize,
                        PenOffset = 1,
                        Location = TextDecorationLocation.Underline,
                        Pen = new Pen(Brushes.Black, 0.1)
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

        public static TextDecorationCollection GetDecorationFromType(SentenceMembers type)
        {
            switch (type)
            {
                case SentenceMembers.Subject:
                    return Underline;
                case SentenceMembers.Predicate:
                    return DoubleUnderline;
                case SentenceMembers.Definition:
                    return WavyUnderline;
                case SentenceMembers.Circumstance:
                    return DashDotedUnderline;
                case SentenceMembers.Addition:
                    return DashedUnderline;
                default:
                    return null;
            }
        }

    }
}