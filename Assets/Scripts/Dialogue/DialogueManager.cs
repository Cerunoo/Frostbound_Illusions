using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FollusionController;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Text nameText;

    public float typeDelay;

    Animator boxAnim;

    Queue<string> sentences;
    bool isTyping;

    private PlayerController player;

    [SerializeField] private InputController input;
    private bool btnDown;

    private void Awake()
    {
        boxAnim = GetComponent<Animator>();
        sentences = new Queue<string>();

        player = FindObjectOfType<PlayerController>();

        input.controls.Dialogue.NextSentence.performed += context =>
        {
            if (!isTyping) DisplayNextSentence();
            else btnDown = true;
        };
    }

    public void StartDialogue(Dialogue dialogue)
    {
        boxAnim.SetBool("show", true);
        player.disableMove = true;

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (btnDown && dialogueText.text.Length > 0 && boxAnim.GetCurrentAnimatorStateInfo(0).IsName("Show") && boxAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
            {
                btnDown = false;
                dialogueText.text = sentence;
                break;
            }
            else btnDown = false;

            dialogueText.text += letter;
            yield return new WaitForSeconds(typeDelay);
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        boxAnim.SetBool("show", false);
        player.disableMove = false;
    }
}