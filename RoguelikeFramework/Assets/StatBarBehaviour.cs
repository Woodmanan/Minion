using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarBehaviour : MonoBehaviour
{
    public Monster examined;
    public Resource resource;

    public Image sub;
    public Image mainBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (examined)
        {
            setCurValue(((float)examined.resources[resource]) / examined.stats.resources[resource]);
        }

    }

    public void setCurValue(float val)
    {

        if (val > 0)
        {
            mainBar.fillAmount = val;
        }
        else
        {
            mainBar.fillAmount = 0;
        }

        if (sub)
        {
            if (Mathf.Abs(sub.fillAmount - mainBar.fillAmount) > .007f && sub.fillAmount > mainBar.fillAmount)
            {
                sub.fillAmount = Mathf.Lerp(sub.fillAmount, mainBar.fillAmount, 0.01f);
            }
            else
            {
                sub.fillAmount = mainBar.fillAmount;
            }
        }
    }
}
