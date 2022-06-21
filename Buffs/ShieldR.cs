using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class ShieldR : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Physical Intervention Shield");
			Description.SetDefault("Protects the user against contact damage");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).RedShield = true;
        }
    }
}