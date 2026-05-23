using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test;

namespace WebApi.Tests.Login.DoLogin
{
    public class DoLoginTest : BarberBossClassFixture
    {
        private const string METHOD = "api/Login";

        private readonly string _name;
        private readonly string _email;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _name = webApplicationFactory.User_Team_Member.GetName();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var result = await DoPost(METHOD, request);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("name").GetString().Should().Be(_name);
            response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_Login_Invalid()
        {
            var request = RequestLoginJsonBuilder.Build();

            var result = await DoPost(requestUri: METHOD, request: request);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
        }
    }
}
