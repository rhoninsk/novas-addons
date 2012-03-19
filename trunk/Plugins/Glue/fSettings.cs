using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Styx.Logic.BehaviorTree;

namespace Glue
{
    public partial class fSettings : Form
    {
        /// <summary>
        /// Our instance for our Settings Form
        /// </summary>
        public static fSettings _Instance;

        public fSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form loading - Load all settings to form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fSettings_Load(object sender, EventArgs e)
        {
            MovementEnabled = Settings._Instance.MovementEnabled;
            MovementTPS = Settings._Instance.MovementTPS;
            MovementDistance = Settings._Instance.MovementDistance;
            HonorbuddyTPS = Settings._Instance.HonorbuddyTPS;
            PlayersOnly = Settings._Instance.PlayersOnly;
        }

        /// <summary>
        /// Form Closing - Save all settings from form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings._Instance.MovementEnabled = MovementEnabled;
            Settings._Instance.HonorbuddyTPS = HonorbuddyTPS;
            Settings._Instance.PlayersOnly = PlayersOnly;
            Settings._Instance.MovementDistance = MovementDistance;
            Settings._Instance.MovementTPS = MovementTPS;

            Settings._Instance.Save();

            TreeRoot.TicksPerSecond = (byte)HonorbuddyTPS;

            // If movement is disabled, we need to remove thread
            if (MovementEnabled == true)
            {
                Movement.tMovement = null;
            }


            e.Cancel = true;
            this.Hide();
        }



        #region Properties
        public bool MovementEnabled
        {
            get
            {
                return cbxMovementEnabled.Checked;
            }
            set
            {
                cbxMovementEnabled.Checked = value;
            }
        }

        public int MovementTPS
        {
            get
            {
                return tbMovementTPS.Value;
            }
            set
            {
                lblMovementTPS.Text = value.ToString();
                tbMovementTPS.Value = value;
            }
        }

        public int MovementDistance
        {
            get
            {
                return tbMovementDistance.Value;
            }
            set
            {
                lblMovementDistance.Text = value.ToString();
                tbMovementDistance.Value = value;
            }
        }

        public int HonorbuddyTPS
        {
            get
            {
                return tbHonorbuddyTPS.Value;
            }
            set
            {
                lblHonorbuddyTPS.Text = value.ToString();
                tbHonorbuddyTPS.Value = value;
            }
        }

        public bool PlayersOnly
        {
            get
            {
                return cbxPlayersOnly.Checked;
            }
            set
            {
                cbxPlayersOnly.Checked = value;
            }
        }
        #endregion

        #region Events
        private void tbMovementTPS_Scroll(object sender, EventArgs e)
        {
            MovementTPS = tbMovementTPS.Value;
        }

        private void tbMovementDistance_Scroll(object sender, EventArgs e)
        {
            MovementDistance = tbMovementDistance.Value;
        }

        private void tbHonorbuddyTPS_Scroll(object sender, EventArgs e)
        {
            HonorbuddyTPS = tbHonorbuddyTPS.Value;
        }
        #endregion

    }
}
