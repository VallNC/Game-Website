using Game_Website.Dtos.Fight;

namespace Game_Website.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;


        public FightService(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        public async Task<ServiceResponse<FightResultsDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultsDto>{
                Data = new FightResultsDto()
            };
            try{
                var characters = await _dataContext.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => request.CharacterIds.Contains(c.Id))
                .ToListAsync();
                bool defeated = false;
                while (!defeated){
                    foreach(var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];
                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2)==0;
                        if (useWeapon&& attacker.Weapon is not null){
                            attackUsed = attacker.Weapon.Name;    
                            damage = DoWeaponAttack(attacker,opponent);                       
                        }
                        else if (!useWeapon && attacker.Skills is not null){
                      var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                      attackUsed = skill.Name;
                      damage = DoSkillAttack(attacker,opponent,skill);
                        }
                        else 
                        {
                        response.Data.Log.Add($"{attacker.Name} was not able to attack :( ");
                        continue;
                        }
                         response.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} for {(damage >=0 ? damage : 0)} damage :) ");
                         if(opponent.HP<=0){
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} was defeated by {attacker.Name}");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HP} HP left");
                            break;
                         }
                    }
                }
                characters.ForEach(c => 
                {c.Fight++;
                c.HP = 100;
                });
                await _dataContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return response;
            }
            return response;
        }


        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
           var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _dataContext.Characters.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var defender = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == request.DefenderId);
                if (attacker is null || defender is null || attacker.Skills is null)
                    throw new Exception("Something fishy is happening?????");
                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill is null)
                {
                    response.Success = false;
                    response.Message = $"{attacker.Name} does not have the necessary skill.";
                    return response;
                }
                int damage = DoSkillAttack(attacker, defender, skill);
                if (defender.HP <= 0)
                {
                    response.Message = $"{defender.Name} was defeated!";
                }
                await _dataContext.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name!,
                    Defender = defender.Name!,
                    AttackerHP = attacker.HP,
                    DefenderHP = defender.HP,
                    Damage = damage
                };
            }
            catch (Exception ex){
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoSkillAttack(Character attacker, Character defender, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(defender.Defence);
            if (damage >= 0)
                defender.HP -= damage;
            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _dataContext.Characters.Include(c => c.Weapon).FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var defender = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == request.DefenderId);
                if (attacker is null || defender is null || attacker.Weapon is null)
                    throw new Exception("Something fishy is happening?????");

                int damage = DoWeaponAttack(attacker, defender);

                if (defender.HP <= 0)
                {
                    response.Message = $"{defender.Name} was defeated!";
                }
                await _dataContext.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name!,
                    Defender = defender.Name!,
                    AttackerHP = attacker.HP,
                    DefenderHP = defender.HP,
                    Damage = damage
                };
            }
            catch (Exception ex){
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character defender)
        {
            if(attacker.Weapon is null)
            throw new Exception ("Attacker has no weapon");
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(defender.Defence);
            if (damage >= 0)
                defender.HP -= damage;
            return damage;
        }
    }
}