using AspNetCoreOAuth2Sample.Model;
using System.Collections.Generic;

namespace AspNetCoreOAuth2Sample.ViewModel
{
    public class RulesViewModel
    {
        public IEnumerable<Rule> Rules { get; set; }
        public IEnumerable<string> Clients { get; set; }
    }
}
