using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconView : MonoBehaviour
{
    public Image icon;
    
    public Text value;
    
    public virtual void SetData(Resource resource)
    {
        icon.sprite = LoadResourceController.GetIconResource(resource.type, resource.id);
        icon.SetNativeSize();
        value.text = resource.number.ToString();
    }
}
