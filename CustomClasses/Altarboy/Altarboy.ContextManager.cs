using System;
using Styx;
using Styx.Logic;

namespace Altarboy
{
    class ContextManager
    {
        internal static WoWContext LastWoWContext { get; set; }

        internal static WoWContext CurrentWoWContext
        {
            get
            {
                if (Battlegrounds.IsInsideBattleground)
                {
                    return WoWContext.Battlegrounds;
                }
                if (StyxWoW.Me.IsInInstance)
                {
                    return WoWContext.Instances;
                }
                return WoWContext.Normal;
            }
        }
    }

    [Flags]
    public enum WoWContext
    {
        None = 0,
        Normal = 0x1,
        Instances = 0x2,
        Battlegrounds = 0x4,

        All = Normal | Instances | Battlegrounds,
    }
}
