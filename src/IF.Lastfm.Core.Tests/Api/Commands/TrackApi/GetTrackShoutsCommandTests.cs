﻿using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Track;

namespace IF.Lastfm.Core.Tests.Api.Commands.TrackApi
{
    
    public class GetTrackShoutsCommandTests : CommandTestsBase
    {
        private GetShoutsCommand _command;

        public GetTrackShoutsCommandTests()
        {
            _command = new GetShoutsCommand(MAuth.Object, "Genesis", "Grimes")
                       {
                           Autocorrect = true,
                           Page = 5,
                           Count = 7
                       };

            _command.SetParameters();
        }

        [Test]
        public void Constructor()
        {
            Assert.AreEqual(_command.Method, "track.getShouts");

            Assert.AreEqual(_command.Parameters["track"], "Genesis");
            Assert.AreEqual(_command.Parameters["artist"], "Grimes");
            Assert.AreEqual(_command.Parameters["autocorrect"], "1");
            Assert.AreEqual(_command.Parameters["page"], "5");
            Assert.AreEqual(_command.Parameters["limit"], "7");
            //Assert.AreEqual(_command.Parameters["disablecachetoken"], "1");
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShouts));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(parsed.Page == 5);
            Assert.IsTrue(parsed.Content.Count() == 7);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsSingle));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(parsed.Content.Count() == 1);
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.IsTrue(parsed.Success);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsError));

            var parsed = await _command.HandleResponse(response);

            Assert.IsFalse(parsed.Success);
            Assert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
            Assert.IsNotNull(parsed.Content);
            Assert.IsTrue(!parsed.Content.Any());
        }
    }
}
