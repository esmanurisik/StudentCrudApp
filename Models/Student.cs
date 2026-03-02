using System.ComponentModel.DataAnnotations;

namespace StudentCrudApp.Models;

public class Student
{
    // Primary Key (Birincil Anahtar) - Otomatik artan ID
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
    [Display(Name = "Öğrenci Adı")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
    [Display(Name = "Öğrenci Soyadı")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sınıf bilgisi zorunludur.")]
    [StringLength(20, ErrorMessage = "Sınıf en fazla 20 karakter olabilir.")]
    [Display(Name = "Sınıfı")]
    public string Class { get; set; } = string.Empty;

    [Required(ErrorMessage = "Okul numarası zorunludur.")]
    [StringLength(20, ErrorMessage = "Okul numarası en fazla 20 karakter olabilir.")]
    [Display(Name = "Okul Numarası")]
    public string Number { get; set; } = string.Empty;

    // View'larda Ad ve Soyadı bitişik göstermek için yardımcı bir özellik (Veritabanına kaydedilmez)
    [Display(Name = "Ad Soyad")]
    public string FullName => $"{FirstName} {LastName}";
}