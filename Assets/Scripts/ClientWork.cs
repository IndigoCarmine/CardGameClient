using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ClientWork : MonoBehaviour
{

    public Card CardPrefab;
    RectTransform HandField;
    RectTransform SelectedField;
    RectTransform OpponentHandField;
    NetWorkManager netWorkManager;
    SendButton sendButton;
    // カード情報リスト
    List<CardData> cardDataList = new List<CardData>();
    // 表示するカード画像情報のリスト
    List<Sprite> imgList = new List<Sprite>();
    Text OpponentName;

    // Start is called before the first frame update
    void Start()
    {

        HandField = GameObject.FindGameObjectWithTag("HandField").GetComponent<RectTransform>();
        SelectedField = GameObject.FindGameObjectWithTag("SelectedField").GetComponent<RectTransform>();
        netWorkManager = GameObject.FindGameObjectWithTag("NetWorkManager").GetComponent<NetWorkManager>();
        OpponentHandField = GameObject.FindGameObjectWithTag("OpponentField").GetComponent<RectTransform>();
        sendButton = GameObject.FindGameObjectWithTag("DeckButton").GetComponent<SendButton>();
        OpponentName = GameObject.FindGameObjectWithTag("OpponentName").GetComponent<Text>();
        // Resources/Imageフォルダ内にある画像を取得する
        imgList.Add(Resources.Load<Sprite>("Image/1"));
        imgList.Add(Resources.Load<Sprite>("Image/2"));
        imgList.Add(Resources.Load<Sprite>("Image/3"));
        for (int i = 0; i < imgList.Count; i++)
        {
            cardDataList.Add(new CardData(i, imgList[i]));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Feildのカード再配置
            LayoutRebuilder.MarkLayoutForRebuild(HandField);
            LayoutRebuilder.MarkLayoutForRebuild(SelectedField);
        }
    }

    /// <summary>
    /// フィールドにカードインスタンスを追加する。
    /// </summary>
    /// <param name="index">CardID</param>
    /// <param name="Field">対象のフィールド</param>
    void AddCardInstance(int index, RectTransform Field)
    {
        // Instantiate で Cardオブジェクトを生成
        Card card = Instantiate<Card>(CardPrefab, Field);
        // データを設定する
        card.Set(cardDataList[index]);
    }

    /// <summary>
    /// フィルド内のすべてカードインスタンスを消去する。
    /// </summary>
    /// <param name="Field"></param>
    void FieldClear(RectTransform Field)
    {
        foreach (Transform childtransform in Field.transform)
        {
            Destroy(childtransform.gameObject);
        }
    }

    /// <summary>
    /// SelectedField上のカードを送信する。
    /// </summary>
    public void SendCards()
    {
        List<int> IDList = new List<int>();
        foreach(Transform childtransform in SelectedField.transform)
        {
            IDList.Add(childtransform.gameObject.GetComponent<Card>().Id);
        }

        //送信成功
        bool IsSuccessed = netWorkManager.Request(NetWorkManager.RequestParameter.Select, string.Join(",", IDList));
        if (IsSuccessed) FieldClear(SelectedField);
    }





    /// <summary>
    /// ドローの要求用
    /// </summary>
    public void Draw()
    {
        Draw(1);
    }
    /// <summary>
    /// ドローの要求用
    /// </summary>
    /// <param name="CardCount">ほしい枚数</param>
    public void Draw(int CardCount)
    {
        netWorkManager.Request(NetWorkManager.RequestParameter.Draw, CardCount.ToString());
    }

    /// <summary>
    /// Uno宣言
    /// </summary>
    public void Uno()
    {
        netWorkManager.Request(NetWorkManager.RequestParameter.Uno, "0");
    }

    /// <summary>
    /// NetWorkManager以外からこの関数を呼ぶ必要はない。
    /// </summary>
    /// <param name="Msg">受け取った内容</param>
    public void Work(string Msg)
    {
        Debug.Log(Msg);

        //TCPで受け取った文字列をもとに処理をする。
        string[] MsgSplit =  Msg.Split('=');
        string Parameter = MsgSplit[0];
        string Content = MsgSplit[1];

        switch (Parameter)
        {
            case "OpponentName":
                OpponentName.text = Content;
                break;
            case "Hand":
                FieldClear(HandField);
                foreach(string CardID in Content.Split(','))
                {
                    AddCardInstance(int.Parse(CardID), HandField);
                }
                break;
            case "OpponentHand":
                FieldClear(OpponentHandField);
                foreach (string CardID in Content.Split(','))
                {
                    AddCardInstance(int.Parse(CardID), OpponentHandField);
                }
                break;
            case "Turn":
                break;
            case "Error":
                switch (Content)
                {
                    case "OverCapacity":
                        Debug.Log("すでに２人が接続済みです。");
                        netWorkManager.Disconnect();
                        break;
                    case "CardDoesntExist":
                        Debug.Log("選択されたカードがサーバ上のリストに存在しません。");
                        //手札データの再取得
                        netWorkManager.Request(NetWorkManager.RequestParameter.DataRequest, "Hand");
                        break;
                    case "OpponentDisconnect":
                        Debug.Log("相手の通信が切断されました。");
                        break;
                    case "UnExpectedError":
                        Debug.LogError(Content);
                        break;
                    default:
                        Debug.LogError(Content);
                        break;
                }
                break;
            case "Finish":
                break;
            case "Start":
                break;
            case "DrawPile":
                foreach (string CardID in Content.Split(','))
                {
                    sendButton.DrawCard.Enqueue(imgList[int.Parse(CardID)]);
                }
                break;
            default:
                Debug.LogError("予期しないパラメータを受信しました。");
                break;
        }
        

        
    }
}
