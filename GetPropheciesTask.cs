using System;
using System.Threading.Tasks;
using System.Linq;
using log4net;
using Loki.Bot;
using Loki.Common;
using Loki.Game;
using Loki.Game.Objects;
using Buddy.Coroutines;
using DialogUi = Loki.Game.LokiPoe.InGameState.NpcDialogUi;
using EXtensions;


namespace Navali
{
    public class GetPropheciesTask : ITask
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private static readonly NavaliSettings Settings = NavaliSettings.Instance;
        static GetPropheciesTask()
        {

        }

        public async Task<bool> Logic(string type, params dynamic[] param)
        {
            if (type != "task_execute") return false;
            var navali = "Navali";

            if ((LokiPoe.Me.IsInTown || LokiPoe.Me.IsInHideout) && (CommunityLib.Data.CachedItemsInStash.Where(d => d.FullName.Equals("Silver Coin")).Sum(s => s.StackCount) >= Int32.Parse(Settings.NumCoinsMin)))
            {
                if (!CommunityLib.Data.ItemsInStashAlreadyCached)
                {
                    Log.ErrorFormat("[Navali] Need to Cache Stash");
                    await CommunityLib.Data.UpdateItemsInStash(true);
                }
                Log.InfoFormat("[Navali] We have {0} coins and need {1} coins to seek Prophecies", CommunityLib.Data.CachedItemsInStash.Where(d => d.FullName.Equals("Silver Coin")).Sum(s => s.StackCount), Int32.Parse(Settings.NumCoinsMin));

                var npcs = LokiPoe.ObjectManager.Objects
                    .OfType<Npc>()
                    .Where(npc => npc.IsTargetable)
                    .ToList();

                if (npcs.Count == 0)
                {
                    Log.Error("[Navali] Failed to find any NPCs. Where the hell are you and why did this trigger when you're not in town?");
                    return false;
                }
                var propheciesCount = LokiPoe.Me.Prophecies.Count;
                var numProphecies = (7 - LokiPoe.Me.Prophecies.Count);
                Log.InfoFormat("[Navali] Now going to get {0} Prophecies", numProphecies);
                if (numProphecies != -1)
                {
                    for (var i = 0; i < numProphecies; i++)
                    {
                        var interacted = await CoroutinesV3.TalkToNpc(navali);
                        if (interacted != CoroutinesV3.TalkToNpcError.None)
                        {
                            Log.ErrorFormat("[Navali] Failed to talk to {0}. Error: \"{1}\".", navali, interacted);
                            return false;
                        }

                        if (!await CoroutinesV3.WaitForNpcDialogPanel(5000))
                        {
                            Log.ErrorFormat("[Navali] Waiting for npc dialog panel timed out.");
                            return false;
                        }

                        var seekProphecy = "Seek Prophecy";
                        var dialog = DialogUi.DialogEntries.FirstOrDefault(d => d.Text.ContainsIgnorecase(seekProphecy))?.Text;
                        if (dialog == null)
                        {
                            Log.ErrorFormat("[Navali] Failed to find any dialog with {0}. Out of currency?", seekProphecy);
                            return false;
                        }

                        var conversed = DialogUi.Converse(dialog);
                        await Coroutine.Sleep(500);
                    }

                }
                else
                {
                    Log.InfoFormat("[Navali] No open Prophecy spots, carry on");
                    return false;
                }
            }
            return false;
        }
        public void Start()
        {
        }

        public void Tick()
        {
        }

        public void Stop()
        {
        }
        public string Name
        {
            get { return "GetPropheciesTask"; }
        }

        public string Description
        {
            get { return "Task to get Prophecies from Navali."; }
        }

        public string Author
        {
            get { return "Clandestine"; }
        }
        public string Version
        {
            get { return "0.0.1.1"; }
        }
        public object Execute(string name, params dynamic[] param)
        {
            return null;
        }
    }

}