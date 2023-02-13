using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles.Realized
{
	public class BlackSwanGooeyWaste : ModProjectile
	{
		public override string Texture => "LobotomyCorp/Projectiles/Realized/BlackSwanR";

		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 120;

			//Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Projectile.velocity.Y += 0.01f;
			if (Main.rand.NextBool(120))
            {
				int type = Main.rand.Next(2, 4);
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type);
				Main.dust[d].noGravity = true;
				Main.dust[d].fadeIn = 1.2f;
			}
		}



        public override bool? CanHitNPC(NPC target)
        {
			if (Projectile.Hitbox.Intersects(target.getRect()))
				target.AddBuff(ModContent.BuffType<Buffs.GooeyWaste>(), 300);
			return false;
        }
    }
}
