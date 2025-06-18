
namespace commercepricing.infrastructure.Repository
{
    /// <summary>
    /// Defines an interface for models that are updateable
    /// </summary>
    /// <typeparam name="T">The type of the model being updated</typeparam>
    public interface IUpdateableModel<in T>
    {
        /// <summary>
        /// Updates the model based on the specified input
        /// </summary>
        /// <param name="model">The model to use to update this model</param>
        void Update(T model);
    }
}
