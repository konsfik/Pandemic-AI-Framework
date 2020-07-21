using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Pandemic_AI_Framework
{
    public static class PD_IO_Utilities
    {
        public static DirectoryInfo SolutionDirectory()
        {
            var currentPath = System.IO.Directory.GetCurrentDirectory();

            var directory = new DirectoryInfo(
            currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }

        public static void SerializeToJsonAndSave(
            object objectToSerialize,
            string saveFilePath,
            bool overwriteIfExists,
            bool throwErrorIfExists
            )
        {
            // more details: https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm

            // convert the game to a .json string...
            string objectSerializedToString =
                JsonConvert.SerializeObject(
                    objectToSerialize,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }
                );

            CreateFile(
                saveFilePath,
                overwriteIfExists,
                throwErrorIfExists
                );

            AppendToFile(
                saveFilePath,
                objectSerializedToString
                );
        }

        public static void SerializeGameToJsonAndSave(
            PD_Game gameToSerialize,
            bool compressed,
            string saveFilePath,
            bool overwriteIfExists,
            bool throwErrorIfExists
            )
        {
            // more details: https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm

            // convert the game to a .json string...
            string gameSerializedToString =
                JsonConvert.SerializeObject(
                    gameToSerialize,
                    compressed ? Formatting.None : Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }
                );

            CreateFile(
                saveFilePath,
                overwriteIfExists,
                throwErrorIfExists
                );

            AppendToFile(
                saveFilePath,
                gameSerializedToString
                );
        }

        public static T DeserializeFromJsonFile<T>(
            string jsonFilePath
            )
        {
            if (FileExists(jsonFilePath) == false)
            {
                throw new System.Exception("file does not exist");
            }

            string fileContents = ReadFile(jsonFilePath);

            // for security TypeNameHandling is required when deserializing
            return JsonConvert.DeserializeObject<T>(
                fileContents,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }
            );
        }

        public static PD_Game DeserializeGameFromJsonFile(
            string jsonFilePath
            )
        {
            if (FileExists(jsonFilePath) == false)
            {
                throw new System.Exception("file does not exist");
            }

            string fileContents = ReadFile(jsonFilePath);

            // for security TypeNameHandling is required when deserializing
            return JsonConvert.DeserializeObject<PD_Game>(
                fileContents,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }
            );
        }

        public static string ReadFile(
            string filePath
            )
        {
            if (File.Exists(filePath) == false)
            {
                throw new System.Exception("file does not exist");
            }
            return File.ReadAllText(filePath);
        }

        public static string ReadFileFromFolder(
            string fileName,
            string folderPath
            )
        {
            string filePath = folderPath + "//" + fileName;
            if (File.Exists(filePath) == false)
            {
                throw new System.Exception("file or folder does not exist");
            }
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Serializes any type of class and saves it to disk, provided a full file save path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fileFullPath"></param>
        public static void SerializeToBinaryAndSave<T>(T source, string fileFullPath)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", "source");

            // Don't serialize a null object, simply return the default for that object
            if (object.ReferenceEquals(source, null))
                throw new System.Exception("The object must not be null");

            BinaryFormatter formatter = new BinaryFormatter();

            Stream fileStream = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write);
            formatter.Serialize(fileStream, source);

            fileStream.Close();
        }

        /// <summary>
        /// Deserializes an object into a specific type, given a full file path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static T LoadBinaryAndDeserialize<T>(string fileFullPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            Stream stream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
            T objnew = (T)formatter.Deserialize(stream);

            return objnew;
        }

        public static string[] GetFilePathsInFolder(string folderPath)
        {
            if (FolderExists(folderPath) == false)
            {
                throw new System.Exception("folder does not exist");
            }
            return Directory.GetFiles(folderPath);
        }

        public static string[] GetFilePathsInFolder_Alphanumeric(string folderPath)
        {
            if (FolderExists(folderPath) == false)
            {
                throw new System.Exception("folder does not exist");
            }

            // code borrowed from: https://stackoverflow.com/questions/5093842/alphanumeric-sorting-using-linq
            string[] filePaths = Directory.GetFiles(folderPath);

            string[] sortedFilePaths = filePaths.OrderBy(x => PadNumbers(x)).ToArray();

            return sortedFilePaths.ToArray();
        }

        public static string PadNumbers(string input)
        {
            return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }

        public static bool FolderExists(string folderPath)
        {
            return Directory.Exists(folderPath);
        }

        public static bool IsFolder(string path)
        {
            return Directory.Exists(path);
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static bool IsFile(string path)
        {
            return File.Exists(path);
        }

        public static void CreateFile(string filePath, bool overwriteIfExists, bool throwErrorIfExists)
        {
            if (FileExists(filePath) == true)
            {
                if (throwErrorIfExists == true)
                {
                    throw new System.Exception("cannot overwrite file!");
                }
                else
                {
                    if (overwriteIfExists == true)
                    {
                        var file = File.Create(filePath);
                        file.Close();
                    }
                    else
                    {
                        // do nothing!!!
                    }
                }
            }
            else if (FileExists(filePath) == false)
            {
                var file = File.Create(filePath);
                file.Close();
            }
        }

        public static void AppendToFile(string filePath, string textToAppend)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.Write(textToAppend);
            }
        }

        public static void CreateFolder(string folderPath, bool throwErrorIfExists)
        {
            if (
                FolderExists(folderPath) == true
                && throwErrorIfExists == true
                )
            {
                throw new System.Exception("folder already exists");
            }
            // this will not overwrite a previous directory
            Directory.CreateDirectory(folderPath);
        }


    }

}
