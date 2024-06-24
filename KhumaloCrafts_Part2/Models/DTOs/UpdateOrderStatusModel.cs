using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KhumaloCrafts_Part2.Models.DTOs;

public class UpdateOrderStatusModel
{
    public int OrderId { get; set; }

    [Required]
    public int OrderStatusId { get; set; }

    public IEnumerable<SelectListItem>? OrderStatusList { get; set; }
}
