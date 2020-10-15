using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyApm.Core
{
    public interface ICoreBuilder
    {
        /// <summary>
        /// 建造
        /// </summary>
        /// <returns></returns>
        IContainer Build();

        /// <summary>
        /// 在启动前执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        ICoreBuilder OnStarting(Action<ContainerBuilder> action, string actionName = null);

        /// <summary>
        /// 在启动完成之后执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        ICoreBuilder OnStarted(Action<IContainer> action, string actionName = null);

    }

    public sealed class CoreBuilder : ICoreBuilder
    {

        private readonly IDictionary<string, Delegate> _actionDictionary = new Dictionary<string, Delegate>();
        private int _actionIdentity;

        public CoreBuilder()
        {
            OnStarted(container =>
            {

            });
        }

        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            foreach (var action in GetBuildingActions())
                action(builder);

            builder.RegisterModule<CoreModule>();

            var container = builder.Build();

            foreach (var action in GetBuildedActions())
                action(container);

            WorkContext.LifetimeScope = container;
            return container;
        }

        /// <summary>
        /// 在启动完成后执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public ICoreBuilder OnStarted(Action<IContainer> action, string actionName = null)
        {
            string str = this.GetActionName(actionName);
            this._actionDictionary[str] = action;
            return this;
        }

        /// <summary>
        /// 在启动前执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public ICoreBuilder OnStarting(Action<ContainerBuilder> action, string actionName = null)
        {
            string str = this.GetActionName(actionName);
            this._actionDictionary[str] = action;
            return this;
        }

        private string GetActionName(string actionName)
        {
            if (!string.IsNullOrWhiteSpace(actionName))
            {
                return string.Concat("Custom_", actionName);
            }
            this._actionIdentity = this._actionIdentity + 1;
            return string.Concat("Default_", this._actionIdentity);
        }

        private IEnumerable<Action<IContainer>> GetBuildedActions()
        {
            return (
                from i in this._actionDictionary
                where i.Value is Action<IContainer>
                select i.Value).OfType<Action<IContainer>>().ToArray<Action<IContainer>>();
        }

        private IEnumerable<Action<ContainerBuilder>> GetBuildingActions()
        {
            return (
                from i in this._actionDictionary
                where i.Value is Action<ContainerBuilder>
                select i.Value).OfType<Action<ContainerBuilder>>().ToArray<Action<ContainerBuilder>>();
        }

    }

}
