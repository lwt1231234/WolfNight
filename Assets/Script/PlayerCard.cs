using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCard : MonoBehaviour {

	GameObject GameControl;
	public GameObject PlayerName;
	public string Role,Name;
	public int PlayerID;
	public bool IsAlive;

	// Use this for initialization
	void Start () {
		//StartCoroutine(OnMouseDown());
		GameControl = GameObject.Find ("GameControl");
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void InputName(string str_name){
		PlayerName.GetComponent<Text>().text=str_name;
		Name = str_name;
	}

	//值得注意的是世界坐标系转化为屏幕坐标系，Z轴是不变的  
    IEnumerator OnMouseDown()  
    {  
		//print (GameControl.GetComponent<GameControl> ().CanClick);
		if (GameControl.GetComponent<GameControl> ().CanClick == false)
			yield break;  
		GameControl.GetComponent<GameControl> ().PlayerNow = this.gameObject;
		Vector3 OldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//如果可以移动
		if (GameControl.GetComponent<GameControl> ().CanMove)
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
			if (GameControl.GetComponent<GameControl> ().GameStage == "准备开始") {
				GameControl.GetComponent<GameControl> ().PlayerUI.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("PlayerInfo/InputField").GetComponent<InputField> ().text = Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "查看身份") {
				while (Input.GetMouseButton (0))
				{
					GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text=Name+"："+Role;
					yield return new WaitForFixedUpdate ();  
				}
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="请点击自己的卡牌查看身份";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "丘比特") {
				//弹出确认选项
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				if (GameControl.GetComponent<GameControl> ().Lover == 0)
					GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+Name+"作为1号情侣";
				else
					GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+Name+"作为2号情侣";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "狼人") {
				//弹出确认选项
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+Name+"作为击杀目标";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "女巫毒药") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="对"+Name+"使用毒药";
			}

			if (GameControl.GetComponent<GameControl> ().GameStage == "守卫") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="守护"+Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "先知") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="查验"+Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "警长") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text=Name+"当选警长";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "选狼人") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text=Name+"被投票处决";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage =="转移警长") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="警长转移给"+Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage =="猎人") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="枪杀"+Name;
			}
		}   
    }  

	public void Delete(){
		Destroy (PlayerName);
		Destroy (this.gameObject);
	}

}
