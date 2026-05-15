using CompanyName.Ddd.Domain.Entities;
using CompanyName.Ddd.Domain.EventBus;
using System;

namespace CompanyName.Data.Fixtures
{
    public class CompanyNameAggregateRoot : AggregateRoot<int>, IDomainEventSource, IIntegrationEventSource
    {
        public CompanyNameAggregateRoot()
        {

        }

        public CompanyNameAggregateRoot(int id)
            : base(id)
        {
            
        }
    }
}
