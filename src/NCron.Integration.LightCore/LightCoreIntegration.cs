using System;
using System.Collections.Generic;
using LightCore;
using NCron.Fluent;

namespace NCron.Integration.LightCore
{
    /// <summary>
    /// Provides extension methods on the fluent API, allowing job registrations using LightCore as resolving container.
    /// </summary>
    public static class LightCoreIntegration
    {
        /// <summary>
        /// Gets or sets the LightCore container which has a reference to the LightCore container.
        /// </summary>
        public static LightCoreContainer LightCoreContainer { get; set; }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/>.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part)
            where TJob : ICronJob
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve<TJob>());
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/>.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="arguments">The arguments that are injected into the resolved type.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part, params object[] arguments)
            where TJob : ICronJob
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve<TJob>(arguments));
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/>.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="arguments">The arguments that are injected into the resolved type.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part, IEnumerable<object> arguments)
            where TJob : ICronJob
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve<TJob>(arguments));
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/>.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="namedArguments">The named arguments that are injected into the resolved type.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part, AnonymousArgument namedArguments)
            where TJob : ICronJob
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve<TJob>(namedArguments));
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/>.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="activatorFunction">The activator function to which the object creation is delegated.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part, Func<IContainer, TJob> activatorFunction)
            where TJob : ICronJob
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve<TJob>(activatorFunction));
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <typeparam name="TJob">The type of job to be registered. Must implement <see cref="ICronJob"/>.</typeparam>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="namedArguments">The named arguments that are injected into the resolved type.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run<TJob>(this SchedulePart part, IDictionary<string, object> namedArguments)
            where TJob : ICronJob
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve<TJob>(namedArguments));
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="contractType">The contract type which is resolved from the container.</param>
        /// <param name="arguments">The named arguments that are injected into the resolved type.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run(this SchedulePart part, Type contractType, params object[] arguments)
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve(contractType, arguments) as ICronJob);
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="contractType">The contract type which is resolved from the container.</param>
        /// <param name="arguments">The named arguments that are injected into the resolved type.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run(this SchedulePart part, Type contractType, IEnumerable<object> arguments)
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve(contractType, arguments) as ICronJob);
        }

        /// <summary>
        /// Registers a job to be executed using LightCore as resolving container.
        /// </summary>
        /// <param name="part">The fluent part upon which the job is to be registered.</param>
        /// <param name="contractType">The contract type which is resolved from the container.</param>
        /// <param name="arguments">The named arguments that are injected into the resolved type.</param>
        /// <returns>A part that allows chained fluent method calls.</returns>
        public static JobPart Run(this SchedulePart part, Type contractType, IDictionary<string, object> arguments)
        {
            return part.With(() => LightCoreContainer.GetContainer()).Run(c => c.Resolve(contractType, arguments) as ICronJob);
        }
    }
}
