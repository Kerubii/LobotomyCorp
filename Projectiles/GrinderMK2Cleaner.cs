using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Utils;
using Terraria.Audio;
using Terraria.GameContent;
/*
namespace LobotomyCorp.Projectiles
{
    public class GrinderMk2Cleaner : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cleaning Tools");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 1000;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public float order
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public Vector2 ProjectileRoot
        {
            get => Projectile.Center + ProjectileRootOffset;
        }

        public Vector2 ProjectileRootOffset
        {
            get => new Vector2(-2 * Projectile.direction, 14).RotatedBy(Projectile.rotation);
        }

        public Vector2 ProjectileElbow(Player player)
        {
            Vector2 elbow = player.RotatedRelativePoint(player.MountedCenter, true);
            int legLength = 34;
            float Length = Vector2.Distance(ProjectileRoot, elbow);
            if (Length > legLength * 2)
                Length = legLength * 2;
            float Angle = (float)Math.Acos(Length * Length / (2 * legLength * Length));
            int dir = -1;
            if (order < 3 && order > 0)
                dir = 1;
            float Rotation = (ProjectileRoot - elbow).ToRotation() + Angle * dir;
            elbow = elbow + new Vector2(legLength, 0).RotatedBy(Rotation);
            return elbow;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.direction = player.direction;

            Projectile.rotation = (Projectile.Center - ownerMountedCenter).ToRotation() + 1.57f;

            int dirX = order % 2 == 0 ? -1 : 1;
            int dirY = order < 2 ? -1 : 1;

            Vector2 targetPos = ownerMountedCenter + new Vector2(32 * dirX, 42 * dirY);
            float speed = 6f;
            if (player.HeldItem.type == ModContent.ItemType<Items.Ruina.Technology.GrinderMk2S>())
            {
                if (player.channel && player.altFunctionUse != 2)
                {
                    Projectile.ai[1]++;
                    Vector2 offset = new Vector2(70 * dirX, 70 * dirY).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 14 * player.direction));
                    targetPos = ownerMountedCenter + offset;
                    speed = 48;
                    LobotomyModPlayer.ModPlayer(player).GrinderMk2Battery--;
                    player.immune = true;
                    player.immuneNoBlink = true;
                    player.immuneTime = 5;

                    if (order == 0)
                    {
                        if (Math.Abs(player.velocity.X) < 14f)
                            player.velocity.X = player.direction * 14f;

                        LiftPlayer(player, 100, -8f);
                    }
                }
                else
                {
                    Projectile.ai[1] = 0;
                }

                if ((LobotomyModPlayer.ModPlayer(player).GrinderMk2Order == order || LobotomyModPlayer.ModPlayer(player).GrinderMk2Order == 4) && player.altFunctionUse == 2 && player.itemAnimation > player.itemAnimationMax / 2)
                {
                    LobotomyModPlayer.ModPlayer(player).GrinderMk2Battery--;
                    targetPos = Main.MouseWorld;
                    if (Vector2.Distance(targetPos, ownerMountedCenter) > 68)
                    {
                        targetPos = Main.MouseWorld - ownerMountedCenter;
                        targetPos.Normalize();
                        targetPos *= 68;
                        targetPos += ownerMountedCenter;
                    }
                    speed = 16f;
                }
            }

            Vector2 delta = targetPos - ProjectileRoot;
            if (delta.Length() > speed)
            {
                Projectile.velocity = new Vector2(speed, 0).RotatedBy(delta.ToRotation());
            }
            else
                Projectile.velocity = delta;
            Projectile.Center += Projectile.velocity;
            if (speed == 16)
                Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
            delta = ProjectileRoot - ownerMountedCenter;
            if (delta.Length() > 68)
            {
                Projectile.Center = ownerMountedCenter + new Vector2(68, 0).RotatedBy(delta.ToRotation()) - ProjectileRootOffset;
            }
            Projectile.timeLeft = 300;
            LobotomyModPlayer.ModPlayer(player).GrinderMk2Battery--;
            if (LobotomyModPlayer.ModPlayer(player).GrinderMk2Battery <= 0 || LobotomyModPlayer.ModPlayer(player).GrinderMk2Recharging)
            {
                Projectile.Kill();
            }

        }

        private void LiftPlayer(Player player, int distance, float speed = -2f)
        {
            Vector2 position = player.position;
            position.Y += player.height;
            if (Collision.SolidTiles(position, player.width, distance, true))
            {
                player.velocity.Y = -2f;

                if (Collision.SolidTiles(position, player.width, distance + 2, true))
                {
                    player.velocity.Y -= player.gravity;
                }
                //Main.NewText("aeiou");
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.player[Projectile.owner].AddBuff(ModContent.BuffType<Buffs.GrinderMk2Recharge>(), 180);
            if (LobotomyModPlayer.ModPlayer(Main.player[Projectile.owner]).GrinderMk2Recharging == false)
            {
                LobotomyModPlayer.ModPlayer(Main.player[Projectile.owner]).GrinderMk2Recharging = true;
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Helper_Down") with {Volume = 0.25f}, Projectile.Center);
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            
            //7/3, 41
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/GrinderMk2Arm2").Value;
            Vector2 pos = ownerMountedCenter - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width/2, 5);
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);
            float rotation = (ProjectileElbow(player) - ownerMountedCenter).ToRotation() - 1.57f;
            Main.EntitySpriteDraw(texture, pos, new Rectangle?(frame), lightColor, rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            
            pos = ProjectileElbow(player) - Main.screenPosition;
            rotation = (ProjectileRoot - ProjectileElbow(player)).ToRotation() - 1.57f;
            Main.EntitySpriteDraw(texture, pos, new Rectangle?(frame), lightColor, rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            texture = TextureAssets.Projectile[Projectile.type].Value;
            if (order < 3 && order > 0)
                texture = Mod.Assets.Request<Texture2D>("Projectiles/GrinderMk2Cleaner2").Value;

            pos = Projectile.Center - Main.screenPosition;
            origin = texture.Size() / 2;
            frame = new Rectangle(0, 0, texture.Width, texture.Height);
            Main.EntitySpriteDraw(texture, pos, new Rectangle?(frame), lightColor, Projectile.rotation, origin, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            if (order > 0)
                return false;

            texture = Mod.Assets.Request<Texture2D>("Projectiles/GrinderMk2Battery").Value;
            ownerMountedCenter.Y -= 48;
            pos = ownerMountedCenter - Main.screenPosition;
            origin = texture.Size() / 2;
            frame = new Rectangle(0, 0, texture.Width, texture.Height);
            lightColor = Lighting.GetColor((int)ownerMountedCenter.X/16, (int)ownerMountedCenter.Y/16);
            Main.EntitySpriteDraw(texture, pos, new Rectangle?(frame), lightColor, 0, origin, Projectile.scale, SpriteEffects.None, 0);
        
            texture = Mod.Assets.Request<Texture2D>("Projectiles/GrinderMk2Bar").Value;
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(player);
            int frameY = (int)((LobotomyModPlayer.GrinderMk2BatteryMax - (float)ModPlayer.GrinderMk2Battery)/((float)LobotomyModPlayer.GrinderMk2BatteryMax/5)) * texture.Height/6;
            if (ModPlayer.GrinderMk2Battery < LobotomyModPlayer.GrinderMk2BatteryMax / 5 && ModPlayer.GrinderMk2Battery % 120 < 60)
                frameY += texture.Height/6;
            frame = new Rectangle(0, frameY, texture.Width, texture.Height/6);
            Main.EntitySpriteDraw(texture, pos, new Rectangle?(frame), Color.White, 0, origin, Projectile.scale, SpriteEffects.None, 0);

            /*TrailingShader Trail = default(TrailingShader);
            Trail.ColorStart = Color.Blue;
            Trail.ColorEnd = Color.Blue;
            Trail.Draw(Projectile);*/
/*
            return false;
        }
    }
}
*/