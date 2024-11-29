using UnityEngine;

[System.Serializable]
public class LinButton
{
    public KeyButton key;
    public enum KeyButton { Y, H, B }

    [Space(5)]
    public float waitPassage = 0.4f;
    public float speedPassage = 1f;

    [Space(5)]
    public Pos pos;
    public enum Pos { L_Bottom, L_Center, L_Top, C_Bottom, C_Center, C_Top, R_Bottom, R_Center, R_Top }
}