using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class CrookedNotes : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crooked Notes");
			Description.SetDefault("I was born just to listen to this music");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
            LobotomyModPlayer.ModPlayer(player).HarmonyConnected = true;
        }
    }
}