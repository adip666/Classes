namespace Enemies.Settings
{
    public interface IEnemySettings
    {
        int MovementSpeed { get; }
        int Life { get; }
        float AttackTimeOffset  { get; }
        float AttackRange { get; }
       
    }
}