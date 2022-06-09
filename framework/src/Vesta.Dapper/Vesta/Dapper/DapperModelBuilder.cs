using Vesta.Dapper.Metadata;

namespace Vesta.Dapper
{
    public class DapperModelBuilder : IModelBuilder
    {
        Dictionary<Type, object> IModelBuilder.EntityTypeBuilders => _entityTypeBuilders;

        private readonly Dictionary<Type, object> _entityTypeBuilders;

        public DapperModelBuilder()
        {
            _entityTypeBuilders = new Dictionary<Type, object>();
        }

        public DapperModelBuilder Entity<TEntity>(Action<DapperEntityTypeBuilder<TEntity>> buildAction) where TEntity : class
        {
            if (!_entityTypeBuilders.ContainsKey(typeof(TEntity)))
            {
                _entityTypeBuilders.Add(typeof(TEntity), buildAction);
            }
            else
            {
                _entityTypeBuilders[typeof(TEntity)] = buildAction;
            }

            return this;
        }
    }
}
