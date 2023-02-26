using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS{
	public class PlayerLocomotion : MonoBehaviour
	{
		Transform cameraObject;
		Vector3 moveDirection;
		InputHandler inputHandler;  

		[HideInInspector]
		public Transform myTransform;
		[HideInInspector]
		public AnimatorHandler animatorHandler;

		public new Rigidbody rigidbody;
		public GameObject normalCamera;

		[Header("Stats")]
		[SerializeField]
		float movementSpeed = 5;
		[SerializeField]
		float rotationSpeed = 10;




			// Start is called before the first frame update
			void Start()
			{
				rigidbody = GetComponent<Rigidbody>();
				inputHandler = GetComponent<InputHandler>();
				cameraObject = Camera.main.transform;
				animatorHandler = GetComponentInChildren<AnimatorHandler>();
				myTransform = transform;
				animatorHandler.Initialize();
			}

			public void Update(){
				float delta = Time.deltaTime;
				inputHandler.TickInput(delta);

				moveDirection = cameraObject.forward * inputHandler.vertical;
				moveDirection += cameraObject.right * inputHandler.horizontal;
				moveDirection.Normalize();
				moveDirection.y = 0;

				float speed = movementSpeed;
				moveDirection *= speed;

				Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
				rigidbody.velocity = projectedVelocity;

				// Debug.Log(inputHandler.moveAmount);
				animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount,0);

				if (animatorHandler.canRotate){
					HandleRotation(delta);
				}
			}

			#region Movement
			Vector3 normalVector;
			Vector3 targetPosition;
			
			private void HandleRotation(float delta){
				Vector3 targetDir = Vector3.zero;
				float moveOverride = inputHandler.moveAmount;

				targetDir = cameraObject.forward * inputHandler.vertical;
				targetDir += cameraObject.right * inputHandler.horizontal;

				targetDir.Normalize();
				targetDir.y = 0;

				if (targetDir == Vector3.zero){
					targetDir = myTransform.forward;
				}

				Quaternion tr = Quaternion.LookRotation(targetDir);
				Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rotationSpeed*delta);

				myTransform.rotation = targetRotation;
			}
			#endregion
	}

}
