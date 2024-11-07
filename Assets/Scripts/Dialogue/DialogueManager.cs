using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Text nameText;
    public float typeDelay;

    Queue<string> sentences;
    private bool isTyping;
    private bool btnDown;

    private Animator boxAnim;
    private PlayerController player;

    private DialogueNPC currentNPC;

    private void Awake()
    {
        boxAnim = GetComponent<Animator>();
        sentences = new Queue<string>();

        if (PlayerController.Instance != null) player = PlayerController.Instance;
    }

    private void InputNextSentence(InputAction.CallbackContext context)
    {
        if (!isTyping) DisplayNextSentence();
        else btnDown = true;
    }

    public void StartDialogue(TextAsset dialog, DialogueNPC npc)
    {
        if (InputController.Instance != null) InputController.Instance.controls.Dialogue.NextSentence.performed += InputNextSentence;

        Dialogue dialogue = JsonUtility.FromJson<Dialogue>(dialog.ToString());
        currentNPC = npc;

        boxAnim.SetBool("show", true);
        player.disableMove = true;

        nameText.text = dialogue.name;
        dialogueText.text = "";
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        // DisplayNextSentence(); // Функция вызывается из ключа события в анимации Show
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            currentNPC?.TriggerDialogueOver();
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
        if (InputController.Instance != null) InputController.Instance.controls.Dialogue.NextSentence.performed -= InputNextSentence;

        if (currentNPC != null) currentNPC.button.pressed = false;
        currentNPC = null;

        boxAnim.SetBool("show", false);
        player.disableMove = false;
    }
}