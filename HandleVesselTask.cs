using Buddy.Coroutines;
using log4net;
using Loki.Bot;
using Loki.Common;
using Loki.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenSacFarm
{
    public class HandleVesselTask : ITask
    {

        public bool openVaalVessel = true;
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();

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
                return "Task for handle vaal vessel";
            }
        }

        public string Name
        {
            get {
                return "Handle Vaal Vessel";
            }
        }

        public string Version
        {
            get {
                return "0.0.1";
            }
        }

        public object Execute(string name, params dynamic[] param)
        {
            return null;
        }

        public async Task<bool> Logic(string type, params dynamic[] param)
        {
            Log.DebugFormat("Handle Vessel Logic");

            if (type == "core_area_changed_event")
            {
                Log.DebugFormat("Core Area Changed, Open Vessel temos que abrir a caixinha!!");
                openVaalVessel = true;
            }

            if (type != "task_execute") 
                return false;

            if(!openVaalVessel){
                return false;
            }
            

            if (LokiPoe.CurrentWorldArea.IsCorruptedArea)
            {
                Log.DebugFormat("Estamos na Area Corrupta!");
                if (openVaalVessel)
                {
                    Log.DebugFormat("Precisamos Abrir o Vessel!");

                    var vessel = LokiPoe.ObjectManager.GetObjectByName("Vaal Vessel");

                    if(vessel == null){
                        return true;
                    }

                    while(vessel.Distance > 15){
                        if(!PlayerMover.MoveTowards(vessel.Position)){
                            Log.Error("[Handle Vessel Task] Fail to move to the Vaal Vessel");
                            return true;
                        }
                        await Coroutine.Sleep(100);
                    }

                    bool isInteracted = await Coroutines.InteractWith(vessel);

                    await Coroutine.Sleep(1000);

                    bool opened = vessel.Components.ChestComponent.IsOpened;

                    if(opened){
                        Log.DebugFormat("Yey abrimos o Vaal Vessel");
                        openVaalVessel = false;
                    }
                    else
                    {
                        Log.DebugFormat("Ohh, nos temos um problema! o vessel nao foi aberto! :(");
                        return true;
                    }                    

                }

            }

            return true;
     
        }
    }
}
