using UnityEngine;
public class CollisionHandler : MonoBehaviour
{
private Mover mover;
private void Start()
{
mover = GetComponent<Mover>();
}
private void OnTriggerEnter(Collider other)
{
Mover otherMover = other.GetComponent<Mover>();
if (otherMover == null) return;
// If THIS one is slower → move aside
if (mover.speed < otherMover.speed)
{
mover.MoveAside();
}
}
private void OnTriggerExit(Collider other)
{
Mover otherMover = other.GetComponent<Mover>();
if (otherMover == null) return;
// Faster one has passed → resume movement
mover.Resume();
}
}