using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Scream : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Horrid Screech");
			// Description.SetDefault("Wings disabled, 8% decreased movement speed");
            Main.debuff[Type] = true;
            //Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.wingTime = 0;
            player.moveSpeed -= 0.08f;

            if (Main.rand.Next(4) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(player.position, player.width, player.head, DustID.Wraith)];
                d.noGravity = true;
            }
        }
    }
}