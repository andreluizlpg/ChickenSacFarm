using System.Windows.Controls;
using System.Threading.Tasks;
using log4net;
using System;
using Loki.Bot;
using Loki.Game;
using Loki.Game.GameData;
using Loki.Common;
using Buddy.Coroutines;
using System.IO;
using System.Windows.Markup;
using System.Windows.Data;

namespace ChickenSacFarm
{
    public class ChickenSacFarm : IPlugin
    {


        public static event Action AreaTransitionEntered;
        public static event Action MapExplorationCompleted;

        public static event Action<uint, uint, DatWorldAreaWrapper> AreaChanged;
        public static event Action PlayerDied;



        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private bool _enabled;
        private TaskManager _taskManager;

        public void Start()
        {
            Log.Debug("[ChickenSacFarm] Started");

            _taskManager = (TaskManager)BotManager.CurrentBot.Execute("GetTaskManager");
            
            if (_taskManager == null)
            {
                Log.Error("[MapRunner] Fail to get the global TaskManager.");
                BotManager.Stop();
                return;
            }

            Log.Debug("[ChickenSacFarm] Removendo Tasks");
            RemoveRedundantTasks();

            Log.Debug("[ChickenSacFarm] Adicionando Task Track Corrupted");
            if (!_taskManager.AddBefore(new TrackCorrupted(), "ExploreTask"))
            {
                Log.Error("ChickenSacFarm Add Task Failed");
                BotManager.Stop();
            }

            Log.Debug("[ChickenSacFarm] Adicionando Task Handle Vessel");
            if (!_taskManager.AddBefore(new HandleVesselTask(), "ExplorationCompleteTask"))
            {
                Log.Error("ChickenSacFarm Add Vessel Task Failed");
                BotManager.Stop();
            }

        }

        private void RemoveRedundantTasks()
        {
            RemoveTask("ReturnToGrindZoneTask");
            RemoveTask("TagWaypointTask");
            RemoveTask("UseLooseCandleTask");
            RemoveTask("UnblockCorruptedAreaTransitionTask");
            RemoveTask("TakeCorruptedAreaTask");
            RemoveTask("HandleA2Q9");
            RemoveTask("HandleA2Q9b");
            RemoveTask("HandleA2Q11");
            RemoveTask("HandleA3Q1");
            RemoveTask("HandleSewers");
            RemoveTask("HandleUndyingBlockage");
            RemoveTask("HandleLockedDoor");
            RemoveTask("HandleDeshretSeal");
            RemoveTask("HandleGlyphWall");
            //RemoveTask("TravelToGrindZoneTask");
            RemoveTask("TravelThroughBossAreasTask-BossFarm");
            RemoveTask("EnterArenaTask");
            RemoveTask("TravelThroughBossAreasTask-PassThrough");
            RemoveTask("ExplorationCompleteTask (Early)");
            RemoveTask("TrackMoreMobs");
            RemoveTask("HandleMerveilArea");
            RemoveTask("HandlePietyArea");
            RemoveTask("HandleDominusArea");
            RemoveTask("HandleKaomArea");
            RemoveTask("HandleDaressoArea");
            RemoveTask("HandleWeaverArea");
            RemoveTask("HandleVaalVesselTask");
            

        }

        private void RemoveTask(string name)
        {
            if (!_taskManager.Remove(name))
            {
                Log.ErrorFormat("ChickenSacFarm Fail to remove \"{0}\".", name);
                BotManager.Stop();
            }
        }

        public void Stop()
        {
            Log.DebugFormat("ChickenSacFarm Stopped");
        }

        public void Tick()
        {
           
        }

        public string Author
        {
            get {
                return "ChickenJoe";    
            }
        }

        public string Description
        {
            get {
                return "Plugin to farm sacrifice pieces in docks";    
            }
        }

        public string Name
        {
            get {
                return "ChickenSacFarm";
            }
        }

        public string Version
        {
            get {
                return "0.0.1";
            }
        }

        public void Deinitialize()
        {
          
        }

        public void Initialize()
        {
            Log.DebugFormat("ChickenSacFarm Initialized");
        }

        public object Execute(string name, params dynamic[] param)
        {
            return null;
        }

        public async Task<bool> Logic(string type, params dynamic[] param)
        {
            if (type == "core_area_changed_event")
            {
                uint oldSeed = (uint)param[0];
                uint newSeed = (uint)param[1];
                var newArea = (DatWorldAreaWrapper)param[3];
                EventInvocators.RaiseAreaChangedEvent(oldSeed, newSeed, newArea);

                Log.DebugFormat("Chicken Sac Farm Logic, Area Changed Event");
                
            }
            if (type == "core_player_died_event")
            {
                EventInvocators.RaisePlayerDiedEvent();
            }
            return false;
        }  

        public bool IsEnabled
        {
            get { 
                return _enabled; 
            }
        }

        public void Enable()
        {
            Log.DebugFormat("ChickenSacFarm Enabled");
            _enabled = true;
        }

        public void Disable()
        {
            Log.DebugFormat("ChickenSacFarm Disabled");
            _enabled = false;
        }

        public UserControl Control
        {
            get
            {
                using (var fs = new FileStream(@"3rdParty\ChickenSacFarm\SettingsGui.xaml", FileMode.Open))
                {
                    var root = (UserControl)XamlReader.Load(fs);
                    var instance = ChickenSacFarmSettings.Instance;

                    if (!Wpf.SetupLabelBinding(root, "fragDuskLabel", "fragDusk", BindingMode.OneWay, instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupTextBoxBinding failed for 'fragDuskLabel'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }
                    if (!Wpf.SetupLabelBinding(root, "fragDawnLabel", "fragDawn", BindingMode.OneWay, instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupTextBoxBinding failed for 'fragDawnLabel'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }
                    if (!Wpf.SetupLabelBinding(root, "fragNoonLabel", "fragNoon", BindingMode.OneWay, instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupTextBoxBinding failed for 'fragNoonLabel'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }
                    if (!Wpf.SetupLabelBinding(root, "fragMidnightLabel", "fragMidnight", BindingMode.OneWay, instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupTextBoxBinding failed for 'fragMidnightLabel'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }
                    if (!Wpf.SetupLabelBinding(root, "vaalGemsLabel", "vaalGems", BindingMode.OneWay, instance))
                    {
                        Log.DebugFormat(
                            "[SettingsControl] SetupTextBoxBinding failed for 'vaalGemsLabel'.");
                        throw new Exception("The SettingsControl could not be created.");
                    }

                    return root;
                }
            }
        }

        public JsonSettings Settings
        {
            get {
                return null;
            }
        }

        #region Event invocators

        public static class EventInvocators
        {           

            public static void RaiseAreaTransitionEnteredEvent()
            {
                var handler = AreaTransitionEntered;
                if (handler != null) handler();
            }

            public static void RaiseMapExplorationCompletedEvent()
            {
                var handler = MapExplorationCompleted;
                if (handler != null) handler();
            }

            public static void RaiseAreaChangedEvent(uint oldSeed, uint newSeed, DatWorldAreaWrapper newArea)
            {
                var handler = AreaChanged;
                if (handler != null) handler(oldSeed, newSeed, newArea);
            }

            public static void RaisePlayerDiedEvent()
            {
                var handler = PlayerDied;
                if (handler != null) handler();
            }
        }

        #endregion

    }
}
