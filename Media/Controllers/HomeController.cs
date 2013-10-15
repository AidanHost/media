using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Media.Models;
using Newtonsoft.Json;
using SharpPotato;

namespace Media.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Series()
        {
            return View(new DataModel());
        }

        public ActionResult Movies(int page = 1, Boolean minimal = false)
        {
            //"http://105.236.193.199:5050/api/6a7d5e8b999f4be2af6aab906ff9cd15/movie.list/?"

            string callparams = "/?status=done&limit_offset=20," + ((page - 1) * 20);
            List<MovieData> movies = new List<MovieData>();

            string IP = "192.168.1.5";
            string Port = "5050";

            string result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(CouchPotatoAPI.ApiCallFormat, IP, Port, "6a7d5e8b999f4be2af6aab906ff9cd15", "movie.list") + callparams);
            request.Method = "GET";
            request.ContentType = "text/json";
            request.Timeout = 300000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }

            dynamic d = JsonConvert.DeserializeObject(result);

            int pages = (int)Math.Round((double)d.total/20, 0, MidpointRounding.AwayFromZero);

            foreach (dynamic m in d.movies)
            {
                Movie movie = Movie.Parse(m, MovieType.Normal);

                if (!minimal)
                {
                    movies.Add(new MovieData(movie.IMDbId));
                }
                else
                {
                    movies.Add(new MovieData { Name = movie.Titles.First(), Url = String.Format("http://www.imdb.com/title/{0}", movie.IMDbId) });
                }
            }

            ViewBag.Page = page;
            ViewBag.Pages = pages;
            ViewBag.Minimal = minimal;

            return View(movies);
        }
    }
}