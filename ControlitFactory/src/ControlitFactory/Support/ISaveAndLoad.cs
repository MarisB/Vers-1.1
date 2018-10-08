using System;
using System.Collections.Generic;
using System.Text;

namespace ControlitFactory.Support
{
    public interface ISaveAndLoad
    {
        string SaveText(string filename, string text);
        string LoadText(string filename);
        void DeleteFile(string filename);
    }
}
