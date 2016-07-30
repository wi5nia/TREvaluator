using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TRMobEval
{
    public class SessionEval
    {
        public string Id { get; set; }
        public string SessionName { get; set; }
        public float SessionScore { get; set; }
        public string ScoreDateString { get; set; }

    }
}
