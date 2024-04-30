using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
  Vector2 inputVector = Vector2.zero;
  int currentScore = 0;
  [SerializeField]
  TMP_Text score;
  [SerializeField]
  float speed = 2;
  float timer = 0;
  void Start()
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }
  void Update()
  {

    Vector3 movement = Camera.main.transform.right * inputVector.x
      + Camera.main.transform.forward * inputVector.y;
      
    if (movement.magnitude > 0)
    {
      movement.y = 0;
      movement.Normalize();
      transform.forward = movement;
    }

    movement = movement * speed * Time.deltaTime;
    
    if (movement.magnitude > 0)
    {
      GetComponent<Animator>().SetBool("IsWalkingForward", true);
    }
    else
    {
        GetComponent<Animator>().SetBool("IsWalkingForward", false);
    }

    GetComponent<CharacterController>().Move(movement);

    if(Input.GetMouseButtonDown(0) && timer >= 1.9)
    {
      GetComponent<Animator>().SetBool("IsAttacking", true);
      timer = 0;
    }
    else
    {
      GetComponent<Animator>().SetBool("IsAttacking", false);
      timer += Time.deltaTime;
    }
  }
  private void OnTriggerStay(Collider other) {
    if(other.gameObject.tag == "Enemy")
    {
      currentScore++;
      Debug.Log("wee");
      UpdateScore();
    }
  }
  void OnMove(InputValue value)
  {
    inputVector = value.Get<Vector2>();
  }
  void UpdateScore()
  {
      score.text = "Score: " + currentScore;
  } 
}

