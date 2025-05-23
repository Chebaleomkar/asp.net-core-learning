using System.ComponentModel.DataAnnotations;
using GameStoreApi.Dtos;

public record class CreateGenreDto(
    [Required][StringLength(25)]string Name
);