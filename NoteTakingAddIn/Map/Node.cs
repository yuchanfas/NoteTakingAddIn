using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
// using System.Windows.Forms;
using System.Collections;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows;
using Debug = System.Diagnostics.Debug;
using NoteTakingAddIn;

using System.Windows.Threading;


namespace NoteTakingAddIn
{

    class Node : Viewbox
    {

        // ノード生成時のスライド順番｜ノードをタッチ→スライドを表示 & サムネイルをハイライト｜
        public readonly int SlideIndex; 

        public Node(string _text,int _slideIndex)
        {
            Label keyword = new Label();
            keyword.Content = _text;
            this.SlideIndex = _slideIndex;
            keyword.FontSize = 12;
            keyword.VerticalContentAlignment = VerticalAlignment.Stretch;
            keyword.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            keyword.Background = Brushes.White;
            keyword.BorderThickness = new Thickness(1);
            keyword.BorderBrush = Brushes.Black;
            this.Child = keyword;
            this.Stretch = Stretch.Fill;
            this.IsManipulationEnabled = true;
            Canvas.SetZIndex(this, Properties.Settings.Default.ZindexForeground);

            MatrixTransform matrix = new MatrixTransform();
            matrix.SetValue(NameProperty, "matrixTransform");
            this.RenderTransform = matrix;
            
            // 長押し時に表示するコンテキストメニュー 
            ContextMenu menuList = new ContextMenu();
            MenuItem showSlideItem = new MenuItem();
            showSlideItem.Header = "スライド表示";
            showSlideItem.Height = 50;
            showSlideItem.Click += showSlideItem_Click;
            menuList.Items.Add(showSlideItem);
            MenuItem deleteNodeItem = new MenuItem();
            deleteNodeItem.Header = "ノード削除";
            deleteNodeItem.Height = 30;
            deleteNodeItem.Click += deleteNodeItem_Click;
            menuList.Items.Add(deleteNodeItem);
            this.ContextMenu = menuList;
        }
        //Nodeのテキストを取得
        public string GetText()
        {
            if(this.Child is Label label)
            {
                return label.Content.ToString();
            }
            return string.Empty;
        }
        void showSlideItem_Click(object sender, RoutedEventArgs e)
        {
            var slide = Globals.ThisAddIn.Application.ActivePresentation.Slides[this.SlideIndex];
            slide.Select();
        }

        // リンクと属性とノードを削除
        private void deleteNodeItem_Click(object sender, RoutedEventArgs e)
        {
            ArrayList deleteLinks = new ArrayList();
            UIElementCollection children = Globals.ThisAddIn.MapArea.grid1.Children;
            foreach (UIElement c in children)
            {
               if (!(c is Arrow)) continue;
               Arrow link = c as Arrow;
               if (link.startNode.Equals(this) || link.endNode.Equals(this))
                    deleteLinks.Add(link);
            }
            foreach(Arrow l in deleteLinks)
            {
                Globals.ThisAddIn.MapArea.grid1.Children.Remove(l);
                Globals.ThisAddIn.MapArea.grid1.Children.Remove(l.attribute);
            }
            Globals.ThisAddIn.MapArea.grid1.Children.Remove(this);
            
        }

        private Boolean HasLinks()
        {
            UIElementCollection children = Globals.ThisAddIn.MapArea.grid1.Children;
            foreach (object c in children)
            {
                if (!(c is Arrow)) continue;
                Arrow link = c as Arrow;
                if (link.startNode == null) continue;
                if (link.startNode.Equals(this)) return true;
                if (link.endNode.Equals(this)) return true;
            }
            return false;
        }

        // node's left top point
        private Point _location;
        public Point Locatoin
        {
            get { return _location; }
        }
        public void SetLocation(Point p,EventArgs e)
        {
            if(e is ManipulationStartingEventArgs || 
                e is ManipulationCompletedEventArgs ||
                e is ManipulationInertiaStartingEventArgs)
            {
                _location = p;
            }else
                Debug.WriteLine("Set Location is only used in Touch Event");
        }

        // nodes' left top point 
        public Point Location(Control parent)
        {
            Point location = this.TranslatePoint(new Point(0,0),parent);
            return location;
        }
        // node's center point
        public Point CenterPoint(Control parent)
        {
            Point location = this.TranslatePoint(new Point(0, 0), parent);
            Point center = new Point(location.X + this.ActualWidth / 2, location.Y + this.ActualHeight / 2);
            return center;
        }

        // ノードをドラッグしたらリンクも合わせて移動
        public void updateLinksPosition(Control parent)
        {
            if (!this.HasLinks()) return;

            ArrayList pointedLinks = new ArrayList(); // 「ノード→」の矢印群
            ArrayList pointingLinks = new ArrayList(); // 「→ノード」の矢印群
            UIElementCollection children = Globals.ThisAddIn.MapArea.grid1.Children;
            foreach (object c in children)
            {
                if (!(c is Arrow)) continue;
                Arrow link = c as Arrow;
                if (link.startNode == null) continue;

                //startNodeにこのノードがあるか | endNodeにこのノードがあるか
                if (link.startNode.Equals(this)) pointingLinks.Add(link);
                else if (link.endNode.Equals(this)) pointedLinks.Add(link);
            }

            // 「ノード→」or「→ノード」によって処理を変える
            Debug.WriteLine("moveLinks:");
            foreach (Arrow l in pointedLinks)
                setLinkTipPositionFrom(l, this, parent);
            
            foreach (Arrow l in pointingLinks)
            {
                l.X2 = CenterPoint(parent).X;
                l.Y2 = CenterPoint(parent).Y;

                setLinkTipPositionFrom(l, l.endNode, parent);
            }
        }

        // 矢印先端の位置座標を、引数nodeの位置座標から設定する
        private void setLinkTipPositionFrom(Arrow l, Node node, Control parent)
        {
            Arrow.EndoNodePositionState positionState = l.PositionStateBetweenTwoNodes(parent);
            Debug.WriteLine("PositionStateBetween:" + positionState);
            if (positionState == Arrow.EndoNodePositionState.MoreRightAndTop)
            {
                l.X1 = node.Location(parent).X;
                l.Y1 = node.Location(parent).Y + node.ActualHeight;
            }
            else if (positionState == Arrow.EndoNodePositionState.MoreRightAndBottom)
            {
                l.X1 = node.Location(parent).X;
                l.Y1 = node.Location(parent).Y;
            }
            else if (positionState == Arrow.EndoNodePositionState.MoreLeftAndTop)
            {
                l.X1 = node.Location(parent).X + node.ActualWidth;
                l.Y1 = node.Location(parent).Y + node.ActualHeight;
            }
            else if (positionState == Arrow.EndoNodePositionState.MoreLeftAndBottom)
            {
                l.X1 = node.Location(parent).X + node.ActualWidth;
                l.Y1 = node.Location(parent).Y;
            }
            else if (positionState == Arrow.EndoNodePositionState.MoreLeft)
            {
                l.X1 = node.Location(parent).X + node.ActualWidth;
                l.Y1 = l.Y2;
            }
            else if (positionState == Arrow.EndoNodePositionState.MoreRight)
            {
                l.X1 = node.Location(parent).X;
                l.Y1 = l.Y2;
            }
            else if (positionState == Arrow.EndoNodePositionState.MoreTop)
            {
                l.X1 = l.X2;
                l.Y1 = node.Location(parent).Y + node.ActualHeight;
            }
            else if (positionState == Arrow.EndoNodePositionState.MoreBottom)
            {
                l.X1 = l.X2;
                l.Y1 = node.Location(parent).Y;
            }
            else if (positionState == Arrow.EndoNodePositionState.ErrorState)
            {
                // None
            }

            l.updateAttriubtePosition(parent);
        }

        

    }

}