using UnityEngine;
using System.Collections;

public class player : Photon.MonoBehaviour
{
	private float initScaleX = 0;

	// Use this for initialization
	void Start () {
		initScaleX = transform.localScale.x;
	}

	void FixedUpdate() {

		// 自分以外は動かさない
		if(!photonView.isMine) return;

		float x = Input.GetAxisRaw("Horizontal") * 0.1f;
		if(x==0) return;

		// 移動する
		transform.Translate (x, 0,0);


		// 向きかえる
		Vector3 scale = transform.localScale;

		if(x>0){scale.x =  initScaleX;}
		else   {scale.x = -initScaleX;}	// 反転する（左向き）

		transform.localScale = scale;
	}
}
