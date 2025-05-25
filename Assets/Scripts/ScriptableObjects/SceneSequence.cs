using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SceneSequence", fileName = "NewSceneSequence")]
public class SceneSequence : ScriptableObject
{
    [Tooltip("Orden de escenas: ")]
    public string[] sceneNames;
}