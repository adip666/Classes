using NUnit.Framework;
using Pyramid.Signal;
using Pyramid.TimeManipulation;
using UnityEngine;
using Zenject;

 namespace PyramidTests.TimeManipulation
{
    [TestFixture]
    public class TimeSystemTest : ZenjectUnitTestFixture
    {
        [Inject]
        private ITimeSystem timeSystem = null;

        [SetUp]
        public void Initialization()
        {
            Install();
        }

        void Install()
        {
            DeclareSignal();
            Container.Bind<ITimeSystem>().To<TimeSystem>().AsSingle();
            Container.Bind<ITimeSystemSettings>().To<TimeSystemSettings>().AsSingle();
            Container.Inject(this);
        }

        void DeclareSignal()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<ISignalSystem>().To<SignalSystem>().AsSingle();
            Container.Bind<ISignalSystemFacade>().To<SignalSystemFacade>().AsSingle();
            Container.DeclareSignal<TimeChangedSignal>().OptionalSubscriber();
        }
        
        [Test]
        public void StartTime_IsTimeModeChangedToStart()
        {
            timeSystem.SetTimeTo(0);
            timeSystem.StartTime();
            int currentMode= timeSystem.GetCurrentTimeMode();
            Assert.AreNotEqual(0, currentMode);
        }

        [Test]
        public void Pause_IsTimeModeChangedToPause()
        {
            timeSystem.Pause();
            int currentMode= timeSystem.GetCurrentTimeMode();
            Assert.AreEqual(0, currentMode);
        }
        
        [Test]
        public void SetFasterTime_IsTimeModeChanged()
        {
            int startMode= timeSystem.GetCurrentTimeMode();
            timeSystem.SetFasterTime();
            int endMode= timeSystem.GetCurrentTimeMode();
            Assert.Less(startMode, endMode);
        }
        
        [Test]
        public void SetSlowerTime_IsTimeModeChanged()
        {
            int startMode= timeSystem.GetCurrentTimeMode();
            timeSystem.SetSlowerTime();
            int endMode= timeSystem.GetCurrentTimeMode();
            Assert.Greater(startMode, endMode);
        }

        [Test]
        public void ChangePauseMode_IsPauseModeChanged()
        {
            int startMode= timeSystem.GetCurrentTimeMode();
            timeSystem.ChangePauseMode();
            int endMode= timeSystem.GetCurrentTimeMode();
            Assert.AreNotEqual(startMode, endMode);
        }
        
        [Test]
        public void SetTimeTo_IsTimeModeForceChangedToParam()
        {
            timeSystem.SetTimeTo(3);
            int endMode= timeSystem.GetCurrentTimeMode();
            Assert.AreEqual(3, endMode);
        }
    }
}

