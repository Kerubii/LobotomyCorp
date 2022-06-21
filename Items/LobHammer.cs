using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace LobotomyCorp.Items
{
	public abstract class LobCorpHeavy : ModItem
	{
		public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
			if (player.altFunctionUse == 2)
				return;

			/* Old 1/3
			//float rotation = ((float)player.itemAnimation / (float)player.itemAnimationMax - 0.5f) * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;
			int restFrame = (int)(player.itemAnimationMax * 0.1f);
			if (player.itemAnimation >= player.itemAnimationMax + restFrame * 2)
			{
				float itemAnim = player.itemAnimation - player.itemAnimationMax - restFrame * 2;
				float itemAnimMax = player.itemAnimationMax * 2 - restFrame * 2;
				player.itemRotation = (1f - (float)itemAnim / (float)itemAnimMax - 0.5f) * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;

				//Main.NewText(itemAnim);
			}
			else if (player.itemAnimation >= player.itemAnimationMax + restFrame)
            {
				player.itemRotation = 0.5f * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;
			}
			else if (player.itemAnimation > restFrame)
			{
				player.itemRotation = ((float)Math.Sin(((float)player.itemAnimation - restFrame - 1) / ((float)player.itemAnimationMax - restFrame) * 1.57f) - 0.5f) * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;
				//Main.NewText(player.itemAnimation - restFrame);
			}
			else
				player.itemRotation = -0.5f * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;

			if (player.itemTime == 0)
				player.itemRotation = -0.5f * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;
			*/
			Item.useStyle = -1;
			Item.noUseGraphic = false;
			Item.noMelee = false;

			int restFrame = (int)(player.itemAnimationMax * 0.25f);
			//Main.NewText(restFrame);
			float percent = -0;
			if (player.itemAnimation > player.itemAnimationMax * 0.5f)
			{
				float min = player.itemAnimation - player.itemAnimationMax * 0.5f;
				float max = player.itemAnimationMax * 1.5f - restFrame;
				percent = 1f - min / max - 0.5f;
			}
			else if (player.itemAnimation > restFrame)
			{
				float min = player.itemAnimation - restFrame;
				float max = player.itemAnimationMax * 0.5f;
				percent = (float)Math.Sin((min / max) * 1.57f) - 0.5f;
				//Main.NewText("Sin: " + (float)Math.Sin((min / max) * 1.57f) + ", Perc: " + (min/max));
			}
			else
				percent = -0.5f;

			if (player.itemTime == 0)
				percent = -0.5f;
			
			player.itemRotation = percent * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;

			float rotation = player.itemRotation - 0.785f;
			player.itemLocation.X = player.position.X + 4 * player.direction;
			if (player.direction == -1)
				player.itemLocation.X += player.width - 4;
			player.itemLocation.Y = player.position.Y + player.height / 2 - 4f;

			player.itemLocation += (new Vector2((player.width * 0.6f) * player.direction, 0)).RotatedBy(rotation);

			if (player.itemAnimation < player.itemAnimationMax * 0.15f)
				player.bodyFrame.Y = player.bodyFrame.Height * 3;
			else if (player.itemAnimation < player.itemAnimationMax * 0.33f)
				player.bodyFrame.Y = player.bodyFrame.Height * 2;
			else if (player.itemAnimation < player.itemAnimationMax * 1f)
				player.bodyFrame.Y = player.bodyFrame.Height;
			else if (player.itemAnimation < player.itemAnimationMax * 1.5f)
				player.bodyFrame.Y = player.bodyFrame.Height * 2;
			else
				player.bodyFrame.Y = player.bodyFrame.Height * 3;

			rotation = rotation - (float)Math.PI / 2f * (float)player.direction;
			player.SetCompositeArmFront(enabled: true, CompositeArmStretchAmount.Full, rotation);
		}

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			if (player.itemAnimation > player.itemAnimationMax * 0.5f - 2 || player.ItemTimeIsZero || Item.noMelee)
			{
				noHitbox = true;
				return;
			}

			float adjustedItemScale = player.GetAdjustedItemScale(Item);
			hitbox.Width = (int)(hitbox.Width * adjustedItemScale);
			hitbox.Height = (int)(hitbox.Height * adjustedItemScale);

			if (player.itemAnimation < player.itemAnimationMax * 0.18)
			{
				if (player.direction == -1)
				{
					hitbox.X -= (int)(hitbox.Width * 1.4 - hitbox.Width);
				}
				hitbox.Width = (int)(hitbox.Width * 1.4);
				hitbox.Y += (int)(hitbox.Height * 0.5 * player.gravDir);
				hitbox.Height = (int)(hitbox.Height * 1.1);
			}
			else if (!(player.itemAnimation < player.itemAnimationMax * 0.33))
			{
				if (player.direction == 1)
				{
					hitbox.X -= (int)(hitbox.Width * 1.2);
				}
				hitbox.Width *= 2;
				hitbox.Y -= (int)((hitbox.Height * 1.4 - hitbox.Height) * player.gravDir);
				hitbox.Height = (int)(hitbox.Height * 1.4);
			}
		}

        public sealed override bool? UseItem(Player player)
        {
			if (player.altFunctionUse == 2)
				return SafeUseItem(player);

			player.itemTime = player.itemAnimation * 2;
			player.itemAnimation = player.itemTime;
			return false;
		}

		public virtual bool? SafeUseItem(Player player)
        {
			return null;
        }
	}
}