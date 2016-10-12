using System.Windows.Controls;
using System.Threading.Tasks;
using log4net;
using Loki.Bot;
using Loki.Common;

namespace Navali
{
    internal class Navali : IPlugin
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private NavaliSettingsGui _instance;
        private TaskManager _taskManager;
        #region Implementation of IAuthored

        /// <summary> The name of the plugin. </summary>
        public string Name
        {
            get
            {
                return "Navali";
            }
        }

        /// <summary> The description of the plugin. </summary>
        public string Description
        {
            get
            {
                return
                    "Navali";
            }
        }

        /// <summary>The author of the plugin.</summary>
        public string Author
        {
            get
            {
                return "Clandestine";
            }
        }

        /// <summary>The version of the plugin.</summary>
        public string Version
        {
            get
            {
                return "0.0.1.0";
            }
        }

        #endregion

        #region Implementation of IBase

        /// <summary>Initializes this plugin.</summary>
        public void Initialize()
        {
            Log.DebugFormat("[Navali] Initialize");
        }

        /// <summary>Deinitializes this object. This is called when the object is being unloaded from the bot.</summary>
        public void Deinitialize()
        {
            Log.DebugFormat("[Navali] Deinitialize");
        }

        #endregion

        #region Implementation of IRunnable

        /// <summary> The plugin start callback. Do any initialization here. </summary>
        public void Start()
        {
            Log.DebugFormat("[Navali] Start");
            _taskManager = (TaskManager)BotManager.CurrentBot.Execute("GetTaskManager");
            if (_taskManager == null)
            {
                Log.Error("[Navali] Failed to get TaskManager.");
                BotManager.Stop();
                return;
            }

            AddPropheciesTask();

        }

        /// <summary> The plugin tick callback. Do any update logic here. </summary>
        public void Tick()
        {
        }

        /// <summary> The plugin stop callback. Do any pre-dispose cleanup here. </summary>
        public void Stop()
        {
            Log.DebugFormat("[Navali] Stop");
        }

        private void AddPropheciesTask()
        {
            AddTask(new GetPropheciesTask(), "SortInventoryTask", AddType.After);
            AddTask(new SealPropheciesTask(), "GetPropheciesTask", AddType.After);
        }
        #endregion

        #region Implementation of ILogic

        /// <summary>
        /// Coroutine logic to execute.
        /// </summary>
        /// <param name="type">The requested type of logic to execute.</param>
        /// <param name="param">Data sent to the object from the caller.</param>
        /// <returns>true if logic was executed to handle this type and false otherwise.</returns>
        public async Task<bool> Logic(string type, params dynamic[] param)
        {
            return false;
        }


        /// <summary>
        /// Non-coroutine logic to execute.
        /// </summary>
        /// <param name="name">The name of the logic to invoke.</param>
        /// <param name="param">The data passed to the logic.</param>
        /// <returns>Data from the executed logic.</returns>
        public object Execute(string name, params dynamic[] param)
        {
            return true;
        }

        #endregion

        #region Implementation of IConfigurable

        /// <summary>The settings object. This will be registered in the current configuration.</summary>
        public JsonSettings Settings
        {
            get
            {
                return NavaliSettings.Instance;
            }
        }

        /// <summary> The plugin's settings control. This will be added to the Exilebuddy Settings tab.</summary>
        public UserControl Control
        {
            get
            {
                return (_instance ?? (_instance = new NavaliSettingsGui()));
            }
        }

        #endregion

        #region Implementation of IEnableable

        /// <summary> The plugin is being enabled.</summary>
        public void Enable()
        {
            Log.DebugFormat("[Navali] Enable");
        }

        /// <summary> The plugin is being disabled.</summary>
        public void Disable()
        {
            Log.DebugFormat("[Navali] Disable");
        }

        #endregion

        #region Override of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name + ": " + Description;
        }

        #endregion
        #region TaskManager utilities

        private void AddTask(ITask task, string name, AddType type)
        {
            bool added = false;
            switch (type)
            {
                case AddType.Before:
                    added = _taskManager.AddBefore(task, name);
                    break;

                case AddType.After:
                    added = _taskManager.AddAfter(task, name);
                    break;

                case AddType.Replace:
                    added = _taskManager.Replace(name, task);
                    break;
            }
            if (!added)
            {
                Log.ErrorFormat($"[Navali] Fail to add \"{task.GetType()}\" {type} \"{name}\".", name);
                BotManager.Stop();
            }
        }

        private void RemoveTask(string name)
        {
            if (!_taskManager.Remove(name))
            {
                Log.ErrorFormat("[Navali] Fail to remove \"{0}\".", name);
                BotManager.Stop();
            }
        }
        private enum AddType
        {
            Before,
            After,
            Replace
        }
        #endregion
    }
}
