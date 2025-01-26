using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace Library.Models;

public class Book
{
    [ValidId(ErrorMessage = "Invalid Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title is too long")]
    public string Title { get; set; }

    [ValidDate(ErrorMessage = "Invalid publication year")]
    public int PublicationYear { get; set; }
    [Required(ErrorMessage = "AuthorId is required")]
    public int AuthorId { get; set; }










    public class ValidDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int year)
            {
                return year >= 1000 && year <= DateTime.Now.Year;
            }
            return false;
        }

    }

    public class ValidId : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int num)
            {
                return num >= 0;
            }
            return false;
        }
    }

}