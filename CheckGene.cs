
namespace App_lacuna
{
  using System;
  using System.Net.Http;
  using System.Text;
  using System.Text.Json;
  using System.Threading.Tasks;
  internal partial class LacunaGenetics
  {
    private static async Task<JobExecutionResponse> CheckGeneJob(string accessToken, string job_id, string strandEncoded, string geneEncoded)
    {
      var strand = DecodeStrand(strandEncoded);
      var isActivated = CheckGene(AdjustStrand(strand), DecodeStrand(geneEncoded));
      var checkGeneRequest = new CheckGeneRequest { isActivated = isActivated };

      var jsonCheckGeneRequest = JsonSerializer.Serialize(checkGeneRequest);

      var stringContent = new StringContent(jsonCheckGeneRequest ?? "", Encoding.UTF8, "application/json");
      var response = await Client.PostAsync(baseAdress + "/api/dna/jobs/" + job_id + "/gene", stringContent);
      var response_str = await response.Content.ReadAsStringAsync();

      Console.Write("CheckGeneJob as a string:\n" + response_str + "\n");

      var jobExecutionResponse = JsonSerializer.Deserialize<JobExecutionResponse>(response_str);
      return jobExecutionResponse ?? new JobExecutionResponse();
    }
    private static bool CheckGene(string strand, string geneEncoded)
    {
      var similarity = CalcSimilarity(strand, geneEncoded);
      return similarity > 50 ? true : false;
    }
    private static double CalcSimilarity(string strand, string geneEncoded)
    {
      int max = 0, j, gene_last;
      var gene_clone1 = geneEncoded;
      var gene_clone2 = geneEncoded;

      for (j = 0; j < geneEncoded.Length; j++)
      // this loop tests every gene substring created by removing chars from the left
      {
        max = Math.Max(max, GeneLoop(strand, gene_clone1));
        gene_clone1 = gene_clone1.Substring(1);
      }
      for (gene_last = geneEncoded.Length; gene_last > 0; gene_last--)
      {
        // this loop tests every gene substring created by removing chars from the right
        max = Math.Max(max, GeneLoop(strand, gene_clone2));
        gene_clone2 = gene_clone2.Remove(gene_last - 1);
      }
      return ((double)max / geneEncoded.Length) * 100;

    }
    private static int GeneLoop(string strand, string geneEncoded)
    {

      int counter = 0, max = 0, i, start;

      for (start = 0; start <= strand.Length - geneEncoded.Length; start++)
      // calculates match value, with a dynamic starting point (moving right)
      {
        for (i = start; i < geneEncoded.Length + start; i++)
        {
          if (strand[i] == geneEncoded[i - start])
          {
            counter++;
          }
          else
          {
            max = Math.Max(max, counter);
            counter = 0;
          }
        }
        max = Math.Max(max, counter);
        counter = 0;
      }
      return max;
    }
    private static string AdjustStrand(string strand)
    {
      var updated_strand = strand.Substring(0, 3) == "CAT" ? strand : GetTemplate(strand);
      return updated_strand;
    }
    private static string GetTemplate(string strand)
    {
      var template_strand = "";
      foreach (char ch in strand)
      {
        template_strand = template_strand + GetTemplateBase(ch);
      }
      return template_strand;
    }
    private static string GetTemplateBase(char ch)
    {
      switch (ch)
      {
        case 'T':
          return "A";
        case 'G':
          return "C";
        case 'C':
          return "G";
        case 'A':
          return "T";
        default:
          return "";

      }
    }
  }
}