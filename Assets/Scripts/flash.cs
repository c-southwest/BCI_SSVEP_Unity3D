using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flash : MonoBehaviour
{
    Image img;
    public float frequency;
    GameObject mindObj;
    mind_script mind_scr;
    void Start()
    {

        mindObj = GameObject.Find("mind");
        mind_scr = mindObj.GetComponent<mind_script>();

        img = GetComponent<Image>();
        StartCoroutine(Flash());

    }

    void Update()
    {
        if (mind_scr.selectedFreq == frequency)
        {
            img.color = Color.red;

        }
        else
        {
            if(mind_scr.selectedFreq > 0)
            {
                img.color = Color.white;
            }
        }
    }

    IEnumerator Flash()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1 / (2 * frequency));
            //if (mind_scr.selectedFreq != frequency) img.color = (img.color == Color.red) ? Color.black : Color.red;
            if (mind_scr.selectedFreq != frequency) img.color = (img.color == Color.white) ? Color.black : Color.white;
        }
    }
}
