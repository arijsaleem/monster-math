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
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Expeimental.XR;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class CustomBehaviour : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    
    [HideInInspector]
    public Entry entry;

    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    [SerializeField]
    private TextMeshPro OverlayText;

    [SerializeField]
    //private string OverlayMathText;
    
    private int textOffset = 2;
    private GameObject childObj;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and names the object based on the result.
    /// </summary>

    // Use this for initialization
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        // Add RemoteTransformations script to object and set its entry
        //this.gameObject.AddComponent<RemoteTransformations>().entry = entry;

        // Qurey additional data to get the name
        // string value = "";
        // if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("name", out value))
        // {
        //     // Set name
        //     this.gameObject.name = value;
        // }

        ////////////////////////TEXT MESH CODE//////////////////////
        childObj = new GameObject();
        //Make block to be parent of this gameobject
        childObj.transform.parent = this.gameObject.transform;
        childObj.name = "Monster Math Text";

        

        //Create TextMesh and modify its properties
        OverlayText = childObj.AddComponent<TextMeshPro>();
        OverlayText.text = "Monster";
        OverlayText.fontSize = 1.5f;
        //childObj.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<Renderer>().bounds.center;
        

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid) {
            PlaceObject();
        }
        //Set postion of the TextMesh with offset
        //childObj.anchoredPosition = AnchorPosition.Center;
        //childObj.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
        OverlayText.alignment = TextAlignmentOptions.Center;
        OverlayText.transform.position = new Vector3(this.gameObject.transform.position.x,  this.gameObject.transform.position.y + textOffset, this.gameObject.transform.position.z);

    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
		{
            placementIndicator.SetActive(false);
		}
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}