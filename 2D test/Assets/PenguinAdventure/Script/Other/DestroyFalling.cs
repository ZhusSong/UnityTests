using UnityEngine;
using System.Collections;
/// <summary>
/// 坠落碰撞体，位于场景下方，当接触到玩家时控制玩家死亡
/// </summary>
public class DestroyFalling : MonoBehaviour {
	public AudioClip soundWater;
	void OnTriggerEnter2D(Collider2D other){
		
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("GAMEOVER");
			SoundManager.PlaySfx (soundWater);
			GameManager.instance.GameOver ();
			other.gameObject.SetActive (false);
		} else
			Destroy (other.gameObject);
	}
}
