using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class WristCutter : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.CopperShortswordStab);
			AIType = ProjectileID.CopperShortswordStab;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(ModContent.BuffType<Buffs.Scars>(), 60);
		}

        public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Scars>(), 60);
		}

		public override bool PreDraw(ref Color lightColor)
        {
			Vector2 pos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle? sourceRectangle2 = null;
			Color color = Projectile.GetAlpha(lightColor);
			Vector2 origin = tex.Size() / 2;
			SpriteEffects spriteEffects = 0;
			if (Projectile.spriteDirection == -1)
				spriteEffects = (SpriteEffects)1;
			float rot = Projectile.rotation - ((float)Math.PI / 4f * Projectile.spriteDirection);
			Main.EntitySpriteDraw(tex, pos, sourceRectangle2, color, rot, origin, Projectile.scale, spriteEffects, 0);
			
			return false;
        }
    }
}