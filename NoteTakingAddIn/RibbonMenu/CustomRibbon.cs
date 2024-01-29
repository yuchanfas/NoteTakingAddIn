using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;

using Microsoft.Office.Tools.Ribbon;
using Debug = System.Diagnostics.Debug;
using System.Windows.Forms;

using System.Xml;
using System.Windows;
using System.Windows.Controls;
using WpfLabel = System.Windows.Controls.Label;
using System.Windows.Media;


// TODO:   リボン (XML) アイテムを有効にするには、次の手順に従います。

// 1: 次のコード ブロックを ThisAddin、ThisWorkbook、ThisDocument のいずれかのクラスにコピーします。

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon();
//  }

// 2. ボタンのクリックなど、ユーザーの操作を処理するためのコールバック メソッドを、このクラスの
//    "リボンのコールバック" 領域に作成します。メモ: このリボンがリボン デザイナーからエクスポートされたものである場合は、
//    イベント ハンドラー内のコードをコールバック メソッドに移動し、リボン拡張機能 (RibbonX) のプログラミング モデルで
//    動作するように、コードを変更します。

// 3. リボン XML ファイルのコントロール タグに、コードで適切なコールバック メソッドを識別するための属性を割り当てます。  

// 詳細については、Visual Studio Tools for Office ヘルプにあるリボン XML のドキュメントを参照してください。


namespace NoteTakingAddIn
{
    [ComVisible(true)]
    public class CustomRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public CustomRibbon()
        {
            
        }



        #region IRibbonExtensibility のメンバー
        String XML_FILE = "NoteTakingAddIn.RibbonMenu.CustomRibbon.xml";
        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText(XML_FILE);
        }

        #endregion

        #region リボンのコールバック
        //ここにコールバック メソッドを作成します。コールバック メソッドの追加方法の詳細については、https://msdn.microsoft.com/ja-jp/library/vstudio/Aa942955(v=VS.110).aspx にアクセスしてください。

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public void SelectAttribute(Office.IRibbonControl control)
        {
            // search text with control id
            string xml = GetResourceText(XML_FILE);
            StringReader stream = new StringReader(xml);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            using (XmlReader reader = XmlReader.Create(stream, settings))
                for (; reader.Read(); )
                {
                    reader.ReadToFollowing("button");
                    if(reader.GetAttribute("id") == control.Id)
                    {
                        // 属性をセットする
                        string attributeText = reader.GetAttribute("label");
                        Viewbox attributeView = (Viewbox)LogicalTreeHelper.FindLogicalNode(Globals.ThisAddIn.MapArea, Properties.Settings.Default.TOUCHUP_ATTRIBUTE);
                        if (attributeView == null)
                        {
                            System.Windows.Forms.MessageBox.Show("先にどこに属性を設定するか決めてください");
                            return;
                        }
                        attributeView.Name = Properties.Settings.Default.NO_NAME;
                        WpfLabel label = (WpfLabel)attributeView.Child; // WpfLabel == Label
                        label.Background = Brushes.OrangeRed;
                        label.Content = attributeText;
                    }
                }
        }

        public void ShowKeyboard(Office.IRibbonControl control)
        {
            Globals.ThisAddIn.TouchKeyboardVisible(true);
        }

        

        #endregion

        #region ヘルパー

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
