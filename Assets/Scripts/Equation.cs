using System;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Equation", menuName = "Custom/Equation")]
public class Equation : ScriptableObject
{

    public Element[] inputs;
    public Element[] outputs;
    public VideoClip result;
    public Question[] questions;

    public int IndexOfInputs(Element element) => IndexOf(inputs, element);
    public int IndexOfOutputs(Element element) => IndexOf(outputs, element);
    private int IndexOf(Element[] arr, Element element)
    {
        if (element == null) return -1;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].name == element.name) return i;
        }

        return -1;
    }

    public bool IsValid(Element element) {
        if (element == null) return false;
        
        foreach (Element e in inputs)
        {
            if (e.name == element.name) {
                return true;
            }
        }

        foreach (Element e in outputs)
        {
            if (e.name == element.name) {
                return true;
            }
        }

        return false;
    }

    [Serializable]
    public struct Question {
        public string content;
        public string[] answers;
        public int correctAnswer;
        
    }
  
}
