using System;
using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using AD.UI;
using UnityEngine;

namespace AD.Game
{
    public class GamePlayer : ADController
    {
        [Header("GamePlayer")]
        [SerializeField] VirtualJoystick _VirtualJoystick;
        [SerializeField] Camera CatchCamera;

        [SerializeField] GameObject BulletObject;
        [SerializeField] Animator animator;

        [SerializeField] AD.UI.Button A, B, C, D;

        public float Speed = 2.5f;

        [Serializable]
        public enum ActionType
        {
            A, B, C, D, None
        }
        public ActionType PreAction = ActionType.None, CurrentAction = ActionType.None; 

        private void Start()
        {
            GameApp.instance.RegisterController(this);
        }

        public override void Init()
        {
            A.AddListener(AttackA);
            B.AddListener(AttackB);
            C.AddListener(ParryC);
            D.AddListener(DodgeD);
        }

        private void Update()
        {
            transform.position += _VirtualJoystick.DirectionValue * Speed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, _VirtualJoystick.GlobalAngle - 90);
        }

        public float accuracy { get { return _VirtualJoystick.DateValue; } }

        public void AttackA()
        {
            if (CurrentAction == ActionType.None)
            {
                animator.SetBool("A", true);
                CurrentAction = ActionType.A;
            }
            else PreAction = ActionType.A;

        }
        public void AttackB()
        {
            if (CurrentAction == ActionType.None)
            {
                animator.SetBool("B", true);
                CurrentAction = ActionType.B;
            }
            else PreAction = ActionType.B;
        }
        public void ParryC()
        {
            if (CurrentAction == ActionType.None)
            {
                animator.SetBool("C", true);
                CurrentAction = ActionType.C;
            }
            else PreAction = ActionType.C;
        }
        public void DodgeD()
        {
            if (CurrentAction == ActionType.None)
            {
                animator.SetBool("D", true);
                CurrentAction = ActionType.D;
            }
            else PreAction = ActionType.D;
        }

        public void AttackA_ShootBullet()
        {
            var bullet = GameObject.Instantiate(BulletObject, transform.parent);
            bullet.transform.position = transform.position;
            bullet.transform.eulerAngles = transform.eulerAngles;
            bullet.GetComponent<Bullet>().Parent = transform;
        }

        public void Revert()
        {
            animator.SetBool("A", false);
            animator.SetBool("B", false);
            animator.SetBool("C", false);
            animator.SetBool("D", false);

            switch (PreAction)
            {
                case ActionType.A:
                    animator.SetBool("A", true);
                    break;
                case ActionType.B:
                    animator.SetBool("B", true);
                    break;
                case ActionType.C:
                    animator.SetBool("C", true);
                    break;
                case ActionType.D:
                    animator.SetBool("D", true);
                    break;
                case ActionType.None:
                    break;
                default:
                    break;
            }
            PreAction = ActionType.None;
            CurrentAction = ActionType.None;
        }

    }
}