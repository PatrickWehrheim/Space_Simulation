
public interface IDamagable
{
    public int Health { get; }
    public int Shield { get; }
    public void GetDemage(int demagePoints);
    public void Die();
}
