using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace NoteTakingAddIn
{
    /// <summary>
    /// ButtonsAra.xaml の相互作用ロジック
    /// </summary>
    public partial class ButtonsAra : System.Windows.Controls.UserControl
    {
        public ButtonsAra()
        {
            InitializeComponent();
        }

        private String selectingText;
        public void setSelectingText(string text)
        {
            this.selectingText = text;
        }

        // change selecting text to Node
        private void annotationBtn_Click(object sender, EventArgs e)
        {
            //if (this.selectingText == null)
            //{
            //    MessageBox.Show("No Text is Selected");
            //    return;
            //}
            //現在のスライドのインデックスを取得
            int currentSlideIndex = Globals.ThisAddIn.Application.ActiveWindow.Selection.SlideRange.SlideIndex;
            List<string> keywordsForCurrentSlide = Globals.ThisAddIn.GetKeywordsForSlide(currentSlideIndex);
            foreach (string keyword in keywordsForCurrentSlide)
            {
                KnowledgeMapArea lsa = Globals.ThisAddIn.MapArea;
                lsa.CreateNode(keyword);
            }
            //KnowledgeMapArea lsa = Globals.ThisAddIn.MapArea;
            //lsa.CreateNode(this.selectingText);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GoToMarkbutton_Click(object sender, EventArgs e)
        {
            Globals.ThisAddIn.myCustomTaskPane.Visible = true;
            Globals.ThisAddIn.MapPane.Visible = false;
            Globals.ThisAddIn.BtnPane.Visible = false;
        }

        private void SaveMapbutton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "ファイルを保存する";
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.Filter = "JSONファイル (*.json)|*.json";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var mapData = Globals.ThisAddIn.MapArea.CollectMapData();
                var jsonData = JsonConvert.SerializeObject(mapData, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(saveFileDialog.FileName, jsonData);
            }

        }
        public void UpdateKeywords(List<string> keywords)
        {
            listBox1.Items.Clear();
            foreach(string keyword in keywords) { 
                listBox1.Items.Add(keyword);
            }
        }
    }
}
