using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;

  private Animator PlayerAnimation;

  private void Awake() {
    PlayerAnimation = GetComponent<Animator>();
  }

  private void Update(){
    PlayerAnimation.SetBool(IS_WALKING,player.IsWalking());
  }
}
