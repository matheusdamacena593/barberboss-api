using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test;

namespace WebApi.Tests.Users.ChangePassword
{
    public class ChangePasswordTest : BarberBossClassFixture
    {
        private const string METHOD = "api/User/change-password";

        private readonly string _token;
        private readonly string _password;
        private readonly string _email;

        public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            var response = await DoPut(requestUri: METHOD, request: request, token: _token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var loginRequest = new RequestLoginJson
            {
                Email = _email,
                Password = _password,
            };

            response = await DoPost(requestUri: "api/Login", request: loginRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            loginRequest.Password = request.NewPassword;

            response = await DoPost(requestUri: "api/Login", request: loginRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Error_Password_Different_Current_Password()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var result = await DoPut(requestUri: METHOD, request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }
    }
}
