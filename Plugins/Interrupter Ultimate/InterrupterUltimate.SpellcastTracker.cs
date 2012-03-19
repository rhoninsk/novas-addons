using System;
using System.Collections.Generic;

using Styx.Helpers;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace InterrupterUltimate
{
    public partial class InterrupterUltimate : HBPlugin
    {
        public class UnitSpellcastingInfo
        {
            public string UnitID = "";
            public WoWUnit Unit = null;
            public bool UnitExists = false;
            public bool Included;

            public string sUnitGUID = "";
            public ulong ulUnitGUID = 0;
            public bool IsUnitEnemy = false;

            public bool IsCasting = false;
            public bool IsChannelling = false;
            public string CastName = "";
            public int SpellID = 0;
            public bool Interruptible = false;

            public float StartTime = 0;
            public float EndTime = 0;
            public float msCastTimeLeft { get { return 
                EndTime > 0 ? 
                    EndTime - Lua.GetReturnVal<float>("return GetTime() * 1000", 0) 
                    : 99999; } }
            public float secCastTimeLeft { get { return 
                EndTime / 1000 > 0 ? 
                    EndTime - Lua.GetReturnVal<float>("return GetTime()", 0) 
                    : 99999; } }
            public float msCastTimeElapsed { get { return 
                StartTime > 0 ? 
                    Lua.GetReturnVal<float>("return GetTime() * 1000", 0) - StartTime 
                    : 0; } }
            public float secCastlTimeElapsed { get { return 
                StartTime / 1000 > 0 ? 
                    Lua.GetReturnVal<float>("return GetTime()", 0) - StartTime 
                    : 0; } }
            public float TotalCastTime = 0;

            public UnitSpellcastingInfo(string unitID, bool included)
            {
                this.UnitID = unitID;
                this.Included = included;
                this.Update();
            }

            public void Update()
            {
                //dlog("Updating: " + UnitID);
                this.UnitExists = Lua.GetReturnVal<Boolean>("return UnitExists('"+ UnitID +"')", 0);
                //dlog(UnitExists.ToString());
                if (this.UnitExists)
                {
                    //dlog("unit exists");
                    this.sUnitGUID = Lua.GetReturnVal<string>(
                        String.Format("return UnitGUID(\"{0}\")", this.UnitID), 0);
                    this.IsUnitEnemy = Lua.GetReturnVal<Boolean>(
                        String.Format("return UnitCanAttack(\"player\", \"{0}\")", this.UnitID), 0);

                    string str = sUnitGUID.Replace("0x", "");
                    this.ulUnitGUID = ulong.Parse(str, System.Globalization.NumberStyles.HexNumber);

                    this.Unit = ObjectManager.GetAnyObjectByGuid<WoWUnit>(ulUnitGUID);

                    List<string> unitCastingInfo = Lua.GetReturnValues(
                                                    String.Format("return UnitCastingInfo(\"{0}\")", this.UnitID));

                    if (unitCastingInfo != null)
                    {
                        //dlog("casting detected");
                        this.CastName = unitCastingInfo[0];
                        this.IsCasting = true;
                        this.IsChannelling = false;
                        this.SpellID = this.Unit.CastingSpellId;

                        this.Interruptible = !unitCastingInfo[8].ToBoolean();

                        this.StartTime = unitCastingInfo[4].ToFloat();
                        this.EndTime = unitCastingInfo[5].ToFloat();
                        this.TotalCastTime = this.EndTime - this.StartTime;
                        return;
                    }

                    unitCastingInfo = Lua.GetReturnValues(String.Format("return UnitChannelInfo(\"{0}\")", this.UnitID));

                    if (unitCastingInfo != null)
                    {
                        //dlog("channeling detected");
                        this.CastName = unitCastingInfo[0];
                        this.IsCasting = false;
                        this.IsChannelling = true;
                        this.SpellID = this.Unit.CastingSpellId;

                        this.Interruptible = !unitCastingInfo[7].ToBoolean();

                        this.StartTime = unitCastingInfo[4].ToFloat();
                        this.EndTime = unitCastingInfo[5].ToFloat();
                        this.TotalCastTime = this.EndTime - this.StartTime;
                        return;
                    }
                }
                Reset();
                return;
            }

            public void Reset()
            {
                this.sUnitGUID = "0";
                this.ulUnitGUID = 0;
                this.Unit = null;

                this.IsCasting = false;
                this.CastName = "";
                this.IsChannelling = false;
                this.Interruptible = false;

                this.StartTime = -1;
                this.EndTime = -1;
                this.TotalCastTime = -1;
            }
        }

        public class SpellcastTracker
        {
            public Dictionary<string, UnitSpellcastingInfo> Targets = new Dictionary<string, UnitSpellcastingInfo>();

            public void Update()
            {
                //dlog("Update general..." + Targets.Count.ToString());
                foreach (KeyValuePair<string, UnitSpellcastingInfo> target in Targets)
                {
                    //dlog("included: " + target.Value.Included.ToString());
                    if (target.Value.Included)
                    {
                        //dlog("match");
                        target.Value.Update();
                    }
                    else
                        target.Value.Reset();
                }
                //dlog("Update general done.");
            }

            public void AddTargetToTrack(string unitID, bool included)
            {
                this.Targets.Add(unitID, new UnitSpellcastingInfo(unitID, included));
            }

            public void VerifyTargetToTrack(string unitID, bool included)
            {
                if (Targets.ContainsKey(unitID))
                {
                    this.Targets[unitID].Included = included;
                    return;
                }
                else
                {
                    dlog("VerifyUnitToTrack: Adding " + unitID + ".");
                    this.Targets.Add(unitID, new UnitSpellcastingInfo(unitID, included));
                    return;
                }
                
            }
        }

        //public void AttachEvents()
        //{
        //    blog("Attaching Lua events...");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_START", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_START attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_STOP", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_STOP attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_CHANNEL_START", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_CHANNEL_START attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_CHANNEL_STOP", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_CHANNEL_STOP attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_CHANNEL_UPDATE", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_CHANNEL_UPDATE attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_DELAYED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_DELAYED attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_FAILED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_FAILED attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_FAILED_QUIET", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_FAILED_QUIET attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_INTERRUPTED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_INTERRUPTED attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_INTERRUPTIBLE", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_INTERRUPTIBLE attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_NOT_INTERRUPTIBLE", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_NOT_INTERRUPTIBLE attached");
        //    Lua.Events.AttachEvent("UNIT_SPELLCAST_SUCCEEDED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_SUCCEEDED attached");
        //    blog("Lua events attached.");
        //}

        //public void DetachEvents()
        //{
        //    blog("Detaching Lua events...");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_START", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_START detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_STOP", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_STOP detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_CHANNEL_START", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_CHANNEL_START detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_CHANNEL_STOP", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_CHANNEL_STOP detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_CHANNEL_UPDATE", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_CHANNEL_UPDATE detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_DELAYED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_DELAYED detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_FAILED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_FAILED detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_FAILED_QUIET", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_FAILED_QUIET detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_INTERRUPTED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_INTERRUPTED detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_INTERRUPTIBLE", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_INTERRUPTIBLE detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_NOT_INTERRUPTIBLE", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_NOT_INTERRUPTIBLE detached");
        //    Lua.Events.DetachEvent("UNIT_SPELLCAST_SUCCEEDED", UpdateTrackerOnEvent);
        //    dlog("UNIT_SPELLCAST_SUCCEEDED detached");
        //    blog("Lua events detached.");
        //}

        //public void UpdateTrackerOnEvent(object sender, LuaEventArgs args)
        //{
        //    if (SpellTracker.Targets.ContainsKey(args.Args[0].ToString()))
        //    {
        //        SpellTracker.Update();
        //    }
        //}

    }
}