using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Chapter")]
public class Chapter : ScriptableObject
{
    public string chapterName;
    public Dialogue[] dialogue;
}
