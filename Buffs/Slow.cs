using System;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Slow : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Qlipoth Disruption");
			Description.SetDefault("Movement speed is reduced");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed /= 2f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.boss)
            {
                if (npc.noGravity && npc.velocity.Length() > 12f)
                    npc.velocity *= 0.95f;
                else if (Math.Abs(npc.velocity.X) > 12f)
                    npc.velocity.X *= 0.95f;
            }
            else
            {
                if (npc.noGravity)
                    npc.velocity *= 0.95f;
                else
                    npc.velocity.X *= 0.95f;
            }
        }
    }
}