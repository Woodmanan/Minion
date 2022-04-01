using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New StatusListImageObject", menuName = "Status Effects/StatusListImageObject")]
public class StatusImageListObject : ScriptableObject
{
    public Sprite[] statusSprites;
}
