using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class Syrinx : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("What cry could be more powerful than one spurred by primal instinct?\n" +
                               "As if everything else were hollow and pointless,\nThe wailing numbs even the brain, making it impossible to think.");
        }

		public override void SetDefaults() {
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/LWeapons/fetus", 3) with { Volume = 0.5f, MaxInstances = 0};
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.SyrinxShot>();
			Item.shootSpeed = 14f;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
    }
}
