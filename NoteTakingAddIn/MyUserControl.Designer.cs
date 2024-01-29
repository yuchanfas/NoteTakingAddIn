namespace NoteTakingAddIn
{
    partial class MyUserControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.Markbutton = new System.Windows.Forms.Button();
            this.GoToMapbutton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Markbutton
            // 
            this.Markbutton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Markbutton.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Markbutton.Location = new System.Drawing.Point(62, 274);
            this.Markbutton.Name = "Markbutton";
            this.Markbutton.Size = new System.Drawing.Size(106, 56);
            this.Markbutton.TabIndex = 0;
            this.Markbutton.Text = "マーク付け   ボタン";
            this.Markbutton.UseVisualStyleBackColor = false;
            this.Markbutton.Click += new System.EventHandler(this.Markbutton_Click);
            // 
            // GoToMapbutton
            // 
            this.GoToMapbutton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.GoToMapbutton.Cursor = System.Windows.Forms.Cursors.Default;
            this.GoToMapbutton.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.GoToMapbutton.Location = new System.Drawing.Point(16, 384);
            this.GoToMapbutton.Name = "GoToMapbutton";
            this.GoToMapbutton.Size = new System.Drawing.Size(202, 43);
            this.GoToMapbutton.TabIndex = 1;
            this.GoToMapbutton.Text = "マップ作成画面へ";
            this.GoToMapbutton.UseVisualStyleBackColor = false;
            this.GoToMapbutton.Click += new System.EventHandler(this.GoToMapbutton_Click);
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 22;
            this.listBox1.Location = new System.Drawing.Point(13, 73);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(218, 134);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // MyUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.GoToMapbutton);
            this.Controls.Add(this.Markbutton);
            this.Name = "MyUserControl";
            this.Size = new System.Drawing.Size(243, 483);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Markbutton;
        private System.Windows.Forms.Button GoToMapbutton;
        public System.Windows.Forms.ListBox listBox1;
    }
}
