using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentGUIPanel : MonoBehaviour
{
    public int slot;
    Monster player;

    [SerializeField] private Image image;
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        yield return new WaitUntil(() => Player.player != null);
        player = Player.player;
        player.equipment.OnEquipmentAdded += UpdateView;
        UpdateView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        player.equipment.OnEquipmentAdded -= UpdateView;
    }

    public void UpdateView()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        EquipmentSlot equipSlot = player.equipment.equipmentSlots[slot];
        if (equipSlot.active)
        {
            image.enabled = true;
            SpriteRenderer render = equipSlot.equipped.held[0].GetComponent<SpriteRenderer>();
            image.sprite = render.sprite;
            image.color = render.color;
        }
        else
        {
            image.enabled = false;
        }

        text.text = equipSlot.slotName.ToUpper();
    }
}
