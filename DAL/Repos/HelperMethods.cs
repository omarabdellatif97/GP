using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GP_API.Repos
{
    public static class HelperMethods
    {

        /// <summary>
        /// Validates whether specific value is not null, and throws an exception if it is null.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="name">The name of the parameter.</param>
        public static T CheckNotNull<T>(this T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            return value;
        }

        /// <summary>
        /// Tracks changes on childs models by comparing with latest database state.
        /// </summary>
        /// <typeparam name="T">The type of model to track.</typeparam>
        /// <param name="context">The database context tracking changes.</param>
        /// <param name="childs">The childs to update, detached from the context.</param>
        /// <param name="existingChilds">The latest existing data, attached to the context.</param>
        /// <param name="match">A function to match models by their primary key(s).</param>
        public static void TrackChildChanges<T>(this DbContext context, ICollection<T> childs, ICollection<T> existingChilds, Func<T, T, bool> match)
            where T : class
        {
            context.CheckNotNull(nameof(context));
            childs.CheckNotNull(nameof(childs));
            existingChilds.CheckNotNull(nameof(existingChilds));

            // Delete childs.
            foreach (var existing in existingChilds.ToList())
            {
                if (!childs.Any(c => match(c, existing)))
                {
                    existingChilds.Remove(existing);
                }
            }

            // Update and Insert childs.
            var existingChildsCopy = existingChilds.ToList();
            foreach (var item in childs.ToList())
            {
                var existingTest = existingChildsCopy
                    .Where(c => match(c, item))
                    .ToList();
                var existing = existingChildsCopy
                    .Where(c => match(c, item))
                    .SingleOrDefault();

                if (existing != null)
                {
                    // Update child.
                    context.Entry(existing).CurrentValues.SetValues(item);
                }
                else
                {
                    // Insert child.
                    existingChilds.Add(item);
                    // context.Entry(item).State = EntityState.Added;
                }
            }
        }

        /// <summary>
        /// Saves changes to a detached model by comparing it with the latest data.
        /// </summary>
        /// <typeparam name="T">The type of model to save.</typeparam>
        /// <param name="context">The database context tracking changes.</param>
        /// <param name="model">The model object to save.</param>
        /// <param name="existing">The latest model data.</param>
        public static void SaveChanges<T>(this DbContext context, T model, T existing)
            where T : class
        {
            context.CheckNotNull(nameof(context));
            model.CheckNotNull(nameof(context));

            context.Entry(existing).CurrentValues.SetValues(model);
            context.SaveChanges();
        }

        /// <summary>
        /// Saves changes to a detached model by comparing it with the latest data.
        /// </summary>
        /// <typeparam name="T">The type of model to save.</typeparam>
        /// <param name="context">The database context tracking changes.</param>
        /// <param name="model">The model object to save.</param>
        /// <param name="existing">The latest model data.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns></returns>
        public static async Task SaveChangesAsync<T>(this DbContext context, T model, T existing, CancellationToken cancellationToken = default)
            where T : class
        {
            context.CheckNotNull(nameof(context));
            model.CheckNotNull(nameof(context));

            context.Entry(existing).CurrentValues.SetValues(model);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
