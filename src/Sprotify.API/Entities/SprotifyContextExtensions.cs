using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprotify.API.Entities
{
    public static class SprotifyContextExtensions
    {
        public static void EnsureSeedDataForContext(this SprotifyContext context)
        {
            // first, clear the database.  This ensures we can always start 
            // fresh with each demo.  Not advised for production environments, obviously :-)

            context.Playlists.RemoveRange(context.Playlists);
            context.SaveChanges();

            // init seed data
            var playlists = new List<Playlist>()
            {
                new Playlist()
                {
                     Id = new Guid("82b44f74-2bcc-410d-bbe9-9e02697f2b99"),
                     Title = "Daily Mix",
                     Description = "Random daily songs",
                     OwnerId = new Guid("24ec92ba-e6a3-421f-a599-2a0bf88c807a"),
                     Songs = new List<Song>()
                     {
                         new Song()
                         {
                             Id = new Guid("90f5b378-5d91-4776-7c42-08d472cad0c0"),
                              Band = "Smashing Pumpkins",
                              Title = "Bodies"
                         },
                          new Song()
                         {
                             Id = new Guid("86fecb64-c59f-4ede-b5b2-0486a2b80aa8"),
                              Band = "Alice in Chains",
                              Title = "Rooster"
                         },
                        new Song()
                         {
                             Id = new Guid("ad6b9090-0353-4738-90c2-80e1254e5b88"),
                              Band = "Foo Fighters",
                              Title = "Everlong"
                         }
                    }
                },
                new Playlist()
                {
                    Id = new Guid("1cdf8769-bcf7-41f2-a4ba-d3e39132a25e"),
                    Title = "The only playlist you'll ever need",
                    Description = "Nirvana.  What else?",
                    OwnerId = new Guid("24ec92ba-e6a3-421f-a599-2a0bf88c807a"),
                    Songs = new List<Song>()
                    {
                         new Song()
                         {
                             Id = new Guid("cc6f1fad-ece3-42d0-b703-a8f0ae95a4ac"),
                              Band = "Nirvana",
                              Title = "Aneurysm"
                         },
                          new Song()
                         {
                             Id = new Guid("5718200c-9926-4685-b1b0-b1903b23b64a"),
                             Band = "Nirvana",
                             Title = "Scentless Apprentice"
                         },
                           new Song()
                         {
                             Id = new Guid("a0809a6d-f4bf-4d32-bece-8358452f9379"),
                              Band = "Nirvana",
                              Title = "Help me, I'm hungry"
                         },
                            new Song()
                         {
                             Id = new Guid("cbd04f26-e441-4e61-aeae-a65f31915e0b"),
                              Band = "Nirvana",
                              Title = "Pennyroyal Tea"
                         },
                        new Song()
                         {
                             Id = new Guid("6d5ad6bc-6c8f-4795-940b-1b467a5cb1c6"),
                              Band = "Nirvana",
                              Title = "Dumb"
                         }
                    }
                }
            };

            context.Playlists.AddRange(playlists);
            context.SaveChanges();
        }
    }
}
