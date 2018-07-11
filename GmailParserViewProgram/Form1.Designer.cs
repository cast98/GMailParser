namespace GmailParserViewProgram
{
    partial class FormMailTrigger
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMailTrigger));
            this.l_autorization = new System.Windows.Forms.Label();
            this.btn_formLogicStart = new System.Windows.Forms.Button();
            this.p_AlertNetwork = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.Tick = new System.Windows.Forms.Timer(this.components);
            this.p_AlertNetwork.SuspendLayout();
            this.SuspendLayout();
            // 
            // l_autorization
            // 
            this.l_autorization.AutoSize = true;
            this.l_autorization.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_autorization.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.l_autorization.Location = new System.Drawing.Point(135, 9);
            this.l_autorization.Name = "l_autorization";
            this.l_autorization.Size = new System.Drawing.Size(107, 40);
            this.l_autorization.TabIndex = 2;
            this.l_autorization.Text = "log in.";
            // 
            // btn_formLogicStart
            // 
            this.btn_formLogicStart.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_formLogicStart.Location = new System.Drawing.Point(22, 61);
            this.btn_formLogicStart.Name = "btn_formLogicStart";
            this.btn_formLogicStart.Size = new System.Drawing.Size(325, 48);
            this.btn_formLogicStart.TabIndex = 3;
            this.btn_formLogicStart.Text = "authorization";
            this.btn_formLogicStart.UseVisualStyleBackColor = true;
            this.btn_formLogicStart.Click += new System.EventHandler(this.btn_formLogicStart_Click);
            // 
            // p_AlertNetwork
            // 
            this.p_AlertNetwork.BackColor = System.Drawing.Color.Red;
            this.p_AlertNetwork.Controls.Add(this.label2);
            this.p_AlertNetwork.Location = new System.Drawing.Point(12, 9);
            this.p_AlertNetwork.Name = "p_AlertNetwork";
            this.p_AlertNetwork.Size = new System.Drawing.Size(352, 100);
            this.p_AlertNetwork.TabIndex = 43;
            this.p_AlertNetwork.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(93, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Network isn\'t avalible!";
            // 
            // Tick
            // 
            this.Tick.Enabled = true;
            this.Tick.Interval = 350;
            this.Tick.Tick += new System.EventHandler(this.Tick_Tick);
            // 
            // FormMailTrigger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(373, 120);
            this.Controls.Add(this.p_AlertNetwork);
            this.Controls.Add(this.btn_formLogicStart);
            this.Controls.Add(this.l_autorization);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMailTrigger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authorization";
            this.Load += new System.EventHandler(this.FormMailTrigger_Load);
            this.p_AlertNetwork.ResumeLayout(false);
            this.p_AlertNetwork.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label l_autorization;
        private System.Windows.Forms.Button btn_formLogicStart;
        private System.Windows.Forms.Panel p_AlertNetwork;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer Tick;
    }
}

