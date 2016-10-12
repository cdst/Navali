using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buddy.Coroutines;
using log4net;
using Loki.Bot;
using Loki.Bot.Pathfinding;
using Loki.Common;
using Loki.Common.MVVM;
using Loki.Game;
using Loki.Game.GameData;
using Loki.Game.NativeWrappers;
using Loki.Game.Objects;

namespace Navali
{

    /// <summary>
    /// A wrapper class for settings bindings.
    /// </summary>
    public class SettingNameEnabledWrapper : NotificationObject
    {
        private bool _isEnabled;
        private string _name;

        /// <summary>
        /// Is this setting enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (value.Equals(_isEnabled))
                {
                    return;
                }
                _isEnabled = value;
                NotifyPropertyChanged(() => IsEnabled);
            }
        }

        /// <summary>
        /// The name of this setting.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                NotifyPropertyChanged(() => Name);
            }
        }
    }

    /// <summary>
    /// A delegate for triggering an item being looted.
    /// </summary>
    /// <param name="item"></param>
    public delegate void TriggerOnLootDelegate(WorldItem item);

    /// <summary>
    /// A delegate for triggering an item being stashed.
    /// </summary>
    /// <param name="item"></param>
    public delegate void TriggerOnStashDelegate(Item item);

    /// <summary>A class for commonly used coroutines.</summary>
    public static class CoroutinesV3
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();

        /// <summary>
        /// Opens the skills panel and will NOT close the skill reset dialog if present.
        /// </summary>
        /// <returns>True on success and false on failure.</returns>
        public static async Task<bool> OpenSkillsPanel(int timeout = 5000)
        {
            // TODO: Readd this API
            return false;

            /*Log.DebugFormat("[OpenSkillsPanel]");

			var sw = Stopwatch.StartNew();

			// Make sure we close all blocking windows so we can actually open the inventory.
			if (!LokiPoe.InGameState.SkillsPanel.IsOpened)
			{
				await CloseBlockingWindows();
			}

			// Open the passive skills panel like a player would.
			while (!LokiPoe.InGameState.SkillsPanel.IsOpened)
			{
				Log.DebugFormat("[OpenSkillsPanel] The SkillsPanel is not opened. Now opening it.");

				if (sw.ElapsedMilliseconds > timeout)
				{
					Log.ErrorFormat("[OpenSkillsPanel] Timeout.");
					return false;
				}

				if (LokiPoe.Me.IsDead)
				{
					Log.ErrorFormat("[OpenSkillsPanel] We are now dead.");
					return false;
				}

				LokiPoe.Input.SimulateKeyEvent(LokiPoe.Input.Binding.open_passive_skills_panel, true, false, false);

				await Coroutines.ReactionWait();
			}

			return true;*/
        }

        /// <summary>
        /// Opens the social panel.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> OpenSocialPanel(int timeout = 5000)
        {
            return false;
            // TODO: Readd this API.
            /*
			Log.DebugFormat("[OpenSocialPanel]");

			var sw = Stopwatch.StartNew();

			// Make sure we close all blocking windows so we can actually open the inventory.
			if (!LokiPoe.InGameState.SocialUi.IsOpened)
			{
				await CloseBlockingWindows();
			}

			// Open the social panel like a player would.
			while (!LokiPoe.InGameState.SocialUi.IsOpened)
			{
				Log.DebugFormat("[OpenSocialPanel] The SocialPanel is not opened. Now opening it.");

				if (sw.ElapsedMilliseconds > timeout)
				{
					Log.ErrorFormat("[OpenSocialPanel] Timeout.");
					return false;
				}

				if (LokiPoe.Me.IsDead)
				{
					Log.ErrorFormat("[OpenSocialPanel] We are now dead.");
					return false;
				}

				LokiPoe.Input.SimulateKeyEvent(LokiPoe.Input.Binding.open_social_panel, true, false, false);

				await Coroutines.ReactionWait();
			}

			return true;
			*/
        }

        /// <summary>
        /// Opens the inventory panel.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> OpenInventoryPanel(int timeout = 5000)
        {
            Log.DebugFormat("[OpenInventoryPanel]");

            var sw = Stopwatch.StartNew();

            // Make sure we close all blocking windows so we can actually open the inventory.
            if (!LokiPoe.InGameState.InventoryUi.IsOpened)
            {
                await Coroutines.CloseBlockingWindows();
            }

            // Open the inventory panel like a player would.
            while (!LokiPoe.InGameState.InventoryUi.IsOpened)
            {
                Log.DebugFormat("[OpenInventoryPanel] The InventoryUi is not opened. Now opening it.");

                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[OpenInventoryPanel] Timeout.");
                    return false;
                }

                if (LokiPoe.Me.IsDead)
                {
                    Log.ErrorFormat("[OpenInventoryPanel] We are now dead.");
                    return false;
                }

                LokiPoe.Input.SimulateKeyEvent(LokiPoe.Input.Binding.open_inventory_panel, true, false, false);

                await Coroutines.ReactionWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the character to not be dead anymore.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on success, and false on failure.</returns>
        public static async Task<bool> WaitForResurrect(int timeout = 5000)
        {
            Log.DebugFormat("[WaitForResurrect]");

            var sw = Stopwatch.StartNew();

            while (LokiPoe.Me.IsDead)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForResurrect] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForResurrect] We have been waiting {0} for the character to resurrect.",
                    sw.Elapsed);

                await Coroutines.LatencyWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutines waits for the character to change positions from a local area transition.
        /// </summary>
        /// <param name="position">The starting position.</param>
        /// <param name="delta">The change in position required.</param>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on success, and false on failure.</returns>
        public static async Task<bool> WaitForPositionChange(Vector2i position, int delta = 30, int timeout = 5000)
        {
            Log.DebugFormat("[WaitForPositionChange]");

            var sw = Stopwatch.StartNew();

            while (LokiPoe.MyPosition.Distance(position) < delta)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForLargerPositionChange] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForLargerPositionChange] We have been waiting {0} for an area change.", sw.Elapsed);

                await Coroutines.LatencyWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutines waits for the character to change areas.
        /// </summary>
        /// <param name="original">The starting area's hash.</param>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on success, and false on failure.</returns>
        public static async Task<bool> WaitForAreaChange(uint original, int timeout = 30000)
        {
            Log.DebugFormat("[WaitForAreaChange]");

            var sw = Stopwatch.StartNew();

            while (LokiPoe.LocalData.AreaHash == original)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForAreaChange] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForAreaChange] We have been waiting {0} for an area change.", sw.Elapsed);

                await Coroutines.LatencyWait();
                await Coroutine.Sleep(1000);
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the instance manager to open.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForInstanceManager(int timeout = 1000)
        {
            Log.DebugFormat("[WaitForInstanceManager]");

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.InGameState.InstanceManagerUi.IsOpened)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForInstanceManager] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForInstanceManager] We have been waiting {0} for the instance manager to open.",
                    sw.Elapsed);

                await Coroutines.LatencyWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for an area transition to be usable.
        /// </summary>
        /// <param name="name">The name of the area transition.</param>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForAreaTransition(string name, int timeout = 3000)
        {
            Log.DebugFormat("[WaitForAreaTransition]");

            var sw = Stopwatch.StartNew();

            while (true)
            {
                var at = LokiPoe.ObjectManager.GetObjectByName<AreaTransition>(name);
                if (at != null)
                {
                    if (at.IsTargetable)
                    {
                        break;
                    }
                }

                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForAreaTransition] Timeout.");
                    return false;
                }

                Log.DebugFormat(
                    "[WaitForAreaTransition] We have been waiting {0} for the area transition {1} to be usable.",
                    sw.Elapsed, name);

                await Coroutines.LatencyWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the world panel to open after clicking on a waypoint.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForWorldPanel(int timeout = 10000)
        {
            Log.DebugFormat("[WaitForWorldPanel]");

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.InGameState.WorldUi.IsOpened)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForWorldPanel] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForWorldPanel] We have been waiting {0} for the world panel to open.", sw.Elapsed);

                await Coroutines.ReactionWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the npc dialog panel to open after clicking on a npc to interact with it.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForNpcDialogPanel(int timeout = 3000)
        {
            Log.DebugFormat("[WaitForNpcDialogPanel]");

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.InGameState.NpcDialogUi.IsOpened)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForNpcDialogPanel] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForNpcDialogPanel] We have been waiting {0} for the npc dialog panel to open.", sw.Elapsed);

                await Coroutines.ReactionWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the sell panel to open after clicking Sell Items.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForSellPanel(int timeout = 5000)
        {
            Log.DebugFormat("[WaitForSellPanel]");

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.InGameState.SellUi.IsOpened)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForSellPanel] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForSellPanel] We have been waiting {0} for the sell panel to open.", sw.Elapsed);

                await Coroutines.ReactionWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the purchase panel to open after buy interaction.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForPurchasePanel(int timeout = 1000)
        {
            Log.DebugFormat("[WaitForPurchasePanel]");

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.InGameState.PurchaseUi.IsOpened)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForPurchasePanel] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForPurchasePanel] We have been waiting {0} for the stash panel to open.", sw.Elapsed);

                await Coroutines.ReactionWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the stash panel to open after stash interaction.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForStashPanel(int timeout = 3000)
        {
            Log.DebugFormat("[WaitForStashPanel]");

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.InGameState.StashUi.IsOpened)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForStashPanel] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForStashPanel] We have been waiting {0} for the stash panel to open.", sw.Elapsed);

                await Coroutines.ReactionWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine waits for the guild stash panel to open after guild stash interaction.
        /// </summary>
        /// <param name="timeout">How long to wait before the coroutine fails.</param>
        /// <returns>true on succes and false on failure.</returns>
        public static async Task<bool> WaitForGuildStashPanel(int timeout = 3000)
        {
            Log.DebugFormat("[WaitForGuildStashPanel]");

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.InGameState.GuildStashUi.IsOpened)
            {
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[WaitForGuildStashPanel] Timeout.");
                    return false;
                }

                Log.DebugFormat("[WaitForGuildStashPanel] We have been waiting {0} for the stash panel to open.", sw.Elapsed);

                await Coroutines.ReactionWait();
            }

            return true;
        }

        /// <summary>
        /// This coroutine will create a portal to town and take it. If the process fails, the coroutine will logout.
        /// </summary>
        /// <returns></returns>
        public static async Task CreateAndTakePortalToTown()
        {
            if (await CreatePortalToTown())
            {
                if (await TakeClosestPortal())
                {
                    return;
                }
            }

            Log.ErrorFormat(
                "[CreateAndTakePortalToTown] A portal to town could not be made/taken. Now logging out to get back to town.");

            await LogoutToTitleScreen();
        }

        /// <summary>
        /// This coroutine creates a portal to town from a Portal Scroll in the inventory.
        /// </summary>
        /// <returns>true if the Portal Scroll was used and false otherwise.</returns>
        public static async Task<bool> CreatePortalToTown()
        {
            if (LokiPoe.Me.IsInTown)
            {
                Log.ErrorFormat("[CreatePortalToTown] Town portals are not allowed in town.");
                return false;
            }

            /*if (LokiPoe.Me.IsInMapRoom)
			{
				Log.ErrorFormat("[CreatePortalToTown] Town portals are not allowed in the map room.");
				return false;
			}*/

            if (LokiPoe.Me.IsInHideout)
            {
                Log.ErrorFormat("[CreatePortalToTown] Town portals are not allowed in hideouts.");
                return false;
            }

            if (LokiPoe.CurrentWorldArea.IsMissionArea || LokiPoe.CurrentWorldArea.IsDenArea ||
                LokiPoe.CurrentWorldArea.IsRelicArea || LokiPoe.CurrentWorldArea.IsDailyArea)
            {
                Log.ErrorFormat("[CreatePortalToTown] Town Portals are not allowed in mission areas.");
                return false;
            }

            await Coroutines.FinishCurrentAction();

            await Coroutines.CloseBlockingWindows();

            var portalSkill = LokiPoe.InGameState.SkillBarHud.Skills.FirstOrDefault(s => s.Name == "Portal");
            if (portalSkill != null && portalSkill.CanUse(true))
            {
                Log.DebugFormat("[CreatePortalToTown] We have a Portal skill on the skill bar. Now using it.");

                var err = LokiPoe.InGameState.SkillBarHud.Use(portalSkill.Slot, false);
                Log.InfoFormat("[CreatePortalToTown] SkillBarHud.Use returned {0}.", err);

                await Coroutines.LatencyWait();

                await Coroutines.FinishCurrentAction();

                if (err == LokiPoe.InGameState.UseResult.None)
                {
                    var sw = Stopwatch.StartNew();
                    while (sw.ElapsedMilliseconds < 3000)
                    {
                        var portal = LokiPoe.ObjectManager.Objects.OfType<Portal>().FirstOrDefault(p => p.Distance < 50);
                        if (portal != null)
                        {
                            return true;
                        }

                        Log.DebugFormat("[CreatePortalToTown] No portal was detected yet, waiting...");

                        await Coroutines.LatencyWait();
                    }
                }
            }

            Log.DebugFormat("[CreatePortalToTown] Now opening the inventory panel.");

            // We need the inventory panel open.
            if (!await OpenInventoryPanel())
            {
                return false;
            }

            await Coroutines.ReactionWait();

            Log.DebugFormat("[CreatePortalToTown] Now searching the main inventory for a Portal Scroll.");

            var item =
                LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).FirstOrDefault(
                    i => i.Name == "Portal Scroll");
            if (item == null)
            {
                Log.ErrorFormat("[CreatePortalToTown] There are no Portal Scrolls in the inventory.");
                return false;
            }

            Log.DebugFormat("[CreatePortalToTown] Now using the Portal Scroll.");

            var err2 = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.UseItem(item.LocalId);
            if (err2 != UseItemResult.None)
            {
                Log.ErrorFormat("[CreatePortalToTown] UseItem returned {0}.", err2);
                return false;
            }

            await Coroutines.LatencyWait();
            await Coroutines.ReactionWait();

            await Coroutines.CloseBlockingWindows();

            return true;
        }

        /// <summary>
        /// This coroutine will attempt to take a portal
        /// </summary>
        /// <returns>true if the portal was taken, and an area change occurred, and false otherwise.</returns>
        public static async Task<bool> TakeClosestPortal()
        {
            var sw = Stopwatch.StartNew();

            if (LokiPoe.ConfigManager.IsAlwaysHighlightEnabled)
            {
                Log.InfoFormat("[TakeClosestPortal] Now disabling Always Highlight to avoid label issues.");
                LokiPoe.Input.SimulateKeyEvent(LokiPoe.Input.Binding.highlight_toggle, true, false, false);
                await Coroutine.Sleep(16);
            }

            NetworkObject portal = null;
            while (portal == null || !portal.IsTargetable)
            {
                Log.DebugFormat("[TakeClosestPortal] Now waiting for the portal to spawn. {0} elapsed.",
                    sw.Elapsed);

                await Coroutines.LatencyWait();

                portal =
                    LokiPoe.ObjectManager.GetObjectsByType<Portal>()
                        .Where(p => p.Distance < 50)
                        .OrderBy(p => p.Distance)
                        .FirstOrDefault();

                if (sw.ElapsedMilliseconds > 10000)
                {
                    break;
                }
            }

            if (portal == null)
            {
                Log.ErrorFormat("[TakeClosestPortal] A portal was not found.");
                return false;
            }

            var pos = ExilePather.FastWalkablePositionFor(portal);

            Log.DebugFormat("[TakeClosestPortal] The portal was found at {0}.", pos);

            if (!await MoveToLocation(pos, 5, 10000, () => false))
            {
                return false;
            }

            var hash = LokiPoe.LocalData.AreaHash;

            // Try to interact 3 times.
            for (var i = 0; i < 3; i++)
            {
                await Coroutines.FinishCurrentAction();

                Log.DebugFormat("[TakeClosestPortal] The portal to interact with is {0} at {1}.",
                    portal.Id, pos);

                if (await InteractWith(portal))
                {
                    if (await WaitForAreaChange(hash))
                    {
                        Log.DebugFormat("[TakeClosestPortal] The portal has been taken.");
                        return true;
                    }
                }

                await Coroutine.Sleep(1000);
            }

            Log.ErrorFormat(
                "[TakeClosestPortal] We have failed to take the portal 3 times.");

            return false;
        }

        /// <summary>
        /// This coroutine will attempt to take an area transition
        /// </summary>
        /// <returns>true if the area transition was taken, and an area change occurred, and false otherwise.</returns>
        public static async Task<bool> TakeClosestAreaTransition()
        {
            var sw = Stopwatch.StartNew();

            NetworkObject at = null;
            while (at == null)
            {
                Log.DebugFormat("[TakeClosestAreaTransition] Now waiting for the portal to spawn. {0} elapsed.",
                    sw.Elapsed);

                await Coroutines.LatencyWait();

                at =
                    LokiPoe.ObjectManager.GetObjectsByType<AreaTransition>()
                        .Where(
                            p =>
                                ExilePather.PathDistance(LokiPoe.MyPosition, ExilePather.FastWalkablePositionFor(p)) <
                                70)
                        .OrderBy(p => p.Distance)
                        .FirstOrDefault();

                if (sw.ElapsedMilliseconds > 3000)
                {
                    break;
                }
            }

            if (at == null)
            {
                Log.ErrorFormat("[TakeClosestAreaTransition] An area transition was not found.");
                return false;
            }

            var pos = ExilePather.FastWalkablePositionFor(at);

            Log.DebugFormat("[TakeClosestAreaTransition] The portal was found at {0}.", pos);

            if (!await MoveToLocation(pos, 5, 10000, () => false))
            {
                return false;
            }

            // Try to interact 3 times.
            for (var i = 0; i < 3; i++)
            {
                Log.DebugFormat("[TakeClosestAreaTransition] The are transition to interact with is {0} at {1}.",
                    at.Id, pos);

                var result = await TakeAreaTransition(at, false, -1);
                if (result != TakeAreaTransitionError.None)
                {
                    Log.ErrorFormat("[TakeClosestAreaTransition] TakeAreaTransition returned {0}.", result);
                }
                else
                {
                    return true;
                }

                await Coroutine.Sleep(1000);
            }

            Log.ErrorFormat(
                "[TakeClosestAreaTransition] We have failed to take the area transition 3 times.");

            return false;
        }

        /// <summary>
        /// This coroutine will logout the character to character selection.
        /// </summary>
        /// <returns></returns>
        public static async Task LogoutToTitleScreen()
        {
            Log.DebugFormat("[LogoutToTitleScreen] Now logging out to Character Selection screen.");

            var err = LokiPoe.EscapeState.LogoutToTitleScreen();
            if (err != LokiPoe.EscapeState.LogoutError.None)
            {
                Log.ErrorFormat(
                    "[LogoutToTitleScreen] EscapeState.LogoutToTitleScreen returned {0}. Now stopping the bot because it cannot continue.");
                BotManager.Stop();
            }

            var sw = Stopwatch.StartNew();

            while (!LokiPoe.IsInLoginScreen)
            {
                Log.DebugFormat("[LogoutToTitleScreen] We have been waiting {0} to get out of game.", sw.Elapsed);

                await Coroutines.ReactionWait();
            }
        }

        /// <summary>
        /// This coroutine moves towards a position until it is within the specified stop distance.
        /// </summary>
        /// <param name="position">The position to move ot.</param>
        /// <param name="stopDistance">How close to the location should we get.</param>
        /// <param name="timeout">How long should the coroutine execute for before stopping due to timeout.</param>
        /// <returns></returns>
        public static async Task<bool> MoveToLocation(Vector2i position, int stopDistance, int timeout, Func<bool> stopCondition)
        {
            var sw = Stopwatch.StartNew();
            var dsw = Stopwatch.StartNew();

            var da = (bool)PlayerMover.Instance.Execute("GetDoAdjustments");
            PlayerMover.Instance.Execute("SetDoAdjustments", false);

            while (LokiPoe.MyPosition.Distance(position) > stopDistance)
            {
                if (LokiPoe.Me.IsDead)
                {
                    Log.ErrorFormat("[MoveToLocation] The player is dead.");
                    PlayerMover.Instance.Execute("SetDoAdjustments", da);
                    return false;
                }

                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.ErrorFormat("[MoveToLocation] Timeout.");
                    PlayerMover.Instance.Execute("SetDoAdjustments", da);
                    return false;
                }

                if (stopCondition())
                {
                    break;
                }

                if (dsw.ElapsedMilliseconds > 100)
                {
                    Log.DebugFormat(
                        "[MoveToLocation] Now moving towards {0}. We have been performing this task for {1}.",
                        position,
                        sw.Elapsed);
                    dsw.Restart();
                }

                if (!PlayerMover.MoveTowards(position))
                {
                    Log.ErrorFormat("[MoveToLocation] MoveTowards failed for {0}.", position);
                }

                await Coroutine.Yield();
            }

            PlayerMover.Instance.Execute("SetDoAdjustments", da);

            await Coroutines.FinishCurrentAction();

            return true;
        }

        /// <summary>
        /// This coroutine attempts to highlight and interact with an object.
        /// Interaction only takes place if the object is highlighted.
        /// </summary>
        /// <param name="obj">The object to interact with.</param>
        /// <param name="holdCtrl">Should control be held? For area transitions.</param>
        /// <returns>true on success and false on failure.</returns>
        public static async Task<bool> InteractWith(NetworkObject obj, bool holdCtrl = false)
        {
            return await InteractWith<NetworkObject>(obj, holdCtrl);
        }

        /// <summary>
        /// This coroutine attempts to highlight and interact with an object.
        /// Interaction only takes place if the object is highlighted or an object of type T is.
        /// </summary>
        /// <typeparam name="T">The type of object acceptable to be highlighted if the intended target is not highlighted.</typeparam>
        /// <param name="holdCtrl">Should control be held? For area transitions.</param>
        /// <param name="obj">The object to interact with.</param>
        /// <returns>true on success and false on failure.</returns>
        public static async Task<bool> InteractWith<T>(NetworkObject obj, bool holdCtrl = false)
        {
            if (obj == null)
            {
                Log.ErrorFormat("[InteractWith] The object is null.");
                return false;
            }

            var id = obj.Id;

            Log.DebugFormat("[InteractWith] Now attempting to highlight {0}.", id);

            await Coroutines.FinishCurrentAction();

            if (!LokiPoe.Input.HighlightObject(obj))
            {
                Log.ErrorFormat("[InteractWith] The target could not be highlighted.");
                return false;
            }

            var target = LokiPoe.InGameState.CurrentTarget;
            if (target != obj && !(target is T))
            {
                Log.ErrorFormat("[InteractWith] The target highlight has been lost.");
                return false;
            }

            Log.DebugFormat("[InteractWith] Now attempting to interact with {0}.", id);

            if (holdCtrl)
            {
                LokiPoe.ProcessHookManager.SetKeyState(Keys.ControlKey, 0x8000);
            }

            LokiPoe.Input.ClickLMB();

            await Coroutines.LatencyWait();

            await Coroutines.FinishCurrentAction(false);

            LokiPoe.ProcessHookManager.ClearAllKeyStates();

            return true;
        }

        /// <summary>
        /// Errors for the TakeWaypointTo coroutine.
        /// </summary>
        public enum TakeAreaTransitionError
        {
            /// <summary>None, the area transition has been taken to the desired area.</summary>
            None,

            /// <summary>No area transition object was detected.</summary>
            NoAreaTransitions,

            /// <summary>Interaction with the area transition failed.</summary>
            InteractFailed,

            /// <summary>The instance manager panel did not open.</summary>
            InstanceManagerDidNotOpen,

            /// <summary>The JoinNew function failed with an error.</summary>
            JoinNewFailed,

            /// <summary>An area change did not happen after taking the area transition.</summary>
            WaitForAreaChangeFailed,

            /// <summary>Too many instances are listed based on user configuration.</summary>
            TooManyInstances,
        }

        /// <summary>
        /// This coroutine interacts with an area transition in order to change areas. It assumes
        /// you are in interaction range with the area transition itself. It can be used both in town,
        /// and out of town, given the previous conditions are met.
        /// </summary>
        /// <param name="obj">The area transition object to take.</param>
        /// <param name="newInstance">Should a new instance be created.</param>
        /// <param name="isLocal">Is the area transition local? In other words, should the couroutine not wait for an area change.</param>
        /// <param name="maxInstances">The max number of instance entries allowed to Join a new instance or -1 to not check.</param>
        /// <returns>A TakeAreaTransitionError that describes the result.</returns>
        public static async Task<TakeAreaTransitionError> TakeAreaTransition(NetworkObject obj, bool newInstance,
            int maxInstances,
            bool isLocal = false)
        {
            Log.InfoFormat("[TakeAreaTransition] {0} {1} {2}", obj.Name, newInstance ? "(new instance)" : "",
                isLocal ? "(local)" : "");

            await Coroutines.CloseBlockingWindows();

            await Coroutines.FinishCurrentAction();

            var hash = LokiPoe.LocalData.AreaHash;
            var pos = LokiPoe.MyPosition;

            if (!await InteractWith(obj, newInstance))
                return TakeAreaTransitionError.InteractFailed;

            if (newInstance)
            {
                if (!await WaitForInstanceManager(5000))
                {
                    LokiPoe.ProcessHookManager.ClearAllKeyStates();

                    return TakeAreaTransitionError.InstanceManagerDidNotOpen;
                }

                LokiPoe.ProcessHookManager.ClearAllKeyStates();

                await Coroutines.LatencyWait();

                await Coroutine.Sleep(1000); // Let the gui stay open a bit before clicking too fast.

                if (LokiPoe.InGameState.InstanceManagerUi.InstanceCount >= maxInstances)
                {
                    return TakeAreaTransitionError.TooManyInstances;
                }

                var nierr = LokiPoe.InGameState.InstanceManagerUi.JoinNewInstance();
                if (nierr != LokiPoe.InGameState.JoinInstanceResult.None)
                {
                    Log.ErrorFormat("[TakeAreaTransition] InstanceManagerUi.JoinNew returned {0}.", nierr);
                    return TakeAreaTransitionError.JoinNewFailed;
                }

                // Wait for the action to take place first.
                await Coroutines.LatencyWait();

                await Coroutines.ReactionWait();
            }

            if (isLocal)
            {
                if (!await WaitForPositionChange(pos))
                {
                    Log.ErrorFormat("[TakeAreaTransition] WaitForPositionChange failed.");
                    return TakeAreaTransitionError.WaitForAreaChangeFailed;
                }
            }
            else
            {
                if (!await WaitForAreaChange(hash))
                {
                    Log.ErrorFormat("[TakeAreaTransition] WaitForAreaChange failed.");
                    return TakeAreaTransitionError.WaitForAreaChangeFailed;
                }
            }

            return TakeAreaTransitionError.None;
        }

        public static async Task<bool> WaitForCursorToBeEmpty(int timeout = 5000)
        {
            var sw = Stopwatch.StartNew();
            while (LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Cursor).Any())
            {
                Log.InfoFormat("[WaitForCursorToBeEmpty] Waiting for the cursor to be empty.");
                await Coroutines.LatencyWait();
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.InfoFormat("[WaitForCursorToBeEmpty] Timeout while waiting for the cursor to become empty.");
                    return false;
                }
            }
            return true;
        }

        public static async Task<bool> WaitForCursorToHaveItem(int timeout = 5000)
        {
            var sw = Stopwatch.StartNew();
            while (!LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Cursor).Any())
            {
                Log.InfoFormat("[WaitForCursorToHaveItem] Waiting for the cursor to have an item.");
                await Coroutines.LatencyWait();
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.InfoFormat("[WaitForCursorToHaveItem] Timeout while waiting for the cursor to contain an item.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This coroutine interacts with an area transition in order to change areas. It assumes
        /// you are in interaction range with the area transition itself. It can be used both in town,
        /// and out of town, given the previous conditions are met.
        /// </summary>
        /// <param name="name">The name of the area transition to take.</param>
        /// <param name="newInstance">Should a new instance be created.</param>
        /// <param name="isLocal">Is the area transition local? In other words, should the couroutine not wait for an area change.</param>
        /// <param name="maxInstances">The max number of instance entries allowed to Join a new instance or -1 to not check.</param>
        /// <returns>A TakeAreaTransitionError that describes the result.</returns>
        public static async Task<TakeAreaTransitionError> TakeAreaTransition(string name, bool newInstance, int maxInstances,
            bool isLocal = false)
        {
            var at = LokiPoe.ObjectManager.AreaTransition(name);
            if (at == null)
                return TakeAreaTransitionError.NoAreaTransitions;

            return await TakeAreaTransition(at, newInstance, maxInstances, isLocal);
        }

        /// <summary>
        /// Errors for the OpenStash function.
        /// </summary>
        public enum OpenStashError
        {
            /// <summary>None, the stash has been interacted with and the stash panel is opened.</summary>
            None,

            /// <summary>There was an error moving to stash.</summary>
            CouldNotMoveToStash,

            /// <summary>No stash object was detected.</summary>
            NoStash,

            /// <summary>Interaction with the stash failed.</summary>
            InteractFailed,

            /// <summary>The stash panel did not open.</summary>
            StashPanelDidNotOpen,
        }

        /// <summary>
        /// This coroutine interacts with stash and waits for the stash panel to open. When called from a hideout,
        /// the stash must be in spawn range, otherwise the coroutine will fail.
        /// </summary>
        /// <returns>An OpenStashError that describes the result.</returns>
        public static async Task<OpenStashError> OpenGuildStash()
        {
            return await OpenStash(true);
        }

        private static NetworkObject DetermineStash(bool guild)
        {
            var stash = LokiPoe.ObjectManager.Stash;
            if (guild)
            {
                stash = LokiPoe.ObjectManager.GuildStash;
            }
            return stash;
        }

        /// <summary>
        /// This coroutine interacts with stash and waits for the stash panel to open. When called from a hideout,
        /// the stash must be in spawn range, otherwise the coroutine will fail.
        /// </summary>
        ///<param name="guild">Should the guild stash be opened?</param>
        /// <returns>An OpenStashError that describes the result.</returns>
        public static async Task<OpenStashError> OpenStash(bool guild = false)
        {
            await Coroutines.CloseBlockingWindows();

            await Coroutines.FinishCurrentAction();

            var stash = DetermineStash(guild);
            if (stash == null)
            {
                if (LokiPoe.Me.IsInHideout)
                {
                    return OpenStashError.NoStash;
                }

                if (
                    !await
                        MoveToLocation(ExilePather.FastWalkablePositionFor(Utility.GuessStashLocation()), 25,
                            60000, () => DetermineStash(guild) != null && DetermineStash(guild).Distance < 75))
                {
                    return OpenStashError.CouldNotMoveToStash;
                }

                stash = DetermineStash(guild);
                if (stash == null)
                {
                    return OpenStashError.NoStash;
                }
            }

            if (stash.Distance > 30)
            {
                var p = stash.Position;
                if (!await MoveToLocation(ExilePather.FastWalkablePositionFor(p), 25, 15000, () => false))
                {
                    return OpenStashError.CouldNotMoveToStash;
                }
            }

            await Coroutines.FinishCurrentAction();

            stash = DetermineStash(guild);
            if (stash == null)
            {
                return OpenStashError.NoStash;
            }

            if (!await InteractWith(stash))
                return OpenStashError.InteractFailed;

            if (guild)
            {
                if (!await WaitForGuildStashPanel())
                    return OpenStashError.StashPanelDidNotOpen;

                await WaitForGuildStashTabChange(-1);
            }
            else
            {
                if (!await WaitForStashPanel())
                    return OpenStashError.StashPanelDidNotOpen;

                await WaitForStashTabChange(-1);
            }

            return OpenStashError.None;
        }

        /// <summary>
        /// Errors for the OpenWaypoint function.
        /// </summary>
        public enum OpenWaypointError
        {
            /// <summary>None, the waypoint has been interacted with and the world panel is opened.</summary>
            None,

            /// <summary>There was an error moving to the waypoint.</summary>
            CouldNotMoveToWaypoint,

            /// <summary>No waypoint object was detected.</summary>
            NoWaypoint,

            /// <summary>Interaction with the waypoint failed.</summary>
            InteractFailed,

            /// <summary>The world panel did not open.</summary>
            WorldPanelDidNotOpen,
        }

        /// <summary>
        /// This coroutine interacts with the waypoint and waits for the world panel to open. When called from a hideout,
        /// the waypoint must be in spawn range, otherwise the coroutine will fail. The movement is done without returning,
        /// so this should be carefully used when not in town.
        /// </summary>
        /// <returns>An OpenStashError that describes the result.</returns>
        public static async Task<OpenWaypointError> OpenWaypoint()
        {
            await Coroutines.CloseBlockingWindows();

            await Coroutines.FinishCurrentAction();

            var waypoint = LokiPoe.ObjectManager.Waypoint;
            if (waypoint == null)
            {
                if (!LokiPoe.Me.IsInTown)
                {
                    return OpenWaypointError.NoWaypoint;
                }

                if (
                    !await
                        MoveToLocation(ExilePather.FastWalkablePositionFor(Utility.GuessWaypointLocation()), 25,
                            60000, () => LokiPoe.ObjectManager.Waypoint != null))
                {
                    return OpenWaypointError.CouldNotMoveToWaypoint;
                }

                waypoint = LokiPoe.ObjectManager.Waypoint;
                if (waypoint == null)
                {
                    return OpenWaypointError.NoWaypoint;
                }
            }

            if (ExilePather.PathDistance(LokiPoe.MyPosition, waypoint.Position) > 30)
            {
                if (!await MoveToLocation(ExilePather.FastWalkablePositionFor(waypoint.Position), 25, 15000, () => false))
                {
                    return OpenWaypointError.CouldNotMoveToWaypoint;
                }
            }

            await Coroutines.FinishCurrentAction();

            waypoint = LokiPoe.ObjectManager.Waypoint;
            if (waypoint == null)
            {
                return OpenWaypointError.NoWaypoint;
            }

            if (!await InteractWith(waypoint))
                return OpenWaypointError.InteractFailed;

            if (!await WaitForWorldPanel())
                return OpenWaypointError.WorldPanelDidNotOpen;

            await Coroutine.Sleep(1000); // Adding this in to let the gui load more

            return OpenWaypointError.None;
        }

        /// <summary>
        /// Errors for the TakeWaypointTo function.
        /// </summary>
        public enum TakeWaypointToError
        {
            /// <summary>
            /// The waypoint has been taken, and the area changed.
            /// </summary>
            None,

            /// <summary>The OpenWaypoint function failed.</summary>
            OpenWaypointFailed,

            /// <summary>The TakeWaypoint function failed.</summary>
            TakeWaypointFailed,

            /// <summary>The WaitForAreaChange function failed.</summary>
            WaitForAreaChangeFailed,

            /// <summary>Too many instances are listed based on user configuration.</summary>
            TooManyInstances,
        }

        /// <summary>
        /// This coroutine will attempt to move to a waypoint and take it to the desired area.
        /// </summary>
        /// <param name="id">The id of the world area to take a waypoint to.</param>
        /// <param name="newInstance">Should a new instance be made.</param>
        /// <param name="maxInstances">The max number of instance entries allowed to Join a new instance or -1 to not check.</param>
        /// <returns>A TakeWaypointToError that describes the result.</returns>
        public static async Task<TakeWaypointToError> TakeWaypointTo(string id, bool newInstance, int maxInstances)
        {
            Log.InfoFormat("[TakeWaypointTo] {0} {1}", id, newInstance ? "(new instance)" : "");
            var hash = LokiPoe.LocalData.AreaHash;

            // Try to walk to it and open it.
            var owerr = await OpenWaypoint();
            if (owerr != OpenWaypointError.None)
                return TakeWaypointToError.OpenWaypointFailed;

            // Try to return to town.
            var twerr = LokiPoe.InGameState.WorldUi.TakeWaypoint(id, newInstance, maxInstances);
            if (twerr != LokiPoe.InGameState.TakeWaypointResult.None)
            {
                Log.ErrorFormat("[TakeWaypointTo] TakeWaypoint returned {0}.", twerr);
                if (twerr == LokiPoe.InGameState.TakeWaypointResult.TooManyInstances)
                {
                    return TakeWaypointToError.TooManyInstances;
                }
                return TakeWaypointToError.TakeWaypointFailed;
            }

            await Coroutines.LatencyWait();

            // If we are in town, we're all done!
            if (!await WaitForAreaChange(hash))
                return TakeWaypointToError.WaitForAreaChangeFailed;

            return TakeWaypointToError.None;
        }

        /// <summary>
        /// This coroutine takes a waypoint to town.
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> TakeWaypointToTown()
        {
            var twterr = await TakeWaypointTo(LokiPoe.CurrentWorldArea.GoverningTown.Id, false, -1);
            if (twterr != TakeWaypointToError.None)
                Log.ErrorFormat("[TakeWaypointToTown] TakeWaypointTo returned {0}.", twterr);
            return twterr == TakeWaypointToError.None;
        }

        /// <summary>
        /// Errors for the TalkToNpc function.
        /// </summary>
        public enum TalkToNpcError
        {
            /// <summary>None, the npc has been interacted with and the npc dialog panel is opened.</summary>
            None,

            /// <summary>There was an error moving to the npc.</summary>
            CouldNotMoveToNpc,

            /// <summary>No waypoint object was detected.</summary>
            NoNpc,

            /// <summary>Interaction with the npc failed.</summary>
            InteractFailed,

            /// <summary>The npc's dialog panel did not open.</summary>
            NpcDialogPanelDidNotOpen,
        }

        /// <summary>
        /// This coroutine interacts with a npc and waits for the npc dialog panel to open. When called for a non-main town npc 
        /// the npc must be in spawn range, otherwise the coroutine will fail. The movement is done without returning,
        /// so this should be carefully used when not in town.
        /// </summary>
        /// <returns>An OpenStashError that describes the result.</returns>
        public static async Task<TalkToNpcError> TalkToNpc(string name)
        {
            await Coroutines.CloseBlockingWindows();

            await Coroutines.FinishCurrentAction();

            var npc = LokiPoe.ObjectManager.GetObjectByName(name);
            if (npc == null)
            {
                var pos = Utility.GuessNpcLocation(name);
                if (pos == Vector2i.Zero)
                {
                    return TalkToNpcError.NoNpc;
                }

                if (
                    !await
                        MoveToLocation(ExilePather.FastWalkablePositionFor(pos), 25,
                            60000, () => LokiPoe.ObjectManager.GetObjectByName(name) != null))
                {
                    return TalkToNpcError.CouldNotMoveToNpc;
                }

                npc = LokiPoe.ObjectManager.GetObjectByName(name);
                if (npc == null)
                {
                    return TalkToNpcError.NoNpc;
                }
            }

            if (ExilePather.PathDistance(LokiPoe.MyPosition, npc.Position) > 30)
            {
                if (!await MoveToLocation(ExilePather.FastWalkablePositionFor(npc.Position), 25, 15000, () => false))
                {
                    return TalkToNpcError.CouldNotMoveToNpc;
                }

                npc = LokiPoe.ObjectManager.GetObjectByName(name);
                if (npc == null)
                {
                    return TalkToNpcError.NoNpc;
                }
            }

            await Coroutines.FinishCurrentAction();

            if (!await InteractWith(npc))
                return TalkToNpcError.InteractFailed;

            if (!await WaitForNpcDialogPanel())
                return TalkToNpcError.NpcDialogPanelDidNotOpen;

            return TalkToNpcError.None;
        }

        /// <summary>
        /// This delegate is used to determine if the bot needs to id.
        /// </summary>
        /// <returns>true if the bot needs to id, and false otherwise.</returns>
        public delegate bool NeedsToIdDelegate();

        /// <summary>
        /// This delegate is called when stashing is completed.
        /// </summary>
        public delegate void IdingCompleteDelegate();

        /// <summary>
        /// The delegate used to determine if an item should be ided.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <param name="user">The user object passed to IdItemsCoroutine.</param>
        /// <returns>true if the item should be ided and false otherwise.</returns>
        public delegate bool IdItemsDelegate(Item item, object user);

        /// <summary>Coroutine results for IdItemsCoroutine.</summary>
        public enum IdItemsCoroutineError
        {
            /// <summary>All items were ided, or there are none to id.</summary>
            None,

            /// <summary>The OpenStash function did not complete successfully.</summary>
            OpenStashFailed,

            /// <summary>The GoToFirstTab function failed.</summary>
            GoToFirstTabFailed,

            /// <summary>There are no id scrolls to use.</summary>
            NoMoreIdScrolls,

            /// <summary>The id operation has been interupted by the inventory window being closed.</summary>
            InventoryClosed,

            /// <summary>The id operation has been interupted by the stash window being closed.</summary>
            StashClosed,

            /// <summary>The UseItem function failed.</summary>
            UseItemFailed,

            /// <summary>We have id scrolls to use, but iding has finished and there's still an unid item.</summary>
            UnidentifiedItemsRemaining
        }

        /// <summary>
        /// This coroutine will id all items in the inventory that match the shouldIdItem
        /// delegate until no more items remain, or there are no more id scrolls to use. It looks
        /// for id scrolls in the main inventory.
        /// </summary>
        /// <param name="shouldIdItem">The delegate that determines if an item should be ided.</param>
        /// <param name="user">The user object that should be passed through the IdItemsDelegate.</param>
        /// <returns>An IdItemsCoroutineError error that describes the result.</returns>
        public static async Task<IdItemsCoroutineError> IdItemsUsingInventoryCoroutine(
            IdItemsDelegate shouldIdItem = null,
            object user = null)
        {
            // The default id coroutine will id all unidentified items.
            if (shouldIdItem == null)
            {
                shouldIdItem = (i, u) => !i.IsIdentified;
            }

            // If we don't have any items to id, we don't need to continue.
            if (
                !LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                    .Any(i => !i.IsIdentified && shouldIdItem(i, user)))
            {
                return IdItemsCoroutineError.None;
            }

            if (
                LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                    .FirstOrDefault(i => i.Name.Equals("Scroll of Wisdom")) == null)
            {
                return IdItemsCoroutineError.NoMoreIdScrolls;
            }

            // Close windows that prevent us from iding. Other tasks should be executing in a way where this doesn't interefere.
            if (LokiPoe.InGameState.PurchaseUi.IsOpened || LokiPoe.InGameState.SellUi.IsOpened ||
                LokiPoe.InGameState.TradeUi.IsOpened)
            {
                await Coroutines.CloseBlockingWindows();
            }

            if (!await OpenInventoryPanel())
            {
                return IdItemsCoroutineError.InventoryClosed;
            }

            await Coroutines.ReactionWait();

            for (int l = 0; l < 2; l++)
            {
                foreach (
                    var item in
                        LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                            .Where(i => !i.IsIdentified && shouldIdItem(i, user))
                            .ToList())
                {
                    // Handle the stash window closing.
                    if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                    {
                        return IdItemsCoroutineError.InventoryClosed;
                    }

                    var sow =
                        LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).FirstOrDefault(
                            i => i.FullName.Equals("Scroll of Wisdom"));
                    if (sow == null)
                    {
                        return IdItemsCoroutineError.NoMoreIdScrolls;
                    }

                    Log.InfoFormat("[IdItemsUsingInventoryCoroutine] Now using {0} on {1}.", sow.Name, item.Name);

                    var ba = item.BaseAddress;

                    var err1 = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.UseItem(sow.LocalId);
                    if (err1 != UseItemResult.None)
                    {
                        Log.InfoFormat("[IdItemsUsingInventoryCoroutine] UseItem returned {0}.", err1);
                        return IdItemsCoroutineError.UseItemFailed;
                    }

                    await Coroutines.LatencyWait();
                    await Coroutines.ReactionWait();

                    var err = InventoryControlWrapper.BeginApplyCursor();
                    if (err != ApplyCursorResult.None)
                    {
                        Log.InfoFormat("[IdItemsUsingInventoryCoroutine] BeginApplyCursor returned {0}.", err);
                        return IdItemsCoroutineError.UseItemFailed;
                    }

                    err = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.ApplyCursorTo(item.LocalId);
                    if (err != ApplyCursorResult.None)
                    {
                        Log.InfoFormat("[IdItemsUsingInventoryCoroutine] ApplyCursorTo returned {0}.", err);
                    }

                    await Coroutines.LatencyWait();
                    await Coroutines.ReactionWait();

                    var err2 = InventoryControlWrapper.EndApplyCursor();
                    if (err2 != ApplyCursorResult.None)
                    {
                        Log.InfoFormat("[IdItemsUsingInventoryCoroutine] EndApplyCursor returned {0}.", err2);
                        return IdItemsCoroutineError.UseItemFailed;
                    }

                    if (err != ApplyCursorResult.None)
                    {
                        return IdItemsCoroutineError.UseItemFailed;
                    }

                    Stopwatch sw = Stopwatch.StartNew();
                    var wait = Math.Max(LatencyTracker.Highest, 1500);
                    while (LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).Any(i => i.BaseAddress == ba))
                    {
                        Log.InfoFormat("[IdItemsUsingInventoryCoroutine] Waiting for the item to be moved.");
                        await Coroutine.Sleep(1);
                        if (sw.ElapsedMilliseconds > wait)
                        {
                            Log.InfoFormat("[IdItemsUsingInventoryCoroutine] Timeout while waiting for the item to be moved.");
                            break;
                        }
                    }
                }
            }

            if (
                LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                    .Any(i => !i.IsIdentified && shouldIdItem(i, user)))
            {
                return IdItemsCoroutineError.UnidentifiedItemsRemaining;
            }

            return IdItemsCoroutineError.None;
        }

        /// <summary>
        /// This coroutine will id all items in the inventory that match the shouldIdItem
        /// delegate until no more items remain, or there are no more id scrolls to use. It 
        /// looks for id scrolls in the Stash.
        /// </summary>
        /// <param name="shouldIdItem">The delegate that determines if an item should be ided.</param>
        /// <param name="user">The user object that should be passed through the IdItemsDelegate.</param>
        /// <returns>An IdItemsCoroutineError error that describes the result.</returns>
        public static async Task<IdItemsCoroutineError> IdItemsUsingStashCoroutine(IdItemsDelegate shouldIdItem = null,
            object user = null)
        {
            // The default stashing coroutine will stash all items.
            if (shouldIdItem == null)
            {
                shouldIdItem = (i, u) => !i.IsIdentified;
            }

            // If we don't have any items to id, we don't need to continue.
            if (
                !LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                    .Any(i => !i.IsIdentified && shouldIdItem(i, user)))
            {
                return IdItemsCoroutineError.None;
            }

            // We need both opened.
            if (!LokiPoe.InGameState.StashUi.IsOpened || !LokiPoe.InGameState.InventoryUi.IsOpened)
            {
                await Coroutines.CloseBlockingWindows();

                var otserr = await OpenStash();
                if (otserr != OpenStashError.None)
                {
                    Log.ErrorFormat("[IdItemsUsingStashCoroutine] OpenStash returned {0}.", otserr);
                    return IdItemsCoroutineError.OpenStashFailed;
                }

                await Coroutines.ReactionWait();
            }

            if (!LokiPoe.InGameState.StashUi.IsOpened)
            {
                return IdItemsCoroutineError.StashClosed;
            }

            if (!LokiPoe.InGameState.InventoryUi.IsOpened)
            {
                return IdItemsCoroutineError.InventoryClosed;
            }

            // We want to go to the first tab, since once we've requested tab contents, it's free.
            var res = LokiPoe.InGameState.StashUi.TabControl.SwitchToTabMouse(0);
            if (res != SwitchToTabResult.None)
            {
                return IdItemsCoroutineError.GoToFirstTabFailed;
            }

            await Coroutines.ReactionWait();

            await WaitForStashTabChange(-1);

            // Loop until something breaks.
            while (true)
            {
                // Handle window closing.
                if (!LokiPoe.InGameState.StashUi.IsOpened)
                {
                    return IdItemsCoroutineError.StashClosed;
                }
                if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                {
                    return IdItemsCoroutineError.InventoryClosed;
                }

                // As long as the tab stays the same, we should be able to cache these.
                var invTab = LokiPoe.InGameState.StashUi.StashTabInfo;
                if (invTab.IsRemoveOnly || invTab.IsPublic) // TODO: Handle this better
                {
                    if (invTab.IsRemoveOnly)
                        Log.DebugFormat("[IdItemsUsingStashCoroutine] The current tab is Remove only. Skipping it.");
                    else
                        Log.DebugFormat("[IdItemsUsingStashCoroutine] The current tab is Public. Skipping it.");

                    var oldId = invTab.InventoryId;

                    var result = LokiPoe.InGameState.StashUi.TabControl.NextTabKeyboard();
                    if (result != SwitchToTabResult.None)
                    {
                        Log.ErrorFormat("[IdItemsUsingStashCoroutine] NextTabKeyboard returned {0}.", result);

                        break;
                    }

                    await WaitForStashTabChange(oldId);

                    await Coroutines.ReactionWait();

                    continue;
                }

                Item sow;

                if (invTab.IsPremiumCurrency)
                {
                    sow = LokiPoe.InGameState.StashUi.InventoryControl_ScrollOfWisdom.CurrencyTabItem;
                }
                else
                {
                    sow = LokiPoe.InGameState.StashUi.InventoryControl.Inventory.FindItemByFullName("Scroll of Wisdom");
                }

                if (sow == null)
                {
                    var oldId = invTab.InventoryId;

                    var result = LokiPoe.InGameState.StashUi.TabControl.NextTabKeyboard();
                    if (result != SwitchToTabResult.None)
                    {
                        Log.ErrorFormat("[IdItemsUsingStashCoroutine] NextTabKeyboard returned {0}.", result);
                        break;
                    }

                    await WaitForStashTabChange(oldId);

                    await Coroutines.ReactionWait();

                    continue;
                }

                var item =
                    LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                        .FirstOrDefault(i => !i.IsIdentified && shouldIdItem(i, user));
                if (item == null)
                    break;

                Log.InfoFormat("[IdItemsUsingStashCoroutine] Now using {0} on {1}.", sow.Name, item.Name);

                var ba = item.BaseAddress;

                if (invTab.IsPremiumCurrency)
                {
                    var err1 = LokiPoe.InGameState.StashUi.InventoryControl_ScrollOfWisdom.UseItem();
                    if (err1 != UseItemResult.None)
                    {
                        Log.InfoFormat("[IdItemsUsingStashCoroutine] UseItem returned {0}.", err1);
                        return IdItemsCoroutineError.UseItemFailed;
                    }
                }
                else
                {
                    var err1 = LokiPoe.InGameState.StashUi.InventoryControl.UseItem(sow.LocalId);
                    if (err1 != UseItemResult.None)
                    {
                        Log.InfoFormat("[IdItemsUsingStashCoroutine] UseItem returned {0}.", err1);
                        return IdItemsCoroutineError.UseItemFailed;
                    }
                }

                await Coroutines.LatencyWait();
                await Coroutines.ReactionWait();

                var err = InventoryControlWrapper.BeginApplyCursor();
                if (err != ApplyCursorResult.None)
                {
                    Log.InfoFormat("[IdItemsUsingStashCoroutine] BeginApplyCursor returned {0}.", err);
                    return IdItemsCoroutineError.UseItemFailed;
                }

                err = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.ApplyCursorTo(item.LocalId);
                if (err != ApplyCursorResult.None)
                {
                    Log.InfoFormat("[IdItemsUsingStashCoroutine] ApplyCursorTo returned {0}.", err);
                }

                var err2 = InventoryControlWrapper.EndApplyCursor();
                if (err2 != ApplyCursorResult.None)
                {
                    Log.InfoFormat("[IdItemsUsingStashCoroutine] EndApplyCursor returned {0}.", err2);
                    return IdItemsCoroutineError.UseItemFailed;
                }

                if (err != ApplyCursorResult.None)
                {
                    return IdItemsCoroutineError.UseItemFailed;
                }

                await Coroutines.LatencyWait();
                await Coroutines.ReactionWait();

                Stopwatch sw = Stopwatch.StartNew();
                var wait = Math.Max(LatencyTracker.Highest, 1500);
                while (LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).Any(i => i.BaseAddress == ba))
                {
                    Log.InfoFormat("[IdItemsUsingStashCoroutine] Waiting for the item to be moved.");
                    await Coroutine.Sleep(1);
                    if (sw.ElapsedMilliseconds > wait)
                    {
                        Log.InfoFormat("[IdItemsUsingStashCoroutine] Timeout while waiting for the item to be moved.");
                        break;
                    }
                }

                if (
                    !LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                        .Any(i => !i.IsIdentified && shouldIdItem(i, user)))
                {
                    break;
                }
            }

            // If we don't have any items to id, we don't need to continue.
            if (
                LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                    .Any(i => !i.IsIdentified && shouldIdItem(i, user)))
            {
                return IdItemsCoroutineError.NoMoreIdScrolls;
            }

            return IdItemsCoroutineError.None;
        }

        /// <summary>
        /// This delegate is used to determine if the bot needs to sell.
        /// </summary>
        /// <returns>true if the bot needs to sell, and false otherwise.</returns>
        public delegate bool NeedsToSellDelegate();

        /// <summary>
        /// This delegate is called when selling is completed.
        /// </summary>
        public delegate void SellingCompleteDelegate();

        /// <summary>
        /// The delegate used to determine if an item should be sold.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <param name="user">The user object passed to SellItemsCoroutine.</param>
        /// <returns>true if the item should be sold and false otherwise.</returns>
        public delegate bool SellItemsDelegate(Item item, object user);

        /// <summary>
        /// This delegate is used to determine if the bot should accept the current sell offer.
        /// </summary>
        /// <returns>true if the bot should accept the sell offer, and false otherwise.</returns>
        public delegate bool ShouldAcceptSellOfferDelegate();

        /// <summary>Coroutine results for SellItemsCoroutine.</summary>
        public enum SellItemsCoroutineError
        {
            /// <summary>All items were sold, or there are none more to sell.</summary>
            None,

            /// <summary>No NPC to vendor to was found.</summary>
            NoNpc,

            /// <summary>The sell operation has been interupted by the sell window being closed.</summary>
            SellClosed,

            /// <summary>The sell operation has been interupted by the inventory window being closed.</summary>
            InventoryClosed,

            /// <summary>The TalkToNpc function failed.</summary>
            TalkToNpcFailed,

            /// <summary>The Converse function failed.</summary>
            ConverseFailed,

            /// <summary>The Sell panel did not open.</summary>
            SellPanelDidNotopen,

            /// <summary>The user refused the offer.</summary>
            OfferRefused,

            /// <summary>The Accept function failed.</summary>
            AcceptFailed,

            /// <summary>The FastMove function failed.</summary>
            FastMoveFailed,
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shouldAcceptOffer"></param>
        /// <param name="shouldSellItem">The delegate that determines if an item should be sold.</param>
        /// <param name="user">The user object that should be passed through the SellItemsDelegate.</param>
        /// <returns>An SellItemsCoroutineError error that describes the result.</returns>
        public static async Task<SellItemsCoroutineError> SellItemsCoroutine(
            ShouldAcceptSellOfferDelegate shouldAcceptOffer,
            SellItemsDelegate shouldSellItem,
            object user = null)
        {
            Tuple<string, Vector2i> npcInfo;

            if (LokiPoe.CurrentWorldArea.IsHideoutArea)
            {
                do
                {
                    NetworkObject master;

                    master = LokiPoe.ObjectManager.Haku;
                    if (master != null)
                    {
                        npcInfo = new Tuple<string, Vector2i>(master.Name, master.Position);
                        break;
                    }

                    master = LokiPoe.ObjectManager.Vorici;
                    if (master != null)
                    {
                        npcInfo = new Tuple<string, Vector2i>(master.Name, master.Position);
                        break;
                    }

                    master = LokiPoe.ObjectManager.Elreon;
                    if (master != null)
                    {
                        npcInfo = new Tuple<string, Vector2i>(master.Name, master.Position);
                        break;
                    }

                    master = LokiPoe.ObjectManager.Catarina;
                    if (master != null)
                    {
                        npcInfo = new Tuple<string, Vector2i>(master.Name, master.Position);
                        break;
                    }

                    master = LokiPoe.ObjectManager.Vagan;
                    if (master != null)
                    {
                        npcInfo = new Tuple<string, Vector2i>(master.Name, master.Position);
                        break;
                    }

                    master = LokiPoe.ObjectManager.Tora;
                    if (master != null)
                    {
                        npcInfo = new Tuple<string, Vector2i>(master.Name, master.Position);
                        break;
                    }

                    master = LokiPoe.ObjectManager.Zana;
                    if (master != null)
                    {
                        npcInfo = new Tuple<string, Vector2i>(master.Name, master.Position);
                        break;
                    }

                    return SellItemsCoroutineError.NoNpc;
                } while (false);
            }
            else
            {
                npcInfo = Utility.GuessWeaponsNpcLocation();
                if (LokiPoe.CurrentWorldArea.Act != 3)
                {
                    if (LokiPoe.Random.Next(0, 2) == 1)
                    {
                        npcInfo = Utility.GuessAccessoryNpcLocation();
                    }
                }
            }

            // The default selling coroutine will sell no items.
            if (shouldSellItem == null)
            {
                shouldSellItem = (i, u) => false;
            }

            // The default selling coroutine will accept any offer.
            if (shouldAcceptOffer == null)
            {
                shouldAcceptOffer = () => true;
            }

            // If we don't have any items to sell, we don't need to continue.
            if (
                !LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                    .Where(i => i.Rarity != Rarity.Quest && !i.HasSkillGemsEquipped && !i.HasMicrotransitionAttachment)
                    .Any(i => shouldSellItem(i, null)))
            {
                return SellItemsCoroutineError.None;
            }

            // Handle opening the sell panel.
            if (!LokiPoe.InGameState.SellUi.IsOpened)
            {
                Log.DebugFormat(
                    "[SellItemsCoroutine] The Npc sell window is not open. Now moving to a vendor to open it.");

                var ttnerr = await TalkToNpc(npcInfo.Item1);
                if (ttnerr != TalkToNpcError.None)
                {
                    Log.ErrorFormat("[SellItemsCoroutine] TalkToNpc returned {0}.", ttnerr);
                    return SellItemsCoroutineError.TalkToNpcFailed;
                }

                await Coroutines.LatencyWait();
                await Coroutines.ReactionWait();

                while (LokiPoe.InGameState.NpcDialogUi.DialogDepth != 1)
                {
                    if (LokiPoe.InGameState.NpcDialogUi.DialogDepth == 2)
                    {
                        Log.DebugFormat("[SellItemsCoroutine] Now closing a dialog/reward window.");
                        LokiPoe.Input.SimulateKeyEvent(Keys.Escape, true, false, false);
                        // Give the client enough time to close the gui itself. It waits for the server to show the new one.
                        await Coroutine.Sleep(1000);
                    }
                    else
                    {
                        Log.InfoFormat("[SellItemsCoroutine] Waiting for the Npc window to open.");
                        await Coroutines.ReactionWait();
                    }
                }

                var cerr = LokiPoe.InGameState.NpcDialogUi.SellItems();
                if (cerr != LokiPoe.InGameState.ConverseResult.None)
                {
                    Log.ErrorFormat("[SellItemsCoroutine] NpcDialogUi.SellItems returned {0}.", ttnerr);
                    return SellItemsCoroutineError.ConverseFailed;
                }

                await Coroutines.LatencyWait();
                await Coroutines.ReactionWait();

                if (!await WaitForSellPanel())
                {
                    return SellItemsCoroutineError.SellPanelDidNotopen;
                }
            }

            Log.DebugFormat("[SellItemsCoroutine] The Sell window is open. Now moving items to sell.");

            foreach (
                var item in
                    LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main)
                        .Where(i => i.Rarity != Rarity.Quest && !i.HasSkillGemsEquipped && !i.HasMicrotransitionAttachment)
                        .ToList()
                        .OrderBy(i => i.LocationTopLeft.X)
                        .ThenBy(i => i.LocationTopLeft.Y))
            {
                // Handle window closing.
                if (!LokiPoe.InGameState.SellUi.IsOpened)
                {
                    return SellItemsCoroutineError.SellClosed;
                }
                if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                {
                    return SellItemsCoroutineError.InventoryClosed;
                }

                if (!shouldSellItem(item, user))
                    continue;

                Log.DebugFormat("[SellItemsCoroutine] Now fast moving the item {0} into the current vendor tab.",
                    item.Name);

                var ba = item.BaseAddress;

                var fmerr = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.FastMove(item.LocalId);
                if (fmerr != FastMoveResult.None)
                {
                    Log.ErrorFormat("[SellItemsCoroutine] FastMove returned {0}.", fmerr);

                    var cerr = LokiPoe.InGameState.SellUi.TradeControl.Cancel();
                    if (cerr != TradeResult.None)
                    {
                        Log.ErrorFormat("[SellItemsCoroutine] SellUi.Cancel returned {0}.", cerr);
                    }

                    await Coroutines.LatencyWait();
                    await Coroutines.ReactionWait();

                    while (LokiPoe.InGameState.SellUi.IsOpened)
                    {
                        cerr = LokiPoe.InGameState.SellUi.TradeControl.Cancel();
                        Log.ErrorFormat(
                            "[SellItemsCoroutine] Cancel returned {0}. Waiting for the SellUi to close (FastMove failed).", cerr);
                        await Coroutines.ReactionWait();
                    }

                    return SellItemsCoroutineError.FastMoveFailed;
                }

                await Coroutines.LatencyWait();

                Stopwatch sw = Stopwatch.StartNew();
                var wait = 3000;
                while (LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).Any(i => i.BaseAddress == ba))
                {
                    Log.InfoFormat("[SellItemsCoroutine] Waiting for the item to be moved.");
                    await Coroutine.Sleep(1);
                    if (sw.ElapsedMilliseconds > wait)
                    {
                        Log.InfoFormat("[SellItemsCoroutine] Timeout while waiting for the item to be moved.");
                        break;
                    }
                }

                await Coroutines.ReactionWait();
            }

            Log.DebugFormat("[SellItemsCoroutine] Moving items to sell has completed.");

            if (!shouldAcceptOffer())
            {
                var cerr = LokiPoe.InGameState.SellUi.TradeControl.Cancel();
                if (cerr != TradeResult.None)
                {
                    Log.ErrorFormat("[SellItemsCoroutine] Cancel returned {0}.", cerr);
                }

                await Coroutines.LatencyWait();
                await Coroutines.ReactionWait();

                while (LokiPoe.InGameState.SellUi.IsOpened)
                {
                    cerr = LokiPoe.InGameState.SellUi.TradeControl.Cancel();
                    Log.ErrorFormat(
                        "[SellItemsCoroutine] Cancel returned {0}. Waiting for the SellUi to close (Offer refused).", cerr);
                    await Coroutines.ReactionWait();
                }

                return SellItemsCoroutineError.OfferRefused;
            }

            var aerr = LokiPoe.InGameState.SellUi.TradeControl.Accept();
            if (aerr != TradeResult.None)
            {
                Log.ErrorFormat("[SellItemsCoroutine] Accept returned {0}.", aerr);
                return SellItemsCoroutineError.AcceptFailed;
            }

            await Coroutines.LatencyWait();
            await Coroutines.ReactionWait();

            while (LokiPoe.InGameState.SellUi.IsOpened)
            {
                aerr = LokiPoe.InGameState.SellUi.TradeControl.Accept();
                if (aerr != TradeResult.None)
                {
                    Log.ErrorFormat("[SellItemsCoroutine] Accept returned {0}.", aerr);
                    return SellItemsCoroutineError.AcceptFailed;
                }

                Log.DebugFormat("[SellItemsCoroutine] Waiting for the SellUi to close (Offer Accepted).");

                await Coroutines.LatencyWait();
                await Coroutines.ReactionWait();
            }

            return SellItemsCoroutineError.None;
        }

        /// <summary>
        /// This delegate is used to determine if the bot needs to stash.
        /// </summary>
        /// <returns>true if the bot needs to stash, and false otherwise.</returns>
        public delegate bool NeedsToStashDelegate();

        /// <summary>
        /// This delegate is called when stashing is completed.
        /// </summary>
        public delegate void StashingCompleteDelegate();

        /// <summary>
        /// The delegate used to determine if an item should be stashed.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <param name="user">The user object passed to StashItemsCoroutine.</param>
        /// <returns>true if the item should be stashed and false otherwise.</returns>
        public delegate bool StashItemsDelegate(Item item, object user);

        /// <summary>
        /// The delegate used to determine where an item should be stashed.
        /// </summary>
        /// <param name="tab">The tab to check.</param>
        /// <param name="inventory">The tab's inventory.</param>
        /// <param name="item">The item being stashed.</param>
        /// <param name="user">The user object passed to StashItemsCoroutine.</param>
        /// <returns>true if the item should be stashed to this tab and false otherwise.</returns>
        public delegate bool BestStashTabForItem(StashTabInfo tab, Inventory inventory, Item item, object user);

        /// <summary>Coroutine results for StashItemsCoroutine.</summary>
        public enum StashItemsCoroutineError
        {
            /// <summary>All items were stashed.</summary>
            None,

            /// <summary>An item on the cursor could not be placed into the stash tab.</summary>
            CursorItemCouldNotBePlaced,

            /// <summary>An item did not fit in the inventory.</summary>
            CouldNotFitAllItems,

            /// <summary>The OpenStash function did not complete successfully.</summary>
            OpenStashFailed,

            /// <summary>The GoToFirstTab function failed.</summary>
            GoToFirstTabFailed,

            /// <summary>The stash operation has been interupted by the stash window being closed.</summary>
            StashClosed,

            /// <summary>The stash operation has been interupted by the inventory window being closed.</summary>
            InventoryClosed,
        }

        /// <summary>
        /// Waits for a stash tab to change. Pass -1 to lastId to wait for the initial tab.
        /// </summary>
        /// <param name="lastId">The last InventoryId before changing tabs.</param>
        /// <param name="timeout">The timeout of the function.</param>
        /// <returns>true if the tab was changed and false otherwise.</returns>
        public static async Task<bool> WaitForStashTabChange(int lastId, int timeout = 5000)
        {
            var sw = Stopwatch.StartNew();
            var invTab = LokiPoe.InGameState.StashUi.StashTabInfo;
            while (invTab == null || invTab.InventoryId == lastId)
            {
                Log.InfoFormat("[WaitForStashTabChange] Waiting...");
                await Coroutine.Sleep(1);
                invTab = LokiPoe.InGameState.StashUi.StashTabInfo;
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.InfoFormat("[WaitForStashTabChange] Timeout...");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Waits for a guild stash tab to change. Pass -1 to lastId to wait for the initial tab.
        /// </summary>
        /// <param name="lastId">The last InventoryId before changing tabs.</param>
        /// <param name="timeout">The timeout of the function.</param>
        /// <returns>true if the tab was changed and false otherwise.</returns>
        public static async Task<bool> WaitForGuildStashTabChange(int lastId, int timeout = 5000)
        {
            var sw = Stopwatch.StartNew();
            var invTab = LokiPoe.InGameState.GuildStashUi.StashTabInfo;
            while (invTab == null || invTab.InventoryId == lastId)
            {
                Log.InfoFormat("[WaitForGuildStashTabChange] Waiting...");
                await Coroutine.Sleep(1);
                invTab = LokiPoe.InGameState.GuildStashUi.StashTabInfo;
                if (sw.ElapsedMilliseconds > timeout)
                {
                    Log.InfoFormat("[WaitForGuildStashTabChange] Timeout...");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This coroutine will deposit all items into Stash that match the specified
        /// StashItemsDelegate. If no StashItemsDelegate is used, the default delegate
        /// will stash all non-quest items.
        /// </summary>
        /// <param name="shouldStashItem">The delegate that determines if an item should be stashed.</param>
        /// <param name="bestStashTabForItem">The delegate that determines where an item should be stashed.</param>
        /// <param name="user">The user object that should be passed through the StashItemsDelegate.</param>
        /// <param name="triggerOnStash">The delegate to call when an item is stashed.</param>
        /// <returns>A StashItemsCoroutineError that describes the result.</returns>
        public static async Task<StashItemsCoroutineError> StashItemsCoroutine(
            StashItemsDelegate shouldStashItem = null,
            BestStashTabForItem bestStashTabForItem = null,
            object user = null,
            TriggerOnStashDelegate triggerOnStash = null
            )
        {
            // The default stashing coroutine will stash all items.
            if (shouldStashItem == null)
            {
                shouldStashItem = (i, u) => true;
            }

            // The default stashing coroutine will stash at the first page available.
            if (bestStashTabForItem == null)
            {
                bestStashTabForItem = (t, iv, i, u) => true;
            }

            // Handle opening the stash.
            if (!LokiPoe.InGameState.StashUi.IsOpened)
            {
                Log.DebugFormat("[StashItemsCoroutine] The Stash window is not open. Now moving to Stash to open it.");

                var otserr = await OpenStash();
                if (otserr != OpenStashError.None)
                {
                    Log.ErrorFormat("[StashItemsCoroutine] OpenStash returned {0}.", otserr);
                    return StashItemsCoroutineError.OpenStashFailed;
                }

                await Coroutines.ReactionWait();
            }

            Log.DebugFormat("[StashItemsCoroutine] The Stash window is open. Now going to the first tab.");

            // We want to go to the first tab, since once we've requested tab contents, it's free.
            var result = LokiPoe.InGameState.StashUi.TabControl.SwitchToTabMouse(0);
            if (result != SwitchToTabResult.None)
            {
                Log.DebugFormat("[StashItemsCoroutine] GoToFirstTab failed.");

                return StashItemsCoroutineError.GoToFirstTabFailed;
            }

            await Coroutines.ReactionWait();

            await WaitForStashTabChange(-1);

            // Loop until something breaks.
            while (true)
            {
                // Handle window closing.
                if (!LokiPoe.InGameState.StashUi.IsOpened)
                {
                    return StashItemsCoroutineError.StashClosed;
                }
                if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                {
                    return StashItemsCoroutineError.InventoryClosed;
                }

                // As long as the tab stays the same, we should be able to cache these.
                var invTab = LokiPoe.InGameState.StashUi.StashTabInfo;
                if (invTab.IsRemoveOnly || invTab.IsPublic) // TODO: Handle this better
                {
                    if (invTab.IsRemoveOnly)
                        Log.DebugFormat("[StashItemsCoroutine] The current tab is Remove only. Skipping it.");
                    else
                        Log.DebugFormat("[IdItemsUsingStashCoroutine] The current tab is Public. Skipping it.");

                    var oldId = invTab.InventoryId;

                    result = LokiPoe.InGameState.StashUi.TabControl.NextTabKeyboard();
                    if (result != SwitchToTabResult.None)
                    {
                        Log.ErrorFormat("[IdItemsUsingStashCoroutine] NextTabKeyboard returned {0}.", result);

                        break;
                    }

                    await WaitForStashTabChange(oldId);

                    await Coroutines.ReactionWait();

                    continue;
                }

                var inv = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.Inventory;

                if (invTab.IsPremiumCurrency)
                {
                    foreach (var item in inv.Items.Where(i => i.Rarity == Rarity.Currency).ToList())
                    {
                        // Handle window closing.
                        if (!LokiPoe.InGameState.StashUi.IsOpened)
                        {
                            return StashItemsCoroutineError.StashClosed;
                        }
                        if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                        {
                            return StashItemsCoroutineError.InventoryClosed;
                        }

                        // Make sure this item should be stashed.
                        if (!shouldStashItem(item, user))
                            continue;

                        // Lookup the inventory control for this particular currency item. If there is none, it's not compatbile with this tab.
                        var invControl = LokiPoe.InGameState.StashUi.GetInventoryControlForMetadata(item.Metadata);
                        if (invControl == null)
                            continue;

                        // Make sure the user wants to stash currency to this tab.
                        if (!bestStashTabForItem(invTab, invControl.Inventory, item, user))
                            continue;

                        // Make sure the item will fit in the currency tab.
                        var existing = invControl.CurrencyTabItem;
                        if (existing != null)
                        {
                            if (existing.StackCount == existing.MaxCurrencyTabStackCount)
                            {
                                continue;
                            }
                        }

                        Log.DebugFormat("[StashItemsCoroutine] Now fast moving the item {0} into the current currency stash tab.",
                            item.Name);

                        if (triggerOnStash != null)
                        {
                            triggerOnStash(item);
                        }

                        var ba = item.BaseAddress;

                        var fmerr = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.FastMove(item.LocalId);
                        if (fmerr != FastMoveResult.None)
                        {
                            Log.ErrorFormat("[StashItemsCoroutine] FastMove returned {0}.", fmerr);
                        }

                        await Coroutines.LatencyWait();

                        Stopwatch sw = Stopwatch.StartNew();
                        var wait = 3000;
                        while (LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).Any(i => i.BaseAddress == ba))
                        {
                            Log.InfoFormat("[StashItemsCoroutine] Waiting for the item to be moved.");
                            await Coroutine.Sleep(1);
                            if (sw.ElapsedMilliseconds > wait)
                            {
                                Log.InfoFormat("[StashItemsCoroutine] Timeout while waiting for the item to be moved.");
                                break;
                            }
                        }

                        await Coroutines.ReactionWait();
                    }
                }
                else
                {
                    var invControl = LokiPoe.InGameState.StashUi.InventoryControl;

                    foreach (
                        var item in inv.Items.Where(i => i.Rarity != Rarity.Quest).ToList())
                    {
                        // Handle window closing.
                        if (!LokiPoe.InGameState.StashUi.IsOpened)
                        {
                            return StashItemsCoroutineError.StashClosed;
                        }
                        if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                        {
                            return StashItemsCoroutineError.InventoryClosed;
                        }

                        if (!shouldStashItem(item, user))
                            continue;

                        if (!bestStashTabForItem(invTab, invControl.Inventory, item, user))
                            continue;

                        // Handle currency items
                        if (item.IsStackable)
                        {
                            // First, see if we can merge stacks automatically.
                            var name = item.Name;
                            if (!invControl.Inventory.Items.Any(i => i.Name == name && i.StackCount < i.MaxStackCount))
                            {
                                // Now check for space.
                                if (!LokiPoe.InGameState.StashUi.InventoryControl.Inventory.CanFitItem(item))
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (!LokiPoe.InGameState.StashUi.InventoryControl.Inventory.CanFitItem(item))
                            {
                                continue;
                            }
                        }

                        Log.DebugFormat("[StashItemsCoroutine] Now fast moving the item {0} into the current stash tab.",
                            item.Name);

                        if (triggerOnStash != null)
                        {
                            triggerOnStash(item);
                        }

                        var ba = item.BaseAddress;

                        var fmerr = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.FastMove(item.LocalId);
                        if (fmerr != FastMoveResult.None)
                        {
                            Log.ErrorFormat("[StashItemsCoroutine] FastMove returned {0}.", fmerr);
                        }

                        await Coroutines.LatencyWait();

                        Stopwatch sw = Stopwatch.StartNew();
                        var wait = 3000;
                        while (LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).Any(i => i.BaseAddress == ba))
                        {
                            Log.InfoFormat("[StashItemsCoroutine] Waiting for the item to be moved.");
                            await Coroutine.Sleep(1);
                            if (sw.ElapsedMilliseconds > wait)
                            {
                                Log.InfoFormat("[StashItemsCoroutine] Timeout while waiting for the item to be moved.");
                                break;
                            }
                        }

                        await Coroutines.ReactionWait();
                    }
                }

                if (
                    LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).Where(i => i.Rarity != Rarity.Quest)
                        .Any(i => shouldStashItem(i, null)))
                {
                    var oldId = invTab.InventoryId;

                    result = LokiPoe.InGameState.StashUi.TabControl.NextTabKeyboard();
                    if (result != SwitchToTabResult.None)
                    {
                        Log.ErrorFormat("[StashItemsCoroutine] NextTabKeyboard returned {0}.", result);
                        break;
                    }

                    await Coroutines.LatencyWait();

                    await WaitForStashTabChange(oldId);

                    await Coroutines.ReactionWait();
                }
                else
                {
                    break;
                }
            }

            if (
                LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main).Where(i => i.Rarity != Rarity.Quest)
                    .Any(i => shouldStashItem(i, null)))
            {
                return StashItemsCoroutineError.CouldNotFitAllItems;
            }

            return StashItemsCoroutineError.None;
        }

        /// <summary>
        /// The delegate used to determine if an item should be withdrawn.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <param name="count">The number of items.</param>
        /// <param name="user">The user object passed to WithdrawItemsCoroutine.</param>
        /// <returns>true if the item should be withdrawn and false otherwise.</returns>
        public delegate bool WithdrawItemsDelegate(Item item, out int count, object user);

        /// <summary>
        /// The delegate used to determine if a tab should be withdrawn from.
        /// </summary>
        /// <param name="tab">The current tab being checked.</param>
        /// <param name="user">The user object passed to WithdrawItemsCoroutine.</param>
        /// <returns>true if items should be withdrawn from this tab, and false otherwise.</returns>
        public delegate bool WithdrawFromTabDelegate(StashTabInfo tab, object user);

        /// <summary>
        /// The delegate used to determine if withdrawing should continue.
        /// </summary>
        /// <param name="user">The user object passed to WithdrawItemsCoroutine.</param>
        /// <returns>true if withdrawing should continue and false otherwise.</returns>
        public delegate bool ContinueWithdrawingDelegate(object user);

        /// <summary>Coroutine results for withdrawItemsCoroutine.</summary>
        public enum WithdrawItemsCoroutineError
        {
            /// <summary>All items have been withdrawn.</summary>
            None,

            /// <summary>The OpenStash function did not complete successfully.</summary>
            OpenStashFailed,

            /// <summary>The GoToFirstTab function failed.</summary>
            GoToFirstTabFailed,

            /// <summary>The id operation has been interupted by the inventory window being closed.</summary>
            InventoryClosed,

            /// <summary>The FastMove function failed.</summary>
            FastMoveFailed,

            /// <summary>The id operation has been interupted by the stash window being closed.</summary>
            StashClosed,

            /// <summary>Withdrawing cannot continue, because the main inventory is full.</summary>
            InventoryFull
        }

        /// <summary>
        /// This coroutine will withdraw items from stash that meet user requirments. Only full stacks of items can
        /// be withdrawn for the time being, as the logic for handling split stacks and properly inventory mergining
        /// is not implemented.
        /// </summary>
        /// <param name="shouldWithdrawFromTab">The delegate that determines if the tab should be used for withdrawing.</param>
        /// <param name="shouldWithdrawItem">The delegate that determines if an item should be withdrawn.</param>
        /// <param name="shouldContinue">The delegate that determines if withdrawing should continue.</param>
        /// <param name="user">The user object that should be passed through the delegates.</param>
        /// <returns>A WithdrawItemsCoroutineError that describes the result.</returns>
        public static async Task<WithdrawItemsCoroutineError> WithdrawItemsCoroutine(
            WithdrawFromTabDelegate shouldWithdrawFromTab,
            WithdrawItemsDelegate shouldWithdrawItem,
            ContinueWithdrawingDelegate shouldContinue,
            object user = null)
        {
            // The default withdraw coroutine will not withdraw anything when passed a null item delegate.
            if (shouldWithdrawItem == null)
            {
                return WithdrawItemsCoroutineError.None;
            }

            // The default withdraw coroutine will only process non-remove only tabs.
            if (shouldWithdrawFromTab == null)
            {
                shouldWithdrawFromTab = (t, u) => !t.IsRemoveOnly && !t.IsPublic;
            }

            // We need both opened.
            if (!LokiPoe.InGameState.StashUi.IsOpened || !LokiPoe.InGameState.InventoryUi.IsOpened)
            {
                await Coroutines.CloseBlockingWindows();

                var otserr = await OpenStash();
                if (otserr != OpenStashError.None)
                {
                    Log.ErrorFormat("[WithdrawItemsCoroutine] OpenStash returned {0}.", otserr);
                    return WithdrawItemsCoroutineError.OpenStashFailed;
                }

                await Coroutines.ReactionWait();
            }

            if (!LokiPoe.InGameState.StashUi.IsOpened)
            {
                return WithdrawItemsCoroutineError.StashClosed;
            }

            if (!LokiPoe.InGameState.InventoryUi.IsOpened)
            {
                return WithdrawItemsCoroutineError.InventoryClosed;
            }

            // We want to go to the first tab, since once we've requested tab contents, it's free.
            var result = LokiPoe.InGameState.StashUi.TabControl.SwitchToTabMouse(0);
            if (result != SwitchToTabResult.None)
            {
                return WithdrawItemsCoroutineError.GoToFirstTabFailed;
            }

            await Coroutines.ReactionWait();

            await WaitForStashTabChange(-1);
            var oldId = -1;

            // Loop until something breaks.
            while (true)
            {
                // Handle window closing.
                if (!LokiPoe.InGameState.StashUi.IsOpened)
                {
                    return WithdrawItemsCoroutineError.StashClosed;
                }
                if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                {
                    return WithdrawItemsCoroutineError.InventoryClosed;
                }

                // As long as the tab stays the same, we should be able to cache these.
                var invTab = LokiPoe.InGameState.StashUi.StashTabInfo;
                if (invTab.IsRemoveOnly || invTab.IsPublic) // TODO: Handle this better
                {
                    if (invTab.IsRemoveOnly)
                        Log.DebugFormat("[WithdrawItemsCoroutine] The current tab is Remove only. Skipping it.");
                    else
                        Log.DebugFormat("[WithdrawItemsCoroutine] The current tab is Public. Skipping it.");

                    oldId = invTab.InventoryId;

                    result = LokiPoe.InGameState.StashUi.TabControl.NextTabKeyboard();
                    if (result != SwitchToTabResult.None)
                    {
                        Log.ErrorFormat("[IdItemsUsingStashCoroutine] NextTabKeyboard returned {0}.", result);

                        break;
                    }

                    await WaitForStashTabChange(oldId);

                    await Coroutines.ReactionWait();

                    continue;
                }

                // Check to see if this tab should be withdrawn from.
                if (!shouldWithdrawFromTab(invTab, user))
                {
                    Log.DebugFormat(
                        "[WithdrawItemsCoroutine] The current tab should not be withdrawn from. Skipping it.");

                    if (!shouldContinue(user))
                    {
                        Log.InfoFormat(
                            "[WithdrawItemsCoroutine] shouldContinue returned false. Now breaking out of the tab loop.");
                        break;
                    }

                    oldId = invTab.InventoryId;

                    result = LokiPoe.InGameState.StashUi.TabControl.NextTabKeyboard();
                    if (result != SwitchToTabResult.None)
                    {
                        Log.ErrorFormat("[WithdrawItemsCoroutine] NextTabKeyboard returned {0}.", result);

                        break;
                    }

                    await WaitForStashTabChange(oldId);

                    await Coroutines.ReactionWait();

                    continue;
                }

                var doBreak = false;

                if (invTab.IsPremiumCurrency)
                {
                    foreach (var inv in LokiPoe.InGameState.StashUi.CurrencyTabInventoryControls)
                    {
                        var item = inv.CurrencyTabItem;
                        if (item == null)
                            continue;

                        int count;
                        if (shouldWithdrawItem(item, out count, user))
                        {
                            if (!LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Main).CanFitItem(item))
                            {
                                return WithdrawItemsCoroutineError.InventoryFull;
                            }

                            var name = item.Name;

                            var ba = item.BaseAddress;

                            var fmerr = inv.FastMove();
                            if (fmerr != FastMoveResult.None)
                            {
                                Log.ErrorFormat("[WithdrawItemsCoroutine] FastMove returned {0} for {1}.", fmerr, name);
                                return WithdrawItemsCoroutineError.FastMoveFailed;
                            }

                            /*if (count == item.StackCount)
							{
								if (item.MaxStackCount == 1)
								{
								}
								else
								{
								}
							}
							else
							{
							}*/

                            await Coroutines.LatencyWait();

                            Stopwatch sw = Stopwatch.StartNew();
                            var wait = 3000;
                            while (inv.Inventory.Items.Any(i => i.BaseAddress == ba))
                            {
                                Log.InfoFormat("[WithdrawItemsCoroutine] Waiting for the item to be moved.");
                                await Coroutine.Sleep(1);
                                if (sw.ElapsedMilliseconds > wait)
                                {
                                    Log.InfoFormat("[WithdrawItemsCoroutine] Timeout while waiting for the item to be moved.");
                                    break;
                                }
                            }

                            await Coroutines.ReactionWait();

                            if (!shouldContinue(user))
                            {
                                Log.InfoFormat(
                                    "[WithdrawItemsCoroutine] shouldContinue returned false. Now breaking out of the withdraw loop.");
                                doBreak = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var inv = LokiPoe.InGameState.StashUi.InventoryControl.Inventory;

                    // Process all items on the tab.
                    foreach (var item in inv.Items.ToList())
                    {
                        int count;
                        if (shouldWithdrawItem(item, out count, user))
                        {
                            if (!LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Main).CanFitItem(item))
                            {
                                return WithdrawItemsCoroutineError.InventoryFull;
                            }

                            var name = item.Name;

                            var ba = item.BaseAddress;

                            var fmerr = LokiPoe.InGameState.StashUi.InventoryControl.FastMove(item.LocalId);
                            if (fmerr != FastMoveResult.None)
                            {
                                Log.ErrorFormat("[WithdrawItemsCoroutine] FastMove returned {0} for {1}.", fmerr, name);
                                return WithdrawItemsCoroutineError.FastMoveFailed;
                            }

                            /*if (count == item.StackCount)
							{
								if (item.MaxStackCount == 1)
								{
								}
								else
								{
								}
							}
							else
							{
							}*/

                            await Coroutines.LatencyWait();

                            Stopwatch sw = Stopwatch.StartNew();
                            var wait = 3000;
                            while (inv.Items.Any(i => i.BaseAddress == ba))
                            {
                                Log.InfoFormat("[WithdrawItemsCoroutine] Waiting for the item to be moved.");
                                await Coroutine.Sleep(1);
                                if (sw.ElapsedMilliseconds > wait)
                                {
                                    Log.InfoFormat("[WithdrawItemsCoroutine] Timeout while waiting for the item to be moved.");
                                    break;
                                }
                            }

                            await Coroutines.ReactionWait();

                            if (!shouldContinue(user))
                            {
                                Log.InfoFormat(
                                    "[WithdrawItemsCoroutine] shouldContinue returned false. Now breaking out of the withdraw loop.");
                                doBreak = true;
                                break;
                            }
                        }
                    }
                }

                if (doBreak)
                {
                    break;
                }

                if (!shouldContinue(user))
                {
                    Log.InfoFormat(
                        "[WithdrawItemsCoroutine] shouldContinue returned false. Now breaking out of the tab loop.");
                    break;
                }

                oldId = invTab.InventoryId;

                result = LokiPoe.InGameState.StashUi.TabControl.NextTabKeyboard();
                if (result != SwitchToTabResult.None)
                {
                    Log.ErrorFormat("[WithdrawItemsCoroutine] NextTabKeyboard returned {0}.", result);

                    break;
                }

                await WaitForStashTabChange(oldId);

                await Coroutines.ReactionWait();
            }

            return WithdrawItemsCoroutineError.None;
        }
    }

    /// <summary>
    /// Shared utility functions.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Checks for a closed door between start and end.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="distanceFromPoint">How far to check around each point for a door object.</param>
        /// <param name="stride">The distance between points to check in the path.</param>
        /// <param name="dontLeaveFrame">Should the current frame not be left?</param>
        /// <returns>true if there's a closed door and false otherwise.</returns>
        public static bool ClosedDoorBetween(NetworkObject start, NetworkObject end, int distanceFromPoint = 10,
            int stride = 10, bool dontLeaveFrame = false)
        {
            return ClosedDoorBetween(start.Position, end.Position, distanceFromPoint, stride, dontLeaveFrame);
        }

        /// <summary>
        /// Checks for a closed door between start and end.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="distanceFromPoint">How far to check around each point for a door object.</param>
        /// <param name="stride">The distance between points to check in the path.</param>
        /// <param name="dontLeaveFrame">Should the current frame not be left?</param>
        /// <returns>true if there's a closed door and false otherwise.</returns>
        public static bool ClosedDoorBetween(NetworkObject start, Vector2i end, int distanceFromPoint = 10,
            int stride = 10, bool dontLeaveFrame = false)
        {
            return ClosedDoorBetween(start.Position, end, distanceFromPoint, stride, dontLeaveFrame);
        }

        /// <summary>
        /// Checks for a closed door between start and end.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="distanceFromPoint">How far to check around each point for a door object.</param>
        /// <param name="stride">The distance between points to check in the path.</param>
        /// <param name="dontLeaveFrame">Should the current frame not be left?</param>
        /// <returns>true if there's a closed door and false otherwise.</returns>
        public static bool ClosedDoorBetween(Vector2i start, NetworkObject end, int distanceFromPoint = 10,
            int stride = 10, bool dontLeaveFrame = false)
        {
            return ClosedDoorBetween(start, end.Position, distanceFromPoint, stride, dontLeaveFrame);
        }

        /// <summary>
        /// Checks for a closed door between start and end.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="distanceFromPoint">How far to check around each point for a door object.</param>
        /// <param name="stride">The distance between points to check in the path.</param>
        /// <param name="dontLeaveFrame">Should the current frame not be left?</param>
        /// <returns>true if there's a closed door and false otherwise.</returns>
        public static bool ClosedDoorBetween(Vector2i start, Vector2i end, int distanceFromPoint = 10, int stride = 10,
            bool dontLeaveFrame = false)
        {
            var doors = LokiPoe.ObjectManager.Doors.Where(d => !d.IsOpened).ToList();

            if (!doors.Any())
                return false;

            var path = ExilePather.GetPointsOnSegment(start, end, dontLeaveFrame);

            for (var i = 0; i < path.Count; i += stride)
            {
                foreach (var door in doors)
                {
                    if (door.Position.Distance(path[i]) <= distanceFromPoint)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="distanceFromPoint"></param>
        /// <param name="dontLeaveFrame">Should the current frame not be left?</param>
        /// <returns></returns>
        public static int NumberOfMobsBetween(NetworkObject start, NetworkObject end, int distanceFromPoint = 5,
            bool dontLeaveFrame = false)
        {
            var mobs = LokiPoe.ObjectManager.GetObjectsByType<Monster>().Where(d => d.IsActive).ToList();
            if (!mobs.Any())
                return 0;

            var path = ExilePather.GetPointsOnSegment(start.Position, end.Position, dontLeaveFrame);

            var count = 0;
            for (var i = 0; i < path.Count; i += 10)
            {
                foreach (var mob in mobs)
                {
                    if (mob.Position.Distance(path[i]) <= distanceFromPoint)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Returns the number of mobs near a target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="distance"></param>
        /// <param name="dead"></param>
        /// <returns></returns>
        public static int NumberOfMobsNear(NetworkObject target, float distance, bool dead = false)
        {
            var mpos = target.Position;

            var curCount = 0;

            foreach (var mob in LokiPoe.ObjectManager.Objects.OfType<Monster>())
            {
                if (mob.Id == target.Id)
                {
                    continue;
                }

                // If we're only checking for dead mobs... then... yeah...
                if (dead)
                {
                    if (!mob.IsDead)
                    {
                        continue;
                    }
                }
                else if (!mob.IsActive)
                {
                    continue;
                }

                if (mob.Position.Distance(mpos) < distance)
                {
                    curCount++;
                }
            }

            return curCount;
        }

        /// <summary>
        /// Returns a location where a portal should be if we're in a town.
        /// </summary>
        /// <returns>A location where a portal should come into view.</returns>
        public static Vector2i GuessPortalLocation()
        {
            var curArea = LokiPoe.LocalData.WorldArea.Id.ToLowerInvariant();

            if (curArea.EndsWith("1_town"))
            {
                return new Vector2i(151, 246);
            }

            if (curArea.EndsWith("2_town"))
            {
                return new Vector2i(246, 165);
            }

            if (curArea.EndsWith("3_town"))
            {
                return new Vector2i(217, 226);
            }

            if (curArea.EndsWith("4_town"))
            {
                return new Vector2i(286, 491);
            }

            throw new Exception(String.Format("GuessPortalLocation called when curArea = {0}", curArea));
        }

        /// <summary>
        /// Returns a location where the stash should be if we're in a town.
        /// </summary>
        /// <returns>A location where the stash should come into view.</returns>
        public static Vector2i GuessStashLocation()
        {
            var curArea = LokiPoe.LocalData.WorldArea.Id.ToLowerInvariant();

            if (curArea.EndsWith("1_town"))
            {
                return new Vector2i(246, 266);
            }

            if (curArea.EndsWith("2_town"))
            {
                return new Vector2i(178, 195);
            }

            if (curArea.EndsWith("3_town"))
            {
                return new Vector2i(206, 306);
            }

            if (curArea.EndsWith("4_town"))
            {
                return new Vector2i(199, 509);
            }

            throw new Exception(string.Format("GuessStashLocation called when curArea = {0}", curArea));
        }

        /// <summary>
        /// Returns a location where the waypoint should be if we're in a town.
        /// </summary>
        /// <returns>A location where the waypoint should come into view.</returns>
        public static Vector2i GuessWaypointLocation()
        {
            var curArea = LokiPoe.LocalData.WorldArea.Id.ToLowerInvariant();

            if (curArea.EndsWith("1_town"))
            {
                return new Vector2i(196, 172);
            }

            if (curArea.EndsWith("2_town"))
            {
                return new Vector2i(188, 135);
            }

            if (curArea.EndsWith("3_town"))
            {
                return new Vector2i(217, 226);
            }

            if (curArea.EndsWith("4_town"))
            {
                return new Vector2i(286, 491);
            }

            throw new Exception(String.Format("GuessWaypointLocation called when curArea = {0}", curArea));
        }

        /// <summary>
        /// Returns a location where the area transition should be if we're in a town.
        /// </summary>
        /// <returns>A location where the waypoint should come into view.</returns>
        public static Vector2i GuessAreaTransitionLocation(string townExitName)
        {
            var curArea = LokiPoe.LocalData.WorldArea.Id.ToLowerInvariant();

            if (curArea.EndsWith("1_town"))
            {
                if (townExitName == "The Coast")
                {
                    return new Vector2i(319, 212);
                }
            }

            if (curArea.EndsWith("2_town"))
            {
                if (townExitName == "The Old Fields")
                {
                    return new Vector2i(299, 173);
                }

                if (townExitName == "The Riverways")
                {
                    return new Vector2i(78, 172);
                }

                if (townExitName == "The Southern Forest")
                {
                    return new Vector2i(186, 89);
                }
            }

            if (curArea.EndsWith("3_town"))
            {
                if (townExitName == "The Slums")
                {
                    return new Vector2i(511, 408);
                }

                if (townExitName == "The City of Sarn")
                {
                    return new Vector2i(296, 75);
                }
            }

            if (curArea.EndsWith("4_town"))
            {
                if (townExitName == "The Aqueduct")
                {
                    return new Vector2i(259, 394);
                }

                if (townExitName == "The Dried Lake")
                {
                    return new Vector2i(94, 441);
                }

                if (townExitName == "The Mines Level 1")
                {
                    return new Vector2i(328, 624);
                }
            }

            throw new Exception(String.Format(
                "GuessAreaTransitionLocation called when curArea = {0} for exit {1}", curArea, townExitName));
        }

        /// <summary>
        /// Returns hardcoded locations for npcs in a town. We need to make sure these don't change while we aren't looking!
        /// Ideally, we'd explore town to find the location if the npc object was not in view.
        /// </summary>
        public static Tuple<string, Vector2i> GuessWeaponsNpcLocation()
        {
            var curArea = LokiPoe.LocalData.WorldArea.Id.ToLowerInvariant();

            var npcName = "";

            if (curArea.EndsWith("1_town"))
            {
                npcName = "Tarkleigh";
            }

            if (curArea.EndsWith("2_town"))
            {
                npcName = "Greust";
            }

            if (curArea.EndsWith("3_town"))
            {
                npcName = "Hargan";
            }

            if (curArea.EndsWith("4_town"))
            {
                npcName = "Kira";
            }

            if (npcName != "")
            {
                return new Tuple<string, Vector2i>(npcName, GuessNpcLocation(npcName));
            }

            throw new Exception(String.Format("GuessWeaponsNpcLocation called when curArea = {0}.", curArea));
        }

        /// <summary>
        /// Returns hardcoded locations for npcs in a town. We need to make sure these don't change while we aren't looking!
        /// Ideally, we'd explore town to find the location if the npc object was not in view.
        /// </summary>
        public static Vector2i GuessNpcLocation(string npcName)
        {
            var curArea = LokiPoe.LocalData.WorldArea.Id.ToLowerInvariant();

            if (curArea.EndsWith("1_town"))
            {
                if (npcName == "Nessa")
                    return new Vector2i(268, 253);

                if (npcName == "Tarkleigh")
                    return new Vector2i(312, 189);
            }

            if (curArea.EndsWith("2_town"))
            {
                if (npcName == "Greust")
                    return new Vector2i(192, 173);

                if (npcName == "Yeena")
                    return new Vector2i(162, 240);
            }

            if (curArea.EndsWith("3_town"))
            {
                if (npcName == "Clarissa")
                    return new Vector2i(147, 326);

                if (npcName == "Hargan")
                    return new Vector2i(281, 357);
            }

            if (curArea.EndsWith("4_town"))
            {
                if (npcName == "Kira")
                    return new Vector2i(169, 500);

                if (npcName == "Petarus and Vanja")
                    return new Vector2i(204, 546);
            }

            return Vector2i.Zero;
        }

        /// <summary>
        /// Returns hardcoded locations for npcs in a town. We need to make sure these don't change while we aren't looking!
        /// Ideally, we'd explore town to find the location if the npc object was not in view.
        /// </summary>
        public static Tuple<string, Vector2i> GuessAccessoryNpcLocation()
        {
            var curArea = LokiPoe.LocalData.WorldArea.Id.ToLowerInvariant();

            var npcName = "";

            if (curArea.EndsWith("1_town"))
            {
                npcName = "Nessa";
            }

            if (curArea.EndsWith("2_town"))
            {
                npcName = "Yeena";
            }

            if (curArea.EndsWith("3_town"))
            {
                npcName = "Clarissa";
            }

            if (curArea.EndsWith("4_town"))
            {
                npcName = "Petarus and Vanja";
            }

            if (npcName != "")
            {
                return new Tuple<string, Vector2i>(npcName, GuessNpcLocation(npcName));
            }

            throw new Exception(String.Format("GuessAccessoryNpcLocation called when curArea = {0}.", curArea));
        }
    }
}