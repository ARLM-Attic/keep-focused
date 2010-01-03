using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeepFocused
{
    public partial class KeepFocusedForm : Form
    {
        #region Form Dragging API Support
        //The SendMessage function sends a message to a window or windows.
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        //ReleaseCapture releases a mouse capture
        [DllImportAttribute("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern bool ReleaseCapture();
        #endregion

        string sessionDuration = "25:00";   //session duration in mm:ss format
        string pauseDuration = "05:00"; //pause duration in mm:ss format
        bool breakPeriod = false;


        public KeepFocusedForm()
        {
            InitializeComponent();
        }

        private void lblMoveHandler_MouseDown(object sender, MouseEventArgs e)
        {
            // drag the form without the caption bar
            // present on left mouse button
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0xa1, 0x2, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] arr;
            arr = lblTimer.Text.Split(':');
            int mins = int.Parse(arr[0]);
            int secs = int.Parse(arr[1]);

            if (mins == 0 && secs == 0)
            {
                if (breakPeriod)
                {
                    timer1.Enabled = false;
                    //mins = int.Parse(SessionDuration);
                    breakPeriod = false;
                }
                else
                {
                    mins = int.Parse(pauseDuration);
                    breakPeriod = true;
                }
            }

            TimeSpan ts = new TimeSpan(0, mins, secs);
            ts = ts.Subtract(new TimeSpan(10));
            lblTimer.Text = ts.Minutes.ToString() + ":" + ts.Seconds.ToString();
        }

        private void KeepFocusedForm_Load(object sender, EventArgs e)
        {
            lblTimer.Text = "25:00";
            lblPlayPause.Image = global::KeepFocused.Properties.Resources.stop_Icon_White;
            lblMoveHandle.Font = new Font("Wingdings", 12.00F, FontStyle.Bold);
            //new Task().ShowDialog();

        }

        /// <summary>
        /// This function is not ued now. It is a candidate for blogging or source library.
        /// </summary>
        /// <param name="button"></param>
        private void RemoveButtonBorder(Button button)
        {
            button.TabStop = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.Transparent;
            //button.FlatAppearance.BorderColor = Color.White;
            //button.FlatAppearance.CheckedBackColor = Color.White;
            //button.FlatAppearance.MouseDownBackColor = Color.White;
            //button.FlatAppearance.MouseOverBackColor = Color.White;

            button.FlatAppearance.BorderColor = this.BackColor;
            button.FlatAppearance.CheckedBackColor = this.BackColor;
            button.FlatAppearance.MouseDownBackColor = this.BackColor;
            button.FlatAppearance.MouseOverBackColor = this.BackColor;

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblPlayPause_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (timer1.Enabled)
            {
                lblTimer.Text = sessionDuration;
                lblPlayPause.Image = global::KeepFocused.Properties.Resources.stop_Icon_White;
                OpenTaskForm();
            }
            else
                lblPlayPause.Image = global::KeepFocused.Properties.Resources.Play_Black_Small;
        }

        private void lblMoveHandler_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.SizeAll;
        }

        private void lblMoveHandler_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void ChangeTextColor(Color color)
        {
            foreach(Control c in this.Controls)
                if(c is Label)
                    c.ForeColor = color;
        }

        private void ChangeBackColor(Color color)
        {
            foreach (Control c in this.Controls)
                    c.BackColor = color;
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {
            new frmInfo().ShowDialog();
        }

        private void OpenTaskForm()
        {
            Task taskForm = new Task();

            int maxLeftPosition = Screen.PrimaryScreen.WorkingArea.Width - (taskForm.Width + 20);
            int maxTopPosition = Screen.PrimaryScreen.WorkingArea.Height - (taskForm.Height + 20);

            if (this.Top + this.Height > maxTopPosition)
                taskForm.Top = this.Top - taskForm.Height - 30;
            else
                taskForm.Top = this.Top + 30;
            
            if (this.Left > maxLeftPosition)
                taskForm.Left = this.Right - taskForm.Width;
            else
                taskForm.Left = this.Left;

            taskForm.ShowDialog();
        }
    }
}
