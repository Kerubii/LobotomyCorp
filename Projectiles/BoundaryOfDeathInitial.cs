using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class BoundaryOfDeathInitial : ModProjectile
	{
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gluttony");
        }

		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 2;
			Projectile.scale = 1f;
			Projectile.alpha = 0;
            Projectile.timeLeft = 30;

			//Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			
			Projectile.extraUpdates = 30;
		}

		public override void AI() {
			if (Projectile.timeLeft == 1)
			{
				Main.player[Projectile.owner].Teleport(Projectile.Center - Main.player[Projectile.owner].Size / 2, 5 , 5);
			}
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.penetrate > 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<BoundaryOfDeath>(), Projectile.damage, knockback, Main.myPlayer, target.whoAmI);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
