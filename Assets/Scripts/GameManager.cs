using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public List<GameObject> AllCardPrefabs;
    public List<GameObject> RandomizedCards;
    public List<GameObject> PlayerCardsOnDesk;
    public GameObject CardDeskPlayer1;
    public GameObject CardDeskPlayer2;
    public GameObject Player_1_CardContainer;
    public GameObject Player_2_CardContainer;
    public GameObject ShuffledCardAnim;
    public Text Player_1_Score;
    public Text Player_2_Score;
    public Text ActivityUpdateText;
    public int ListItemsCounts;

    [SerializeField]
    int _player_1_scoreValue = 0;
    [SerializeField]
    int _player_2_scoreValue = 0;
    [SerializeField]
    List<GameObject> _player_1_Cards;
    [SerializeField]
    List<GameObject> _player_2_Cards;
    [SerializeField]
    GameObject _player_1_cardOnDesk;
    [SerializeField]
    GameObject _player_2_cardOnDesk;

    // Start is called before the first frame update
    void Start()
    {
        ActivityUpdateText.gameObject.SetActive(false);
        ActivityUpdateText.transform.localScale = Vector3.zero;
        StartCoroutine(StartTheGame());
        
    }

    // Start Game Process
    IEnumerator StartTheGame()
    {
        ResatScores();
        ShuffleCardList(ListItemsCounts);
        yield return new WaitForSeconds(1);
        CardDistribution(ListItemsCounts);
        yield return new WaitForSeconds(2);
        StartCoroutine(CardMoveToPlayers());
    }

    // Resat to default the game
    public void ResatScores()
    {
        Player_1_Score.text = "Player 1 - 0";
        Player_2_Score.text = "Player 2 - 0";
    }

    // Randomize the card list
    public void ShuffleCardList(int ListItemsCount)
    {
        for (int i = 0; i < ListItemsCount; i++)
        {
            int RandNumber = Random.Range(0, AllCardPrefabs.Count);

            RandomizedCards.Add(AllCardPrefabs[RandNumber].gameObject);
            //print(AllCardPrefabs[RandNumber].gameObject);
            //print(" Count " + AllCardPrefabs.Count);
            AllCardPrefabs.RemoveAt(RandNumber);
        }
    }

    // Start Card distribution to both player.
    public void CardDistribution(int ListItemsCount)
    {
        //print("ListItemsCount " + ListItemsCount);
        for (int i = 0; i < ListItemsCount; i++)
        {
            //print("ListItemsCount inside loop " + ListItemsCount);
            if (i < 26)
                _player_1_Cards.Add(RandomizedCards[i].gameObject);
            else
                _player_2_Cards.Add(RandomizedCards[i].gameObject);

        }

    }


    IEnumerator CardMoveToPlayers()
    {
        // Loop throug the card available to distribute to each players
        for (int i = 0; i < 26; i++)
        {
            // Card distributes to player 1
            _player_1_Cards[i].GetComponent<RectTransform>().DOMove(Player_1_CardContainer.GetComponent<RectTransform>().position, .2f);
            _player_1_Cards[i].gameObject.tag = "Player1";
            yield return new WaitForSeconds(.2f);
            _player_1_Cards[i].transform.SetParent(Player_1_CardContainer.transform);
            ///////////////////////////////////

            // Card distributes to player 2
            _player_2_Cards[i].GetComponent<RectTransform>().DOMove(Player_2_CardContainer.GetComponent<RectTransform>().position, .2f);
            _player_2_Cards[i].gameObject.tag = "Player2";
            yield return new WaitForSeconds(.2f);
            _player_2_Cards[i].transform.SetParent(Player_2_CardContainer.transform);
            //////////////////////////////////
            
            if (i == 25)
            {
                ShuffledCardAnim.SetActive(false);
                ActivityUpdateText.text = "Click on card to start the Game";
                StartCoroutine(ShowActivityUpdate());
            }
                
        }
    }

    // Player turns his cards to desk.
    public void Player_Turn(GameObject ThrownCard)    
    {
        if (PlayerCardsOnDesk.Count == 2)
            return;

        if(ThrownCard.tag == "Player1")
        {
            // If Player1 already played their turn
            if (_player_1_cardOnDesk != null)
                return;

            // Player1 playes their turn
            ThrownCard.GetComponent<RectTransform>().SetParent(CardDeskPlayer1.transform.parent);
            ThrownCard.GetComponent<RectTransform>().localPosition = CardDeskPlayer1.transform.localPosition;
            ThrownCard.GetComponent<RectTransform>().localRotation = CardDeskPlayer1.transform.localRotation;
            _player_1_cardOnDesk = ThrownCard;
        }
        else
        {
            // If Player2 already played their turn
            if (_player_2_cardOnDesk != null)
                return;

            // Player2 playes their turn
            ThrownCard.GetComponent<RectTransform>().SetParent(CardDeskPlayer2.transform.parent);
            ThrownCard.GetComponent<RectTransform>().localPosition = CardDeskPlayer2.transform.localPosition;
            ThrownCard.GetComponent<RectTransform>().localRotation = CardDeskPlayer2.transform.localRotation;
            _player_2_cardOnDesk = ThrownCard;
        }

        PlayerCardsOnDesk.Add(ThrownCard);
        AllCardPrefabs.Add(ThrownCard);

        // Start Comparing card
        if (PlayerCardsOnDesk.Count == 2)
            StartCoroutine(CompareCards());
    }

    // comparing cards of both players.
    IEnumerator CompareCards()
    {
        yield return new WaitForSeconds(1);
        if (_player_1_cardOnDesk != null && _player_2_cardOnDesk != null)
        {
            // defined all cases
            // case 1
            if (_player_1_cardOnDesk.GetComponent<CardInformation>().CardValue > _player_2_cardOnDesk.GetComponent<CardInformation>().CardValue)
            {
                _player_1_scoreValue += 10;
                Player_1_Score.text = "Player 1 - " + _player_1_scoreValue;
                ActivityUpdateText.text = "Player 1 won 10 Points";
                StartCoroutine(ShowActivityUpdate());

            }
            // case 2
            else if (_player_1_cardOnDesk.GetComponent<CardInformation>().CardValue < _player_2_cardOnDesk.GetComponent<CardInformation>().CardValue)
            {
                _player_2_scoreValue += 10;
                Player_2_Score.text = "Player 2 - " + _player_2_scoreValue;
                ActivityUpdateText.text = "Player 2 won 10 Points";
                StartCoroutine(ShowActivityUpdate());
            }
            // case 3
            else if (_player_1_cardOnDesk.GetComponent<CardInformation>().CardValue == _player_2_cardOnDesk.GetComponent<CardInformation>().CardValue)
            {
               
                ActivityUpdateText.text = "Nobody won the Points";
                StartCoroutine( ShowActivityUpdate());
            }

            yield return new WaitForSeconds(3);

            _player_1_cardOnDesk = null;
            _player_2_cardOnDesk = null;
            PlayerCardsOnDesk[0].gameObject.SetActive(false);
            PlayerCardsOnDesk[1].gameObject.SetActive(false);
            PlayerCardsOnDesk.Clear();

        }

    }

    // Shows Players score update to the screen.
    IEnumerator ShowActivityUpdate()
    {
        ActivityUpdateText.gameObject.SetActive(true);
        ActivityUpdateText.transform.DOScale(Vector3.one, 1);
        yield return new WaitForSeconds(2);
        ActivityUpdateText.transform.DOScale(Vector3.zero, 1);

        if (AllCardPrefabs.Count == 52)
            StartCoroutine(GameFinishScreen());

    }

    // Game finish Process
    IEnumerator GameFinishScreen()
    {
        yield return new WaitForSeconds(2);

        // case 1
        if (_player_1_scoreValue > _player_2_scoreValue)
        {
            ActivityUpdateText.text = "Congratulation Player 1 Won";
            StartCoroutine(ShowActivityUpdate());
        }

        // case 2
        else if (_player_1_scoreValue < _player_2_scoreValue)
        {
            ActivityUpdateText.text = "Congratulation Player 2 Won";
            StartCoroutine(ShowActivityUpdate());
        }

        // case 3
        else if (_player_1_scoreValue == _player_2_scoreValue)
        {
            ActivityUpdateText.text = "Match Draw";
            StartCoroutine(ShowActivityUpdate());
        }
        yield return new WaitForSeconds(3);
        // Go to Menu Screen
        SceneManager.LoadScene(0);
    }

}
