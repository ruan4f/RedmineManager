using Newtonsoft.Json;
using RedmineManager.Model;
using RedmineManager.Util;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RedmineManager
{
    public class Program
    {
        static readonly HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            #region Verificar Processo Ativo

            var process = Process.GetCurrentProcess();
            var isRunning = Process.GetProcessesByName(process.ProcessName).Any(p => p.Id != process.Id);

            if (isRunning)
            {
                LogWriter.get().WriteMessage("Já existe uma instancia do processo em execução! Por favor, tente novamente em alguns minutos.");
                return;
            }

            #endregion

            LogWriter.get().WriteMessage("Processo Iniciado!");

            string usuario = ConfigurationManager.AppSettings["Usuario"];
            string senha = ConfigurationManager.AppSettings["Senha"];

            string endPoint = ConfigurationManager.AppSettings["EndPoint"];
            string filePath = ConfigurationManager.AppSettings["FilePath"];
            string fileName = ConfigurationManager.AppSettings["FileName"];

            client.BaseAddress = new Uri(endPoint);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usuario, senha));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            string finalPath = Path.Combine(filePath, fileName);

            // Example #2
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = File.ReadAllLines(finalPath);

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of WriteLines2.txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.                
                string[] valores = line.Split('|');

                RunAsync(valores).GetAwaiter().GetResult();

                /*
                var teste = "{\"issue\":{\"id\":27800,\"project\":{\"id\":94,\"name\":\"SystemsSupport\"},\"tracker\":{\"id\":18,\"name\":\"Analisi\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":2,\"name\":\"Normal\"},\"author\":{\"id\":365,\"name\":\"RuanFerreira\"},\"assigned_to\":{\"id\":365,\"name\":\"RuanFerreira\"},\"subject\":\"[]SUP\\SMILECH\\\",\"start_date\":\"2021-11-24\",\"done_ratio\":100,\"estimated_hours\":1.0,\"total_estimated_hours\":1.0,\"custom_fields\":[{\"id\":37,\"name\":\"Sistema\",\"multiple\":true,\"value\":\"SMILECH\"}],\"created_on\":\"2021-11-24T11:45:45Z\",\"updated_on\":\"2021-11-24T11:45:45Z\"}}";

                teste = teste.Replace("\\", "\\\\");
                */
            }

            Console.WriteLine("");
        }

        private static async Task RunAsync(string[] valores)
        {
            try
            {
                var bodyIssue = new BodyIssue
                {
                    issue = new Issue
                    {
                        project_id = 94,
                        tracker_id = 18,
                        subject = $"[{valores[0]}] SUP\\SMILE CH\\{valores[1]}",
                        status_id = 1,
                        priority_id = 2,
                        assigned_to_id = 365,
                        start_date = "2021-11-24",
                        estimated_hours = 1,
                        done_ratio = 100,
                        custom_field_values = new CustomField { Numero = "SMILE CH" }
                    }
                };


                var issueAsJson = JsonConvert.SerializeObject(bodyIssue);
                var contentIssue = new StringContent(issueAsJson, Encoding.UTF8, "application/json");

                LogWriter.get().WriteMessage($"Criar Tarefa: [{valores[0]}]");
                var responseIssue = await client.PostAsync("issues.json", contentIssue);
                LogWriter.get().WriteMessage($"Lançar hora retorno: [{valores[0]}] - {responseIssue.StatusCode}");

                var responseIssueBody = await responseIssue.Content.ReadAsStringAsync();
                responseIssueBody = responseIssueBody.Replace("\\", "\\\\");

                if (responseIssue.StatusCode.Equals(HttpStatusCode.OK) || responseIssue.StatusCode.Equals(HttpStatusCode.Created))
                {
                    BodyResponseIssue retornoIssue = null;

                    try
                    {
                        retornoIssue = JsonConvert.DeserializeObject<BodyResponseIssue>(responseIssueBody);
                    }
                    catch (JsonReaderException exc)
                    {
                        Console.WriteLine("Invalid JSON." + exc.Message);
                    }

                    var bodyTimeEntry = new BodyTimeEntry
                    {
                        time_entry = new TimeEntry
                        {
                            issue_id = retornoIssue.issue.id,
                            spent_on = "2021-11-24",
                            hours = 0.05,
                            comments = "Análise",
                            activity_id = 8
                        }
                    };

                    var timeEntryAsJson = JsonConvert.SerializeObject(bodyTimeEntry);
                    var contentTimeEntry = new StringContent(timeEntryAsJson, Encoding.UTF8, "application/json");

                    LogWriter.get().WriteMessage($"Lançar hora: [{valores[0]}]");
                    var responseTimeEntry = await client.PostAsync("time_entries.json", contentTimeEntry);
                    LogWriter.get().WriteMessage($"Lançar hora retorno: [{valores[0]}] - {responseTimeEntry.StatusCode}");

                }
            }
            catch (Exception ex)
            {
                LogWriter.get().WriteMessage("Erro - Program: " + ex.Message);
            }
        }
    }
}
