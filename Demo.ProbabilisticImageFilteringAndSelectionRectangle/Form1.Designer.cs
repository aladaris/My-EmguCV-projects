/*
   Copyright © 2014 Fernando González López - Peñalver <aladaris@gmail.com>
   This file is part of EmguCV-Projects.

    EmguCV-Projects is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License.

    EmguCV-Projects is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with EmguCV-Projects.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace Demo.ProbabilisticImageFilteringAndSelectionRectangle
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageBox_Main = new Emgu.CV.UI.ImageBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.imageBox_output = new Emgu.CV.UI.ImageBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_cov_33 = new System.Windows.Forms.Label();
            this.label_cov_32 = new System.Windows.Forms.Label();
            this.label_cov_31 = new System.Windows.Forms.Label();
            this.label_cov_23 = new System.Windows.Forms.Label();
            this.label_cov_22 = new System.Windows.Forms.Label();
            this.label_cov_21 = new System.Windows.Forms.Label();
            this.label_cov_13 = new System.Windows.Forms.Label();
            this.label_cov_12 = new System.Windows.Forms.Label();
            this.label_cov_11 = new System.Windows.Forms.Label();
            this.label_colorAvg = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imageBox_sample = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_output)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_sample)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox_Main
            // 
            this.imageBox_Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox_Main.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox_Main.Location = new System.Drawing.Point(3, 3);
            this.imageBox_Main.Name = "imageBox_Main";
            this.imageBox_Main.Size = new System.Drawing.Size(760, 310);
            this.imageBox_Main.TabIndex = 2;
            this.imageBox_Main.TabStop = false;
            this.imageBox_Main.Paint += new System.Windows.Forms.PaintEventHandler(this.imageBox1_Paint);
            this.imageBox_Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseDown);
            this.imageBox_Main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseMove);
            this.imageBox_Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageBox_Main_MouseUp);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.label_colorAvg);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.imageBox_sample);
            this.splitContainer1.Size = new System.Drawing.Size(959, 632);
            this.splitContainer1.SplitterDistance = 766;
            this.splitContainer1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.imageBox_Main);
            this.flowLayoutPanel1.Controls.Add(this.imageBox_output);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(766, 632);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // imageBox_output
            // 
            this.imageBox_output.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox_output.Location = new System.Drawing.Point(3, 319);
            this.imageBox_output.Name = "imageBox_output";
            this.imageBox_output.Size = new System.Drawing.Size(760, 310);
            this.imageBox_output.TabIndex = 2;
            this.imageBox_output.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.label_cov_33, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_32, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_31, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_23, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_22, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_21, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_13, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_12, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_cov_11, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 217);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(189, 226);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label_cov_33
            // 
            this.label_cov_33.AutoSize = true;
            this.label_cov_33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_33.Location = new System.Drawing.Point(128, 150);
            this.label_cov_33.Name = "label_cov_33";
            this.label_cov_33.Size = new System.Drawing.Size(58, 76);
            this.label_cov_33.TabIndex = 8;
            this.label_cov_33.Text = "0";
            this.label_cov_33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_32
            // 
            this.label_cov_32.AutoSize = true;
            this.label_cov_32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_32.Location = new System.Drawing.Point(65, 150);
            this.label_cov_32.Name = "label_cov_32";
            this.label_cov_32.Size = new System.Drawing.Size(57, 76);
            this.label_cov_32.TabIndex = 7;
            this.label_cov_32.Text = "0";
            this.label_cov_32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_31
            // 
            this.label_cov_31.AutoSize = true;
            this.label_cov_31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_31.Location = new System.Drawing.Point(3, 150);
            this.label_cov_31.Name = "label_cov_31";
            this.label_cov_31.Size = new System.Drawing.Size(56, 76);
            this.label_cov_31.TabIndex = 6;
            this.label_cov_31.Text = "0";
            this.label_cov_31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_23
            // 
            this.label_cov_23.AutoSize = true;
            this.label_cov_23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_23.Location = new System.Drawing.Point(128, 75);
            this.label_cov_23.Name = "label_cov_23";
            this.label_cov_23.Size = new System.Drawing.Size(58, 75);
            this.label_cov_23.TabIndex = 5;
            this.label_cov_23.Text = "0";
            this.label_cov_23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_22
            // 
            this.label_cov_22.AutoSize = true;
            this.label_cov_22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_22.Location = new System.Drawing.Point(65, 75);
            this.label_cov_22.Name = "label_cov_22";
            this.label_cov_22.Size = new System.Drawing.Size(57, 75);
            this.label_cov_22.TabIndex = 4;
            this.label_cov_22.Text = "0";
            this.label_cov_22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_21
            // 
            this.label_cov_21.AutoSize = true;
            this.label_cov_21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_21.Location = new System.Drawing.Point(3, 75);
            this.label_cov_21.Name = "label_cov_21";
            this.label_cov_21.Size = new System.Drawing.Size(56, 75);
            this.label_cov_21.TabIndex = 3;
            this.label_cov_21.Text = "0";
            this.label_cov_21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_13
            // 
            this.label_cov_13.AutoSize = true;
            this.label_cov_13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_13.Location = new System.Drawing.Point(128, 0);
            this.label_cov_13.Name = "label_cov_13";
            this.label_cov_13.Size = new System.Drawing.Size(58, 75);
            this.label_cov_13.TabIndex = 2;
            this.label_cov_13.Text = "0";
            this.label_cov_13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_12
            // 
            this.label_cov_12.AutoSize = true;
            this.label_cov_12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_12.Location = new System.Drawing.Point(65, 0);
            this.label_cov_12.Name = "label_cov_12";
            this.label_cov_12.Size = new System.Drawing.Size(57, 75);
            this.label_cov_12.TabIndex = 1;
            this.label_cov_12.Text = "0";
            this.label_cov_12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_cov_11
            // 
            this.label_cov_11.AutoSize = true;
            this.label_cov_11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_cov_11.Location = new System.Drawing.Point(3, 0);
            this.label_cov_11.Name = "label_cov_11";
            this.label_cov_11.Size = new System.Drawing.Size(56, 75);
            this.label_cov_11.TabIndex = 0;
            this.label_cov_11.Text = "0";
            this.label_cov_11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_colorAvg
            // 
            this.label_colorAvg.AutoSize = true;
            this.label_colorAvg.Location = new System.Drawing.Point(107, 9);
            this.label_colorAvg.Name = "label_colorAvg";
            this.label_colorAvg.Size = new System.Drawing.Size(42, 13);
            this.label_colorAvg.TabIndex = 4;
            this.label_colorAvg.Text = "B, G, R";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Covariance Matrix (BGR):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Color Mean (BGR):";
            // 
            // imageBox_sample
            // 
            this.imageBox_sample.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.imageBox_sample.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox_sample.Location = new System.Drawing.Point(0, 443);
            this.imageBox_sample.Name = "imageBox_sample";
            this.imageBox_sample.Size = new System.Drawing.Size(189, 189);
            this.imageBox_sample.TabIndex = 2;
            this.imageBox_sample.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 632);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Probabilistic Color Filtering demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Main)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_output)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_sample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox_Main;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Emgu.CV.UI.ImageBox imageBox_sample;
        private System.Windows.Forms.Label label_colorAvg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_cov_33;
        private System.Windows.Forms.Label label_cov_32;
        private System.Windows.Forms.Label label_cov_31;
        private System.Windows.Forms.Label label_cov_23;
        private System.Windows.Forms.Label label_cov_22;
        private System.Windows.Forms.Label label_cov_21;
        private System.Windows.Forms.Label label_cov_13;
        private System.Windows.Forms.Label label_cov_12;
        private System.Windows.Forms.Label label_cov_11;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Emgu.CV.UI.ImageBox imageBox_output;
        private System.Windows.Forms.Label label2;
    }
}

