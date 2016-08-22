using UnityEngine;
using System.Collections;

public class RoleToggle : MonoBehaviour {

	public int ID;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Clicked(bool ison){
		GameObject.Find ("GameControl").GetComponent<GameControl>().ToggleControl (ID, ison);
	}
}
