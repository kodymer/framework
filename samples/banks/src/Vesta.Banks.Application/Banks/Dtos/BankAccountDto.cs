using Vesta.Ddd.Application.Dtos;

namespace Vesta.Banks.Dtos
{
    // [Create][EntityName][Dto]     - Cuando el DTO expone las mismas propiedades que la entidad de negocio
    // [Update][EntityName][Input]   - Cuando el DTO de entrada se construye bajo demanda del negocio
    // [Delete][EntityName][Output]  - Cuando necesitamos adaptar la salida de valores al solicitante


    public class BankAccountDto  : EntityDto<Guid>
    {
        public string Number { get; set; }

        public decimal Balance { get; set; }
    }
}