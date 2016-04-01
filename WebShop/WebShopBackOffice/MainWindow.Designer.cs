namespace WebShopBackOffice
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
            this.ordersList = new System.Windows.Forms.ListBox();
            this.shipBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ordersList
            // 
            this.ordersList.FormattingEnabled = true;
            this.ordersList.Location = new System.Drawing.Point(12, 12);
            this.ordersList.Name = "ordersList";
            this.ordersList.Size = new System.Drawing.Size(610, 407);
            this.ordersList.TabIndex = 0;
            // 
            // shipBtn
            // 
            this.shipBtn.Location = new System.Drawing.Point(12, 425);
            this.shipBtn.Name = "shipBtn";
            this.shipBtn.Size = new System.Drawing.Size(610, 32);
            this.shipBtn.TabIndex = 1;
            this.shipBtn.Text = "Ship Order";
            this.shipBtn.UseVisualStyleBackColor = true;
            this.shipBtn.Click += new System.EventHandler(this.shipBtn_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 467);
            this.Controls.Add(this.shipBtn);
            this.Controls.Add(this.ordersList);
            this.Name = "MainWindow";
            this.Text = "BackOffice";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ordersList;
        private System.Windows.Forms.Button shipBtn;
    }
}

