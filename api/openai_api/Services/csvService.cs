using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using CsvHelper;
using openai_api.DTO;
using openai_api.Mappers;
using CSVReadWrite;
using System.Net;
using CSVFile;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace openai_api.Services
{
    public class csvService
    {
        //dlp system check
        public static bool PostDLPcheck(csvDTO newCheck)
        {
            return true;
        }

        //check the amount of user transactions from today for specific user and return true for allowed users
        public static bool CheckUserUsage(IConfiguration config, string userEmail)
        {
            var path_to_csv = config["path_to_csv"];
            var user_allow_transaction_in_day = config["user_allow_transaction_in_day"];

            if (!int.TryParse(user_allow_transaction_in_day, out int allowedTransactionCount) || string.IsNullOrEmpty(path_to_csv))
            {
                throw new InvalidOperationException("Invalid configuration value for user_allow_transaction_in_day or path_to_csv");
            }
            else
            {
                DateTime today = DateTime.Today;

                if (!File.Exists(path_to_csv))
                {
                    throw new FileNotFoundException($"CSV file not found at path: {path_to_csv}");
                }
                else
                {
                   using (var reader = new StreamReader(path_to_csv))
                   using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
                   {
                       csv.Context.RegisterClassMap<csvMap>();
                       var records = csv.GetRecords<csvDTO>()
                                        .Where(record => record.userEmail == userEmail && record.date == today)
                                        .ToList();
                       int amount = records.Count;

                       return amount < allowedTransactionCount;
                   }
                }
            }
        }

        public static async Task<string> PostOpenAIquestion(Uri callUrl, IConfiguration config, string NewQuestion, string clientIP, string userEmail)
        {
            csvDTO newSession = new csvDTO();
            newSession.userEmail = userEmail;
            newSession.clientIP = clientIP;
            newSession.question = NewQuestion;
            newSession.date = DateTime.Today;
            newSession.dlpAns = PostDLPcheck(newSession);

            bool IsAllowedToContinue = CheckUserUsage(config, userEmail);

            if (IsAllowedToContinue)
            {
                var openai_deployment_url = config["openai_deployment_url"];
                var openai_deployment_key = config["openai_deployment_key"];
                var openai_deployment_name = config["openai_deployment_name"];
                var app_url = config["app_url"];


                if (string.IsNullOrEmpty(openai_deployment_url))
                {
                    throw new InvalidOperationException("openai_deployment_url is missing in the configuration");
                }
                else if (string.IsNullOrEmpty(openai_deployment_key))
                {
                    throw new InvalidOperationException("openai_deployment_key is missing in the configuration");
                }
                else if (string.IsNullOrEmpty(openai_deployment_name))
                {
                    throw new InvalidOperationException("openai_deployment_name is missing in the configuration");
                }
                else if (string.IsNullOrEmpty(app_url))
                {
                    throw new InvalidOperationException("app_url is missing in the configuration");
                }
                else
                {
                    if (callUrl.ToString().Contains(app_url))
                    {
                        newSession.type = "UI";
                    }
                    else
                    {
                        newSession.type = "API";
                    }
                    OpenAIClient client = new OpenAIClient(
                    new Uri(openai_deployment_url),
                    new AzureKeyCredential(openai_deployment_key));
                    string engine = openai_deployment_name;

                    Response<ChatCompletions> response = await client.GetChatCompletionsAsync(engine,
                    new ChatCompletionsOptions()
                    {
                        Messages =
                        {
                    new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information."),
                    new ChatMessage(ChatRole.User, @NewQuestion)
                        },
                        Temperature = (float)0.7,
                        MaxTokens = 800,
                        NucleusSamplingFactor = (float)0.95,
                        FrequencyPenalty = 0,
                        PresencePenalty = 0,
                    });

                    newSession.answer = response.Value.Choices[0].Message.Content;
                    PostNewSession(config, newSession);
                    return response.Value.Choices[0].Message.Content;
                }
            }
            else
            {
                return "You have reached the limit of questions for today\nPlease try again tomorrow";
            }
        }

        //write new session to the database
        public static void PostNewSession(IConfiguration config, csvDTO session)
        {
            var path_to_csv = config["path_to_csv"];

            if (string.IsNullOrEmpty(path_to_csv))
            {
                throw new InvalidOperationException("Invalid configuration value for path_to_csv");
            }
            else
            {
                using (StreamWriter sw = new StreamWriter("data.csv", true, new UTF8Encoding(true)))
                using (CsvWriter cw = new CsvWriter(sw, new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    cw.Context.RegisterClassMap<csvMap>();
                    cw.WriteRecord(session);
                    cw.NextRecord();
                }
            }
        }
    }
}
