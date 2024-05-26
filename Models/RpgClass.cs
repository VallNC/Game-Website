using System.Text.Json.Serialization;

namespace Game_Website.Models
{
 [JsonConverter(typeof(JsonStringEnumConverter))]
   public enum RpgClass
    {
        Rockerboy = 1,
        Solo = 2,
        Netrunner = 3,
        Tech = 4,
        Medtech = 5,
        Media = 6,
        Exec = 7,
        Lawmen = 8,
        Fixer = 9,
        Nomad = 10
    }
}