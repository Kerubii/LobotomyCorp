using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.He
{
    public class SanguineDesire : LobCorpLight
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("The axe may seem light, but the wielder musn't forget that it has hurt countless people as a consequence of poor choices.\n" +
                               "The weapon is stronger when used by an employee with strong conviction."); */
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 15;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = LobotomyCorp.WeaponSounds.Axe;
            Item.autoReuse = true;
            Item.scale = 0.7f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BloodLustCluster)
            .AddIngredient(ItemID.HellstoneBar, 5)
            .AddTile(Mod, "BlackBox2")
            .Register();

            CreateRecipe()
            .AddIngredient(ItemID.WarAxeoftheNight)
            .AddIngredient(ItemID.HellstoneBar, 5)
            .AddTile(Mod, "BlackBox2")
            .Register();
        }
    }
}