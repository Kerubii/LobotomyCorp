using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace LobotomyCorp.Items.Ruina.Technology
{
	public class RegretR : SEgoItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("A Realized E.G.O.\n" +
							   "\"What's really pitiful is people like you dying to the likes of me.\"");
            
		}

		public override void SetDefaults() 
		{
            PassiveText = "Ends, Begins, Ends - Third strikes creates shockwaves, temporarily decreases enemy defense\n" +
                          "Metallic Ringing - Enemies with reduced defense creates shockwaves on hit\n" +
						  "Chained Wrath - Movement speed is converted to damage" + 
                          "|Binding Jacket - 35% decreased damage, Movement speed bonuses nullified, -10 defense and disable Chained Wrath while having any debuff";

			EgoColor = LobotomyCorp.TethRarity;

			Item.damage = 63;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 26;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.autoReuse = true;
            Item.channel = true;

			Item.shoot = ModContent.ProjectileType<Projectiles.Realized.RegretR>();
			Item.shootSpeed = 1f;
		}

        public override bool SafeCanUseItem(Player player)
        {
			return base.SafeCanUseItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes() 
		{
		}
	}
}