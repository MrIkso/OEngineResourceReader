
using OEngineResourceReader.Forms;

namespace OEngineResourceReader
{
    public class Program
    {

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            Application.Run(new MainForm());

        }
    }
}