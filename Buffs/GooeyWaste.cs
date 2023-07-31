using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class GooeyWaste : ModBuff
	{
		public override void SetStaticDefaults()
        {
			// DisplayName.SetDefault("Gooey Waste");
			// Description.SetDefault("Reduced movement speed and Defense");
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed -= 0.1f;
            player.statDefense -= 20;
            if (!Main.rand.NextBool(90))
                return;
            int type = Main.rand.Next(2, 4);
            int d = Dust.NewDust(player.position, player.width, player.height, type);
            Main.dust[d].noGravity = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense -= 20;
            if (!npc.boss)

            if (!Main.rand.NextBool(90))
                return;
            int type = Main.rand.Next(2, 4);
            int d = Dust.NewDust(npc.position, npc.width, npc.height, type);
            Main.dust[d].noGravity = true;
        }
    }
}