

using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace HelsinkiPopulation.Utilities
{
    public static class JSonParser
    {
        /// <summary>
        /// Parses the population data. 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParsePopulationData(JObject json)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            for (int i = 1; i < json["dataset"]["dimension"]["ika"]["category"]["index"].Count(); i++)
            {
                var key = json["dataset"]["dimension"]["ika"]["category"]["index"][i].Value<string>();
                var keyValue = json["dataset"]["dimension"]["ika"]["category"]["label"][key].Value<string>();
                var value = json["dataset"]["value"][i].Value<string>();
                results.Add(keyValue, value);
            }
            return results;
        }

        /// <summary>
        /// Parses the year data.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<int, int> ParseYearData(JObject json)
        {
            Dictionary<int, int> results = new Dictionary<int, int>();
            for (int i = 1; i < json["dataset"]["dimension"]["vuosi"]["category"]["index"].Count(); i++)
            {
                var val = json["dataset"]["dimension"]["vuosi"]["category"]["index"][i].Value<int>();

                results.Add(i, val);
            }
            return results;
        } 
    }
}
