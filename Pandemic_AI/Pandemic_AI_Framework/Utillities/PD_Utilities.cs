using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Reflection;
using System.Linq;


namespace Pandemic_AI_Framework
{
    public static class PD_Utilities
    {
        public static List<Type> GetAvailableAIPlayerTypes()
        {

            var allAvailableTypes = Assembly.GetExecutingAssembly().GetTypes().ToList();

            var allNonAbstractClasses = allAvailableTypes.FindAll(
                x =>
                x.IsClass == true &&
                x.IsAbstract == false
                );

            var allAIPlayerTypes = allNonAbstractClasses.FindAll(
                x =>
                x.IsSubclassOf(typeof(PD_AI_Action_Agent_Base))
                );

            return allAIPlayerTypes;
        }

        public static T GetDeepCopy_Via_BinarySerialization<T>(this T objectToCopy)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, objectToCopy);

            memoryStream.Position = 0;
            T returnValue = (T)binaryFormatter.Deserialize(memoryStream);

            memoryStream.Close();
            memoryStream.Dispose();

            return returnValue;
        }

        

        

    }
}
