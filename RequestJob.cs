namespace App_lacuna
{
  using System.Text.Json;
  using System.Threading.Tasks;
  internal partial class LacunaGenetics
  {
    private static async Task<JobRequestResponse> RequestJob(string accessToken)
    {

      var response = await Client.GetAsync(baseAdress + "/api/dna/jobs");
      var content = await response.Content.ReadAsStringAsync();
      var jobRequestResponse = JsonSerializer.Deserialize<JobRequestResponse>(content);
      return jobRequestResponse ?? new JobRequestResponse();
    }

  }
}

