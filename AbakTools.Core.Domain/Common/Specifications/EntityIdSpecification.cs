using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Common.Specifications
{
    public class EntityIdSpecification : Specification<IEntity>
    {
        private readonly int _id;

        private EntityIdSpecification(int id) => _id = id;

        public static EntityIdSpecification Of(int id) => new EntityIdSpecification(id);

        public override Expression<Func<IEntity, bool>> ToExpression()
        {
            return x => x.Id == _id;
        }
    }
}
