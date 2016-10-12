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
using CommunityLib;

namespace Navali
{
    public class SealPropheciesTask : ITask
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private static readonly NavaliSettings Settings = NavaliSettings.Instance;
        static SealPropheciesTask()
        {

        }

        public async Task<bool> Logic(string type, params dynamic[] param)
        {
            if (type != "task_execute") return false;

            //Log.ErrorFormat("[Navali] are we in town, and how many coins do we have ({0}/{1})?", CommunityLib.Data.CachedItemsInStash.Where(d => d.FullName.Equals("Silver Coin")).Sum(s => s.StackCount), Int32.Parse(Settings.NumCoinsMin));

            if ((LokiPoe.Me.IsInTown || LokiPoe.Me.IsInHideout) && (CommunityLib.Data.CachedItemsInStash.Where(d => d.FullName.Equals("Silver Coin")).Sum(s => s.StackCount) >= Int32.Parse(Settings.NumCoinsMin)))
            {
                //Log.ErrorFormat("[Navali] We are in town, and have enough coins to trigger ({0}/{1})?", CommunityLib.Data.CachedItemsInStash.Where(d => d.FullName.Equals("Silver Coin")).Sum(s => s.StackCount), Int32.Parse(Settings.NumCoinsMin));
                if (!CommunityLib.Data.ItemsInStashAlreadyCached)
                {
                    Log.ErrorFormat("[Navali] Need to update Stash cache");
                    await CommunityLib.Data.UpdateItemsInStash(true);
                }
                //Log.ErrorFormat("[Navali] Caching NPCs");
                var npcs = LokiPoe.ObjectManager.Objects
                    .OfType<Npc>()
                    .Where(npc => npc.IsTargetable)
                    .ToList();
                //Log.ErrorFormat("[Navali] We cached {0} NPCs", npcs.Count);
                if (npcs.Count == 0)
                {
                    Log.Error("[Navali] Failed to find any NPCs. Where the hell are you and why did this trigger when you're not in town?");
                    return false;
                }


                //Log.ErrorFormat("[Navali] Now checking how many prophecies we have");
                var propheciesCount = LokiPoe.Me.Prophecies.Count;
                //Log.ErrorFormat("[Navali] We have {0} Prophecies", propheciesCount);
                //Log.ErrorFormat("[Navali] starting sealing, num prophecies: {0}.", propheciesCount);

                if (LokiPoe.Me.Prophecies.Count >= 1)
                {
                    for (var i = 0; i < propheciesCount; i++)
                    {
                        if (!await SealProphecies()) { break;  }
                    }

                }
                return false;    
            }
            return false;
        }
        private async Task<bool> SealProphecies()
        {
            // we have propheciesCount prophecies, after sealing a prophecy we need to re-read them from memory.
            //Log.ErrorFormat("[Navali] Foreach Prophecies");
            var propheciesCount = LokiPoe.Me.Prophecies.Count;
            var navali = "Navali";
            for (int i = 0; i < propheciesCount; i++)
            {
                var that = LokiPoe.Me.Prophecies.ElementAt(i);
                Log.DebugFormat("[Navali] do we want to seal {0}?", that.DatPropheciesWrapper.Name);
                if (checkProphecyToSeal(that.DatPropheciesWrapper.Name))
                {
                    Log.DebugFormat("[Navali] Interacting with Navali");
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
                    var sealProphecy = "Seal Prophecy";
                    var dialog = DialogUi.DialogEntries.FirstOrDefault(d => d.Text.ContainsIgnorecase(sealProphecy))?.Text;
                    if (dialog == null)
                    {
                        Log.ErrorFormat("[Navali] Failed to find any dialog with {0}. Out of currency?", sealProphecy);
                        return false;
                    }
                    var conversed = DialogUi.Converse(dialog);
                    await Coroutine.Sleep(500);
                    if (!LokiPoe.InGameState.ChallengesUi.IsOpened)
                    {
                        Log.Error("[Navali] ChallengesUI is not opened, something went wrong.");
                        return false;
                    }
                    await Coroutine.Sleep(500);
                    var seal = LokiPoe.InGameState.ChallengesUi.SealProphecy(that.Index, true);
                    if (seal.Equals("none")) return false;
                    // Seal Prophecy for XXX coins?
                    if (!LokiPoe.InGameState.GlobalWarningDialog.ConfirmDialog(true))
                    {
                        Log.ErrorFormat("[Navali] Unable to accept Seal, can't continue, abort!");
                        BotManager.Stop();
                        return false;
                    }
                    if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                    {
                        Log.ErrorFormat("[Navali] Inventory is not open, open it");
                        await LibCoroutines.OpenInventoryPanel();
                        await Coroutines.ReactionWait();
                    }
                    var cursorItem = LokiPoe.InGameState.CursorItemOverlay.Item;
                    if (cursorItem != null)
                    {

                        int col, row;
                        if (!LokiPoe.InGameState.InventoryUi.InventoryControl_Main.Inventory.CanFitItem(cursorItem.Size, out col, out row))
                        {
                            BotManager.Stop();
                            return false;

                        }
                        if (!await LokiPoe.InGameState.InventoryUi.InventoryControl_Main.PlaceItemFromCursor(new Vector2i(col, row)))
                        {
                            Log.Error("[Navali] Unable to place item in inventory, now stopping the bot because it cannot continue.");
                            BotManager.Stop();
                            return false;

                        }
                    }
                    else
                    {
                        Log.Error("[Navali] No item on cursor, return");
                        return false;
                    }
                    if (propheciesCount == LokiPoe.Me.Prophecies.Count)
                    {
                        Log.InfoFormat("[Navali] Couldn't seal a prophecy, bail out");
                        return false;

                    }
                    await Coroutines.ReactionWait();
                    await Coroutines.CloseBlockingWindows();
                    return true;
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
            get { return "SealPropheciesTask"; }
        }

        public string Description
        {
            get { return "Task to Seal Prophecies"; }
        }

        public string Author
        {
            get { return "Clandestine"; }
        }
        public string Version
        {
            get { return "0.0.1.0"; }
        }
        public object Execute(string name, params dynamic[] param)
        {
            return null;
        }

        private bool checkProphecyToSeal(string Prophecy)
        {
            switch (Prophecy)
            {
                case "A Gracious Master":
                    if (Settings.m1) return true;
                    break;
                case "A Master Seeks Help":
                    if (Settings.m2) return true;
                    break;
                case "The Sharpened Blade":
                    if (Settings.c1) return true;
                    break;
                case "The Hardened Armour":
                    if (Settings.c2) return true;
                    break;
                case "The Beautiful Guide":
                    if (Settings.c3) return true;
                    break;
                case "Erasmus' Gift":
                    if (Settings.c4) return true;
                    break;
                case "A Valuable Combination":
                    if (Settings.c5) return true;
                    break;
                case "The Jeweller's Touch":
                    if (Settings.c6) return true;
                    break;
                case "Fated Connections":
                    if (Settings.c7) return true;
                    break;
                case "Trash to Treasure":
                    if (Settings.c8) return true;
                    break;
                case "Golden Touch":
                    if (Settings.c9) return true;
                    break;
                case "Vital Transformation":
                    if (Settings.c10) return true;
                    break;
                case "Resistant to Change":
                    if (Settings.c11) return true;
                    break;
                case "Touched by the Wind":
                    if (Settings.c12) return true;
                    break;
                case "The King's Path":
                    if (Settings.f1) return true;
                    break;
                case "The Karui Rebellion":
                    if (Settings.f2) return true;
                    break;
                case "The King and the Brambles":
                    if (Settings.f3) return true;
                    break;
                case "The Flow of Energy":
                    if (Settings.f4) return true;
                    break;
                case "The Bowstring's Music":
                    if (Settings.f5) return true;
                    break;
                case "From The Void":
                    if (Settings.f6) return true;
                    break;
                case "Heavy Blows":
                    if (Settings.f7) return true;
                    break;
                case "Fire and Brimstone":
                    if (Settings.f8) return true;
                    break;
                case "A Forest of False Idols":
                    if (Settings.f9) return true;
                    break;
                case "Fire and Ice":
                    if (Settings.f10) return true;
                    break;
                case "Pleasure and Pain":
                    if (Settings.f11) return true;
                    break;
                case "The Bloody Flowers Redux":
                    if (Settings.f12) return true;
                    break;
                case "Dying Cry":
                    if (Settings.f13) return true;
                    break;
                case "Mouth of Horrors":
                    if (Settings.f14) return true;
                    break;
                case "Nature's Resilience":
                    if (Settings.f15) return true;
                    break;
                case "The Snuffed Flame":
                    if (Settings.f16) return true;
                    break;
                case "The Apex Predator":
                    if (Settings.f17) return true;
                    break;
                case "Severed Limbs":
                    if (Settings.f18) return true;
                    break;
                case "Ancient Doom":
                    if (Settings.f19) return true;
                    break;
                case "The Silverwood":
                    if (Settings.f20) return true;
                    break;
                case "The Servant's Heart":
                    if (Settings.f21) return true;
                    break;
                case "Winter's Mournful Melodies":
                    if (Settings.f22) return true;
                    break;
                case "Power Magnified":
                    if (Settings.f23) return true;
                    break;
                case "The Beginning and the End":
                    if (Settings.f24) return true;
                    break;
                case "The Misunderstood Queen":
                    if (Settings.f25) return true;
                    break;
                case "The Singular Spirit":
                    if (Settings.e0) return true;
                    break;
                case "Nemesis of Greed":
                    if (Settings.e1) return true;
                    break;
                case "The Blacksmith":
                    if (Settings.e2) return true;
                    break;
                case "The Walking Mountain":
                    if (Settings.e3) return true;
                    break;
                case "The Prison Guard":
                    if (Settings.e4) return true;
                    break;
                case "The Sword King's Passion":
                    if (Settings.e5) return true;
                    break;
                case "The Eagle's Cry":
                    if (Settings.e6) return true;
                    break;
                case "From Death Springs Life":
                    if (Settings.e7) return true;
                    break;
                case "Storm on the Horizon":
                    if (Settings.e8) return true;
                    break;
                case "A Firm Foothold":
                    if (Settings.e9) return true;
                    break;
                case "The Ward's Ward":
                    if (Settings.e10) return true;
                    break;
                case "Fear's Wide Reach":
                    if (Settings.e11) return true;
                    break;
                case "The Prison Key":
                    if (Settings.e12) return true;
                    break;
                case "Roth's Legacy":
                    if (Settings.e13) return true;
                    break;
                case "The Soulless Beast":
                    if (Settings.e14) return true;
                    break;
                case "The Sinner's Stone":
                    if (Settings.e15) return true;
                    break;
                case "Baptism by Death":
                    if (Settings.e16) return true;
                    break;
                case "The Lady in Black":
                    if (Settings.e17) return true;
                    break;
                case "Against the Tide":
                    if (Settings.e18) return true;
                    break;
                case "The Brutal Enforcer":
                    if (Settings.e19) return true;
                    break;
                case "Graceful Flames":
                    if (Settings.e20) return true;
                    break;
                case "Storm on the Shore":
                    if (Settings.e21) return true;
                    break;
                case "The Petrified":
                    if (Settings.e22) return true;
                    break;
                case "The Vanguard":
                    if (Settings.e23) return true;
                    break;
                case "Cleanser of Sins":
                    if (Settings.e24) return true;
                    break;
                case "Strong as a Bull":
                    if (Settings.e25) return true;
                    break;
                case "The Lost Undying":
                    if (Settings.e26) return true;
                    break;
                case "A Whispered Prayer":
                    if (Settings.e27) return true;
                    break;
                case "The Last Watch":
                    if (Settings.e28) return true;
                    break;
                case "A Prodigious Hand":
                    if (Settings.e29) return true;
                    break;
                case "The Watcher's Watcher":
                    if (Settings.e30) return true;
                    break;
                case "Blood of the Betrayed":
                    if (Settings.e31) return true;
                    break;
                case "Heart of the Fire":
                    if (Settings.e32) return true;
                    break;
                case "Notched Flesh":
                    if (Settings.e33) return true;
                    break;
                case "Custodians of Silence":
                    if (Settings.e34) return true;
                    break;
                case "Flesh of the Beast":
                    if (Settings.e35) return true;
                    break;
                case "Fire, Wood and Stone":
                    if (Settings.e36) return true;
                    break;
                case "Abnormal Effulgence":
                    if (Settings.e37) return true;
                    break;
                case "Wind and Thunder":
                    if (Settings.e38) return true;
                    break;
                case "Lost in the Pages":
                    if (Settings.e39) return true;
                    break;
                case "Weeping Death":
                    if (Settings.e40) return true;
                    break;
                case "The Flayed Man":
                    if (Settings.e41) return true;
                    break;
                case "The Hollow Pledge":
                    if (Settings.e42) return true;
                    break;
                case "A Call into the Void":
                    if (Settings.e43) return true;
                    break;
                case "Blood in the Eyes":
                    if (Settings.e44) return true;
                    break;
                case "The Nest":
                    if (Settings.e45) return true;
                    break;
                case "The Mysterious Gift":
                    if (Settings.e46) return true;
                    break;
                case "The Queen's Vaults":
                    if (Settings.e47) return true;
                    break;
                case "Overflowing Riches":
                    if (Settings.s0) return true;
                    break;
                case "Erased from Memory":
                    if (Settings.s1) return true;
                    break;
                case "The Alchemist":
                    if (Settings.s2) return true;
                    break;
                case "A Regal Death":
                    if (Settings.s3) return true;
                    break;
                case "The Corrupt":
                    if (Settings.s4) return true;
                    break;
                case "The God of Misfortune":
                    if (Settings.s5) return true;
                    break;
                case "Deadly Twins":
                    if (Settings.n0) return true;
                    break;
                case "Living Fires":
                    if (Settings.n1) return true;
                    break;
                case "Fallow At Last":
                    if (Settings.n2) return true;
                    break;
                case "Path of Betrayal":
                    if (Settings.n3) return true;
                    break;
                case "The Forgotten Soldiers":
                    if (Settings.n4) return true;
                    break;
                case "Pools of Wealth":
                    if (Settings.n5) return true;
                    break;
                case "The Child of Lunaris":
                    if (Settings.n6) return true;
                    break;
                case "The Forgotten Garrison":
                    if (Settings.n7) return true;
                    break;
                case "The Stockkeeper":
                    if (Settings.n8) return true;
                    break;
                case "Unnatural Energy":
                    if (Settings.n9) return true;
                    break;
                case "Holding the Bridge":
                    if (Settings.n10) return true;
                    break;
                case "In the Grasp of Corruption":
                    if (Settings.n11) return true;
                    break;
                case "Defiled in the Sceptre":
                    if (Settings.n12) return true;
                    break;
                case "The Twins":
                    if (Settings.n13) return true;
                    break;
                case "Possessed Foe":
                    if (Settings.n14) return true;
                    break;
                case "Vaal Invasion":
                    if (Settings.n15) return true;
                    break;
                case "Monstrous Treasure":
                    if (Settings.n16) return true;
                    break;
                case "Visions of the Drowned":
                    if (Settings.n17) return true;
                    break;
                case "Soil, Worms and Blood":
                    if (Settings.n18) return true;
                    break;
                case "The Hungering Swarm":
                    if (Settings.n19) return true;
                    break;
                case "The Cursed Choir":
                    if (Settings.n20) return true;
                    break;
                case "The Trembling Earth":
                    if (Settings.n21) return true;
                    break;
                case "Ending the Torment":
                    if (Settings.n22) return true;
                    break;
                case "Hidden Reinforcements":
                    if (Settings.n23) return true;
                    break;
                case "Rebirth":
                    if (Settings.n24) return true;
                    break;
                case "An Unseen Peril":
                    if (Settings.n25) return true;
                    break;
                case "Mysterious Invaders":
                    if (Settings.n26) return true;
                    break;
                case "The Invader":
                    if (Settings.n27) return true;
                    break;
                case "Undead Uprising":
                    if (Settings.n28) return true;
                    break;
                case "Waiting in Ambush":
                    if (Settings.n29) return true;
                    break;
                case "Forceful Exorcism":
                    if (Settings.n30) return true;
                    break;
                case "The Dreamer's Dream":
                    if (Settings.n31) return true;
                    break;
                case "The Aesthete's Spirit":
                    if (Settings.n32) return true;
                    break;
                case "Brothers in Arms":
                    if (Settings.n33) return true;
                    break;
                case "Echoes of Witchcraft":
                    if (Settings.n34) return true;
                    break;
                case "Echoes of Mutation":
                    if (Settings.n35) return true;
                    break;
                case "Echoes of Lost Love":
                    if (Settings.n36) return true;
                    break;
                case "Hidden Vaal Pathways":
                    if (Settings.n37) return true;
                    break;
                case "The Undead Brutes":
                    if (Settings.n38) return true;
                    break;
                case "The Four Feral Exiles":
                    if (Settings.n39) return true;
                    break;
                case "Risen Blood":
                    if (Settings.n40) return true;
                    break;
                case "The Brothers of Necromancy":
                    if (Settings.n41) return true;
                    break;
                case "Reforged Bonds":
                    if (Settings.n42) return true;
                    break;
                case "The Wealthy Exile":
                    if (Settings.o0) return true;
                    break;
                case "The Scout":
                    if (Settings.o1) return true;
                    break;
                case "Hunter's Lesson":
                    if (Settings.o2) return true;
                    break;
                case "Gilded Within":
                    if (Settings.o3) return true;
                    break;
                case "Kalandra's Craft":
                    if (Settings.o4) return true;
                    break;
                case "Bountiful Traps":
                    if (Settings.o5) return true;
                    break;
                case "The Unbreathing Queen I":
                    if (Settings.p0) return true;
                    break;
                case "The Unbreathing Queen II":
                    if (Settings.p1) return true;
                    break;
                case "The Unbreathing Queen III":
                    if (Settings.p2) return true;
                    break;
                case "The Unbreathing Queen IV":
                    if (Settings.p3) return true;
                    break;
                case "The Unbreathing Queen V":
                    if (Settings.p4) return true;
                    break;
                case "Unbearable Whispers I":
                    if (Settings.p5) return true;
                    break;
                case "Unbearable Whispers II":
                    if (Settings.p6) return true;
                    break;
                case "Unbearable Whispers III":
                    if (Settings.p7) return true;
                    break;
                case "Unbearable Whispers IV":
                    if (Settings.p8) return true;
                    break;
                case "Unbearable Whispers V":
                    if (Settings.p9) return true;
                    break;
                case "The Plaguemaw I":
                    if (Settings.p10) return true;
                    break;
                case "The Plaguemaw II":
                    if (Settings.p11) return true;
                    break;
                case "The Plaguemaw III":
                    if (Settings.p12) return true;
                    break;
                case "The Plaguemaw IV":
                    if (Settings.p13) return true;
                    break;
                case "The Plaguemaw V":
                    if (Settings.p14) return true;
                    break;
                case "The Feral Lord I":
                    if (Settings.p15) return true;
                    break;
                case "The Feral Lord II":
                    if (Settings.p16) return true;
                    break;
                case "The Feral Lord III":
                    if (Settings.p17) return true;
                    break;
                case "The Feral Lord IV":
                    if (Settings.p18) return true;
                    break;
                case "The Feral Lord V":
                    if (Settings.p19) return true;
                    break;
                case "The Ambitious Bandit I":
                    if (Settings.p20) return true;
                    break;
                case "The Ambitious Bandit II":
                    if (Settings.p21) return true;
                    break;
                case "The Ambitious Bandit III":
                    if (Settings.p22) return true;
                    break;
                case "Deadly Rivalry I":
                    if (Settings.p23) return true;
                    break;
                case "Deadly Rivalry II":
                    if (Settings.p24) return true;
                    break;
                case "Deadly Rivalry III":
                    if (Settings.p25) return true;
                    break;
                case "Deadly Rivalry IV":
                    if (Settings.p26) return true;
                    break;
                case "Deadly Rivalry V":
                    if (Settings.p27) return true;
                    break;
                case "The Warmongers I":
                    if (Settings.p28) return true;
                    break;
                case "The Warmongers II":
                    if (Settings.p29) return true;
                    break;
                case "The Warmongers III":
                    if (Settings.p30) return true;
                    break;
                case "The Warmongers IV":
                    if (Settings.p31) return true;
                    break;
                case "Beyond Sight I":
                    if (Settings.p32) return true;
                    break;
                case "Beyond Sight II":
                    if (Settings.p33) return true;
                    break;
                case "Beyond Sight III":
                    if (Settings.p34) return true;
                    break;
                case "Beyond Sight IV":
                    if (Settings.p35) return true;
                    break;
                case "Thaumaturgical History I":
                    if (Settings.p36) return true;
                    break;
                case "Thaumaturgical History II":
                    if (Settings.p37) return true;
                    break;
                case "Thaumaturgical History III":
                    if (Settings.p38) return true;
                    break;
                case "Thaumaturgical History IV":
                    if (Settings.p39) return true;
                    break;
                case "Ancient Rivalries I":
                    if (Settings.p40) return true;
                    break;
                case "Ancient Rivalries II":
                    if (Settings.p41) return true;
                    break;
                case "Ancient Rivalries III":
                    if (Settings.p42) return true;
                    break;
                case "Ancient Rivalries IV":
                    if (Settings.p43) return true;
                    break;
                case "Anarchy's End I":
                    if (Settings.p44) return true;
                    break;
                case "Anarchy's End II":
                    if (Settings.p45) return true;
                    break;
                case "Anarchy's End III":
                    if (Settings.p46) return true;
                    break;
                case "Anarchy's End IV":
                    if (Settings.p47) return true;
                    break;
                case "Day of Sacrifice I":
                    if (Settings.p48) return true;
                    break;
                case "Day of Sacrifice II":
                    if (Settings.p49) return true;
                    break;
                case "Day of Sacrifice III":
                    if (Settings.p50) return true;
                    break;
                case "Day of Sacrifice IV":
                    if (Settings.p51) return true;
                    break;
                case "Fire from the Sky":
                    if (Settings.t0) return true;
                    break;
                case "Ice from Above":
                    if (Settings.t1) return true;
                    break;
                case "Lightning Falls":
                    if (Settings.t2) return true;
                    break;
                case "The Undead Storm":
                    if (Settings.t3) return true;
                    break;
                case "Crushing Squall":
                    if (Settings.t4) return true;
                    break;
                case "Vaal Winds":
                    if (Settings.t5) return true;
                    break;
                case "The Fortune Teller's Collection":
                    if (Settings.x0) return true;
                    break;
                case "Lasting Impressions":
                    if (Settings.x1) return true;
                    break;
                case "Plague of Rats":
                    if (Settings.x2) return true;
                    break;
                case "Plague of Frogs":
                    if (Settings.x3) return true;
                    break;
                case "Twice Enchanted":
                    if (Settings.x4) return true;
                    break;
                case "The Dream Trial":
                    if (Settings.x5) return true;
                    break;
                case "The Lost Maps":
                    if (Settings.x6) return true;
                    break;
                case "Smothering Tendrils":
                    if (Settings.x7) return true;
                    break;
                default:
                    return false;
            }
            return false;
        }
    }
}