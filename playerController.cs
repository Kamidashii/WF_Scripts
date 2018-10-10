using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{

    //переменная для установки макс. скорости персонажа
    public float maxSpeed = 10f;
    //переменная для определения направления персонажа вправо/влево
    private bool isFacingRight = true;
    //ссылка на компонент анимаций
    private Animator anim;

    public float health;
    public Slider healthSlider;

    public GameObject damageLeft;
    public GameObject damageRight;

    public string spaceProblemText;
    public string shiftProblemText;

    public GameControllerScript gameController;

    public bool key = false;

    public Text scoreText; //ui текст для вывода счета
    public Text dialogText; //ui текст для вывода диалогов
    public Text hintText; //ui текст для подсказок

    private string currentText;
    private string currentHint;

    public float fireRate;
    public float reloading;

    public string weaponTag;
    public string emptyTag;

    public bool dying = false;

    private Dealer_Script dealer = null;
    private bool nearGemsObject = false; //есть ли рядом активный объект типа gems 
    private bool nearDangerObject = false; //есть ли рядом активный объект типа danger
    private ActiveObject activeObject = null; //ссылка на активный объект рядом
    private DangerObjectScript dangerObject = null; //ссылка на активный объект наносящий урон рядом
    public Coord_Script coordObject = null;
    public Inventory inventory;

    private float move;

    private void ResetTexts()
    {
        dialogText.text = "";
        hintText.text = "";
    }
    
    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Door")
        {
            coordObject = collider2D.gameObject.GetComponent<Coord_Script>();
        }

        else if (collider2D.tag == "Bullet")
        {
            health -= collider2D.gameObject.GetComponent<DangerObjectScript>().damage;
            Destroy(collider2D.gameObject);
        }
        else if (collider2D.tag == "Danger")
        {
            nearDangerObject = true;
            dangerObject = collider2D.gameObject.GetComponent<DangerObjectScript>();
        }

        else if ((collider2D.tag == "GemsObject") || (collider2D.tag == "Key") || (collider2D.tag == "Book"))
        {
            activeObject = collider2D.GetComponent<ActiveObject>();
            if (activeObject != null)
            {
                nearGemsObject = true;
            }
        }
        else if (collider2D.tag == "Dealer")
        {
            dealer = collider2D.gameObject.GetComponent<Dealer_Script>();
        }
    }


    public IEnumerator WaitAndResetTexts()
    {
        yield return new WaitForSeconds(5);
        ResetTexts();
    }
    

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Danger")
        {
            nearDangerObject = false;
            dangerObject = null;
        }

        else if ((collider2D.tag == "GemsObject") || (collider2D.tag == "Key") || (collider2D.tag == "Book"))
        {
            if (collider2D.tag == "Key" && key)
            {
                StartCoroutine(WaitAndResetTexts());
            }
            else
            {
                ResetTexts();
                dialogText.GetComponent<DialogTextScript>().paper.SetActive(false);
            }
            nearGemsObject = false;
            activeObject = null;
        }

        else if (collider2D.tag == "Dealer")
        {
            dealer = null;
        }

        else if (collider2D.tag == "Door")
        {
            coordObject = null;
            ResetTexts();
        }
    }


    private void Start()
    {
        damageRight.GetComponent<Collider2D>().tag = emptyTag;
        damageLeft.GetComponent<Collider2D>().tag = emptyTag;
        anim = GetComponent<Animator>();
    }

    public void DamageActive()
    {
        damageLeft.SetActive(true);
        damageRight.SetActive(true);
    }

    public void DamageDeactive()
    {
        damageLeft.SetActive(false);
        damageRight.SetActive(false);
    }


    void Move()
    {
        move = Input.GetAxis("Horizontal");

        if (move > 0 && !isFacingRight)
            Flip();
        else if (move < 0 && isFacingRight)
            Flip();

        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("isRight", isFacingRight);

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    void CheckReloading()
    {
        if (reloading > 0)
        {
            reloading -= Time.deltaTime;
        }
    }
    void DoHit()
    {
        if (reloading <= 0)
        {
            if (isFacingRight)
            {
                damageRight.GetComponent<Collider2D>().tag = weaponTag;
            }
            else
            {
                damageLeft.GetComponent<Collider2D>().tag = weaponTag;
            }
            reloading = fireRate;
        }
    }

    private void FixedUpdate()
    {
        Move();

        CheckReloading();

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.K))
        {
            DoHit();
        }

    }

    public void StopAnim()
    {
        anim.SetFloat("Speed", 0);
        anim.SetBool("isRight", isFacingRight);
    }

    private IEnumerator KeyMessage(string text)
    {
        hintText.text = text;
        yield return new WaitForSeconds(4);
        if (hintText.text == text)
        {
            hintText.text = "";
        }
    }

    void Die()
    {
        StartCoroutine(gameController.Respawn());
        dying = true;
    }

    void GetHit()
    {
        if (dangerObject.reload <= 0)
        {
            health -= dangerObject.damage;
            dangerObject.reload = dangerObject.fireRate;
        }
        else
        {
            dangerObject.reload -= Time.deltaTime;
        }
    }

    void PressKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(KeyMessage(spaceProblemText));
        }

        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(KeyMessage(shiftProblemText));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            E_Interact();
        }

        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            CloseBooks();
        }
    }

    private void Update()
    {

        healthSlider.value = health;

        if (health <= 0 && !dying)
        {
            Die();
        }

        if (nearDangerObject)
        {
            GetHit();
        }

        PressKey();
    }

    void CloseBooks()
    {
        dialogText.GetComponent<DialogTextScript>().paper.SetActive(false);
        ResetTexts();
    }

    void KeyPick()
    {
        key = true;
        gameController.key = key;
        dialogText.GetComponent<Text>().text = activeObject.usingText;
        currentText = dialogText.GetComponent<Text>().text;
        if (activeObject.data.count == 0)
        {
            hintText.text = activeObject.hintText;
            currentHint = hintText.text;
            activeObject.Take();
        }
        else
        {
            if (currentHint == activeObject.hintText)
            {
                dialogText.GetComponent<Text>().text = activeObject.usingText;
                hintText.text = "";
                Destroy(activeObject.gameObject);
                activeObject = null;
                nearGemsObject = false;
            }
        }
    }

    void DoorPick()
    {
        var script = coordObject.GetComponent<ActiveObject>();
        if (!script.isOpen)
        {
            if (key && !script.isOpen)
            {
                script.isOpen = true;
                script.Take();
                dialogText.text = script.usingText;
            }
            else
            {
                dialogText.text = script.usingText;
            }
        }
        else
        {
            gameObject.GetComponent<Transform>().position = coordObject.gameObject.GetComponent<Coord_Script>().coordinate.position;
        }
    }

    private void E_Interact()
    {

        if (dealer != null)
        {
            dealer.Interact();
        }

        else if (nearGemsObject)
        {
            if (activeObject.isPick && activeObject.data.count > 0)
            {
                inventory.Add_Item(activeObject.item);
            }
            if (activeObject.tag == "Key")
            {
                KeyPick();
            }
            else if ((activeObject.tag == "GemsObject" || activeObject.tag == "Book"))
            {
                dialogText.GetComponent<DialogTextScript>().SetText(activeObject.usingText);
                hintText.text = activeObject.hintText;
                if (activeObject.tag == "Book" && activeObject.data.count > 0)
                {
                    dialogText.GetComponent<DialogTextScript>().paper.SetActive(true);
                }

                if (activeObject.data.count > 0)
                {
                    if (activeObject.data.count < 2)
                    {
                        gameController.gems += activeObject.gems;

                        scoreText.GetComponent<ScoreChangeScript>().ChangeScore(activeObject.gems);
                    }
                    if(activeObject.tag != "Book")
                    {
                        hintText.text = "";
                    }
                    dialogText.text = activeObject.usingText;
                }

                activeObject.Take();
            }
        }
        else if (coordObject != null) //if teleport object
        {
            DoorPick();
        }
    }
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
    }
}
