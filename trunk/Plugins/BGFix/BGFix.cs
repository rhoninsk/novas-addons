using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Styx;
using Styx.Plugins.PluginClass;
using Styx.Logic.BehaviorTree;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;
using Styx.Patchables;
using Styx.Logic;
using Styx.Plugins;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;

using Styx.Logic.POI;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.Logic;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Styx.Logic.POI;
using System.Media;

namespace BGFix
{
	 #region form

    public class SettingsForm : Form
    {
        private FlowLayoutPanel panel = new FlowLayoutPanel();
        private Label LabelBgTimeout = new Label();
        private TextBox TextBgTimeout = new TextBox();
         
	 
        public SettingsForm()
        {
            LabelBgTimeout.Text = "Timeout:";
             	    

            TextBgTimeout.Text = BGFix.settings.BgTimeout.ToString();
            TextBgTimeout.Width = 180;
               
            panel.Dock = DockStyle.Fill;
            panel.Controls.Add(LabelBgTimeout);
            panel.Controls.Add(TextBgTimeout);
            
              
            this.Text = "Timeout";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Height = 90;
            this.Width = 190;
            this.Controls.Add(panel);
        }

        protected override void Dispose(bool disposing)
        {
           BGFix.settings.BgTimeout = int.Parse(TextBgTimeout.Text);
               
            BGFix.settings.Save();
            base.Dispose(disposing);
        }
    }

    #endregion

    #region settings

    class  BGFixSettings : Settings
    {
        public  BGFixSettings() : base(Logging.ApplicationPath + "\\Settings\\BGFix_" + StyxWoW.Me.Name + ".xml") 
        {
        Load();
        }

        [Setting, DefaultValue(90)]
        public int BgTimeout { get; set; }

              
       }

    #endregion



        class BGFix : HBPlugin
	{

	public override bool WantButton { get { return true; } }
        public override string Name { get { return "- Custom - BGFix"; } }
        public override string Author { get { return "bcrazy"; } }
        public override Version Version { get { return new Version(1,0,0);} }
	private double EnterTimeout = 5;
	private Stopwatch BGTimer = new Stopwatch();
	private double CombatTimeoutBG = 90;
	private Stopwatch combatTimerBG = new Stopwatch();
	private Stopwatch antispam = new Stopwatch();
	private double EnterTimeoutBG = 20;
	private LocalPlayer Me { get { return ObjectManager.Me; } }
	private Stopwatch SotaTimer = new Stopwatch();
	private double SotaTimeout = 60;
	private Stopwatch Sotamsg = new Stopwatch();
		
	public static BGFixSettings settings = new  BGFixSettings();
	
	public override string ButtonText
        {
            get
            {
                return "BG Timer";
            }
        }

	public override void OnButtonPress()
        {
            new SettingsForm().Show();
        }


	

       



	private static void PlaySound(string soundFile)
        {
        new SoundPlayer(Path.Combine(Logging.ApplicationPath, @"Plugins\BGfix\Sounds\") + soundFile).Play();
        }
	
	public override void Pulse()
	{
	try


	{
	
	if (Battlegrounds.IsInsideBattleground && Me.ZoneId != 4384)
	{
	
	ObjectManager.Update();
	BGTimer.Start();
	combatTimerBG.Start();
		
	}
	
	if (!Battlegrounds.IsInsideBattleground)
	{
	combatTimerBG.Reset();
	BGTimer.Reset();
	SotaTimer.Reset();
	Sotamsg.Reset();
	}
	

	
		if (combatTimerBG.Elapsed.TotalSeconds > 90 && combatTimerBG.Elapsed.TotalSeconds < 92)
		{
				
		Logging.Write(Color.Red,"[BGHelper] - BG Mode Running - Starting NoW!");
			//combatTimerBG.Stop();
			//BGTimer.Reset();
			//BGTimer.Stop();
		}	

		
	while (BGTimer.Elapsed.TotalSeconds > EnterTimeout && Battlegrounds.IsInsideBattleground && combatTimerBG.Elapsed.TotalSeconds < settings.BgTimeout && Me.ZoneId != 4384)
		{
			
			//combatTimer.Reset();
			//BGTimer.Stop();
			BotPoi.Clear();
			//Thread.Sleep(110000);
			antispam.Start();
			if (antispam.Elapsed.TotalSeconds > 20)
			{
			PlaySound("Enter.wav");	
			Logging.Write(Color.Red,"[BGHelper] - BG Detected - Wait Routine Running");
			antispam.Reset();
			
			}
			
			
		}

	// SOTA
	while (Me.HasAura("Preparation") && Battlegrounds.IsInsideBattleground && Me.ZoneId == 4384)
		{
			
			//combatTimer.Reset();
			//BGTimer.Stop();
			BotPoi.Clear();
			//Thread.Sleep(110000);
			antispam.Start();
			if (antispam.Elapsed.TotalSeconds > 20)
			{
			PlaySound("Enter.wav");	
			Logging.Write(Color.Red,"[BGHelper] - We are in Sota - Wait Till BG Starts");
			antispam.Reset();
			
			}
		
		}
		
	if (!Me.HasAura("Preparation") && Battlegrounds.IsInsideBattleground && Me.ZoneId == 4384)
	 	{
			Sotamsg.Start();
			if (Sotamsg.Elapsed.TotalSeconds < 3)
			{
			Logging.Write(Color.Red,"[BGHelper] - Sota Running - Enjoy Botting!");
			}

		}
	
	}
	catch (Exception e)
            {
                Log("ERROR: " + e.Message + ". See debug log.");
                Logging.WriteDebug("exception:");
                Logging.WriteException(e);
            }

	
	}
	 public override void Initialize()
        {
          Log("BG Monitoring" + Version );
          Log("Settings: " + settings.BgTimeout + " seconds Wait Time");
        }
	private static void Log(string format, params object[] args)
        {
            Logging.Write(Color.Red, "[BGFix] " + format, args);
        }

    }
}