namespace TRANSIT
{
    partial class frm_principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_principal));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.txtTDD = new DevExpress.XtraEditors.TextEdit();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_lancer = new DevExpress.XtraEditors.SimpleButton();
            this.btn_lister = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.dgSource = new System.Windows.Forms.DataGridView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblrecup = new DevExpress.XtraEditors.LabelControl();
            this.cmbBase1 = new System.Windows.Forms.ComboBox();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBase = new System.Windows.Forms.ComboBox();
            this.drpdestinataire = new System.Windows.Forms.ComboBox();
            this.drpsource = new System.Windows.Forms.ComboBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTDD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnPrint);
            this.layoutControl1.Controls.Add(this.dataLayoutControl1);
            this.layoutControl1.Controls.Add(this.dgSource);
            this.layoutControl1.Controls.Add(this.groupControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1484, 747);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnPrint
            // 
            this.btnPrint.Enabled = false;
            this.btnPrint.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.btnPrint.Location = new System.Drawing.Point(12, 230);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btnPrint.Size = new System.Drawing.Size(1460, 91);
            this.btnPrint.StyleController = this.layoutControl1;
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.txtTDD);
            this.dataLayoutControl1.Controls.Add(this.btn_save);
            this.dataLayoutControl1.Controls.Add(this.btn_lancer);
            this.dataLayoutControl1.Controls.Add(this.btn_lister);
            this.dataLayoutControl1.Location = new System.Drawing.Point(12, 137);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.layoutControlGroup1;
            this.dataLayoutControl1.Size = new System.Drawing.Size(1460, 89);
            this.dataLayoutControl1.TabIndex = 6;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // txtTDD
            // 
            this.txtTDD.Location = new System.Drawing.Point(48, 14);
            this.txtTDD.Name = "txtTDD";
            this.txtTDD.Size = new System.Drawing.Size(1333, 22);
            this.txtTDD.StyleController = this.dataLayoutControl1;
            this.txtTDD.TabIndex = 21;
            this.txtTDD.EditValueChanged += new System.EventHandler(this.txtTDD_EditValueChanged);
            // 
            // btn_save
            // 
            this.btn_save.Appearance.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_save.Appearance.Options.UseFont = true;
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(1202, 46);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(246, 27);
            this.btn_save.StyleController = this.dataLayoutControl1;
            this.btn_save.TabIndex = 10;
            this.btn_save.Text = "Exporter en TXT";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_lancer
            // 
            this.btn_lancer.Appearance.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_lancer.Appearance.Options.UseFont = true;
            this.btn_lancer.Enabled = false;
            this.btn_lancer.Location = new System.Drawing.Point(12, 46);
            this.btn_lancer.Name = "btn_lancer";
            this.btn_lancer.Size = new System.Drawing.Size(1186, 27);
            this.btn_lancer.StyleController = this.dataLayoutControl1;
            this.btn_lancer.TabIndex = 9;
            this.btn_lancer.Text = "Lancer le traitement ...";
            this.btn_lancer.Click += new System.EventHandler(this.btn_lancer_Click);
            // 
            // btn_lister
            // 
            this.btn_lister.Appearance.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_lister.Appearance.Options.UseFont = true;
            this.btn_lister.Enabled = false;
            this.btn_lister.Location = new System.Drawing.Point(1385, 12);
            this.btn_lister.Name = "btn_lister";
            this.btn_lister.Size = new System.Drawing.Size(63, 27);
            this.btn_lister.StyleController = this.dataLayoutControl1;
            this.btn_lister.TabIndex = 11;
            this.btn_lister.Text = "Afficher";
            this.btn_lister.Click += new System.EventHandler(this.btn_lister_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1460, 89);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem5.Control = this.btn_lancer;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 31);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(1190, 38);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem6.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem6.Control = this.btn_save;
            this.layoutControlItem6.Location = new System.Drawing.Point(1190, 31);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(250, 38);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem7.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem7.Control = this.txtTDD;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(1373, 31);
            this.layoutControlItem7.Text = "TDD";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(24, 16);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_lister;
            this.layoutControlItem8.Location = new System.Drawing.Point(1373, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(67, 31);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // dgSource
            // 
            this.dgSource.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgSource.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSource.Location = new System.Drawing.Point(28, 325);
            this.dgSource.Name = "dgSource";
            this.dgSource.ReadOnly = true;
            this.dgSource.RowHeadersWidth = 51;
            this.dgSource.RowTemplate.Height = 24;
            this.dgSource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSource.Size = new System.Drawing.Size(1444, 410);
            this.dgSource.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblrecup);
            this.groupControl1.Controls.Add(this.cmbBase1);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.cmbBase);
            this.groupControl1.Controls.Add(this.drpdestinataire);
            this.groupControl1.Controls.Add(this.drpsource);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1460, 121);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "Options";
            // 
            // lblrecup
            // 
            this.lblrecup.Location = new System.Drawing.Point(881, 138);
            this.lblrecup.Name = "lblrecup";
            this.lblrecup.Size = new System.Drawing.Size(0, 16);
            this.lblrecup.TabIndex = 18;
            this.lblrecup.Visible = false;
            // 
            // cmbBase1
            // 
            this.cmbBase1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBase1.Enabled = false;
            this.cmbBase1.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBase1.FormattingEnabled = true;
            this.cmbBase1.Location = new System.Drawing.Point(472, 85);
            this.cmbBase1.Name = "cmbBase1";
            this.cmbBase1.Size = new System.Drawing.Size(378, 24);
            this.cmbBase1.TabIndex = 13;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(337, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(117, 16);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "Nom de la base dest";
            // 
            // cmbBase
            // 
            this.cmbBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBase.Enabled = false;
            this.cmbBase.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBase.FormattingEnabled = true;
            this.cmbBase.Location = new System.Drawing.Point(472, 54);
            this.cmbBase.Name = "cmbBase";
            this.cmbBase.Size = new System.Drawing.Size(378, 24);
            this.cmbBase.TabIndex = 9;
            this.cmbBase.DropDownClosed += new System.EventHandler(this.cmbBase_DropDownClosed);
            // 
            // drpdestinataire
            // 
            this.drpdestinataire.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpdestinataire.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpdestinataire.FormattingEnabled = true;
            this.drpdestinataire.Location = new System.Drawing.Point(153, 88);
            this.drpdestinataire.Name = "drpdestinataire";
            this.drpdestinataire.Size = new System.Drawing.Size(176, 24);
            this.drpdestinataire.TabIndex = 8;
            this.drpdestinataire.DropDownClosed += new System.EventHandler(this.drpdestinataire_DropDownClosed);
            // 
            // drpsource
            // 
            this.drpsource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpsource.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpsource.FormattingEnabled = true;
            this.drpsource.Location = new System.Drawing.Point(153, 53);
            this.drpsource.Name = "drpsource";
            this.drpsource.Size = new System.Drawing.Size(176, 24);
            this.drpsource.TabIndex = 7;
            this.drpsource.DropDownClosed += new System.EventHandler(this.drpsource_DropDownClosed);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(335, 61);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(131, 16);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "Nom de la base source";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(16, 96);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(117, 16);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Serveur Destinataire";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 57);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(89, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Serveur Source";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem3});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1484, 747);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.groupControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1464, 125);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.dgSource;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 313);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1464, 414);
            this.layoutControlItem2.Text = " ";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(4, 16);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.dataLayoutControl1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 125);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(1464, 93);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnPrint;
            this.layoutControlItem3.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.layoutControlItem3.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlItem3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem3.ImageOptions.Image")));
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 218);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(1464, 95);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // frm_principal
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 747);
            this.Controls.Add(this.layoutControl1);
            this.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_principal.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frm_principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TRANSIT";
            this.Load += new System.EventHandler(this.frm_principal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTDD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        public System.Windows.Forms.DataGridView dgSource;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.SimpleButton btn_lancer;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.ComboBox drpsource;
        private System.Windows.Forms.ComboBox drpdestinataire;
        private System.Windows.Forms.ComboBox cmbBase;
        private DevExpress.XtraEditors.SimpleButton btn_lister;
        private System.Windows.Forms.ComboBox cmbBase1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        public DevExpress.XtraEditors.LabelControl lblrecup;
        private DevExpress.XtraEditors.TextEdit txtTDD;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}

