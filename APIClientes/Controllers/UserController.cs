namespace APIClientes.Controllers
{
    using APIClientes.Models;
    using APIClientes.Models.Dto;
    using APIClientes.Repository;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository userrepositorio;
        protected ResponseDto _response;

        public UserController(IUserRepository userrepositorio)
        {
            this.userrepositorio = userrepositorio;
            this._response = new ResponseDto();
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserDto user)
        {
            var response = await userrepositorio.Register(
                    new User
                    {
                        UserName = user.UserName,

                    }, user.Password
                );
            if (response == -1)
            {
                _response.IsSucces = false;
                _response.DisplayMessage = "Usuario ya existe";
                return BadRequest(_response);
            }
            else if (response == -500)
            {
                _response.IsSucces = false;
                _response.DisplayMessage = "Error al crear usuario";
                return BadRequest(_response);
            }
            _response.DisplayMessage = "usuario creado";
            _response.Result = response;
            return Ok(_response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserDto user)
        {
            var response = await userrepositorio.Login(user.UserName, user.Password);

            if (response == "nouser")
            {
                _response.IsSucces = false;
                _response.DisplayMessage = "El usuario no existe";
                return BadRequest(_response);
            }
            else if (response == "wpassword")
            {
                _response.IsSucces = false;
                _response.DisplayMessage = "Clave incorrecta";
                return BadRequest(_response);
            }
            _response.Result = response;
            _response.DisplayMessage = "usuario conectada";
            return Ok(_response);
        }
    }
}
