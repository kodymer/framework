using Ardalis.Specification;

namespace Samples.Banks.Transfers
{
    public class BankTransfersOrderedByCreationTimeAscSpecification : Specification<BankTransfer>
    {
        public BankTransfersOrderedByCreationTimeAscSpecification()
        {
            Query
                .OrderBy(bankTransfer => bankTransfer.CreationTime);
        }
    }
}
