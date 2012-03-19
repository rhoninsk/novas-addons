using System.IO;
using Styx;
using Styx.Helpers;

namespace Dunatanks3
{
    public class DunatanksSettings : Settings
    {
        public static readonly DunatanksSettings Instance = new DunatanksSettings();

        public DunatanksSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/Config/Dunatanks-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        
        // Specc

        [Setting, DefaultValue(false)]
        public bool useArms { get; set; }

        [Setting, DefaultValue(false)]
        public bool useFury { get; set; }

        [Setting, DefaultValue(false)]
        public bool useProt { get; set; }        
        
        // PvP

        [Setting, DefaultValue(false)]
        public bool usePvPRota { get; set; }

        [Setting, DefaultValue(false)]
        public bool UsePiercingHowlPvP { get; set; }

        // Shouts

        [Setting, DefaultValue(true)]
        public bool ArmsuseBattleShout { get; set; }

        [Setting, DefaultValue(false)]
        public bool ArmsuseCommandingShout { get; set; }

        [Setting, DefaultValue(true)]
        public bool FuryuseBattleShout { get; set; }

        [Setting, DefaultValue(false)]
        public bool FuryuseCommandingShout { get; set; }

        [Setting, DefaultValue(false)]
        public bool ProtuseBattleShout { get; set; }

        [Setting, DefaultValue(true)]
        public bool ProtuseCommandingShout { get; set; }
        
        // Healthstone

        [Setting, DefaultValue(true)]
        public bool ArmsuseHealthStone { get; set; }

        [Setting, DefaultValue(true)]
        public bool FuryuseHealthStone { get; set; }

        [Setting, DefaultValue(true)]
        public bool ProtuseHealthStone { get; set; }

        [Setting, DefaultValue(15)]
        public int ArmsHealthStonePercent { get; set; }

        [Setting, DefaultValue(15)]
        public int FuryHealthStonePercent { get; set; }

        [Setting, DefaultValue(15)]
        public int ProtHealthStonePercent { get; set; }

        // Stances

        [Setting, DefaultValue(false)]
        public bool ArmsAutoSwitchBattleStance { get; set; }

        [Setting, DefaultValue(true)]
        public bool FuryAutoSwitchBerserkerStance { get; set; }

        [Setting, DefaultValue(true)]
        public bool ProtAutoSwitchDefensiveStance { get; set; }

        // Fury

        [Setting, DefaultValue(true)]
        public bool useTG { get; set; }

        [Setting, DefaultValue(true)]
        public bool useSMF { get; set; }
        
        // Demo Shout

        [Setting, DefaultValue(true)]
        public bool useDemoShoutArmsBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool useDemoShoutArmsAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool useDemoShoutFuryBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool useDemoShoutFuryAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool useDemoShoutProtBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool useDemoShoutProtAoE { get; set; }

        // Pummel

        [Setting, DefaultValue(true)]
        public bool usePummelArmsBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool usePummelArmsAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool usePummelFuryBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool usePummelFuryAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool usePummelProtBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool usePummelProtAoE { get; set; }

        // Victory Rush

        [Setting, DefaultValue(true)]
        public bool useVictoryRushArmsBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVictoryRushArmsAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVictoryRushFuryBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVictoryRushFuryAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVictoryRushProtBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVictoryRushProtAoE { get; set; }

        // Cleave

        [Setting, DefaultValue(true)]
        public bool useCleaveArmsAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool useCleaveFuryAoE { get; set; }

        // WW

        [Setting, DefaultValue(true)]
        public bool useWhirlWindFuryAoE { get; set; }

        // Disarm

        [Setting, DefaultValue(true)]
        public bool useDisarmProtBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool useDisarmProtAoE { get; set; }

        // Taunt

        [Setting, DefaultValue(true)]
        public bool useTauntProtAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool useChallengingShoutProtAoE { get; set; }

        // ShieldWall

        [Setting, DefaultValue(true)]
        public bool useShieldWallProtAoE { get; set; }

        // Vigilance

        [Setting, DefaultValue(true)]
        public bool useVigilanceProt { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVigilanceOnRandom { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVigilanceOnSpecific { get; set; }

        [Setting, DefaultValue("")]
        public string VigilanceSpecificName { get; set; }

        // Last Stand

        [Setting, DefaultValue(30)]
        public int LastStandPercentBoss { get; set; }

        [Setting, DefaultValue(30)]
        public int LastStandPercentAoE { get; set; }

        // Recklessness

        [Setting, DefaultValue(true)]
        public bool useRecklessnessArmsOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool useRecklessnessArmsBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool useRecklessnessArmsEnrage { get; set; }

        [Setting, DefaultValue(false)]
        public bool DoNotuseRecklessnessArms { get; set; }

        [Setting, DefaultValue(true)]
        public bool useRecklessnessFuryOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool useRecklessnessFuryBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool useRecklessnessFuryEnrage { get; set; }

        [Setting, DefaultValue(false)]
        public bool DoNotuseRecklessnessFury { get; set; }

        [Setting, DefaultValue(true)]
        public bool useRecklessnessArmsAoE { get; set; }

        [Setting, DefaultValue(true)]
        public bool useRecklessnessFuryAoE { get; set; }

        // Death Wish

        [Setting, DefaultValue(true)]
        public bool useDeathWishOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool useDeathWishBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool useDeathWishEnrage { get; set; }

        [Setting, DefaultValue(false)]
        public bool DoNotuseDeathWish { get; set; }

        [Setting, DefaultValue(true)]
        public bool useDeathWishFuryAoE { get; set; }

        // Bladestorm

        [Setting, DefaultValue(false)]
        public bool useBladestormArmsAoE { get; set; }

        // Sweeping Strikes

        [Setting, DefaultValue(false)]
        public bool useSweepingStrikesArmsAoE { get; set; }

        // Deadly Calm

        [Setting, DefaultValue(true)]
        public bool useDeadlyCalmArmsOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool useDeadlyCalmArmsBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool useDeadlyCalmArmsEnrage { get; set; }

        [Setting, DefaultValue(false)]
        public bool DoNotuseDeadlyCalm { get; set; }

        // Stack up Sunder Armor

        [Setting, DefaultValue(false)]
        public bool StackUpSunderArms { get; set; }

        [Setting, DefaultValue(false)]
        public bool StackUpSunderFury { get; set; }

        // Potions

        [Setting, DefaultValue(true)]
        public bool ArmsusePotion { get; set; }

        [Setting, DefaultValue(true)]
        public bool FuryusePotion { get; set; }

        [Setting, DefaultValue(true)]
        public bool ProtusePotion { get; set; }

        [Setting, DefaultValue(20)]
        public int ArmsPotionPercent { get; set; }

        [Setting, DefaultValue(20)]
        public int FuryPotionPercent { get; set; }

        [Setting, DefaultValue(20)]
        public int ProtPotionPercent { get; set; }

        // Colossus Smash

        [Setting, DefaultValue(false)]
        public bool UseColossusSmashOnCdArms { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseColossusSmashOnCdFury { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseColossusSmashRageArms { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseColossusSmashRageFury { get; set; }

        [Setting, DefaultValue(40)]
        public int ColossusSmashPercentArms { get; set; }

        [Setting, DefaultValue(40)]
        public int ColossusSmashPercentFury { get; set; }

        // Inner Rage

        [Setting, DefaultValue(false)]
        public bool UseInnerRageArmsAoE { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseInnerRageFuryAoE { get; set; }

        [Setting, DefaultValue(false)]
        public bool PrioCleaveArmsAoE { get; set; }

        [Setting, DefaultValue(false)]
        public bool PrioCleaveFuryAoE { get; set; }

        // Trinket One

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneArmsOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneFuryOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneProtOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneArmsHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneFuryHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneProtHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneArmsBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneFuryBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneProtBelow20 { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketOneArms { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketOneFury { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketOneProt { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketOneArmsCC { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketOneFuryCC { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketOneProtCC { get; set; }


        // Trinket Two

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoArmsOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoFuryOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoProtOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoArmsHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoFuryHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoProtHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoArmsBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoFuryBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoProtBelow20 { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketTwoArms { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketTwoFury { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketTwoProt { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketTwoArmsCC { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketTwoFuryCC { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketTwoProtCC { get; set; }

        // Berserker Rage

        [Setting, DefaultValue(false)]
        public bool UseBerserkerRageArmsAoE { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseBerserkerRageFuryAoE { get; set; }

        // Flasks

        [Setting, DefaultValue(false)]
        public bool UseFlaskOfBattle { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseFlaskOfFlowingWater { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseFlaskOfSteelskin { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseFlaskOfDraconicMind { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseFlaskOfTheWinds { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseFlaskOfTitanicStrength { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseFlask { get; set; }

        // Potions

        [Setting, DefaultValue(false)]
        public bool UseGolembloodPotionBelow20 { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseGolembloodPotionHeroism { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseGolembloodPotion { get; set; }

        // Movement

        [Setting, DefaultValue(false)]
        public bool DisableMovement { get; set; }

        [Setting, DefaultValue(true)]
        public bool PullHeroicThrow { get; set; }

        [Setting, DefaultValue(true)]
        public bool PullCharge { get; set; }

        // Pull

        [Setting, DefaultValue(false)]
        public bool usePullBehaviour { get; set; }

        [Setting, DefaultValue(20)]
        public int PullRange { get; set; }

        [Setting, DefaultValue(false)]
        public bool PullUseRanged { get; set; }

        [Setting, DefaultValue(false)]
        public bool PullUseHeroicThrow { get; set; }

        // Sounds

        [Setting, DefaultValue(true)]
        public bool UseSounds { get; set; }

        // Combat distance

        [Setting, DefaultValue(10)]
        public int CombatDistance { get; set; }

        // Pummel at the end of cast

        [Setting, DefaultValue(false)]
        public bool usePummelAtEndArms { get; set; }

        [Setting, DefaultValue(false)]
        public bool usePummelAtEndFury { get; set; }

        [Setting, DefaultValue(false)]
        public bool usePummelAtEndProt { get; set; }

        // Auto-Target

        [Setting, DefaultValue(false)]
        public bool useAutoTargetProt { get; set; }

        // Force Single Target rotation

        [Setting, DefaultValue(false)]
        public bool forceSingleTargetRotationArms { get; set; }

        [Setting, DefaultValue(false)]
        public bool forceSingleTargetRotationFury { get; set; }

        // Hamstring

        [Setting, DefaultValue(false)]
        public bool useHamstring { get; set; }

        // AoEPummel

        [Setting, DefaultValue(false)]
        public bool usePummelAoEAutoArms { get; set; }

        [Setting, DefaultValue(false)]
        public bool usePummelAoEAutoFury { get; set; }

        [Setting, DefaultValue(false)]
        public bool usePummelAoEAutoProt { get; set; }

        // Intercept

        [Setting, DefaultValue(false)]
        public bool useInterceptApproachPvP { get; set; }

        [Setting, DefaultValue(false)]
        public bool useInterceptInterruptPvP { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseIntercept { get; set; }

        // Rest

        [Setting, DefaultValue(true)]
        public bool UseRest { get; set; }

        [Setting, DefaultValue(25)]
        public int RestPercent { get; set; }

        // ZA
        [Setting, DefaultValue(true)]
        public bool chainhealtrash { get; set; }
        [Setting, DefaultValue(true)]
        public bool malafh { get; set; }
        [Setting, DefaultValue(true)]
        public bool malahl { get; set; }
        [Setting, DefaultValue(true)]
        public bool malahw { get; set; }
        [Setting, DefaultValue(true)]
        public bool malapummelfb { get; set; }
        [Setting, DefaultValue(true)]
        public bool malareflfb { get; set; }
        [Setting, DefaultValue(true)]
        public bool malapummelcl { get; set; }
        [Setting, DefaultValue(true)]
        public bool malareflcl { get; set; }
        // ZG
        [Setting, DefaultValue(true)]
        public bool hpvwhisper { get; set; }
        [Setting, DefaultValue(true)]
        public bool hazzapummelwrath { get; set; }
        [Setting, DefaultValue(true)]
        public bool hazzareflwrath { get; set; }
        [Setting, DefaultValue(true)]
        public bool hpktob { get; set; }
        [Setting, DefaultValue(true)]
        public bool hpkpummelsb { get; set; }
        [Setting, DefaultValue(true)]
        public bool hpkreflsb { get; set; }
        [Setting, DefaultValue(true)]
        public bool zanzilpummelvb { get; set; }
        [Setting, DefaultValue(true)]
        public bool zanzilreflvb { get; set; }
        // BoT
        [Setting, DefaultValue(true)]
        public bool halfussn { get; set; }
        [Setting, DefaultValue(true)]
        public bool felupummelhl { get; set; }
        [Setting, DefaultValue(true)]
        public bool felureflhl { get; set; }
        [Setting, DefaultValue(true)]
        public bool choconversion { get; set; }
        [Setting, DefaultValue(true)]
        public bool chotwisteddevotion { get; set; }
        // BD
        [Setting, DefaultValue(true)]
        public bool arcanotronsn { get; set; }
        [Setting, DefaultValue(true)]
        public bool arcanotronpummelaa { get; set; }
        [Setting, DefaultValue(true)]
        public bool arcanotronreflaa { get; set; }
        [Setting, DefaultValue(true)]
        public bool maloriakas { get; set; }
        [Setting, DefaultValue(true)]
        public bool nefarianbn { get; set; }
        // TotFW
        [Setting, DefaultValue(false)]
        public bool totfwtrash { get; set; }
        // FL
        [Setting, DefaultValue(true)]
        public bool alysrazorf { get; set; }
        [Setting, DefaultValue(true)]
        public bool alysrazori { get; set; }
        // HoT
        [Setting, DefaultValue(true)]
        public bool tyrandes { get; set; }
        [Setting, DefaultValue(true)]
        public bool queento { get; set; }




    }
}