using System.Collections.Generic;
using PacMan.Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


namespace PacMan.AI
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private IPlayer player;
        float speed = 3;
        private Vector3 currentDirection = Vector3.forward;

        [Inject]
        public void Inject(IPlayer player)
        {
            this.player = player;
        }

        public void FixedTick()
        {
            transform.Translate(currentDirection * speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag(Keys.Tag.WALL) || other.transform.CompareTag(Keys.Tag.ENEMY))
            {
                currentDirection = DrawDirection(currentDirection);
            }

            if (other.transform.CompareTag(Keys.Tag.PLAYER))
            {
                player.AddDamage();
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.transform.CompareTag(Keys.Tag.WALL) || other.transform.CompareTag(Keys.Tag.ENEMY))
            {
                currentDirection = DrawDirection(currentDirection);
            }
        }

        Vector3 DrawDirection(Vector3 currentDirection)
        {
            List<Vector3> directions = new List<Vector3>()
                { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            directions.Remove(currentDirection);
            int randomIndex = Random.Range(0, directions.Count);

            return directions[randomIndex];
        }
    }
}