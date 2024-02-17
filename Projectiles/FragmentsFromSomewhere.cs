using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Projectiles
{
	public class FragmentsFromSomewhere : LobcorpSpear
	{
        protected override float HoldoutRangeMax => base.HoldoutRangeMax - 16;
    }
}
