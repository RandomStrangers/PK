/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCGalaxy)
    
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using PattyKaki.Scripting;

namespace PattyKaki
{
    /// <summary> This class provides for simple modification to PattyKaki </summary>
    public abstract class Plugin_Simple : Plugin
    {
        public override void Load(bool auto)
        {
            Load();
        }
        public override void Unload(bool auto)
        {
            Unload();
        }
        /// <summary> Hooks into events and initalises states/resources etc </summary>
        public abstract void Load();
        /// <summary> Unhooks from events and disposes of state/resources etc </summary>
        public abstract void Unload();
        /// <summary> Called when a player does /Help on the plugin. Typically tells the player what this plugin is about. </summary>
        /// <param name="p"> Player who is doing /Help. </param>
        public abstract override void Help(Player p);
        /// <summary> Name of the plugin. </summary>
        public abstract override string Name { get; }
        /// <summary> Message to display once this plugin is loaded. </summary>
        public abstract override string Welcome { get; }
        /// <summary> Version of this plugin. </summary>
        public abstract override int Build { get; }
        /// <summary> Oldest version of PattyKaki this plugin is compatible with. </summary>
        public abstract override string PK_Version { get; }
        /// <summary> The Creator/author of this plugin. (Your name) </summary>
        public abstract override string Creator { get; }
        /// <summary> Whether or not to auto load this plugin on server startup. </summary>
        public abstract override bool LoadAtStartup { get; }
        public static List<Plugin_Simple> all = new List<Plugin_Simple>();
        public static Plugin_Simple Find(string name)
        {
            foreach (Plugin_Simple spl in all)
            {
                if (spl.Name.CaselessEq(name)) return spl;
            }
            return null;
        }
        public static bool Load(Plugin_Simple p)
        {
            try
            {
                all.Add(p);
                if (p.LoadAtStartup)
                {
                    p.Load();
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "Simple plugin {0} was not loaded, you can load it with /psload", p.Name);
                }
                if (!string.IsNullOrEmpty(p.Welcome)) Logger.Log(LogType.SystemActivity, p.Welcome);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading simple plugin " + p.Name, ex);
                Logger.Log(LogType.Warning, "You can go bug {0} about it.", p.Creator);
                return false;
            }
        }
        public static bool Unload(Plugin_Simple p)
        {
            bool success = true;
            try
            {
                p.Unload();
                Logger.Log(LogType.SystemActivity, "Simple plugin {0} was unloaded.", p.Name);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading simple plugin " + p.Name, ex);
                success = false;
            }
            all.Remove(p);
            return success;
        }
        public static new void UnloadAll()
        {
            for (int i = 0; i < all.Count; i++)
            {
                Unload(all[i]); i--;
            }
        }
        public static new void LoadAll()
        {
            IScripting_Simple.AutoloadSimplePlugins();
        }
    }
}