namespace commercepricing.domain.Interfaces
{
    public interface IHasId<out IdType> where IdType : class
    {
        IdType Id { get; }
    }
}