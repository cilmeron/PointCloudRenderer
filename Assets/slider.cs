using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class slider : MonoBehaviour
{
    public FileSelector a;
    public float minSize = 0.05f;
    public float maxSize = 1.0f;
    // Start is called before the first frame update
    public UnityEngine.UI.Slider slid;

    void Start()
    {
        slid = GetComponent<UnityEngine.UI.Slider>();
        slid.onValueChanged.AddListener(delegate { OnSliderValueChanged();});
        slid.minValue = minSize;
        slid.maxValue = maxSize;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSliderValueChanged()
    {
        a.sphereSize = slid.value;
        a.updatesize();
    }
}
