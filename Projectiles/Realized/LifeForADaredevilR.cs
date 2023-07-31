using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using LobotomyCorp.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.Collections.Generic;

namespace LobotomyCorp.Projectiles.Realized
{
	public class LifeForADaredevilR : ModProjectile
	{
        public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;

			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Player player;
			LobotomyModPlayer modPlayer;
			if (Projectile.owner > -1 && !Main.player[Projectile.owner].dead)
			{
				player = Main.player[Projectile.owner];
				modPlayer = LobotomyModPlayer.ModPlayer(player);
			}
			else
			{
				Projectile.Kill();
				return;
			}

			if (Projectile.localAI[0] > 0) //Sparkle FX
            {
				Projectile.localAI[0]--;
            }

			if (player.channel)//Player Stance
			{
				Projectile.timeLeft = player.itemAnimationMax;
				player.itemAnimation = player.itemAnimationMax + 1;
				player.itemTime = player.itemAnimation;

				if (Projectile.ai[0] > 10)//Counter Ready
				{
					modPlayer.LifeForADareDevilCounterStance = true;
				}
				else
				{
					Projectile.ai[0]++;
					if (Projectile.ai[0] == 10)
					{
						Projectile.localAI[0] = sparkleFX; //Sparkle FX Start
						SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Armor_Down"), player.position);
					}
				}
				return;
			}
			else
			{
				Projectile.ai[0]--;
				if (Projectile.ai[0] <= 2)//Counter Time
				{
					modPlayer.LifeForADareDevilCounterStance = false;
				}
            }

			if (!modPlayer.LifeForADareDevilGiftActive)
            {
				if (player.itemAnimation == player.itemAnimationMax / 2 + 1)
                {
					player.itemAnimation = 0;
					player.itemTime = 0;
					Projectile.Kill();
                }
            }

			Projectile.Center = player.Center;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? CanDamage()
        {
			return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			overPlayers.Add(index);

            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Player player = Main.player[Projectile.owner];

			Texture2D sword = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D sheath = ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/Realized/LifeForADaredevilRSheath").Value;

			Vector2 pos = player.Center - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
			Rectangle Frame = sword.Frame();
			Vector2 originPoint = new Vector2( 0, Frame.Height);
			float Rotation = 0;
			SpriteEffects flip = 0;
			int dir = player.direction;

			int animationHalf = player.itemAnimationMax / 2;
			if (dir == -1)
            {
				Rotation = MathHelper.ToRadians(180);
				originPoint.X = Frame.Width;
				flip = SpriteEffects.FlipHorizontally;
            }

			if (player.channel)
            {
				Rotation += MathHelper.ToRadians(90) * dir + rotOffset(player.direction);
				Vector2 sheathPos = pos;
				sheathPos.Y -= 32;
				sheathPos.X += 10 * dir;

				if (Projectile.ai[0] > 4)
                {
					Vector2 swordPos = sheathPos;
					swordPos.Y -= 0.33f * (Projectile.ai[0] - 4);

					Main.EntitySpriteDraw(sword, swordPos, Frame, lightColor, Rotation, originPoint, 1f, flip, 0);
				}
				Main.EntitySpriteDraw(sheath, sheathPos, Frame, lightColor, Rotation, originPoint, 1f, flip, 0);
			}
			else
            {
				Vector2 swordPos = pos;
				swordPos.X += 30f * dir;
				swordPos.Y -= 14;
				float sheathRot = rotOffset(dir) + MathHelper.ToRadians(150) * dir;
				if (dir == -1)
					sheathRot += 3.14f;
				Main.EntitySpriteDraw(sheath, swordPos, Frame, lightColor, sheathRot, originPoint, 1f, flip, 0);

				if (player.itemAnimation < animationHalf)
				{
					float progress = 1f - player.itemAnimation / (player.itemAnimationMax / 2f);
					Rotation += MathHelper.ToRadians(120 - 240 * ((float)Math.Pow(0.9f, 60 * progress))) * dir + rotOffset(dir);

					//float progress = player.itemAnimation / (player.itemAnimationMax / 2f);

					//Rotation += MathHelper.ToRadians(120 - 240 * (float)Math.Sin(progress * 1.57f)) + rotOffset(player.direction);
				}
				else
                {
					dir *= -1;

					if (dir == -1)
					{
						Rotation = MathHelper.ToRadians(180);
						originPoint.X = Frame.Width;
						flip = SpriteEffects.FlipHorizontally;
					}
					else
                    {
						Rotation = 0;
						originPoint.X = 0;
						flip = 0;
					}

					float progress = 1f - ((player.itemAnimation / (player.itemAnimationMax / 2f)) - 1f);
					//int prog = player.itemAnimationMax - player.itemAnimation - 1;
					Rotation += MathHelper.ToRadians(180 - 30 + 150 * (float)Math.Sin(progress * 1.57f)) * dir + rotOffset(dir);
				}

				swordPos = pos;
				Main.EntitySpriteDraw(sword, swordPos, Frame, lightColor, Rotation, originPoint, 1f, flip, 0);

			}

			if (Projectile.localAI[0] > 0)
            {
				Texture2D shine = ModContent.Request<Texture2D>("LobotomyCorp/Projectiles/Realized/sparkle").Value;

				pos.X += 10 * player.direction;
				Frame = shine.Frame();
				originPoint = Frame.Size() / 2;

				float scale = 2f - 1.5f * (Projectile.localAI[0] / sparkleFX);
				Color color = Color.White * (Projectile.localAI[0] / sparkleFX);

				Main.EntitySpriteDraw(shine, pos, Frame, color, 0f, originPoint, scale, 0, 0);
			}
			return false;//base.PreDraw(ref lightColor);
		}

		private const float sparkleFX = 30;

		private float rotOffset(int direction)
		{
			if (direction == 1)
				return 1.0472f;
			return 1.0472f * 2;
		}
	}
}
