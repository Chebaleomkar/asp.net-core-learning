using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.Dtos;

public record  class CreateGameDto(
    [Required][StringLength(50)] string name ,
    [Required][StringLength(20)] string Genre ,
    [Range(1,100)]decimal Price ,
    DateOnly ReleaseDate
);