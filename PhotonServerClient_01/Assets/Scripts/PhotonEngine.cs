using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;//引入命名空间

public class PhotonEngine : MonoBehaviour,IPhotonPeerListener {

    private static PhotonEngine Instance;

    private static PhotonPeer peer = null;
    //客户端发消息给服务端 需要使用该属性
    public static PhotonPeer Peer {
        get {
            return peer;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //跳转到第二个场景时，第一个场景的PhotonEngine Instance，
        //需要将其删除掉，场景中只保留一个PhotonEngine Instance
        //删除多余的PhotonEngine Instance
        else if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

    }

    // Use this for initialization
    void Start () {
        //通过IPhotonPeerListener接收服务端反馈的结果
        //peer需要定义Listener和协议
        peer = new PhotonPeer(this,ConnectionProtocol.Udp);

        //传入服务端的IP地址和端口号以及AppicationName
        //要与PhotonServer.config，也就是服务端配置信息保持一致
        peer.Connect("127.0.0.1:5055", "MyGame1");//连接服务端
	}
	
	// Update is called once per frame
	void Update () {
        //if (peer.PeerState == PeerStateValue.Connected)
        //{
        //    peer.Service();//需要在Update里面一直调用该方法
        //}
        //不需要判断peer的状态
        peer.Service();//需要在Update里面一直调用该方法
    }

    private void OnDestroy()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();//断开连接
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    //客户端没有请求，服务端通知客户端时执行该函数
    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 1:
                Debug.Log("Receive server response by OnEvent! opCode : 1");
                //处理服务端传送过来的数据
                Dictionary<byte, object> data = eventData.Parameters;
                object intValue1, intValue2;
                data.TryGetValue(1, out intValue1);
                data.TryGetValue(2, out intValue2);
                Debug.Log(intValue1.ToString() + intValue2.ToString());
                break;
            default:
                break;
        }
    }

    //客户端向服务端发起请求，服务端相应并返回结果时执行该函数
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        //通过服务端的OperationResponse对象的OperationCode区分服务端的反馈
        switch (operationResponse.OperationCode)
        {
            case 1:
                Debug.Log("Receive server response! opCode : 1");
                //处理服务端传送过来的数据
                Dictionary<byte, object> data = operationResponse.Parameters;
                object intValue1, intValue2;
                data.TryGetValue(1,out intValue1);
                data.TryGetValue(2, out intValue2);
                Debug.Log(intValue1.ToString() + intValue2.ToString());
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    //peer有五种状态，一旦状态变化就会执行该函数
    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
    }
}
