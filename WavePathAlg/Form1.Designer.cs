
namespace WavePathAlg
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._msMain = new System.Windows.Forms.MenuStrip();
            this._toolResetAll = new System.Windows.Forms.ToolStripMenuItem();
            this._toolSearch = new System.Windows.Forms.ToolStripMenuItem();
            this._toolResetPath = new System.Windows.Forms.ToolStripMenuItem();
            this._toolResetWalls = new System.Windows.Forms.ToolStripMenuItem();
            this._msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _msMain
            // 
            this._msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolResetAll,
            this._toolResetPath,
            this._toolSearch,
            this._toolResetWalls});
            this._msMain.Location = new System.Drawing.Point(0, 0);
            this._msMain.Name = "_msMain";
            this._msMain.Size = new System.Drawing.Size(800, 24);
            this._msMain.TabIndex = 0;
            this._msMain.Text = "menuStrip1";
            // 
            // _toolResetAll
            // 
            this._toolResetAll.Name = "_toolResetAll";
            this._toolResetAll.Size = new System.Drawing.Size(64, 20);
            this._toolResetAll.Text = "Reset All";
            // 
            // _toolSearch
            // 
            this._toolSearch.Name = "_toolSearch";
            this._toolSearch.Size = new System.Drawing.Size(54, 20);
            this._toolSearch.Text = "Search";
            // 
            // _toolResetPath
            // 
            this._toolResetPath.Name = "_toolResetPath";
            this._toolResetPath.Size = new System.Drawing.Size(74, 20);
            this._toolResetPath.Text = "Reset Path";
            // 
            // _toolResetWalls
            // 
            this._toolResetWalls.Name = "_toolResetWalls";
            this._toolResetWalls.Size = new System.Drawing.Size(78, 20);
            this._toolResetWalls.Text = "Reset Walls";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._msMain);
            this.MainMenuStrip = this._msMain;
            this.Name = "Form1";
            this.Text = "Form1";
            this._msMain.ResumeLayout(false);
            this._msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _msMain;
        private System.Windows.Forms.ToolStripMenuItem _toolResetAll;
        private System.Windows.Forms.ToolStripMenuItem _toolSearch;
        private System.Windows.Forms.ToolStripMenuItem _toolResetPath;
        private System.Windows.Forms.ToolStripMenuItem _toolResetWalls;
    }
}

