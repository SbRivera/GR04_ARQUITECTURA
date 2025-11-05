using System;
using System.Windows.Forms;

namespace _02_CLIESC.Views
{
    partial class Index
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtCelsius;
        private TextBox txtFahrenheit;
        private Button btnConvertToFahrenheit;
        private Button btnConvertToCelsius;
        private Label lblCelsius;
        private Label lblFahrenheit;
        private Label lblResult;
        private PictureBox pictureBox1;
        private Button LogOut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Index));
            this.lblCelsius = new System.Windows.Forms.Label();
            this.txtCelsius = new System.Windows.Forms.TextBox();
            this.btnConvertToFahrenheit = new System.Windows.Forms.Button();
            this.lblFahrenheit = new System.Windows.Forms.Label();
            this.txtFahrenheit = new System.Windows.Forms.TextBox();
            this.btnConvertToCelsius = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LogOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCelsius
            // 
            this.lblCelsius.Location = new System.Drawing.Point(98, 87);
            this.lblCelsius.Name = "lblCelsius";
            this.lblCelsius.Size = new System.Drawing.Size(73, 23);
            this.lblCelsius.TabIndex = 0;
            this.lblCelsius.Text = "Celsius:";
            // 
            // txtCelsius
            // 
            this.txtCelsius.Location = new System.Drawing.Point(253, 88);
            this.txtCelsius.Name = "txtCelsius";
            this.txtCelsius.Size = new System.Drawing.Size(105, 22);
            this.txtCelsius.TabIndex = 1;
            // 
            // btnConvertToFahrenheit
            // 
            this.btnConvertToFahrenheit.Location = new System.Drawing.Point(415, 87);
            this.btnConvertToFahrenheit.Name = "btnConvertToFahrenheit";
            this.btnConvertToFahrenheit.Size = new System.Drawing.Size(114, 23);
            this.btnConvertToFahrenheit.TabIndex = 2;
            this.btnConvertToFahrenheit.Text = "Convert to °F";
            this.btnConvertToFahrenheit.Click += new System.EventHandler(this.btnConvertToFahrenheit_Click);
            // 
            // lblFahrenheit
            // 
            this.lblFahrenheit.Location = new System.Drawing.Point(98, 149);
            this.lblFahrenheit.Name = "lblFahrenheit";
            this.lblFahrenheit.Size = new System.Drawing.Size(73, 23);
            this.lblFahrenheit.TabIndex = 3;
            this.lblFahrenheit.Text = "Fahrenheit:";
            // 
            // txtFahrenheit
            // 
            this.txtFahrenheit.Location = new System.Drawing.Point(253, 150);
            this.txtFahrenheit.Name = "txtFahrenheit";
            this.txtFahrenheit.Size = new System.Drawing.Size(105, 22);
            this.txtFahrenheit.TabIndex = 4;
            // 
            // btnConvertToCelsius
            // 
            this.btnConvertToCelsius.Location = new System.Drawing.Point(415, 149);
            this.btnConvertToCelsius.Name = "btnConvertToCelsius";
            this.btnConvertToCelsius.Size = new System.Drawing.Size(114, 23);
            this.btnConvertToCelsius.TabIndex = 5;
            this.btnConvertToCelsius.Text = "Convert to °C";
            this.btnConvertToCelsius.Click += new System.EventHandler(this.btnConvertToCelsius_Click);
            // 
            // lblResult
            // 
            this.lblResult.Location = new System.Drawing.Point(98, 206);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(410, 23);
            this.lblResult.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-2, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(615, 327);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // LogOut
            // 
            this.LogOut.Location = new System.Drawing.Point(521, 283);
            this.LogOut.Name = "LogOut";
            this.LogOut.Size = new System.Drawing.Size(80, 30);
            this.LogOut.TabIndex = 8;
            this.LogOut.Text = "LogOut";
            this.LogOut.UseVisualStyleBackColor = true;
            this.LogOut.Click += new System.EventHandler(this.button1_Click);
            // 
            // Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 325);
            this.Controls.Add(this.LogOut);
            this.Controls.Add(this.lblCelsius);
            this.Controls.Add(this.txtCelsius);
            this.Controls.Add(this.btnConvertToFahrenheit);
            this.Controls.Add(this.lblFahrenheit);
            this.Controls.Add(this.txtFahrenheit);
            this.Controls.Add(this.btnConvertToCelsius);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Index";
            this.Text = "Temperature Conversion";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
