using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DescriptionPopup : MonoBehaviour
{
    public static DescriptionPopup singleton;

    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;


    Camera cam;
    TooltipControl control;
    RectTransform rect;
    int UILayer;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        cam = Camera.main;
        control = GetComponent<TooltipControl>();
        TurnOff();
        UILayer = LayerMask.NameToLayer("UI");
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePosition);

        Vector2Int location = new Vector2Int((int) (worldPos.x + .5f), (int) (worldPos.y + .5f));

        if(IsPointerOverUIElement())
        {
            EquipmentGUIPanel panel = GetPanel(GetEventSystemRaycastResults());
            if (panel)
            {
                if (Player.player.equipment.equipmentSlots[panel.slot].active)
                {
                    //This is horrifying
                    Item item = Player.player.equipment.equipmentSlots[panel.slot].equipped.held[0];
                    title.text = item.GetName();
                    description.text = item.description;
                    TurnOn();
                    return;
                }
            }
            TurnOff();
            return;
        }
        
        if (Map.current)
        {
            if (location.x < 0 || location.y < 0 || location.x >= Map.current.width || location.y >= Map.current.width)
            {
                return;
            }
            CustomTile tile = Map.current.GetTile(location);

            if (tile.currentlyStanding)
            {
                title.text = tile.currentlyStanding.displayName;
                description.text = tile.currentlyStanding.description;
                TurnOn();
                return;
            }
            else
            {
                if (tile.inventory.Count > 0)
                {
                    Item item = tile.GetComponent<ItemVisiblity>().visible;
                    title.text = item.GetName();
                    description.text = item.description;
                    TurnOn();
                    return;
                }
            }
        }

        TurnOff();

    }

    public void TurnOn()
    {
        background.enabled = true;
        title.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        control.Resize();
        rect.ForceUpdateRectTransforms();
    }

    public void TurnOff()
    {
        background.enabled = false;
        title.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
    }


    //Stolen code from forum!
    //https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaycastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    private EquipmentGUIPanel GetPanel(List<RaycastResult> eventSystemRaycastResults)
    {
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaycastResults[index];
            Transform trans = curRaysastResult.gameObject.transform;
            while (trans.parent != null)
            {
                if (trans.tag.Equals("EquipBox"))
                {
                    return trans.GetComponent<EquipmentGUIPanel>();
                }
                trans = trans.parent;
            }
        }
        return null;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}
