using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ClientWorkTest : MonoBehaviour
{

    public Card CardPrefab;
    RectTransform HandField;
    RectTransform SelectedField;
    private Vector2 MousePosition;
    private RaycastHit2D hit2d;
    private Vector2 ObjectPosition;
    public bool ObjectFlag;

    // カード情報リスト
    List<CardData> cardDataList = new List<CardData>();
    // 表示するカード画像情報のリスト
    List<Sprite> imgList = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {

        HandField = GameObject.FindGameObjectWithTag("HandField").GetComponent<RectTransform>();
        SelectedField = GameObject.FindGameObjectWithTag("SelectedField").GetComponent<RectTransform>();


        // Resources/Imageフォルダ内にある画像を取得する
        imgList.Add(Resources.Load<Sprite>("Image/1"));
        imgList.Add(Resources.Load<Sprite>("Image/2")); 
        imgList.Add(Resources.Load<Sprite>("Image/3"));
        for(int i = 0; i < imgList.Count; i++)
        {
            cardDataList.Add(new CardData(i, imgList[i]));
        }

        foreach (CardData _cardData in cardDataList)
        {

            // Instantiate で Cardオブジェクトを生成
            Card card = Instantiate<Card>(CardPrefab, HandField);
            // データを設定する
            card.Set(_cardData);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            LayoutRebuilder.MarkLayoutForRebuild(HandField);
            LayoutRebuilder.MarkLayoutForRebuild(SelectedField);
        }
    }
}
