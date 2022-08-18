using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AmazingChatBot
{
    static class SaveLoadManager
    {
        public static async Task<string> LoadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            return await File.ReadAllTextAsync(filePath);
        }

        public static async Task SaveFileAsync(string filePath, object dataToSave)
        {
            try
            {
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(dataToSave, Newtonsoft.Json.Formatting.Indented);
                await File.WriteAllTextAsync(filePath, jsonData, Encoding.UTF8);
            }
            catch (Exception e) { Program.DisplayMessageInConsole($"ERROR WHILE SAVING DATA: {e.Message}", ConsoleColor.Red); }
        }
    }
}
