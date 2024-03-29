﻿/* *********************************************************************
 * Date: 20 Feb 2008
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using System;
using System.Collections.Generic;

namespace Forge.Configuration.Shared
{

    /// <summary>
    /// Configuration Access Helper
    /// </summary>
    public static class ConfigurationAccessHelper
    {

        #region Public static method(s)

        /// <summary>
        /// Get configuration value by path like X-Path.
        /// </summary>
        /// <param name="propertyItems">Root categoryproperty where the search begins</param>
        /// <param name="configPath">The expression, example: Animals\Dogs\Charles</param>
        /// <returns>
        /// The value or NULL if item does not exist
        /// </returns>
        /// <example>
        ///   <code>
        /// string autoLoadValue = ConfigurationAccessHelper.GetValueByPath(RemotingConfiguration.Settings.CategoryPropertyItems, "Settings/AutomaticallyLoadChannels");
        /// if (string.IsNullOrEmpty(autoLoadValue))
        /// {
        /// ThrowHelper.ThrowArgumentNullException("autoLoadValue");
        /// }
        ///   </code>
        ///   </example>
        /// <exception cref="System.ArgumentNullException">
        /// propertyItems
        /// or
        /// configPath
        /// </exception>
        public static string GetValueByPath(CategoryPropertyItems propertyItems, string configPath)
        {
            if (propertyItems == null)
            {
                throw new ArgumentNullException("propertyItems");
            }
            if (string.IsNullOrEmpty(configPath))
            {
                throw new ArgumentNullException("configPath");
            }

            List<string> keys = new List<string>(configPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            string result = FindValueByKey(propertyItems, keys);
            keys.Clear();
            return result;
        }

        /// <summary>
        /// Get configuration value by path like X-Path.
        /// </summary>
        /// <param name="propertyItem">Root categoryproperty where the search begins</param>
        /// <param name="configPath">The expression, example: Animals\Dogs\Charles</param>
        /// <returns>
        /// The value or NULL if item does not exist
        /// </returns>
        /// <example>
        ///   <code>
        /// string autoLoadValue = ConfigurationAccessHelper.GetValueByPath(RemotingConfiguration.Settings.CategoryPropertyItems, "Settings/AutomaticallyLoadChannels");
        /// if (string.IsNullOrEmpty(autoLoadValue))
        /// {
        /// ThrowHelper.ThrowArgumentNullException("autoLoadValue");
        /// }
        ///   </code>
        ///   </example>
        /// <exception cref="System.ArgumentNullException">
        /// propertyItems
        /// or
        /// configPath
        /// </exception>
        public static string GetValueByPath(IPropertyItem propertyItem, string configPath)
        {
            if (propertyItem == null)
            {
                throw new ArgumentNullException("propertyItem");
            }
            if (string.IsNullOrEmpty(configPath))
            {
                throw new ArgumentNullException("configPath");
            }

            List<string> keys = new List<string>(configPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            string result = FindValueByKey(propertyItem, keys);
            keys.Clear();
            return result;
        }

        /// <summary>
        /// Get configuration categoryproperty item by path like X-Path.
        /// </summary>
        /// <param name="propertyItems">Root categoryproperty where the search begins</param>
        /// <param name="configPath">The expression, example: Animals\Dogs\Charles</param>
        /// <returns>
        /// The value or NULL if item does not exist
        /// </returns>
        /// <example>
        ///   <code>
        /// CategoryPropertyItem configItem = ConfigurationAccessHelper.GetCategoryPropertyByPath(StorageConfiguration.Settings.CategoryPropertyItems, "NHibernateProvider/NHibernateStorages/Default");
        /// if (configItem != null)
        /// {
        /// DEFAULT_SESSION_FACTORY = CreateEntityManagerFactory(configItem);
        /// }
        ///   </code>
        ///   </example>
        /// <exception cref="System.ArgumentNullException">
        /// propertyItems
        /// or
        /// configPath
        /// </exception>
        public static CategoryPropertyItem GetCategoryPropertyByPath(CategoryPropertyItems propertyItems, string configPath)
        {
            if (propertyItems == null)
            {
                throw new ArgumentNullException("propertyItems");
            }
            if (string.IsNullOrEmpty(configPath))
            {
                throw new ArgumentNullException("configPath");
            }

            List<string> keys = new List<string>(configPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            CategoryPropertyItem result = FindCategoryPropertyByKey(propertyItems, keys);
            keys.Clear();
            return result;
        }

        /// <summary>
        /// Get configuration categoryproperty item by path like X-Path.
        /// </summary>
        /// <param name="propertyItem">Root categoryproperty where the search begins</param>
        /// <param name="configPath">The expression, example: Animals\Dogs\Charles</param>
        /// <returns>
        /// The value or NULL if item does not exist
        /// </returns>
        /// <example>
        ///   <code>
        /// CategoryPropertyItem configItem = ConfigurationAccessHelper.GetCategoryPropertyByPath(StorageConfiguration.Settings.CategoryPropertyItems, "NHibernateProvider/NHibernateStorages/Default");
        /// if (configItem != null)
        /// {
        /// DEFAULT_SESSION_FACTORY = CreateEntityManagerFactory(configItem);
        /// }
        ///   </code>
        ///   </example>
        /// <exception cref="System.ArgumentNullException">
        /// propertyItems
        /// or
        /// configPath
        /// </exception>
        public static IPropertyItem GetPropertyByPath(IPropertyItem propertyItem, string configPath)
        {
            if (propertyItem == null)
            {
                throw new ArgumentNullException("propertyItem");
            }
            if (string.IsNullOrEmpty(configPath))
            {
                throw new ArgumentNullException("configPath");
            }

            List<string> keys = new List<string>(configPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            IPropertyItem result = FindPropertyByKey(propertyItem, keys);
            keys.Clear();
            return result;
        }

        /// <summary>
        /// Parses the short value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseShortValue(CategoryPropertyItems root, string entryId, short minValue, short maxValue, ref short value)
        {
            bool result = false;
            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = short.TryParse(pi.EntryValue, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the short value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseShortValue(IPropertyItem root, string entryId, short minValue, short maxValue, ref short value)
        {
            bool result = false;
            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = short.TryParse(pi.Value, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the int value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// this.mMaxSendMessageSize = 32768;
        /// int value = 32768;
        /// if (ConfigurationAccessHelper.ParseIntValue(pi.PropertyItems, "MaxSendMessageSize", 1, Int32.MaxValue, ref value))
        /// {
        /// this.mMaxSendMessageSize = value;
        /// }
        ///   </code>
        ///   </example>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseIntValue(CategoryPropertyItems root, string entryId, int minValue, int maxValue, ref int value)
        {
            bool result = false;
            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = int.TryParse(pi.EntryValue, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the int value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// this.mMaxSendMessageSize = 32768;
        /// int value = 32768;
        /// if (ConfigurationAccessHelper.ParseIntValue(pi.PropertyItems, "MaxSendMessageSize", 1, Int32.MaxValue, ref value))
        /// {
        /// this.mMaxSendMessageSize = value;
        /// }
        ///   </code>
        ///   </example>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseIntValue(IPropertyItem root, string entryId, int minValue, int maxValue, ref int value)
        {
            bool result = false;
            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = int.TryParse(pi.Value, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the long value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// ConfigurationAccessHelper.ParseLongValue(pi.PropertyItems, "DefaultErrorResponseTimeout", Timeout.Infinite, long.MaxValue, ref mDefaultErrorResponseTimeout);
        ///   </code>
        ///   </example>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseLongValue(CategoryPropertyItems root, string entryId, long minValue, long maxValue, ref long value)
        {
            bool result = false;
            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = long.TryParse(pi.EntryValue, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the long value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// ConfigurationAccessHelper.ParseLongValue(pi.PropertyItems, "DefaultErrorResponseTimeout", Timeout.Infinite, long.MaxValue, ref mDefaultErrorResponseTimeout);
        ///   </code>
        ///   </example>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseLongValue(IPropertyItem root, string entryId, long minValue, long maxValue, ref long value)
        {
            bool result = false;
            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = long.TryParse(pi.Value, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the double value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseDoubleValue(CategoryPropertyItems root, string entryId, double minValue, double maxValue, ref double value)
        {
            bool result = false;
            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = double.TryParse(pi.EntryValue, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the double value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseDoubleValue(IPropertyItem root, string entryId, double minValue, double maxValue, ref double value)
        {
            bool result = false;
            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = double.TryParse(pi.Value, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the decimal value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseDecimalValue(CategoryPropertyItems root, string entryId, decimal minValue, decimal maxValue, ref decimal value)
        {
            bool result = false;
            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = decimal.TryParse(pi.EntryValue, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the decimal value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseDecimalValue(IPropertyItem root, string entryId, decimal minValue, decimal maxValue, ref decimal value)
        {
            bool result = false;
            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = decimal.TryParse(pi.Value, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the float value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseFloatValue(CategoryPropertyItems root, string entryId, float minValue, float maxValue, ref float value)
        {
            bool result = false;
            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = float.TryParse(pi.EntryValue, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the float value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <exception cref="InvalidConfigurationValueException">
        /// </exception>
        public static bool ParseFloatValue(IPropertyItem root, string entryId, float minValue, float maxValue, ref float value)
        {
            bool result = false;
            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = float.TryParse(pi.Value, out value);
                if (result)
                {
                    if (value < minValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Minimum value ({0}) is out of range ({1}) for item: {2}", minValue, result, entryId));
                    }
                    if (value > maxValue)
                    {
                        throw new InvalidConfigurationValueException(string.Format("Maximum value ({0}) is out of range ({1}) for item: {2}", maxValue, result, entryId));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the boolean value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// mSessionReusable = true;
        /// ConfigurationAccessHelper.ParseBooleanValue(pi.PropertyItems, "SessionReusable", ref mSessionReusable);
        ///   </code>
        ///   </example>
        public static bool ParseBooleanValue(CategoryPropertyItems root, string entryId, ref bool value)
        {
            bool result = false;

            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = bool.TryParse(pi.EntryValue, out value);
            }

            return result;
        }

        /// <summary>
        /// Parses the boolean value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// mSessionReusable = true;
        /// ConfigurationAccessHelper.ParseBooleanValue(pi.PropertyItems, "SessionReusable", ref mSessionReusable);
        ///   </code>
        ///   </example>
        public static bool ParseBooleanValue(IPropertyItem root, string entryId, ref bool value)
        {
            bool result = false;

            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                result = bool.TryParse(pi.Value, out value);
            }

            return result;
        }

        /// <summary>
        /// Parses the string value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// ConfigurationAccessHelper.ParseStringValue(UpdateConfiguration.Settings.CategoryPropertyItems, DOWNLOAD_FOLDER, ref mDownloadFolder);
        ///   </code>
        ///   </example>
        public static bool ParseStringValue(CategoryPropertyItems root, string entryId, ref string value)
        {
            bool result = false;

            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                value = pi.EntryValue;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Parses the string value.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True, if the parse was successful, otherwise False.
        /// </returns>
        /// <example>
        ///   <code>
        /// ConfigurationAccessHelper.ParseStringValue(UpdateConfiguration.Settings.CategoryPropertyItems, DOWNLOAD_FOLDER, ref mDownloadFolder);
        ///   </code>
        ///   </example>
        public static bool ParseStringValue(IPropertyItem root, string entryId, ref string value)
        {
            bool result = false;

            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                value = pi.Value;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Parses the enum value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool ParseEnumValue<TEnum>(CategoryPropertyItems root, string entryId, ref TEnum value) where TEnum : struct
        {
            bool result = false;

            CategoryPropertyItem pi = GetCategoryPropertyByPath(root, entryId);
            if (pi != null)
            {
                try
                {
                    value = (TEnum)Enum.Parse(typeof(TEnum), pi.EntryValue, true);
                    result = true;
                }
                catch (Exception) { }
            }

            return result;
        }

        /// <summary>
        /// Parses the enum value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="root">The root.</param>
        /// <param name="entryId">The entry id.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool ParseEnumValue<TEnum>(IPropertyItem root, string entryId, ref TEnum value) where TEnum : struct
        {
            bool result = false;

            IPropertyItem pi = GetPropertyByPath(root, entryId);
            if (pi != null)
            {
                try
                {
                    value = (TEnum)Enum.Parse(typeof(TEnum), pi.Value, true);
                    result = true;
                }
                catch (Exception) { }
            }

            return result;
        }

        #endregion

        #region Private static method(s)

        private static string FindValueByKey(CategoryPropertyItems propertyItems, List<string> keys)
        {
            string result = null;
            CategoryPropertyItem item = propertyItems[keys[0]];
            if (item != null)
            {
                if (item.Id.Equals(keys[0]))
                {
                    if (keys.Count == 1)
                    {
                        result = item.EntryValue;
                    }
                    else
                    {
                        keys.RemoveAt(0);
                        result = FindValueByKey(item.PropertyItems, keys);
                    }
                }
            }
            return result;
        }

        private static string FindValueByKey(IPropertyItem propertyItem, List<string> keys)
        {
            string result = null;
            IPropertyItem item = null;
            if (propertyItem.Items.ContainsKey(keys[0])) item = propertyItem.Items[keys[0]];
            if (item != null)
            {
                if (keys.Count == 1)
                {
                    result = item.Value;
                }
                else
                {
                    keys.RemoveAt(0);
                    result = FindValueByKey(item, keys);
                }
            }
            return result;
        }

        private static CategoryPropertyItem FindCategoryPropertyByKey(CategoryPropertyItems propertyItems, List<string> keys)
        {
            CategoryPropertyItem result = null;
            CategoryPropertyItem item = propertyItems[keys[0]];
            if (item != null)
            {
                if (item.Id.Equals(keys[0]))
                {
                    if (keys.Count == 1)
                    {
                        result = item;
                    }
                    else
                    {
                        keys.RemoveAt(0);
                        result = FindCategoryPropertyByKey(item.PropertyItems, keys);
                    }
                }
            }
            return result;
        }

        private static IPropertyItem FindPropertyByKey(IPropertyItem propertyItem, List<string> keys)
        {
            IPropertyItem result = null;
            IPropertyItem item = null;
            if (propertyItem.Items.ContainsKey(keys[0])) item = propertyItem.Items[keys[0]];
            if (item != null)
            {
                if (keys.Count == 1)
                {
                    result = item;
                }
                else
                {
                    keys.RemoveAt(0);
                    result = FindPropertyByKey(item, keys);
                }
            }
            return result;
        }

        #endregion

    }

}
