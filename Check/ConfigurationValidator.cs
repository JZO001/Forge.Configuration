/* *********************************************************************
 * Date: 20 Feb 2008
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using Forge.Logging.Abstraction;
using System;
using System.Configuration;
using System.Diagnostics;

namespace Forge.Configuration.Check
{

    /// <summary>
    /// Configuration validator which validates the application xml configuration.
    /// </summary>
    public static class ConfigurationValidator
    {

        #region Field(s)

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ConfigurationValidator).FullName);

#if IS_WINDOWS
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string mLog = "Application";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string mEventLogSource = string.Empty;
#endif

        #endregion

#region Public properties

#if IS_WINDOWS

        /// <summary>
        /// Get or set the Log where the EventLogEntry will be written
        /// Default is the "Application"
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        [DebuggerHidden]
        public static string Log
        {
            get { return mLog; }
            set { mLog = value; }
        }

        /// <summary>
        /// Get or set the EventLog Source name in the Application
        /// Do not specify existing Source name
        /// </summary>
        /// <value>
        /// The event log source.
        /// </value>
        [DebuggerHidden]
        public static string EventLogSource
        {
            get { return mEventLogSource; }
            set { mEventLogSource = value; }
        }

#endif

#endregion

#region Public methods

#if IS_WINDOWS

        /// <summary>
        /// Create the EventLog log if it does not exist
        /// </summary>
        [DebuggerStepThrough]
        public static void CreateEventLog()
        {
            try
            {
                bool found = false;
                foreach (EventLog el in EventLog.GetEventLogs())
                {
                    if (el.Log.Equals(mLog))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    EventLog.CreateEventSource(mEventLogSource, mLog);
                }
            }
            catch (Exception e)
            {
                if (LOGGER.IsErrorEnabled) LOGGER.Error(string.Format("[CHECK] {0}", e.ToString()));
            }
        }

#endif

        /// <summary>
        /// Validates the configuration.
        /// </summary>
        /// <param name="configFile">The configuration file.</param>
        /// <returns>True, if the configuration file content is valid, otherwise False.</returns>
        public static bool ValidateConfiguration(string configFile)
        {
            return ValidateConfiguration(configFile, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// Validates the configuration
        /// </summary>
        /// <param name="configFile">The config file.</param>
        /// <param name="userLevel">The user level.</param>
        /// <returns>
        /// True, if the configuration file content is valid, otherwise False.
        /// </returns>
        [DebuggerStepThrough]
        public static bool ValidateConfiguration(string configFile, ConfigurationUserLevel userLevel)
        {
            bool success = true;

            try
            {
                ExeConfigurationFileMap fMap = new ExeConfigurationFileMap();
                fMap.ExeConfigFilename = configFile;
                ConfigurationManager.OpenMappedExeConfiguration(fMap, userLevel);
            }
            catch (Exception ex)
            {
                success = false;
#if IS_WINDOWS
                WriteEventLog(ex.ToString());
#endif
                if (LOGGER.IsErrorEnabled) LOGGER.Error(string.Format("[CHECK] {0}", ex.ToString()));
            }

            return success;
        }

#if IS_WINDOWS

        /// <summary>
        /// Write an eventlog entry to the specified Log
        /// </summary>
        /// <param name="message">Message to write</param>
        [DebuggerStepThrough]
        public static void WriteEventLog(string message)
        {
            try
            {
                foreach (EventLog el in EventLog.GetEventLogs())
                {
                    if (el.Log.Equals(mLog))
                    {
                        el.Source = mEventLogSource;
                        el.WriteEntry(message, EventLogEntryType.Error);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                if (LOGGER.IsErrorEnabled) LOGGER.Error(string.Format("[CHECK] {0}", e.ToString()));
            }
        }

#endif

#endregion

    }

}
