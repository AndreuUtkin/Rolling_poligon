namespace Катящийся_многоугольник
{
    internal static class Program
    {

        [STAThread]    
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new PolygonForm(3,50,20,3,50,true));
        }
    }
}