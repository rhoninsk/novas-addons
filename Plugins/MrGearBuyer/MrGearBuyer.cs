//Mr.GearBuyer - Created by CodenameGamma - 12/26/11 - For WoW Version 4.0.3+
//www.honorbuddy.de
//this is a free plugin, and should not be sold, or repackaged.
//Donations Accepted. 
//Version 1.4


using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MrGearBuyer.GUI;
using Styx.Database;
using Styx.Logic;
using System;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Logic.POI;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.IO;
using System.Xml.Linq;
using Styx.Plugins.PluginClass;
using Styx;

namespace MrGearBuyer
{
    public class MrGearBuyer : HBPlugin
    {
        #region Override of HBPlugin

        //Normal Stuff.
        public override string Name { get { return "Mr.GearBuyer"; } }
        public override string Author { get { return "CnG"; } }
        public override Version Version { get { return new Version(1, 6); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Mr.GearBuyer"; } }

        private bool _initialized;
        public override void Initialize()
        {
            if (_initialized)
                return;

            Slog("LOCKED AND LOADED!");
            
            Vendors.OnBuyItems += BuyItems;

            // Loading item from settings file.
            LoadSettings();
            // Should subscribe to listchanged after loading items. So any add/remove will save the settings file behind the scene.
            BuyItemList.RaiseListChangedEvents = true;
            BuyItemList.ListChanged += BuyItemListListChanged;
            _initialized = true;
        }

        private Form _myForm;
        public override void OnButtonPress()
        {
            if (!_initialized)
                Initialize();

            var form = _myForm ?? (_myForm = new MGBConfig());
            form.ShowDialog();
        }
        private static WaitTimer _pulseTimer = new WaitTimer(TimeSpan.FromSeconds(4));


        public override void Pulse()
        {
            if (!_pulseTimer.IsFinished)
                return;

            _pulseTimer.Reset();
            
            if (LogoutAtCap && WoWCurrency.GetCurrencyByType(WoWCurrencyType.HonorPoints).Amount > 3750 && WoWCurrency.GetCurrencyByType(WoWCurrencyType.JusticePoints).Amount > 3750)
            {
                Slog("HonorPoints and Justice Points are Capped! Logging out!");
                InactivityDetector.ForceLogout(true);
            }
            // We should avoid overriding any poi set by the core
            if (BotPoi.Current.Type != PoiType.None)
                return;

            // There are no vendors in battlegrounds or dungeons !
            if (Styx.Logic.Battlegrounds.IsInsideBattleground || Me.IsInInstance)
                return;

            // First item in the list. We should check item by item so we don't end up buying the last item in the list with lower cost.
            var firstItem = BuyItemList.FirstOrDefault();

            // BuyItemList looks to be empty. Wait for user to populate the list
            if (firstItem == null)
                return;

            // Should check if we have enough currency.
            var currencyType = Enum.Parse(typeof (WoWCurrencyType), firstItem.ItemCostType);

            // Something went wrong with parsing. We should avoid buying that item.
            if (!(currencyType is WoWCurrencyType))
            {
                Slog("Couldn't parse item's cost type ({0}). Please consult to the plugin writer", firstItem.ItemCostType);
                BuyItemList.Remove(firstItem);
                
                return;
            }

            // Actually checking if we have enough of that currency now.
            var currency = WoWCurrency.GetCurrencyByType((WoWCurrencyType)currencyType);
            var currencyJp = WoWCurrency.GetCurrencyByType(WoWCurrencyType.JusticePoints);
            var currencyHp = WoWCurrency.GetCurrencyByType(WoWCurrencyType.HonorPoints);
            if (currency == null)
                return;

            if (currency.Amount < firstItem.ItemCost)
            {
                // Don't ever buy justice points to buy honor points and vice versa. Otherwise we will enter in an endless loop which will drop 
                // the total of our points.
                if (firstItem.ItemId != 392 && firstItem.ItemId != 395 && BuyOppositePointToBuildUp)
                {
                    if (currency.CurrencyType == WoWCurrencyType.JusticePoints && currencyHp.Amount >= 375)
                    {
                        // We set this to true here. So we don't end up spending all our honor points if the Only remove hp/jp points when capped is true
                        _forceAddedPoints = true;
                        if (Me.IsAlliance)
                        {
                            var buyJusticePoint = new BuyItemInfo
                                                        {
                                                            ItemCost = 375,
                                                            ItemName = "Justice Points",
                                                            ItemSupplierId = 52029,
                                                            ItemId = 395,
                                                            ItemCostType = WoWCurrencyType.HonorPoints.ToString()
                                                        };
                            Slog(
                                "Adding Justice Point to the Buy List so we can build up Justice Points to buy {0}.", firstItem.ItemName);
                            BuyItemList.Insert(0, buyJusticePoint);
                        }
                        if (Me.IsHorde)
                        {
                            var buyJusticePoint = new BuyItemInfo
                                                        {
                                                            ItemCost = 375,
                                                            ItemName = "Justice Points",
                                                            ItemSupplierId = 52033,
                                                            ItemId = 395,
                                                            ItemCostType = WoWCurrencyType.HonorPoints.ToString()
                                                        };
                            Slog(
                                "Adding Justice Point to the Buy List so we can build up Justice Points to buy {0}.", firstItem.ItemName);
                            BuyItemList.Insert(0, buyJusticePoint);
                        }
                    }
                    if (currency.CurrencyType == WoWCurrencyType.HonorPoints && currencyJp.Amount >= 375)
                    {
                        // We set this to true here. So we don't end up spending all our justice points if the Only remove hp/jp points when capped is true
                        _forceAddedPoints = true;
                        if (Me.IsAlliance)
                        {
                            var buyHonorPoint = new BuyItemInfo
                                                    {
                                                        ItemCost = 375,
                                                        ItemName = "Honor Points",
                                                        ItemSupplierId = 52028,
                                                        ItemId = 392,
                                                        ItemCostType = WoWCurrencyType.JusticePoints.ToString()
                                                    };
                            Slog("Adding HonorPoint to the Buy List so we can build up HonorPoints to buy {0}.", firstItem.ItemName);
                            BuyItemList.Insert(0, buyHonorPoint);
                        }
                        if (Me.IsHorde)
                        {
                            var buyHonorPoint = new BuyItemInfo
                                                    {
                                                        ItemCost = 375,
                                                        ItemName = "Honor Points",
                                                        ItemSupplierId = 52034,
                                                        ItemId = 392,
                                                        ItemCostType = WoWCurrencyType.JusticePoints.ToString()
                                                    };
                            Slog("Adding HonorPoint to the Buy List so we can build up HonorPoints to buy {0}.", firstItem.ItemName);
                            BuyItemList.Insert(0, buyHonorPoint);
                        }
                    }
                }
                return;
            }  
          

            // We need to find the vendor
            var vendorAsUnit =
                ObjectManager.GetObjectsOfType<WoWUnit>(false, false).FirstOrDefault(
                    u => u.Entry == firstItem.ItemSupplierId);
            Vendor vendor;
            // Vendor is not around. This won't work
            if (vendorAsUnit == null)
            {
                // Check the database for the vendor as a second hope
                NpcResult npc = NpcQueries.GetNpcById(firstItem.ItemSupplierId);
                if (npc != null)
                {
                    vendor = new Vendor(npc.Entry, npc.Name, Vendor.VendorType.Unknown, npc.Location);
                }
                else
                {
                    Slog("Please move your toon close to the vendor. Otherwise HonorCap won't be able to buy items.");
                    return;
                }
            }
            else
            {
               
                vendor = new Vendor(vendorAsUnit, Vendor.VendorType.Unknown);
            }

            // Setting ItemToBuy here so VendorBehavior knows which item we want.
            ItemToBuy = firstItem;

            //We need to make sure vender is usable, so removing blacklist. 
            if (Blacklist.Contains(vendorAsUnit))
            {
                Slog("For whatever reason vender is blacklisted, Clearing Blacklist.");
                Blacklist.Flush();
            }

            // Finally setting the poi
            BotPoi.Current = new BotPoi(vendor, PoiType.Buy);
           
        }

        #endregion

        #region Methods

        //Logging Class for your conviance
        public static void Slog(string format, params object[] args)
        {
            Logging.Write(Color.LightGreen, "[Mr.GearBuyer]: " + format, args);
        }
        public static void Dlog(string format, params object[] args)
        {
            Logging.WriteDebug(Color.LightGreen, "[Mr.GearBuyer-DEBUG]: " + format, args);
        }

        private static void BuyItemListListChanged(object sender, ListChangedEventArgs e)
        {
            SaveSettings();
        }

        private static void BuyItems(BuyItemsEventArgs args)
        {
            if (ItemToBuy == null)
                return;

            // Little hack here for just Honor and Justice points. Core fails to buy these 2 
            if (ItemToBuy.ItemId == 392 || ItemToBuy.ItemId == 395)
            {
                var item = MerchantFrame.Instance.GetAllMerchantItems().FirstOrDefault(i => i.ItemId == ItemToBuy.ItemId);

                if (item == null)
                {
                    Slog("Oops! Something went wrong while trying to buy {0}[{1}]. Please consult to the profile writer", ItemToBuy.ItemName, ItemToBuy.ItemId);
                }
                else
                {
                    Lua.DoString("BuyMerchantItem(" + (item.Index + 1) + ")");
                    // Adding a sleep here to let WoW update itself with currencies. So we don't end up buying something wrong.
                    Thread.Sleep(1000);
                    _pulseTimer.Reset();
                }
            }
            else
            {
                args.BuyItemsIds.Add(ItemToBuy.ItemId, 1);
            }

            Slog("Bought item \"{0}\".", ItemToBuy.ItemName);

            if (!RemoveHPJPWhenCapped || (ItemToBuy.ItemId != 392 && ItemToBuy.ItemId != 395) || _forceAddedPoints)
            {
                Slog("Removing {0} from List Since we bought the Item", ItemToBuy.ItemName);
                BuyItemList.Remove(ItemToBuy);
                _forceAddedPoints = false;
            }
            else
            {
                if (ItemToBuy.ItemId == 392 && WoWCurrency.GetCurrencyByType(WoWCurrencyType.HonorPoints).Amount >= 3750)
                {
                    Slog("Removing Honor Points from List Since Capped");
                    BuyItemList.Remove(ItemToBuy);
                }
                else if (ItemToBuy.ItemId == 395 && WoWCurrency.GetCurrencyByType(WoWCurrencyType.JusticePoints).Amount >= 3750)
                {
                    Slog("Removing Justice Points from List Since Capped");
                    BuyItemList.Remove(ItemToBuy);
                }
            }

            ItemToBuy = null;
        }

        #endregion

        #region Properties and Fields

        private static bool _forceAddedPoints;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static BuyItemInfo ItemToBuy { get; set; }

        public static readonly BindingList<BuyItemInfo> BuyItemList = new BindingList<BuyItemInfo>();

        private static bool _logoutAtCap = true;
        public static bool LogoutAtCap { get { return _logoutAtCap; } set { _logoutAtCap= value; SaveSettings(); } }

        private static bool _removeHPJPWhenCapped = true;
        public static bool RemoveHPJPWhenCapped { get { return _removeHPJPWhenCapped; } set { _removeHPJPWhenCapped = value; SaveSettings(); } }

        private static bool _buyOppositePointToBuildUp = true;
        public static bool BuyOppositePointToBuildUp { get { return _buyOppositePointToBuildUp; } set { _buyOppositePointToBuildUp = value; SaveSettings(); } }

        #endregion

        #region Settings

        private static string SavePath
        {
            get { return string.Format("{0}\\Settings\\MrGearBuyerSettings_{1}.xml", Logging.ApplicationPath, StyxWoW.Me.Name); }
        }

        private static void LoadSettings()
        {
            if (!File.Exists(SavePath))
                return;

            var xml = XElement.Load(SavePath);

            // Loading settings first.
            var settingsXml = xml.Element("Settings");

            if (settingsXml != null)
            {
                var logoutAtCap = settingsXml.Element("LogoutAtCap");

                if (logoutAtCap != null)
                    _logoutAtCap = Convert.ToBoolean(logoutAtCap.Value);

                var removeHPJPWhenCapped = settingsXml.Element("RemoveHPJPWhenCapped");

                if (removeHPJPWhenCapped != null)
                    _removeHPJPWhenCapped = Convert.ToBoolean(removeHPJPWhenCapped.Value);

                var buyOppositePointToBuildUp = settingsXml.Element("BuyOppositePointToBuildUp");

                if (buyOppositePointToBuildUp != null)
                    _buyOppositePointToBuildUp = Convert.ToBoolean(buyOppositePointToBuildUp.Value);
            }

            var items = xml.Element("Items");

            if (items != null)
            {
                foreach (var t in items.Elements("Item"))
                {
                    BuyItemList.Add(new BuyItemInfo(t));
                }
            }
        }

        private static void SaveSettings()
        {
// ReSharper disable AssignNullToNotNullAttribute
            if (!Directory.Exists(Path.GetDirectoryName(SavePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
// ReSharper restore AssignNullToNotNullAttribute

            var xml = new XElement("MrGearBuyerConfig",
                            new XElement("Settings",
                                new XElement("LogoutAtCap", LogoutAtCap),
                                new XElement("RemoveHPJPWhenCapped", RemoveHPJPWhenCapped),
                                new XElement("BuyOppositePointToBuildUp", BuyOppositePointToBuildUp)
                                ),
                            new XElement("Items",
                                from i in BuyItemList
                                select i.ToXml()));

            xml.Save(SavePath);
        }

        #endregion

        #region Nested Class BuyItemInfo

        public class BuyItemInfo : INotifyPropertyChanged
        {
            #region Constructors

            public BuyItemInfo()
            { }

            public BuyItemInfo(XElement xml)
            {
                // ReSharper disable PossibleNullReferenceException
                ItemId = Convert.ToUInt32(xml.Element("ItemId").Value);
                ItemName = xml.Element("ItemName").Value;
                ItemCost = Convert.ToInt32(xml.Element("ItemCost").Value);
                ItemCostType = xml.Element("ItemCostType").Value;
                ItemSupplierId = Convert.ToUInt32(xml.Element("ItemSupplierId").Value);
                // ReSharper restore PossibleNullReferenceException
            }

            #endregion

            #region Properties

            private string _itemName;

            public string ItemName
            {
                get { return _itemName; }
                set
                {
                    _itemName = value;
                    OnPropertyChanged("ItemName");
                }
            }

            private uint _itemId;
            [Browsable(false)]
            public uint ItemId
            {
                get { return _itemId; }
                set
                {
                    _itemId = value;
                    OnPropertyChanged("ItemID");
                }
            }

            private int _itemCost;
            public int ItemCost
            {
                get { return _itemCost; }
                set
                {
                    _itemCost = value;
                    OnPropertyChanged("ItemCost");
                }
            }

            private string _itemCostType;
            public string ItemCostType
            {
                get { return _itemCostType; }
                set
                {
                    _itemCostType = value;
                    OnPropertyChanged("ItemCostType");
                }
            }

            private uint _itemSupplierId;
            [Browsable(false)]
            public uint ItemSupplierId
            {
                get { return _itemSupplierId; }
                set
                {
                    _itemSupplierId = value;
                    OnPropertyChanged("ItemSupplierId");
                }
            }

            #endregion

            #region Methods

            public XElement ToXml()
            {
                return new XElement(
                            "Item",
                            new XElement("ItemId", ItemId),
                            new XElement("ItemName", ItemName),
                            new XElement("ItemCost", ItemCost),
                            new XElement("ItemCostType", ItemCostType),
                            new XElement("ItemSupplierId", ItemSupplierId)
                            );
            }

            #endregion

            #region Overrides

            public override string ToString()
            {
                return string.Format("ItemId:{0} ItemName:{1} ItemCost:{2} ItemCostType:{3} ItemSupplierId:{4}", ItemId, ItemName, ItemCost, ItemCostType, ItemSupplierId);
            }

            #endregion

            #region Implementation of INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string fieldName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(fieldName));
                }
            }

            #endregion
        }

        #endregion
    }
}

