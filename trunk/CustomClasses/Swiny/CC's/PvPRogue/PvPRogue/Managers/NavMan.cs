﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Logic;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;

namespace PvPRogue.Managers
{
    public static class NavMan
    {

        public static void MoveBehind(WoWUnit Unit)
        {
            // Movement Checks
            if (GlueEnabled && Unit.Distance < 10) return;
            if (!ClassSettings._Instance.GeneralMovement) return;


            WoWPoint BehindLocation = WoWMathHelper.CalculatePointBehind(Unit.Location, Unit.Rotation, 2.3f);
            Navigator.MoveTo(BehindLocation);
        }


        public static bool GlueEnabled
        {
            get
            {
                int PluginCount = (from MyPlugin in Styx.Plugins.PluginManager.Plugins
                                   where MyPlugin.Name == "Glue"
                                   where MyPlugin.Enabled == true
                                   select MyPlugin).Count();

                if (PluginCount >= 1) return true;
                return false;
            }
        }

    }
}
