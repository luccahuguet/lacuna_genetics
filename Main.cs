namespace App_lacuna
{
  using System;
  using System.Net.Http;
  using System.Threading.Tasks;
  internal partial class LacunaGenetics
  {
    private static readonly HttpClient Client = new HttpClient();
    public static readonly string baseAdress = "https://gene.lacuna.cc/";

    private static async Task Main(string[] args)
    {
      // await PostUser(); // useful to create new users

      var tokenResponse = await RequestAccessToken();
      var accessToken = tokenResponse.accessToken ?? "";

      Client.DefaultRequestHeaders.Add("Authorization", accessToken);

      var jobRequestResponse = await RequestJob(accessToken);
      var job_type = jobRequestResponse.job?.type ?? "";
      var job_id = jobRequestResponse.job?.id ?? "";

      Console.WriteLine("\n\nJob type: " + job_type); // Helpful print
      var strand = "";

      // notice I let the responses available below if you want to print them.
      switch (job_type)
      {
        case "DecodeStrand":
          var strandEncoded = jobRequestResponse.job?.strandEncoded ?? "";
          var decodeStrandResponse = await DecodeStrandJob(accessToken, job_id, strandEncoded);
          break;
        case "EncodeStrand":
          strand = jobRequestResponse.job?.strand ?? "";
          var encodeStrandResponse = await EncodeStrandJob(accessToken, job_id, strand);
          break;
        case "CheckGene":
          strandEncoded = jobRequestResponse.job?.strandEncoded ?? "";
          var geneEncoded = jobRequestResponse.job?.geneEncoded ?? "";
          var isActivated = await CheckGeneJob(accessToken, job_id, strandEncoded, geneEncoded);
          break;
        default:
          break;
      }
    }
  }
}

