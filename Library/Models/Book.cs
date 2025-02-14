using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Library.Models;

public class Book
{
    public required int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title is too long")]
    public required string Title { get; set; }

    [ValidDate(ErrorMessage = "Invalid publication year")]
    public required int PublicationYear { get; set; }


    [Required(ErrorMessage = "AuthorId is required")]
    public required int AuthorId { get; set; }










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



}