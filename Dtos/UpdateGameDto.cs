using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.Dtos;

public record class UpdateGameDto(
    [StringLength(50)]string? name ,
    [StringLength(20)]string? Genre ,
    [Range(1,100)]decimal? Price ,
    DateOnly? ReleaseDate
);