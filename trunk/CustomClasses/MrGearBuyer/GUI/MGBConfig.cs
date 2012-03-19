using System;
using System.ComponentModel;
using System.Windows.Forms;
using Styx;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Patchables;
using Styx.WoWInternals;
using Styx.Helpers;

namespace MrGearBuyer.GUI
{
    public partial class MGBConfig : Form
    {
        public MGBConfig()
        {
            InitializeComponent();
            LogoutAtCap.Checked = MrGearBuyer.LogoutAtCap;
            RemoveJPHPWhenCapped.Checked = MrGearBuyer.RemoveHPJPWhenCapped;
            BuyOppositePointToBuildUp.Checked = MrGearBuyer.BuyOppositePointToBuildUp;
        }

        private BindingList<MrGearBuyer.BuyItemInfo> BuyItemList { get { return MrGearBuyer.BuyItemList; } }

        private void Hc2ConfigLoad(object sender, EventArgs e)
        {
            dgvBuyList.DataSource = BuyItemList;
            dgvVenderList.DataSource = _venderList;
        }

        private void DgvBuyListCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            RemoveBuyClick(null, null);
        }

        private void RemoveBuyClick(object sender, EventArgs e)
        {
            foreach (var item in dgvBuyList.SelectedRows)
            {
                var row = (DataGridViewRow)item;
                BuyItemList.RemoveAt(row.Index);
            }
        }

        private void BtnMoveUpClick(object sender, EventArgs e)
        {
            var count = dgvBuyList.SelectedRows.Count;

            if (count == 0)
            {
                MessageBox.Show("Please select a row first", "Select a row", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (count >= 2)
            {
                MessageBox.Show("Please select only one row to move it up", "Select only one row", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            var index = dgvBuyList.SelectedRows[0].Index;

            // Dont do anything if its the first item.
            if (index == 0)
                return;

            var item = BuyItemList[index];

            BuyItemList.RemoveAt(index);

            var newIndex = index - 1;

            BuyItemList.Insert(newIndex, item);

            foreach (var row in dgvBuyList.Rows)
            {
                ((DataGridViewRow)row).Selected = false;
            }

            dgvBuyList.Rows[newIndex].Selected = true;
        }

        private void BtnMoveDownClick(object sender, EventArgs e)
        {
            var count = dgvBuyList.SelectedRows.Count;

            if (count == 0)
            {
                MessageBox.Show("Please select a row first", "Select a row", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (count >= 2)
            {
                MessageBox.Show("Please select only one row to move it down", "Select only one row", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            var index = dgvBuyList.SelectedRows[0].Index;

            // Dont do anything if its the last item.
            if (index == BuyItemList.Count - 1)
                return;

            var item = BuyItemList[index];

            BuyItemList.RemoveAt(index);

            var newIndex = index + 1;

            BuyItemList.Insert(newIndex, item);

            foreach (var row in dgvBuyList.Rows)
            {
                ((DataGridViewRow)row).Selected = false;
            }

            dgvBuyList.Rows[newIndex].Selected = true;
        }

        private void DgvVenderListCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AddtoBuyClick(null, null);
        }

        private void AddtoBuyClick(object sender, EventArgs e)
        {
            foreach (var item in dgvVenderList.SelectedRows)
            {
                BuyItemList.Add((MrGearBuyer.BuyItemInfo)((DataGridViewRow)item).DataBoundItem);
            }
        }

        private readonly BindingList<MrGearBuyer.BuyItemInfo> _venderList = new BindingList<MrGearBuyer.BuyItemInfo>();
        private void FetchClick(object sender, EventArgs e)
        {
            ObjectManager.Update();
            if (!MerchantFrame.Instance.IsVisible)
            {
                MrGearBuyer.Slog("No Vender Frame was Open, Talk to a Vender to see the goods, then try again");
            }
            if (MerchantFrame.Instance.IsVisible)
            {
                _venderList.Clear();
                using (new FrameLock())
                {
                    foreach (var vendorItem in MerchantFrame.Instance.GetAllMerchantItems())
                    {
                        var fixedIndex = vendorItem.Index + 1;

                        var name = vendorItem.Name;

                        var costTypeLua =
                            Lua.GetReturnVal<string>(
                                "return GetMerchantItemCostItem(" + fixedIndex + ", GetMerchantItemCostInfo(" + fixedIndex + "))", 0);

                        if (string.IsNullOrEmpty(costTypeLua))
                        {
                            MrGearBuyer.Dlog("Vender item is not bought with points, Skipping. [{0}]", name);
                            continue;                            
                        }

                        var costType = "";
                        if (costTypeLua.Contains("Honor"))
                            costType = "HonorPoints";
                        else if (costTypeLua.Contains("Justice"))
                            costType = "JusticePoints";
                        else if (costTypeLua.Contains("Conquest"))
                            costType = "ConquestPoints";

                        var honorCost =
                            Lua.GetReturnVal<int>(
                                "return GetMerchantItemCostItem(" + fixedIndex + ", GetMerchantItemCostInfo(" + fixedIndex + "))", 1);

                        // Sometimes name returns empty from lua. Do another one to make sure its valid
                        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(costType) || honorCost == 0)
                        {
                            MrGearBuyer.Dlog("Vender item not usable, Skipping. [{0}]", name);
                            continue;
                        }
                        var item = new MrGearBuyer.BuyItemInfo
                        {
                            ItemCost = honorCost,
                            ItemName = name,
                            ItemSupplierId = StyxWoW.Me.CurrentTarget.Entry,
                            ItemId = vendorItem.ItemId,
                            ItemCostType = costType.Replace(" ", "")
                        };
                        _venderList.Add(item);
                    }
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void LogoutAtCapCheckedChanged(object sender, EventArgs e)
        {

            MrGearBuyer.LogoutAtCap = LogoutAtCap.Checked;

        }

        private void RemoveJphpWhenCappedCheckedChanged(object sender, EventArgs e)
        {

            MrGearBuyer.RemoveHPJPWhenCapped = RemoveJPHPWhenCapped.Checked;
        }

        private void BuyOppositePointToBuildUp_CheckedChanged(object sender, EventArgs e)
        {
            MrGearBuyer.BuyOppositePointToBuildUp = BuyOppositePointToBuildUp.Checked;
        }
    }
}
