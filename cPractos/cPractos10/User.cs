namespace CarShowroomApp
{
    class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        


        public User(string login, string password, Role role, string name)
        {
            
            Login = login;
            Password = password;
            Name = name;
            Role = role;
      
        }
    }
}