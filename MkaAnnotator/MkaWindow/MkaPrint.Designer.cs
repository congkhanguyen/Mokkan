namespace MokkAnnotator.MkaWindow
{
    partial class MkaPrint
    {
        /// <summary>
        /// 必要なデザイナ変数です。
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkaPrint));
            this.document = new System.Drawing.Printing.PrintDocument();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.previewDialog = new System.Windows.Forms.PrintPreviewDialog();
            // 
            // document
            // 
            this.document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.document_PrintPage);
            // 
            // printDialog
            // 
            this.printDialog.Document = this.document;
            this.printDialog.UseEXDialog = true;
            // 
            // pageSetupDialog
            // 
            this.pageSetupDialog.Document = this.document;
            // 
            // previewDialog
            // 
            this.previewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.previewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.previewDialog.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.previewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.previewDialog.Document = this.document;
            this.previewDialog.Enabled = true;
            this.previewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("previewDialog.Icon")));
            this.previewDialog.Name = "previewDialog";
            this.previewDialog.ShowIcon = false;
            this.previewDialog.Visible = false;

        }

        #endregion

        private System.Drawing.Printing.PrintDocument document;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.PrintPreviewDialog previewDialog;
    }
}
