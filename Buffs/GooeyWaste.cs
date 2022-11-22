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
			DisplayName.SetDefault("Gooey Waste");
			Description.SetDefault("Reduced movement speed and Defense");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed -= 0.1f;
            player.statDefense -= 20;
            int type = Main.rand.Next(2, 4);
            Dust.NewDust(player.Center, player.width, player.height, type);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense -= 20;
            int type = Main.rand.Next(2, 4);
            Dust.NewDust(npc.Center, npc.width, npc.height, type);
        }
    }
}