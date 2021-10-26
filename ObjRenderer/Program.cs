using ObjRenderer.Views;
using System;

namespace ObjRenderer
{
    /// <summary>
    /// Class Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [STAThread]
        static void Main(string[] args)
        {
            MainWindow wnd = new MainWindow();
            wnd.ShowDialog();
        }
    }
}
