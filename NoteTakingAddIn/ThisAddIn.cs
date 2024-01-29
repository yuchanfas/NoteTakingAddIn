using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;

using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Debug = System.Diagnostics.Debug;

using System.Drawing;

using System.Runtime.InteropServices;
using System.Diagnostics;

using Microsoft.Office.Tools;



namespace NoteTakingAddIn
{
    public partial class ThisAddIn
    {
        private Dictionary<int, SlideKeywordInfo> slideKeywordsDictionary = new Dictionary<int, SlideKeywordInfo>();//
        public MyUserControl myUserControl;
        public Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;
        public KnowledgeMapArea MapArea;
        public WpfHostControl wpfHost;
        public ButtonsAra BtnAreas;
        public ButtonArea BtnArea;
        public CustomTaskPane MapPane;
        public CustomTaskPane BtnPane;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //---マーク付けウィンドの作成---//
            myUserControl = new MyUserControl();
            myCustomTaskPane = this.CustomTaskPanes.Add(myUserControl, "My Task Pane");
            myCustomTaskPane.Visible = true;
            myCustomTaskPane.Width = 260;

            //---マップ画面作成---//
            MapArea = new KnowledgeMapArea();
            wpfHost = new WpfHostControl();
            BtnAreas = new ButtonsAra();

            wpfHost.WpfElementControl.Child = MapArea;
            wpfHost.wpfElement2Control.Child = BtnAreas;
            //wpfHost.WpfElementControl.HostContainer.Children.Add(MapArea);
            MapPane = this.CustomTaskPanes.Add(wpfHost, "KnowledgeMap");
            //マップ表示するか
            MapPane.Visible = false;
            MapPane.Width = 800;
            MapArea.Width = 800;

            BtnArea = new ButtonArea();
            BtnPane = this.CustomTaskPanes.Add(BtnArea,"_");

            //ノードボタン表示
            BtnPane.Visible = false;
            BtnPane.Width = 260;

            // Event Setting
            this.Application.WindowSelectionChange += new PowerPoint.EApplication_WindowSelectionChangeEventHandler(Application_WindowSelectionChange);
            this.Application.SlideSelectionChanged += Application_SlideSelectionChanged;
            
            // this.Application.WindowBeforeRightClick += new PowerPoint.EApplication_WindowBeforeRightClickEventHandler(Application_WindowBeforeRightClick);
            // this.Application.WindowBeforeDoubleClick += new PowerPoint.EApplication_WindowBeforeDoubleClickEventHandler(Application_WindowBeforeDoubleClick);
        }






        // カスタマイズしたリボンをセット
        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
          return new CustomRibbon();
        }


        public void TouchKeyboardVisible(bool flag)
        {
            if (flag) Process.Start("C:/Program Files/Common Files/microsoft shared/ink/TabTip.exe");
            else
            {
                // タッチキーボードを非表示
                Process[] ps = Process.GetProcesses();
                foreach (Process p in ps)
                    if (p.ProcessName == "TabTip")
                    {
                        p.Kill();
                        Debug.WriteLine("Keyboard is Close");
                    }
            }
        }
        // スライド上で選択したテキストを取得。BtnAreaを押せばノードが作成される。


        //スライド上で選択した要素を取得。取得したものがテキストであれば出力
        private void Application_WindowSelectionChange(PowerPoint.Selection Sel)
        {
            try
            {
                if (Sel.Type == PowerPoint.PpSelectionType.ppSelectionText)
                {
                    TouchKeyboardVisible(false);
                    
                    var text = Sel.TextRange.Text;
                    BtnArea.setSelectingText(text);
                    Debug.WriteLine(text);
                }
                else if (Sel.Type == PowerPoint.PpSelectionType.ppSelectionShapes)
                {
                    string text = Sel.TextRange.Text;
                    if (text == null) return;

                    Sel.TextRange.Select();
                    
                    BtnArea.setSelectingText(text);
                    Debug.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ErrorMessage" + e.Message);
            }  
        }


        //---スライドのキーワードをListで保持---//
        public class SlideKeywordInfo
        {
            public int SlideIndex { get; set; }//slideインデックス
            public List<string> keywords { get; set; }//マークされたキーワードのリスト

            public SlideKeywordInfo(int slideIndex)
            {
                SlideIndex = slideIndex;
                keywords = new List<string>();
            }
        }


        //---選択しているスライドが変わった時の処理---//
        private void Application_SlideSelectionChanged(PowerPoint.SlideRange SldRange)
        {
            if (SldRange.Count > 0)
            {
                int selectedSlideIndex = SldRange[1].SlideIndex;
                List<string> keywordsForCurrentSlide = Globals.ThisAddIn.GetKeywordsForSlide(selectedSlideIndex);

                //リストボックスにキーワードを表示
                myUserControl.listBox1.Items.Clear();
                myUserControl.listBox1.Items.AddRange(keywordsForCurrentSlide.ToArray());
                BtnArea.listBox1.Items.Clear();
                BtnArea.listBox1.Items.AddRange(keywordsForCurrentSlide.ToArray());
                BtnAreas.listBox1.Items.Clear();
                BtnAreas.UpdateKeywords(keywordsForCurrentSlide);
            }
        }

        //---選択したテキストとそのslide情報をディクショナリに追加するメソッド---//
        //スライドごとにキーワード情報を更新
        public void AddSelectedTextToSlideKeyWords(string selectedText, int slideIndex)
        {
            if (!slideKeywordsDictionary.ContainsKey(slideIndex))
            {
                slideKeywordsDictionary[slideIndex] = new SlideKeywordInfo(slideIndex);
            }
            slideKeywordsDictionary[slideIndex].keywords.Add(selectedText);
        }

        //---現在のスライドのマーク付けしてあるキーワードを渡すメソッド---//
        public List<string> GetKeywordsForSlide(int slideIndex)
        {
            if (slideKeywordsDictionary.ContainsKey(slideIndex))
            {
                return slideKeywordsDictionary[slideIndex].keywords;
            }
            else
            {
                return new List<string>();//スライド内にマーク付けされたキーワードがない場合は空のリストを返す
            }
        }
        //---ListBoxを更新---//
        public void Update(int currentSlideIndex)
        {
            //ThisAddIn.csのメソッドを呼び出しキーワードをスライドごとに保存

            List<string> keywordsForCurrentSlide = Globals.ThisAddIn.GetKeywordsForSlide(currentSlideIndex);

            myUserControl.listBox1.Items.Clear();
            myUserControl.listBox1.Items.AddRange(keywordsForCurrentSlide.ToArray());

            BtnArea.listBox1.Items.Clear();
            BtnArea.listBox1.Items.AddRange(keywordsForCurrentSlide.ToArray());
            BtnAreas.UpdateKeywords(keywordsForCurrentSlide);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        /* 
        * タブレットでタッチしても反応しない
       internal void Application_WindowBeforeDoubleClick(PowerPoint.Selection Sel, ref bool Cancel)
       {
           Debug.WriteLine("DoubleClick");
       }
       // マウスの右クリックのみ反応
       void Application_WindowBeforeRightClick(PowerPoint.Selection Sel, ref bool Cancel)
       {
           Debug.WriteLine("RightClick");
           Cancel = false;
       }
        * */

        /* TODO:以下のプログラムを作る予定
         * １．タッチした要素を複製して同じ位置に貼り付ける
         * ２．タッチした要素から手を放すと消滅
         * ３．タッチした要素の×座標が1000pxを超えたら、右パネルに表示
         * OR
         * １．要素を長押し（右クリック）してメニュー表示
         * ２．メニューには「AddKeyword」ボタンを作っておく。タッチされたらノード生成
         * 
         * ***/

        #region VSTO で生成されたコード

        /// <summary>
        /// デザイナーのサポートに必要なメソッドです。
        /// このメソッドの内容をコード エディターで変更しないでください。
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
