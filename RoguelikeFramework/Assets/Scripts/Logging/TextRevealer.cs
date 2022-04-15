using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * Code copied from:
 * https://www.gamasutra.com/blogs/MiguelSantirso/20180129/313803/Better_text_reveals_Unity_code_inside.php
 */
public class TextRevealer : MonoBehaviour
{
	[SerializeField]
	private float waitTime = 0.025f;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void TriggerRevealText(TextMeshProUGUI text)
	{
		StartCoroutine(RevealText(text));
	}

	IEnumerator RevealText(TextMeshProUGUI text)
	{
		var originalString = text.text;
		text.text = "";

		var numCharsRevealed = 0;
		while (numCharsRevealed < originalString.Length)
		{
			//commented this out - it doesn't really affect the speed 
			//and having the spaces take a tick is good for the sound
			//while (originalString[numCharsRevealed] == ' ')
			//	++numCharsRevealed;

			text.text = originalString.Substring(0, ++numCharsRevealed);

			// Skip forward if this is the beginning of a control code
			if (originalString[numCharsRevealed - 1] == '<')
			{
				while (originalString[numCharsRevealed] != '>')
					numCharsRevealed++;
				numCharsRevealed++;
			}

			if(numCharsRevealed == originalString.Length || (numCharsRevealed % 2 == 0 && originalString[numCharsRevealed] != ' ')) {
				// Resolve later - text scroll probably goes through FMOD now
				// AudioManager.i.TextScroll();
			}

			yield return new WaitForSeconds(waitTime);
		}
	}
}
