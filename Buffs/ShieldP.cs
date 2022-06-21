using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class ShieldP : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pale Shield");
			Description.SetDefault("Protects the user against damage to the soul, and hits people back I guess");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).PaleShield = true;
        }
    }
}