using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Media.Models
{
    public class DataModel
    {
        public List<SeriesData> Series { get; set; }
        public List<MovieData> Movies { get; set; }
        public static String[] FileExtensions = ConfigurationManager.AppSettings["fileExtensions"].Split(',');

        public DataModel()
        {
            Series = new List<SeriesData>();
            Movies = new List<MovieData>();

            // Get list of Series folders
            var folders = Directory.GetDirectories(ConfigurationManager.AppSettings["SeriesPath"]);

            foreach (var folder in folders)
            {
                Series.Add(new SeriesData(Regex.Match(folder, @"\[([^]]*)\]").Groups[1].Value, folder));
            }

            /*folders = Directory.GetDirectories(ConfigurationManager.AppSettings["AnimeSeriesPath"]);

            foreach (var folder in folders)
            {
                Series.Add(new SeriesData(folder.Substring(folder.LastIndexOf("\\") + 1)));
            }*/

            //Series = Series.OrderBy(x => x.Name).ToList();

            //Series = new List<SeriesData> { new SeriesData("Archer"), new SeriesData("Game of Thrones") };
            //movies = new List<string> { "The Man who fell to Earth", "The Man from Earth" };
        }
    }
}