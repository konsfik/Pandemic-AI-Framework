using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// References to paths, used all over the project, 
    /// gathered here for easy access and global changes
    /// </summary>
    public static class PD_PathReferences
    {

        public static string Get_UserCreatedSavedGames_FolderPath()
        {
            return "C:\\___PD_Output\\fX0_PD_SavedGames";
        }

        public static string Get_RandomSavedGamesPool_FolderPath()
        {
            return "C:\\___PD_Output\\fX1_PD_RandomSavedGamesPool";
        }

        public static string GetExperimentResultsFolderPath()
        {
            return "C:\\___PD_Output\\fA0_PD_ExperimentResults";
        }

        public static string GetExperimentResultsFolderPath(string driveLetter)
        {
            return driveLetter + ":\\___PD_Output\\fA0_PD_ExperimentResults";
        }
    }
}
