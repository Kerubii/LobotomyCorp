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
			DisplayName.SetDefault("Regret"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault(GetTooltip());
		}

		public override void SetDefaults() 
		{
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

			Item.shoot = 0;//ModContent.ProjectileType<Projectiles.Realized.RegretR2>();
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

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			Vector2 delta = Main.MouseWorld - player.Center;
			//velocity *= delta;
        }

        public override void AddRecipes() 
		{
		}
	}
}