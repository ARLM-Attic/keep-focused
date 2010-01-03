﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KeepFocused
{
    public partial class Task : Form
    {
        string TaskFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\KeepFocusedTask.txt";
        public Task()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.AppendAllText(TaskFileName, DateTime.Now.ToString("dd:MMM:yyyy hh:mm") + ":: " +  txtTask.Text + "\r\n");
            this.Close();
        }

        private void Task_Load(object sender, EventArgs e)
        {
            txtSessionDataFileName.Text = TaskFileName;
        }

        private void btnViewDataFile_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(TaskFileName);
        }
    }
}
