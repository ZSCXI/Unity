using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            SendRequest();
        }
	}

    private void SendRequest()
    {
        //OpCustom(byte customOpCode, Dictionary<byte, object> customOpParameters, bool sendReliable);
        //customOpCode 为操作代码用于服务端区分这些请求 customOpParameters 传递给服务端的参数 sendReliable 是否建立可靠连接
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        //数据通过字典的方式传输 一般在项目里面key不会单纯的用int类型来表示而是通过枚举类型
        data.Add(1,100);
        data.Add(2,"qweq数");
        PhotonEngine.Peer.OpCustom(1,data,true);//发消息给客户端
       
}
}
