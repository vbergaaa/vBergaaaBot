using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Helpers
{
    public static class DictionaryHelper
    {
        public static void Increment<TKey>(Dictionary<TKey,int> dict,TKey unitType)
        {
            if (!dict.ContainsKey(unitType))
                dict.Add(unitType, 1);
            else
                dict[unitType]++;
        }
        public static void Increment(Dictionary<uint,int> dict, uint unitType)
        {
            Increment<uint>(dict, unitType);
        }
    }
}
