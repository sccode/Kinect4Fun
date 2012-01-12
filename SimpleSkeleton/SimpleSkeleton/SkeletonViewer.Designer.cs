namespace SimpleSkeleton
{
    partial class SkeletonViewer
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
            this.videoViewer = new System.Windows.Forms.PictureBox();
            this.textJoints = new System.Windows.Forms.TextBox();
            this.textStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.videoViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // videoViewer
            // 
            this.videoViewer.Location = new System.Drawing.Point(4, 8);
            this.videoViewer.Name = "videoViewer";
            this.videoViewer.Size = new System.Drawing.Size(640, 480);
            this.videoViewer.TabIndex = 0;
            this.videoViewer.TabStop = false;
            // 
            // textJoints
            // 
            this.textJoints.Location = new System.Drawing.Point(650, 8);
            this.textJoints.Multiline = true;
            this.textJoints.Name = "textJoints";
            this.textJoints.ReadOnly = true;
            this.textJoints.Size = new System.Drawing.Size(408, 542);
            this.textJoints.TabIndex = 2;
            // 
            // textStatus
            // 
            this.textStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textStatus.Location = new System.Drawing.Point(109, 513);
            this.textStatus.Name = "textStatus";
            this.textStatus.ReadOnly = true;
            this.textStatus.Size = new System.Drawing.Size(191, 24);
            this.textStatus.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(8, 513);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Kinect Status";
            // 
            // SkeletonViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 562);
            this.Controls.Add(this.textStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textJoints);
            this.Controls.Add(this.videoViewer);
            this.Name = "SkeletonViewer";
            this.Text = "SkeletonViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SkeletonViewer_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.videoViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox videoViewer;
        private System.Windows.Forms.TextBox textJoints;
        private System.Windows.Forms.TextBox textStatus;
        private System.Windows.Forms.Label label2;
    }
}

