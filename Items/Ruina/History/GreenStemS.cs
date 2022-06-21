using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LobotomyCorp.Items.Ruina.History
{
	public class GreenStemS : SEgoItem
	{
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("Green Stem");
			Tooltip.SetDefault("A Realized E.G.O.\n" + 
                               "\"The day a ripe apple fell off the tree in the garden where the princess and the king stood, the witch's heart shattered.\"");
		}

        public override void SetDefaults()
        {
            PassiveText = "Enroaching Malice - Slowly spread vines around you\n" +//, attacking enemies in them\n" +
                          //"Teleport - Teleport and leave behind the vines, only usable when vines are near its maximum length\n" +
                          //"Barrier of thorns - Periodically spawns thorns that blocks a projectile on vines" +
                          "Vines fraught with Spite - Vines attacks enemies near the spread out vines\n" +
                          "Malice - Deal extra damage inversely proportional to health" +
                          "|Crumbling Life - 30% decreased damage reduction while vines are active";

            EgoColor = LobotomyCorp.WawRarity;

            Item.damage = 48;
            Item.DamageType = DamageClass.Magic;;
            Item.width = 40;
            Item.height = 40;

            Item.useTime = 10;
            Item.useAnimation = 10;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0;
            Item.value = 10000;
            Item.rare = 2;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.GreenStemArea>();
            
            Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/SnowWhite_Vine") with { Volume = 0.25f };
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.channel = true;

            Item.mana = 120;
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<GreenStem>())
            .AddIngredient(ItemID.Vilethorn)
            .AddIngredient(ItemID.NettleBurst)
            .AddIngredient(ItemID.Vine, 10)
            .AddTile<Tiles.BlackBox3>()
            .Register();
        }
	}

}