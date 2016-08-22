using UnityEngine;
using System.Collections;

public class PlayerCard : MonoBehaviour {

	GameObject GameControl;

	// Use this for initialization
	void Start () {
		//StartCoroutine(OnMouseDown());
		GameControl = GameObject.Find ("GameControl");
	}

	// Update is called once per frame
	void Update () {
	
	}

	//值得注意的是世界坐标系转化为屏幕坐标系，Z轴是不变的  
    IEnumerator OnMouseDown()  
    {  
		Vector3 OldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

        
		if (GameControl.GetComponent<GameControl>().CanMove)
	        //当鼠标左键按下时  
	        while(Input.GetMouseButton(0))  
	        {  
				
				Vector3 offset = Camera.main.ScreenToWorldPoint (Input.mousePosition) - OldMousePosition;
				OldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 NewPosition = transform.position + offset; 
				GetComponent<Rigidbody2D> ().MovePosition (NewPosition);
	            yield return new WaitForFixedUpdate();  
	        }
          
    }  
}
