using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class ShieldB : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Erosion Shield");
			Description.SetDefault("Protects the user against projectile and contact damage");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).BlackShield = true;
        }
    }
}