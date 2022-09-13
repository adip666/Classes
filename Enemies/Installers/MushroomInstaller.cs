using Enemies;
using Enemies.Settings;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class MushroomInstaller : MonoInstaller
    {
        [SerializeField]
        private Mushroom mushroom;

        [SerializeField] private MushroomSettingsInstaller mushroomSettingsInstaller = null;
        public override void InstallBindings()
        {
            mushroomSettingsInstaller.InstallBindingInSubContainer(Container);
            Container.Bind<IEnemySettings>().To<MushroomSettings>().FromResolve().WhenInjectedInto<Mushroom>();
            Container.BindFactory<Mushroom, EnemyFactory<Mushroom>>().FromComponentInNewPrefab(mushroom);

        }
    }
}