using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test;

namespace WebApi.Tests.Users.Update
{
    public class UpdateUserTest : BarberBossClassFixture
    {
        private const string METHOD = "api/User";

        private readonly string _token;

        public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var result = await DoPut(requestUri: METHOD, request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Error_Empty_Name()
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var result = await DoPut(requestUri: METHOD, request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.NAME_EMPTY));
        }
    }
}
