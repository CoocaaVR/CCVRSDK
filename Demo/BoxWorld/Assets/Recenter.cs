using UnityEngine;
using System.Collections;

public class Recenter : MonoBehaviour 
{
    public GameObject SvrManager;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SvrManager.GetComponent<SvrManager>().RecenterTracking();
        }
	}
}
