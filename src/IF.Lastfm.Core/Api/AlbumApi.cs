﻿using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Album;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace IF.Lastfm.Core.Api
{
    public class AlbumApi : IAlbumApi
    {
        public ILastAuth Auth { get; private set; }

        public AlbumApi(ILastAuth auth)
        {
            Auth = auth;
        }

        public async Task<LastResponse<LastAlbum>> GetAlbumInfoAsync(string artistname, string albumname, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth, albumname, artistname)
                          {
                              Autocorrect = autocorrect
                          };

            return await command.ExecuteAsync();
        }

        public async Task<LastResponse<LastAlbum>> GetAlbumInfoByMbidAsync(string albumMbid, bool autocorrect = false)
        {
            var command = new GetInfoCommand(Auth)
            {
                AlbumMbid = albumMbid,
                Autocorrect = autocorrect
            };

            return await command.ExecuteAsync();
        }

        //public Task<PageResponse<BuyLink>> GetBuyLinksForAlbumAsync(string artist, string album, CountryCode country, bool autocorrect = false)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<PageResponse<LastTag>> GetTagsByUserAsync(string artist, string album, string username, bool autocorrect = false)
        {
            var command = new GetTagsByUserCommand(Auth, artist, album, username)
            {
                Autocorrect = autocorrect
            };

            return command.ExecuteAsync();
        }

        public async Task<PageResponse<LastTag>> GetTopTagsForAlbumAsync(string artist, string album, bool autocorrect = false)
        {
            var command = new GetTopTagsCommand(Auth)
            {
                ArtistName = artist,
                AlbumName = album
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastAlbum>> SearchForAlbumAsync(string albumname, int page = 1, int itemsPerPage = LastFm.DefaultPageLength)
        {
            var command = new SearchCommand(Auth, albumname)
            {
                Page = page,
                Count = itemsPerPage
            };

            return await command.ExecuteAsync();
        }

        public async Task<PageResponse<LastShout>> GetShoutsAsync(string albumname, string artistname, bool autocorrect = false, int page = 1, int count = LastFm.DefaultPageLength)
        {
            var command = new GetShoutsCommand(Auth, albumname, artistname)
                          {
                              Page = page,
                              Autocorrect = autocorrect,
                              Count = count
                          };

            return await command.ExecuteAsync();
        }

        //public async Task<LastResponse> AddShoutAsync(string albumname, string artistname, string message)
        //{
        //    var command = new AddShoutCommand(Auth, albumname, artistname, message);

        //    return await command.ExecuteAsync();
        //}
    }
}