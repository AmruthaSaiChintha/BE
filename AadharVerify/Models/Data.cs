using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Data
{

    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; }

    public int Age { get; set; }


    [Column(TypeName = "nvarchar(150)")]
    public string Address { get; set; }

    public string Phone { get; set; }


    public string AadharNumber { get; set; }
    public string Email { get; set; }

    

}