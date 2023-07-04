using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class LoadSaveName 
{
    public static string TryLoadName()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string Path = Application.persistentDataPath + "/Main/Name.Bin";

        if (File.Exists(Path))
        {
            FileStream Stream = new FileStream(Path, FileMode.Open);

            NameSaveData NameData = formatter.Deserialize(Stream) as NameSaveData;

            Stream.Close();

            return NameData.Name;
        }
        else
        {
            return null;
        }
    }
    public static void WriteName(string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string Path = Application.persistentDataPath + "/Main";

        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);

        }

        FileStream Stream = new FileStream(Path + "/Name.Bin", FileMode.Create);
        NameSaveData saveData = new NameSaveData(name);

        formatter.Serialize(Stream, saveData);
        Stream.Close();

    }
}
