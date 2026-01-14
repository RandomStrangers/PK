using System;

namespace PattyKaki.Commands.Chatting
{
    public sealed class CmdEnd : Command2
    {
        public override string Name { get { return "End"; } }
        public override string Type { get { return CommandTypes.Other; } }
        public override LevelPermission DefaultRank { get { return LevelPermission.Owner; } }
 
        public override void Use(Player p, string message, CommandData data)
        {
            End(p);
        }
        public static void End(Player p)
        {
            if (!CheckPerms(p))
            {
                p.Message("Only PattyKaki or the Server Owner can end the server."); return;
            }
            Environment.Exit(0);        
        }
        public static bool CheckPerms(Player p)
        {
            if (p.IsPK) return true;

            if (Server.Config.OwnerName.CaselessEq("Notch")) return false;
            return p.name.CaselessEq(Server.Config.OwnerName);
        }
        public override void Help(Player p)
        {
            p.Message("&T/End &H- Kills the server");
        }
    }
}