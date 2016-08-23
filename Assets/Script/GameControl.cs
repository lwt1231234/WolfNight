using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

	//全局静态变量
	int PlayerCardZ = 1;

	//全局变量
	int PlayerNum = 0;
	int[] Toggles = new int[20];
	public bool CanMove = true;
	//预设体
	public GameObject PlayerCard, PlayerName;
	//全局对象
	public GameObject ConfigUI,PlayerUI,PlayerNow;

	//局部变量
	GameObject[] Player = new GameObject[20];
	string[] PlayerRole = new string[20];
	string[] RoleList = {"狼人"};

	// Use this for initialization
	void Start () {
		PlayerNum = 0;
		Toggles [0] = 0;
		ConfigUI.SetActive (false);
		PlayerUI.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//添加角色
	public void AddPlayer(){
		Vector3 Position = new Vector3(0, 0, PlayerCardZ);
		Player [PlayerNum] = (GameObject)Instantiate (PlayerCard, Position, Quaternion.identity);
		GameObject GN= (GameObject)Instantiate (PlayerName, Position, Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerName = GN;
		GN.transform.SetParent (GameObject.Find ("MainCanvas").transform);
		GN.transform.Translate (Camera.main.WorldToScreenPoint (Position));
		PlayerNum++;
	}
	//移动角色卡
	public void MoveCard(bool ison){
		CanMove = ison;
	}
	//配置
	public void GameConfig(){
		if(ConfigUI.activeSelf)
			ConfigUI.SetActive (false);
		else
			ConfigUI.SetActive (true);
	}
	//玩家名字输入
	public void PlayerInputName(string Name){
		PlayerNow.GetComponent<PlayerCard>().InputName (Name);
	}
	//删除玩家
	public void PlayerDelete(){
		PlayerNow.GetComponent<PlayerCard>().Delete ();
	}

	//角色信息配置完成
	public void PlayerInfoExit(){
		PlayerUI.SetActive (false);
	}

	//角色选择响应
	public void ToggleControl(int ID,bool ison){
		if (ison) {
			Toggles [0]++;
			Toggles [ID] = 1;
		} else {
			Toggles [0]--;
			Toggles [ID] = 0;
		}
	}
	//开始
	public void GameStart(){
		if (Toggles [0] > PlayerNum) {
			GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="角色数量大于玩家数量";
			return;
		}
		if (Toggles [0] < PlayerNum) {
			GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="角色数量小于玩家数量";
			return;
		}
		GameObject.Find ("MainCanvas/GameStatus").GetComponent<Text>().text="游戏开始";
		int i, j;
		j = 0;
		for (i = 1; i <= 18; i++) {
			if (Toggles [i] == 1) {
				PlayerRole [j] = RoleList [i];
				j++;
			}
				
		}
	}

}
