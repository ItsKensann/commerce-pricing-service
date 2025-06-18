
using commercepricing.infrastructure.Interfaces;

namespace commercepricing.infrastructure.Models
{
    /// <summary>
    /// Specifies the abstract base class for an <see cref="IUpdateableModel{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the updateable model</typeparam>
    public abstract class BaseUpdateableModel<T> : IUpdateableModel<T> where T : IUpdateableModel<T>
    {
        public abstract void Update(T model);

        /// <summary>
        /// Updates a sub <see cref="IUpdateableModel{U}"/> model
        /// </summary>
        /// <typeparam name="U">The type of the sub model being updated</typeparam>
        /// <param name="self">The sub model being updated</param>
        /// <param name="other">The sub model to use to perform the update</param>
        /// <returns>The updated sub model</returns>
        protected U UpdateSubModel<U>(U self, U other) where U : IUpdateableModel<U>
        {
            if (self == null && other != null)
            {
                return other;
            }
            else
            {
                self?.Update(other!);
                return self!;
            }
        }

        /// <summary>
        /// Updates a list of <see cref="IUpdateableModel{U}"/> sub models
        /// </summary>
        /// <typeparam name="U">The type of the sub model being updated</typeparam>
        /// <param name="self">The list of sub models to update</param>
        /// <param name="other">The list of sub models to use to perform the update</param>
        /// <param name="comparer">Compares two sub models for equality</param>
        /// <returns>An updated list of sub models</returns>
        protected IEnumerable<U> UpdateSubModelList<U>(IEnumerable<U> self, IEnumerable<U> other, Func<U, U, bool> comparer) where U : IUpdateableModel<U>
        {
            if (other != null && other.Any())
            {
                if (self == null || !self.Any())
                {
                    return other;
                }
                else
                {
                    foreach (var i in other)
                    {
                        var ii = self.FirstOrDefault(iii => comparer(iii, i));
                        if (ii == null)
                        {
                            self = self.Append(i);
                        }
                        else
                        {
                            ii.Update(i);
                        }
                    }
                }
            }

            return self;
        }
    }
}