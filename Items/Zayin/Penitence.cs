using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;

namespace LobotomyCorp.Items.Zayin
{
    public class Penitence : LobCorpLight
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("To know means to understand.\n" +
							   "We successfully extracted the archetype and materialized it,\n" +
                               "and the observer reshaped it into a weapon."); */
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 15;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = LobotomyCorp.WeaponSounds.Mace;
            Item.autoReuse = true;

            //Item.shoot = 1;
        }

        public override void AddRecipes()
        {

        }
    }
}