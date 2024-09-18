using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Teth
{
    [AutoloadEquip(EquipType.Head)]
    public class RegretGift : LobItemBase
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            // Links the ego weapon to the armor, used for temporary vanity set acquiration
            PEBox.EgoSets.Add(
                ModContent.ItemType<Regret>(),
                new int[3] { ModContent.ItemType<RegretGift>(), ModContent.ItemType<RegretChestplate>(), ModContent.ItemType<RegretPants>() }
                );
        }

        public override void SetDefaults()
        {
            Item.vanity = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
            EGORiskLevel = RiskLevel.Teth;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class RegretChestplate : LobItemBase
    {
        public override void SetDefaults()
        {
            Item.vanity = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
            EGORiskLevel = RiskLevel.Teth;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class RegretPants : LobItemBase
    {
        public override void SetDefaults()
        {
            Item.vanity = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
            EGORiskLevel = RiskLevel.Teth;
        }
    }
}