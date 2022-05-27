using API.DTOs.UserDTOs;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] PaginationParams pageParams)
        {
            var users = await _unitOfWork.UsersRepository.GetUsers(pageParams);
            if (users?.Count() > 0) return Ok(users);
            return NotFound("No users found...");
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] NewUserDto newUser)
        {
            var user = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName
            };
            _unitOfWork.UsersRepository.AddNewUser(user);
            if (await _unitOfWork.Complete()) return Ok(user);
            return BadRequest("Issue adding new user");
        }
    }
}