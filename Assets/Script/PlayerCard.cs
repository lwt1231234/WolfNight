using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCard : MonoBehaviour {

	GameObject GameControl;
	public GameObject PlayerName;

	// Use this for initialization
	void Start () {
		//StartCoroutine(OnMouseDown());
		GameControl = GameObject.Find ("GameControl");
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void InputName(string Name){
		PlayerName.GetComponent<Text>().text=Name;
	}

	//值得注意的是世界坐标系转化为屏幕坐标系，Z轴是不变的  
    IEnumerator OnMouseDown()  
    {  
		Vector3 OldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

        //如果处于移动玩家状态
		if (GameControl.GetComponent<GameControl> ().CanMove)
	        //当鼠标左键按下时  
	        while (Input.GetMouseButton (0)) {  
				
				Vector3 offset = Camera.main.ScreenToWorldPoint (Input.mousePosition) - OldMousePosition;
				OldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 NewPosition = transform.position + offset; 
				GetComponent<Rigidbody2D> ().MovePosition (NewPosition);
				PlayerName.GetComponent<Rigidbody2D> ().MovePosition (Camera.main.WorldToScreenPoint (NewPosition));
				yield return new WaitForFixedUpdate ();  
			}
		//如果处于非移动状态
		else {
			GameControl.GetComponent<GameControl> ().PlayerUI.SetActive (true);
			GameControl.GetComponent<GameControl> ().PlayerNow = this.gameObject;
		} 
    }  

	public void Delete(){
		Destroy (PlayerName);
		Destroy (this.gameObject);
	}

}
