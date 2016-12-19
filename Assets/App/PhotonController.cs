using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PhotonController: Photon.PunBehaviour {

    /*********************************************************************
     * Unity funcs
     *********************************************************************/
    void Start () {
		Debug.Log("Start");

		// シーン同期もする
		PhotonNetwork.automaticallySyncScene = true;

		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	public void OnGUI()
    {
        // ToDo 毎回更新は負荷が重いので、数秒おきの更新にしたい。


        // ルーム数とか
        this.drawStatistics();

        // 何か右端に出すおまじない
        GUILayout.BeginArea(new Rect(500, 0, Screen.width, Screen.height));
        
        // photon接続情報
        this.drawStatus();

        GUILayout.EndArea();
    }


    private void drawStatus()
    {
        GUILayout.Label("PHOTON STATUS: " + PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label(PhotonNetwork.lobby.ToString());

        if (PhotonNetwork.room != null){
            GUILayout.Label(PhotonNetwork.room.ToString());
        }else{
            GUILayout.Label("Room: Not in Room.");
        }
    }

    private void drawStatistics()
    {
        GUILayout.Label("STATISTICS");
        GUILayout.Label(this._loggingLobbyList());
        GUILayout.Label(this._loggingRoomList());
    }

/*********************************************************************
 * PUN
 *********************************************************************/
	// when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
	public override void OnConnectedToMaster(){
		Debug.Log("OnConnectedToMaster");
		PhotonNetwork.JoinLobby(getLobbyInfo());

		//static bool PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby); //コレ使えば楽なんじゃねー？
	}

	public override void OnJoinedLobby(){
		Debug.Log("OnJoinedLobby");
	}

	// ボタンアタッチとかで起動。ROOMに入る
	public void JoinRoom(){
		Debug.Log("tryJoinRoom");

		// 部屋に入ってる場合
		if (PhotonNetwork.room != null) {
			PhotonNetwork.LeaveRoom();
			return;
		}

		PhotonNetwork.JoinRandomRoom(getRoomProperties(), 0);
	}


    // ボタンアタッチとかで起動。ROOMを作る
    public void CreateRoom()
    {
        // 部屋に入ってる場合
        if (PhotonNetwork.room != null){
            Debug.Log("You're in room.");
            return;
        }

        Debug.Log("CreateRoom");
        PhotonNetwork.CreateRoom(null, getRoomOptions(), null);
    }


    public void OnPhotonRandomJoinFailed(){
		Debug.Log("OnPhotonRandomJoinFailed");

        this.CreateRoom();
	}
		
	public override void OnJoinedRoom(){
		Debug.Log("OnJoinedRoom");

		// Scene移動
		//SceneManager.LoadScene("Game");
	}

	private TypedLobby getLobbyInfo(){
        //string lobby_name = "Union0"+player.GetComponent<player>().union;
        string lobby_name = "Test_Lobby";
        return new TypedLobby(lobby_name, LobbyType.Default);
	}

	private Hashtable getRoomProperties(){
		return new Hashtable(){
	//		{"LvZone", (int)player.GetComponent<player>().level /10}
		};
	}

	private RoomOptions getRoomOptions(){
		RoomOptions option = new RoomOptions();
		option.IsVisible  = true;
        option.IsOpen = true;

        option.PlayerTtl    = 10 * 1000;   //millisec
        option.EmptyRoomTtl = 10 * 1000;   //millisec

        option.MaxPlayers = 3;

		return option;
	}





/*********************************************************************
 * for DEBUG
 *********************************************************************/
	private string _loggingLobbyList(){

        Debug.Log("Logging Lobby");

        var list = "===Lobbies: ";
		list += PhotonNetwork.LobbyStatistics.Count+"===\n";

		PhotonNetwork.LobbyStatistics.ForEach(q => list += q.ToString()+"\n");

        return list;
	}

	public string _loggingRoomList(){

        Debug.Log("Logging Room");

        var list = "===Rooms: ";

        if (!PhotonNetwork.insideLobby){
            list += "?===\n";
            list += "You're out of Lobby.(You can get Room info only in Lobby.)";
            return list;
		}

		var rooms = PhotonNetwork.GetRoomList();
		list += rooms.Length+"===\n";
		Array.ForEach(rooms, q => list += q.ToString()+"\n");

        return list;
	}
}
