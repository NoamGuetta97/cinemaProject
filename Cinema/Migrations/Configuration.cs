﻿namespace Cinema.Migrations
{
    using Cinema.Context;
    using Cinema.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Cinema.Context.CinemaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Cinema.Context.CinemaDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
            SeedGenres(context);
            SeedAgesRestriction(context);
            SeedMovieTypes(context);
            SeedCinemas(context);
            SeedMovies(context);
            SeedPositions(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
        private void SeedRoles(CinemaDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(CinemaDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var user = new ApplicationUser { UserName = "Admin" };
                var adminresult = manager.Create(user, "12345678");

                if (adminresult.Succeeded) manager.AddToRole(user.Id, "Admin");
            }
        }
        private void SeedGenres(CinemaDbContext context)
        {
            var genres = new List<Genre>{
                new Genre { Id = 1, Name = "All" },
                new Genre { Id = 2, Name = "Action" },
                new Genre { Id = 3, Name = "Biographical" },
                new Genre { Id = 4, Name = "Documentary" },
                new Genre { Id = 5, Name = "Drama" },
                new Genre { Id = 6, Name = "Family" },
                new Genre { Id = 7, Name = "Horror" },
                new Genre { Id = 8, Name = "Comedy" },
                new Genre { Id = 9, Name = "Musical" },
                new Genre { Id = 10, Name = "Sci-Fi" },
                new Genre { Id = 11, Name = "Thriller" }
            };
            genres.ForEach(g => context.Genres.AddOrUpdate(p => p.Name, g));
            context.SaveChanges();
        }
        private void SeedAgesRestriction(CinemaDbContext context)
        {
            var ages = new List<AgeRestriction>{
                new AgeRestriction { Id = 1, Name = "Age6+" },
                new AgeRestriction { Id = 2, Name = "Age15+" },
                new AgeRestriction { Id = 3, Name = "Age18+" },
                new AgeRestriction { Id = 4, Name = "AgeNoLimit" },
                new AgeRestriction { Id = 5, Name = "AgeNA" },  // NA -> Not Available
            };
            ages.ForEach(g => context.AgesRestriction.AddOrUpdate(g));
            context.SaveChanges();
        }
        private void SeedMovieTypes(CinemaDbContext context)
        {
            var movietypes = new List<MovieType>{
                new MovieType { Id = 1, Name="2D" },
                new MovieType { Id = 2, Name="3D" },
                new MovieType { Id = 3, Name="Dubbing" },
                new MovieType { Id = 4, Name="Lector EN" },
                new MovieType { Id = 5, Name="Lector PL" },
                new MovieType { Id = 6, Name="Lyrics EN" },
                new MovieType { Id = 7, Name="Lyrics PL" },
                new MovieType { Id = 8, Name="IMAX" },
            };
            movietypes.ForEach(g => context.MovieTypes.AddOrUpdate(p => p.Name, g));
            context.SaveChanges();
        }
        private void SeedCinemas(CinemaDbContext context)
        {
            //var cinemas = new List<CinemaPlace>{
            //    new CinemaPlace { Id = 1, City="Bydgoszcz", Name="FocusMall", Street="Jagiellońska", Number=39, PostCode=85-097  },
            //    new CinemaPlace { Id = 2, City="Bydgoszcz", Name="Galeria Pomorska", Street="Fordońska", Number=141, PostCode=85-739 },
            //    new CinemaPlace { Id = 3, City="Poznań", Name="Posnania", Street="Pleszewska ", Number=1, PostCode=61-136 },
            //};
            //cinemas.ForEach(g => context.Cinemas.AddOrUpdate(g));
            context.Cinemas.AddOrUpdate(new CinemaPlace { Id = 1, City = "Bydgoszcz", Name = "FocusMall", Street = "Jagiello", Number = 39, PostCode = 85 - 094 });
            context.Cinemas.AddOrUpdate(new CinemaPlace { Id = 2, City = "Bydgoszcz", Name = "Galeria Pomorska", Street = "Fordońska", Number = 141, PostCode = 85 - 739 });
            context.Cinemas.AddOrUpdate(new CinemaPlace { Id = 3, City = "Poznań", Name = "Posnania", Street = "Pleszewska ", Number = 1, PostCode = 61 - 136 });
            context.SaveChanges();
        }
        private void SeedMovies(CinemaDbContext context)
        {
            CinemaDbContext db = new CinemaDbContext();

            var movies = new List<Movie>{
                new Movie { Id = 1, Title="Król Artur: Legenda miecza" , OryginalTitle="King Arthur: Legend of the Sword", Description="Młody Artur zdobywa miecz Excalibur i wiedzę na temat swojego królewskiego pochodzenia. Przyłącza się do rebelii, aby pokonać tyrana, który zamordował jego rodziców.", Duration=126, Premiere= new DateTime(2017,6,16), Director="Guy Ritchie", Cast="Charlie Hunnam, Astrid Bergès-Frisbey, Jude Law", Production="CUSA 2017", AgeRestrictionId=3  ,TrailerLinkYoutube = "https://www.youtube.com/watch?v=40wycr0oMYA" }, 
                new Movie { Id = 2, Title="Skyfall", OryginalTitle="Skyfall", Description="Lojalność agenta 007 wobec M zostaje wystawiona na próbę po ataku na siedzibę MI6 i kradzieży tajnych danych. Trop wiedzie do osoby z przeszłości szefowej brytyjskiego wywiadu. ", Duration=143, Premiere= new DateTime(2012,10,26), Director="Sam Mendes", Cast="Daniel Craig, Judi Dench, Javier Bardem", Production="Metro-Goldwyn-Mayer", AgeRestrictionId=2, TrailerLinkYoutube = "https://www.youtube.com/watch?v=6kw1UVovByw" },
                new Movie { Id = 3, Title="X-Men", OryginalTitle="X-Men", Description="Ekranizacja popularnej serii komiksów o obdarzonych nadnaturalnymi zdolnościami mutantach. Schronienie przed prześladowaniami ze strony ludzi znajdują w specjalnej szkole profesora Charlesa Xaviera.", Duration=102, Premiere= new DateTime(2000,10,13), Director="Bryan Singer", Cast="Hugh Jackman, Patrick Stewart, Ian McKellen", Production="Lauren Shuler Donner", AgeRestrictionId=4 , TrailerLinkYoutube = "https://www.youtube.com/watch?v=Iy5R5_T243w"}
            };
            movies.ForEach(g => context.Movies.AddOrUpdate(p => p.Title, g));
            context.SaveChanges();
        }
        private void SeedPositions(CinemaDbContext context)
        {
            CinemaDbContext db = new CinemaDbContext();

            var positions = new List<MoviePosition>{
                new MoviePosition { Id = 1, DateTime= new DateTime(2017,11,1), CinemaId=1, MovieId=2, MovieTypeId=3 },
                //new MoviePosition { Id = 2, Cinema=db.Cinemas.SingleOrDefault(cinema=>cinema.Id == 2), Movie=db.Movies.SingleOrDefault(movie=>movie.Id == 3), DateTime= new DateTime(2017,11,2), MovieType = db.MovieTypes.SingleOrDefault(type=>type.Id == 2) },
                //new MoviePosition { Id = 3, Cinema=db.Cinemas.SingleOrDefault(cinema=>cinema.Id == 2), Movie=db.Movies.SingleOrDefault(movie=>movie.Id == 2), DateTime= new DateTime(2017,11,3), MovieType = db.MovieTypes.SingleOrDefault(type=>type.Id == 1)}
            };
            positions.ForEach(g => context.MoviePositions.AddOrUpdate(g));
            context.SaveChanges();
        }
    }
}
