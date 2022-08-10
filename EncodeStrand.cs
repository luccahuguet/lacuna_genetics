
namespace App_lacuna
{
  using System;
  using System.Net.Http;
  using System.Text;
  using System.Text.Json;
  using System.Threading.Tasks;
  internal partial class LacunaGenetics
  {
    private static async Task<JobExecutionResponse> EncodeStrandJob(string accessToken, string job_id, string strand)
    {
      var encodedStrand = EncodeStrand(strand);
      var encodeStrandRequest = new EncodeStrandRequest { strandEncoded = encodedStrand };

      var jsonEncodeStrandRequest = JsonSerializer.Serialize(encodeStrandRequest);

      var stringContent = new StringContent(jsonEncodeStrandRequest ?? "", Encoding.UTF8, "application/json");
      var response = await Client.PostAsync(baseAdress + "/api/dna/jobs/" + job_id + "/encode", stringContent);
      var response_str = await response.Content.ReadAsStringAsync();

      Console.Write("JobExecutionResponse as a string:\n" + response_str + "\n");

      var jobExecutionResponse = JsonSerializer.Deserialize<JobExecutionResponse>(response_str);
      return jobExecutionResponse ?? new JobExecutionResponse();
    }
    private static string EncodeStrand(string strand)
    {
      var len = strand.Length;
      byte[] byte_array = new byte[len / 4];

      int i, j;
      for (j = 0; j < len; j++)// iterates over each char
      {
        i = j / 4;
        var binaryEncoding = GetNum(strand[j]);
        var right_shift = (j % 4) * 2;
        byte_array[i] = (byte)((byte_array[i] | (binaryEncoding >> right_shift)));
      }
      return Convert.ToBase64String(byte_array) ?? "";
    }
    private static byte GetNum(char ch)
    {
      switch (ch)
      {
        case 'A':
          return (byte)(0b00 << 6);
        case 'C':
          return (byte)(0b01 << 6);
        case 'G':
          return (byte)(0b10 << 6);
        case 'T':
          return (byte)(0b11 << 6);
        default:
          return (byte)1;

      }
    }
  }
}