using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class ShieldCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Breached Shield");
			// Description.SetDefault("Shield recharging");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).CooldownShield = true;
        }
    }
}