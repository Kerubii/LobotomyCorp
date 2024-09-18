using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ReLogic.Content;
using LobotomyCorp.Utils;

namespace LobotomyCorp.PlayerDrawEffects
{
    public class LobotomyPlayerParticle : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.MountBack);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return !Main.gameMenu;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(drawInfo.drawPlayer);
            AuraParticle[] particle = modPlayer.PlayerParticles;
            if (particle != null)
            {
                for (int i = 0; i < particle.Length; i++)
                {
                    if (particle[i] != null && particle[i].Active)
                    {
                        drawInfo.DrawDataCache.Add(particle[i].Draw(ref drawInfo, Mod));
                    }
                }
            }
        }

        public static void GenerateAuraParticle(LobotomyModPlayer modPlayer, AuraBehavior auraUsed)
        {
            Player player = modPlayer.Player;
            for (int i = 0; i < modPlayer.PlayerParticles.Length; i++)
            {
                AuraParticle particle = modPlayer.PlayerParticles[i];
                if (particle == null || !particle.Active)
                {
                    modPlayer.PlayerParticles[i] = new AuraParticle(modPlayer.Player, player.direction, player.gravDir, (float)Main.timeForVisualEffects, auraUsed, i);
                    break;
                }
            }
        }

        public static void GeneratePlayerParticle(LobotomyModPlayer modPlayer, AuraBehavior particleBehavior)
        {
            Player player = modPlayer.Player;
            for (int i = 0; i < modPlayer.PlayerParticles.Length; i++)
            {
                AuraParticle particle = modPlayer.PlayerParticles[i];
                if (particle == null || !particle.Active)
                {
                    modPlayer.PlayerParticles[i] = new AuraParticle(modPlayer.Player, player.direction, player.gravDir, (float)Main.timeForVisualEffects, particleBehavior, i);
                    break;
                }
            }
        }
    }

    public interface AuraBehavior
    {
        /// <summary>
        /// How often this Aura spawns if its on a player
        /// </summary>
        int intensity { get; }

        /// <summary>
        /// Null if none
        /// </summary>
        //CustomShaderData shaderData { get ; }

        /// <summary>
        /// Use { return mod.Assets.Request<Texture2D>("Folder/File").Value; }
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        Texture2D GetTexture(Mod mod);
        Rectangle GetSourceRect(Texture2D texture, int index);
        Color GetColor(PlayerDrawSet drawInfo, AuraParticle particle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dir"></param>
        /// <param name="gravDir"></param>
        /// <param name="time"></param>
        /// <param name="particle"></param>
        /// <param name="index"></param>
        void SpawnParam(Player player, int dir, float gravDir, float time, AuraParticle particle, int index);
        void Behavior(Player player, int dir, float gravDir, float time, AuraParticle particle);
    }

    public class AuraParticle
    {
        public bool Active;
        private bool local;
        public bool IsLocal { get { return local; } }
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public float Scale;
        public int textureIndex;
        public int particleTime;
        AuraBehavior behavior;

        /*
        public AuraBehavior GetAuraBehavior { get { return behavior; } }
        public Vector2 Position { get { return position; } }
        public float Rotation { get { return rotation; } }
        public float Scale { get { return scale; } }
        */

        public AuraParticle(Player player, int dir, float gravDir, float time, AuraBehavior newBehavior, int index, bool isLocal = false)
        {
            behavior = newBehavior;
            Active = true;
            Position = player.Center;
            Velocity = Vector2.Zero;
            Rotation = 0;
            Scale = 1f;
            textureIndex = 0;
            particleTime = 0;
            local = isLocal;
            behavior.SpawnParam(player, dir, gravDir, time, this, index);
        }

        public void Update(Player player, int dir, float gravDir, float time)
        {
            behavior.Behavior(player, dir, gravDir, time, this);
            particleTime++;
        }

        public DrawData Draw(ref PlayerDrawSet drawInfo, Mod mod)
        {
            Texture2D tex = behavior.GetTexture(mod);
            Rectangle frame = behavior.GetSourceRect(tex, textureIndex);

            DrawData data = new DrawData(
                tex,
                Position - Main.screenPosition,
                frame,
                behavior.GetColor(drawInfo, this),
                Rotation,
                frame.Size() / 2,
                Scale,
                0,
                0);

            return data;
        }
    }

    public class TestAura : AuraBehavior
    {
        public int intensity => 3;

        public CustomShaderData shaderData => null;

        public Texture2D GetTexture(Mod mod) { return mod.Assets.Request<Texture2D>("Misc/FlameParticlesL").Value; }

        public Rectangle GetSourceRect(Texture2D texture, int index)
        {
            return texture.Frame(4, 1, index);
        }

        public Color GetColor(PlayerDrawSet drawInfo, AuraParticle particle) { return Color.Blue * 0.9f * (1f - particle.particleTime / 13f); }

        public void SpawnParam(Player player, int dir, float gravDir, float time, AuraParticle particle, int index)
        {
            particle.textureIndex = Main.rand.Next(4);
            particle.Position.Y -= 12;
            particle.Position.X += (8 * (float)Math.Sin(0.436332f * time));
            particle.Velocity.Y -= 2f;
            particle.Rotation = Main.rand.NextFloat(6.28f);
            particle.Scale = 1f;
        }

        public void Behavior(Player player, int dir, float gravDir, float time, AuraParticle particle)
        {
            particle.Position.Y += particle.Velocity.Y;
            particle.Scale -= 0.04f;

            if (particle.particleTime > 10)
            {
                particle.Active = false;
            }
        }
    }
}
