using Enemies;
using InputSystem;
using Player.ObjectPool;
using SaveSystem;
using Signals;
using SignalsSystem;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerSystem : MonoBehaviour, IPlayerSystem, ITickable
    {
        [Inject] private IPlayerSystemSettings playerSystemSettings;
        [Inject] private IInputSystem inputSystem;
        [Inject] private ISignalSystem signalSystem;
        [Inject] private ISaveSystem saveSystem;
        [Inject] private PlayerEffectsFactory effects;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform attackPoint;


        private int currentLife;
        public int CurrentLife => currentLife;
        private bool isGrounded = false;
        private bool isFacingRight = true;
        private bool isDead = false;
        private bool canAttack = true;
        private float currentAttackTime;
        

        public void Initialize()
        {
            currentLife = saveSystem.IsExist(DataKeys.CURRENT_PLAYER_LIFE)
                    ? saveSystem.LoadPlayerLife()
                    : playerSystemSettings.PlayerStartedLife;
        }

        public void Tick()
        {
            if (!isDead)
            {
                CheckGround();
                Movement(inputSystem.Movement);
                Jump(inputSystem.IsJump);
                Attack(inputSystem.IsAttack);
            }
            else
            {
                float dieAnimationTime = Utils.Utils.GetCurrentAnimatorTime(animator);
                if (dieAnimationTime > .9f)
                {
                    signalSystem.FireSignal<PlayerDeadSignal>();
                }
            }
        }

        public void AddDamage(Vector2 forceDirection)
        {
            currentLife--;
            animator.SetTrigger("Hit");
            OnPlayerLifeChanged();
            if (currentLife > 0)
            {
                rigidbody.AddRelativeForce(forceDirection * playerSystemSettings.DamageForce);
            }
            else
            {
                PlayerDie();
            }
        }

        public void Deinitialize()
        {
            saveSystem.SavePlayerLife(currentLife);
        }


        private void Movement(float horizontalInput)
        {
            Vector3 direction = new Vector3(horizontalInput, 0, 0);
            transform.Translate(direction * playerSystemSettings.PlayerSpeed * Time.deltaTime);

            if (direction.x > 0f && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0f && isFacingRight)
            {
                Flip();
            }

            if (isGrounded)
                animator.SetBool("IsRun", horizontalInput != 0);
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void CheckGround()
        {
            isGrounded = false;
            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(groundCheck.position, playerSystemSettings.GroundDetectionRange,
                    playerSystemSettings.GroundMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                }
            }

            animator.SetBool("IsGrounded", isGrounded);
        }

        private void Jump(bool isJump)
        {
            if (isGrounded && isJump)
            {
                isGrounded = false;
                animator.SetTrigger("Jump");
                animator.SetBool("Fall", false);
                rigidbody.AddForce(new Vector2(0f, playerSystemSettings.JumpForce));
            }

            if (!isGrounded && rigidbody.velocity.y < 0)
            {
                animator.SetBool("Fall", true);
            }
        }

        private void Attack(bool attack)
        {
            if (attack && canAttack)
            {
                canAttack = false;
                currentAttackTime = 0;
                animator.SetTrigger("OnAttack");
                Collider2D[] hitEnemies =
                    Physics2D.OverlapCircleAll(attackPoint.position, playerSystemSettings.AttackRange,
                        playerSystemSettings.EnemyLayer);
                
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<Enemy>().AddDamage();
                  var sparks = effects.Create();
                  sparks.transform.position = attackPoint.position;
                }
            }

            if (!canAttack)
            {
                currentAttackTime += Time.deltaTime;
                if (currentAttackTime > playerSystemSettings.AttackTimeOffset)
                {
                    canAttack = true;
                    currentAttackTime = 0;
                }
            }
        }

        private void OnPlayerLifeChanged()
        {
            signalSystem.FireSignal(new PlayerLifeChangedSignal(currentLife));
        }

        private void PlayerDie()
        {
            isDead = true;
            animator.SetTrigger("Die");
        }
    }
}