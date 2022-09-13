using Player;
using Player.ObjectPool;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class PlayerSystemInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSystem player;
        [SerializeField] private PlayerEffect effectsPrefab;
       
        [SerializeField, Header("Settings")]
        private PlayerSystemSettingsInstaller playerSettingsInstaller = null;
        public override void InstallBindings()
        {
            playerSettingsInstaller.InstallBindingInSubContainer(Container);
            Container.Bind<IPlayerSystemSettings>().To<PlayerSystemSettings>().FromResolve();
            Container.BindFactory<PlayerSystem, PlayerFactory>().FromComponentInNewPrefab(player);
            Container.BindFactory<PlayerEffect, PlayerEffectsFactory>()
                .FromPoolableMemoryPool<PlayerEffect, PlayerEffectsPool>(poolBinder => poolBinder
                    .WithInitialSize(4)
                    .FromComponentInNewPrefab(effectsPrefab)
                    .UnderTransformGroup("==== EFFECTS ===="));
        }
    }
}
