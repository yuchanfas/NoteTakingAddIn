using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;

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

namespace NoteTakingAddIn.Map
{
    class Attribute:Viewbox
    {
        Arrow Link;

        public Attribute(Arrow link)
        {
            this.Link = link;

            Label label = new Label();
            label.Content = "   ";
            label.FontSize = 11;
            label.VerticalContentAlignment = VerticalAlignment.Stretch;
            label.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            label.Background = Brushes.OrangeRed;
            label.BorderThickness = new Thickness(1);
            label.BorderBrush = Brushes.White;
            label.Foreground = Brushes.White;

            this.Child = label;
            this.Stretch = Stretch.Fill;
            this.TouchUp += new EventHandler<TouchEventArgs>(TouchUpAttribute);
            Canvas.SetLeft(this, -100); // 生成時はユーザに見えない位置に表示
            Canvas.SetTop(this, -100);  // リンクをノードにつけると見える位置に表示
            Canvas.SetZIndex(this, Properties.Settings.Default.ZindexForeground);

            // 長押し時に表示するメニュー
            ContextMenu menuList = new ContextMenu();
            MenuItem selectAttrItem = new MenuItem();
            selectAttrItem.Header = "属性選択する";
            selectAttrItem.Height = 50;
            selectAttrItem.Click += SelectAttrItem_Click;
            menuList.Items.Add(selectAttrItem);
            MenuItem compareSlidesItem = new MenuItem();
            compareSlidesItem.Header = "スライドを比較";
            compareSlidesItem.Height = 40;
            compareSlidesItem.Click += CompareSlidesItem_Click;
            menuList.Items.Add(compareSlidesItem);
            MenuItem deleteLinkItem = new MenuItem();
            deleteLinkItem.Header = "リンク削除";
            deleteLinkItem.Height = 30;
            deleteLinkItem.Click += DeleteLinkItem_Click;
            menuList.Items.Add(deleteLinkItem);
            
            this.ContextMenu = menuList;
            this.Link.ContextMenu = menuList;
        }
        //属性の内容を取得
        public string GetContent()
        {
            return ((Label)this.Child).Content as String;
        }

        
        // 属性のメニュー表示
        private void TouchUpAttribute(object sender, TouchEventArgs e)
        {
            Attribute attri = (Attribute)sender;
            UserControl parent = ((Canvas)attri.Parent).Parent as UserControl;
            Canvas.SetLeft(attri.ContextMenu, e.GetTouchPoint(parent).Position.X);
            Canvas.SetTop(attri.ContextMenu, e.GetTouchPoint(parent).Position.Y);
            attri.ContextMenu.IsOpen = true;
        }

        void SelectAttrItem_Click(object sender, RoutedEventArgs e)
        {
            //他に選択中のAttributeViewがないか確認
            UIElementCollection children = Globals.ThisAddIn.MapArea.grid1.Children;
            foreach (UIElement c in children)
            {
                if (!(c is Attribute)) continue;
                Attribute _attri = (Attribute)c;
                if (_attri.Name.Equals(Properties.Settings.Default.TOUCHUP_ATTRIBUTE))
                {
                    _attri.Name = Properties.Settings.Default.NO_NAME;
                    ((Label)_attri.Child).Background = Brushes.OrangeRed;
                    ((Label)_attri.Child).Content = "未入力";
                    // TODO: 文字幅が変わったらAttributeViewの位置がリンク中心に更新されて欲しい
                    Control parent = (this.Parent as Canvas).Parent as Control;
                    _attri.Link.attribute = _attri;
                    this.Link.updateAttriubtePosition(parent);
                    break;
                }
            }
            // 属性選択モード-オン
            this.Name = Properties.Settings.Default.TOUCHUP_ATTRIBUTE;
            Label label = (Label)this.Child;
            label.Background = Brushes.Orange;
            label.Content = "属性を選択してください";
        }

        //スライド画面にStartNodeとEndNodeのスライドを二つ表示
        void CompareSlidesItem_Click(object sender, RoutedEventArgs e)
        {
            Node sNode = this.Link.startNode;
            Node eNode = this.Link.endNode;
            
            var sSlide = Globals.ThisAddIn.Application.ActivePresentation.Slides[sNode.SlideIndex];
            sSlide.Select();
            
            /* TODO:スライドの表示倍率の変更および画像の表示
             * Microsoft.Office.Interop.PowerPoint.View view = new Microsoft.Office.Interop.PowerPoint.View();
             * view.Zoom;
            /* 
             * .SlideWidth = 29.7 * 72 / 2.54
　　         * .SlideHeight = 21 * 72 / 2.54
             * */
            Globals.ThisAddIn.Application.ActivePresentation.PageSetup.SlideWidth = (float)(20 * 72 / 2.54);
            Globals.ThisAddIn.Application.ActivePresentation.PageSetup.SlideHeight = (float)(15 * 72 / 2.54);

        }

        private void DeleteLinkItem_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("DeleteLinkItem");

            if (MessageBoxResult.OK == MessageBox.Show("本当に削除しますか？", "リンク削除", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation))
            {

                Globals.ThisAddIn.MapArea.grid1.Children.Remove(this);
                Globals.ThisAddIn.MapArea.grid1.Children.Remove(this.Link);
            }

        }

        private void updateLabelText(string text)
        {
            (this.Child as Label).Content = text;
            this.Link.attribute = this; // 属性サイズを更新（意味あるの？）
        }

    }
}
