using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;

namespace LobotomyCorp.Items.Ruina.Literature
{
	public class SanguineDesireR : SEgoItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Sanguine Desire"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault(GetTooltip());
		}

		public override void SetDefaults() 
		{
			EgoColor = LobotomyCorp.HeRarity;

			Item.width = 70;
			Item.height = 70;

			Item.damage = 62;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 2.3f;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.value = 10000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Yellow;

			Item.shoot = ModContent.ProjectileType<Projectiles.Realized.SanguineDesireR>();
			Item.shootSpeed = 16f;
		}

        public override bool SafeCanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				Item.shoot = ModContent.ProjectileType<Projectiles.Realized.SanguineDesireRHeavy>();
			}
			else
            {
				Item.shoot = ModContent.ProjectileType<Projectiles.Realized.SanguineDesireR>();
			}

			return base.SafeCanUseItem(player);
        }

        public override float UseSpeedMultiplier(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				return 0.8f;
			}
			return base.UseSpeedMultiplier(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes() 
		{
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			LobotomyGlobalNPC.SanguineDesireApplyBleed(target, 0.8f, target.damage * 3, 60, 600);

            base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        /// <summary>
        /// Returns true if an npc was Glitter'd
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool Glitter(Player player, float chance = 0.8f)
        {
			bool GlitterSuccess = false;
			foreach (NPC n in Main.npc)
            {
				if (n.active && n.life > 0 && !n.friendly && Main.rand.NextFloat(1f) < chance)
                {
					GlitterSuccess = true;
					n.AddBuff(ModContent.BuffType<Buffs.Glitter>(), 300);
					LobotomyGlobalNPC.LNPC(n).SanguineDesireGlitter = true;
					LobotomyGlobalNPC.LNPC(n).SanguineDesireGlitterTarget = player.whoAmI;
					n.target = player.whoAmI;
				}
            }
			return GlitterSuccess;
        }
	}
}