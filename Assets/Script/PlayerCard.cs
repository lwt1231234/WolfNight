using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCard : MonoBehaviour {

	GameObject GameControl;
	public GameObject PlayerName,PlayerMark;
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
		Vector3 MarkY = new Vector3(0, 0.5f, 0);
		Vector3 NameY = new Vector3(0, -0.5f, 0);
		//如果可以移动
		if (GameControl.GetComponent<GameControl> ().CanMove)
			while (Input.GetMouseButton (0)) {  

				Vector3 offset = Camera.main.ScreenToWorldPoint (Input.mousePosition) - OldMousePosition;
				OldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 NewPosition = transform.position + offset; 
				GetComponent<Rigidbody2D> ().MovePosition (NewPosition);
				PlayerName.GetComponent<Rigidbody2D> ().MovePosition (Camera.main.WorldToScreenPoint(NewPosition+NameY));
				if(PlayerMark!=null)
					PlayerMark.GetComponent<Rigidbody2D> ().MovePosition (Camera.main.WorldToScreenPoint(NewPosition+MarkY));
				yield return new WaitForFixedUpdate ();  
			}
		//如果处于非移动状态
		else {
			if (GameControl.GetComponent<GameControl> ().GameStage == "准备开始") {
				GameControl.GetComponent<GameControl> ().PlayerUI.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("PlayerInfo/InputField").GetComponent<InputField> ().text = Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "标记") {
				Destroy (PlayerMark);
				PlayerID = GameControl.GetComponent<GameControl> ().MarkID;
				PlayerMark = (GameObject)Instantiate (PlayerName, Camera.main.WorldToScreenPoint(transform.position+MarkY), Quaternion.identity);
				PlayerMark.transform.SetParent (GameObject.Find ("MainCanvas").transform);
				PlayerMark.GetComponent<Text>().text = PlayerID.ToString();
				GameControl.GetComponent<GameControl> ().MarkID++;
				if (GameControl.GetComponent<GameControl> ().MarkID > GameControl.GetComponent<GameControl> ().PlayerNum) {

					GameControl.GetComponent<GameControl> ().GameStage = "准备开始";
                    GameControl.GetComponent<GameControl>().CanMove = GameControl.GetComponent<GameControl>().CanMove_hode;
                    GameObject.Find ("MainCanvas/Addplayer").GetComponent<Button> ().interactable=true;
					GameObject.Find ("MainCanvas/GameConfigButton").GetComponent<Button> ().interactable=true;
					GameObject.Find ("MainCanvas/Gamestart").GetComponent<Button> ().interactable=true;
					GameObject.Find ("MainCanvas/GameStop").GetComponent<Button> ().interactable=true;
					GameControl.GetComponent<GameControl> ().GameStatus.GetComponent<Text>().text="准备开始游戏";
				}
					
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "查看身份") {
				while (Input.GetMouseButton (0))
				{
					GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text=PlayerID.ToString()+Name+"："+Role;
					yield return new WaitForFixedUpdate ();  
				}
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="请点击自己的卡牌查看身份";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "丘比特") {
				//弹出确认选项
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				if (GameControl.GetComponent<GameControl> ().Lover == 0)
					GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+PlayerID.ToString()+Name+"作为1号情侣";
				else
					GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+PlayerID.ToString()+Name+"作为2号情侣";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "狼人") {
				//弹出确认选项
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="选择"+PlayerID.ToString()+Name+"作为击杀目标";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "女巫毒药") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="对"+PlayerID.ToString()+Name+"使用毒药";
			}

			if (GameControl.GetComponent<GameControl> ().GameStage == "守卫") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="守护"+PlayerID.ToString()+Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "先知") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="查验"+PlayerID.ToString()+Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "警长") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text=PlayerID.ToString()+Name+"当选警长";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage == "选狼人") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text=PlayerID.ToString()+Name+"被投票处决";
			}
			if (GameControl.GetComponent<GameControl> ().GameStage =="转移警长") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="警长转移给"+PlayerID.ToString()+Name;
			}
			if (GameControl.GetComponent<GameControl> ().GameStage =="猎人") {
				GameControl.GetComponent<GameControl> ().ChoosePlayer.SetActive (true);
				GameControl.GetComponent<GameControl> ().CanClick = false;
				GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="枪杀"+PlayerID.ToString()+Name;
			}
		}   
    }  

	public void Delete(){
		Destroy (PlayerName);
		Destroy (PlayerMark);
		Destroy (this.gameObject);
	}

}
