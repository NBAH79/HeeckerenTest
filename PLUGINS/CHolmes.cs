
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
    public class CHolmes : PRoConPluginAPI, IPRoConPluginInterface
    {
        public enum mode { PBguid, EAguid, BOTHguids };

        class SinglePlayer
        {
            public string name;
            public string pbguid;
            public string eaguid;
        }

        private List<SinglePlayer> Players;

        private string LogDirectory;
        private string Mode;
        private bool PBGuid;
        private bool EAGuid;

        public CHolmes()
        {
            this.Players = new List<SinglePlayer>();

            this.LogDirectory = "Guids";
            this.Mode= "EA guid";
            this.PBGuid= false;
            this.EAGuid= true;
        }

        /*public void LogWrite(String msg)
        {
            //StreamLogWrite(msg);
            //this.ExecuteCommand("procon.protected.pluginconsole.write", msg);
        }*/

        public void ConsoleWrite(String msg)
        {
            this.ExecuteCommand("procon.protected.pluginconsole.write", msg);
        }

        public string GetPluginName()
        {
            return "Holmes";
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
            this.RegisterEvents(this.GetType().Name, "OnServerInfo", "OnRoundOver", "OnLevelStarted", "OnListPlayers", "OnPunkbusterPlayerInfo");
        }

        public void OnPluginEnable()
        {
            ConsoleWrite("Holmes enabled!");
        }

        public void OnPluginDisable()
        {
            this.ExecuteCommand("procon.protected.tasks.remove", "Holmes");
            ConsoleWrite("Holmes disabled!");
        }

        public List<CPluginVariable> GetDisplayPluginVariables()
        {
            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
            lstReturn.Add(new CPluginVariable("Settings|Log directory", typeof(string), this.LogDirectory));
            lstReturn.Add(new CPluginVariable("Settings|Mode", "enum.mode(EA guid|PB guid|Both guids)", this.Mode));
            return lstReturn;
        }

        public List<CPluginVariable> GetPluginVariables()
        {
            return GetDisplayPluginVariables();
        }

        public void SetPluginVariable(String strVariable, String strValue)
        {
            if (Regex.Match(strVariable, "Log directory").Success) this.LogDirectory = strValue;
            if (Regex.Match(strVariable, "Mode").Success) this.Mode=strValue;
            if (Regex.Match(strValue, "EA guid").Success) {EAGuid=true;PBGuid=false;return; }
            if (Regex.Match(strValue, "PB guid").Success) { EAGuid = false; PBGuid = true; return; }
            if (Regex.Match(strValue, "Both guids").Success) { EAGuid = true; PBGuid = true; return; }
        }

        private void UnregisterAllCommands() { }

        private void SetupHelpCommands() { }

        private void RegisterAllCommands() { }

        public override void OnServerInfo(CServerInfo csiServerInfo) { }

        public override void OnRoundOver(int winningTeamId)
        {
            string filename = LogDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd-HH-mm") + ".txt";
            StreamWriter streamlog = null;
            try
            {
                if (!Directory.Exists(LogDirectory))  // if it doesn't exist, create
                    Directory.CreateDirectory(LogDirectory);

                streamlog = File.CreateText(filename);
                lock(Players){
                    foreach(SinglePlayer sp in Players) {
                        streamlog.Write(sp.name);
                        streamlog.Write(" ");
                        streamlog.Write(sp.eaguid);
                        streamlog.Write(" ");
                        streamlog.WriteLine(sp.pbguid);
                    }
                }
                streamlog.Close();
            }
            catch (Exception e)
            {
                ConsoleWrite(e.Message + filename);
            }
            ConsoleWrite("Holmes:" + filename);
        }

        public override void OnLevelStarted()
        {
            this.Players = new List<SinglePlayer>();
        }

        public override void OnPunkbusterPlayerInfo(CPunkbusterInfo cpbiPlayer) //triggered every 30 seconds
        {
            if (!PBGuid) return;
            lock(Players){
                foreach (SinglePlayer sp in Players) if (String.Compare(cpbiPlayer.SoldierName,sp.name)==0) { sp.pbguid = cpbiPlayer.GUID; return; }
                Players.Add(new SinglePlayer {name = cpbiPlayer.SoldierName, pbguid = cpbiPlayer.GUID });
            }
        }

        public override void OnListPlayers(List<CPlayerInfo> players, CPlayerSubset subset) //triggered every 30 seconds
        {
            if (!EAGuid) return;
            lock (Players)
            {
                foreach (CPlayerInfo cpiPlayer in players) SetPlayerInfo(cpiPlayer.SoldierName,cpiPlayer.GUID);
            }
        }

        public void SetPlayerInfo(string name,string eaguid) 
        {
            foreach (SinglePlayer sp in Players) if (String.Compare(name, sp.name) == 0) { sp.eaguid = eaguid; return; }
            Players.Add(new SinglePlayer { name = name, eaguid = eaguid });
        }

        public static string GetDesc()
        {
            return @"
			<div style='margin:20px'>
                <h2>
                    Description
                </h2>
		        <span style='color:000000;font-size:12pt;margin:20px;'>
                    Player's guids logger
                </span>
            </div>    
            <div style='margin:20px'>
                <p style='color:000000;font-size:12pt;margin:20px;'>
                    Saves all visitor's guids
                </p>
                <p>
                    Named after detective Sherlock Holmes.
                </p>
	        </div>";
        }
    }
}
