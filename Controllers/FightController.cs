using Game_Website.Dtos.Fight;
using Microsoft.AspNetCore.Mvc;

namespace Game_Website.Controllers
{
    [ApiController]
    [Route("controller")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;


        public FightController(IFightService fightService){
            _fightService = fightService;

        }
        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
        {
            return Ok(await _fightService.WeaponAttack(request));
        }
        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack (SkillAttackDto request)
        {
            return Ok(await _fightService.SkillAttack(request));
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightRequestDto>>> Fight (FightRequestDto request)
        {
            return Ok(await _fightService.Fight(request));
        }
    }
}