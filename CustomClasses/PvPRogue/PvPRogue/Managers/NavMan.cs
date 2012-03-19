using System;
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
            WoWPoint BehindLocation = WoWMathHelper.CalculatePointBehind(Unit.Location, Unit.Rotation, 2.3f);
            Navigator.MoveTo(BehindLocation);
        }


        public static bool DoMove()
        {
            //Styx.Plugins.PluginManager.Plugins['Glue'].Enabled

            return true;
        }

    }
}
