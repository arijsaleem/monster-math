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

    GameObject OptionText;
    TextMesh OptionTextMesh;

    public GameObject bullet;

    public static int z_distance;
    int respawn = 0;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and names the object based on the result.
    /// </summary>

    // Use this for initialization
    void Start()
    {

        int num1 = (int) (Random.Range(0.0f, 12.0f));
        int num2 = (int) (Random.Range(0.0f, 12.0f));
        int answer = num1 * num2;
        int option1 = answer;
        int option2 = (int) (Random.Range(0.0f, 144.0f));
        int option3 = (int) (Random.Range(0.0f, 144.0f));

        // Add RemoteTransformations script to object and set its entry
        this.gameObject.AddComponent<RemoteTransformations>().entry = entry;

        ////////////////////////TEXT MESH CODE//////////////////////
        childObj = new GameObject();
        //Make block to be parent of this gameobject
        childObj.transform.parent = this.gameObject.transform;
        childObj.name = "Monster Math Text";

        //Create TextMesh and modify its properties
        OverlayText = childObj.AddComponent<TextMeshPro>();
        OverlayText.text = num1 + " x " + num2 + " = ?";
        OverlayText.fontSize = 1.5f;

        // Qurey additional data to get the name
        string value = "";
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("name", out value))
        {
            // Set name
            this.gameObject.name = value;
        }

        BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {

        //Text stuff
        OverlayText.alignment = TextAlignmentOptions.Center;
        OverlayText.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + textOffset, this.gameObject.transform.position.z);
       
        //Monster Generation stuff
        respawn = respawn + 1;
        if (respawn == 100 ) {
            Vector3 position = new Vector3(Random.Range(-300.0f, 300.0f), 0, 200.0f);
            Instantiate(this.gameObject, position, Quaternion.identity);
            Debug.Log(this.gameObject.name + " has spawned");
        }

        //Selection stuff
        if (Input.touchCount > 0)
        {
            //get the touch input and check if the projected ray hits a game piece
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            //ray from camera to check if it hit an object
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //if the ray hit a game object and it is this object
                if (hit.collider != null & hit.collider.gameObject != null & hit.collider.gameObject == this.gameObject)
                {
                    // print collided object
                    // Debug.Log(hit.collider.gameObject.name);
                    //handle the touch of the game piece
                    //handleTouch(hit.collider.gameObject);

                    // Shoots a bullet
                    Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
                    bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Rigidbody rigidBody = bullet.AddComponent<Rigidbody>();
                    rigidBody.useGravity = false;
                    rigidBody.AddForce(renderer.bounds.center * 300f);

                    // Remove GameObject and from list
                    Destroy(hit.transform.gameObject); // removes

                }
            }
        }

    }

    void handleTouch(GameObject g)
    {

       


    }
   



}
