using log4net;
using Loki.Bot;
using Loki.Bot.Pathfinding;
using Loki.Common;
using Loki.Game;
using System.Threading.Tasks;
using Buddy.Coroutines;
using System.Diagnostics;
using CommunityLib;

namespace ChickenSacFarm
{
    class TrackCorrupted : ITask
    {
        private readonly Stopwatch swFrags = new Stopwatch();

        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private bool _enabled = true;
        public bool _hasCorrupted = false;
        private const int Range = 20;
        
        public void Start()
        {
            
        }

       
        public void Stop()
        {
            
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
                return "Task for help BasicGrindBot to search and move to corrupted area in The Docks";
            }
        }

        public string Name
        {
            get {
                return "TrackCorrupted Task";
            }
        }

        public string Version
        {
            get {
                return "0.0.1";
            }
        }

        public object Execute(string name, params dynamic[] param){

            

            return null;
        }

        public async Task<bool> Logic(string type, params dynamic[] param){

            Log.DebugFormat("Starting Logic TrackCorrupted");                       

            if (type == "core_area_changed_event")
            {
                Log.DebugFormat("-------------Variavel Corrupt = False");
                _hasCorrupted = false;
            }

            if (type == "oldgrindbot_on_loot")
            {
                BasicGrindBot_OnLoot(param[0]);
            }

            if (LokiPoe.CurrentWorldArea.Name != "The Docks") { 
                return false;
            }           

            Log.DebugFormat("Verificando Area Corrupta");

            LokiPoe.TerrainDataEntry[,] tgtEntries = LokiPoe.TerrainData.TgtEntries;
            for (int i = 0; i < tgtEntries.GetLength(0); i++)
            {
                for (int j = 0; j < tgtEntries.GetLength(1); j++)
                {
                    LokiPoe.TerrainDataEntry terrainDataEntry = tgtEntries[i, j];
                    if (terrainDataEntry != null)
                    {
                        if (terrainDataEntry.TgtName == "Art/Models/Terrain/Act3/AreaTransitions/act3_docks_v01_01_c1r1.tgt")
                        {
                            _hasCorrupted = true;
                            if (LokiPoe.Me.Position.Distance(FindWalkablePathToTerrainObject(i, j)) > Range)
                            {
                                if (!await CoroutinesV3.MoveToLocation(FindWalkablePathToTerrainObject(i, j), 10, 60000, () => false))
                                {
                                    Log.ErrorFormat("[TrackCorrupted] MoveTowards failed for {0}.", FindWalkablePathToTerrainObject(i, j));
                                }
                                else
                                {
                                    Log.InfoFormat("[TrackCorrupted] We have arrived.");
                                    await CoroutinesV3.TakeAreaTransition(LokiPoe.ObjectManager.CorruptedAreaTransition(), false, -1);                        
                                }

                            }
                            else
                            {
                                Log.InfoFormat("[TrackCorrupted] We have arrived.");
                                await CoroutinesV3.TakeAreaTransition(LokiPoe.ObjectManager.CorruptedAreaTransition(), false, -1);
                            }
                        }
                    }
                }
            }
            if (!_hasCorrupted)
            {
                Log.InfoFormat("[TrackCorrupted] This dock has no corrupted area, remaking instance.");
                await CoroutinesV3.TakeWaypointTo("3_3_9", true, -1);                                
            }

            return _hasCorrupted;
       
        }

        private Vector2i FindWalkablePathToTerrainObject(int i, int j)
        {
            Vector2i pos = new Vector2i(i * 23 + 10, j * 23 + 10);
            return ExilePather.FastWalkablePositionFor(pos, 10);

        }

        public bool IsEnabled
        {
            get { return _enabled; }
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        void BasicGrindBot_OnLoot(string iName)
        {
            if (iName.Contains("Sacrifice at Dusk"))
                ChickenSacFarmSettings.Instance.IncreaseDusk();
            if (iName.Contains("Sacrifice at Dawn"))
                ChickenSacFarmSettings.Instance.IncreaseDawn();
            if (iName.Contains("Sacrifice at Noon"))
                ChickenSacFarmSettings.Instance.IncreaseNoon();
            if (iName.Contains("Sacrifice at Midnight"))
                ChickenSacFarmSettings.Instance.IncreaseMidnight();
        }


    }
}
