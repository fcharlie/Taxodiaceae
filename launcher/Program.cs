using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Taxodiaceae
{
    class App
    {
        static int Main(string[] args)
        {
            var selfdir = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var selfname = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //var filename =  selfdir+ "\\bin\\Taxodiaceae.exe";
            var filename = System.IO.Path.Combine(selfdir, "bin\\Taxodiaceae.exe");
            //MessageBox.Show(filename);
            if (!System.IO.File.Exists(filename))
            {
                if (String.Compare(selfname,"Taxodiaceae.exe",true)==0)
                {
                    MessageBox.Show(filename, "Not Found Taxodiaceae", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
                filename = System.IO.Path.Combine(selfdir, "Taxodiaceae.exe");
                if (!System.IO.File.Exists(filename))
                {
                    MessageBox.Show(filename, "Not Found Taxodiaceae", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
            Process ps = new Process();
            ps.StartInfo.FileName = filename;
            if(args.Length>=1 &&System.IO.File.Exists(args[0])){
                ps.StartInfo.Arguments = args[0];
            }
            try
            {
                ps.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Start Taxodiaceae Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return 0;
        }
    }
}
