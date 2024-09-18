using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class ShieldW : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Trauma Shield");
			// Description.SetDefault("Protects the user against projectiles");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).WhiteShield = true;
        }
    }
}