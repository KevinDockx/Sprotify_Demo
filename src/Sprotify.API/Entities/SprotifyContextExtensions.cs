using System;
using System.Collections.Generic;

namespace Sprotify.API.Entities
{
    public static class SprotifyContextExtensions
    {
        public static void EnsureSeedDataForContext(this SprotifyContext context)
        {
            // first, clear the database.  This ensures we can always start 
            // fresh with each demo.  Not advised for production environments, obviously :-)
            context.Playlists.RemoveRange(context.Playlists);
            context.Songs.RemoveRange(context.Songs);
            context.Users.RemoveRange(context.Users);

            context.SaveChanges();

            // init seed data
            var users = new List<User>()
            {
                new User
                {
                    Id = new Guid("45ae3bb3-5922-42ed-a441-3f78356f3755"),
                    Email = "wesley.cabus@realdolmen.com",
                    Username = "Wesley",
                    Playlists = new List<Playlist>
                    {
                        new Playlist()
                        {
                            Id = new Guid("5c75897a-c97c-4b98-b7c9-435daf99ab49"),
                            Title = "Wesley's Fishy List",
                            Description = "No-one knows.",
                            Songs = new List<PlaylistSong>()
                            {
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("d305389d-2d95-409c-90f4-5cdb96c138a0"),
                                        Band = "Scooter ",
                                        Title = "How Much is the Fish?",
                                        Duration = new TimeSpan(0, 3, 47)
                                    },
                                    Index = 1
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("4d3dfd48-ece8-4419-826a-a392394766f0"),
                                        Band = "Meghan Trainor",
                                        Title = "All About That Bass",
                                        Duration = new TimeSpan(0, 3, 08)
                                    },
                                    Index = 2
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("ba69680c-9f56-4803-af05-cca8214d4da0"),
                                        Band = "Good Shape",
                                        Title = "Bake my Cod",
                                        Duration = new TimeSpan(0, 3, 40)
                                    },
                                    Index = 3
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("41fe6518-01bf-46f0-ad0d-63e031b42b93"),
                                        Band = "Jimmy Frey",
                                        Title = "Breng die zalmen naar Sandra",
                                        Duration = new TimeSpan(0, 2, 58)
                                    },
                                    Index = 4
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("5fca5d89-05bf-4bb0-8be5-f66ded50cb6e"),
                                        Band = "Moby",
                                        Title = "That's When I Reach for my Revolver",
                                        Duration = new TimeSpan(0, 3, 57)
                                    },
                                    Index = 5
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("11eabfb8-d211-4575-8e51-64fbcc729a37"),
                                        Band = "Reel Big Fish",
                                        Title = "Take On Me",
                                        Duration = new TimeSpan(0, 3, 19)
                                    },
                                    Index = 6
                                }
                            }
                        }
                    }
                },
                new User
                {
                    Id = new Guid("24ec92ba-e6a3-421f-a599-2a0bf88c807a"),
                    Email = "kevin.dockx@realdolmen.com",
                    Username = "Kevin",
                    Playlists = new List<Playlist>
                    {
                        new Playlist()
                        {
                            Id = new Guid("82b44f74-2bcc-410d-bbe9-9e02697f2b99"),
                            Title = "Daily Mix",
                            Description = "Random daily songs",
                            Songs = new List<PlaylistSong>()
                            {
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("90f5b378-5d91-4776-7c42-08d472cad0c0"),
                                        Band = "Smashing Pumpkins",
                                        Title = "Bodies",
                                        Duration = new TimeSpan(0, 4, 12)
                                    },
                                    Index = 1
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("86fecb64-c59f-4ede-b5b2-0486a2b80aa8"),
                                        Band = "Alice in Chains",
                                        Title = "Rooster",
                                        Duration = new TimeSpan(0, 6, 14)
                                    },
                                    Index = 2
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("ad6b9090-0353-4738-90c2-80e1254e5b88"),
                                        Band = "Foo Fighters",
                                        Title = "Everlong",
                                        Duration = new TimeSpan(0, 4, 11)
                                    },
                                    Index = 3
                                }
                            }
                        },
                        new Playlist()
                        {
                            Id = new Guid("1cdf8769-bcf7-41f2-a4ba-d3e39132a25e"),
                            Title = "The only playlist you'll ever need",
                            Description = "Nirvana.  What else?",
                            Songs = new List<PlaylistSong>()
                            {
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("cc6f1fad-ece3-42d0-b703-a8f0ae95a4ac"),
                                        Band = "Nirvana",
                                        Title = "Aneurysm",
                                        Duration = new TimeSpan(0, 4, 46)
                                    },
                                    Index = 1
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("5718200c-9926-4685-b1b0-b1903b23b64a"),
                                        Band = "Nirvana",
                                        Title = "Scentless Apprentice",
                                        Duration = new TimeSpan(0, 3, 48)
                                    },
                                    Index = 2
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("a0809a6d-f4bf-4d32-bece-8358452f9379"),
                                        Band = "Nirvana",
                                        Title = "Help me, I'm Hungry",
                                        Duration = new TimeSpan(0, 2, 40)
                                    },
                                    Index = 3
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("cbd04f26-e441-4e61-aeae-a65f31915e0b"),
                                        Band = "Nirvana",
                                        Title = "Pennyroyal Tea",
                                        Duration = new TimeSpan(0, 3, 41)
                                    },
                                    Index = 4
                                },
                                new PlaylistSong() {
                                    Song = new Song()
                                    {
                                        Id = new Guid("6d5ad6bc-6c8f-4795-940b-1b467a5cb1c6"),
                                        Band = "Nirvana",
                                        Title = "Dumb",
                                        Duration = new TimeSpan(0, 2, 53)
                                    },
                                    Index = 5
                                }
                            }
                        }
                    }
                }
            };
            
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
