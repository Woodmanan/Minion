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

        val = Mathf.Clamp(val, 0, 1);


        if (mainBar.fillAmount > val)
        {
            mainBar.fillAmount = val;
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
        else
        {
            mainBar.fillAmount = val;//Mathf.Lerp(mainBar.fillAmount, val, 0.01f);
            if (sub)
            {
                sub.fillAmount = Mathf.Lerp(sub.fillAmount, mainBar.fillAmount, 0.01f);
            }
        }
    }
}
