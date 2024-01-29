using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace NoteTakingAddIn
{
    public partial class MyUserControl : UserControl
    {
        public MyUserControl()
        {
            InitializeComponent();
        }

        private void Markbutton_Click(object sender, EventArgs e)
        {
            //ハイライト
            PowerPoint.Application app = Globals.ThisAddIn.Application;
            PowerPoint.Presentation pres = app.ActivePresentation;

            // 現在のスライドのインデックスを取得する
            int currentSlideIndex = Globals.ThisAddIn.Application.ActiveWindow.Selection.SlideRange.SlideIndex;
                
            
            if (app.ActiveWindow.Selection.Type == PowerPoint.PpSelectionType.ppSelectionText)
            {
                Microsoft.Office.Core.TextRange2 selectedText1 = app.ActiveWindow.Selection.TextRange2;
                Microsoft.Office.Core.Font2 textFont = selectedText1.Font;
                //既存のフォント情報を保持
                float originalSize = textFont.Size;
                textFont.Highlight.RGB = (255) | (255 << 8) | (0 << 16);
                //保存しておいたフォント情報を再適用
                textFont.Size = originalSize;
                string keyword = selectedText1.Text;
                //ThisAddIn.csのメソッドを呼び出しキーワードをスライドごとに保存
                Globals.ThisAddIn.AddSelectedTextToSlideKeyWords(keyword, currentSlideIndex);
            }
            //リストボックスにキーワードを表示
            Globals.ThisAddIn.Update(currentSlideIndex);
        }

        private void GoToMapbutton_Click(object sender, EventArgs e)
        {
            Globals.ThisAddIn.myCustomTaskPane.Visible = false;
            Globals.ThisAddIn.MapPane.Visible = true;
            Globals.ThisAddIn.BtnPane.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
