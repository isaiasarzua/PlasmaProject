using System;
using Newtonsoft.Json;
using UnityEngine;

/*
Free Dictionary API JSON Model:
	[
	{
		"word": "teacher",
		"phonetic": "/ˈtiːt͡ʃə/",
		"phonetics": [
			{
				"text": "/ˈtiːt͡ʃə/",
				"audio": "https://api.dictionaryapi.dev/media/pronunciations/en/teacher-uk.mp3",
				"sourceUrl": "https://commons.wikimedia.org/w/index.php?curid=9027555",
				"license": {
					"name": "BY 3.0 US",
					"url": "https://creativecommons.org/licenses/by/3.0/us"
				}
			},
			{
	"text": "/ˈtit͡ʃɚ/",
				"audio": "https://api.dictionaryapi.dev/media/pronunciations/en/teacher-us.mp3",
				"sourceUrl": "https://commons.wikimedia.org/w/index.php?curid=1769786",
				"license": {
		"name": "BY-SA 3.0",
					"url": "https://creativecommons.org/licenses/by-sa/3.0"
				}
}
		],
		"meanings": [
			{
				"partOfSpeech": "noun",
				"definitions": [
					{
						"definition": "A person who teaches, especially one employed in a school.",
						"synonyms": [],
						"antonyms": []
					},
					{
	"definition": "The index finger; the forefinger.",
						"synonyms": [],
						"antonyms": []
					},
					{
	"definition": "An indication; a lesson.",
						"synonyms": [],
						"antonyms": []
					},
					{
	"definition": "The second highest office in the Aaronic priesthood, held by priesthood holders of at least the age of 14.",
						"synonyms": [],
						"antonyms": []
					}
				],
				"synonyms": [
					"preceptor"
				],
				"antonyms": []
			}
		],
		"license": {
	"name": "CC BY-SA 3.0",
			"url": "https://creativecommons.org/licenses/by-sa/3.0"
		},
		"sourceUrls": [
			"https://en.wiktionary.org/wiki/teacher"
		]
	}
]
*/

// Removed unnecessary fields license, sourceUrls, phonetic, synonyms and antonyms
public partial class Word : MonoBehaviour
{
    [JsonProperty("word")]
    public string word { get; set; }

    [JsonProperty("phonetics")]
    public Phonetic[] Phonetics { get; set; }

    [JsonProperty("meanings")]
    public Meaning[] Meanings { get; set; }
}

public partial class Meaning
{
    [JsonProperty("partOfSpeech")]
    public string PartOfSpeech { get; set; }

    [JsonProperty("definitions")]
    public Definition[] Definitions { get; set; }
}

public partial class Definition
{
    [JsonProperty("definition")]
    public string text { get; set; }
}

public partial class Phonetic
{
    [JsonProperty("text")]
    public string text { get; set; }

    [JsonProperty("audio")]
    public Uri Audio { get; set; }
}