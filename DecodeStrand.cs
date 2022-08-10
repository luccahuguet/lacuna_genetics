
namespace App_lacuna
{
  using System;
  using System.Net.Http;
  using System.Text;
  using System.Text.Json;
  using System.Threading.Tasks;
  internal partial class LacunaGenetics
  {

    private static async Task<JobExecutionResponse> DecodeStrandJob(string accessToken, string job_id, string strandEncoded)
    {
      var decodedStrand = DecodeStrand(strandEncoded);
      var decodeStrandRequest = new DecodeStrandRequest { strand = decodedStrand };

      var jsonDecodeStrandRequest = JsonSerializer.Serialize(decodeStrandRequest);

      var stringContent = new StringContent(jsonDecodeStrandRequest ?? "", Encoding.UTF8, "application/json");
      var response = await Client.PostAsync(baseAdress + "/api/dna/jobs/" + job_id + "/decode", stringContent);
      var response_str = await response.Content.ReadAsStringAsync();

      Console.Write("JobExecutionResponse as a string:\n" + response_str + "\n");

      var jobExecutionResponse = JsonSerializer.Deserialize<JobExecutionResponse>(response_str);
      return jobExecutionResponse ?? new JobExecutionResponse();
    }
    private static string DecodeStrand(string strandEncoded)
    {
      byte[] byte_array = Convert.FromBase64String(strandEncoded);

      int i, j;
      var strand = "";

      for (j = 0; j < byte_array.Length; j++)
      {
        for (i = 0; i < 4; i++)
        {
          var num = (byte_array[j] & (0b11000000 >> i * 2)) >> 6 - i * 2;
          strand = strand + GetBase(num);
        }
      }
      return strand ?? "";
    }
    private static string GetBase(int my_byte)
    {
      switch (my_byte)
      {
        case 0b00:
          return "A";
        case 0b01:
          return "C";
        case 0b10:
          return "G";
        case 0b11:
          return "T";
        default:
          return "";
      }
    }
  }
}