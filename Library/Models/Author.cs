using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Library.Models;


public class Author
{

    private DateTime _date;
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name is too long")]
    public required string Name { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly DateOfBirth { get; set; }

    public List<Book>? Books { get; set; } = new List<Book>();


    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
            
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base (
            d => d.ToDateTime(TimeOnly.MinValue).ToUniversalTime(),
            d => DateOnly.FromDateTime(d))
        {}
    }
    
    
}