/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Entry entry;

    [SerializeField]
    private TextMeshPro OverlayText;

    [SerializeField]
    private string OverlayMathText;
    
    private int textOffset = 2;
    private GameObject childObj;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and names the object based on the result.
    /// </summary>

    // Use this for initialization
    void Start()
    {
        // Add RemoteTransformations script to object and set its entry
        this.gameObject.AddComponent<RemoteTransformations>().entry = entry;

        // Qurey additional data to get the name
        string value = "";
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("name", out value))
        {
            // Set name
            this.gameObject.name = value;
        }

        ////////////////////////TEXT MESH CODE//////////////////////
        childObj = new GameObject();
        //Make block to be parent of this gameobject
        childObj.transform.parent = this.gameObject.transform;
        childObj.name = "Monster Math Text";

        

        //Create TextMesh and modify its properties
        OverlayText = childObj.AddComponent<TextMeshPro>();
        OverlayText.text = "Monster";
        OverlayText.fontSize = 1.5f;
        childObj.GetComponent<RectTransform>().anchoredPosition = this.gameObject.transform.renderer.bounds.center;
        

    }

    // Update is called once per frame
    void Update()
    {
        //Set postion of the TextMesh with offset
        //childObj.anchoredPosition = AnchorPosition.Center;
        //childObj.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
        OverlayText.alignment = TextAlignmentOptions.Center;
        OverlayText.transform.position = new Vector3(this.gameObject.transform.position.x,  this.gameObject.transform.position.y + textOffset, this.gameObject.transform.position.z);

    }
}