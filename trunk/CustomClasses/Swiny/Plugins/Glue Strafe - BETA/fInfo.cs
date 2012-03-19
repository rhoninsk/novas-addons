using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx;

namespace Glue
{
    public partial class fInfo : Form
    {
        public static fInfo _Instance;

        public fInfo()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tmrUpdate.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            textBox1.Text = StyxWoW.Me.MovementInfo.CurrentSpeed.ToString();
            textBox2.Text = StyxWoW.Me.MovementInfo.IsStrafing.ToString();
            textBox3.Text = StyxWoW.Me.CurrentTarget.Distance.ToString();
            textBox4.Text = StyxWoW.Me.CurrentTarget.RotationDegrees.ToString();
            textBox5.Text = StyxWoW.Me.MovementInfo.MovingForward.ToString();
            textBox6.Text = StyxWoW.Me.MovementInfo.MovingStrafeLeft.ToString();

            textBox12.Text = StyxWoW.Me.CurrentTarget.MovementInfo.CurrentSpeed.ToString();
            textBox11.Text = StyxWoW.Me.CurrentTarget.MovementInfo.IsStrafing.ToString();
            textBox10.Text = StyxWoW.Me.CurrentTarget.MovementInfo.RunSpeed.ToString();
            textBox9.Text = StyxWoW.Me.CurrentTarget.MovementInfo.BackwardSpeed.ToString();
            textBox8.Text = StyxWoW.Me.CurrentTarget.MovementInfo.MovingForward.ToString();
            textBox7.Text = StyxWoW.Me.CurrentTarget.MovementInfo.MovingStrafeLeft.ToString();
        }

        private void Info_Load(object sender, EventArgs e)
        {
            label1.Text = "CurrentSpeed";
            label2.Text = "IsStrafing";
            label3.Text = "Distance";
            label4.Text = "RotationDeg";
            label5.Text = "MovingForward";
            label6.Text = "MovingStrafeLeft";

            label12.Text = "CurrentSpeed";
            label11.Text = "IsStrafing";
            label10.Text = "RunSpeed";
            label9.Text = "BackwardSpeed";
            label8.Text = "MovingForward";
            label7.Text = "MovingStrafeLeft";
        }

        private void fInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


    }
}
