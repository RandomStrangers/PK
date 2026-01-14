namespace PattyKaki.Commands.Misc
{
    public class CmdBye : Command
    {
        public override string Name { get { return "Bye"; } }
        public override string Shortcut { get { return ""; } }
        public override string Type { get { return CommandTypes.Other; } }
        public override LevelPermission DefaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player p2 in players)
            {
                if (string.IsNullOrEmpty(message)) 
                { 
                    p2.Disconnect();
                }
                else
                {
                    p2.Leave(message);
                }
            }
        }
        public override void Help(Player p)
        {
            if (p == null || p.IsSuper || p.IsPK)
            {
                p.Message("&T/Bye &H- Makes ALL players leave the server with an optional message");
                return;
            }
            else
            {
                p.Disconnect();
            }
        }
    }
}