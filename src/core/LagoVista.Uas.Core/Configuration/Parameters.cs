/* 
 * Based on code and configuration from MissionPlanner
 * https://github.com/ArduPilot/MissionPlanner
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LagoVista.Uas.Core.Configuration
{
    public sealed class ParameterMetaDataConstants
    {
        public const string ParamDelimeter = "@";
        public const string PathDelimeter = ",";
        public const string Param = "Param";
        public const string Group = "Group";
        public const string Path = "Path";

        public const string NestedGroup = @"AP_NESTEDGROUPINFO\((.+),.+\)";
        
        public const string DisplayName = "DisplayName";
        public const string Description = "Description";
        public const string Units = "Units";
        public const string Range = "Range";
        public const string Values = "Values";
        public const string Increment = "Increment";
        public const string User = "User";
        public const string RebootRequired = "RebootRequired";
        public const string Bitmask = "Bitmask";
        public const string ReadOnly = "ReadOnly";
        
        public const string Advanced = "Advanced";
        public const string Standard = "Standard";
    }

    public static class Parameters
    {
        /// <summary>
        /// Gets the parameter meta data.
        /// </summary>
        /// <param name="nodeKey">The node key.</param>
        /// <param name="metaKey">The meta key.</param>
        /// <returns></returns>
        public static string GetParameterMetaData(string nodeKey, string metaKey, string vechileType)
        {
            if (vechileType == "PX4")
            {
                return PX4.GetParameterMetaData(nodeKey, metaKey, vechileType);
            }
            else
            {
                //return ParameterMetaDataRepositoryAPMpdef.GetParameterMetaData(nodeKey, metaKey, vechileType);
                return APM.GetParameterMetaData(nodeKey, metaKey, vechileType);
            }
        }

        /// <summary>
        /// Return a key, value list off all options selectable
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public static List<KeyValuePair<int, string>> GetParameterOptionsInt(string nodeKey, string vechileType)
        {
            string availableValuesRaw = GetParameterMetaData(nodeKey, ParameterMetaDataConstants.Values, vechileType);
            string[] availableValues = availableValuesRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (availableValues.Any())
            {
                var splitValues = new List<KeyValuePair<int, string>>();
                // Add the values to the ddl
                foreach (string val in availableValues)
                {
                    try
                    {
                        string[] valParts = val.Split(new[] { ':' });
                        splitValues.Add(new KeyValuePair<int, string>(int.Parse(valParts[0].Trim()),
                            (valParts.Length > 1) ? valParts[1].Trim() : valParts[0].Trim()));
                    }
                    catch
                    {
                        Console.WriteLine("Bad entry in param meta data: " + nodeKey);
                    }
                }
                ;

                return splitValues;
            }

            return new List<KeyValuePair<int, string>>();
        }

        public static List<KeyValuePair<int, string>> GetParameterBitMaskInt(string nodeKey, string vechileType)
        {
            string availableValuesRaw;

            availableValuesRaw = GetParameterMetaData(nodeKey, ParameterMetaDataConstants.Bitmask, vechileType);

            string[] availableValues = availableValuesRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (availableValues.Any())
            {
                var splitValues = new List<KeyValuePair<int, string>>();
                // Add the values to the ddl
                foreach (string val in availableValues)
                {
                    try
                    {
                        string[] valParts = val.Split(new[] { ':' });
                        splitValues.Add(new KeyValuePair<int, string>(int.Parse(valParts[0].Trim()),
                            (valParts.Length > 1) ? valParts[1].Trim() : valParts[0].Trim()));
                    }
                    catch
                    {
                        Console.WriteLine("Bad entry in param meta data: " + nodeKey);
                    }
                }
                ;

                return splitValues;
            }

            return new List<KeyValuePair<int, string>>();
        }

        public static bool GetParameterRange(string nodeKey, ref double min, ref double max, string vechileType)
        {
            string rangeRaw = GetParameterMetaData(nodeKey, ParameterMetaDataConstants.Range, vechileType);

            string[] rangeParts = rangeRaw.Split(new[] { ' ' });
            if (rangeParts.Count() == 2)
            {
                float lowerRange;
                if (float.TryParse(rangeParts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lowerRange))
                {
                    float upperRange;
                    if (float.TryParse(rangeParts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out upperRange))
                    {
                        min = lowerRange;
                        max = upperRange;

                        return true;
                    }
                }
            }

            return false;
        }

        public static bool GetParameterRebootRequired(string nodeKey, string vechileType)
        {
            // set the default answer
            bool answer = false;

            string rebootrequired = GetParameterMetaData(nodeKey,
                ParameterMetaDataConstants.RebootRequired, vechileType);

            if (!string.IsNullOrEmpty(rebootrequired))
            {
                bool.TryParse(rebootrequired, out answer);
            }

            return answer;
        }

        public static bool GetParameterIncrement(string nodeKey, ref double inc, string vechileType)
        {
            string incrementAmt = GetParameterMetaData(nodeKey, ParameterMetaDataConstants.Increment, vechileType);
            if (incrementAmt.Length == 0) return false;
            float Amt = 0;
            float.TryParse(incrementAmt, NumberStyles.Float, CultureInfo.InvariantCulture, out Amt);
            inc = Amt;
            return true;
        }
    }
}
