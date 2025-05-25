using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public enum StepType { ShowText, WaitForInput, WaitForEvent }
    public StepType Type;
    public string Text;     
    public KeyCode Key;     
    public string EventName;
}