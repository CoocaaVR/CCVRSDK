using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SvrDebugHud : MonoBehaviour 
{
    public GameObject Head;

    public GameObject Orientation;
    public GameObject Position;

    private Text _orientationText;
    private Text _positionText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Quaternion orientation = Head.transform.localRotation;

        Vector3 position = Head.transform.localPosition;
		
        if (_orientationText != null)
        {
            _orientationText.text = string.Format("{0:F4}, {1:F4}, {2:F4}, {3:F4}", orientation.x, orientation.y, orientation.z, orientation.w);
        }

        if (_positionText != null)
        {
            _positionText.text = string.Format("{0:F4}, {1:F4}, {2:F4}", position.x, position.y, position.z);
        }
	}

    private void Awake()
    {
        _orientationText = Orientation.GetComponent<Text>();
        _positionText = Position.GetComponent<Text>();
    }
}
