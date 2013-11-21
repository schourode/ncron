using System;
using System.Collections.Generic;
using LightCore;

namespace NCron.Integration.LightCore
{
    /// <summary>
    /// The container for the LightCore integration. All methods are mapped to corresponding LightCore resolve methods.
    /// Necessary because LightCore isn't disposable.
    /// </summary>
    public class LightCoreContainer : IDisposable
    {
        /// <summary>
        /// Gets or sets the root container which is used for job execution.
        /// This property should usually only be set once, and its value is never disposed.
        /// </summary>
        private IContainer RootContainer { get; set; }

        /// <summary>
        /// There's nothing to dispose in the LightCore framework.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        /// Construct a wrapper container with the given root container.
        /// </summary>
        /// <param name="rootContainer">The LightCore root container.</param>
        public LightCoreContainer(IContainer rootContainer)
        {
            RootContainer = rootContainer;
        }

        /// <summary>
        /// Resolves a contract (include subcontracts).
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The resolved instance as TContract.</returns>
        public TContract Resolve<TContract>()
        {
            return RootContainer.Resolve<TContract>();
        }

        /// <summary>
        /// Resolve an object and inject the given arguments into it.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="arguments">The constructor arguments.</param>
        /// <returns>The resolved instance as TContract.</returns>
        public TContract Resolve<TContract>(params object[] arguments)
        {
            return RootContainer.Resolve<TContract>(arguments);
        }

        /// <summary>
        /// Resolve an object and inject the given arguments into it.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="arguments">The constructor arguments.</param>
        /// <returns>The resolved instance as TContract.</returns>
        public TContract Resolve<TContract>(IEnumerable<object> arguments)
        {
            return RootContainer.Resolve<TContract>(arguments);
        }

        /// <summary>
        /// Resolve an object and inject the given arguments into it.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="namedArguments">The named constructor arguments.</param>
        /// <returns>The resolved instance as TContract.</returns>
        public TContract Resolve<TContract>(IDictionary<string, object> namedArguments)
        {
            return RootContainer.Resolve<TContract>(namedArguments);
        }

        /// <summary>
        /// Resolve an object and inject the given arguments into it.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="namedArguments">The named constructor arguments.</param>
        /// <returns>The resolved instance as TContract.</returns>
        public TContract Resolve<TContract>(AnonymousArgument namedArguments)
        {
            return RootContainer.Resolve<TContract>(namedArguments);
        }

        /// <summary>
        /// Resolve an object and inject the given arguments into it.
        /// </summary>
        /// <param name="contractType">The contract type.</param>
        /// <param name="arguments">The constructor arguments.</param>
        /// <returns>The resolved instance as TContract.</returns>
        public object Resolve(Type contractType, params object[] arguments)
        {
            return RootContainer.Resolve(contractType, arguments);
        }

        /// <summary>
        /// Resolve an object and inject the given arguments into it.
        /// </summary>
        /// <param name="contractType">The contract type.</param>
        /// <param name="arguments">The constructor arguments.</param>
        /// <returns>The resolved instance as TContract.</returns>
        public object Resolve(Type contractType, IEnumerable<object> arguments)
        {
            return RootContainer.Resolve(contractType, arguments);
        }

        /// <summary>
        /// Resolve an object and inject the given arguments into it.
        /// </summary>
        /// <param name="contractType">The contract type.</param>
        /// <param name="namedArguments">The named constructor arguments.</param>
        /// <returns>The resolved instance as TContract.</returns>
        public object Resolve(Type contractType, IDictionary<string, object> namedArguments)
        {
            return RootContainer.Resolve(contractType, namedArguments);
        }

        /// <summary>
        /// Resolves the container. The class <see cref="LightCoreIntegration"/> uses this to receive the underlying LightCore instance.
        /// </summary>
        /// <returns>The LightCore container.</returns>
        public LightCoreContainer GetContainer()
        {
            return this;
        }
    }
}
