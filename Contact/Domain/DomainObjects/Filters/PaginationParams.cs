using System.ComponentModel.DataAnnotations;

namespace Domain.DomainObjects.Filters;

public class PaginationParams
{
    [Range(1, int.MaxValue, ErrorMessage = "O número da página deve ser maior ou igual a 1.")]
    public int PageNumber { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O tamanho da página deve ser maior ou igual a 1.")]
    public int PageSize { get; set; }
}
