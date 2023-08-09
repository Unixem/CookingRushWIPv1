using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField] private float MoveSpeed = 7f;
  [SerializeField] private GameInput gameInput;
  [SerializeField] private LayerMask countersLayerMask;


  private bool isWalking;
  private Vector3 lastInteractDir;
  private ClearCounter selectedCounter;

  private void Start() {
    gameInput.OnInteractAction += GameInput_OnInteractAction;
  }
  private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
      if (selectedCounter != null) {
        selectedCounter.Interact();
      }
      Debug.Log("test");
    }
   private void Update () 
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking() {
      return isWalking;
    }
    private void HandleInteractions(){

       Vector2 inputVector = gameInput.GetMovementVectorNormalized();

       Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

      if (moveDir != Vector3.zero){
        lastInteractDir = moveDir;
      }
      float interactDistence = 2f;
      if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistence, countersLayerMask)) {
          if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
            //Has ClearCounter
            if (clearCounter != selectedCounter) {
              selectedCounter = clearCounter;
            }
          } else {
            selectedCounter = null;
          }
      } else {
        selectedCounter = null;
      }

     
    }
    private void HandleMovement(){
         Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        float moveDistance = MoveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove) {
          //cannot move torwards this direction

          //Only X

          Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
          canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) {
              //Can only move on the X
              moveDir = moveDirX;
            } else {
              //cannot move only on X

              //Attempt only Z movement
              Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
              canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

              if (canMove) {
                //can only move on the Z
                moveDir = moveDirZ;
              } else {
                //cannot move in any direction
              }
            }

        }
        if (canMove) {
          transform.position += moveDir * MoveSpeed * Time.deltaTime;
        }

        isWalking = moveDir != Vector3.zero;

        float rotatespeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir,Time.deltaTime * rotatespeed);

    }
}
