namespace enigma_pro
{
    partial class MainWindow
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
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.dbMenuItem = new System.Windows.Forms.MenuItem();
            this.newDBMenuItem = new System.Windows.Forms.MenuItem();
            this.openDBMenuItem = new System.Windows.Forms.MenuItem();
            this.closeDBMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.saveDBMenuItem = new System.Windows.Forms.MenuItem();
            this.saveAsDBMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.changeMasterKeyMenuItem = new System.Windows.Forms.MenuItem();
            this.exportToCSVFileMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.quitMenuItem = new System.Windows.Forms.MenuItem();
            this.entriesMenuItem = new System.Windows.Forms.MenuItem();
            this.addEntryMenuItem = new System.Windows.Forms.MenuItem();
            this.editViewEntryMenuItem = new System.Windows.Forms.MenuItem();
            this.duplicateEntryMenuItem = new System.Windows.Forms.MenuItem();
            this.delEntryMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.cpUsernameMenuItem = new System.Windows.Forms.MenuItem();
            this.cpPasswordMenuItem = new System.Windows.Forms.MenuItem();
            this.openURLMenuItem = new System.Windows.Forms.MenuItem();
            this.helpMenuItem = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.dbMenuItem,
            this.entriesMenuItem,
            this.helpMenuItem});
            // 
            // dbMenuItem
            // 
            this.dbMenuItem.Index = 0;
            this.dbMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newDBMenuItem,
            this.openDBMenuItem,
            this.closeDBMenuItem,
            this.menuItem4,
            this.saveDBMenuItem,
            this.saveAsDBMenuItem,
            this.menuItem8,
            this.changeMasterKeyMenuItem,
            this.exportToCSVFileMenuItem,
            this.menuItem11,
            this.quitMenuItem});
            this.dbMenuItem.Text = "Database";
            // 
            // newDBMenuItem
            // 
            this.newDBMenuItem.Index = 0;
            this.newDBMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.newDBMenuItem.Text = "New Database";
            this.newDBMenuItem.Click += new System.EventHandler(this.newDBMenuItem_Click);
            // 
            // openDBMenuItem
            // 
            this.openDBMenuItem.Index = 1;
            this.openDBMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.openDBMenuItem.Text = "Open Database";
            // 
            // closeDBMenuItem
            // 
            this.closeDBMenuItem.Index = 2;
            this.closeDBMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
            this.closeDBMenuItem.Text = "Close";
            this.closeDBMenuItem.Click += new System.EventHandler(this.closeDBMenuItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "-";
            // 
            // saveDBMenuItem
            // 
            this.saveDBMenuItem.Index = 4;
            this.saveDBMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.saveDBMenuItem.Text = "Save Database";
            // 
            // saveAsDBMenuItem
            // 
            this.saveAsDBMenuItem.Index = 5;
            this.saveAsDBMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
            this.saveAsDBMenuItem.Text = "Save Database as...";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 6;
            this.menuItem8.Text = "-";
            // 
            // changeMasterKeyMenuItem
            // 
            this.changeMasterKeyMenuItem.Index = 7;
            this.changeMasterKeyMenuItem.Text = "Change Master-Key";
            // 
            // exportToCSVFileMenuItem
            // 
            this.exportToCSVFileMenuItem.Index = 8;
            this.exportToCSVFileMenuItem.Text = "Export to CSV";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 9;
            this.menuItem11.Text = "-";
            // 
            // quitMenuItem
            // 
            this.quitMenuItem.Index = 10;
            this.quitMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
            this.quitMenuItem.Text = "Quit";
            this.quitMenuItem.Click += new System.EventHandler(this.quitMenuItem_Click);
            // 
            // entriesMenuItem
            // 
            this.entriesMenuItem.Index = 1;
            this.entriesMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.addEntryMenuItem,
            this.editViewEntryMenuItem,
            this.duplicateEntryMenuItem,
            this.delEntryMenuItem,
            this.menuItem17,
            this.cpUsernameMenuItem,
            this.cpPasswordMenuItem,
            this.openURLMenuItem});
            this.entriesMenuItem.Text = "Entries";
            this.entriesMenuItem.Select += new System.EventHandler(this.entriesMenuItem_Select);
            // 
            // addEntryMenuItem
            // 
            this.addEntryMenuItem.Index = 0;
            this.addEntryMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.addEntryMenuItem.Text = "Add Entry";
            this.addEntryMenuItem.Click += new System.EventHandler(this.addEntryMenuItem_Click);
            // 
            // editViewEntryMenuItem
            // 
            this.editViewEntryMenuItem.Index = 1;
            this.editViewEntryMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.editViewEntryMenuItem.Text = "Edit/View Entry";
            this.editViewEntryMenuItem.Click += new System.EventHandler(this.editViewEntryMenuItem_Click);
            // 
            // duplicateEntryMenuItem
            // 
            this.duplicateEntryMenuItem.Index = 2;
            this.duplicateEntryMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlK;
            this.duplicateEntryMenuItem.Text = "Duplicate Entry";
            this.duplicateEntryMenuItem.Click += new System.EventHandler(this.duplicateEntryMenuItem_Click);
            // 
            // delEntryMenuItem
            // 
            this.delEntryMenuItem.Index = 3;
            this.delEntryMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.delEntryMenuItem.Text = "Delete Entry";
            this.delEntryMenuItem.Click += new System.EventHandler(this.delEntryMenuItem_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 4;
            this.menuItem17.Text = "-";
            // 
            // cpUsernameMenuItem
            // 
            this.cpUsernameMenuItem.Index = 5;
            this.cpUsernameMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlB;
            this.cpUsernameMenuItem.Text = "Copy Username";
            this.cpUsernameMenuItem.Click += new System.EventHandler(this.cpUsernameMenuItem_Click);
            // 
            // cpPasswordMenuItem
            // 
            this.cpPasswordMenuItem.Index = 6;
            this.cpPasswordMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.cpPasswordMenuItem.Text = "Copy Password";
            this.cpPasswordMenuItem.Click += new System.EventHandler(this.cpPasswordMenuItem_Click);
            // 
            // openURLMenuItem
            // 
            this.openURLMenuItem.Index = 7;
            this.openURLMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.openURLMenuItem.Text = "Open URL";
            this.openURLMenuItem.Click += new System.EventHandler(this.openURLMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Index = 2;
            this.helpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.aboutMenuItem});
            this.helpMenuItem.Text = "Help";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Index = 0;
            this.aboutMenuItem.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.aboutMenuItem.Text = "About";
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 491);
            this.Menu = this.mainMenu;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainWindow";
            this.SizeChanged += new System.EventHandler(this.MainWindow_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem dbMenuItem;
        private System.Windows.Forms.MenuItem newDBMenuItem;
        private System.Windows.Forms.MenuItem openDBMenuItem;
        private System.Windows.Forms.MenuItem closeDBMenuItem;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem saveDBMenuItem;
        private System.Windows.Forms.MenuItem saveAsDBMenuItem;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem changeMasterKeyMenuItem;
        private System.Windows.Forms.MenuItem exportToCSVFileMenuItem;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem quitMenuItem;
        private System.Windows.Forms.MenuItem entriesMenuItem;
        private System.Windows.Forms.MenuItem addEntryMenuItem;
        private System.Windows.Forms.MenuItem editViewEntryMenuItem;
        private System.Windows.Forms.MenuItem delEntryMenuItem;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem cpUsernameMenuItem;
        private System.Windows.Forms.MenuItem cpPasswordMenuItem;
        private System.Windows.Forms.MenuItem openURLMenuItem;
        private System.Windows.Forms.MenuItem helpMenuItem;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem duplicateEntryMenuItem;
    }
}

