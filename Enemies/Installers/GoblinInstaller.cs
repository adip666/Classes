using Enemies;
using Enemies.Settings;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class GoblinInstaller : MonoInstaller
    {
        [SerializeField]
        private Goblin goblin;

        [SerializeField] private GoblinSettingsInstaller goblinSettingsInstaller = null;
        public override void InstallBindings()
        {
            goblinSettingsInstaller.InstallBindingInSubContainer(Container);
            Container.Bind<IEnemySettings>().To<GoblinSettings>().FromResolve().WhenInjectedInto<Goblin>();
            Container.BindFactory<Goblin, EnemyFactory<Goblin>>().FromComponentInNewPrefab(goblin);

        }
    }
}