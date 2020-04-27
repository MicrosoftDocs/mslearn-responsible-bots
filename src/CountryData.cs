using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples
{
    public class Country
    {
        public Country(string s)
        {
            var t = s.Split(',');
            Capital = t[0].Trim('"');
            Name = t[4].Trim('"');
            // Population = int.Parse(t[9].Trim('"'));
        }

        public string Name { get; set; }
        public string Capital { get; set; }
        public int Population { get; set; }
    }

    public class CountryData
    {
        protected Country[] Countries { get; set; }

        public CountryData(string fn)
        {
            var data = File.ReadAllLines(fn);
            Countries = (from z in data.Skip(1)
                         select new Country(z)).ToArray();
        }

        public string GetCapital(string country)
        {
            return Countries.FirstOrDefault(
                c => c.Name.ToLower() == country.ToLower())?.Capital;
        }

    }
}
