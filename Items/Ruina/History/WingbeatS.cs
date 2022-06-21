using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Audio;

namespace LobotomyCorp.Items.Ruina.History
{
	public class WingbeatS : SEgoItem
	{
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "LobotomyCorp/Items/Wingbeat_Hands", EquipType.HandsOn, null, "LobotomyCorp:WingBeatBlades");
                //WingbeatHandOff = EquipLoader.AddEquipTexture(Mod, "LobotomyCorp/Items/Wingbeat_Hands", EquipType.HandsOff);
            }
        }

        public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Wingbeat");
			Tooltip.SetDefault("A Realized E.G.O.\n" + 
                               "\"Everything will be peaceful while you are under the fairies' care.\"\n");
        }

        public override void SetDefaults() 
		{
            PassiveText = "Ravenous Hunger - First enemy hit has a 10% lifesteal\n" +
                          "Fairies Care - Send a fairy to heal an enemy or ally\n" +
                          "Prepared Meal - 15% increased damage and 20% increased lifesteal against enemies under Fairies Care" +
                          "|Predation - Your health slowly drains, temporarily stopped after killing an enemy";

            EgoColor = LobotomyCorp.ZayinRarity;

            Item.damage = 48;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;

			Item.useTime = 22;
			Item.useAnimation = 22;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = 2;
            Item.shootSpeed = 16f;
            Item.shoot = ModContent.ProjectileType<Projectiles.WingbeatR>();

            Item.noUseGraphic = true;
            Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/Fairy_QueenAtk") with { Volume = 0.5f };
            Item.noMelee = true;
			Item.autoReuse = true;
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool SafeCanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shootSpeed = 8f;
                Item.shoot = ModContent.ProjectileType<Projectiles.WingbeatFairy>();
                Item.UseSound = null;
            }
            else
            {
                Item.shootSpeed = 12f;
                Item.shoot = ModContent.ProjectileType<Projectiles.WingbeatR>();
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/Fairy_QueenAtk") with { Volume = 0.5f };
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.WingbeatR>()] == 0;
        }

        public override void HoldItem(Player player)
        {
            //player.handoff = (sbyte)WingbeatHandOff;
            //player.handon = (sbyte)WingbeatHandOn;
            player.handon = (sbyte)EquipLoader.GetEquipSlot(Mod, "LobotomyCorp:WingBeatBlades", EquipType.HandsOn);
            if (!player.HasBuff(ModContent.BuffType<Buffs.Gluttony>()))
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Fairy_QueenChange") with { Volume = 0.5f }, player.Center);
            player.AddBuff(ModContent.BuffType<Buffs.Gluttony>(), 180);

            if (player.itemTime == 1)
            {
                for (int i = 0; i <3; i++)
                {
                    int d = Dust.NewDust(player.position, player.width, player.height, DustID.DungeonSpirit);
                    Dust dust = Main.dust[d];
                    dust.noGravity = true;
                    dust.velocity *= 0;
                    dust.fadeIn = 1.2f;
                }
            }
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Wingbeat>())
            .AddIngredient(ItemID.Bell)
            .AddIngredient(ItemID.PixieDust, 25)
            .AddTile<Tiles.BlackBox3>()
            .Register();
        }
	}
}