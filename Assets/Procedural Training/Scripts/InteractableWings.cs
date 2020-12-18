//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem.Sample
{
	//-------------------------------------------------------------------------
	[RequireComponent(typeof(Interactable))]
	public class InteractableWings : MonoBehaviour
	{
		private Vector3 oldPosition;
		private Quaternion oldRotation;

		private float attachTime;

		private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

		private Interactable interactable;

		//support for placing Object in slot

		[SerializeField] private Transform destinationTransform;

		private Vector3 currentPos;

		[SerializeField] private float proximityRange = 0.2f;
		private bool isObjWithinProx = false;
		private bool IsObjWithinProx
        {
            get
            {
				return isObjWithinProx;
            }
            set
            {
				if (value != isObjWithinProx)
                {
					isObjWithinProx = value;
					onWithinProximity(value);
                }
            }
        }

		//create event when object is attached to hand
		public delegate void WhenAttachedToHand(bool isAttached);
		public static event WhenAttachedToHand onAttachedToHand;

		//Event when Object is within proximity
		public delegate void WhenWithinProx(bool isWithinProx);
		public static event WhenWithinProx onWithinProximity = delegate { };

		//-------------------------------------------------
		void Awake()
		{
			var textMeshs = GetComponentsInChildren<TextMesh>();


			interactable = this.GetComponent<Interactable>();
		}


		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
		private void OnHandHoverBegin(Hand hand)
		{
		}


		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
		private void OnHandHoverEnd(Hand hand)
		{
		}


		//-------------------------------------------------
		// Called every Update() while a Hand is hovering over this object
		//-------------------------------------------------
		private void HandHoverUpdate(Hand hand)
		{
			GrabTypes startingGrabType = hand.GetGrabStarting();
			bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

			if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
			{
				// Save our position/rotation so that we can restore it when we detach
				oldPosition = transform.position;
				oldRotation = transform.rotation;

				// Call this to continue receiving HandHoverUpdate messages,
				// and prevent the hand from hovering over anything else
				hand.HoverLock(interactable);

				// Attach this object to the hand
				hand.AttachObject(gameObject, startingGrabType, attachmentFlags);

				if (onAttachedToHand != null)
					onAttachedToHand(true);
			}
			else if (isGrabEnding)
			{
				// Detach this object from the hand
				hand.DetachObject(gameObject);

				// Call this to undo HoverLock
				hand.HoverUnlock(interactable);

				if (isObjWithinProx)
				{
					transform.position = destinationTransform.position;
					transform.rotation = destinationTransform.rotation;
				}
				else
				{// Restore position/rotation
					transform.position = oldPosition;
					transform.rotation = oldRotation;
				}
				if (onAttachedToHand != null)
				{
					onAttachedToHand(false);
				}
			}
		}


		//-------------------------------------------------
		// Called when this GameObject becomes attached to the hand
		//-------------------------------------------------
		private void OnAttachedToHand(Hand hand)
		{
			attachTime = Time.time;
		}

		private bool lastHovering = false;
		private void Update()
		{
			if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
			{
				lastHovering = interactable.isHovering;
			}

			//logic to determine if object is in proximity 
			currentPos = transform.parent.transform.position;
			float diffInDist = Vector3.Distance(currentPos, destinationTransform.position);
			if (diffInDist < proximityRange && diffInDist > -proximityRange)
			{
				print("object is within proximity!");
				IsObjWithinProx = true;
			}
			else
			{
				IsObjWithinProx = false;
			}
		}
	}
}
