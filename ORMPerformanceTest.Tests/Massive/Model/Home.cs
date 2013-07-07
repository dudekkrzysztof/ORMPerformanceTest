namespace Massive.Model
{
    public class Home : DynamicModel
    {
        public Home(string connectionStringName)
            : base(connectionStringName, "Home", "Id")
        {
        }
    }
}