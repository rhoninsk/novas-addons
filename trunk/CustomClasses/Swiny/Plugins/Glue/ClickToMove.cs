using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Logic.Pathing;
using Styx.WoWInternals;


namespace Glue
{
    internal static class ClickToMove
    {

        /*
   OLD ->
              if ( dword_DD4A24 != 13 )
              {
                if ( dword_DD4A24 == 4 )           <------- Push
                {
                  _EAX = a2;
                  __asm
                  {
                    movss   xmm2, dword_DD4A98
                    movss   xmm0, dword ptr [eax+14h]
                    movss   xmm1, dword ptr [eax+24h]
                    movss   xmm3, dword_DD4A9C
                    movss   xmm4, dword_DD4A94
         * 
         * 
         * 
              if ( dword_DD43EC != 13 )
              {
                if ( dword_DD43EC == 4 )
                {
                  _EAX = a2;
                  __asm
                  {
                    movss   xmm2, dword_DD4460
                    movss   xmm0, dword ptr [eax+14h]
                    movss   xmm1, dword ptr [eax+24h]
                    movss   xmm3, dword_DD4464
                    movss   xmm4, dword_DD445C
        */


        /// <summary>
        /// do not touch these or your computer will blow up, SRSLY.
        /// </summary>
        enum eClickToMove
        {
            DestinationX = 0x9D445C,                  // 4.3.2.15354
            DestinationY = DestinationX + 0x4,          // 4.3.2.15354
            DestinationZ = DestinationX + 0x8,          // 4.3.2.15354
            Type = 0x9D43EC,                          // 4.3.2.15354
        }



        internal static void MoveTo(WoWPoint Location)
        {
            MoveTo(Location.X, Location.Y, Location.Z);

            // Rewrite this using just 1 write, as a struct.
            // from mem it was this ->

            //var CTM = new WoWMovement.ClickToMoveInfoStruct;
            
        }

        internal static void MoveTo(float X, float Y, float Z)
        {
            ObjectManager.Wow.Write<float>((uint)ObjectManager.Wow.ImageBase + (uint)eClickToMove.DestinationX, X);
            ObjectManager.Wow.Write<float>((uint)ObjectManager.Wow.ImageBase + (uint)eClickToMove.DestinationY, Y);
            ObjectManager.Wow.Write<float>((uint)ObjectManager.Wow.ImageBase + (uint)eClickToMove.DestinationZ, Z);
            ObjectManager.Wow.Write<int>((uint)ObjectManager.Wow.ImageBase + (uint)eClickToMove.Type, (int)WoWMovement.ClickToMoveType.Move);
        }

        

        // Might have to implement something like this soon
        // This is out of my Gatherer Standalone ( for further refrence etc )
        
        // Note Apoc says that wow has update so this only works 70% of the time
        // Needs to be Updated(); - But since im moving 45 tps, its pre much updating every possible frame as is.
        
        // Could be a hackish way around it

        ///// <summary>
        ///// Gets my rotation ( view )
        ///// </summary>
        //public static float Rotation
        //{
        //    get { return ProcessManager.WoWProcess.ReadFloat((uint)ObjectManager.pPlayerBase + (uint)Addresses.eUnitOffsets.R); }
        //    set { ProcessManager.WoWProcess.WriteFloat((uint)ObjectManager.pPlayerBase + (uint)Addresses.eUnitOffsets.R, value); }
        //}
    }
}
