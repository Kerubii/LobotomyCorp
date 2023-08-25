using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace LobotomyCorp.Tiles
{
    public class QlipothDeterence : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			//TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TEQlipothDeterence>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Qlipoth Detterence Field");
			AddMapEntry(new Color(190, 230, 190), name);
			DustType = 11;
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

        public override bool RightClick(int i, int j)
        {
			HitWire(i, j);
			return true;
		}

        public override void HitWire(int i, int j)
        {
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
			if (Wiring.running)
			{
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x, y + 2);
				Wiring.SkipWire(x + 1, y);
				Wiring.SkipWire(x + 1, y + 1);
				Wiring.SkipWire(x + 1, y + 2);
			}
			NetMessage.SendTileSquare(-1, x, y + 1, 3);
		}

        public override bool AutoSelect(int i, int j, Item item)
        {
            return base.AutoSelect(i, j, item);
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
	/*
	public class TEQlipothDeterence : ModTileEntity
	{
		private const int range = 320;

		public override void Update()
		{
			if (Main.tile[Position.X, Position.Y].TileFrameY >= 56)
				for (int i = 0; i < 3; i++)
				{
					Vector2 dustPos = new Vector2(Position.X * 16, Position.Y * 16) + new Vector2(range, 0).RotateRandom(6.28f);
					Dust d = Dust.NewDustPerfect(dustPos, DustID.BlueTorch);
					d.noGravity = true;
					d.fadeIn = 1.4f;
				}
			//npc thing;
		}

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == ModContent.TileType<QlipothDeterence>() && tile.TileFrameX == 0 && (tile.TileFrameY == 0 || tile.TileFrameY == 56);
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			// i - 1 and j - 2 come from the fact that the origin of the tile is "new Point16(1, 2);", so we need to pass the coordinates back to the top left tile. If using a vanilla TileObjectData.Style, make sure you know the origin value.
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3); // this is -1, -1, however, because -1, -1 places the 3 diameter square over all the tiles, which are sent to other clients as an update.
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - 1, j - 2, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i - 1, j - 2);
		}
	}*/
}