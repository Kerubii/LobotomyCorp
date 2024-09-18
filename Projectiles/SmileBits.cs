using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class SmileBits : ModProjectile
	{
		public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Smile");
            Main.projFrames[Projectile.type] = 3;
        }

		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
            Projectile.timeLeft = 120;
            Projectile.scale = 0.5f;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
		}

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.frame = Main.rand.Next(3);
            }
            Projectile.rotation += 0.01f;

            //Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith);
                Main.dust[d].noGravity = true;
            }

            if (Projectile.timeLeft < 10)
                Projectile.scale -= 0.05f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Vomit>(), 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Rectangle frame = new Rectangle(0, 0, tex.Width, tex.Height / 3);
            frame.Y = frame.Height * Projectile.frame;
            Vector2 origin = frame.Size() / 2;

            Main.EntitySpriteDraw(tex, pos, frame, lightColor, Projectile.rotation, origin, Projectile.scale, 0f, 0);
            return false;
        }
    }

    public class SmileBitsFriendly : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/SmileBits";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Smile");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.scale = 0.5f;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.frame = Main.rand.Next(3);
            }
            Projectile.rotation += 0.01f;

            //Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith);
                Main.dust[d].noGravity = true;
            }

            if (Projectile.timeLeft < 10)
                Projectile.scale -= 0.05f;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)Projectile.ai[0])
                return false;

            return base.CanHitNPC(target);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Rectangle frame = new Rectangle(0, 0, tex.Width, tex.Height / 3);
            frame.Y = frame.Height * Projectile.frame;
            Vector2 origin = frame.Size() / 2;

            Main.EntitySpriteDraw(tex, pos, frame, lightColor, Projectile.rotation, origin, Projectile.scale, 0f, 0);
            return false;
        }
    }
}
