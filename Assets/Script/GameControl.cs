using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

	//全局静态变量
	int PlayerCardZ = 1;

	//全局变量
	public int PlayerNum = 0,DeadNum;
	int[] Toggles = new int[20];
	public bool CanMove=true, CanClick;
	public string GameStage;
	public int Lover,MarkID;
	//预设体
	public GameObject PlayerCard, PlayerName,DeadMark;
	//全局对象
	public GameObject ConfigUI,PlayerUI,ChoosePlayer,ShowInfo,ShowPlayer,GoNext,PlayerNow,GameStatus;

	//局部变量
	GameObject[] Player = new GameObject[20],Lovers = new GameObject[2],Deaded= new GameObject[20];
	string[] PlayerRole = new string[20];
	string[] RoleList = {"狼人","狼人","狼人","狼人","先知","女巫","守卫","丘比特","猎人","长老","村民","村民","村民","村民","村民","村民","村民","村民"};
	GameObject jisha,shouhu,nvwu,dusha,toupiao,jingzhang;
	bool jieyaoyongle,duyaoyongle,shiyongjieyao,shaguozhanglao,diyiye,jingzhangsiwang,lierensiwang;
	string jieguo,time;
	int day;


	// Use this for initialization
	void Start () {
		//初始化游戏界面
		CanClick = true;
		ConfigUI.SetActive (false);
		PlayerUI.SetActive (false);
		ChoosePlayer.SetActive (false);
		ShowInfo.SetActive (false);
		ShowPlayer.SetActive (false);
		GameStage = "准备开始";
		GameObject.Find ("MainCanvas/NextStage").GetComponent<Button> ().interactable=false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)||Input.GetKeyDown (KeyCode.KeypadEnter)) {
			PlayerInfoExit ();
		}
	}
	//添加角色
	public void AddPlayer(){
		Vector3 Position = new Vector3(0, 0, PlayerCardZ);
		Vector3 offset = new Vector3(0, -0.5f, 0);
		Player [PlayerNum] = (GameObject)Instantiate (PlayerCard, Position, Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerID = 0;
		GameObject GN= (GameObject)Instantiate (PlayerName, Position, Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerName = GN;
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerMark = null;
		GN.transform.SetParent (GameObject.Find ("MainCanvas").transform);
		Position+=offset;
		GN.transform.Translate (Camera.main.WorldToScreenPoint (Position));
		PlayerNum++;
	}

	public void UpdatePlayerList(){
		int i, j;
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i] == null) {
				print ("null!");
				for (j = i; j < PlayerNum - 1; j++)
					Player [j] = Player [j + 1];
				PlayerNum--;
				for (j = 0; j < PlayerNum; j++)
					print (Player [j].GetComponent<PlayerCard> ().PlayerID);
			}
		}
	}
	//移动角色卡
	public void MoveCard(bool ison){
		CanMove = ison;
	}
	public void MarkCard(){
		int i;
		for (i = 0; i < PlayerNum; i++) {
			Destroy (Player [i].GetComponent<PlayerCard> ().PlayerMark);
			Player [i].GetComponent<PlayerCard> ().PlayerID = 0;
		}
		GameStage = "标记";
		GameStatus.GetComponent<Text>().text="请按顺序点击玩家为其编号";
		CanClick = true;
		MarkID = 1;
		GameObject.Find ("MainCanvas/Addplayer").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameConfigButton").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/Gamestart").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameStop").GetComponent<Button> ().interactable=false;


	}

	//配置
	public void GameConfig(){
		if (ConfigUI.activeSelf) {
			ConfigUI.SetActive (false);
			CanClick = true;
		} else {
			ConfigUI.SetActive (true);
			CanClick = false;
		}
	}

	public void GameCheck(){
		if (ShowPlayer.activeSelf) {
			ShowPlayer.SetActive (false);
		} else {
			ShowPlayer.SetActive (true);
			string tmp;
			int i;
			GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text = "";
			for (i = 0; i < PlayerNum; i++) {
				tmp = Player [i].GetComponent<PlayerCard> ().PlayerID.ToString() + "\n";
				GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text += tmp;
				tmp = Player [i].GetComponent<PlayerCard> ().Name + "\n";
				GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text += tmp;
				tmp = Player [i].GetComponent<PlayerCard> ().Role + "\n";
				GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text += tmp;
				if (Player [i].GetComponent<PlayerCard> ().IsAlive)
					GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text +="存活\n";
				else
					GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text +="死亡\n";
				if (Player [i] == jingzhang)
					GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "警长\n";
				else
					GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "\n";
				if (Player [i] == Lovers [0] || Player [i] == Lovers [1])
					GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "情侣\n";
				else
					GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "\n";
			}
		}
	}
	//玩家名字输入
	public void PlayerInputName(string Name){
		PlayerNow.GetComponent<PlayerCard>().InputName (Name);
	}
	//删除玩家
	public void PlayerDelete(){
		PlayerNow.GetComponent<PlayerCard>().Delete ();
		Invoke ("UpdatePlayerList", 0.5f);
	}

	//角色信息配置完成
	public void PlayerInfoExit(){
		PlayerUI.SetActive (false);
		CanClick = true;
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
		if (GameStage != "准备开始")
			return;
		if (PlayerNum < 1) {
			GameStatus.GetComponent<Text>().text="压根没玩家啊！";
			return;
		}
		if (Toggles [0] > PlayerNum) {
			GameStatus.GetComponent<Text>().text="角色数量大于玩家数量";
			return;
		}
		if (Toggles [0] < PlayerNum) {
			GameStatus.GetComponent<Text>().text="角色数量小于玩家数量";
			return;
		}
		ConfigUI.SetActive (false);
		PlayerUI.SetActive (false);
		GameStatus.GetComponent<Text>().text="游戏开始";
		//初始化游戏参数
		DeadNum =0;
		day = 0;
		Lover = 0;
		Lovers [0] = null;
		Lovers [1] = null;
		jisha = null;
		shouhu = null;
		nvwu = null;
		dusha = null;
		toupiao = null;
		jingzhang = null;
		jieyaoyongle = false;
		duyaoyongle = false;
		shiyongjieyao = false;
		shaguozhanglao = false;
		jingzhangsiwang = false;
		lierensiwang = false;
		time = "";
		diyiye = true;
		//根据配置获得角色列表
		int i, j;
		j = 0;
		for (i = 1; i <= 18; i++) {
			if (Toggles [i] == 1) {
				PlayerRole [j] = RoleList [i-1];
				j++;
			}
		}
		//随机给每个玩家分配角色
		int rand;
		for(i=0;i<PlayerNum;i++){
			rand = Random.Range (0, PlayerNum - i);
			Player[i].GetComponent<PlayerCard> ().Role = PlayerRole [rand];
			Player [i].GetComponent<PlayerCard> ().IsAlive = true;
			if (PlayerRole [rand] == "女巫")
				nvwu = Player [i];
			for(j=rand;j<PlayerNum-1;j++){
				PlayerRole[j]=PlayerRole[j+1];
			}
		}
		GameStatus.GetComponent<Text>().text="请点击自己的卡牌查看身份";
		GameStage = "查看身份";
		CanClick = true;
		GameObject.Find ("MainCanvas/Mark").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/Gamestart").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameExit").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/Addplayer").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameConfigButton").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/NextStage").GetComponent<Button> ().interactable=true;
		SortByRole ();
	}

	void SortByRole(){
		int i,j;
		GameObject tmp;
		j = 0;
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "狼人") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "女巫") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "守卫") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "先知") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "猎人") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "长老") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "丘比特") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
	}

	public void GameStop(){
		int i;
		GameStage = "准备开始";
		GameStatus.GetComponent<Text>().text="游戏已重置，请准备重新开始";
		for (i = 0; i < DeadNum; i++)
			Destroy (Deaded [i]);
		GameObject.Find ("MainCanvas/Mark").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/Gamestart").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/GameExit").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/Addplayer").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/GameConfigButton").GetComponent<Button> ().interactable=true;
		Start ();
	}

	public void GameExit(){
		Application.Quit ();
	}

	public void NextStage(){
		if(GameStage=="查看身份"){
			GameStatus.GetComponent<Text>().text="天黑请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 3.0f);
			return;
		}
		//情侣》狼人
		if (GameStage == "情侣") {
			GameStatus.GetComponent<Text> ().text = "情侣请闭眼";
			CanClick = false;
			ShowInfo.SetActive (false);
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		//狼人》女巫
		if (GameStage == "狼人") {
			jisha = null;
			GameStatus.GetComponent<Text>().text="狼人请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "女巫解药") {
			CanClick = false;
			Choose_NO ();
			return;
		}
		if (GameStage == "女巫毒药") {
			dusha = null;
			GameStatus.GetComponent<Text>().text="女巫请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "守卫") {
			shouhu = null;
			GameStatus.GetComponent<Text>().text="守卫请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "先知") {
			GameStatus.GetComponent<Text>().text="先知请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "警长") {
			GameStatus.GetComponent<Text>().text="警长平票，无警长";
			jingzhang = null;
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "夜晚结算") {
			CanClick = false;
			MoveOn ();
			return;
		}
		if (GameStage == "讨论") {
			MoveOn ();
			return;
		}
		if (GameStage == "选狼人") {
			toupiao = null;
			MoveOn ();
			return;
		}
		if (GameStage == "白天结算") {
			ShowInfo.SetActive (false);
			if (jingzhangsiwang||lierensiwang) {
				MoveOn ();
			} else {
				GameStatus.GetComponent<Text> ().text = "天黑请闭眼";
				GoNext.GetComponent<Button> ().interactable=false;
				Invoke ("MoveOn", 3.0f);
			}
				
			return;
		}
		if (GameStage == "转移警长") {
			GameStatus.GetComponent<Text> ().text = "警徽被撕，无警长";
			jingzhang = null;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 3.0f);
			return;
		}
		if (GameStage == "猎人") {
			GameStatus.GetComponent<Text> ().text = "猎人放弃";
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
	}

	void MoveOn(){
		GoNext.GetComponent<Button> ().interactable=true;
		if (GameStage == "查看身份") {
			Stage_qiubite ();
			return;
		}
		if (GameStage == "丘比特") {
			Stage_langren ();
			return;
		}
		if (GameStage == "情侣") {
			Stage_langren ();
			return;
		}
		if (GameStage == "狼人") {
			Stage_nvwu_jieyao ();
			return;
		}
		if (GameStage == "女巫解药") {
			Stage_nvwu_duyao ();
			return;
		}
		if (GameStage == "女巫毒药") {
			Stage_shouwei ();
			return;
		}
		if (GameStage == "守卫") {
			Stage_xianzhi ();
			return;
		}
		if (GameStage == "先知") {
			if (diyiye) {
				diyiye = false;
				Stage_jingzhang ();
			}
			else {
				yewanjiesuan ();
			}
			return;
		}
		if (GameStage == "警长") {
			yewanjiesuan ();
			return;
		}
		if (GameStage == "夜晚结算") {
			if (lierensiwang) {
				time = "晚上";
				lierendie ();
			} else {
				if (jingzhangsiwang) {
					time = "晚上";
					jingzhangdie ();

				} else {
					Stage_taolun ();
				}
			}
			return;
		}
		if (GameStage == "转移警长") {
			if (time == "晚上") {
				Stage_taolun ();
				return;
			}
			if (time == "白天") {
				GameStatus.GetComponent<Text>().text="天黑请闭眼";
				GoNext.GetComponent<Button> ().interactable=false;
				Invoke ("Stage_langren", 3.0f);
				return;
			}
		}
		if (GameStage == "猎人") {
			ShowInfo.SetActive (true);
			GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = jieguo;
			if (time == "晚上") {
				GameStage = "夜晚结算";
				return;
			}
			if (time == "白天") {
				GameStage = "白天结算";
				return;
			}
			return;
		}
		if (GameStage == "讨论") {
			Stage_xuanlangren ();
			return;
		}
		if (GameStage == "选狼人") {
			baitianjiesuan ();
			return;
		}
		if (GameStage == "白天结算") {
			if (lierensiwang) {
				time = "白天";
				lierendie ();
			} else {
				if (jingzhangsiwang) {
					time = "白天";
					jingzhangdie ();

				} else {
					Stage_langren ();
				}
			}
			return;
		}
	}

	public void Choose_YES(){
		ChoosePlayer.SetActive (false);
		CanClick = true;
		//选择情侣
		if (GameStage == "丘比特") {
			if (Lovers [0] == PlayerNow)
				GameStatus.GetComponent<Text> ().text = "恋人重复了！";
			else {
				Lovers [Lover] = PlayerNow;
				Lover++;
			}
			if (Lover > 1) {
				GameStatus.GetComponent<Text>().text="丘比特请闭眼";
				CanClick = false;
				GoNext.GetComponent<Button> ().interactable=false;
				Invoke ("ShowLovers", 2.0f);
			}
			return;
		}
		if (GameStage == "狼人") {
			jisha = PlayerNow;
			GameStatus.GetComponent<Text>().text="狼人请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);	
			return;	
		}
		if (GameStage == "女巫解药") {
			shiyongjieyao = true;
			jieyaoyongle = true;
			CanClick = false;
			MoveOn ();
			return;
		}
		if (GameStage == "女巫毒药") {
			dusha = PlayerNow;
			duyaoyongle = true;
			CanClick = false;
			MoveOn ();
			return;
		}
		if (GameStage == "守卫") {
			if (shouhu == PlayerNow)
				GameStatus.GetComponent<Text> ().text = "不能连续2晚守护同一个目标";
			else {
				CanClick = false;
				shouhu = PlayerNow;
				MoveOn ();
			}
			return;
		}
		if (GameStage == "先知") {
			GameStatus.GetComponent<Text> ().text = PlayerNow.GetComponent<PlayerCard> ().PlayerID.ToString () + PlayerNow.GetComponent<PlayerCard> ().Name+"的身份是"+PlayerNow.GetComponent<PlayerCard> ().Role+"\n先知请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 5.0f);
			return;
		}
		if (GameStage == "警长") {
			jingzhang = PlayerNow;
			GameStatus.GetComponent<Text> ().text = PlayerNow.GetComponent<PlayerCard> ().PlayerID.ToString () + PlayerNow.GetComponent<PlayerCard> ().Name+"当选警长";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "选狼人") {
			toupiao = PlayerNow;
			GameStatus.GetComponent<Text> ().text = PlayerNow.GetComponent<PlayerCard> ().PlayerID.ToString () + PlayerNow.GetComponent<PlayerCard> ().Name+"被处决";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 1.0f);
			return;
		}
		if (GameStage == "转移警长") {
			jingzhang = PlayerNow;
			GameStatus.GetComponent<Text> ().text = PlayerNow.GetComponent<PlayerCard> ().PlayerID.ToString () + PlayerNow.GetComponent<PlayerCard> ().Name+"成为警长";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "猎人") {
			die (PlayerNow, "枪杀");
			GameStatus.GetComponent<Text> ().text = PlayerNow.GetComponent<PlayerCard> ().PlayerID.ToString () + PlayerNow.GetComponent<PlayerCard> ().Name+"被枪杀";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
	}

	public void Choose_NO(){
		GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=true;
		ChoosePlayer.SetActive (false);
		CanClick = true;
		if (GameStage == "女巫解药") {
			MoveOn ();
		}
	}

	void Stage_qiubite(){
		GameStage = "丘比特";
		if (Toggles [8] == 0||diyiye==false) {
			MoveOn ();
			return;
		}
		GameStatus.GetComponent<Text>().text="请丘比特睁眼,选择情侣";
		CanClick = true;
	}

	void ShowLovers(){
		string name1, name2, role1, role2,ID1,ID2;
		GoNext.GetComponent<Button> ().interactable=true;
		GameStage = "情侣";
		if (Toggles [8] == 0||diyiye==false) {
			MoveOn ();
			return;
		}
		name1 = Lovers [0].GetComponent<PlayerCard> ().Name;
		name2 = Lovers [1].GetComponent<PlayerCard> ().Name;
		role1 = Lovers [0].GetComponent<PlayerCard> ().Role;
		role2 = Lovers [1].GetComponent<PlayerCard> ().Role;
		ID1 = Lovers [0].GetComponent<PlayerCard> ().PlayerID.ToString ();
		ID2 = Lovers [1].GetComponent<PlayerCard> ().PlayerID.ToString ();
		GameStatus.GetComponent<Text>().text="请情侣互看身份";
		ShowInfo.SetActive (true);
		GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = ID1 + name1 + "和" + ID2 + name2 + "连为了情侣\n" + ID1 + name1 + "的身份是" + role1 + "\n" + ID2 + name2 + "的身份是" + role2;
	}

	void Stage_langren(){
		GoNext.GetComponent<Button> ().interactable=true;
		GameStatus.GetComponent<Text>().text="请狼人睁眼并选择击杀目标\n如果平票直接点下一步";
		GameStage = "狼人";
		CanClick = true;
	}

	void Stage_nvwu_jieyao(){
		string text;
		GameStage = "女巫解药";
		if (Toggles [6] == 0) {
			MoveOn ();
			return;
		}
		text = "女巫请睁眼,";
		ChoosePlayer.SetActive (true);
		CanClick = false;
		//狼人平票
		if (jisha == null) {
			GameStatus.GetComponent<Text>().text="请女巫睁眼,狼人没有杀人,使用解药?";
			GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=false;
			return;
		}
		//女巫被杀
		if(jisha==nvwu){
			text+=jisha.GetComponent<PlayerCard> ().PlayerID.ToString()+jisha.GetComponent<PlayerCard> ().Name+"被杀了,不能自救";
			GameStatus.GetComponent<Text> ().text = text;
			GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=false;
			return;
		}
		//解药没了
		if (jieyaoyongle == true) {
			text+=jisha.GetComponent<PlayerCard> ().PlayerID.ToString()+jisha.GetComponent<PlayerCard> ().Name+"被杀了,解药没了";
			GameStatus.GetComponent<Text> ().text = text;
			GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=false;
			return;
		}
		text+=jisha.GetComponent<PlayerCard> ().PlayerID.ToString()+jisha.GetComponent<PlayerCard> ().Name+"被杀了,使用解药?";
		GameStatus.GetComponent<Text> ().text = text;
	}

	void Stage_nvwu_duyao(){
		GameStage = "女巫毒药";
		if (Toggles [6] == 0) {
			MoveOn ();
			return;
		}
		if (duyaoyongle == true) {
			GameStatus.GetComponent<Text> ().text = "是否使用毒药？毒药用过了，直接点下一步";
		} else {
			GameStatus.GetComponent<Text> ().text = "是否使用毒药？不使用就直接点下一步";
			CanClick = true;
		}
	}

	void Stage_shouwei(){
		GameStage = "守卫";
		if (Toggles [7] == 0) {
			MoveOn ();
			return;
		}
		GameStatus.GetComponent<Text> ().text = "守卫请睁眼，选择守护目标";
		CanClick = true;
	}

	void Stage_xianzhi(){
		GameStage = "先知";
		if (Toggles [5] == 0) {
			MoveOn ();
			return;
		}
		GameStatus.GetComponent<Text> ().text = "先知请睁眼，选择查验目标";
		CanClick = true;
	}

	void Stage_jingzhang(){
		GameStage = "警长";
		GameStatus.GetComponent<Text> ().text = "天亮了，请竞选警长";
		CanClick = true;
	}

	void yewanjiesuan(){
		GameStage = "夜晚结算";
		day++;
		jieguo = "";
		diyiye = false;
		if (dusha != null) {
			die (dusha, "毒杀");
			dusha = null;
		}
		if (jisha != null) {
			//有杀人
			if (shouhu != jisha) {
				//没守对
				if (shiyongjieyao) {
					//用解药了
					jieyaoyongle = true;
					shiyongjieyao = false;
				} else {
					//没用解药
					if (jisha.GetComponent<PlayerCard> ().Role == "长老" && shaguozhanglao == false) {
						//杀到长老第一条命
						shaguozhanglao = true;
					}
					else{
						die(jisha,"击杀");
					}
				}
			}
		}
		GameStatus.GetComponent<Text> ().text = "天亮了,昨晚结果为";
		ShowInfo.SetActive (true);
		if (jieguo == "")
			jieguo = "平安夜";
		GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = jieguo;
	}

	void jingzhangdie(){
		jingzhangsiwang = false;
		GameStage ="转移警长";
		ShowInfo.SetActive (false);
		GameStatus.GetComponent<Text> ().text = "警长死亡，请移交警徽";
		CanClick = true;
	}

	void lierendie(){
		lierensiwang = false;
		GameStage ="猎人";
		ShowInfo.SetActive (false);
		GameStatus.GetComponent<Text> ().text = "猎人死亡，选择枪杀对象";
		CanClick = true;
	}

	void Stage_taolun(){
		GameStage ="讨论";
		ShowInfo.SetActive (false);
		if (day % 2 == 1) {
			GameStatus.GetComponent<Text> ().text = "请从警长/死者右手开始发言";
		} else {
			GameStatus.GetComponent<Text> ().text = "请从警长/死者左手开始发言";
		}
	}

	void Stage_xuanlangren(){
		GameStage = "选狼人";
		GameStatus.GetComponent<Text> ().text = "票选狼人,2次平票直接点下一步";
		CanClick = true;
	}

	void baitianjiesuan(){
		GameStage = "白天结算";
		ShowInfo.SetActive (true);
		jieguo = "";
		if (toupiao == null) {
			jieguo = "平安日";
		} else {
			die(toupiao,"处决");
		}
		GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = jieguo;
	}

	void die(GameObject p,string way){
		if (p.GetComponent<PlayerCard> ().IsAlive == false)
			return;
		p.GetComponent<PlayerCard> ().IsAlive = false;
		if (jingzhang == p) {
			jieguo += "(警长)";
			jingzhangsiwang = true;
		}
		if (p.GetComponent<PlayerCard> ().Role == "猎人") {
			jieguo += "(猎人)";
			lierensiwang = true;
		}
		jieguo+=p.GetComponent<PlayerCard> ().PlayerID.ToString()+p.GetComponent<PlayerCard> ().Name+"死于"+way+"\n";
		Vector3 Position = p.GetComponent<Transform>().position;
		Deaded[DeadNum]= (GameObject)Instantiate (DeadMark, Position, Quaternion.identity);
		DeadNum++;
		if (Lovers [0] == p) {
			die (Lovers [1], "殉情");
		}
		if (Lovers [1] == p) {
			die (Lovers [0], "殉情");
		}
	}
}
