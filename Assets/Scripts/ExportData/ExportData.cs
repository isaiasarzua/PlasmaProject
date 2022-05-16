// Uses System.IO.StreamWriter: https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter?view=net-6.0
using UnityEngine;
using System.IO;

public class ExportData : MonoBehaviour
{
    public void ExportTextFile(string inputText)
    {
        // If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.
        StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/Export Files/Log.txt", false);
        writer.WriteLine(inputText);
        writer.Close();
    }
}