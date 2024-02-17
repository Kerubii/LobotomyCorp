using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Teth
{
    [AutoloadEquip(EquipType.Head)]
    public class RegretGift : LobCorpHeavy
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class RegretChestplate : LobCorpHeavy
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class RegretPants : LobCorpHeavy
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 56;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }
    }
}