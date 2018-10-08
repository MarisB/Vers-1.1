using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ControlitFactory.iOS;
using ControlitFactory.Support;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace ControlitFactory.iOS
{
    public class SaveAndLoad : ISaveAndLoad
    {
        public string SaveText(string filename, string text)
        {
            try
            {
                //var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                //var fi = Path.Combine(path.ToString(), filename);
                //try
                //{
                //    using (var streamWriter = new StreamWriter(fi, false, Portable.Text.Encoding.UTF8))
                //    {
                //        streamWriter.WriteLine(text);
                //    }
                //    return fi;
                //}
                //catch (Exception)
                //{

                //}
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var f = Path.Combine(documents, filename);
                File.WriteAllText(filename, text);

            }
            catch (Exception)
            {

            }
            return "";
        }
        public string LoadText(string filename)
        {
            var documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);

            return System.IO.File.ReadAllText(filePath);
        }

        public void DeleteFile(string filename)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filename))
                    File.Delete(filename);
            }
            catch (Exception)
            {

            }
        }
    }
}
