using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

class DreamSimulation
{
    private const string FileName = "Dream_Simulation_Checkpoint.json";
    private const string LogFile = "Trial_Progress_Log.xml";
    private const string ArchiveFolder = "archive";
    private const int LogRetentionDays = 30;  // Keep logs for 30 days

    private static Random random = new Random();
    private static Dictionary<string, double> checkpoint = LoadCheckpoint(FileName);

    private static double maxEnderPearls = checkpoint.ContainsKey("max_ender_pearls") ? checkpoint["max_ender_pearls"] : 0;
    private static double maxBlazeRods = checkpoint.ContainsKey("max_blaze_rods") ? checkpoint["max_blaze_rods"] : 0;
    private static double numberOfTrials = checkpoint.ContainsKey("number_of_trials") ? checkpoint["number_of_trials"] : 0.0;
    private static double bestTrialNumber = checkpoint.ContainsKey("best_trial_number") ? checkpoint["best_trial_number"] : 0;
    private static double bestPearlTrial = checkpoint.ContainsKey("best_pearl_trial") ? checkpoint["best_pearl_trial"] : 0;
    private static double bestRodTrial = checkpoint.ContainsKey("best_rod_trial") ? checkpoint["best_rod_trial"] : 0;
    private static double totalEnderPearls = checkpoint.ContainsKey("total_ender_pearls") ? checkpoint["total_ender_pearls"] : 0;
    private static double totalBlazeRods = checkpoint.ContainsKey("total_blaze_rods") ? checkpoint["total_blaze_rods"] : 0;

    private static double mostPearlsEver = checkpoint.ContainsKey("most_pearls_ever") ? checkpoint["most_pearls_ever"] : 0;
    private static double mostPearlsEverRods = checkpoint.ContainsKey("most_pearls_ever_rods") ? checkpoint["most_pearls_ever_rods"] : 0;
    private static double mostRodsEver = checkpoint.ContainsKey("most_rods_ever") ? checkpoint["most_rods_ever"] : 0;
    private static double mostRodsEverPearls = checkpoint.ContainsKey("most_rods_ever_pearls") ? checkpoint["most_rods_ever_pearls"] : 0;

    static void Main()
    {
        string log_update_message = "";
        int numberOfEnderPearls = 0;
        int numberOfBlazeRods = 0;
        while (true)
        {
            numberOfEnderPearls = 0;
            numberOfBlazeRods = 0;
            numberOfTrials++;

            // Simulating piglin bartering (262 trades)
            for (int i = 0; i < 262; i++)
            {
                if (random.Next(1, 424) <= 20)
                {
                    numberOfEnderPearls++;
                }
            }

            // Simulating blaze fights (305 fights)
            for (int i = 0; i < 305; i++)
            {
                if (random.Next(1, 3) == 2)
                {
                    numberOfBlazeRods++;
                }
            }

            totalEnderPearls += numberOfEnderPearls;
            totalBlazeRods += numberOfBlazeRods;

            bool logUpdate = false;

            if (numberOfBlazeRods > maxBlazeRods || numberOfEnderPearls > maxEnderPearls || numberOfTrials % 10000 == 0)
            {
                if (numberOfEnderPearls > maxEnderPearls && numberOfBlazeRods >= maxBlazeRods)
                {
                    maxEnderPearls = numberOfEnderPearls;
                    maxBlazeRods = numberOfBlazeRods;
                    bestTrialNumber = numberOfTrials;
                    logUpdate = true;
                }

                if (numberOfBlazeRods > mostRodsEver)
                {
                    mostRodsEver = numberOfBlazeRods;
                    mostRodsEverPearls = numberOfEnderPearls;
                    bestRodTrial = numberOfTrials;
                    logUpdate = true;
                }

                if (numberOfEnderPearls > mostPearlsEver)
                {
                    mostPearlsEver = numberOfEnderPearls;
                    mostPearlsEverRods = numberOfBlazeRods;
                    bestPearlTrial = numberOfTrials;
                    logUpdate = true;
                }

                if (numberOfTrials % 10000000 == 0)
                {
                    logUpdate = true;
                }

                if (logUpdate)
                {
                    log_update_message = "";
                    log_update_message += $"Theoretical:            (12.378, 152.5)\n";
                    log_update_message += $"Goal:                   (42, 211)\n";
                    log_update_message += $"Average:                ({(double)totalEnderPearls / numberOfTrials:F3}, {(double)totalBlazeRods / numberOfTrials:F3})\n";
                    log_update_message += $"Best Complete Run:      ({maxEnderPearls}, {maxBlazeRods}, {bestTrialNumber:N0})\n";
                    log_update_message += $"Most Ender Pearls Ever: ({mostPearlsEver}, {mostPearlsEverRods}, {bestPearlTrial:N0})\n";
                    log_update_message += $"Most Blaze Rods Ever:   ({mostRodsEverPearls}, {mostRodsEver}, {bestRodTrial:N0})\n";
                    log_update_message += $"Trial Number:           ({numberOfTrials:N0})\n";
                    SaveCheckpoint(FileName);
                    LogToXml(log_update_message);
                }
            }
        }
    }

    static void SaveCheckpoint(string filePath)
    {
        var checkpointData = new Dictionary<string, double>
        {
            { "total_ender_pearls", totalEnderPearls },
            { "number_of_trials", numberOfTrials },
            { "total_blaze_rods", totalBlazeRods },
            { "max_ender_pearls", maxEnderPearls },
            { "max_blaze_rods", maxBlazeRods },
            { "best_trial_number", bestTrialNumber },
            { "most_pearls_ever", mostPearlsEver },
            { "most_pearls_ever_rods", mostPearlsEverRods },
            { "best_pearl_trial", bestPearlTrial },
            { "most_rods_ever_pearls", mostRodsEverPearls },
            { "most_rods_ever", mostRodsEver },
            { "best_rod_trial", bestRodTrial }
        };

        File.WriteAllText(filePath, JsonConvert.SerializeObject(checkpointData, Formatting.Indented));
    }

    static Dictionary<string, double> LoadCheckpoint(string filePath)
    {
        if (File.Exists(filePath))
        {
            // Read JSON string
            string jsonText = File.ReadAllText(filePath);

            // Deserialize using custom settings
            return JsonConvert.DeserializeObject<Dictionary<string, double>>(
                jsonText,
                new JsonSerializerSettings
                {
                    FloatParseHandling = FloatParseHandling.Double // Ensures all numbers are read as doubles
                }
            );
        }
        return new Dictionary<string, double>();
    }

    static void LogToXml(string message)
    {
        string logFileName = $"Dream_Simulation_Checkpoint_{DateTime.Now:yyyy-MM-dd}.xml";

        if (!File.Exists(logFileName))
        {
            new XDocument(new XElement("logs")).Save(logFileName);
        }

        XDocument doc = XDocument.Load(logFileName);
        doc.Root.Add(new XElement("log",
            new XAttribute("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new XElement("message", message)
        ));

        doc.Save(logFileName);
        RotateLogs();
    }

    static void RotateLogs()
    {
        DateTime cutoffDate = DateTime.Now.AddDays(-LogRetentionDays);
        if (!Directory.Exists(ArchiveFolder))
        {
            Directory.CreateDirectory(ArchiveFolder);
        }

        foreach (string file in Directory.GetFiles(".", "Dream_Simulation_Checkpoint_*.xml"))
        {
            DateTime logDate = DateTime.ParseExact(Path.GetFileNameWithoutExtension(file).Replace("Dream_Simulation_Checkpoint_", ""), "yyyy-MM-dd", null);
            if (logDate < cutoffDate)
            {
                File.Delete(file);
            }
        }
    }
}
