using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace LobotomyCorp.Utils
{
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
