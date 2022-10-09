using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class DarkFlame : ModBuff
	{
		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Dark Flame");
			Description.SetDefault("70% increased Magic Bullet damage");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(player);
            modPlayer.MagicBulletDarkFlame = true;
        }
    }
}