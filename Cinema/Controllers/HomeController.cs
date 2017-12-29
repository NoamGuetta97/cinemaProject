﻿using Cinema.Context;
using Cinema.CustomAttributes;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        CinemaDbContext db = new CinemaDbContext();

        public ActionResult Index()
        {

            ViewBag.CinemasList = new SelectList(db.Cinemas,"Id", "FullName");
            ViewBag.MoviesList = new SelectList(db.Movies, "Id", "Title");
            ViewBag.TypesList = new SelectList(db.MovieTypes, "Id", "Name");

            HomeViewModel vm = new HomeViewModel()
            {
                MovieNowBooking = db.Movies.Where(a => a.Status == Status.NowBooking),
                MovieSoon = db.Movies.Where(a => a.Status == Status.Soon),
                Genres = db.Genres
            };
            
            return View(vm);
        }
        [AjaxChildActionOnly]
        public JsonResult ShowGenre(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var moviesWithGenreId = db.Movies.Where(a => a.GenreId == 1).ToList();
            if(id != 1)
            {
                moviesWithGenreId = db.Movies.Where(a => a.GenreId == id && a.Status == Status.NowBooking).ToList();
            }
            else
            {
                moviesWithGenreId = db.Movies.Where(a => a.Status == Status.NowBooking).ToList();
            }

            return Json(moviesWithGenreId, JsonRequestBehavior.AllowGet);
        }
        [AjaxChildActionOnly]
        public JsonResult GetCinemas()
        {
            var cinemas = db.Cinemas.ToList();
            var positionslist = new SelectList(cinemas, "Id", "FullName");
            return Json(positionslist, JsonRequestBehavior.AllowGet);
        }
        [AjaxChildActionOnly]
        public JsonResult GetMovieById(int cinemaid)
        {
            var position = db.MoviePositions.Include("Movie").Where(a => a.CinemaId == cinemaid && a.Movie.Status == Status.NowBooking).Select(p => p.Movie);
            db.Configuration.ProxyCreationEnabled = false;
            var positionslist = new SelectList(position, "Id", "Title");
            return Json(positionslist,JsonRequestBehavior.AllowGet);
        }
        [AjaxChildActionOnly]
        public JsonResult GetMoviesByIdToTable(int cinemaid, string currentdate)
        {
            var moviepositions = db.MoviePositions.Where(a => a.CinemaId == cinemaid && a.Movie.Status == Status.NowBooking).ToList();
            var positions = db.MoviePositionsDates.Include("MoviePosition").Where(a => a.MoviePosition.CinemaId == cinemaid).ToList();
            db.Configuration.ProxyCreationEnabled = false;


            var positionslist = new List<MoviePosition>();
            foreach (var position in moviepositions)
            {
                MoviePosition movieposition = new MoviePosition { Id = position.Id, DateTimeWithMovieType = new List<DateTimeAndMovieTypePair>(), MovieTitle = position.Movie.Title, MovieDuration = position.Movie.Duration };

                foreach (var pos in positions)
                {
                   if (pos.DateTime.ToString().Substring(0, 10).Replace(".", "-") == currentdate && movieposition.MovieTitle == pos.MoviePosition.Movie.Title)
                    {
                        DateTimeAndMovieTypePair datetimewithmovietype = new DateTimeAndMovieTypePair()
                        {
                            DateTime = pos.DateTime,
                            MovieType = pos.MovieType.Name
                        };
                        movieposition.DateTimeWithMovieType.Add(datetimewithmovietype);
                    }

                }
                if(movieposition.DateTimeWithMovieType.Count !=0)
                {

                positionslist.Add(movieposition);
                }
               
            }
            return Json(positionslist, JsonRequestBehavior.AllowGet);
        }
        [AjaxChildActionOnly]
        public JsonResult GetMovieByIdToTable(int movieid, int cinemaid, string currentdate)
        {
            var positions = db.MoviePositionsDates.Include("MoviePosition").Where(a => a.MoviePosition.CinemaId == cinemaid && a.MoviePosition.MovieId == movieid).ToList();
            var position = db.MoviePositions.Include("Movie").Where(a => a.MovieId == movieid && a.CinemaId == cinemaid && a.Movie.Status == Status.NowBooking).FirstOrDefault();
            db.Configuration.ProxyCreationEnabled = false;
            var positionslist = new List<MoviePosition>();
                MoviePosition movieposition = new MoviePosition { Id = position.Id, DateTimeWithMovieType = new List<DateTimeAndMovieTypePair>(), MovieTitle = position.Movie.Title, MovieDuration = position.Movie.Duration };

                foreach (var pos in positions)
                {
                    if (pos.DateTime.ToString().Substring(0, 10).Replace(".", "-") == currentdate && movieposition.MovieTitle == pos.MoviePosition.Movie.Title)
                    {
                        DateTimeAndMovieTypePair datetimewithmovietype = new DateTimeAndMovieTypePair()
                        {
                            DateTime = pos.DateTime,
                            MovieType = pos.MovieType.Name
                        };
                        movieposition.DateTimeWithMovieType.Add(datetimewithmovietype);
                    }

                }
            return Json(movieposition, JsonRequestBehavior.AllowGet);
        }
        [AjaxChildActionOnly]
        public JsonResult GetMovieTypeById(int movieid,int cinemaid)
        {
            var position = db.MoviePositionsDates.Include("MovieType").Include("MoviePosition").Where(a => a.MoviePosition.MovieId == movieid && a.MoviePosition.CinemaId == cinemaid && a.MoviePosition.Movie.Status == Status.NowBooking).Select(p => p.MovieType).Distinct();
            db.Configuration.ProxyCreationEnabled = false;
            var positionslist = new SelectList(position, "Id", "Name");
            return Json(positionslist, JsonRequestBehavior.AllowGet);
        }
        [AjaxChildActionOnly]
        public JsonResult GetMovieTypeByIdToTable(int movieid, int cinemaid, int movietypeid,string currentdate)
        {
            var positions = db.MoviePositionsDates.Include("MoviePosition").Where(a => a.MoviePosition.CinemaId == cinemaid && a.MoviePosition.MovieId == movieid).ToList();
            var position = db.MoviePositions.Include("Movie").Where(a => a.MovieId == movieid && a.CinemaId == cinemaid && a.Movie.Status == Status.NowBooking).FirstOrDefault();
            db.Configuration.ProxyCreationEnabled = false;
            var positionslist = new List<MoviePosition>();
            MoviePosition movieposition = new MoviePosition { Id = position.Id, DateTimeWithMovieType = new List<DateTimeAndMovieTypePair>(), MovieTitle = position.Movie.Title, MovieDuration = position.Movie.Duration };

            foreach (var pos in positions)
            {
                if (pos.DateTime.ToString().Substring(0, 10).Replace(".", "-") == currentdate && movieposition.MovieTitle == pos.MoviePosition.Movie.Title && pos.MovieTypeId == movietypeid)
                {
                    DateTimeAndMovieTypePair datetimewithmovietype = new DateTimeAndMovieTypePair()
                    {
                        DateTime = pos.DateTime,
                        MovieType = pos.MovieType.Name
                    };
                    movieposition.DateTimeWithMovieType.Add(datetimewithmovietype);
                }

            }
            return Json(movieposition, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}