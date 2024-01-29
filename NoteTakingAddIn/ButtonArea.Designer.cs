namespace NoteTakingAddIn
{
    partial class ButtonArea
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.annotationBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.GoToMarkbutton = new System.Windows.Forms.Button();
            this.SaveMapbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // annotationBtn
            // 
            this.annotationBtn.BackColor = System.Drawing.SystemColors.Menu;
            this.annotationBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.annotationBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.annotationBtn.Font = new System.Drawing.Font("メイリオ", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.annotationBtn.Location = new System.Drawing.Point(54, 232);
            this.annotationBtn.Margin = new System.Windows.Forms.Padding(0);
            this.annotationBtn.Name = "annotationBtn";
            this.annotationBtn.Size = new System.Drawing.Size(118, 63);
            this.annotationBtn.TabIndex = 0;
            this.annotationBtn.Text = "ANNOTATION";
            this.annotationBtn.UseVisualStyleBackColor = false;
            this.annotationBtn.Click += new System.EventHandler(this.annotationBtn_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 22;
            this.listBox1.Location = new System.Drawing.Point(12, 59);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(218, 114);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // GoToMarkbutton
            // 
            this.GoToMarkbutton.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.GoToMarkbutton.Location = new System.Drawing.Point(25, 417);
            this.GoToMarkbutton.Name = "GoToMarkbutton";
            this.GoToMarkbutton.Size = new System.Drawing.Size(200, 41);
            this.GoToMarkbutton.TabIndex = 2;
            this.GoToMarkbutton.Text = "マーク付け画面へ";
            this.GoToMarkbutton.UseVisualStyleBackColor = true;
            this.GoToMarkbutton.Click += new System.EventHandler(this.GoToMarkbutton_Click);
            // 
            // SaveMapbutton
            // 
            this.SaveMapbutton.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SaveMapbutton.Location = new System.Drawing.Point(54, 344);
            this.SaveMapbutton.Name = "SaveMapbutton";
            this.SaveMapbutton.Size = new System.Drawing.Size(118, 56);
            this.SaveMapbutton.TabIndex = 3;
            this.SaveMapbutton.Text = "SaveMap";
            this.SaveMapbutton.UseVisualStyleBackColor = true;
            this.SaveMapbutton.Click += new System.EventHandler(this.SaveMapbutton_Click);
            // 
            // ButtonArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SaveMapbutton);
            this.Controls.Add(this.GoToMarkbutton);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.annotationBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ButtonArea";
            this.Size = new System.Drawing.Size(243, 483);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button annotationBtn;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button GoToMarkbutton;
        private System.Windows.Forms.Button SaveMapbutton;
    }
}
