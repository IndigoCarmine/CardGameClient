using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    public int Id;

    //自身のImage
    public Image CardImage;

    // カードの設定
    public void Set(CardData data)
    {

        // IDを設定する
        this.Id = data.ImageID;

        // 表示する画像を設定する
        this.CardImage.sprite = data.ImageSprite;

    }
}

public class CardData
{
    public int ImageID { get; private set; }
    public Sprite ImageSprite;
    public CardData(int id, Sprite sprite)
    {
        ImageID = id;
        ImageSprite = sprite;
    }

}