using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarBehaviour : MonoBehaviour
{

    private Slider s;

    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<Slider>();
    }

    public void setMaxValue(float val)
    {
        GetComponent<Slider>().maxValue = val;
        GetComponent<Slider>().value = val;
    }

    public void setCurValue(float val)
    {
        
        if(val > s.maxValue)
            s.value = s.maxValue;
        else
            s.value = val;
    }
}
