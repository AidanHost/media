using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Media.Models
{
    public class Season
    {
        public string Name { get; set; }
        public int EpisodeCount { get; set; }

        public Season(string name, int episodeCount)
        {
            Name = name;
            EpisodeCount = episodeCount;
        }
    }
}