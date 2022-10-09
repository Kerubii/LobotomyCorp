﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace LobotomyCorp.Tiles
{
	public class DisciplinaryShell : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Disciplinary Shell");
			AddMapEntry(new Color(0, 0, 0), name);
			DustType = DustID.Blood;
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.DisciplinaryShell>());
		}

        public override bool RightClick(int i, int j)
        {
			int redMistType = ModContent.NPCType<NPCs.RedMist.RedMist>();
			if (!NPC.AnyNPCs(redMistType))
			{
				WorldGen.KillTile(i, j);

				NPC.SpawnBoss(i * 16, (j + 1) * 16, redMistType, Main.myPlayer);
				Gore.NewGore(null, new Vector2(i * 16, j * 16), new Vector2(-1, 0), ModContent.Find<ModGore>("LobotomyCorp/ShellGore").Type);
				Gore.NewGore(null, new Vector2(i * 16, j * 16), new Vector2(1, 0), ModContent.Find<ModGore>("LobotomyCorp/ShellGore2").Type);

				return true;
			}

			int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
			for (int l = x; l < x + 2; l++)
			{
				for (int m = y; m < y + 3; m++)
				{
					if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
					{
						if (Main.tile[l, m].TileFrameY < 54)
						{
							Main.tile[l, m].TileFrameY += 54;
						}
						else
						{
							Main.tile[l, m].TileFrameY -= 54;
						}
					}
				}
			}
			NetMessage.SendTileSquare(-1, x, y + 1, 3);
			return true;
        }

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY < 54)
				return;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			spriteBatch.Draw(
				TextureAssets.Tile[tile.TileType].Value,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
				new Rectangle(tile.TileFrameX + 36, tile.TileFrameY, 16, 16),
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}