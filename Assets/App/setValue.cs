using UnityEngine;
using UnityEngine.UI;

public class setValue : MonoBehaviour {

	public void setSec(float val){
		var myText  = GetComponentInChildren<Text>();
		myText.text = (int)val+"sec";
	}
}
