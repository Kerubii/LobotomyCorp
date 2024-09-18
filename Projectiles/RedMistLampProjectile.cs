using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class RedMistLampProjectile : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.height = 8;
			Projectile.width = 8;
			Projectile.aiStyle = -1;

			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.hostile = true;

			Projectile.alpha = 255;
		}

        private int Target => (int)Projectile.ai[0];

        public override void AI()
		{
			Projectile.ai[1]++;
			if (Projectile.ai[1] < 20)
			{
                if (Projectile.ai[1] == 1)
                {
                    float anglevar = Main.rand.NextFloat(1.57f);
                    for (int i = 0; i < 16; i++)
                    {
                        float angle = (float)Math.PI * 2f * (i / 16f) + anglevar;

                        Dust dd = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, new Vector2(1.2f, 0).RotatedBy(angle));
                        dd.noGravity = true;
                    }
                }

                Projectile.velocity *= 0.98f;
                int type = DustID.GemTopaz;
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type);
                Main.dust[d].noGravity = true;
				Projectile.alpha -= 20;
				if (Projectile.alpha < 0)
					Projectile.alpha = 0;

                Projectile.ai[0] = Player.FindClosest(Projectile.position, Projectile.width, Projectile.height);
            }
			else
			{
				if (Projectile.ai[1] == 20)
				{
					for (int i = 0; i < 4; i++)
					{
						float angle = 1.57f * i;
						Vector2 vel = new Vector2(4, 0).RotatedBy(angle);
						Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, vel * 1.2f);
						d.noGravity = true;
						d = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, vel);
						d.noGravity = true;
						d = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, vel * 0.5f);
						d.noGravity = true;
					}

					//Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 16f;
				}

                if (Target >= 0)
                {
                    Player p = Main.player[Target];
                    if (p.dead)
                        Projectile.ai[0] = -1;
                    else
                    {
                        Vector2 targetPos = p.MountedCenter;

                        float speed = 0.2f;
                        float maxSpeed = 14f;

                        Vector2 pos = targetPos - Projectile.Center;
                        Vector2 NormPos = pos;
                        NormPos.Normalize();

                        if (Projectile.velocity.X < NormPos.X * maxSpeed)
                        {
                            Projectile.velocity.X += speed;
                            if (Projectile.velocity.X > maxSpeed)
                                Projectile.velocity.X = maxSpeed;
                        }
                        else if (Projectile.velocity.X > NormPos.X * maxSpeed)
                        {
                            Projectile.velocity.X -= speed;
                            if (Projectile.velocity.X > 0)
                                Projectile.velocity.X -= speed * 4;
                            if (Projectile.velocity.X < -maxSpeed)
                                Projectile.velocity.X = -maxSpeed;
                        }

                        if (Projectile.velocity.Y < NormPos.Y * maxSpeed)
                        {
                            Projectile.velocity.Y += speed;
                            if (Projectile.velocity.Y < 0)
                                Projectile.velocity.Y += speed * 4;
                            if (Projectile.velocity.Y > maxSpeed)
                                Projectile.velocity.Y = maxSpeed;
                        }
                        else if (Projectile.velocity.Y > NormPos.Y * maxSpeed)
                        {
                            Projectile.velocity.Y -= speed;
                            if (Projectile.velocity.Y > 0)
                                Projectile.velocity.Y -= speed * 4;
                            if (Projectile.velocity.Y < -maxSpeed)
                                Projectile.velocity.Y = -maxSpeed;
                        }

                        if (Projectile.velocity.Length() * 10 > pos.Length())
                            Projectile.ai[0] = -1;
                    }
                }
                else if (Projectile.timeLeft > 10)
                {
                    Projectile.timeLeft = 10;
                }

                for (int i = 0; i < 2; i++)
                {
                    int type = DustID.GemTopaz;
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type);
                    Main.dust[d].velocity *= 0;
                    Main.dust[d].noGravity = true;
                }
            }

			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
		}

        public override void OnKill(int timeLeft)
        {
            float anglevar = Main.rand.NextFloat(1.57f);
            for (int i = 0; i < 16; i++)
            {
                float angle = (float)Math.PI * 2f * (i / 16f) + anglevar;

                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, new Vector2(4, 0).RotatedBy(angle));
                d.noGravity = true;
            }

            SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Entity/ApocalypseBird/BossBird_laser_Boom") with { Volume = 0.2f, PitchVariance = 0.2f}, Projectile.position);
        }

        public override bool CanHitPlayer(Player target)
        {
            if (Projectile.ai[1] < 60)
                return false;

            return base.CanHitPlayer(target);
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = tex.Frame();
			lightColor = Color.White;
			lightColor.A = 100;
			lightColor *= 1f - (Projectile.alpha / 255f);
			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY, frame, lightColor, Projectile.rotation, frame.Size() / 2, Projectile.scale, 0, 0);

            return false;
        }
    }
}