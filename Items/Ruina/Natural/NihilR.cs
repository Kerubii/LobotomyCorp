using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Ruina.Natural
{
	public class NihilR : SEgoItem
	{
        public int ArcanaManaCost = 500;

		public override void SetStaticDefaults() {
			Tooltip.SetDefault(GetTooltip());
            
            //Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            EgoColor = LobotomyCorp.AlephRarity;
            Item.damage = 30;
			Item.DamageType = DamageClass.Generic;
			Item.width = 20;
			Item.height = 20;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 10000; 
			Item.UseSound = SoundID.Item11; 
			Item.autoReuse = true;
            Item.channel = true;
        }

        public override void HoldItem(Player player)
        {
            if (LobotomyModPlayer.ModPlayer(player).NihilCheckActive())
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Realized.Nihil.NihilQOH>()] == 0)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.Nihil.NihilQOH>(), Item.damage, Item.knockBack, player.whoAmI);
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.Nihil.NihilSOW>(), Item.damage, Item.knockBack, player.whoAmI);
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.Nihil.NihilKOG>(), Item.damage, Item.knockBack, player.whoAmI);
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Realized.Nihil.NihilKOD>(), Item.damage, Item.knockBack, player.whoAmI);
                }
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 0);
        }
    }
}
