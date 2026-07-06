using System.Text.RegularExpressions;
using OrderManagement.API.DTOs;
using OrderManagement.API.Model;
using OrderManagement.API.Repositories;
using OrderManagement.API.Utills;

namespace OrderManagement.API.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly JWTTokenService _jwtTokenService;
        public UserService(UserRepository userRepository, JWTTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<ApiResponse<UserRegisterDTO>> RegisterUser(UserRegisterDTO user) 
        {
            try
            {
                if (user.Name.Trim() == "")
                    return new ApiResponse<UserRegisterDTO> { Message = "Name is required", Success = false, Data = user };
                if (user.Password.Trim() == "")
                    return new ApiResponse<UserRegisterDTO> { Message = "Password is required", Success = false, Data = user };
                else if (user.Password.Length < 8)
                    return new ApiResponse<UserRegisterDTO> { Message = "Password length atleast must be 8", Success = false, Data = user };
                if (user.Email.Trim() == "")
                    return new ApiResponse<UserRegisterDTO> { Message = "email is required", Success = false, Data = user };
                else if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return new ApiResponse<UserRegisterDTO> { Message = "Invalid email", Success = false, Data = user };
                else if(await _userRepository.FindByEmail(user.Email)!=null)
                        return new ApiResponse<UserRegisterDTO> { Message = "User Already Exist. Please Login", Success = false, Data = user };

                var userData = new UserModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Cart = [],
                    PlacedOrders = []
                };

                var res = await _userRepository.RegisterUser(userData);
                return new ApiResponse<UserRegisterDTO>
                {
                    Success = true,
                    Message = "Created",
                    Data = new UserRegisterDTO { Email = res.Email,Password = res.Password,Name = res.Name },
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse<UserRegisterDTO> { Message = ex.Message, Success = false, Data = user };
            }

        }
        public async Task<ApiResponse<LoginResponseDTO>> LoginUser(UserLoginDTO user)
        {
            if (user.Password.Trim() == "")
                return new ApiResponse<LoginResponseDTO> { Message = "Password is required", Success = false,
                    Data = new LoginResponseDTO
                    {
                        token = ""
                    }
                };
            else if (user.Password.Length < 8)
                return new ApiResponse<LoginResponseDTO> { Message = "Password length atleast must be 8", Success = false,
                    Data = new LoginResponseDTO
                    {
                        token = ""
                    }
                };
            if (user.Email.Trim() == "")
                return new ApiResponse<LoginResponseDTO> { Message = "email is required", Success = false,
                    Data = new LoginResponseDTO
                    {
                        token = ""
                    }
                };
            else if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return new ApiResponse<LoginResponseDTO> { Message = "Invalid email", Success = false,
                    Data = new LoginResponseDTO
                    {
                        token = ""
                    }
                };

            var u = await _userRepository.FindByEmail(user.Email);
            if (u==null)
                return new ApiResponse<LoginResponseDTO> { Message = "User not found Exist. Please check", Success = false,
                    Data = new LoginResponseDTO
                    {
                        token = ""
                    }
                };
            // need to change something
            var token = _jwtTokenService.GenerateToken(new UserTokenGenerateDTO { Email=user.Email,Name=u.Name,Id=u.Id});

            return new ApiResponse<LoginResponseDTO> { Success = true,
                Data = new LoginResponseDTO
                {
                    token = token
                }, Message = "Token Created" };



        }
        public async Task <ApiResponse<UserModel>>GetUser(int userId)
        {
            //handle error later
            var user = await _userRepository.GetUserById(userId);
            if (user == null) throw new Exception("User Not Found");
            return new ApiResponse<UserModel> { Data = user, Success = true };
        }
   
    }
}
