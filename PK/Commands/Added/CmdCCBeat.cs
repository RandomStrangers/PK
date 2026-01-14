using System;
using System.IO;
using PattyKaki.Network;
namespace PattyKaki.Commands
{
    public class CmdHeartbeat : Command
    {
        public override string Name { get { return "Heartbeat"; } }
        public override string Shortcut { get { return "beat"; } }
        public override string Type { get { return CommandTypes.Moderation; } }
        public override bool MuseumUsable { get { return true; } }
        public override LevelPermission DefaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message)
        {
            for (int i = 0; i <= Heartbeat.Heartbeats.Count; i++)
            {
                try
                {
                    Heartbeat.Heartbeats[i].Pump();
                    p.Message("Heartbeat pump sent.");
                }
                catch (Exception e)
                {
                    Logger.Log(LogType.Error, "Error with heartbeat pump.", e);
                    p.Message("Error with heartbeat pump: " + e + ".");
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Heartbeat &H- Forces a pump for the server heartbeats.");
        }
    }
    public sealed class CmdURL : Command2
    {
        public override string Name { get { return "ServerURL"; } }
        public override string Shortcut { get { return "URL"; } }
        public override string Type { get { return CommandTypes.Information; } }
        public override bool SuperUseable { get { return true; } }
        public override LevelPermission DefaultRank { get { return LevelPermission.Banned; } }


        public override void Use(Player p, string message, CommandData data)
        {
                string file = "./text/externalurl.txt";
                string contents = File.ReadAllText(file);
                p.Message("Server URL: " + contents);
                return;
        }
        public override void Help(Player p)
        {
            p.Message("&T/ServerURL &H- Shows the server's URL.");
        }
    }
}