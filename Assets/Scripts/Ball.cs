using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball: MonoBehaviour {
    public GameObject[] stars;
    public GameObject gameManager;
    public Vector3 initPosition = new Vector3(0.585f, 1.248f, -1.326f);
    private int starsCollected;
    private int totalStarsCount;
	// Use this for initialization
	void Start () {
        starsCollected = 0;
        totalStarsCount = stars.Length;
        MeshRenderer m = GetComponent<MeshRenderer>();
        Material curM = m.material;
        curM.SetColor("_Color", Color.blue);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.tag == "ground")
        {
            Rigidbody rgbdy = this.GetComponent<Rigidbody>();
            this.transform.position = initPosition;
            rgbdy.velocity = Vector3.zero;
            rgbdy.angularVelocity = Vector3.zero;
            rgbdy.isKinematic = false;
            rgbdy.useGravity = true;

            for (int i = 0; i < stars.Length; i += 1) 
            {
                stars[i].SetActive(true);
            }

            starsCollected = 0;
        }
        else if (col.collider.gameObject.tag == "trampoline")
        {
            Rigidbody rgbdy = this.GetComponent<Rigidbody>();
            rgbdy.velocity = new Vector3(rgbdy.velocity.x, 5, rgbdy.velocity.z);
        }
        else if (col.collider.gameObject.tag == "teleporter")
        {
            GameObject teleporter = col.collider.gameObject;
            TeleportLink teleportScript = teleporter.GetComponent<TeleportLink>();
            GameObject targetLocation = teleportScript.targetLocation;
            this.transform.position = new Vector3(targetLocation.transform.position.x, targetLocation.transform.position.y - 0.3f, targetLocation.transform.position.z);
            Rigidbody rgbdy = this.GetComponent<Rigidbody>();
            rgbdy.velocity = Vector3.zero;
            rgbdy.angularVelocity = Vector3.zero;
        }
        else if (col.collider.gameObject.tag == "star")
        {
            MeshRenderer m = GetComponent<MeshRenderer>();
            Color curMColor = m.material.color;
            
            if (curMColor != Color.red) {
                starsCollected += 1;
                col.collider.gameObject.SetActive(false);
                if (starsCollected == stars.Length)
                {
                    Debug.Log("player has won");
                    GetActiveScene managerScript = gameManager.GetComponent<GetActiveScene>();
                    managerScript.levelWon = true;
                }
            }

        }
    }
}
// TODO need to make stars dispear on contact and reinitate it