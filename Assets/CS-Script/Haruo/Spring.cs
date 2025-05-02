using System;
using UnityEngine;

namespace CS_Script.Haruo
{
    public class Spring : BackBox
    {
        private PlayerMovement playerMovement;
        private Shooter shooter;

        protected override void Start()
        {
            base.Start();
            GameObject playerObject = GameObject.FindWithTag("Player");
            playerMovement = playerObject.GetComponent<PlayerMovement>();
            shooter = playerObject.GetComponent<Shooter>();

            OnBounce += () =>
            {
                if (playerMovement != null)
                {
                    playerMovement.Direction = -playerMovement.Direction;
                    shooter.Flip();
                }
            };
        }
    }
}