using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pandemic_AI_Framework
{
    public static class DataUtilities
    {
        /// <summary>
        /// This method is responsible to: 
        /// - find the correct folder where the game creation data are located, 
        /// - read the game data file included in this folder
        /// - return its contents.
        /// 
        /// </summary>
        /// <param name="dataFileName"></param>
        /// <returns></returns>
        public static string Read_GameCreationData()
        {
            string dataFileName = "gameCreationData.csv";
            string currentDirectory = Directory.GetCurrentDirectory();
            string dataDirectory = currentDirectory + "\\GameCreationData";
            string gameCreationData = PD_IO_Utilities.ReadFileFromFolder(dataFileName, dataDirectory);
            return gameCreationData;
        }

    }
}
