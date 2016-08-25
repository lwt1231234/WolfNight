using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCard : MonoBehaviour {

	GameObject GameControl;
	public GameObject PlayerName;
	public string Role;
	public int PlayerID;

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
		GameControl.GetComponent<GameControl> ().PlayerNow = this.gameObject;

		print (GameControl.GetComponent<GameControl> ().CanClick);
		if (GameControl.GetComponent<GameControl> ().CanClick == false)
			yield break;  

		//如果游戏处于准备阶段
		if (GameControl.GetComponent<GameControl> ().GameStage == "准备开始") {
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
				GameControl.GetComponent<GameControl> ().CanClick = false;
			} 
		}

		//如果游戏处于查看身份阶段
		if (GameControl.GetComponent<GameControl> ().GameStage == "查看身份") {
			while (Input.GetMouseButton (0))
			{
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text=PlayerName.GetComponent<Text>().text+"："+Role;
				yield return new WaitForFixedUpdate ();  
			}
			GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="请点击自己的卡牌查看身份";
		}

		//如果游戏处于丘比特阶段
		if (GameControl.GetComponent<GameControl> ().GameStage == "丘比特") {
			//弹出确认选项
			GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
			GameControl.GetComponent<GameControl> ().CanClick = false;
			if (GameControl.GetComponent<GameControl> ().Lover == 0)
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+PlayerName.GetComponent<Text>().text+"作为1号情侣";
			else
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+PlayerName.GetComponent<Text>().text+"作为2号情侣";
		}
        
    }  

	public void Delete(){
		Destroy (PlayerName);
		Destroy (this.gameObject);
	}

}
