using Core;
using Gameplay.Controllers;
using Gameplay.Events;
using Gameplay.Events.Aggregators;
using Gameplay.Managers;
using Gameplay.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Scopes
{
    public class MatchGameplayLifetimeScope : LifetimeScope
    {
        [Header("Controllers")]
        public InputController InputController;
        public TimerController TimerController;
        
        [Header("Views")]
        public PoolView PoolView;
        public TableView TableView;
        public GameResultWindowView GameResultWindowView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterManagers(builder);
            RegisterEvents(builder);
        }
        
        private void RegisterEvents(IContainerBuilder builder)
        {
            builder.RegisterEvent<ObjectSelectedEvent>();
            builder.RegisterEvent<ObjectAddedToPoolEvent>();
            builder.RegisterEvent<PoolOverflowEvent>();
            builder.RegisterEvent<OutOfTimeEvent>();
            builder.RegisterEvent<TableClearedEvent>();
            builder.RegisterEvent<GameplayFinishEvent>();
            builder.RegisterEvent<MatchingEvent>();
            
            builder.Register<GameplayEventsAggregator>(Lifetime.Transient);
            builder.Register<GamePoolEventsAggregator>(Lifetime.Transient);
        }
        
        private void RegisterManagers(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayManager>().WithParameter(GameResultWindowView);
            builder.RegisterEntryPoint<SelectionManager>().WithParameter(InputController);
            builder.RegisterEntryPoint<GamePoolManager>().WithParameter(PoolView);
            builder.RegisterEntryPoint<TableManager>().WithParameter(TableView);
            builder.RegisterEntryPoint<TimerManager>().WithParameter(TimerController);
        }
    }
}