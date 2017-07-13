using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsCodeGenerator.Tests
{
    [Table("User", Schema = "tmp")]
    public partial class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(Order = 1)]
        public string FirstName { get; set; }

        [Column("LastName", Order = 2)]
        public string FamilyName { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return FirstName + FamilyName; }
        }
    }
}
