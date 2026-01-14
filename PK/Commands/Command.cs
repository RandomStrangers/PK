/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)

    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    https://opensource.org/license/ecl-2-0/
    https://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using PattyKaki.Commands;
using PattyKaki.Maths;
using PattyKaki.Scripting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PattyKaki
{
    public abstract partial class Command
    {
        /// <summary> The full name of this command (e.g. 'Copy') </summary>
        public abstract string Name { get; }
        /// <summary> The shortcut/short name of this command (e.g. `"c"`) </summary>
        public virtual string Shortcut { get { return ""; } }
        /// <summary> The type/group of this command (see `CommandTypes` class) </summary>
        public abstract string Type { get; }
        /// <summary> Whether this comand can be used in museums </summary>
        /// <remarks> Level altering (e.g. places a block) commands should return false </remarks>
        public virtual bool MuseumUsable { get { return true; } }
        /// <summary> The default minimum rank that is required to use this command </summary>
        public virtual LevelPermission DefaultRank { get { return LevelPermission.Guest; } }

        public virtual void Use(Player p, string message)
        {
            if (LoopCount < 5)
            {
                LoopCount++;
                Use(p, message, p.DefaultCmdData);
            }
        }
        public virtual void Use(Player p, string message, CommandData data)
        {
            if (LoopCount < 5)
            {
                LoopCount++;
                Use(p, message);
            }
        }
        public virtual void Help(Player p)
        {
        }
        public virtual void Help(Player p, string message) { Help(p); Formatter.PrintCommandInfo(p, this); }

        public virtual CommandPerm[] ExtraPerms { get { return null; } }
        public virtual CommandAlias[] Aliases { get { return null; } }

        /// <summary> Whether this command is usable by 'super' players (PK, IRC, etc) </summary>
        public virtual bool SuperUseable { get { return true; } }
        public virtual bool MessageBlockRestricted { get { return Type.CaselessContains("mod"); } }
        /// <summary> Whether this command can be used when a player is frozen </summary>
        /// <remarks> Only informational commands should override this to return true </remarks>
        public virtual bool UseableWhenFrozen { get { return false; } }

        /// <summary> Whether using this command is logged to server logs </summary>
        /// <remarks> return false to prevent this command showing in logs (e.g. /pass) </remarks>
        public virtual bool LogUsage { get { return true; } }
        /// <summary> Whether this commands updates the 'most recent command used' by players </summary>
        /// <remarks> return false to prevent this command showing in /last (e.g. /pass, /hide) </remarks>
        public virtual bool UpdatesLastCmd { get { return true; } }

        public virtual CommandParallelism Parallelism
        {
            get { return Type.CaselessEq(CommandTypes.Information) ? CommandParallelism.NoAndWarn : CommandParallelism.Yes; }
        }
        public CommandPerms Permissions;

        public static List<Command> allCmds = new List<Command>();
        /*public static bool IsCore(Command cmd)
        {
            return cmd.GetType().Assembly == Assembly.GetExecutingAssembly(); // TODO common method
        }*/
        public static List<Command> coreCmds = new List<Command>();
        public static bool IsCore(Command cmd) 
        { 
            return coreCmds.Contains(cmd); 
        }
        public static List<Command> CopyAll() { return new List<Command>(allCmds); }


        static void RegisterCore(params Command[] cmds)
        {
            foreach (Command cmd in cmds)
            {
                if (Server.Config.DisabledCommands.CaselessContains(cmd.Name)) continue;
                coreCmds.Add(cmd);
                Register(cmd);
            }
        }
        public static void InitAll()
        {
            allCmds.Clear();
            coreCmds.Clear();
            Alias.coreAliases.Clear();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (!type.IsSubclassOf(typeof(Command)) || type.IsAbstract || !type.IsPublic) continue;

                Command cmd = (Command)Activator.CreateInstance(type);
                if (Server.Config.DisabledCommands.CaselessContains(cmd.Name)) continue;
                RegisterCore(cmd);
            }

            IScripting.AutoloadCommands();
        }
        public static void Register(params Command[] commands)
        {
            foreach (Command cmd in commands) Register(cmd);
        }
        public static void Register(Command cmd)
        {
            allCmds.Add(cmd);
            cmd.Permissions = CommandPerms.GetOrAdd(cmd.Name, cmd.DefaultRank);

            CommandPerm[] extra = cmd.ExtraPerms;
            if (extra != null)
            {
                for (int i = 0; i < extra.Length; i++)
                {
                    CommandExtraPerms exPerms = CommandExtraPerms.GetOrAdd(cmd.Name, i + 1, extra[i].Perm);
                    exPerms.Desc = extra[i].Description;
                }
            }
            Alias.RegisterDefaults(cmd);
        }

        public static void TryRegister(bool announce, params Command[] commands)
        {
            foreach (Command cmd in commands)
            {
                if (Find(cmd.Name) != null) continue;

                Register(cmd);
                if (announce) Logger.Log(LogType.SystemActivity, "Command /{0} loaded", cmd.Name);
            }
        }

        public static bool Unregister(Command cmd)
        {
            bool removed = allCmds.Remove(cmd);

            // typical usage: Command.Unregister(Command.Find("xyz"))
            // So don't throw exception if Command.Find returned null
            if (cmd != null) Alias.UnregisterDefaults(cmd);
            return removed;
        }

        public static void Unregister(params Command[] commands)
        {
            foreach (Command cmd in commands) Unregister(cmd);
        }


        public static string GetColoredName(Command cmd)
        {
            LevelPermission perm = cmd.Permissions.MinRank;
            return Group.GetColor(perm) + cmd.Name;
        }

        public static Command Find(string name)
        {
            foreach (Command cmd in allCmds)
            {
                if (cmd.Name.CaselessEq(name)) return cmd;
            }
            return null;
        }

        public static void Search(ref string cmdName, ref string cmdArgs)
        {
            if (cmdName.Length == 0) return;
            Alias alias = Alias.Find(cmdName);

            // Aliases override built in command shortcuts
            if (alias == null)
            {
                foreach (Command cmd in allCmds)
                {
                    if (!cmd.Shortcut.CaselessEq(cmdName)) continue;
                    cmdName = cmd.Name; return;
                }
                return;
            }

            cmdName = alias.Target;
            string format = alias.Format;
            if (format == null) return;

            if (format.Contains("{args}"))
            {
                cmdArgs = format.Replace("{args}", cmdArgs);
            }
            else
            {
                cmdArgs = format + " " + cmdArgs;
            }
            cmdArgs = cmdArgs.Trim();
        }
    }

    public enum CommandContext : byte
    {
        Normal, Static, SendCmd, Purchase, MessageBlock
    }

    public struct CommandData
    {
        public LevelPermission Rank;
        public CommandContext Context;
        public Vec3S32 MBCoords;
    }

    // Clunky design, but needed to stay backwards compatible with custom commands
    public abstract class Command2 : Command
    {
        public override void Use(Player p, string message)
        {
            Use(p, message, p.DefaultCmdData);
        }
    }

    public enum CommandParallelism
    {
        NoAndSilent, NoAndWarn, Yes
    }
}

namespace PattyKaki.Commands
{
    public struct CommandPerm
    {
        public LevelPermission Perm;
        public string Description;

        public CommandPerm(LevelPermission perm, string desc)
        {
            Perm = perm; Description = desc;
        }
    }

    public struct CommandAlias
    {
        public string Trigger, Format;

        public CommandAlias(string cmd, string format = null)
        {
            Trigger = cmd; Format = format;
        }
    }
}