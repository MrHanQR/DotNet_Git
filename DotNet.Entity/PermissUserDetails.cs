using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet.Entity
{
    [Table("PermissUserDetails")]
    public class PermissUserDetails
    {
        public PermissUserDetails()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(16)]
        public string RealName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birth { get; set; }
        [MinLength(16),MaxLength(19)]
        public string IdentityCardNumber { get; set; }
        [StringLength(48)]
        public string Address { get; set; }
        [StringLength(13)]
        public string PhoneNumber { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
    }
}