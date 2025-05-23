
using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.Dtos;

public record class UpdateGenreDto(
    [Required][StringLength(30)]string Name
);