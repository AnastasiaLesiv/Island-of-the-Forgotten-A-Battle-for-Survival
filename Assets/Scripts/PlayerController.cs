using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public Animator animator;
   public Rigidbody rigidbody;
   public Transform mainCamera;
   public float jumpForce = 3.5f;
   public float runSpeed = 2f;
   public float fastRunSpeed = 6f;
   public float currentSpeed;
   private float animationInterpolation = 1f;

   private void Start()
   {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
   }

   void Run()
   {
      animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
      animator.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
      animator.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
      currentSpeed = Mathf.Lerp(currentSpeed, fastRunSpeed, Time.deltaTime * 3);
   }

   void Walk()
   {
      animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
      animator.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
      animator.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
      currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime * 3);
   }

   private void Update()
   {
      transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

      if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
      {
         if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
         {
            Walk();
         }
         else
         {
            Run();
         }
      }
      else
      {
         Walk();
      }

      if (Input.GetKeyDown(KeyCode.Space))
      {
         animator.SetTrigger("jump");
      }
   }

   private void FixedUpdate()
   {
      Vector3 camF = mainCamera.forward;
      Vector3 camR = mainCamera.right;

      camF.y = 0;
      camR.y = 0;
      Vector3 movingVector;
      movingVector =
         Vector3.ClampMagnitude(
            camF.normalized * Input.GetAxis("Vertical") * currentSpeed +
            camR.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);
      animator.SetFloat("magnitude", movingVector.magnitude / currentSpeed);
      rigidbody.velocity = new Vector3(movingVector.x, rigidbody.velocity.y, movingVector.z);
      rigidbody.angularVelocity = Vector3.zero;
   }

   public void Jump()
   {
      rigidbody.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
   }
   
}
