using API.DTOs.UserDTOs;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            ITokenService tokenService, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto input)
        {
            var normalizedUsername = input.UserName.ToUpper();
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.NormalizedUserName == normalizedUsername);

            if (user == null) return Unauthorized("No account found with this username/password combination");

            var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);

            if (!result.Succeeded) return Unauthorized("No account found with this username/password combination");

            return _mapper.Map<UserDto>(user);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .ProjectTo<NewUserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (users == null) return NotFound("No users currently exist");

            return Ok(users);
        }

        /*
        [HttpPost("register")]
        public async Task<ActionResult<UserLoginDto>> Register(RegisterDto input)
        {
            if (await UserExists(input.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(input);

            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
        
        */
    }
}