namespace PattyKaki.Commands.Misc
{
    public class CmdDisconnect : Command
    {
        public override string Name { get { return "Disconnect"; } }
        public override string Shortcut { get { return "leave"; } }
        public override string Type { get { return CommandTypes.Other; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool SuperUseable { get { return false; } }

        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                p.Disconnect();
            }
            else
            {
                p.Leave(message);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Disconnect (message) &H- Leaves the server with an optional message.");
        }
    }
}