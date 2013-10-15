using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace Media.Models
{
    public class MovieData
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Poster { get; set; }
        public string Rating { get; set; }
        public string Year { get; set; }

        public MovieData()
        {
            
        }

        public MovieData(string imdbId)
        {
            Name = "";
            Url = "";
            Poster = "";
            Rating = "0";
            Year = "0";

            ApiCall(imdbId);
        }

        public void ApiCall(string imdbId)
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
                        var posterUrl = (string)dictionary["Poster"];

                        if (posterUrl != "N/A")
                        {
                            var posterFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images\\Posters\\Movies\\" + imdbId + ".jpg");

                            if (!File.Exists(posterFile))
                                new WebClient().DownloadFile(posterUrl, posterFile);

                            Poster = "/Images/Posters/Movies/" + imdbId + ".jpg";
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