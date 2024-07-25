using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Globalization;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private AIQuery aiQuery;
    private Animator animator;
    public GameObject SpawnPoint;

        void Start()
        {
        animator = GetComponent<Animator>();
        }


    private void OnEnable()
    {
        aiQuery.OnAIResponseReceived += HandleAIResponse;
    }

    private void OnDisable()
    {
        aiQuery.OnAIResponseReceived -= HandleAIResponse;
    }

    private void HandleAIResponse(string animationTrigger, string responseWithoutTrigger)
    {
        //Debug.Log("Animation to play: " + animationTrigger);
        //Debug.Log("Response from LLM: " + responseWithoutTrigger);

        string lowerCaseAnimationTrigger = animationTrigger.ToLower();

        string matchedAnimationName = animationNames.FirstOrDefault(name => name.ToLower().Equals(lowerCaseAnimationTrigger));

        if (!string.IsNullOrEmpty(matchedAnimationName))
        {
            //Debug.Log("is" + matchedAnimationName);
            animator.SetTrigger("is" + matchedAnimationName);
        }
        else
        {
            animator.SetTrigger("isExplaining");
        }

        animator.SetTrigger("isIdle");
    }

    private string[] animationNames = new string[]
    {
        "Explaining",
        "Thinking",
        "FingerPointing",
        "HandsClasped",
        "Height",
        "Juxtaposition",
        "OneOfTheOther",
        "RubbingHandsTogether",
        "SizeSmallToLarge",
        "Speaking"
    };
    private void Update()
    {
        transform.position = SpawnPoint.transform.position;
    }
}