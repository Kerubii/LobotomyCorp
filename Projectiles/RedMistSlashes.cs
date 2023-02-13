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
            DisplayName.SetDefault("Twilight");
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
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = Main.rand.NextFloat(6.28f);
                Projectile.spriteDirection = Main.rand.Next(2) * 2 - 1;
                Projectile.scale = 0.1f;
                Projectile.ai[1] = MathHelper.ToRadians(Main.rand.Next(12, 18));

                Projectile.ai[0]++;
            }
            Projectile.rotation += Projectile.ai[1] * Projectile.spriteDirection;
            Projectile.scale += 0.035f;
            
            if (Projectile.timeLeft <= 10)
                Projectile.alpha -= 25;
        }

        public override bool? CanDamage()
        {
            if (Projectile.timeLeft >= 10 && Projectile.timeLeft < 15)
                return base.CanDamage();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Rectangle frame = tex.Frame();
            Vector2 origin = frame.Size() / 2;
            Color color = lightColor * (float)(Projectile.alpha/255f);

            Main.EntitySpriteDraw(tex, pos, frame, color, Projectile.rotation, origin, Projectile.scale, 0f, 0);
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
                if (Main.myPlayer == Projectile.owner)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), SlashPosition, Vector2.Zero, ModContent.ProjectileType<Projectiles.RedMistSlashes>(), Projectile.damage, 0);
            }
        }

        public override bool? CanDamage()
        {
            return false;
        }
    }
}
