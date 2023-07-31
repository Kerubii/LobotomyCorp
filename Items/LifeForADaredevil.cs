using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items
{
	public class LifeForADaredevil : LobCorpLight
	{
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("An ancient sword.\n" +
                               "Just as its archetype desired, it will be useless in the hands of the frightened\n" +
                               "Ignores target's defense"); */
        }

        public override void SetDefaults() {
			Item.CloneDefaults(ItemID.Katana);
            Item.useStyle = 15;
			Item.damage = 32;
            Item.DamageType = DamageClass.Generic;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = LobotomyCorp.WeaponSound("katana");
		}

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ScalingArmorPenetration += 1f;
        }

        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage.Base = (int)(player.statLifeMax2 * (float)Item.damage * 0.01f);
        }

        public override void AddRecipes() {
            CreateRecipe()
            .AddIngredient(ItemID.IronBar, 10)
            .AddIngredient(ItemID.LeadBar, 10)
            .AddIngredient(ItemID.Obsidian, 5)
            .AddTile(Mod, "BlackBox2")
            .Register();
        }
	}
}