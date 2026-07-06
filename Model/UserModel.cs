using System.ComponentModel.DataAnnotations;

namespace OrderManagement.API.Model
{
    public class UserModel
    {

        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        //public string EmailConfirmed { get; set; }
        //= string.Empty;
        //public string PasswordConfirmed { get; set; }
        //= string.Empty;
        public CartModel[]? Cart { get; set; }
        public int[]? PlacedOrders { get; set; }

        public static implicit operator string?(UserModel? v)
        {
            throw new NotImplementedException();
        }
    }




}
