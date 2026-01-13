using UnityEngine;
public class Mover : MonoBehaviour
{
public float speed = 2f;
public float sideOffset = 2f;
public bool giveWay = false;
public bool sideMovementComplete = false;
private Vector3 sideLane;
private bool movingToSide = false;
private Rigidbody rb;
private void Start()
{
rb = GetComponent<Rigidbody>();
// Calculate side lane only once
sideLane = new Vector3(transform.position.x,
transform.position.y,
transform.position.z + sideOffset);
}
private void FixedUpdate()
{
if (movingToSide)
{
// Smooth sidestep
transform.position = Vector3.MoveTowards(
transform.position,sideLane,
Time.deltaTime * 2f
);
// Once reached side lane, stop side movement
if (Vector3.Distance(transform.position, sideLane) < 0.05f)
{
movingToSide = false;
sideMovementComplete = true;
}
return;
}
// If giving way and side movement is complete → stay still
if (giveWay && sideMovementComplete)
return;
// Otherwise move forward normally
rb.MovePosition(transform.position + transform.forward * speed *
Time.fixedDeltaTime);
}
public void MoveAside()
{
giveWay = true;
movingToSide = true;
}
public void Resume()
{
giveWay = false;
sideMovementComplete = false;
}
}