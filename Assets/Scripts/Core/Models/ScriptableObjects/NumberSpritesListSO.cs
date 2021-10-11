using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Number Sprites List", menuName = "Create NumberSpritesList")]
public class NumberSpritesListSO : ScriptableObject
{
    public List<NumberSprite> list;
}
