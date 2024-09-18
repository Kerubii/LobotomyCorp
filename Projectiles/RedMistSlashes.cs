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
	public class RedMistSlashes : ModProjectile
	{
		public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Twilight");
        }

		public override void SetDefaults() {
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.aiStyle = -1;
            Projectile.timeLeft = 20;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
		}

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[1] = Main.rand.NextFloat(6.28f);

                Projectile.scale = Main.rand.NextFloat(0.6f, 1.2f);
                Projectile.rotation = Projectile.localAI[1] - MathHelper.ToRadians(270f) * (Main.rand.NextBool(2) ? -1 : 1);
            }
            else
                Projectile.rotation = Projectile.rotation + (Projectile.localAI[1] - Projectile.rotation) * 0.2f;

            Projectile.localAI[0]++;

            if (Projectile.timeLeft <= 5)
                Projectile.alpha -= 50;
        }

        public override bool? CanDamage()
        {
            if (Projectile.timeLeft >= 10 && Projectile.timeLeft < 15)
                return base.CanDamage();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectiles.TwilightSlashes.TwilightSlashTex;//TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Rectangle frame = tex.Frame();
            Vector2 origin = frame.Size() / 2;
            Color color = Color.DarkRed * (float)(Projectile.alpha / 255f);

            Main.EntitySpriteDraw(tex, pos, frame, color, Projectile.rotation, origin, Projectile.scale, 0f, 0);
            return false;
        }
    }

    public class RedMistStrikes : ModProjectile
    {

        public override string Texture => "LobotomyCorp/Projectiles/TwilightStrikes";

        public override void SetDefaults()
        {
            Projectile.height = 12;
            Projectile.width = 12;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.hostile = true;
            Projectile.alpha = 255;

            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            Projectile.localAI[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.timeLeft < 5)
                Projectile.alpha -= 51;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = SpearExtender.SpearTrail;
            Vector2 pos = Projectile.Center - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
            Rectangle frame = tex.Frame();
            float length = Projectile.velocity.Length() * Projectile.localAI[0];
            Vector2 origin = new Vector2(287, 53);
            float nana = 287f;
            Vector2 scale = new Vector2(length / nana, 0.2f);
            Color color = Color.DarkRed * (Projectile.alpha / 255f);
            color.A = (byte)(color.A * 0.7f);
            color *= 0.9f;
            Main.EntitySpriteDraw(tex, pos, frame, color, Projectile.rotation, origin, scale, 0, 0);
            return false;
        }
    }

    public class RedMistSlashSpawner : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/RedMistSlashes";


        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 2 == 0)
            {
                Vector2 SlashPosition = Projectile.Center + new Vector2(Main.rand.Next(-16, 17), Main.rand.Next(-16, 17));

                float angle = Main.rand.NextFloat(6.28f);
                Vector2 velocity = new Vector2(16f, 0f).RotatedBy(angle) * Main.rand.NextFloat(0.5f, 1f);
                Vector2 position = -velocity * 15;
                int offsetX = 5;
                int offsetY = 5;

                position += SlashPosition;

                position.X += Main.rand.Next(-offsetX, offsetX);
                position.Y += Main.rand.Next(-offsetY, offsetY);
                int type = ModContent.ProjectileType<Projectiles.RedMistStrikes>();
                if (Main.rand.NextBool(5))
                {
                    position += velocity * 15;
                    velocity *= 0;
                    type = ModContent.ProjectileType<Projectiles.RedMistSlashes>();
                }
                if (Main.myPlayer == Projectile.owner)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, type, Projectile.damage, 0);
            }
        }

        public override bool? CanDamage()
        {
            return false;
        }
    }
}
