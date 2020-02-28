using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForbiddenWordsSearch
{
    static class Program
    {
        private const string appGuid = "9de1f651-5d37-4d75-b59c-003a0ea34c1d";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using var mutex = new Mutex(false, @$"Global\{appGuid}");
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("Instance already running");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WordsForm());
        }
    }
}
