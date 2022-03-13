using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsDisplay : MonoBehaviour
{
    public Transform statusEffectUIParent;
    public GameObject statusEffectUIObject;

    private Dictionary<Effect, GameObject> effectUIMappings = new Dictionary<Effect, GameObject>();

    public void AddStatusEffectToDisplay(Effect effect)
    {
        GameObject statusEffectDisplayObject = Instantiate(statusEffectUIObject, statusEffectUIParent);
        statusEffectDisplayObject.GetComponent<StatusEffectUIChunk>().statusEffect = effect;
        effectUIMappings.Add(effect, statusEffectDisplayObject);
    }

    public void RemoveStatusEffectFromDisplay(Effect effect)
    {
        Destroy(effectUIMappings[effect]);
        effectUIMappings.Remove(effect);
    }
}
