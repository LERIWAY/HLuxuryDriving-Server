using System.ComponentModel.DataAnnotations;

public class ReservationModel
{
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    public string? Prenom { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string? Nom { get; set; }

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Le format de l'email est invalide")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Le téléphone est obligatoire")]
    [Phone(ErrorMessage = "Le format du téléphone est invalide")]
    public string? Telephone { get; set; }

    [Required(ErrorMessage = "L'adresse de départ est obligatoire")]
    public string? Depart { get; set; }

    [Required(ErrorMessage = "L'adresse d'arrivée est obligatoire")]
    public string? Arrivee { get; set; }

    [Required(ErrorMessage = "La date du trajet est obligatoire")]
    [DataType(DataType.Date)]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Le commentaire est obligatoire")]
    public string? Commentaire { get; set; }
}
