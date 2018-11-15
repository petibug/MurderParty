using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

    public Image Player_icon;
    public Image Player_background;

    public Sprite Target_icon;
    public Sprite Dead_icon;
    public Sprite Removed_icon;
    public Sprite Victorious_icon;

    public Text NameText;
    public Text ScoreText;

    public Sprite SpriteDead;
    public Sprite SpriteRemoved;
    public Sprite SpriteWinner;
    public Sprite SpriteTarget;

    public Color TextNormalColor;
    public Color TextDeadColor;
    public Color TextRemovedColor;
    public Color TextWinnerColor;

    public Color BGNormalColor;
    public Color BGDeadColor;
    public Color BGRemovedColor;
    public Color BGWinnerColor;


    public enum IconType { target, dead, removed, victorious, nothing}

    public void Reset()
    {
        ScoreText.text = "0";
        SetIcon((int)IconType.nothing);
    }

    public void SetIcon(int type)
    {
        switch (type)
        {
            case (int)IconType.target:
                Player_icon.sprite = Target_icon;
                Player_icon.color = Color.white;
                Player_background.sprite = SpriteTarget;
                Player_background.color = BGNormalColor;
                NameText.color = TextNormalColor;
                ScoreText.color = TextNormalColor;

                // Debug.Log("icon target");
                break;

            case (int)IconType.dead:
                Player_icon.sprite = Dead_icon;
                Player_icon.color = Color.white;
                Player_background.sprite = SpriteDead;
                Player_background.color = BGDeadColor;
                NameText.color = TextDeadColor;
                ScoreText.color = TextDeadColor;
                // Debug.Log("icon dead");
                break;

            case (int)IconType.removed:
                Player_icon.sprite = Removed_icon;
                Player_icon.color = Color.white;
                Player_background.sprite = SpriteRemoved;
                Player_background.color = BGRemovedColor;
                NameText.color = TextRemovedColor;
                ScoreText.color = TextRemovedColor;
                // Debug.Log("icon removed");
                break;

            case (int)IconType.victorious:
                Player_icon.sprite = Victorious_icon;
                Player_icon.color = Color.white;
                Player_background.sprite = SpriteWinner;
                Player_background.color = BGWinnerColor;
                NameText.color = TextWinnerColor;
                ScoreText.color = TextWinnerColor;
                // Debug.Log("icon removed");
                break;

            case (int)IconType.nothing:
                Player_icon.sprite = null;
                Player_icon.color = Color.black;
                Player_background.sprite = null;
                Player_background.color = BGNormalColor;
                NameText.color = TextNormalColor;
                ScoreText.color = TextNormalColor;
                // Debug.Log("icon nothing");
                break;
        }
    }
}
