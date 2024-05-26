namespace Game_Website.Dtos.Character
{
    public class AddCharacterDto
    {
        
        public string? Name {get;set;} = "Vall";
        public int HP {get;set;} = 100;
        public int Strength {get;set;} = 10;
        public int Defence {get;set;} = 10;
        public int Intelligence {get;set;} = 10;
        public RpgClass Class { get; set; } = RpgClass.Solo;
    }
}