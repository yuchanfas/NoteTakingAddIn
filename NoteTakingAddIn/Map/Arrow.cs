using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Diagnostics;

using System.Collections.Generic;
using Attribute = NoteTakingAddIn.Map.Attribute;

namespace NoteTakingAddIn
{
    class Arrow : Shape
    {
        
        public Node startNode;    
        public Node endNode;      //矢の先端
        public Attribute attribute; // 属性

        public Arrow(Node node, Point e, Control parent)
        {
            Point location = node.Location(parent);
            this.Margin = new Thickness(0, 0, 0, 0);
            this.Fill = Brushes.Tomato;
            this.Stroke = Brushes.Tomato;
            this.StrokeThickness = 1.5;
            this.X1 = e.X;
            this.Y1 = e.Y;
            this.X2 = location.X + node.ActualWidth / 2;
            this.Y2 = location.Y + node.ActualHeight / 2;
            Canvas.SetZIndex(this, Properties.Settings.Default.ZindexBackground);
            this.Name = Properties.Settings.Default.DRAG_LINK; // set name to find dragging link
            this.startNode = node;

            CreateAttributeView();

        }

        private void CreateAttributeView()
        {
            Attribute attri = new Attribute(this);
            this.attribute = attri;
            Globals.ThisAddIn.MapArea.grid1.Children.Add(attribute);
        }
        
        public enum EndoNodePositionState
        {
            MoreBottom = 0,
            MoreLeft = 1,
            MoreLeftAndBottom = 2,
            MoreLeftAndTop = 3,
            MoreRight = 4,
            MoreRightAndBottom = 5,
            MoreRightAndTop = 6,
            MoreTop = 7,
            ErrorState = 8
        };

        public EndoNodePositionState PositionStateBetweenTwoNodes(Control parent)
        {
            // StartNodeの中心点からEndNodeの各端点（合計8点）の距離を計算して、
            // 最も短い距離の点からEndNodePositionを把握する
            if (startNode == null || endNode == null) return EndoNodePositionState.ErrorState;
            Point sp = this.startNode.Location(parent);
            Point ep = this.endNode.Location(parent);
            Point scp = this.startNode.CenterPoint(parent);
            Point ecp = this.endNode.CenterPoint(parent);
            double sw = this.startNode.ActualWidth;
            double sh = this.startNode.ActualHeight;
            double ew = this.endNode.ActualWidth;
            double eh = this.endNode.ActualHeight;
            Dictionary<Point,EndoNodePositionState> pointDic = new Dictionary<Point,EndoNodePositionState>()
            {
                {new Point(ep.X,ep.Y),EndoNodePositionState.MoreRightAndBottom},
                {new Point(ep.X+ew,ep.Y),EndoNodePositionState.MoreLeftAndBottom},
                {new Point(ep.X,ep.Y+eh),EndoNodePositionState.MoreRightAndTop},
                {new Point(ep.X+ew,ep.Y+eh),EndoNodePositionState.MoreLeftAndTop},
                {new Point(ecp.X,ep.Y),EndoNodePositionState.MoreBottom},
                {new Point(ecp.X,ep.Y+eh),EndoNodePositionState.MoreTop},
                {new Point(ep.X,ecp.Y),EndoNodePositionState.MoreRight},
                {new Point(ep.X+ew,ecp.Y),EndoNodePositionState.MoreLeft}
            };
            double minDistance = -1;
            Point nearestPoint = new Point();
            foreach (Point p in pointDic.Keys)
            {
                double dis = GetDistanceBetween(scp, p);
                if (minDistance == -1 || minDistance > dis)
                {
                    nearestPoint = p;
                    minDistance = dis;
                }
            }
            EndoNodePositionState state;
            if (pointDic.TryGetValue(nearestPoint, out state)) return state;
            else return EndoNodePositionState.ErrorState;
        }

        private static double GetDistanceBetween(Point p, Point q)
        {
            double a = p.X - q.X;
            double b = p.Y - q.Y;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        public void updateAttriubtePosition(Control parent)
        {
            Arrow link = this;
            NoteTakingAddIn.Map.Attribute attri = this.attribute;
            double left = link.X1 + (link.X2 - link.X1) * 2 / 5; // 少し先端に近い位置に属性を配置
            double top = link.Y1 + (link.Y2 - link.Y1) * 2 / 5;
            Canvas.SetLeft(attri, left - attri.ActualWidth / 2);
            Canvas.SetTop(attri, top - attri.ActualHeight / 2);
        }

        /*
         * こっから下は外部サイトからのコピペ
         * 
         * *****************************************************************/


        public static readonly DependencyProperty X1Property; // 矢じり側のX1 座標
        public static readonly DependencyProperty Y1Property; // 同、Y1 座標
        public static readonly DependencyProperty X2Property;
        public static readonly DependencyProperty Y2Property;
        public static readonly DependencyProperty ArrowLengthProperty; // 矢じり部の長さ
        public static readonly DependencyProperty ArrowWidthProperty;  // 同、幅（幅を長さの2 倍にすると直交三角形になる


        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get { return (double)base.GetValue(X1Property); }
            set { base.SetValue(X1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get { return (double)base.GetValue(Y1Property); }
            set { base.SetValue(Y1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double ArrowLength
        {
            get { return (double)base.GetValue(ArrowLengthProperty); }
            set { base.SetValue(ArrowLengthProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double ArrowWidth
        {
            get { return (double)base.GetValue(ArrowWidthProperty); }
            set { base.SetValue(ArrowWidthProperty, value); }
        }

        // コントロールの形状を定義する
        protected override Geometry DefiningGeometry
        {
            get
            {
                // 直線部の長さ
                double length = Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));

                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = new Point(this.X1, this.Y1 - length); // 矢じりでない側の位置

                Point[] pointArray = new Point[4];
                pointArray[0] = new Point(this.X1, this.Y1); // 矢じりの先端
                pointArray[1] = new Point(this.X1 - ArrowWidth / 2, this.Y1 - ArrowLength);
                pointArray[2] = new Point(this.X1 + ArrowWidth / 2, this.Y1 - ArrowLength);
                pointArray[3] = new Point(this.X1, this.Y1);

                pathFigure.Segments.Add(new PolyLineSegment(pointArray, true));
                pathFigure.IsFilled = true;

                PathGeometry geometry = new PathGeometry();
                geometry.FillRule = FillRule.Nonzero;
                geometry.Figures.Add(pathFigure);

                // 以上の操作は、垂直に立てた状態のときの形状である。次に、角度をつけるために座標変換する
                double angle = 180 - Math.Atan2(this.X2 - this.X1, this.Y2 - this.Y1) * 180 / Math.PI;
                RotateTransform transform = new RotateTransform(angle, this.X1, this.Y1);
                geometry.Transform = transform;

                return geometry;
            }
        }

        //-----------------------------------------------------------------------------------
        // コンストラクタ
        static Arrow()
        {
            X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Arrow),
              new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
            Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Arrow),
              new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
            X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Arrow),
              new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
            Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Arrow),
              new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
            ArrowLengthProperty = DependencyProperty.Register("ArrowLength", typeof(double), typeof(Arrow),
              new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
            ArrowWidthProperty = DependencyProperty.Register("AllowWidth", typeof(double), typeof(Arrow),
              new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        public Arrow()
        {
            this.Stroke = Brushes.Black;
            this.Fill = this.Stroke;
            this.SnapsToDevicePixels = true;
        }

       
    } // end of Arrow class
} // end of namespace
