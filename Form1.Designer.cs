namespace chess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBox1 = new PictureBox();
            button1 = new Button();
            pictureBox2 = new PictureBox();
            button3 = new Button();
            button5 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            button4 = new Button();
            button6 = new Button();
            label2 = new Label();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ButtonFace;
            pictureBox1.Location = new Point(63, 36);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(680, 680);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // button1
            // 
            button1.Location = new Point(800, 376);
            button1.Name = "button1";
            button1.Size = new Size(100, 43);
            button1.TabIndex = 1;
            button1.Text = "Начать новую партию";
            button1.UseVisualStyleBackColor = true;
            button1.MouseClick += button1_MouseClick;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = SystemColors.Control;
            pictureBox2.Location = new Point(800, 36);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(120, 120);
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            pictureBox2.Visible = false;
            pictureBox2.MouseDown += pictureBox2_MouseDown;
            // 
            // button3
            // 
            button3.Location = new Point(836, 647);
            button3.Name = "button3";
            button3.Size = new Size(33, 26);
            button3.TabIndex = 4;
            button3.Text = "<";
            button3.UseVisualStyleBackColor = true;
            button3.Visible = false;
            button3.Click += button3_Click;
            // 
            // button5
            // 
            button5.Location = new Point(914, 647);
            button5.Name = "button5";
            button5.Size = new Size(33, 26);
            button5.TabIndex = 6;
            button5.Text = ">I";
            button5.UseVisualStyleBackColor = true;
            button5.Visible = false;
            button5.Click += button5_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(688, 625);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 7;
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(797, 693);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(33, 23);
            textBox1.TabIndex = 8;
            textBox1.Visible = false;
            // 
            // button4
            // 
            button4.Location = new Point(875, 647);
            button4.Name = "button4";
            button4.Size = new Size(33, 26);
            button4.TabIndex = 5;
            button4.Text = ">";
            button4.UseVisualStyleBackColor = true;
            button4.Visible = false;
            button4.Click += button4_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.White;
            button6.Location = new Point(962, 649);
            button6.Name = "button6";
            button6.Size = new Size(91, 24);
            button6.TabIndex = 9;
            button6.Text = "Вернуть ход";
            button6.UseVisualStyleBackColor = false;
            button6.Visible = false;
            button6.Click += button6_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(947, 390);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 10;
            // 
            // button2
            // 
            button2.Location = new Point(797, 647);
            button2.Name = "button2";
            button2.Size = new Size(33, 26);
            button2.TabIndex = 11;
            button2.Text = "I<";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(1173, 795);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(button6);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(pictureBox2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Шахматы";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private PictureBox pictureBox2;
        private Button button3;
        private Button button5;
        private Label label1;
        private TextBox textBox1;
        private Button button4;
        private Button button6;
        private Label label2;
        private Button button2;
    }
}
