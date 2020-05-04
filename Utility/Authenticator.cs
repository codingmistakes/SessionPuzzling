using SessionPuzzling.Models;

namespace SessionPuzzling.Utility
{
    public class Authenticator
    {
        public static User Authenticate(string username, string password)
        {
            // authentication mockup
            if (username == "john" && password == "password")
            {
                return new User()
                {
                    Email = "john.doe@example.com",
                    Name = "John Doe",
                    Username = "john",
                    Phone = "1234"
                };
            }

            if (username == "adam" && password == "password")
            {
                return new User()
                {
                    Email = "adam.smith@example.com",
                    Name = "Adam Smith",
                    Username = "adam",
                    Phone = "5678"
                };
            }

            return null;
        }

        public static User FindUser(string phone, string email)
        {
            if (phone == "1234" && email == "john.doe@example.com")
            {
                // find the user and return mockup
                return new User()
                {
                    Username = "john",
                    Email = "john.doe@example.com",
                    Name = "John Doe",
                    Phone = "1234"
                };
            }

            if (phone == "5678" && email == "adam.smith@example.com")
            {
                // find the user and return mockup
                return new User()
                {
                    Username = "adam",
                    Email = "adam.smith@example.com",
                    Name = "Adam Smith",
                    Phone = "5678"
                };
            }

            return null;
        }
    }
}
