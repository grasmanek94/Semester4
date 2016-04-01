namespace WebShopClient
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
            this.shopNameLabel = new System.Windows.Forms.Label();
            this.productsView = new System.Windows.Forms.DataGridView();
            this.productsLabel = new System.Windows.Forms.Label();
            this.productNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.inputDescriptionText = new System.Windows.Forms.RichTextBox();
            this.buyBtn = new System.Windows.Forms.Button();
            this.inputProductNameLabel = new System.Windows.Forms.Label();
            this.inputPriceLabel = new System.Windows.Forms.Label();
            this.inputInStockLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.productsView)).BeginInit();
            this.SuspendLayout();
            // 
            // shopNameLabel
            // 
            this.shopNameLabel.AutoSize = true;
            this.shopNameLabel.Location = new System.Drawing.Point(9, 9);
            this.shopNameLabel.Name = "shopNameLabel";
            this.shopNameLabel.Size = new System.Drawing.Size(54, 13);
            this.shopNameLabel.TabIndex = 0;
            this.shopNameLabel.Text = "Loading...";
            // 
            // productsView
            // 
            this.productsView.AllowUserToAddRows = false;
            this.productsView.AllowUserToDeleteRows = false;
            this.productsView.AllowUserToResizeColumns = false;
            this.productsView.AllowUserToResizeRows = false;
            this.productsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productsView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.productsView.Location = new System.Drawing.Point(12, 48);
            this.productsView.MultiSelect = false;
            this.productsView.Name = "productsView";
            this.productsView.ReadOnly = true;
            this.productsView.ShowCellErrors = false;
            this.productsView.ShowCellToolTips = false;
            this.productsView.ShowEditingIcon = false;
            this.productsView.ShowRowErrors = false;
            this.productsView.Size = new System.Drawing.Size(444, 216);
            this.productsView.TabIndex = 1;
            // 
            // productsLabel
            // 
            this.productsLabel.AutoSize = true;
            this.productsLabel.Location = new System.Drawing.Point(9, 32);
            this.productsLabel.Name = "productsLabel";
            this.productsLabel.Size = new System.Drawing.Size(98, 13);
            this.productsLabel.TabIndex = 2;
            this.productsLabel.Text = "Available Products:";
            // 
            // productNameLabel
            // 
            this.productNameLabel.AutoSize = true;
            this.productNameLabel.Location = new System.Drawing.Point(475, 48);
            this.productNameLabel.Name = "productNameLabel";
            this.productNameLabel.Size = new System.Drawing.Size(89, 13);
            this.productNameLabel.TabIndex = 3;
            this.productNameLabel.Text = "Selected Product";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(475, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Price";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(475, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "In Stock";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(475, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Product Descirption:";
            // 
            // inputDescriptionText
            // 
            this.inputDescriptionText.Location = new System.Drawing.Point(478, 103);
            this.inputDescriptionText.Name = "inputDescriptionText";
            this.inputDescriptionText.ReadOnly = true;
            this.inputDescriptionText.Size = new System.Drawing.Size(365, 133);
            this.inputDescriptionText.TabIndex = 7;
            this.inputDescriptionText.Text = "";
            // 
            // buyBtn
            // 
            this.buyBtn.Location = new System.Drawing.Point(478, 241);
            this.buyBtn.Name = "buyBtn";
            this.buyBtn.Size = new System.Drawing.Size(365, 23);
            this.buyBtn.TabIndex = 8;
            this.buyBtn.Text = "Buy";
            this.buyBtn.UseVisualStyleBackColor = true;
            this.buyBtn.Click += new System.EventHandler(this.buyBtn_Click);
            // 
            // inputProductNameLabel
            // 
            this.inputProductNameLabel.AutoSize = true;
            this.inputProductNameLabel.Location = new System.Drawing.Point(585, 48);
            this.inputProductNameLabel.Name = "inputProductNameLabel";
            this.inputProductNameLabel.Size = new System.Drawing.Size(0, 13);
            this.inputProductNameLabel.TabIndex = 9;
            // 
            // inputPriceLabel
            // 
            this.inputPriceLabel.AutoSize = true;
            this.inputPriceLabel.Location = new System.Drawing.Point(585, 61);
            this.inputPriceLabel.Name = "inputPriceLabel";
            this.inputPriceLabel.Size = new System.Drawing.Size(0, 13);
            this.inputPriceLabel.TabIndex = 10;
            // 
            // inputInStockLabel
            // 
            this.inputInStockLabel.AutoSize = true;
            this.inputInStockLabel.Location = new System.Drawing.Point(585, 74);
            this.inputInStockLabel.Name = "inputInStockLabel";
            this.inputInStockLabel.Size = new System.Drawing.Size(0, 13);
            this.inputInStockLabel.TabIndex = 11;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 279);
            this.Controls.Add(this.inputInStockLabel);
            this.Controls.Add(this.inputPriceLabel);
            this.Controls.Add(this.inputProductNameLabel);
            this.Controls.Add(this.buyBtn);
            this.Controls.Add(this.inputDescriptionText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.productNameLabel);
            this.Controls.Add(this.productsLabel);
            this.Controls.Add(this.productsView);
            this.Controls.Add(this.shopNameLabel);
            this.Name = "MainWindow";
            this.Text = "Webshop Client";
            ((System.ComponentModel.ISupportInitialize)(this.productsView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label shopNameLabel;
        private System.Windows.Forms.DataGridView productsView;
        private System.Windows.Forms.Label productsLabel;
        private System.Windows.Forms.Label productNameLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox inputDescriptionText;
        private System.Windows.Forms.Button buyBtn;
        private System.Windows.Forms.Label inputProductNameLabel;
        private System.Windows.Forms.Label inputPriceLabel;
        private System.Windows.Forms.Label inputInStockLabel;
    }
}

