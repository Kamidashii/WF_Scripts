using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FridgeScript : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public bool hasItem=false;
        public State currentState=State.WithCapIdle;
    }


    public enum State { WithCapIdle, WithCapOpening, ShutDown, JustIdle }
    public Data data = new Data();
    [SerializeField]
    string helloReplic, needReplic, whatNeedHint,transitionHint;
    Animator animator;
    GameControllerScript gameController;
    [SerializeField]
    Text dialogText,hintText;
    private bool enable = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameController = GameObject.FindObjectOfType<GameControllerScript>();

        CheckCurrentState();
    }

    public void CheckCurrentState()
    {
        Debug.Log("Current state: ");
        switch (data.currentState)
        {
            case State.WithCapOpening:
                {
                    Debug.Log("WithCapOpening");
                    WithCapIdleAction();
                }
                break;
            case State.ShutDown:
                {
                    Debug.Log("ShutDown");
                    WithCapOpeningAction();
                }
                break;
            case State.JustIdle:
                {
                    Debug.Log("JustIdle");
                    FinalIdle();
                }
                break;
            default:
                break;
        }
    }

    void WithCapIdleAction()
    {
        gameController.ResetTexts();
        animator.SetBool("Opening", true);
        dialogText.text = helloReplic;
        hintText.text = whatNeedHint;
        data.currentState = State.WithCapOpening;
    }

    void WithCapOpeningAction()
    {
        gameController.ResetTexts();
        animator.SetBool("Opening", false);
        animator.SetBool("ShutDown", true);
        data.currentState = State.ShutDown;
        Debug.Log("ShutDown");
        /////PlayShot ShutDown sound
    }

    void ShutDownAction()
    {
        gameController.ResetTexts();
        StartCoroutine(OpenAndIdle());

        hintText.text = transitionHint;
        data.currentState = State.JustIdle;
    }

    IEnumerator OpenAndIdle()
    {
        animator.SetBool("FinalOpening", true);
        /////PlayShot Opening sound
        yield return new WaitForSeconds(2);
        FinalIdle();
    }

    void FinalIdle()
    {
        animator.SetBool("FinalIdle", true);
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag=="Player")
        {
            enable = true;
            if (data.currentState == State.WithCapOpening)
            {
                OpenFridge();
            }
        }
    }

    void CloseFridge()
    {
        animator.SetBool("Opening", false);
        animator.SetBool("Close", true);
    }

    void OpenFridge()
    {
        animator.SetBool("Opening", true);
        animator.SetBool("Close", false);
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.tag=="Player")
        {
            enable = false;
            if (data.currentState == State.WithCapOpening)
            {
                CloseFridge();
            }
        }
    }

    void JustTalking()
    {
        if (data.currentState == State.WithCapIdle)
        {
            animator.SetBool("Opening", true);
            dialogText.text = helloReplic;
            hintText.text = whatNeedHint;
            data.currentState = State.WithCapOpening;
        }
        else
        {
            gameController.ResetTexts();
            dialogText.text = needReplic;
            hintText.text = whatNeedHint;
        }
    }

    void Update()
    {
        if (enable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (data.hasItem)
                {
                    switch (data.currentState)
                    {
                        case State.WithCapIdle:
                            {
                                WithCapIdleAction();
                            }
                            break;
                        case State.WithCapOpening:
                            {
                                WithCapOpeningAction();
                            }
                            break;
                        case State.ShutDown:
                            {
                                ShutDownAction();
                            }
                            break;
                        case State.JustIdle:
                            {
                                ;//LoadCave!
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    JustTalking();
                }
            }
        }
    }
}
