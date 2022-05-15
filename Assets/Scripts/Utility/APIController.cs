// Based on UnityWebRequest: https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html
// System.Net.HttpWebRequest is sometimes preferred over UnityWebRequest, but when building for WebGL, HttpWebRequest is not supported: https://docs.unity3d.com/Manual/webgl-networking.html
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

public class APIController : MonoBehaviour
{
    public static APIController instance;

    private void Awake()
    {
        instance = this;
    }

    // This is using 2 open-source APIs to get random words and their definitions, because Random Word API does not provide definitions.

    // Random Word API: https://github.com/RazorSh4rk/random-word-api
    private IEnumerator GetRandomWords()
    {
        // Get 4 random nouns
        UnityWebRequest nounRequest = UnityWebRequest.Get("https://random-word-form.herokuapp.com/random/noun?count=4");

        // Wait for request
        yield return nounRequest.SendWebRequest();

        // Process request result
        if (nounRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("No response/Connection Error");
        }
        else
        {
            // Parse words
            JArray jArray = JArray.Parse(nounRequest.downloadHandler.text);

            // Return words to WordManager
            yield return JsonConvert.DeserializeObject<string[]>(jArray.ToString());
        }
    }

    // If no definition is found for a word, will return:
    // {"title":"No Definitions Found"}
    // Since this isn't a connection error, we check this manually and skip the word

    // Free Dictionary API: https://github.com/meetDeveloper/freeDictionaryAPI
    private IEnumerator GetWordDefinitions(string[] arr)
    {
        List<Word> results = new List<Word>();

        foreach (string word in arr)
        {
            Debug.Log("Finding definition for: " + word);

            UnityWebRequest definitionRequest = UnityWebRequest.Get("https://api.dictionaryapi.dev/api/v2/entries/en/" + word);

            yield return definitionRequest.SendWebRequest();

            if (definitionRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("No response from website");
            }
            else
            {
                Debug.Log(definitionRequest.downloadHandler.text);

                // If no definition is found for a word, will return:
                // {"title":"No Definitions Found"}
                // Since this isn't a connection error, we check this manually and skip the word
                //if ()
                //{

                //}


                JArray jArray;

                try
                {
                    // Parse definitions
                    jArray = JArray.Parse(definitionRequest.downloadHandler.text);
                }
                catch (JsonReaderException e)
                {
                    Debug.Log("cound not parse json");
                    Debug.Log(e.Message);
                    throw new JsonReaderException("Could not parse, likely because definition was not found.", e);
                }

                // Deserialize into a Word object
                results.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Word>(jArray[0].ToString()));
            }
        }
        // Return to WordManager
        yield return results.ToArray();
    }


    public IEnumerator GetWords(System.Action<string[]> callback)
    {
        var routine = GetRandomWords();
        yield return routine;
        callback((string[])routine.Current);
    }
    public IEnumerator GetDefinition(string[] arr, System.Action<Word[]> callback)
    {
        var routine = GetWordDefinitions(arr);
        yield return routine;
        callback((Word[])routine.Current);
    }
}