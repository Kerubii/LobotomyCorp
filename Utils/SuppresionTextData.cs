using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace LobotomyCorp.Utils
{
    class SuppressionTextData
    {
        public static Dictionary<int, string> Gebura;
        public static Dictionary<string, string> GeburaBark;

        public static void Initialize()
        {
            GeburaInit();
        }
        
        public static void Unload()
        {
            Gebura = null;
            GeburaBark = null;
        }

        public static void GeburaInit()
        {
            int i = 0;
            Gebura = new Dictionary<int, string>();
            //0 - 4
            Gebura[i++] = "I'll destroy a shoddy place like this with my own hands.";
            Gebura[i++] = "What's left for me, the one who failed to protect them?";
            Gebura[i++] = "Do you really think feeble cowards like you can stop me?";
            Gebura[i++] = "Let me show you how to actually wield E.G.O.";
            //5 - 6
            Gebura[i++] = "I'm back; the Red Mist has walked out from a sea of pain.";
            Gebura[i++] = "I'm no longer weak like I used to be; I can replace any body part even if it gets cut off, and I can be repaired even if I'm broken.";
            //7 - 8
            Gebura[i++] = "Some things simply couldn't be forgotten, no matter how much time has passed.";
            Gebura[i++] = "Hatred is a poison that eviscerates me, and yet it makes me open my eyes once more.";
            //9 - 10
            Gebura[i++] = "Some things just wouldn't cool down, no matter how long they were left in the cold.";
            Gebura[i++] = "Those monsters always kill people, there is no end to this sin; I have descended to bring about their reckoning.";
            //11
            Gebura[i++] = "The right path is too far away, and I have too far to go. My heart is pulsing with anger, and I must keep pushing.";
            //12 - 13
            Gebura[i++] = "Ah… I'm breaking… I won't drop my sword, even if I turn to dust...";
            Gebura[i++] = "Even after all this, I can’t do a single thing with this power...";

            GeburaBark = new Dictionary<string, string>();
            GeburaBark["RedEyes"] = "Red Eyes";
            GeburaBark["Penitence"] = "Penitence";
            GeburaBark["Both"] = "Get blown to pieces";
            GeburaBark["GoldRush1"] = "The Road of Gold opens";

            GeburaBark["Heaven"] = "The Burrowing Heaven";

            GeburaBark["Mimicry1"] = "Nothing will Remain";
            GeburaBark["Mimicry2"] = "I'll mow you down";

            GeburaBark["DaCapo1"] = "From the Overture";
            GeburaBark["DaCapo2"] = "Adagio e Tranquillo";

            GeburaBark["Dash1"] = "Stop running";
            GeburaBark["Dash2"] = "You can't escape from me";

            GeburaBark["Teleport1"] = "Legato";
            GeburaBark["Teleport2"] = "Let's do this, partner";
            GeburaBark["Teleport3"] = "Only bloody mist remains";

            GeburaBark["Shift1"] = "This isn't enough";
            GeburaBark["Shift2"] = "I wasn't slacking off all this time";

            GeburaBark["Smile1"] = "Black Laughter";
            GeburaBark["Smile2"] = "Be Eaten";

            GeburaBark["Justitia1"] = "Justitia";
            GeburaBark["Justitia2"] = "Judgement";

            GeburaBark["Shift3"] = "Let's put an end to this";
            GeburaBark["Shift4"] = "The apocalypse is here…";

            GeburaBark["Pass1"] = "Beat it, coward";
            GeburaBark["Pass2"] = "Don't try and stop me";
            GeburaBark["Pass3"] = "You're weak";

            GeburaBark["Arrive"] = "Be torn apart before my eyes";

            GeburaBark["GoldRush2"] = "The hunt begins";
            GeburaBark["GoldRush3"] = "The Road of the King opens";

            GeburaBark["Tired1"] = "I'm just not as capable as I used to be...";
            GeburaBark["Tired2"] = "I'll break it down...";
            GeburaBark["Tired3"] = "I'll kill all of you...";
            GeburaBark["Tired4"] = "I can't stop";
            GeburaBark["Tired5"] = "It just isn't enough";
        }
    }
}
