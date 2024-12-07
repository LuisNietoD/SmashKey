public interface IEnemy
{
    public int damage { get; }
    public int health { get; set; }
    public void Attack();
    public void Hit(int damageAmount);
}