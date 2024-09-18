using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Chat;
using LobotomyCorp;


namespace LobotomyCorp.NPCs.OneBadManyGood
{
    class OneBadManyGood : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<Configs.LobotomyServerConfig>().TestItemEnable;
        }

        private bool SinsConfessed = false;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("One Sin and Hundred of Good Deeds");
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 80;
            NPC.lifeMax = 1200;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.damage = 0;
			NPC.defense = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.0f;
			NPC.HitSound = SoundID.NPCHit1;
            LobotomyGlobalNPC.LNPC(NPC).RiskLevel = (int)RiskLevel.Zayin;
        }
		
		public override void AI()
		{
            NPC.localAI[0] += MathHelper.ToRadians(1);
            NPC.localAI[1] += MathHelper.ToRadians(2);
            NPC.gfxOffY = 10 * (float)Math.Sin(NPC.localAI[0]);
		}

        public override bool CanChat()
        {
            return true;
        }

        public override string GetChat()
        {
            // NPC.SpawnedFromStatue value is kept when the NPC is transformed.
            switch (Main.rand.Next(2))
            {
                case 0:
                    return "A giant skull shows itself in your vicinity, its presence odd and terrifying.";
                default:
                    return "As you stand near the giant skull, your body feels slightly lighter.";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                Main.npcChatText = "You kneel and pray, nothing happens.";
            }
            else
            {
                if (!SinsConfessed)
                {
                    Main.npcChatText = "You confess your sins, Its mouth begins to quiver and your body feels lighter. It drops a mysterious material.";
                    Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_Loot(), ItemID.Bone, 25);
                }
                else
                    Main.npcChatText = "You do not know what to confess, It seems to glow brighter.";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Pray";
            button2 = "Confess";
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/OneBadManyGood/OneBadManyGoodBack").Value;
            Vector2 position = NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY);
            Vector2 origin = texture.Size() / 2;
            spriteBatch.Draw(texture, position, texture.Frame(), drawColor, NPC.rotation, origin, NPC.scale, 0, 0f);
            texture = Mod.Assets.Request<Texture2D>("NPCs/OneBadManyGood/OneBadManyGoodJaw").Value;
            origin = texture.Size() / 2 - new Vector2(0, 2 + 2 * (float)Math.Sin(NPC.localAI[1]));
            spriteBatch.Draw(texture, position, texture.Frame(), drawColor, NPC.rotation, origin, NPC.scale, 0, 0f);
            texture = Mod.Assets.Request<Texture2D>("NPCs/OneBadManyGood/OneBadManyGood").Value;
            origin = texture.Size() / 2;
            spriteBatch.Draw(texture, position, texture.Frame(), drawColor, NPC.rotation, origin, NPC.scale, 0, 0f);
            return false;
        }
	}
}
