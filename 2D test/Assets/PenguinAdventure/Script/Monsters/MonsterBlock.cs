using UnityEngine;
using System.Collections;
/// <summary>
/// 检测怪物是否被唤醒，使用地图中已预制的碰撞盒进行检测。
/// </summary>
public class MonsterBlock : MonoBehaviour {
	public GameObject BlockMonster;
	public GameObject BlockEyes;
	public bool isSurprise = false;
	public AudioClip soundShowUp;

	void Awake(){
		BlockMonster.SetActive (false);
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			if (isSurprise)
				SoundManager.PlaySfx (soundShowUp);
			BlockMonster.SetActive (true);
			BlockEyes.SetActive (false);
			GetComponent<BoxCollider2D> ().enabled = false;
			enabled = false;
		}
	}
}
