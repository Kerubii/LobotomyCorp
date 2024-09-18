using LobotomyCorp.Items.Zayin;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class WingbeatGift : LobItemBase
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            // Links the ego weapon to the armor, used for temporary vanity set acquiration
            PEBox.EgoSets.Add(
                ModContent.ItemType<Wingbeat>(),
                new int[3] { ModContent.ItemType<WingbeatGift>(), ModContent.ItemType<WingbeatSuit>(), ModContent.ItemType<WingbeatPants>() }
                );
        }

        public override void SetDefaults()
        {
            Item.vanity = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            EGORiskLevel = RiskLevel.Zayin;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class WingbeatSuit : LobItemBase
    {
        public override void SetDefaults()
        {
            Item.vanity = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            EGORiskLevel = RiskLevel.Zayin;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class WingbeatPants : LobItemBase
    {
        public override void SetDefaults()
        {
            Item.vanity = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            EGORiskLevel = RiskLevel.Zayin;
        }
    }
}