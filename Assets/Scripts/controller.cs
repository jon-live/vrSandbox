using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<PhotonView>().isMine)
            transform.Translate(0.2f * Time.deltaTime, 0f, 0f);
	
	}
}
