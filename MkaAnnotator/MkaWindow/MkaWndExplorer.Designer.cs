namespace MokkAnnotator.MkaWindow
{
    partial class MkaWndExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkaWndExplorer));
            this.tabImgList = new System.Windows.Forms.ImageList(this.components);
            this.treeExplorer = new System.Windows.Forms.TreeView();
            this.batMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mniBatAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBatAddNewGlass = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBatAddExistingGlass = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBatAddNewGlassImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBatRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBatSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniBatClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBatSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniBatProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.glassMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mniGlassOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGlassRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGlassSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGlassRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGlassSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGlassProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.tsExplorer = new System.Windows.Forms.ToolStrip();
            this.tsList = new System.Windows.Forms.ToolStripButton();
            this.tsThumbnail = new System.Windows.Forms.ToolStripButton();
            this.thumbExplorer = new System.Windows.Forms.ListView();
            this.viewImgList = new System.Windows.Forms.ImageList(this.components);
            this.batMenu.SuspendLayout();
            this.glassMenu.SuspendLayout();
            this.tsExplorer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabImgList
            // 
            this.tabImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tabImgList.ImageStream")));
            this.tabImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.tabImgList.Images.SetKeyName(0, "explorerlist.png");
            this.tabImgList.Images.SetKeyName(1, "exploreroutline.png");
            this.tabImgList.Images.SetKeyName(2, "filebat.png");
            this.tabImgList.Images.SetKeyName(3, "fileglass.png");
            // 
            // treeExplorer
            // 
            this.treeExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeExplorer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeExplorer.ImageIndex = 2;
            this.treeExplorer.ImageList = this.tabImgList;
            this.treeExplorer.Indent = 19;
            this.treeExplorer.ItemHeight = 19;
            this.treeExplorer.LabelEdit = true;
            this.treeExplorer.Location = new System.Drawing.Point(0, 28);
            this.treeExplorer.Name = "treeExplorer";
            this.treeExplorer.SelectedImageIndex = 2;
            this.treeExplorer.Size = new System.Drawing.Size(249, 359);
            this.treeExplorer.TabIndex = 1;
            this.treeExplorer.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeExplorer_BeforeLabelEdit);
            this.treeExplorer.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeExplorer_AfterLabelEdit);
            this.treeExplorer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeExplorer_AfterSelect);
            this.treeExplorer.DoubleClick += new System.EventHandler(this.OpenGlassFile);
            this.treeExplorer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeExplorer_KeyDown);
            this.treeExplorer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.treeExplorer_KeyPress);
            this.treeExplorer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeExplorer_MouseUp);
            // 
            // batMenu
            // 
            this.batMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniBatAdd,
            this.mniBatRename,
            this.mniBatSep1,
            this.mniBatClose,
            this.mniBatSep2,
            this.mniBatProperty});
            this.batMenu.Name = "batMenu";
            this.batMenu.Size = new System.Drawing.Size(176, 104);
            // 
            // mniBatAdd
            // 
            this.mniBatAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniBatAddNewGlass,
            this.mniBatAddExistingGlass,
            this.mniBatAddNewGlassImage});
            this.mniBatAdd.Name = "mniBatAdd";
            this.mniBatAdd.Size = new System.Drawing.Size(175, 22);
            this.mniBatAdd.Text = "追加(&A)";
            // 
            // mniBatAddNewGlass
            // 
            this.mniBatAddNewGlass.Name = "mniBatAddNewGlass";
            this.mniBatAddNewGlass.Size = new System.Drawing.Size(195, 22);
            this.mniBatAddNewGlass.Text = "新しいガラス板(&N)...";
            this.mniBatAddNewGlass.Click += new System.EventHandler(this.mniBatAddNewGlass_Click);
            // 
            // mniBatAddExistingGlass
            // 
            this.mniBatAddExistingGlass.Name = "mniBatAddExistingGlass";
            this.mniBatAddExistingGlass.Size = new System.Drawing.Size(195, 22);
            this.mniBatAddExistingGlass.Text = "既存のガラス板(&G)...";
            this.mniBatAddExistingGlass.Click += new System.EventHandler(this.mniBatAddExistingGlass_Click);
            // 
            // mniBatAddNewGlassImage
            // 
            this.mniBatAddNewGlassImage.Name = "mniBatAddNewGlassImage";
            this.mniBatAddNewGlassImage.Size = new System.Drawing.Size(195, 22);
            this.mniBatAddNewGlassImage.Text = "新しいガラス板画像(&I)...";
            this.mniBatAddNewGlassImage.Click += new System.EventHandler(this.mniBatAddNewGlassImage_Click);
            // 
            // mniBatRename
            // 
            this.mniBatRename.Name = "mniBatRename";
            this.mniBatRename.Size = new System.Drawing.Size(175, 22);
            this.mniBatRename.Text = "バット番号の変更(&R)";
            this.mniBatRename.Click += new System.EventHandler(this.mniRename_Click);
            // 
            // mniBatSep1
            // 
            this.mniBatSep1.Name = "mniBatSep1";
            this.mniBatSep1.Size = new System.Drawing.Size(172, 6);
            // 
            // mniBatClose
            // 
            this.mniBatClose.Name = "mniBatClose";
            this.mniBatClose.Size = new System.Drawing.Size(175, 22);
            this.mniBatClose.Text = "バットを閉じる(&C)";
            this.mniBatClose.Click += new System.EventHandler(this.mniBatClose_Click);
            // 
            // mniBatSep2
            // 
            this.mniBatSep2.Name = "mniBatSep2";
            this.mniBatSep2.Size = new System.Drawing.Size(172, 6);
            // 
            // mniBatProperty
            // 
            this.mniBatProperty.Name = "mniBatProperty";
            this.mniBatProperty.Size = new System.Drawing.Size(175, 22);
            this.mniBatProperty.Text = "プロパティ(&P)";
            this.mniBatProperty.Click += new System.EventHandler(this.mniBatProperty_Click);
            // 
            // glassMenu
            // 
            this.glassMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniGlassOpen,
            this.mniGlassRename,
            this.mniGlassSep1,
            this.mniGlassRemove,
            this.mniGlassSep2,
            this.mniGlassProperty});
            this.glassMenu.Name = "glassMenu";
            this.glassMenu.Size = new System.Drawing.Size(157, 104);
            // 
            // mniGlassOpen
            // 
            this.mniGlassOpen.Name = "mniGlassOpen";
            this.mniGlassOpen.Size = new System.Drawing.Size(156, 22);
            this.mniGlassOpen.Text = "開く(&O)";
            this.mniGlassOpen.Click += new System.EventHandler(this.mniGlassOpen_Click);
            // 
            // mniGlassRename
            // 
            this.mniGlassRename.Name = "mniGlassRename";
            this.mniGlassRename.Size = new System.Drawing.Size(156, 22);
            this.mniGlassRename.Text = "名前の変更(&R)";
            this.mniGlassRename.Click += new System.EventHandler(this.mniRename_Click);
            // 
            // mniGlassSep1
            // 
            this.mniGlassSep1.Name = "mniGlassSep1";
            this.mniGlassSep1.Size = new System.Drawing.Size(153, 6);
            // 
            // mniGlassRemove
            // 
            this.mniGlassRemove.Name = "mniGlassRemove";
            this.mniGlassRemove.Size = new System.Drawing.Size(156, 22);
            this.mniGlassRemove.Text = "バットから除外(&J)";
            this.mniGlassRemove.Click += new System.EventHandler(this.mniGlassRemove_Click);
            // 
            // mniGlassSep2
            // 
            this.mniGlassSep2.Name = "mniGlassSep2";
            this.mniGlassSep2.Size = new System.Drawing.Size(153, 6);
            // 
            // mniGlassProperty
            // 
            this.mniGlassProperty.Name = "mniGlassProperty";
            this.mniGlassProperty.Size = new System.Drawing.Size(156, 22);
            this.mniGlassProperty.Text = "プロパティ(&P)";
            this.mniGlassProperty.Click += new System.EventHandler(this.mniGlassProperty_Click);
            // 
            // tsExplorer
            // 
            this.tsExplorer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsList,
            this.tsThumbnail});
            this.tsExplorer.Location = new System.Drawing.Point(0, 0);
            this.tsExplorer.Name = "tsExplorer";
            this.tsExplorer.Size = new System.Drawing.Size(249, 25);
            this.tsExplorer.TabIndex = 2;
            this.tsExplorer.Text = "toolStrip1";
            // 
            // tsList
            // 
            this.tsList.Checked = true;
            this.tsList.CheckOnClick = true;
            this.tsList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsList.Image = global::MokkAnnotator.Properties.Resources.explorerlist;
            this.tsList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsList.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tsList.Name = "tsList";
            this.tsList.Size = new System.Drawing.Size(23, 22);
            this.tsList.Text = "リスト表示";
            this.tsList.Click += new System.EventHandler(this.tsViewModeChange_Click);
            // 
            // tsThumbnail
            // 
            this.tsThumbnail.CheckOnClick = true;
            this.tsThumbnail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsThumbnail.Image = global::MokkAnnotator.Properties.Resources.exploreroutline;
            this.tsThumbnail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsThumbnail.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.tsThumbnail.Name = "tsThumbnail";
            this.tsThumbnail.Size = new System.Drawing.Size(23, 22);
            this.tsThumbnail.Text = "サムネイル表示";
            this.tsThumbnail.Click += new System.EventHandler(this.tsViewModeChange_Click);
            // 
            // thumbExplorer
            // 
            this.thumbExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.thumbExplorer.LargeImageList = this.viewImgList;
            this.thumbExplorer.Location = new System.Drawing.Point(0, 28);
            this.thumbExplorer.MultiSelect = false;
            this.thumbExplorer.Name = "thumbExplorer";
            this.thumbExplorer.Size = new System.Drawing.Size(249, 359);
            this.thumbExplorer.TabIndex = 3;
            this.thumbExplorer.UseCompatibleStateImageBehavior = false;
            this.thumbExplorer.DoubleClick += new System.EventHandler(this.thumbExplorer_DoubleClick);
            this.thumbExplorer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.thumbExplorer_KeyPress);
            // 
            // viewImgList
            // 
            this.viewImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("viewImgList.ImageStream")));
            this.viewImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.viewImgList.Images.SetKeyName(0, "arrow_right.png");
            // 
            // MkaWndExplorer
            // 
            this.AutoHidePortion = 0.15D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 389);
            this.Controls.Add(this.thumbExplorer);
            this.Controls.Add(this.tsExplorer);
            this.Controls.Add(this.treeExplorer);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MkaWndExplorer";
            this.Text = "エクスプローラー";
            this.ToolTipText = "エクスプローラー";
            this.batMenu.ResumeLayout(false);
            this.glassMenu.ResumeLayout(false);
            this.tsExplorer.ResumeLayout(false);
            this.tsExplorer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList tabImgList;
        private System.Windows.Forms.TreeView treeExplorer;
        private System.Windows.Forms.ContextMenuStrip batMenu;
        private System.Windows.Forms.ContextMenuStrip glassMenu;
        private System.Windows.Forms.ToolStripMenuItem mniBatAdd;
        private System.Windows.Forms.ToolStripMenuItem mniBatAddNewGlass;
        private System.Windows.Forms.ToolStripMenuItem mniBatAddExistingGlass;
        private System.Windows.Forms.ToolStripMenuItem mniBatAddNewGlassImage;
        private System.Windows.Forms.ToolStripMenuItem mniBatRename;
        private System.Windows.Forms.ToolStripSeparator mniBatSep1;
        private System.Windows.Forms.ToolStripMenuItem mniBatClose;
        private System.Windows.Forms.ToolStripSeparator mniBatSep2;
        private System.Windows.Forms.ToolStripMenuItem mniBatProperty;
        private System.Windows.Forms.ToolStripMenuItem mniGlassOpen;
        private System.Windows.Forms.ToolStripSeparator mniGlassSep1;
        private System.Windows.Forms.ToolStripMenuItem mniGlassRemove;
        private System.Windows.Forms.ToolStripSeparator mniGlassSep2;
        private System.Windows.Forms.ToolStripMenuItem mniGlassRename;
        private System.Windows.Forms.ToolStripMenuItem mniGlassProperty;
        private System.Windows.Forms.ToolStrip tsExplorer;
        private System.Windows.Forms.ToolStripButton tsList;
        private System.Windows.Forms.ToolStripButton tsThumbnail;
        private System.Windows.Forms.ListView thumbExplorer;
        private System.Windows.Forms.ImageList viewImgList;
    }
}