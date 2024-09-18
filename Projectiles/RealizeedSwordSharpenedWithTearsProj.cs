using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Utils;
using Terraria.Graphics.Shaders;
using Terraria.GameContent;

namespace LobotomyCorp.Projectiles
{
	public class RealizedSwordSharpenedWithTearsProj : ModProjectile
	{
		public override void SetStaticDefaults() {
            //DisplayName.SetDefault("Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.alpha = 0;
            Projectile.timeLeft = 16;

			//Projectile.hide = true;
			//Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
		}

		private float state
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public float order
        {
            get => Projectile.ai[0];
        }

        private bool playerAttack(bool rightClick = false)
        {
            Player player = Main.player[Projectile.owner];
            return player.itemAnimation == player.itemAnimationMax - 1 && (rightClick ? player.altFunctionUse == 2: true);
        }

		public override void AI() {
            if (state < 2)
            {
                state++;
            }
            if (state == 2)
            {
                Player player = Main.player[Projectile.owner];

                Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation() + MathHelper.ToRadians(135);

                Vector2 Pos = player.Center + new Vector2(24, 0).RotatedBy((Main.MouseWorld - Projectile.Center).ToRotation());
                Pos.Y -= 76;
                Pos.X += 64 * order;
                if (order != 0)
                    Pos.Y += 16;

                float speed = 4f;
                Vector2 delta = Pos - Projectile.Center;
                float mag = 1f;
                if (delta.Length() > 0f)
                    mag = speed / delta.Length();
                if (delta.Length() > speed)
                    delta *= mag;

                Projectile.velocity = delta;
                Projectile.timeLeft = 300;

                if (player.HeldItem.type != ModContent.ItemType<Items.Ruina.Natural.SwordSharpenedWithTearsR>())
                    Projectile.Kill();

                Projectile.localAI[0]++;
                if (Projectile.localAI[0] > 360)
                    Projectile.localAI[0] = 0;
                Projectile.gfxOffY = 8 * (float)Math.Sin(MathHelper.ToRadians(Projectile.localAI[0]));

                if (playerAttack(true))
                {
                    Projectile.velocity = new Vector2(16, 0).RotatedBy(Projectile.rotation - MathHelper.ToRadians(135));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, order);
                    Projectile.tileCollide = true;
                    state++;
                }
                else if (playerAttack())
                {
                    if (LobotomyModPlayer.ModPlayer(player).RealizedSword != order || LobotomyModPlayer.ModPlayer(player).RealizedSwordShoot)
                        return;
                    LobotomyModPlayer.ModPlayer(player).RealizedSword++;
                    if (LobotomyModPlayer.ModPlayer(player).RealizedSword > 1)
                        LobotomyModPlayer.ModPlayer(player).RealizedSword = -1;
                    LobotomyModPlayer.ModPlayer(player).RealizedSwordShoot = true;
                    Projectile.velocity = new Vector2(16, 0).RotatedBy(Projectile.rotation - MathHelper.ToRadians(135));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, order);
                    Projectile.tileCollide = true;
                    state++;
                }
            }
            else if (state == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 15);
                    Main.dust[d].fadeIn = 0.15f;
                    Main.dust[d].noGravity = false;
                }
                if (ProjectileColliding() || Projectile.timeLeft == 1)
                {
                    Projectile.velocity *= 0;
                    Projectile.timeLeft = 300;
                    Projectile.tileCollide = false;
                    state = 5;
                }
            }
            else if (state == 4)
            {
                for (int i = 0; i < 3; i++)
                {
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 15);
                    Main.dust[d].fadeIn = 0.15f;
                    Main.dust[d].noGravity = false;
                }
                Projectile.tileCollide = false;
                if (Projectile.timeLeft < 10)
                    Projectile.alpha += 25;
            }
            else if (state >= 5)
            {
                Player player = Main.player[Projectile.owner];
                float rotation = state - 5;
                if (state == 5)
                    state = 5 + (float)MathHelper.ToRadians(Main.rand.Next(1, 361));
                if (Projectile.timeLeft > 260)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int d = Dust.NewDust(player.Center + new Vector2(320, 0).RotatedBy(rotation + Math.PI), Projectile.width, Projectile.height, 15);
                        Main.dust[d].fadeIn = 0.15f;
                        Main.dust[d].noGravity = false;
                    }
                    //Projectile.rotation += MathHelper.ToRadians(10 * (Projectile.timeLeft % 2 == 0 ? 1 : -1));
                    if (Projectile.timeLeft < 270)
                        Projectile.alpha -= 25;
                }
                else
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DespairSword>(), Projectile.damage, Projectile.knockBack, Projectile.owner, rotation);
                    Projectile.Kill();
                }
            }
		}

        private bool ProjectileColliding()
        {
            bool collide = false;
            Vector2 targetPos = Projectile.position;
            for (int i = (int)targetPos.X; i < targetPos.X + Projectile.width; i += 16)
            {
                for (int j = (int)targetPos.Y; j < targetPos.Y + Projectile.width; j += 16)
                {
                    if (Main.tile[i/16, j/16].HasTile && Main.tileSolid[Main.tile[i/16, j/16].TileType] && !Main.tileSolidTop[Main.tile[i / 16, j / 16].TileType])
                        collide = true;
                }
            }
            return collide;
        }

        public override bool? CanDamage()
        {
            return state == 3 || state == 4;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitDissapear();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitDissapear();
        }

        private void OnHitDissapear()
        {
            state = 4;
            if (Projectile.timeLeft > 30)
                Projectile.timeLeft = 15;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            float alpha = ((float)(255f - (float)Projectile.alpha) / 255f);
            if (state > 2)
                for (int i = 0; i < 4; i++)
                {
                    //Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/SwordSharpenedWithTearsGlow").Value;
                    position = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;

                    Color color = lightColor * ((float)(255 - Projectile.alpha) / 255f) * 0.5f;
                    color *= (4f - i) / 4f;

                    Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Microsoft.Xna.Framework.Rectangle?
                            (
                                new Rectangle
                                (
                                    0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                )
                            ),
                    color * alpha, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);

                    /*
                    Main.EntitySpriteDraw(texture, position, new Microsoft.Xna.Framework.Rectangle?
                                        (
                                            new Rectangle
                                            (
                                                0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                            )
                                        ),
                    color, Projectile.rotation, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0);*/
                }
            position = Projectile.Center - Main.screenPosition;
            position.Y += Projectile.gfxOffY;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Microsoft.Xna.Framework.Rectangle?
                                    (
                                        new Rectangle
                                        (
                                            0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                        )
                                    ),
                lightColor * alpha, Projectile.rotation, Projectile.Size/2, Projectile.scale, SpriteEffects.None, 0);

            /*spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);*/

            //GameShaders.Misc["TrailingShader"].Apply();

            //SlashTrail trail = new SlashTrail();
            //trail.DrawTrail(Projectile, LobotomyCorp.LobcorpShaders["TwilightSlash"]);

            return false;
        }
    }

    public class DespairSword : ModProjectile
    {
        public override string Texture => "LobotomyCorp/Projectiles/RealizedSwordSharpenedWithTearsProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 255;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            if (Projectile.ai[1] < 60)
            {
                Player player = Main.player[Projectile.owner];
                Projectile.ai[1]++;
                Projectile.Center = player.Center + new Vector2(320, 0).RotatedBy(Projectile.ai[0] + Math.PI);
                Projectile.rotation = Projectile.ai[0] + MathHelper.ToRadians(135);
                if (Projectile.alpha > 0)
                    Projectile.alpha -= 25;
            }
            else if (Projectile.ai[1] == 60)
            {
                Projectile.ai[1]++;
                Projectile.velocity = new Vector2(10, 0).RotatedBy(Projectile.ai[0]);
            }
            else
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 15);
                Main.dust[d].fadeIn = 0.15f;
                Main.dust[d].noGravity = false;
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            return target.whoAmI == Projectile.owner;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            float alpha = ((float)(255f - (float)Projectile.alpha) / 255f);
            for (int i = 0; i < 4; i++)
            {
                //Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/SwordSharpenedWithTearsGlow").Value;
                position = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;

                Color color = lightColor * ((float)(255 - Projectile.alpha) / 255f) * 0.5f;
                color *= (4f - i) / 4f;

                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Microsoft.Xna.Framework.Rectangle?
                        (
                            new Rectangle
                            (
                                0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                            )
                        ),
                color * alpha, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);

                /*
                Main.EntitySpriteDraw(texture, position, new Microsoft.Xna.Framework.Rectangle?
                                    (
                                        new Rectangle
                                        (
                                            0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                        )
                                    ),
                color, Projectile.rotation, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0);*/
            }
            position = Projectile.Center - Main.screenPosition;
            position.Y += Projectile.gfxOffY;
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, new Microsoft.Xna.Framework.Rectangle?
                                    (
                                        new Rectangle
                                        (
                                            0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()
                                        )
                                    ),
                lightColor * alpha, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
