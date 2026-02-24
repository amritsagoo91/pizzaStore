using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; // For IFormFile

namespace PizzaStore.Models;

public class Pizza
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(5, 100)]
    public decimal Price { get; set; }

    public string Size { get; set; }
    
   
}