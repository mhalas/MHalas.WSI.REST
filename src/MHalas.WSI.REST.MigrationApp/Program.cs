namespace MHalas.WSI.REST.MigrationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MigrationData migration = new MigrationData();
            migration.Migrate();
        }
    }
}
