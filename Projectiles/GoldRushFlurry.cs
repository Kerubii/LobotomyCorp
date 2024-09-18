using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.On;

namespace LobotomyCorp.Projectiles
{
	public class GoldRushFlurry : ModProjectile
	{
        public override string Texture => "LobotomyCorp/Projectiles/GoldRushPunches";

        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("ORA");
        }

		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
            Projectile.timeLeft = 12;
			Projectile.alpha = 255;

			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;

			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] % 5 == 0)
			{
                DiamondDust(Projectile.Center, 3, 2, 5, 1f, Projectile.velocity.ToRotation());
			}
            for (int i = 0; i < 3; i++)
            {
                Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin)];
                d.noGravity = true;
				d.fadeIn = 0.1f;
				d.velocity = Projectile.velocity * 0.4f;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
			if (target.immune[Projectile.owner] > 0)
				return false;
            return base.CanHitNPC(target);
        }

        private void DiamondDust(Vector2 pos, int amount, int x, int y, float fade = 1f, float angle = 0)
        {
            for (int i = 0; i < amount; i++)
            {
                float prog = i / (float)amount;
                float x1 = x * prog;
                float x2 = x * (1f - prog);
                float y1 = y * prog;
                float y2 = y * (1f - prog);

                Dust d = Dust.NewDustPerfect(pos, DustID.GoldCoin, new Vector2(x1, -y2).RotatedBy(angle));
                d.noGravity = true;
                d.fadeIn = fade;
                d = Dust.NewDustPerfect(pos, DustID.GoldCoin, new Vector2(-x2, -y1).RotatedBy(angle));
                d.noGravity = true;
                d.fadeIn = fade;
                d = Dust.NewDustPerfect(pos, DustID.GoldCoin, new Vector2(-x1, y2).RotatedBy(angle));
                d.noGravity = true;
                d.fadeIn = fade;
                d = Dust.NewDustPerfect(pos, DustID.GoldCoin, new Vector2(x2, y1).RotatedBy(angle));
                d.noGravity = true;
                d.fadeIn = fade;
            }
        }
    }
}
