namespace ec.edu.monster.controlador
{
    public class AuthController
    {
        public bool Login(string usuario, string password) =>
            usuario == "MONSTER" && password == "MONSTER9";
    }
}
