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
        public decimal Rating { get; set; }

        public MovieData(string name)
        {
            Name = name;

            Url = "";
            Poster = "";
            Rating = 0;

            // Call to IMDB api

            var request = WebRequest.Create("http://imdbapi.org/?title=" + name);
            request.ContentType = "application/json; charset=utf-8";

            //get response-stream, and use a streamReader to read the content
            using (var s = request.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var jsonData = sr.ReadToEnd();

                    var dictionary = Json.Decode(jsonData);

                    if (dictionary[0] != null)
                    {
                        Url = (string) dictionary[0]["imdb_url"];
                        Poster = (string) dictionary[0]["poster"];
                        Rating = (decimal) dictionary[0]["rating"];
                    }
                }
            }
        }
    }
}