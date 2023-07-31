using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using LobotomyCorp.Utils;

namespace LobotomyCorp.Buffs
{
	public class RecklessFoolishness : ModBuff
	{
        private static AuraBehavior BuffAura => new RecklessAura1();
        private static AuraBehavior BuffAura2 => new RecklessAura2();

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Reckless Foolishness");
			// Description.SetDefault("");
			BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            int gift = LobotomyModPlayer.ModPlayer(Main.LocalPlayer).LifeForADareDevilGift;
            if (gift < 600)
            {
                tip = "15% increased movement speed, melee speed and counter damage and 15% decreased damage reduction";
            }
            else
                tip = "15% increased movement speed, melee speed and counter damage and 10% decreased damage reduction";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
			player.moveSpeed += 0.15f;

            LobotomyModPlayer.ModPlayer(player).LifeForADareDevilGiftActive = true;
            int gift = LobotomyModPlayer.ModPlayer(player).LifeForADareDevilGift;
            if (gift < 600)
            {
                player.endurance -= 0.15f;
                LobotomyModPlayer.ModPlayer(player).CurrentAura = BuffAura2;
            }
            else if (gift < 1200)
            {
                player.endurance -= 0.1f;
                LobotomyModPlayer.ModPlayer(player).CurrentAura = BuffAura;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }

    public class RecklessAura1 : AuraBehavior
    {
        public int intensity => 3;

        public Texture2D GetTexture(Mod mod) { return mod.Assets.Request<Texture2D>("Misc/FlameParticlesL").Value; }

        public Rectangle GetSourceRect(Texture2D texture, int index)
        {
            return texture.Frame(4, 1, index);
        }

        public Color GetColor(PlayerDrawSet drawInfo, AuraParticle particle) { return Color.Orange * 0.9f * (1f - particle.particleTime / 13f); }

        public void SpawnParam(Player player, int dir, float gravDir, float time, AuraParticle particle)
        {
            particle.textureIndex = Main.rand.Next(4);
            particle.position.Y -= 12;
            particle.position.X += (8 * (float)Math.Sin(0.436332f * time));
            particle.velocity.Y -= 2f;
            particle.rotation = Main.rand.NextFloat(6.28f);
            particle.scale = 1f;
        }

        public void Behavior(Player player, int dir, float gravDir, float time, AuraParticle particle)
        {
            particle.position.Y += particle.velocity.Y;
            particle.scale -= 0.04f;

            if (particle.particleTime > 10)
            {
                particle.Active = false;
            }
        }
    }

    public class RecklessAura2 : AuraBehavior
    {
        public int intensity => 3;

        public Texture2D GetTexture(Mod mod) { return mod.Assets.Request<Texture2D>("Misc/FlameParticlesL").Value; }

        public Rectangle GetSourceRect(Texture2D texture, int index)
        {
            return texture.Frame(4, 1, index);
        }

        public Color GetColor(PlayerDrawSet drawInfo, AuraParticle particle) { return Color.Red * 0.9f * (1f - particle.particleTime / 13f); }

        public void SpawnParam(Player player, int dir, float gravDir, float time, AuraParticle particle)
        {
            particle.textureIndex = Main.rand.Next(4);
            particle.position.Y -= 12;
            particle.position.X += (8 * (float)Math.Sin(0.436332f * time));
            particle.velocity.Y -= 2f;
            particle.rotation = Main.rand.NextFloat(6.28f);
            particle.scale = 1f;
        }

        public void Behavior(Player player, int dir, float gravDir, float time, AuraParticle particle)
        {
            particle.position.Y += particle.velocity.Y;
            particle.scale -= 0.04f;

            if (particle.particleTime > 10)
            {
                particle.Active = false;
            }
        }
    }
}