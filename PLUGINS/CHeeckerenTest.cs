
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using PRoCon.Core;
using PRoCon.Core.Plugin;
using PRoCon.Core.Plugin.Commands;
using PRoCon.Core.Players;
using PRoCon.Core.Players.Items;
using PRoCon.Core.Battlemap;
using PRoCon.Core.Maps;

namespace PRoConEvents
{
    public class CHeeckerenTest : PRoConPluginAPI, IPRoConPluginInterface
    {

        class SingleKill
        {
            public DateTime time;
            public string killer;
            public string victim;
        }

        private List<SingleKill> Kills;

        private string LogDirectory;

        public CHeeckerenTest()
        {
            this.Kills = new List<SingleKill>();

            this.LogDirectory = "Heeckeren";
        }

        public void ConsoleWrite(String msg)
        {
            this.ExecuteCommand("procon.protected.pluginconsole.write", msg);
        }

        public string GetPluginName()
        {
            return "HeeckerenTest";
        }

        public string GetPluginVersion()
        {
            return "3.20.10.12";
        }

        public string GetPluginAuthor()
        {
            return "Pavlovsky Ivan 'NBAH79RUS'";
        }

        public string GetPluginWebsite()
        {
            return "";
        }

        public string GetPluginDescription()
        {
            return GetDesc();
        }

        public void OnPluginLoaded(string strHostName, string strPort, string strPRoConVersion)
        {
            this.RegisterEvents(this.GetType().Name, "OnServerInfo", "OnPlayerKilled", "OnRoundOver", "OnLevelStarted");
        }

        public void OnPluginEnable()
        {
            ConsoleWrite("HeeckerenTest enabled!");
        }

        public void OnPluginDisable()
        {
            this.ExecuteCommand("procon.protected.tasks.remove", "HeeckerenTest");
            ConsoleWrite("HeeckerenTest disabled!");
        }

        public List<CPluginVariable> GetDisplayPluginVariables()
        {
            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
            lstReturn.Add(new CPluginVariable("Settings|Log directory", typeof(string), this.LogDirectory));
            return lstReturn;
        }

        public List<CPluginVariable> GetPluginVariables()
        {
            return GetDisplayPluginVariables();
        }

        public void SetPluginVariable(String strVariable, String strValue)
        {
            if (Regex.Match(strVariable, "Log directory").Success) this.LogDirectory = strValue;
        }

        private void UnregisterAllCommands() { }

        private void SetupHelpCommands() { }

        private void RegisterAllCommands() { }


        public override void OnServerInfo(CServerInfo csiServerInfo) { }

        public override void OnRoundOver(int winningTeamId)
        {
            string filename= LogDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd-HH-mm") + ".txt";
            StreamWriter streamlog = null;
            try
            {
                if (!Directory.Exists(LogDirectory))  // if it doesn't exist, create
                    Directory.CreateDirectory(LogDirectory);
                 
                streamlog = File.AppendText(filename);
                lock(Kills){
                    foreach(SingleKill sk in Kills){
                        streamlog.Write(sk.time.ToString());
                        streamlog.Write(" ");
                        streamlog.Write(sk.killer);
                        streamlog.Write(" ");
                        streamlog.WriteLine(sk.victim);
                    }
                }
                streamlog.Close();
            }
            catch (Exception e)
            {
                ConsoleWrite(e.Message+filename);
            }
            ConsoleWrite("CHeeckerenTest:"+filename);
        }

        public override void OnLevelStarted()
        {
            this.Kills = new List<SingleKill>();
        }

        public override void OnPlayerKilled(Kill KDetails)
        {
            if (KDetails.IsSuicide) return;
            if (KDetails.Killer.SoldierName.Length==0) return; //empty killer's name happens
            Kills.Add(new SingleKill{time = DateTime.Now,killer = KDetails.Killer.SoldierName,victim = KDetails.Victim.SoldierName});
        }

        public static string GetDesc()
        {
            return @"
			<div style='margin:20px'>
                <h2>
                    Description
                </h2>
		        <span style='color:000000;font-size:12pt;margin:20px;'>
                    Kills logger
                </span>
            </div>    
            <div style='margin:20px'>
                <p style='color:000000;font-size:12pt;margin:20px;'>
                    Records all kills at every round and saves it to files
                    in [Date] [Killer] [Victim] format. Where Date is DateTime structure.
                </p>
                <p>
                    Named after lieutenant Georges Charles de Heeckeren d'Anthes 
                    known as a killer of Alexander Sergeevich Pushkin. The duel took place in 1837.
                </p>
	        </div>";
        }
    }
}
