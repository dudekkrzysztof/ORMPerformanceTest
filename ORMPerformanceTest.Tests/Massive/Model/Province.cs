namespace Massive.Model
{
    public class Province : DynamicModel
    {
        public Province(string connectionStringName)
            : base(connectionStringName, "Province", "Id")
        {
        }
    }
}