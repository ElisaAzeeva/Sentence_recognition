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
                sp.Children.Add(new Rectangle { Width = 2, Height = 1, Fill = Brushes.Black });
                sp.Children.Add(new Rectangle { Width = 2, Height = 4 });
                sp.Children.Add(new Rectangle { Width = 2, Height = 1, Fill = Brushes.Black });

                
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
                        }, 0.2)
                    }
                };
            }

            // Init WavyUnderline
            {
                var h = 2;
                var w = 2;

                WavyUnderline = new TextDecorationCollection {
                    new TextDecoration {
                        PenThicknessUnit = TextDecorationUnit.FontRenderingEmSize,
                        //PenOffset = 1,
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
                                StrokeThickness = 0.8,
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
                        }, 0.2*h)
                    }
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