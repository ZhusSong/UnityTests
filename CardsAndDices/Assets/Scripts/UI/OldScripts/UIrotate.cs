using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIrotate : MonoBehaviour {
    private GameObject ThisIM;
	// Use this for initialization
	void Start () {
        ThisIM = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        ThisIM.transform.DOLocalRotate(new Vector3(0,180,0),2f);
	}
}
