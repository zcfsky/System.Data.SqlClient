namespace PDATest
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOpenCon = new System.Windows.Forms.Button();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.btnTestSql = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOpenCon);
            this.panel1.Controls.Add(this.txtSql);
            this.panel1.Controls.Add(this.btnTestSql);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 191);
            // 
            // btnOpenCon
            // 
            this.btnOpenCon.Location = new System.Drawing.Point(18, 155);
            this.btnOpenCon.Name = "btnOpenCon";
            this.btnOpenCon.Size = new System.Drawing.Size(130, 33);
            this.btnOpenCon.TabIndex = 2;
            this.btnOpenCon.Text = "打开数据库";
            this.btnOpenCon.Click += new System.EventHandler(this.btnOpenCon_Click);
            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(3, 3);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.Size = new System.Drawing.Size(311, 146);
            this.txtSql.TabIndex = 1;
            this.txtSql.Text = "Data Source=192.168.3.123;Initial Catalog = Test;User ID = sa;Password = 123456;";
            // 
            // btnTestSql
            // 
            this.btnTestSql.Location = new System.Drawing.Point(170, 155);
            this.btnTestSql.Name = "btnTestSql";
            this.btnTestSql.Size = new System.Drawing.Size(118, 33);
            this.btnTestSql.TabIndex = 0;
            this.btnTestSql.Text = "数据库测试";
            this.btnTestSql.Click += new System.EventHandler(this.btnTestSql_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.panel1);
            this.Name = "FrmMain";
            this.Text = "测试";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOpenCon;
        private System.Windows.Forms.TextBox txtSql;
        private System.Windows.Forms.Button btnTestSql;
    }
}

