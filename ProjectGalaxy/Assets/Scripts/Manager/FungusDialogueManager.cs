using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class FungusDialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject sayDialogue; 
    [SerializeField] private Flowchart flowchart; 
    [SerializeField] private bool isDialogueActive = false;
    [SerializeField] private GameObject openWhileDialoguePanel;
    
    private void Start()
    {
        sayDialogue.SetActive(true);
        //openWhileDialoguePanel.SetActive(true);
    }

    private void OnEnable()
    {
        BlockSignals.OnBlockStart += OnDialogueStart;
        BlockSignals.OnBlockEnd += OnDialogueEnd;
    }

    private void OnDialogueStart(Block block)
    {
        openWhileDialoguePanel.SetActive(true);
        isDialogueActive = true;
    }

    private void OnDialogueEnd(Block block)
    {
        openWhileDialoguePanel.SetActive(false);
        isDialogueActive = false;
    }
}
