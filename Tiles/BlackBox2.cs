﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace LobotomyCorp.Tiles
{
	public class BlackBox2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Black Box - Container");
			AddMapEntry(new Color(0, 0, 0), name);
			DustType = DustID.Wraith;
			TileID.Sets.DisableSmartCursor[Type] = true;

			AdjTiles = new int[] { ModContent.TileType<BlackBox>() };
		}

		/*public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.BlackBox2>());
		}*/
	}
}