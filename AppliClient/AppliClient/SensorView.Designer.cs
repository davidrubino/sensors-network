namespace AppliClient
{
    partial class SensorView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_type = new AppliClient.MouseTransparentLabel();
            this.SuspendLayout();
            // 
            // label_type
            // 
            this.label_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_type.Location = new System.Drawing.Point(0, 0);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(30, 30);
            this.label_type.TabIndex = 0;
            this.label_type.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SensorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.label_type);
            this.Name = "SensorView";
            this.Size = new System.Drawing.Size(30, 30);
            this.LocationChanged += new System.EventHandler(this.SensorView_LocationChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private MouseTransparentLabel label_type;


    }
}
