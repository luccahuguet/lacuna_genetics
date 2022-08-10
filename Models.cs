namespace App_lacuna
{
  public class RegisterUserResponse
  {

    public string code { get; set; } = "";
    public string? message { get; set; } = "";

  }

  public class RegisterUserInfo
  {
    public string username { get; set; } = "";
    public string email { get; set; } = "";
    public string password { get; set; } = "";
  }
  public class LoginUserInfo
  {
    public string username { get; set; } = "";
    public string password { get; set; } = "";
  }
  public class TokenRequestResponse
  {

    public string? accessToken { get; set; } = "";
    public string code { get; set; } = "";
    public string? message { get; set; } = "";

  }
  public class Job
  {
    public string id { get; set; } = "";
    public string type { get; set; } = "";
    public string? strand { get; set; } = "";
    public string? strandEncoded { get; set; } = "";
    public string? geneEncoded { get; set; } = "";
  }
  public class JobRequestResponse
  {
    public Job? job { get; set; } = new Job();
    public string code { get; set; } = "";
    public string? message { get; set; } = "";
  }
  public class JobExecutionResponse
  {
    public string code { get; set; } = "";
    public string? message { get; set; } = "";
  }
  public class DecodeStrandRequest
  {
    public string strand { get; set; } = "";
  }
  public class EncodeStrandRequest
  {
    public string strandEncoded { get; set; } = "";
  }
  public class CheckGeneRequest
  {
    public bool isActivated { get; set; } = true;
  }

}