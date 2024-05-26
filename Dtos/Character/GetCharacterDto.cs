using Game_Website.Dtos.Skill;
using Game_Website.Dtos.Weapon;

namespace Game_Website.Dtos.Character
{
    public class GetCharacterDto
    {
         public int Id {get;set;}
        public string? Name {get;set;} = "Vall";
        public int HP {get;set;} = 100;
        public int Strength {get;set;} = 10;
        public int Defence {get;set;} = 10;
        public int Intelligence {get;set;} = 10;
        public RpgClass Class { get; set; } = RpgClass.Solo;
        public GetWeaponDto? Weapon {get;set;}
        public List<GetSkillDto>? Skills { get; set; }
        public int Fight {get;set;}
        public int Victories {get;set;}
        public int Defeats {get;set;}
    }
}