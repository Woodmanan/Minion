using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGUIController : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        yield return new WaitUntil(() => Player.player != null);
        foreach (EquipmentSlot s in Player.player.equipment.equipmentSlots)
        {
            GameObject spawned = Instantiate(prefab, holder.transform);
            spawned.GetComponent<EquipmentGUIPanel>().slot = s.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
