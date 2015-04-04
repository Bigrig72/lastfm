using System;
using System.Net.Http;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IF.Lastfm.Core.Api.Commands.Library
{
    internal class GetTracksCommand : GetAsyncCommandBase<PageResponse<LastTrack>>
    {
        public string Username { get; private set; }

        public string Artist { get; private set; }

        public string Album { get; private set; }

        public DateTimeOffset From { get; private set; }

        public GetTracksCommand(ILastAuth auth, string username, string artist, string album, DateTimeOffset from)
            : base(auth)
        {
            Method = "library.getTracks";

            Username = username;
            Artist = artist;
            Album = album;
            From = from;
        }

        public override void SetParameters()
        {
            Parameters.Add("user", Username);
            Parameters.Add("artist", Artist);
            Parameters.Add("album", Album);
            Parameters.Add("from", From.AsUnixTime().ToString());

            AddPagingParameters();
            DisableCaching();
        }

        public async override Task<PageResponse<LastTrack>> HandleResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            LastResponseStatus status;
            if (LastFm.IsResponseValid(json, out status) && response.IsSuccessStatusCode)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(json).SelectToken("tracks");
                var tracksToken = jtoken.SelectToken("track");
                var pageInfoToken = jtoken.SelectToken("@attr");
 
                return PageResponse<LastTrack>.CreateSuccessResponse(tracksToken, pageInfoToken, LastTrack.ParseJToken, LastPageResultsType.Attr);
            }
            else
            {
                return LastResponse.CreateErrorResponse<PageResponse<LastTrack>>(status);
            }
        }
    }
}