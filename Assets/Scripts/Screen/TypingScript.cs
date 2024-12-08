using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TypingScript : MonoBehaviour
{
    private string textToTyping;
    public TextMeshPro textDisplay;
    public string fullText;
    public int letterCount = 0;

    private int currentIndex = 0;
    private string currentDisplayedText = "";
    private int maxLines = 26;
    public int keyAtTheSameTime = 10;

    private void Awake()
    {
        GameMetrics.Global.ResetLetters();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < keyAtTheSameTime; i++)
            {
                TypeNextCharacter();
            }
            UpdateDisplay();
            StatsManager.Instance.OnKeyTapped?.Invoke(1);
        }
    }

    void TypeNextCharacter()
    {
        if (currentIndex >= fullText.Length)
            return;

        char currentChar = fullText[currentIndex];

        if (currentChar == '<')
        {
            int tagEndIndex = fullText.IndexOf('>', currentIndex);
            if (tagEndIndex != -1)
            {
                string tag = fullText.Substring(currentIndex, tagEndIndex - currentIndex + 1);
                currentDisplayedText += tag;
                currentIndex = tagEndIndex + 1;
                return;
            }
        }

        currentDisplayedText += currentChar;
        currentIndex++;
        letterCount++;
        GameMetrics.Global.AddLetter();
    }

    void UpdateDisplay()
    {
        List<string> lines = currentDisplayedText.Split("\\n").ToList();

        if (lines.Count > maxLines)
        {
            if(lines[0].Length > 0)
                currentDisplayedText = currentDisplayedText.Replace(lines[0], string.Empty);
            currentDisplayedText = RemoveFirstNewline(currentDisplayedText);
            lines.RemoveAt(0);
        }
        Debug.Log(lines.Count + ": " + maxLines);
        textDisplay.text = currentDisplayedText;
    }
    
    string RemoveFirstNewline(string input)
    {
        // Look for the first occurrence of "\\n" and remove it
        int index = input.IndexOf("\\n");
        if (index != -1)
        {
            input = input.Remove(index, 2); // Remove "\\n" (2 characters)
        }

        return input;
    }
}

