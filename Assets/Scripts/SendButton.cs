using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendButton : MonoBehaviour
{
    public Image CardImage;


    public void OnClick()
    {

        GameObject.FindGameObjectWithTag("ClientManager").GetComponent<ClientWork>().SendCards();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Queue<Sprite> DrawCard = new Queue<Sprite>();
    
    private float timeleft = 0.0f;
    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if(timeleft <= 0.0f)
        {
            if(DrawCard.Count != 0)
            {
                Sprite cardSprite = DrawCard.Dequeue();
                CardImage.sprite = cardSprite;

            }
            timeleft = 0.5f;

        }
    }
}
