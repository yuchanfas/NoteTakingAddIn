using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NoteTakingAddIn.Map
{
   public class MapData
    {
        public List<NodeData> Nodes { get; set; } = new List<NodeData>();
        public List<LinkData> Links { get; set; } = new List<LinkData>();
        public List<AttributeData> Attributes { get; set; } = new List<AttributeData>();

    }
    public class NodeData
    {
        public string Text { get; set; }//ノードの表示されるテキスト
        public int SlideIndex { get; set; }//ノードが関連付けられているスライド番号
        //ノードの座標を取得
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class LinkData
    {
        public int StartNodeIndex { get; set; }//リンクの始点のインデックス
        public int EndNodeIndex { get; set; }//リンクの終点インデックス
        //リンクの座標を取得
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }
    public class AttributeData
    {
        public int LinkIndex { get; set; }//属性が関連付けられているリンク
        public string Content { get; set; }//属性の内容                     
    }
}

