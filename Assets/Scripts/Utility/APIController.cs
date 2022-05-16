// Based on UnityWebRequest: https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html
// System.Net.HttpWebRequest is sometimes preferred over UnityWebRequest, but when building for WebGL, HttpWebRequest is not supported: https://docs.unity3d.com/Manual/webgl-networking.html
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class APIController : MonoBehaviour
{
    public static APIController instance;
    private int timeout = 2;

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
                Debug.Log("Received response");

                // Since we are given an array, remove surrounding [] using Substring, then serialize to string
                string newNoun = JsonConvert.DeserializeObject<string>(nounRequest.downloadHandler.text.Substring(1, nounRequest.downloadHandler.text.Length - 2));

                // Find definition of word
                UnityWebRequest definitionRequest = UnityWebRequest.
                    Get("https://api.dictionaryapi.dev/api/v2/entries/en/" + newNoun);

                definitionRequest.timeout = timeout;

                // Wait for request
                yield return definitionRequest.SendWebRequest();

                // Process request result
                if (definitionRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    // If Dictionary API is not available skip it and manually build word before returning to WordManager
                    Word newWord = new Word();
                    newWord.word = newNoun;
                    newWord.Meanings = new Meaning[] {
                        new Meaning { PartOfSpeech = "noun", Definitions =
                        new Definition[] { new Definition { text = "Not available" } } } };

                    words.Add(newWord);
                }
                else if (definitionRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Received response from API");
                    Debug.Log(definitionRequest.result);
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
                        Debug.Log("continuing process");
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
            UnityWebRequest adjRequest = UnityWebRequest.Get("https://random-word-form.herokuapp.com/random/adjective");

            // Wait for request
            yield return adjRequest.SendWebRequest();

            // Process request result
            if (adjRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("No response/Connection Error");
            }
            else
            {
                // Since we are given an array, remove surrounding [] using Substring, then serialize to string
                string newAdj = JsonConvert.DeserializeObject<string>(adjRequest.downloadHandler.text.Substring(1, adjRequest.downloadHandler.text.Length - 2));

                // Find definition of word
                UnityWebRequest definitionRequest = UnityWebRequest.
                    Get("https://api.dictionaryapi.dev/api/v2/entries/en/" + newAdj);

                definitionRequest.timeout = timeout;

                yield return definitionRequest.SendWebRequest();

                // Process request result
                if (definitionRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    // If Dictionary API is not available skip it and manually build word before returning to WordManager
                    Word newWord = new Word();
                    newWord.word = newAdj;
                    newWord.Meanings = new Meaning[] {
                        new Meaning { PartOfSpeech = "adjective", Definitions =
                        new Definition[] { new Definition { text = "Not available" } } } };

                    words.Add(newWord);
                }
                else if (definitionRequest.result == UnityWebRequest.Result.Success)
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