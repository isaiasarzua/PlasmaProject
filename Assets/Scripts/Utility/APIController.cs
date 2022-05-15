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
    private IEnumerator GetRandomWords(int wordCount)
    {
        // Prep our return
        List<Word> words = new List<Word>();

        // Get nouns and definitions
        for (int i = 0; i < wordCount; i++)
        {
            UnityWebRequest nounRequest = UnityWebRequest.Get("https://random-word-form.herokuapp.com/random/noun");

            // Wait for request
            yield return nounRequest.SendWebRequest();

            // Process request result
            if (nounRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("No response/Connection Error");
            }
            else
            {
                Debug.Log(nounRequest.downloadHandler.text);

                // Find definition of word
                // Since we are given an array, remove first and last character using Substring
                UnityWebRequest definitionRequest = UnityWebRequest.
                    Get("https://api.dictionaryapi.dev/api/v2/entries/en/" + JsonConvert.
                    DeserializeObject<string>(nounRequest.downloadHandler.text.Substring(1, nounRequest.downloadHandler.text.Length - 2)));

                yield return definitionRequest.SendWebRequest();

                // Process request result
                if (definitionRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("No response/Connection Error");
                }
                else
                {
                    // Process JSON
                    JArray jArray;
                    try
                    {
                        // Parse definitions
                        jArray = JArray.Parse(definitionRequest.downloadHandler.text);
                    }
                    catch (JsonReaderException)
                    {
                        // Could not parse because definition was not found. Get another word and repeat process.
                        i--;
                        continue;
                    }
                    words.Add(JsonConvert.DeserializeObject<Word>(jArray[0].ToString()));
                }
            }
        }


        // Repeat the process for adjectives
        for (int i = 0; i < wordCount; i++)
        {
            UnityWebRequest adjRequest = UnityWebRequest.Get("https://random-word-form.herokuapp.com/random/noun");

            // Wait for request
            yield return adjRequest.SendWebRequest();

            // Process request result
            if (adjRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("No response/Connection Error");
            }
            else
            {
                Debug.Log(adjRequest.downloadHandler.text);

                // Find definition
                // Since it's given as an array, remove first and last character using Substring
                UnityWebRequest definitionRequest = UnityWebRequest.
                    Get("https://api.dictionaryapi.dev/api/v2/entries/en/" + JsonConvert.
                    DeserializeObject<string>(adjRequest.downloadHandler.text.Substring(1, adjRequest.downloadHandler.text.Length - 2)));

                yield return definitionRequest.SendWebRequest();

                // Process request result
                if (definitionRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("No response/Connection Error");
                }
                else
                {
                    // Process JSON
                    JArray jArray;
                    try
                    {
                        // Parse definitions
                        jArray = JArray.Parse(definitionRequest.downloadHandler.text);
                    }
                    catch (JsonReaderException)
                    {
                        // Could not parse because definition was not found. Get another word and repeat process.
                        i--;
                        continue;
                    }
                    words.Add(JsonConvert.DeserializeObject<Word>(jArray[0].ToString()));
                }
            }
        }

        // Return words to WordManager
        yield return words;
    }

    public IEnumerator GetWords(System.Action<List<Word>> callback)
    {
        var routine = GetRandomWords(4);
        yield return routine;
        callback((List<Word>)routine.Current);
    }
}