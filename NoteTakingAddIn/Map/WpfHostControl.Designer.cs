namespace NoteTakingAddIn
{
    partial class WpfHostControl
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
            this.wpfElement = new System.Windows.Forms.Integration.ElementHost();
            this.wpfElement2 = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // wpfElement
            // 
            this.wpfElement.Location = new System.Drawing.Point(2, -2);
            this.wpfElement.Margin = new System.Windows.Forms.Padding(2);
            this.wpfElement.Name = "wpfElement";
            this.wpfElement.Size = new System.Drawing.Size(791, 655);
            this.wpfElement.TabIndex = 0;
            this.wpfElement.Text = "elementHost1";
            this.wpfElement.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.wpfElement_ChildChanged);
            this.wpfElement.Child = null;
            // 
            // wpfElement2
            // 
            this.wpfElement2.Location = new System.Drawing.Point(0, 655);
            this.wpfElement2.Name = "wpfElement2";
            this.wpfElement2.Size = new System.Drawing.Size(793, 145);
            this.wpfElement2.TabIndex = 1;
            this.wpfElement2.Text = "elementHost1";
            this.wpfElement2.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.wpfElement2_ChildChanged);
            this.wpfElement2.Child = null;
            // 
            // WpfHostControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wpfElement2);
            this.Controls.Add(this.wpfElement);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WpfHostControl";
            this.Size = new System.Drawing.Size(810, 800);
            this.Load += new System.EventHandler(this.WpfHostControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost wpfElement;
        private System.Windows.Forms.Integration.ElementHost wpfElement2;
    }
}
