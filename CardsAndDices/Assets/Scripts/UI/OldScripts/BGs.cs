using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BGs : MonoBehaviour {
  
    public SpriteRenderer Bgs;
    public Sprite ThisBG;
    public Texture2D Tex;
	// Use this for initialization
    void Start()
    {
        Bgs =GetComponent<SpriteRenderer>();
        Tex = Resources.Load("redBG") as Texture2D;
        ThisBG = Sprite.Create(Tex, new Rect(1f,1f, Tex.width, Tex.height), new Vector2(0.5f, 0.5f));
        Bgs.sprite = ThisBG;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
