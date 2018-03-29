using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RealFishki.Services
{
    class LocalPathToFile : ILocalPathToFile
    {
        public string GetLocalPathToFile(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
        }
    }
}
