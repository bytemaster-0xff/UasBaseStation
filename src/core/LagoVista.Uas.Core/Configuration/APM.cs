/* 
 * Based on code and configuration from MissionPlanner
 * https://github.com/ArduPilot/MissionPlanner
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace LagoVista.Uas.Core.Configuration
{
    public class APM
    {
        private static XDocument _parameterMetaDataXML;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterMetaDataRepository"/> class.
        /// </summary>
        public static void CheckLoad()
        {
            if (_parameterMetaDataXML == null)
                Reload();
        }

        public static void Reload()
        {
            //TODO: Need to get from XML (or other source)/
        }

        /// <summary>
        /// Gets the parameter meta data.
        /// </summary>
        /// <param name="nodeKey">The node key.</param>
        /// <param name="metaKey">The meta key.</param>
        /// <returns></returns>
        public static string GetParameterMetaData(string nodeKey, string metaKey, string vechileType)
        {
            CheckLoad();

            if (_parameterMetaDataXML != null)
            {
                // Use this to find the endpoint node we are looking for
                // Either it will be pulled from a file in the ArduPlane hierarchy or the ArduCopter hierarchy
                try
                {
                    var elements = _parameterMetaDataXML.Element("Params").Elements(vechileType);

                    foreach (var element in elements)
                    {
                        if (element != null && element.HasElements)
                        {
                            var node = element.Element(nodeKey);
                            if (node != null && node.HasElements)
                            {
                                var metaValue = node.Element(metaKey);
                                if (metaValue != null)
                                {
                                    return metaValue.Value;
                                }
                            }
                        }
                    }
                }
                catch
                {
                } // Exception System.ArgumentException: '' is an invalid expanded name.
            }

            return string.Empty;
        }
    }
}
