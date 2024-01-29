using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Debug = System.Diagnostics.Debug;
using System.Diagnostics;

namespace NoteTakingAddIn
{
    /// <summary>
    /// KnowledgeMapArea.xaml の相互作用ロジック
    /// </summary>
    public partial class KnowledgeMapArea : UserControl
    {
        private Node _selectedNode;
        private Arrow _currentLink;
        private bool _isDraggingNode;
        private bool _isCreatingLink;
        private Point _lastMousePosition;
        private readonly Stopwatch _clickTimer = new Stopwatch();
        private int _clickCount;
        
        public KnowledgeMapArea()
        {
            InitializeComponent();
            //マウスイベントハンドラ設定
            this.MouseLeftButtonDown += Window_MouseLeftBUttonDown;
            //this.MouseLeftButtonUp += Window_MouseLeftButtonUp;
            this.MouseMove += Window_MouseMove;
            this.MouseLeftButtonUp += Window_MouseLeftButtonUp;
            this.MouseDoubleClick += window_MouseDoubleClick;
        }

        private void window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _lastMousePosition = e.GetPosition(this);//現在のマウスの位置取得
            Node hitNode = GetNodeFromPoint(_lastMousePosition);//クリックされた位置にノードがあるかノードの検出を行う
            _isCreatingLink = true;
            StartlinkCreation(_lastMousePosition);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _clickTimer.Stop();
            if(_clickCount == 1 && _clickTimer.ElapsedMilliseconds >= 500)//1秒以上ウで長押しと判定
            {
                    if (_isDraggingNode)
                    {
                        _isDraggingNode = false;
                        _selectedNode = null;
                    }
            }
            else if(_clickCount == 2 && _clickTimer.ElapsedMilliseconds >= 500)
            {
                FinishLinkDrag(e);
                _selectedNode = null;
                _isCreatingLink = false;
            }
            else
            {
                _isDraggingNode = false;
                _isCreatingLink = false;
            }

            _clickCount = 0;
 
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(this);
            if(_isDraggingNode && _selectedNode != null)
            {
                //マウスの移動量を計算
                double deltaX = currentMousePosition.X - _lastMousePosition.X;
                double deltaY = currentMousePosition.Y - _lastMousePosition.Y;
                //ノードのRenderTransformを取得
                if(_selectedNode.RenderTransform is MatrixTransform matrixTransform)
                {
                    Matrix matrix = matrixTransform.Matrix;//マトリックスに移動量を適用
                    matrix = SetDeletaTranslationForMouse(_selectedNode, matrix, currentMousePosition, _lastMousePosition);
                    _selectedNode.RenderTransform = new MatrixTransform(matrix);
                }
                else
                {
                    //RenderTransformがMatrixTransform出ない場合新しいMatrixTrnsformを作成
                    Matrix matrix = new Matrix();
                    matrix.Translate(deltaX, deltaY);
                    _selectedNode.RenderTransform = new MatrixTransform(matrix);
                }
               
                _selectedNode.updateLinksPosition(this);
                _lastMousePosition = currentMousePosition;
            }else if (_isCreatingLink)
            {
                UpdateLinkPosition(currentMousePosition);
            }
        }

        private void Window_MouseLeftBUttonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMousePosition = e.GetPosition(this);//現在のマウスの位置取得
            _clickCount = e.ClickCount;
            _clickTimer.Restart();

            Node hitNode = GetNodeFromPoint(_lastMousePosition);//クリックされた位置にノードがあるかノードの検出を行う
            if (hitNode != null && _clickCount == 1)
            {
                //ノードがある場合
                _selectedNode = hitNode;
                _isDraggingNode = true;
            }
            else
            {
                //ノードがない場合
                _isDraggingNode = false;
            }
        }
        private void UpdateLinkPosition(Point currentMousePosition)
        {
            if(_currentLink != null){
                _currentLink.X1 = currentMousePosition.X;
                _currentLink.Y1 = currentMousePosition.Y;
            }
        }
        private void FinishLinkDrag(MouseEventArgs e)
        {
            //現在のマウスの位置を取得
            Point mousePosition = e.GetPosition(this);
            //マウスの位置にあるノードを取得
            Node endNode = GetNodeFromPoint(mousePosition);
            //リンクの終了処理
            if(_currentLink != null)
            {
                if(endNode == null)
                {
                    grid1.Children.Remove(_currentLink);
                }
                else if(endNode.Equals(_currentLink.startNode)) 
                {
                    grid1.Children.Remove(_currentLink);
                }
                else
                {
                    _currentLink.endNode = endNode;
                }
                
            }

        }
        private readonly Stopwatch _doubleClicStopwatc = new Stopwatch();

        private bool IsDoubleClic(MouseButtonEventArgs e)
        {
            Point currentMousePosition = Mouse.GetPosition(this);
            bool clicksAreCloseInDistance = GetDistanceBetween(currentMousePosition, _lastTapLocation) < 40;
            _lastTapLocation = currentMousePosition;

            TimeSpan elapsed = _doubleClicStopwatc.Elapsed;
            _doubleClicStopwatc.Restart();
            bool cliclsAreCloseInTime = (elapsed != TimeSpan.Zero && elapsed < TimeSpan.FromSeconds(0.7));
            return clicksAreCloseInDistance && cliclsAreCloseInTime;
        }

        private readonly Stopwatch _longPressStopwatch = new Stopwatch();
        private bool IsLongPress(MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1 && e.ButtonState == MouseButtonState.Pressed)
            {
                //マウスがクリックされた瞬間に計測を開始
                _longPressStopwatch.Restart();
            }
            else if(e.ClickCount == 1 && e.ButtonState == MouseButtonState.Released)
            {
                //マウスが離れた瞬間に経過時間を確認
                TimeSpan elapsed = _longPressStopwatch.Elapsed;
                //経過時間が1秒で長押しとみなす
                return (elapsed > TimeSpan.FromSeconds(1));
            }
            else
            {
                //それ以外の場合、計測をリセット
            }
            return false;
        }

        /// <summary>
        /// リンクの終点を更新
        /// </summary>
        /// <returns></returns>
        /// 
        private void UpdateLinkEndPoint()
        {
            if(_currentLink == null || _currentLink.endNode == null)
            {
                //リンクの終点ノードの位置状態を取得
                Arrow.EndoNodePositionState positionState = _currentLink.PositionStateBetweenTwoNodes(this);
                //終点ノードの中心を取得
                Point endNodeCenter = _currentLink.endNode.CenterPoint(this);
                //終点ノードの寸法を取得
                double endNodeWidth = _currentLink.endNode.ActualWidth;
                double endNodeHeight = _currentLink.endNode.ActualHeight;

                //終点の座標を設定
                switch (positionState)
                {
                    case Arrow.EndoNodePositionState.MoreRight:
                        _currentLink.X2 = endNodeCenter.X + endNodeWidth / 2;
                        _currentLink.Y2 = endNodeCenter.Y;
                        break;
                    case Arrow.EndoNodePositionState.MoreLeft:
                        _currentLink.X2 = endNodeCenter.X - endNodeWidth / 2;
                        _currentLink.Y2 = endNodeCenter.Y;
                        break;
                    case Arrow.EndoNodePositionState.MoreTop:
                        _currentLink.X2 = endNodeCenter.X;
                        _currentLink.Y2 = endNodeCenter.Y - endNodeHeight / 2;
                        break;
                    case Arrow.EndoNodePositionState.MoreBottom:
                        _currentLink.X2 = endNodeCenter.X;
                        _currentLink.Y2 = endNodeCenter.Y + endNodeHeight / 2;
                        break;
                    // [他の状態に応じて追加のケースを設定]
                    default:
                        // エラー状態または未定義の状態の場合、何もしない
                        break;
                }
            }
        }

        // TODO
        // knowledge map left top point
        public Point Location()
        {
            WpfHostControl parent = Globals.ThisAddIn.wpfHost;
            Point location = this.TranslatePoint(new Point(0,0),null);
            return location;
        }

        public void CreateNode(string selectedText)
        {
            int currentSlideIndex = Globals.ThisAddIn.Application.ActiveWindow.Selection.SlideRange.SlideIndex;
            Node node = new Node(selectedText,currentSlideIndex);
            grid1.Children.Add(node);
        }

        Boolean LINK_DRAG_MODE = false; // ノードをダブルタップするとTRUEになり、リンクを生成する

        //node生成
        private void StartlinkCreation(Point startPoint)
        {
            _currentLink = new Arrow(_selectedNode,startPoint, this);
            grid1.Children.Add(_currentLink);
        }

        private Point _lastTapLocation;


        //ノード間の直線距離を求める
        public static double GetDistanceBetween(Point p, Point q)
        {
            double a = p.X - q.X;
            double b = p.Y - q.Y;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        //画面枠外にNodeが移動しないようにNode位置を設定
        //マウス用
        private Matrix SetDeletaTranslationForMouse(Node rect,Matrix matrix,Point currentMousePosition,Point LastMousePosition)
        {
            double deltaX = currentMousePosition.X -LastMousePosition.X;
            double deltaY = currentMousePosition.Y -LastMousePosition.Y;
            Point point = rect.CenterPoint(this);

            bool overLeft = (point.X < 0 && deltaX < 0);
            bool overTop = (point.Y < 0 && deltaY < 0);
            bool overRight = (point.X > this.ActualWidth && deltaX > 0);
            bool overBottom = (point.Y > this.ActualHeight && deltaY > 0);

            //画面外に出たら画面内に表示されるように位置座標を設定
            if (overLeft) matrix.OffsetX = -10;
            else if(overTop) matrix.OffsetY = -10;
            else if(overRight) matrix.OffsetX = -10;
            else if(overBottom) matrix.OffsetY = -10;
            else
            {
                //通常のドラッグ移動
                matrix.Translate(deltaX, deltaY);
            }
                  
            return matrix;
        }
        
        //指定された位置にノードがあるか
        //Canvas(grid1)上のどのNodeオブジェクト上にあるか判断
        private Node GetNodeFromPoint(Point p)
        {
           foreach(object c in grid1.Children)
               if(c is Node)//ノードオブジェクトか
               {
                   Node n = c as Node;
                   Point l = n.Location(this);//ノードの現在の位置を取得
                    //指定されたpがノードの領域内にあるか確認
                   if(l.X < p.X && l.X + n.ActualWidth >= p.X &&
                       l.Y < p.Y && l.Y + n.ActualHeight >= p.Y)
                       return n;
               }
           return null;
        }
        //JSONファイルに保存するメソッド

    }
}
