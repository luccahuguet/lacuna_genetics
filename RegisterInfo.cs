namespace App_lacuna
{
  using System.Net.Http;
  using System.Text;
  using System.Text.Json;
  using System.Threading.Tasks;
  internal partial class LacunaGenetics
  {
    private static async Task PostUser()
    {

      var registerUserInfo = new RegisterUserInfo { username = "lucca9", email = "lucca.huguet2@gmail.com", password = "api_password" };

      var requestUri = baseAdress + "/api/users/create";
      var stringContent = new StringContent(registerUserInfo.ToString() ?? "", Encoding.UTF8, "application/json");

      var response = await Client.PostAsync(requestUri, stringContent);
      var content = await response.Content.ReadAsStringAsync();

      var registerUserResponse = JsonSerializer.Deserialize<RegisterUserResponse>(content);
    }

    private static async Task<TokenRequestResponse> RequestAccessToken()
    {

      var my_user = new LoginUserInfo { username = "lucca9", password = "api_password" };

      string jsonString = JsonSerializer.Serialize(my_user);

      var requestUri = baseAdress + "/api/users/login";
      var stringContent = new StringContent(jsonString ?? "", Encoding.UTF8, "application/json");

      var response = await Client.PostAsync(requestUri, stringContent);
      var content = await response.Content.ReadAsStringAsync();
      var tokenRequestResponse = JsonSerializer.Deserialize<TokenRequestResponse>(content);

      return tokenRequestResponse ?? new TokenRequestResponse();
    }

  }
}

