using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace Media.Models
{
    public class SeriesData
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Poster { get; set; }
        public string Rating { get; set; }
        public string Year { get; set; }
        public List<Season> Seasons { get; set; }

        public SeriesData(string imdbId, string folder)
        {
            try
            {
                Name = "";
                Url = "";
                Poster = "";
                Rating = "0";
                Year = "0";

                Seasons = GetSeasons(folder);

                ApiCall(imdbId, folder);
            }
            catch (Exception ex)
            {
                
            }
        }

        private List<Season> GetSeasons(string folder)
        {
            var seasons = new List<Season>();

            var seasonFolders = Directory.GetDirectories(folder);

            foreach (var seasonFolder in seasonFolders)
            {
                if (seasonFolder.Contains("Season 0") || seasonFolder.Contains("Extras"))
                {
                    continue;
                }
                var seasonFolderName = seasonFolder;
                var files = Directory.GetFiles(seasonFolder);
                int episodeCount = 0;

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (DataModel.FileExtensions.Contains(fileInfo.Extension))
                    {
                        episodeCount++;
                    }
                }
                int startIndex = seasonFolderName.LastIndexOf("\\") + 1;
                seasons.Add(new Season(seasonFolderName.Substring(startIndex, seasonFolderName.Length - startIndex), episodeCount));
            }

            return seasons;
        }

        private void ApiCall(string imdbId, string folder)
        {
            var request = WebRequest.Create(String.Format("http://www.omdbapi.com/?i={0}", imdbId));
            request.ContentType = "application/json; charset=utf-8";

            //get response-stream, and use a streamReader to read the content
            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var jsonData = sr.ReadToEnd();

                    var dictionary = Json.Decode(jsonData);

                    if (dictionary != null)
                    {
                        Name = (string)dictionary["Title"];
                        Url = String.Format("http://www.imdb.com/title/{0}", imdbId);
                        Rating = (string)dictionary["imdbRating"];
                        Year = (string)dictionary["Year"];

                        // save poster
                        var posterUrl = (string) dictionary["Poster"];

                        if (posterUrl != "N/A")
                        {
                            var posterFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images\\Posters\\Series\\" + imdbId + ".jpg");

                            if (!File.Exists(posterFile))
                                new WebClient().DownloadFile(posterUrl, posterFile);

                            Poster = "\\Images\\Posters\\Series\\" + imdbId + ".jpg";
                        }
                        else
                        {
                            Poster = "";
                        }

                        
                    }
                }
            }
        }
    }
}